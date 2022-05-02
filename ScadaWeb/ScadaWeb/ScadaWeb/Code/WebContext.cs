/*
 * Copyright 2022 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Webstation Application
 * Summary  : Contains web application level data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Client;
using Scada.Config;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using Scada.Storages;
using Scada.Web.Config;
using Scada.Web.Lang;
using Scada.Web.Plugins;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Scada.Web.Code
{
    /// <summary>
    /// Contains web application level data.
    /// <para>Содержит данные уровня веб-приложения.</para>
    /// </summary>
    internal class WebContext : IWebContext
    {
        /// <summary>
        /// Specifies the configuration update steps.
        /// </summary>
        private enum ConfigUpdateStep { Idle, LoadConfig, ReadBase }

        /// <summary>
        /// The period of attempts to read the configuration database.
        /// </summary>
        private static readonly TimeSpan ReadBasePeriod = TimeSpan.FromSeconds(10);

        private StorageWrapper storageWrapper;     // contains the application storage
        private Thread configThread;               // the configuration update thread
        private volatile bool terminated;          // necessary to stop the thread
        private volatile bool pluginsReady;        // plugins are loaded
        private bool configUpdateRequired;         // indicates that the configuration should be updated
        private ConfigUpdateStep configUpdateStep; // the current step of configuration update
        private Stats stats;                       // provides a statistics ID


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebContext()
        {
            storageWrapper = null;
            configThread = null;
            terminated = false;
            pluginsReady = false;
            configUpdateRequired = false;
            configUpdateStep = ConfigUpdateStep.Idle;
            stats = null;

            IsReady = false;
            IsReadyToLogin = false;
            InstanceConfig = new InstanceConfig();
            AppConfig = new WebConfig();
            AppDirs = new WebDirs();
            Log = LogStub.Instance;
            ConfigBase = new ConfigBase();
            ClientPool = new ScadaClientPool();
            PluginHolder = new PluginHolder();
            CacheExpirationTokenSource = new CancellationTokenSource();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Gets a value indicating whether the application is ready for operating.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Gets a value indicating whether a user can login.
        /// </summary>
        public bool IsReadyToLogin { get; private set; }

        /// <summary>
        /// Gets the instance configuration.
        /// </summary>
        public InstanceConfig InstanceConfig { get; }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public WebConfig AppConfig { get; private set; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public WebDirs AppDirs { get; }

        /// <summary>
        /// Gets the application storage.
        /// </summary>
        public IStorage Storage => storageWrapper.Storage;

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; private set; }

        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        public ConfigBase ConfigBase { get; private set; }

        /// <summary>
        /// Gets the client pool.
        /// </summary>
        public ScadaClientPool ClientPool { get; }

        /// <summary>
        /// Gets the object containing plugins.
        /// </summary>
        public PluginHolder PluginHolder { get; }

        /// <summary>
        /// Gets the source object that can send expiration notification to the memory cache.
        /// </summary>
        public CancellationTokenSource CacheExpirationTokenSource { get; private set; }

        /// <summary>
        /// Gets the statistics ID.
        /// </summary>
        public string StatsID
        {
            get
            {
                if (stats == null)
                    stats = new Stats(Storage, Log);

                return stats.StatsID;
            }
        }


        /// <summary>
        /// Writes information about the unhandled exception to the log.
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Log.WriteError(args.ExceptionObject as Exception, CommonPhrases.UnhandledException);
        }

        /// <summary>
        /// Loads the instance configuration.
        /// </summary>
        private void LoadInstanceConfig()
        {
            Locale.SetCultureToEnglish();

            if (InstanceConfig.Load(InstanceConfig.GetConfigFileName(AppDirs.InstanceDir), out string errMsg))
            {
                Locale.SetCulture(InstanceConfig.Culture);
                AppDirs.UpdateLogDir(InstanceConfig.LogDir);
            }
            else
            {
                Console.WriteLine(errMsg);
                Locale.SetCultureToDefault();
            }
        }

        /// <summary>
        /// Localizes the application.
        /// </summary>
        private void LocalizeApp()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaCommon", out string errMsg))
                Log.WriteError(errMsg);

            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaWeb", out errMsg))
                Log.WriteError(errMsg);

            CommonPhrases.Init();
            WebPhrases.Init();
        }

        /// <summary>
        /// Initializes the application storage.
        /// </summary>
        private bool InitStorage()
        {
            storageWrapper = new StorageWrapper(new StorageContext
            {
                App = ServiceApp.Web,
                AppDirs = AppDirs,
                Log = Log
            }, InstanceConfig);

            return storageWrapper.InitStorage();
        }

        /// <summary>
        /// Loads the application configuration.
        /// </summary>
        private bool LoadAppConfig(out WebConfig webConfig)
        {
            webConfig = new WebConfig();

            if (webConfig.Load(Storage, WebConfig.DefaultFileName, out string errMsg))
            {
                if (Log is LogFile logFile)
                    logFile.CapacityMB = webConfig.GeneralOptions.MaxLogSize;

                UpdateCulture(webConfig);
                return true;
            }
            else
            {
                Log.WriteError(errMsg);
                webConfig = null;
                return false;
            }
        }

        /// <summary>
        /// Updates the application culture according to the configuration.
        /// </summary>
        private void UpdateCulture(WebConfig webConfig)
        {
            string cultureName = ScadaUtils.FirstNonEmpty(
                InstanceConfig.Culture,
                webConfig.GeneralOptions.DefaultCulture,
                Locale.DefaultCulture.Name);

            if (Locale.Culture.Name != cultureName)
            {
                Locale.SetCulture(cultureName);
                LocalizeApp();
            }
        }

        /// <summary>
        /// Initializes plugins.
        /// </summary>
        private void InitPlugins()
        {
            if (pluginsReady)
            {
                Log.WriteInfo(Locale.IsRussian ?
                    "Плагины добавляются один раз при запуске приложения" :
                    "Plugins are added once at application startup");
            }
            else
            {
                PluginHolder.Log = Log;

                foreach (string pluginCode in AppConfig.PluginCodes)
                {
                    if (PluginFactory.GetPluginLogic(AppDirs.ExeDir, pluginCode, this, 
                        out PluginLogic pluginLogic, out string message))
                    {
                        Log.WriteAction(message);
                        PluginHolder.AddPlugin(pluginLogic);
                    }
                    else
                    {
                        Log.WriteError(message);
                    }
                }
            }

            PluginHolder.DefineFeaturedPlugins(AppConfig.PluginAssignment);
            PluginHolder.LoadDictionaries();
            PluginHolder.LoadConfig();
        }

        /// <summary>
        /// Reads the configuration database.
        /// </summary>
        private bool ReadBase(out ConfigBase configBase)
        {
            Log.WriteAction(Locale.IsRussian ?
                "Приём базы конфигурации" :
                "Receive the configuration database");

            // check connection
            ScadaClient scadaClient = new(AppConfig.ConnectionOptions);

            try
            {
                scadaClient.GetStatus(out bool serverIsReady, out bool userIsLoggedIn);

                if (!serverIsReady)
                {
                    scadaClient.TerminateSession();
                    Log.WriteError(Locale.IsRussian ?
                        "Сервер не готов" :
                        "Server is not ready");
                    configBase = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при проверке соединения с сервером" :
                    "Error checking server connection"));
                configBase = null;
                return false;
            }

            // receive tables
            string tableName = CommonPhrases.UndefinedTable;

            try
            {
                configBase = new ConfigBase();

                foreach (IBaseTable baseTable in configBase.AllTables)
                {
                    tableName = baseTable.Name;
                    scadaClient.DownloadBaseTable(baseTable);
                }

                tableName = CommonPhrases.UndefinedTable;
                PostprocessBase(configBase);
                Log.WriteAction(Locale.IsRussian ?
                    "База конфигурации получена успешно" :
                    "The configuration database has been received successfully");
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при приёме базы конфигурации, таблица {0}" :
                    "Error receiving the configuration database, the {0} table", tableName);
                configBase = null;
                return false;
            }
            finally
            {
                scadaClient.TerminateSession();
            }
        }

        /// <summary>
        /// Post-processes the received configuration database.
        /// </summary>
        private static void PostprocessBase(ConfigBase configBase)
        {
            // duplicate channels for arrays and strings
            List<Cnl> duplicatedCnls = new();

            foreach (Cnl cnl in configBase.CnlTable.Enumerate())
            {
                if (cnl.IsArray() && cnl.IsArchivable())
                {
                    string name = cnl.Name;
                    int dataLen = cnl.DataLen.Value;
                    int cnlTypeID = CnlTypeID.UnsetOutput(cnl.CnlTypeID);

                    if (cnl.IsNumeric())
                    {
                        cnl.Name += "[0]";
                        cnl.DataLen = null;
                    }

                    for (int i = 1; i < dataLen; i++)
                    {
                        Cnl newCnl = cnl.ShallowCopy();
                        newCnl.CnlNum = cnl.CnlNum + i;
                        newCnl.Name = name + "[" + i + "]";
                        newCnl.DataLen = null;
                        newCnl.CnlTypeID = cnlTypeID;
                        duplicatedCnls.Add(newCnl);
                    }
                }
            }

            duplicatedCnls.ForEach(cnl => configBase.CnlTable.AddItem(cnl));

            // initialize data objects
            configBase.Init();
        }

        /// <summary>
        /// Updates the configuration in a separate thread.
        /// </summary>
        private void ExecuteConfigUpdate()
        {
            DateTime readBaseDT = DateTime.MinValue;

            while (!terminated)
            {
                try
                {
                    switch (configUpdateStep)
                    {
                        case ConfigUpdateStep.Idle:
                            if (configUpdateRequired)
                            {
                                configUpdateRequired = false;
                                configUpdateStep = ConfigUpdateStep.LoadConfig;
                            }
                            break;

                        case ConfigUpdateStep.LoadConfig:
                            if (LoadAppConfig(out WebConfig webConfig))
                            {
                                AppConfig = webConfig;
                                InitPlugins();
                            }

                            pluginsReady = true;
                            configUpdateStep = ConfigUpdateStep.ReadBase;
                            break;

                        case ConfigUpdateStep.ReadBase:
                            DateTime utcNow = DateTime.UtcNow;

                            if (utcNow - readBaseDT >= ReadBasePeriod)
                            {
                                readBaseDT = utcNow;

                                if (ReadBase(out ConfigBase configBase))
                                {
                                    ConfigBase = configBase;
                                    IsReadyToLogin = true;

                                    if (IsReady)
                                    {
                                        ResetCache(); // after reading the configuration database
                                        Log.WriteInfo(Locale.IsRussian ?
                                            "Приложение готово к входу пользователей" :
                                            "The application is ready for user login");
                                    }
                                    else
                                    {
                                        IsReady = true;
                                        Log.WriteInfo(Locale.IsRussian ?
                                            "Приложение готово к работе" :
                                            "The application is ready for operating");
                                    }

                                    PluginHolder.OnAppReady();
                                    configUpdateStep = ConfigUpdateStep.Idle;
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(ex, Locale.IsRussian ?
                        "Ошибка при обновлении конфигурации" :
                        "Error updating configuration");
                }
                finally
                {
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
        }

        /// <summary>
        /// Stops the configuration update thread.
        /// </summary>
        private void StopProcessing()
        {
            try
            {
                if (configThread != null)
                {
                    terminated = true;
                    configThread.Join();
                    configThread = null;
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при остановке обновления конфигурации" :
                    "Error stopping configuration update");
            }
        }


        /// <summary>
        /// Initializes the application context.
        /// </summary>
        public bool Init()
        {
            AppDirs.Init(Assembly.GetExecutingAssembly());
            LoadInstanceConfig();

            Log = new LogFile(LogFormat.Full)
            {
                FileName = Path.Combine(AppDirs.LogDir, WebUtils.LogFileName),
                Capacity = int.MaxValue
            };

            Log.WriteBreak();
            Log.WriteAction(Locale.IsRussian ?
                "Вебстанция {0} запущена" :
                "Webstation {0} started", WebUtils.AppVersion);
            LocalizeApp();

            if (InitStorage())
            {
                return true;
            }
            else
            {
                Log.WriteError(CommonPhrases.ExecutionImpossible);
                return false;
            }
        }

        /// <summary>
        /// Finalizes the application context.
        /// </summary>
        public void FinalizeContext()
        {
            StopProcessing();
            storageWrapper?.CloseStorage();

            Log.WriteAction(Locale.IsRussian ?
                "Вебстанция остановлена" :
                "Webstation is stopped");
            Log.WriteBreak();
        }

        /// <summary>
        /// Starts a background process of configuration update.
        /// </summary>
        public void StartProcessing()
        {
            try
            {
                if (configThread == null)
                {
                    terminated = false;
                    pluginsReady = false;
                    configUpdateRequired = true;
                    configUpdateStep = ConfigUpdateStep.Idle;
                    IsReadyToLogin = false;

                    configThread = new Thread(ExecuteConfigUpdate);
                    configThread.Start();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при запуске обновления конфигурации" :
                    "Error starting configuration update");
            }
        }

        /// <summary>
        /// Waits for plugins to be initialized.
        /// </summary>
        public void WaitForPlugins()
        {
            while (!terminated && !pluginsReady)
            {
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }

        /// <summary>
        /// Gets a specification of the specified view entity.
        /// </summary>
        public ViewSpec GetViewSpec(View viewEntity)
        {
            ArgumentNullException.ThrowIfNull(viewEntity, nameof(viewEntity));

            if (viewEntity.ViewTypeID == null)
            {
                return PluginHolder.GetViewSpecByExt(Path.GetExtension(viewEntity.Path));
            }
            else
            {
                ViewType viewType = ConfigBase.ViewTypeTable.GetItem(viewEntity.ViewTypeID.Value);
                return viewType == null ? null : PluginHolder.GetViewSpecByCode(viewType.Code);
            }
        }

        /// <summary>
        /// Reloads the application configuration and resets the memory cache.
        /// </summary>
        public bool ReloadConfig()
        {
            if (configThread == null || terminated)
            {
                return false;
            }
            else if (configUpdateStep == ConfigUpdateStep.Idle)
            {
                Log.WriteAction(Locale.IsRussian ?
                    "Перезагрузка конфигурации" :
                    "Reload configuration");
                IsReadyToLogin = false;
                configUpdateRequired = true;
                return true;
            }
            else
            {
                Log.WriteWarning(Locale.IsRussian ?
                    "Перезагрузка конфигурации уже выполняется" :
                    "Reload configuration is already in progress");
                return false;
            }
        }

        /// <summary>
        /// Resets the memory cache.
        /// </summary>
        public void ResetCache()
        {
            if (configThread != null && !terminated)
            {
                try
                {
                    Log.WriteAction(Locale.IsRussian ?
                        "Очистка кэша" :
                        "Reset memory cache");
                    CacheExpirationTokenSource.Cancel();
                    PluginHolder.ResetCache();
                }
                catch (Exception ex)
                {
                    Log.WriteError(ex, Locale.IsRussian ?
                        "Ошибка при очистке кэша" :
                        "Error clearing memory cache");
                }
                finally
                {
                    CacheExpirationTokenSource = new CancellationTokenSource();
                }
            }
        }
    }
}
