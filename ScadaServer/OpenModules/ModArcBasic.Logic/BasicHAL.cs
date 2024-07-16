// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Adapters;
using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules.ModArcBasic.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using static Scada.Server.Modules.ModArcBasic.Logic.ModuleConst;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Implements the historical data archive logic.
    /// <para>Реализует логику архива исторических данных.</para>
    /// </summary>
    internal class BasicHAL : HistoricalArchiveLogic
    {
        private readonly ModuleConfig moduleConfig;        // the module configuration
        private readonly BasicHAO options;                 // the archive options
        private readonly int writingPeriod;                // the writing period in seconds
        private readonly int writingOffset;                // the writing offset in seconds
        private readonly ILog appLog;                      // the application log
        private readonly ILog arcLog;                      // the archive log
        private readonly DataQueue<Slice> sliceQueue;      // contains slices for writing
        private readonly TrendTableAdapter readingAdapter; // reads historical data
        private readonly TrendTableAdapter writingAdapter; // writes historical data
        private readonly MemoryCache<DateTime, TrendTable> tableCache; // the cache containing trend tables
        private readonly object readingLock;               // synchronizes reading from the archive
        private readonly object writingLock;               // synchronizes writing to the archive

        private Thread thread;            // the thread for writing data
        private volatile bool terminated; // necessary to stop the thread
        private DateTime nextWriteTime;   // the next time to write data to the archive
        private int[] cnlIndexes;         // the channel mapping indexes
        private CnlNumList cnlNumList;    // the list of the channel numbers processed by the archive


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicHAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums,
            ModuleConfig moduleConfig) : base(archiveContext, archiveConfig, cnlNums)
        {
            this.moduleConfig = moduleConfig ?? throw new ArgumentNullException(nameof(moduleConfig));
            options = new BasicHAO(archiveConfig.CustomOptions);
            writingPeriod = ConvertToSeconds(options.WritingPeriod, options.WritingPeriodUnit);
            writingOffset = ConvertToSeconds(options.WritingOffset, options.WritingOffsetUnit);
            appLog = archiveContext.Log;
            arcLog = options.LogEnabled ? CreateLog(ModuleUtils.ModuleCode) : null;
            sliceQueue = new DataQueue<Slice>(options.MaxQueueSize);
            readingAdapter = CreateAdapter();
            writingAdapter = CreateAdapter();
            tableCache = new MemoryCache<DateTime, TrendTable>(CacheExpiration, CacheCapacity);
            readingLock = new object();
            writingLock = new object();

            thread = null;
            terminated = false;
            nextWriteTime = DateTime.MinValue;
            cnlIndexes = null;
            cnlNumList = new CnlNumList(cnlNums);
        }


        /// <summary>
        /// Gets the archive options.
        /// </summary>
        protected override HistoricalArchiveOptions ArchiveOptions => options;

        /// <summary>
        /// Gets the current archive status as text.
        /// </summary>
        public override string StatusText
        {
            get
            {
                return GetStatusText(sliceQueue.Stats, sliceQueue.Count);
            }
        }


        /// <summary>
        /// Creates a trend table adapter.
        /// </summary>
        private TrendTableAdapter CreateAdapter()
        {
            return new TrendTableAdapter
            {
                ArchiveCode = Code,
                CnlNumCache = new MemoryCache<long, CnlNumList>(CacheExpiration, CacheCapacity)
            };
        }

        /// <summary>
        /// Validates the archive options and throws an exception on fail.
        /// </summary>
        private void ValidateOptions()
        {
            if (options.ReadOnly)
                throw new ScadaException(ServerPhrases.ReadOnlyNotSupported);

            if (options.WriteOnChange)
                throw new ScadaException(ServerPhrases.WritingOnChangeNotSupported);

            if (options.WritingPeriod <= 0)
                throw new ScadaException(ServerPhrases.InvalidWritingPeriod);
        }

        /// <summary>
        /// Checks and updates the today's trend table.
        /// </summary>
        private void CheckCurrentTrendTable(DateTime nowDT)
        {
            TrendTable currentTable = new TrendTable(nowDT.Date, writingPeriod) { CnlNumList = cnlNumList };
            currentTable.SetDefaultMetadata();

            string tableDir = readingAdapter.GetTablePath(currentTable);
            string metaFileName = readingAdapter.GetMetaPath(currentTable);

            if (Directory.Exists(tableDir))
            {
                TrendTableMeta srcTableMeta = readingAdapter.ReadMetadata(metaFileName);

                if (srcTableMeta == null)
                {
                    // the existing table is invalid and should be deleted
                    Directory.Delete(tableDir, true);
                }
                else if (srcTableMeta.Equals(currentTable.Metadata))
                {
                    if (currentTable.GetDataPosition(nowDT, PositionKind.Ceiling,
                        out TrendTablePage page, out _))
                    {
                        string pageFileName = readingAdapter.GetPagePath(page);
                        CnlNumList srcCnlNums = readingAdapter.ReadCnlNums(pageFileName);

                        if (srcCnlNums == null)
                        {
                            // make sure that there is no page file
                            File.Delete(pageFileName);
                        }
                        else if (srcCnlNums.Equals(cnlNumList))
                        {
                            // re-create the channel list to use the existing list ID
                            cnlNumList = new CnlNumList(srcCnlNums.ListID, cnlNumList);
                        }
                        else
                        {
                            // update the current page
                            string msg = string.Format(Locale.IsRussian ?
                                "Обновление номеров каналов страницы {0}" :
                                "Update channel numbers of the page {0}", pageFileName);
                            appLog.WriteAction(ServerPhrases.ArchiveMessage, Code, msg);
                            arcLog?.WriteAction(msg);
                            readingAdapter.UpdatePageChannels(page, srcCnlNums);
                        }
                    }
                }
                else
                {
                    // updating the entire table structure would take too long, so just backup the table
                    string msg = string.Format(Locale.IsRussian ?
                        "Резервное копирование таблицы {0}" :
                        "Backup the table {0}", tableDir);
                    appLog.WriteAction(ServerPhrases.ArchiveMessage, Code, msg);
                    arcLog?.WriteAction(msg);
                    readingAdapter.BackupTable(currentTable);
                }
            }

            // create an empty table if it does not exist
            if (!Directory.Exists(tableDir))
            {
                readingAdapter.WriteMetadata(metaFileName, currentTable.Metadata);
                currentTable.IsReady = true;
            }

            // add the archive channel list to the cache
            readingAdapter.CnlNumCache.Add(cnlNumList.ListID, cnlNumList);
        }

        /// <summary>
        /// Gets the trend table from the cache, creating a table if necessary.
        /// </summary>
        private TrendTable GetTrendTable(DateTime timestamp)
        {
            DateTime tableDate = timestamp.Date;

            return tableCache.GetOrCreate(tableDate, () =>
            {
                return new TrendTable(tableDate, writingPeriod) { CnlNumList = cnlNumList };
            });
        }

        /// <summary>
        /// Writes slices from the queue.
        /// </summary>
        private void WriteSlices()
        {
            bool updated = false;

            try
            {
                for (int i = 0; i < SlicesPerIteration; i++)
                {
                    if (sliceQueue.TryDequeueValue(out Slice slice))
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        TrendTable trendTable = GetTrendTable(slice.Timestamp);

                        lock (trendTable)
                        {
                            writingAdapter.WriteSlice(trendTable, slice);
                        }

                        updated = true;
                        stopwatch.Stop();
                        arcLog?.WriteAction(ServerPhrases.WritingSliceCompleted,
                            slice.Length, stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                sliceQueue.Stats.HasError = true;
                appLog?.WriteError(ex, ServerPhrases.ArchiveMessage, Code, ServerPhrases.WriteFileError);
                arcLog?.WriteError(ex, ServerPhrases.WriteFileError);
                Thread.Sleep(ScadaUtils.ErrorDelay);
            }
            finally
            {
                if (updated)
                    LastWriteTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Writing loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {
                WriteSlices();
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            ValidateOptions();

            string parentDir = Path.Combine(moduleConfig.SelectArcDir(options.UseCopyDir), Code);
            readingAdapter.ParentDirectory = parentDir;
            writingAdapter.ParentDirectory = parentDir;
            Directory.CreateDirectory(parentDir);

            DateTime utcNow = DateTime.UtcNow;
            CheckCurrentTrendTable(utcNow);

            if (options.WriteWithPeriod)
                nextWriteTime = GetNextWriteTime(utcNow, writingPeriod, writingOffset);

            // start thread for writing data
            terminated = false;
            thread = new Thread(Execute);
            thread.Start();
        }

        /// <summary>
        /// Closes the archive.
        /// </summary>
        public override void Close()
        {
            if (thread != null)
            {
                terminated = true;
                thread.Join();
                thread = null;
            }

            WriteSlices(); // flush
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public override void DeleteOutdatedData()
        {
            DirectoryInfo arcDirInfo = new DirectoryInfo(readingAdapter.ParentDirectory);

            if (arcDirInfo.Exists)
            {
                lock (readingLock)
                {
                    DateTime minDT = DateTime.UtcNow.AddDays(-options.Retention);
                    string minDirName = TrendTableAdapter.GetTableDirectory(Code, minDT);
                    appLog.WriteAction(ServerPhrases.DeleteOutdatedData, Code, minDT.ToLocalizedDateString());

                    foreach (DirectoryInfo dirInfo in
                        arcDirInfo.EnumerateDirectories(Code + "*", SearchOption.TopDirectoryOnly))
                    {
                        if (string.CompareOrdinal(dirInfo.Name, minDirName) < 0)
                            dirInfo.Delete(true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the trends of the specified channels.
        /// </summary>
        public override TrendBundle GetTrends(TimeRange timeRange, int[] cnlNums)
        {
            lock (readingLock)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                TrendBundle trendBundle;
                List<TrendBundle> bundles = new List<TrendBundle>();
                int totalCapacity = 0;

                foreach (DateTime date in EnumerateDates(timeRange))
                {
                    TrendTable trendTable = GetTrendTable(date);
                    TrendBundle bundle;

                    lock (trendTable)
                    {
                        bundle = readingAdapter.ReadTrends(trendTable, timeRange, cnlNums);
                    }

                    bundles.Add(bundle);
                    totalCapacity += bundle.Timestamps.Count;
                }

                if (bundles.Count <= 0)
                {
                    trendBundle = new TrendBundle(cnlNums, 0);
                }
                else if (bundles.Count == 1)
                {
                    trendBundle = bundles[0];
                }
                else
                {
                    // unite bundles
                    trendBundle = new TrendBundle(cnlNums, totalCapacity);

                    foreach (TrendBundle bundle in bundles)
                    {
                        trendBundle.Timestamps.AddRange(bundle.Timestamps);

                        for (int i = 0, trendCnt = trendBundle.Trends.Count; i < trendCnt; i++)
                        {
                            trendBundle.Trends[i].AddRange(bundle.Trends[i]);
                        }
                    }
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingTrendsCompleted,
                    trendBundle.Timestamps.Count, stopwatch.ElapsedMilliseconds);
                return trendBundle;
            }
        }

        /// <summary>
        /// Gets the trend of the specified channel.
        /// </summary>
        public override Trend GetTrend(TimeRange timeRange, int cnlNum)
        {
            lock (readingLock)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                Trend resultTrend;
                List<Trend> trends = new List<Trend>();
                int totalCapacity = 0;

                foreach (DateTime date in EnumerateDates(timeRange))
                {
                    TrendTable trendTable = GetTrendTable(date);
                    Trend trend;

                    lock (trendTable)
                    {
                        trend = readingAdapter.ReadTrend(trendTable, timeRange, cnlNum);
                    }

                    trends.Add(trend);
                    totalCapacity += trend.Points.Count;
                }

                if (trends.Count <= 0)
                {
                    resultTrend = new Trend(cnlNum, 0);
                }
                else if (trends.Count == 1)
                {
                    resultTrend = trends[0];
                }
                else
                {
                    // unite trends
                    resultTrend = new Trend(cnlNum, totalCapacity);

                    foreach (Trend trend in trends)
                    {
                        resultTrend.Points.AddRange(trend.Points);
                    }
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingTrendCompleted,
                    resultTrend.Points.Count, stopwatch.ElapsedMilliseconds);
                return resultTrend;
            }
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public override List<DateTime> GetTimestamps(TimeRange timeRange)
        {
            lock (readingLock)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                List<DateTime> resultTimestamps;
                List<List<DateTime>> listOfTimestamps = new List<List<DateTime>>();
                int totalCapacity = 0;

                foreach (DateTime date in EnumerateDates(timeRange))
                {
                    TrendTable trendTable = GetTrendTable(date);
                    List<DateTime> timestamps;

                    lock (trendTable)
                    {
                        timestamps = readingAdapter.ReadTimestamps(trendTable, timeRange);
                    }

                    listOfTimestamps.Add(timestamps);
                    totalCapacity += timestamps.Count;
                }

                if (listOfTimestamps.Count <= 0)
                {
                    resultTimestamps = new List<DateTime>();
                }
                else if (listOfTimestamps.Count == 1)
                {
                    resultTimestamps = listOfTimestamps[0];
                }
                else
                {
                    // unite timestamps
                    resultTimestamps = new List<DateTime>(totalCapacity);

                    foreach (List<DateTime> timestamps in listOfTimestamps)
                    {
                        resultTimestamps.AddRange(timestamps);
                    }
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingTimestampsCompleted,
                    resultTimestamps.Count, stopwatch.ElapsedMilliseconds);
                return resultTimestamps;
            }
        }

        /// <summary>
        /// Gets the slice of the specified channels at the timestamp.
        /// </summary>
        public override Slice GetSlice(DateTime timestamp, int[] cnlNums)
        {
            lock (readingLock)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                TrendTable trendTable = GetTrendTable(timestamp);
                Slice slice;

                lock (trendTable)
                {
                    slice = readingAdapter.ReadSlice(trendTable, timestamp, cnlNums);
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingSliceCompleted, slice.Length, stopwatch.ElapsedMilliseconds);
                return slice;
            }
        }

        /// <summary>
        /// Gets the channel data.
        /// </summary>
        public override CnlData GetCnlData(DateTime timestamp, int cnlNum)
        {
            if (GetRecentCnlData(writingLock, timestamp, cnlNum, out CnlData cnlData))
                return cnlData;

            lock (readingLock)
            {
                TrendTable trendTable = GetTrendTable(timestamp);

                lock (trendTable)
                {
                    return readingAdapter.ReadCnlData(trendTable, timestamp, cnlNum);
                }
            }
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override void ProcessData(ICurrentData curData)
        {
            if (options.WriteWithPeriod && nextWriteTime <= curData.Timestamp)
            {
                DateTime writeTime = GetClosestWriteTime(curData.Timestamp, writingPeriod, writingOffset);
                nextWriteTime = writeTime.AddSeconds(writingPeriod);

                Slice slice = new Slice(writeTime, CnlNums);
                InitCnlIndexes(curData, ref cnlIndexes);
                CopyCnlData(curData, slice, cnlIndexes);
                sliceQueue.Enqueue(slice.Timestamp, slice);
            }
        }

        /// <summary>
        /// Accepts or rejects data with the specified timestamp.
        /// </summary>
        public override bool AcceptData(ref DateTime timestamp)
        {
            if (TimeInsideRetention(timestamp, DateTime.UtcNow))
            {
                return options.PullToPeriod > 0 
                    ? PullTimeToPeriod(ref timestamp, writingPeriod, writingOffset, options.PullToPeriod)
                    : TimeIsMultipleOfPeriod(timestamp, writingPeriod, writingOffset);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Maintains performance when data is written one at a time.
        /// </summary>
        public override void BeginUpdate(UpdateContext updateContext)
        {
            Monitor.Enter(writingLock);
        }

        /// <summary>
        /// Updates the channel data.
        /// </summary>
        public override void UpdateData(UpdateContext updateContext, int cnlNum, CnlData cnlData)
        {
            updateContext.UpdatedData[cnlNum] = cnlData;
        }

        /// <summary>
        /// Completes the update operation.
        /// </summary>
        public override void EndUpdate(UpdateContext updateContext)
        {
            // convert updated data to slice
            if (updateContext.UpdatedData.Count > 0)
            {
                Slice slice = new Slice(updateContext.Timestamp, updateContext.UpdatedData.Count);
                int i = 0;

                foreach (KeyValuePair<int, CnlData> pair in updateContext.UpdatedData)
                {
                    slice.CnlNums[i] = pair.Key;
                    slice.CnlData[i] = pair.Value;
                    i++;
                }

                sliceQueue.Enqueue(slice.Timestamp, slice);
            }

            Monitor.Exit(writingLock);
        }
    }
}
