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
        }


        /// <summary>
        /// Gets the module code.
        /// </summary>
        public override string Code => ModuleUtils.ModuleCode;


        /// <summary>
        /// Initializes channel groups according to the module configuration.
        /// </summary>
        private bool InitChannelGroups(out string errMsg)
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
                DateTime utcNow = DateTime.UtcNow;
                channelGroups.ForEach(group => ProcessChannelGroup(group, utcNow));
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }

        /// <summary>
        /// Performs calculations for the specified channel group, if necessary.
        /// </summary>
        private void ProcessChannelGroup(ChannelGroup group, DateTime utcNow)
        {
            try
            {
                if (group.IsTimeToCalculate(utcNow, out DateTime timestamp1, out DateTime timestamp2))
                {
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
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при обработке группы \"{0}\"" :
                    "Error processing \"{0}\" group", group.GroupConfig.DisplayName);
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

            // load configuration
            if (moduleConfig.Load(Storage, ModuleConfig.DefaultFileName, out string errMsg) &&
                InitChannelGroups(out errMsg))
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
    }
}
