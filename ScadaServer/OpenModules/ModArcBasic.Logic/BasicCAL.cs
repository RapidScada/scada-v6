// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Adapters;
using Scada.Data.Models;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules.ModArcBasic.Config;
using System;
using System.Diagnostics;
using System.IO;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Implements the current data archive logic.
    /// <para>Реализует логику архива текущих данных.</para>
    /// </summary>
    internal class BasicCAL : CurrentArchiveLogic
    {
        /// <summary>
        /// The current data file name.
        /// </summary>
        private const string CurDataFileName = "current.dat";

        private readonly ModuleConfig moduleConfig; // the module configuration
        private readonly BasicCAO options;          // the archive options
        private readonly ILog arcLog;               // the archive log
        private readonly Stopwatch stopwatch;       // measures the time of operations
        private readonly SliceTableAdapter adapter; // reads and writes current data
        private readonly Slice slice;               // the slice for writing

        private DateTime nextWriteTime; // the next time to write the current data
        private int[] cnlIndexes;       // the channel mapping indexes


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicCAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums, 
            ModuleConfig moduleConfig) : base(archiveContext, archiveConfig, cnlNums)
        {
            this.moduleConfig = moduleConfig ?? throw new ArgumentNullException(nameof(moduleConfig));
            options = new BasicCAO(archiveConfig.CustomOptions);
            arcLog = options.LogEnabled ? CreateLog(ModuleUtils.ModuleCode) : null;
            stopwatch = new Stopwatch();
            adapter = new SliceTableAdapter();
            slice = new Slice(DateTime.MinValue, cnlNums);

            nextWriteTime = DateTime.MinValue;
            cnlIndexes = null;
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            adapter.FileName = Path.Combine(moduleConfig.SelectArcDir(options.UseCopyDir), Code, CurDataFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(adapter.FileName));
            nextWriteTime = GetNextWriteTime(DateTime.UtcNow, options.FlushPeriod);
        }

        /// <summary>
        /// Reads the current data.
        /// </summary>
        public override void ReadData(ICurrentData curData, out bool completed)
        {
            if (File.Exists(adapter.FileName))
            {
                stopwatch.Restart();
                Slice slice = adapter.ReadSingleSlice();

                for (int i = 0, cnlCnt = slice.CnlNums.Length; i < cnlCnt; i++)
                {
                    int cnlNum = slice.CnlNums[i];
                    int cnlIndex = curData.GetCnlIndex(cnlNum);

                    if (cnlIndex >= 0)
                    {
                        curData.Timestamps[cnlIndex] = slice.Timestamp;
                        curData.CnlData[cnlIndex] = slice.CnlData[i];
                    }
                }

                completed = true;
                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingSliceCompleted, 
                    slice.CnlNums.Length, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                completed = false;
            }
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public override void WriteData(ICurrentData curData)
        {
            stopwatch.Restart();
            InitCnlIndexes(curData, ref cnlIndexes);
            CopyCnlData(curData, slice, cnlIndexes);
            adapter.WriteSingleSlice(slice);

            stopwatch.Stop();
            arcLog?.WriteAction(ServerPhrases.WritingSliceCompleted,
                slice.CnlNums.Length, stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override bool ProcessData(ICurrentData curData)
        {
            if (nextWriteTime <= curData.Timestamp)
            {
                nextWriteTime = GetNextWriteTime(curData.Timestamp, options.FlushPeriod);
                WriteData(curData);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
