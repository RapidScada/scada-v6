/*
 * Copyright 2024 Rapid Software LLC
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
 * Modified : 2023
 */

using Scada.Comm.Config;
using Scada.Comm.Lang;
using Scada.Config;
using Scada.Lang;
using Scada.Log;
using Scada.Storages;
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
        private ILog log;                          // the application log
        private StorageWrapper storageWrapper;     // contains the application storage
        private AssemblyResolver assemblyResolver; // searches for assemblies
        private CoreLogic coreLogic;               // the Communicator logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Manager()
        {
            log = LogStub.Instance;
            storageWrapper = null;
            assemblyResolver = null;
            coreLogic = null;

            AppDirs = new CommDirs();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public CommDirs AppDirs { get; }


        /// <summary>
        /// Localizes the application.
        /// </summary>
        private bool LocalizeApp()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaCommon", out string errMsg))
                log.WriteError(errMsg);

            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaComm", out errMsg))
                log.WriteError(errMsg);

            CommonPhrases.Init();
            CommPhrases.Init();
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
        /// Handles an AssemblyResolve event of the current application domain.
        /// </summary>
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string errMsg = "";
            Assembly assembly = assemblyResolver?.Resolve(args.Name, args.RequestingAssembly, out errMsg);

            if (!string.IsNullOrEmpty(errMsg))
                log.WriteError(errMsg);

            return assembly;
        }


        /// <summary>
        /// Starts the service.
        /// </summary>
        public bool StartService()
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            // load instance configuration
            AppDirs.Init(Assembly.GetExecutingAssembly());
            InstanceConfig instanceConfig = new InstanceConfig();
            Locale.SetCultureToEnglish();

            if (instanceConfig.Load(InstanceConfig.GetConfigFileName(AppDirs.InstanceDir), out string errMsg))
            {
                Locale.SetCulture(instanceConfig.Culture);
                AppDirs.UpdateLogDir(instanceConfig.LogDir);
            }
            else
            {
                Console.WriteLine(errMsg);
                Locale.SetCultureToDefault();
            }

            // initialize log
            LogFile logFile = new LogFile(LogFormat.Full, Path.Combine(AppDirs.LogDir, CommUtils.LogFileName))
            {
                Capacity = int.MaxValue
            };

            log = logFile;
            log.WriteBreak();

            // prepare to start service
            log.WriteAction(Locale.IsRussian ?
                "Коммуникатор {0} запущен" :
                "Communicator {0} started", EngineUtils.AppVersion);

            storageWrapper = new StorageWrapper(new StorageContext
            {
                InstanceConfig = instanceConfig,
                App = ServiceApp.Comm,
                AppDirs = AppDirs,
                Log = log
            });

            assemblyResolver = new AssemblyResolver(AppDirs.GetProbingDirs());
            CommConfig appConfig = new CommConfig();

            if (AppDirs.CheckExistence(out errMsg) && 
                LocalizeApp() &&
                storageWrapper.InitStorage() &&
                appConfig.Load(storageWrapper.Storage, CommConfig.DefaultFileName, out errMsg))
            {
                // start service
                logFile.CapacityMB = appConfig.GeneralOptions.MaxLogSize;
                coreLogic = new CoreLogic(appConfig, AppDirs, storageWrapper.Storage, log);

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
                "Коммуникатор остановлен" :
                "Communicator is stopped");
            log.WriteBreak();
        }
    }
}
