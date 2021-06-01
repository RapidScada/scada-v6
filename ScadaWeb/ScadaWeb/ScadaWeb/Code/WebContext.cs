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
 * Module   : Webstation Application
 * Summary  : Contains web application level data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Client;
using Scada.Config;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Web.Config;
using Scada.Web.Lang;
using System.IO;

namespace Scada.Web.Code
{
    /// <summary>
    /// Contains web application level data.
    /// <para>Содержит данные уровня веб-приложения.</para>
    /// </summary>
    internal class WebContext : IWebContext
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebContext()
        {
            IsReady = false;
            IsReadyToLogin = false;
            InstanceConfig = new InstanceConfig();
            AppConfig = new WebConfig();
            AppDirs = new WebDirs();
            Log = LogStub.Instance;
            BaseDataSet = new BaseDataSet();
            ClientPool = new ScadaClientPool();
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
        public WebConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public WebDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; private set; }

        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        public BaseDataSet BaseDataSet { get; private set; }

        /// <summary>
        /// Gets the client pool.
        /// </summary>
        public ScadaClientPool ClientPool { get; }


        /// <summary>
        /// Loads the instance configuration.
        /// </summary>
        private void LoadInstanceConfig()
        {
            if (InstanceConfig.Load(Path.Combine(AppDirs.ExeDir, "..", "Config", InstanceConfig.DefaultFileName),
                out string errMsg))
            {
                Locale.SetCulture(InstanceConfig.Culture);
            }
            else
            {
                Log.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Loads the application configuration.
        /// </summary>
        private void LoadAppConfig()
        {
            if (AppConfig.Load(Path.Combine(AppDirs.ConfigDir, WebConfig.DefaultFileName), out string errMsg))
            {
                if (Log is LogFile logFile)
                    logFile.Capacity = AppConfig.GeneralOptions.MaxLogSize;
            }
            else
            {
                Log.WriteError(errMsg);
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
        /// Initializes the application context.
        /// </summary>
        public void Init(string exeDir)
        {
            AppDirs.Init(exeDir);

            Log = new LogFile(LogFormat.Full)
            {
                FileName = Path.Combine(AppDirs.LogDir, WebUtils.LogFileName),
                Capacity = int.MaxValue
            };

            Log.WriteBreak();
            LoadInstanceConfig();
            LocalizeApp();

            Log.WriteAction(Locale.IsRussian ?
                "Вебстанция {0} запущена" :
                "Webstation {0} started", WebUtils.AppVersion);
        }

        /// <summary>
        /// Finalizes the application context.
        /// </summary>
        public void FinalizeContext()
        {
            Log.WriteAction(Locale.IsRussian ?
                "Вебстанция остановлена" :
                "Webstation is stopped");
            Log.WriteBreak();
        }

        /// <summary>
        /// Starts a process of loading the application configuration and configuration database.
        /// </summary>
        public void StartLoadingConfig()
        {
            LoadAppConfig();

            IsReady = true;
            IsReadyToLogin = true;
            Log.WriteAction(Locale.IsRussian ?
                "Приложение готово к работе" :
                "The application is ready for operating");
        }
    }
}
