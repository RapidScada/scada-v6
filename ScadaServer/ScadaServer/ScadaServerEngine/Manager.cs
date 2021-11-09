/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaServerEngine
 * Summary  : Represents a Server manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using Scada.Config;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Storages;
using System;
using System.IO;
using System.Reflection;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents a Server manager.
    /// <para>Представляет менеджер Сервера.</para>
    /// </summary>
    public class Manager
    {
        private ILog log;                      // the application log
        private StorageWrapper storageWrapper; // contains the application storage
        private CoreLogic coreLogic;           // the Server logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Manager()
        {
            log = LogStub.Instance;
            storageWrapper = null;
            coreLogic = null;
            AppDirs = new ServerDirs();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public ServerDirs AppDirs { get; }


        /// <summary>
        /// Localizes the application.
        /// </summary>
        private bool LocalizeApp()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaCommon", out string errMsg))
                log.WriteError(errMsg);

            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaServer", out errMsg))
                log.WriteError(errMsg);

            CommonPhrases.Init();
            ServerPhrases.Init();
            return true;
        }

        /// <summary>
        /// Writes information about the unhandled exception to the log.
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            log.WriteError(args.ExceptionObject as Exception, CommonPhrases.UnhandledException);
        }


        /// <summary>
        /// Starts the service.
        /// </summary>
        public bool StartService()
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            // initialize directories
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AppDirs.Init(exeDir);

            // initialize log
            LogFile logFile = new LogFile(LogFormat.Full, Path.Combine(AppDirs.LogDir, ServerUtils.LogFileName))
            {
                Capacity = int.MaxValue
            };

            log = logFile;
            log.WriteBreak();

            // load instance configuration
            InstanceConfig instanceConfig = new InstanceConfig();
            string configFileName = Path.Combine(AppDirs.InstanceDir, "Config", InstanceConfig.DefaultFileName);

            if (instanceConfig.Load(configFileName, out string errMsg))
                Locale.SetCulture(instanceConfig.Culture);
            else
                log.WriteError(errMsg);

            // prepare to start service
            log.WriteAction(Locale.IsRussian ?
                "Сервер {0} запущен" :
                "Server {0} started", ServerUtils.AppVersion);

            storageWrapper = new StorageWrapper(new StorageContext
            {
                App = ServiceApp.Server,
                AppDirs = AppDirs,
                Log = log
            }, instanceConfig);

            ServerConfig appConfig = new ServerConfig();

            if (AppDirs.CheckExistence(out errMsg) &&
                LocalizeApp() &&
                storageWrapper.InitStorage() &&
                appConfig.Load(storageWrapper.Storage, ServerConfig.DefaultFileName, out errMsg))
            {
                // start service
                logFile.CapacityMB = appConfig.GeneralOptions.MaxLogSize;
                coreLogic = new CoreLogic(instanceConfig, appConfig, AppDirs, storageWrapper.Storage, log);

                if (coreLogic.StartProcessing())
                    return true;
            }
            else if (!string.IsNullOrEmpty(errMsg))
            {
                log.WriteError(errMsg);
            }

            log.WriteError(CommonPhrases.ExecutionImpossible);
            return false;
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        public void StopService()
        {
            coreLogic?.StopProcessing();
            storageWrapper?.CloseStorage();

            log.WriteAction(Locale.IsRussian ?
                "Сервер остановлен" :
                "Server is stopped");
            log.WriteBreak();
        }
    }
}
