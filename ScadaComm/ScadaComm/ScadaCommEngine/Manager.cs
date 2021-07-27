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
 * Module   : ScadaCommEngine
 * Summary  : Represents a Communicator manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Comm.Config;
using Scada.Comm.Lang;
using Scada.Config;
using Scada.Lang;
using Scada.Log;
using System;
using System.IO;
using System.Reflection;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Represents a Communicator manager.
    /// <para>Представляет менеджер Коммуникатора.</para>
    /// </summary>
    public class Manager
    {
        private ILog log;            // the application log
        private CoreLogic coreLogic; // the Communicator logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Manager()
        {
            log = LogStub.Instance;
            coreLogic = null;
            AppDirs = new CommDirs();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public CommDirs AppDirs { get; }


        /// <summary>
        /// Localizes the application.
        /// </summary>
        private void LocalizeApp(string langDir)
        {
            if (!Locale.LoadDictionaries(langDir, "ScadaCommon", out string errMsg))
                log.WriteError(errMsg);

            if (!Locale.LoadDictionaries(langDir, "ScadaComm", out errMsg))
                log.WriteError(errMsg);

            CommonPhrases.Init();
            CommPhrases.Init();
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

            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AppDirs.Init(exeDir);

            LogFile logFile = new LogFile(LogFormat.Full, Path.Combine(AppDirs.LogDir, CommUtils.LogFileName))
            {
                Capacity = int.MaxValue
            };

            log = logFile;
            log.WriteBreak();

            if (!Locale.LoadCulture(Path.Combine(exeDir, "..", "Config", InstanceConfig.DefaultFileName),
                out string errMsg))
            {
                log.WriteError(errMsg);
            }

            log.WriteAction(Locale.IsRussian ?
                "Коммуникатор {0} запущен" :
                "Communicator {0} started", CommUtils.AppVersion);

            if (AppDirs.CheckExistence(out errMsg))
            {
                LocalizeApp(AppDirs.LangDir);
                string configFileName = Path.Combine(AppDirs.ConfigDir, CommConfig.DefaultFileName);
                CommConfig config = new CommConfig();
                coreLogic = new CoreLogic(config, AppDirs, log);

                if (config.Load(configFileName, out errMsg) &&
                    coreLogic.StartProcessing())
                {
                    logFile.Capacity = config.GeneralOptions.MaxLogSize;
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
                "Коммуникатор остановлен" :
                "Communicator is stopped");
            log.WriteBreak();
        }
    }
}
