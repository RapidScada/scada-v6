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
 * Module   : ScadaAgentEngine
 * Summary  : Represents a server manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Agent.Config;
using Scada.Config;
using Scada.Lang;
using Scada.Log;
using System;
using System.IO;
using System.Reflection;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Represents an Agent manager.
    /// <para>Представляет менеджер Агента.</para>
    /// </summary>
    public class Manager
    {
        private ILog log;            // the application log
        private CoreLogic coreLogic; // the Agent logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Manager()
        {
            log = LogStub.Instance;
            coreLogic = null;
            AppDirs = new AppDirs();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; }


        /// <summary>
        /// Localizes the application.
        /// </summary>
        private bool LocalizeApp()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaCommon", out string errMsg))
                log.WriteError(errMsg);

            CommonPhrases.Init();
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
            log = new LogFile(LogFormat.Full, Path.Combine(AppDirs.LogDir, AgentUtils.LogFileName));
            log.WriteBreak();

            // load culture
            if (!Locale.LoadCulture(Path.Combine(AppDirs.InstanceDir, "Config", InstanceConfig.DefaultFileName),
                out string errMsg))
            {
                log.WriteError(errMsg);
            }

            // prepare to start service
            log.WriteAction(Locale.IsRussian ?
                "Агент {0} запущен" :
                "Agent {0} started", AgentUtils.AppVersion);

            AgentConfig appConfig = new AgentConfig();

            if (AppDirs.CheckExistence(out errMsg) &&
                LocalizeApp() &&
                appConfig.Load(Path.Combine(AppDirs.ConfigDir, AgentConfig.DefaultFileName), out errMsg))
            {
                // start service
                coreLogic = new CoreLogic(appConfig, AppDirs, log);

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

            log.WriteAction(Locale.IsRussian ?
                "Агент остановлен" :
                "Agent is stopped");
            log.WriteBreak();
        }
    }
}
