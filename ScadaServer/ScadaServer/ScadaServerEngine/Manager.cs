/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : Represents a server manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using Scada.Log;
using Scada.Server.Config;
using System;
using System.IO;
using System.Reflection;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents a server manager.
    /// <para>Представляет менеджер сервера.</para>
    /// </summary>
    public class Manager
    {
        private ILog log;            // the application log
        private CoreLogic coreLogic; // the server logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Manager()
        {
            log = new LogStub();
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
        private void LocalizeApp(string langDir)
        {
            if (!Locale.LoadDictionaries(langDir, "ScadaData", out string errMsg))
                log.WriteError(errMsg);

            if (!Locale.LoadDictionaries(langDir, "ScadaServer", out errMsg))
                log.WriteError(errMsg);

            CommonPhrases.Init();
            ServerPhrases.Init();
        }

        /// <summary>
        /// Writes information about the unhandled exception to the log.
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            log.WriteException(ex, string.Format(Locale.IsRussian ?
                "Необработанное исключение" :
                "Unhandled exception"));
        }


        /// <summary>
        /// Starts the service.
        /// </summary>
        public bool StartService()
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AppDirs.Init(exeDir);

            log = new LogFile(LogFormat.Full, AppDirs.LogDir + ServerUtils.LogFileName);
            log.WriteBreak();

            if (!Locale.LoadCulture(Path.Combine(exeDir, "..", ScadaUtils.ScadaConfigFileName), out string errMsg))
                log.WriteError(errMsg);

            log.WriteAction(string.Format(Locale.IsRussian ?
                "Сервер {0} запущен" :
                "Server {0} started", ServerUtils.AppVersion));

            if (AppDirs.CheckExistence(out errMsg))
            {
                LocalizeApp(AppDirs.LangDir);
                string configFileName = AppDirs.ConfigDir + ServerConfig.DefaultFileName;
                ServerConfig config = new ServerConfig();
                coreLogic = new CoreLogic(config, AppDirs, log);

                if (config.Load(configFileName, out errMsg) &&
                    coreLogic.StartProcessing())
                {
                    return true;
                }
                else if (!string.IsNullOrEmpty(errMsg))
                {
                    log.WriteError(errMsg);
                }
            }
            else
            {
                log.WriteError(errMsg);
            }

            log.WriteError(Locale.IsRussian ?
                "Нормальная работа программы невозможна" :
                "Normal program execution is impossible");
            return false;
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        public void StopService()
        {
            coreLogic?.StopProcessing();

            log.WriteAction(Locale.IsRussian ?
                "Сервер остановлен" :
                "Server is stopped");
            log.WriteBreak();
        }
    }
}
