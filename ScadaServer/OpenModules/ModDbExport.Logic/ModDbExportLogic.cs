// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Lang;
using Scada.Server.Modules.ModDbExport.Config;

namespace Scada.Server.Modules.ModDbExport.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModDbExportLogic : ModuleLogic
    {
        /// <summary>
        /// The module log file name.
        /// </summary>
        private const string LogFileName = "ModDbExport.log";

        private readonly LogFile moduleLog;         // the module log
        private readonly ModuleConfig moduleConfig; // the module configuration
        private readonly List<Exporter> exporters;  // the active exporters

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDbExportLogic(IServerContext serverContext)
            : base(serverContext)
        {
            moduleLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(serverContext.AppDirs.LogDir, LogFileName),
                CapacityMB = serverContext.AppConfig.GeneralOptions.MaxLogSize
            };
            moduleConfig = new ModuleConfig();
            exporters = [];
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
            HashSet<int> exporterIDs = [];

            foreach (ExportTargetConfig exporterConfig in moduleConfig.ExportTargets)
            {
                if (exporterConfig.GeneralOptions.Active)
                {
                    if (!exporterIDs.Add(exporterConfig.GeneralOptions.ID))
                    {
                        moduleLog.WriteError(Locale.IsRussian ?
                            "Дублируется идентификатор цели экспорта {0}" :
                            "Duplicate export target ID {0}",
                            exporterConfig.GeneralOptions.Title);
                    }
                    else
                    {
                        Exporter exporter = new(exporterConfig, ServerContext);
                        exporters.Add(exporter);
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
                moduleLog.WriteAction(Locale.IsRussian ?
                    "Запуск экспорта" :
                    "Start export");

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
                moduleLog.WriteAction(Locale.IsRussian ?
                    "Остановка экспорта" :
                    "Stop export");

                foreach (Exporter exporter in exporters)
                {
                    exporter.Stop();
                }
            }

            // write to log
            moduleLog.WriteAction(ServerPhrases.StopModule, Code);
            moduleLog.WriteBreak();
        }

        /// <summary>
        /// Performs actions on a new iteration of the main operating loop.
        /// </summary>
        public override void OnIteration()
        {
            foreach (Exporter exporter in exporters)
            {
                exporter.EnqueueCalculatedData();
            }
        }

        /// <summary>
        /// Performs actions after receiving and processing new current data.
        /// </summary>
        /// <remarks>In general, channel numbers are not sorted.</remarks>
        public override void OnCurrentDataProcessed(Slice slice)
        {
            foreach (Exporter exporter in exporters)
            {
                exporter.EnqueueCurrentData(slice);
            }
        }

        /// <summary>
        /// Performs actions after receiving and processing new historical data.
        /// </summary>
        public override void OnHistoricalDataProcessed(int archiveMask, Slice slice)
        {
            foreach (Exporter exporter in exporters)
            {
                exporter.EnqueueHistoricalData(slice);
            }
        }

        /// <summary>
        /// Performs actions after creating and before writing an event.
        /// </summary>
        public override void OnEvent(int archiveMask, Event ev)
        {
            foreach (Exporter exporter in exporters)
            {
                exporter.EnqueueEvent(ev);
            }
        }

        /// <summary>
        /// Performs actions when acknowledging an event.
        /// </summary>
        public override void OnEventAck(EventAck eventAck)
        {
            foreach (Exporter exporter in exporters)
            {
                exporter.EnqueueEventAck(eventAck);
            }
        }

        /// <summary>
        /// Performs actions after receiving and before enqueuing a telecontrol command.
        /// </summary>
        public override void OnCommand(TeleCommand command, CommandResult commandResult)
        {
            foreach (Exporter exporter in exporters)
            {
                exporter.EnqueueCommand(command, commandResult);
            }
        }
    }
}
