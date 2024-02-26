// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Protocol;
using Scada.Server.Lang;
using Scada.Server.Modules.ModDiffCalculator.Config;

namespace Scada.Server.Modules.ModDiffCalculator.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModDiffCalculatorLogic : ModuleLogic
    {
        /// <summary>
        /// The module log file name.
        /// </summary>
        private const string LogFileName = ModuleUtils.ModuleCode + ".log";

        private readonly LogFile moduleLog;                // the module log
        private readonly ModuleConfig moduleConfig;        // the module configuration
        private readonly List<ChannelGroup> channelGroups; // the processed channel groups

        private Thread moduleThread;      // the working thread of the module
        private volatile bool terminated; // necessary to stop the thread
        private CalculationTask calcTask; // the user task to calculate differences


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDiffCalculatorLogic(IServerContext serverContext)
            : base(serverContext)
        {
            moduleLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(serverContext.AppDirs.LogDir, LogFileName),
                CapacityMB = serverContext.AppConfig.GeneralOptions.MaxLogSize
            };
            moduleConfig = new ModuleConfig();
            channelGroups = [];

            moduleThread = null;
            terminated = false;
            calcTask = null;
        }


        /// <summary>
        /// Gets the module code.
        /// </summary>
        public override string Code => ModuleUtils.ModuleCode;


        /// <summary>
        /// Initializes channel groups according to the module configuration.
        /// </summary>
        private bool InitGroups(out string errMsg)
        {
            DateTime utcNow = DateTime.UtcNow;

            foreach (GroupConfig groupConfig in moduleConfig.Groups)
            {
                if (groupConfig.Active)
                {
                    if (groupConfig.Items.Count == 0)
                    {
                        moduleLog.WriteWarning(Locale.IsRussian ?
                            "Отсутствуют элементы в группе \"{0}\"" :
                            "Missing items in \"{0}\" group",
                            groupConfig.DisplayName);
                    }
                    else if (groupConfig.PeriodType == PeriodType.Custom && groupConfig.CustomPeriod <= TimeSpan.Zero)
                    {
                        moduleLog.WriteError(Locale.IsRussian ?
                            "Не задан пользовательский период для группы \"{0}\"" :
                            "Custom period is not specified for \"{0}\" group",
                            groupConfig.DisplayName);
                    }
                    else
                    {
                        // check archive of the group
                        Archive archiveEntity = ServerContext.ConfigDatabase.ArchiveTable
                            .Where(a => a.Bit == groupConfig.ArchiveBit).FirstOrDefault();

                        if (archiveEntity == null)
                        {
                            moduleLog.WriteError(Locale.IsRussian ?
                                "Не найден архив для группы \"{0}\"" :
                                "Archive not found for \"{0}\" group",
                                groupConfig.DisplayName);
                        }
                        else if (archiveEntity.ArchiveKindID != ArchiveKindID.Historical)
                        {
                            moduleLog.WriteError(Locale.IsRussian ?
                                "Недопустимый вид архива в группе \"{0}\"" :
                                "Invalid archive kind in \"{0}\" group",
                                groupConfig.DisplayName);
                        }
                        else
                        {
                            // create a channel group
                            ChannelGroup channelGroup = new(groupConfig);
                            channelGroup.InitTime(utcNow);
                            channelGroups.Add(channelGroup);
                        }
                    }
                }
            }

            if (channelGroups.Count > 0)
            {
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = Locale.IsRussian ?
                    "Отсутствуют активные группы каналов." :
                    "Active channel groups missing.";
                return false;
            }
        }

        /// <summary>
        /// Performs calculations in a separate thread.
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {
                // regular calculation
                DateTime utcNow = DateTime.UtcNow;

                foreach (ChannelGroup group in channelGroups)
                {
                    ProcessGroup(group, utcNow);
                }

                // calculation by task
                if (calcTask != null)
                {
                    ExecuteTask(calcTask);
                    calcTask = null;
                }

                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }

        /// <summary>
        /// Performs calculations for the specified channel group, if necessary.
        /// </summary>
        private void ProcessGroup(ChannelGroup group, DateTime utcNow)
        {
            try
            {
                if (group.IsTimeToCalculate(utcNow, out DateTime timestamp1, out DateTime timestamp2))
                    CalculateDiffs(group, timestamp1, timestamp2);
            }
            catch (Exception ex)
            {
                moduleLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при обработке группы \"{0}\"" :
                    "Error processing \"{0}\" group",
                    group.GroupConfig.DisplayName);
            }
        }

        /// <summary>
        /// Executes the calculation task.
        /// </summary>
        private void ExecuteTask(CalculationTask task)
        {
            try
            {
                moduleLog.WriteAction(Locale.IsRussian ?
                    "Выполнение расчётной задачи за период с {0} по {1}" :
                    "Execute calculation task from {0} to {1}",
                    task.StartDT, task.EndDT);

                foreach (ChannelGroup group in channelGroups)
                {
                    DateTime timestamp1 = group.GetCalculationTime(task.StartDT);
                    DateTime timestamp2 = group.GetNextCalculationTime(timestamp1);

                    while (timestamp2 < task.EndDT)
                    {
                        CalculateDiffs(group, timestamp1, timestamp2);
                        timestamp1 = timestamp2;
                        timestamp2 = group.GetNextCalculationTime(timestamp2);
                    }
                }

                moduleLog.WriteAction(Locale.IsRussian ?
                    "Выполнение расчётной задачи завершено успешно" :
                    "Calculation task completed successfully");
            }
            catch (Exception ex)
            {
                moduleLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при выполнении расчётной задачи" :
                    "Error executing calculation task");
            }
        }

        /// <summary>
        /// Calculates differences for the specified channel group.
        /// </summary>
        private void CalculateDiffs(ChannelGroup group, DateTime timestamp1, DateTime timestamp2)
        {
            moduleLog.WriteAction(Locale.IsRussian ?
                "Расчёт разностей для группы \"{0}\" на моменты времени {1} и {2}" :
                "Calculate differences for \"{0}\" group at times {1} and {2}",
                group.GroupConfig.DisplayName, timestamp1.ToLocalizedString(), timestamp2.ToLocalizedString());

            int archiveBit = group.GroupConfig.ArchiveBit;
            Slice srcSlice1 = ServerContext.GetSlice(archiveBit, timestamp1, group.SrcCnlNums);
            Slice srcSlice2 = ServerContext.GetSlice(archiveBit, timestamp2, group.SrcCnlNums);
            Slice destSlice = new(timestamp2, group.DestCnlNums);

            for (int i = 0; i < srcSlice1.Length; i++)
            {
                CnlData cnlData1 = srcSlice1.CnlData[i];
                CnlData cnlData2 = srcSlice2.CnlData[i];

                if (cnlData1.IsDefined && cnlData2.IsDefined)
                {
                    destSlice.CnlData[i] = new CnlData(
                        cnlData2.Val - cnlData1.Val,
                        CnlStatusID.Defined);
                }
            }

            int archiveMask = ScadaUtils.SetBit(0, archiveBit, true);
            ServerContext.WriteHistoricalData(archiveMask, destSlice, WriteDataFlags.Default);
        }


        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            // write to log
            moduleLog.WriteBreak();
            moduleLog.WriteAction(ServerPhrases.StartModule, Code, Version);

            // load configuration
            if (moduleConfig.Load(Storage, ModuleConfig.DefaultFileName, out string errMsg) &&
                InitGroups(out errMsg))
            {
                // start module thread
                moduleThread = new Thread(Execute);
                moduleThread.Start();
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
            // stop module thread
            if (moduleThread != null)
            {
                terminated = true;
                moduleThread.Join();
                moduleThread = null;
            }

            // write to log
            moduleLog.WriteAction(ServerPhrases.StopModule, Code);
            moduleLog.WriteBreak();
        }

        /// <summary>
        /// Performs actions after receiving and before enqueuing a telecontrol command.
        /// </summary>
        public override void OnCommand(TeleCommand command, CommandResult commandResult)
        {
            // handle module command
            if (!string.IsNullOrEmpty(command.CmdCode) &&
                command.CmdCode == moduleConfig.GeneralOptions.CmdCode)
            {
                commandResult.TransmitToClients = false;
                IDictionary<string, string> cmdArgs = command.GetCmdDataArgs();
                string cmd = cmdArgs.GetValueAsString("cmd");

                if (cmd == CalculationTask.CommandName)
                {
                    if (calcTask == null)
                    {
                        calcTask = new CalculationTask
                        {
                            StartDT = cmdArgs.GetValueAsDateTime("startDT", DateTimeKind.Utc),
                            EndDT = cmdArgs.GetValueAsDateTime("endDT", DateTimeKind.Utc)
                        };
                    }
                    else
                    {
                        moduleLog.WriteError(Locale.IsRussian ?
                            "Расчётная задача уже в процессе выполнения" :
                            "Calculation task is already in progress");
                    }
                }
                else
                {
                    moduleLog.WriteError(Locale.IsRussian ?
                        "Неизвестная команда модуля" :
                        "Unknown module command");
                }
            }
        }
    }
}
