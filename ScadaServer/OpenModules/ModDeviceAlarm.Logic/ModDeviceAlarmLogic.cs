// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Lang;
using Scada.Server.Modules.ModDeviceAlarm.Config;

namespace Scada.Server.Modules.ModDeviceAlarm.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// </summary>
    public class ModDeviceAlarmLogic : ModuleLogic
    {
        /// <summary>
        /// The module log file name.
        /// </summary>
        private const string LogFileName = "ModDeviceAlarm.log";

        private readonly ILog moduleLog;            // the module log
        private readonly ModuleConfig moduleConfig; // the module configuration
        private readonly List<Exporter> exporters;  // the active exporters

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDeviceAlarmLogic(IServerContext serverContext)
            : base(serverContext)
        {
            moduleLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(serverContext.AppDirs.LogDir, LogFileName),
                CapacityMB = serverContext.AppConfig.GeneralOptions.MaxLogSize
            };
            moduleConfig = new ModuleConfig();
            exporters = new List<Exporter>();
        }


        /// <summary>
        /// Gets the module code.
        /// </summary>
        public override string Code
        {
            get
            {
                return ModuleUtils.ModuleCode;
            }
        }


        /// <summary>
        /// Initializes and validates exporters.
        /// </summary>
        private bool InitExporters(out string errMsg)
        {
            HashSet<int> exporterIDs = new();

            foreach (ExportTargetConfig exporterConfig in moduleConfig.ExportTargets)
            {
                if (exporterConfig.GeneralOptions.Active)
                {
                    if (!exporterIDs.Add(exporterConfig.GeneralOptions.ID))
                    {
                        moduleLog.WriteError("Duplicate export target ID {0}", exporterConfig.GeneralOptions.Title);
                    }
                    else
                    {
                        for (int i = 0; i < exporterConfig.Triggers.Count; i++)
                        {
                            if (exporterConfig.Triggers[i].Active)
                            {
                                Exporter exporter = new(moduleConfig.EmailDeviceConfig, exporterConfig, exporterConfig.Triggers[i], ServerContext);
                                exporters.Add(exporter);
                            }
                            else
                            {
                                moduleLog.WriteInfo($"[Inactive]{exporterConfig.Triggers[i].Name}");
                            }
                        }
                    }
                }
            }

            if (exporters.Count > 0)
            {
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = Locale.IsRussian ?
                    "Отсутствуют активные цели экспорта." :
                    "Active export targets missing.";
                return false;
            }
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            // write to log
            moduleLog.WriteBreak();
            moduleLog.WriteAction(ServerPhrases.StartModule, Code, Version);

            // load configuration and start gates
            if (moduleConfig.Load(Storage, ModuleConfig.DefaultFileName, out string errMsg) &&
                InitExporters(out errMsg))
            {
                moduleLog.WriteAction("Start export");

                foreach (Exporter exporter in exporters)
                {
                    exporter.Start();
                }
            }
            else
            {
                Log.WriteError(ServerPhrases.ModuleMessage, Code, errMsg);
                moduleLog.WriteError(errMsg);
                moduleLog.WriteError(CommonPhrases.ExecutionImpossible);
            }
        }

        /// <summary>
        /// Performs actions when the service stops.
        /// </summary>
        public override void OnServiceStop()
        {
            // stop exporters
            if (exporters.Count > 0)
            {
                moduleLog.WriteAction("Stop export");

                foreach (Exporter exporter in exporters)
                {
                    exporter.Stop();
                }
            }

            // write to log
            moduleLog.WriteAction(ServerPhrases.StopModule, Code);
            moduleLog.WriteBreak();
        }
    }
}
