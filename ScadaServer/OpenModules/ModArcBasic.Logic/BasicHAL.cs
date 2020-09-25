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
 * Module   : ModArcBasic
 * Summary  : Implements the historical data archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;
using Scada.Data.Adapters;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Implements the historical data archive logic.
    /// <para>Реализует логику архива исторических данных.</para>
    /// </summary>
    internal class BasicHAL : HistoricalArchiveLogic
    {
        /// <summary>
        /// Represents archive options.
        /// </summary>
        private class ArchiveOptions
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public ArchiveOptions(CustomOptions options)
            {
                IsCopy = options.GetValueAsBool("IsCopy");
                WritingPeriod = options.GetValueAsInt("WritingPeriod", 1);
                WritingUnit = options.GetValueAsEnum("WritingUnit", TimeUnit.Minute);
                WritingMode = options.GetValueAsEnum("WritingMode", WritingMode.Auto);
                StoragePeriod = options.GetValueAsInt("StoragePeriod", 365);
                LogEnabled = options.GetValueAsBool("LogEnabled");
            }

            /// <summary>
            /// Gets or sets a value indicating whether the archive stores a copy of the data.
            /// </summary>
            public bool IsCopy { get; set; }
            /// <summary>
            /// Gets the period of writing data to a file.
            /// </summary>
            public int WritingPeriod { get; set; }
            /// <summary>
            /// Gets the unit of measure for the writing period.
            /// </summary>
            public TimeUnit WritingUnit { get; set; }
            /// <summary>
            /// Gets the writing mode.
            /// </summary>
            public WritingMode WritingMode { get; set; }
            /// <summary>
            /// Gets the data storage period in days.
            /// </summary>
            public int StoragePeriod { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether to write the archive log.
            /// </summary>
            public bool LogEnabled { get; set; }
        }

        private readonly ArchiveOptions options;    // the archive options
        private readonly ILog appLog;               // the application log
        private readonly ILog arcLog;               // the archive log
        private readonly Stopwatch stopwatch;       // measures the time of operations
        private readonly TrendTableAdapter adapter; // reads and writes historical data
        private readonly MemoryCache<DateTime, TrendTable> tableCache; // the cache containing trend tables
        private readonly Slice slice;               // the slice for writing
        private readonly int writingPeriod;         // the writing period in seconds

        private DateTime nextWriteTime;             // the next time to write data to the archive
        private int[] cnlIndices;                   // the indices that map the input channels
        private CnlNumList cnlNumList;              // the list of the input channel numbers processed by the archive
        private TrendTable currentTable;            // the today's trend table
        private TrendTable updatedTable;            // the trend table that is currently being updated


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicHAL(ArchiveConfig archiveConfig, int[] cnlNums, 
            ServerConfig serverConfig, ServerDirs serverDirs, ILog log) 
            : base(archiveConfig, cnlNums)
        {
            options = new ArchiveOptions(archiveConfig.CustomOptions);
            appLog = log ?? throw new ArgumentNullException("log");
            arcLog = options.LogEnabled ? 
                ModUtils.CreateArchiveLog(serverDirs.LogDir, Code, serverConfig.GeneralOptions.MaxLogSize) : null;
            stopwatch = new Stopwatch();
            adapter = new TrendTableAdapter
            {
                ParentDirectory = GetArchivePath(serverConfig.PathOptions),
                ArchiveCode = Code,
                CnlNumCache = new MemoryCache<long, CnlNumList>(ModUtils.CacheExpiration, ModUtils.CacheCapacity)
            };
            tableCache = new MemoryCache<DateTime, TrendTable>(ModUtils.CacheExpiration, ModUtils.CacheCapacity);
            slice = new Slice(DateTime.MinValue, cnlNums);
            writingPeriod = GetWritingPeriodInSec(options);

            nextWriteTime = options.WritingMode == WritingMode.Auto ? 
                GetNextWriteTime(DateTime.UtcNow, writingPeriod) : DateTime.MinValue;
            cnlIndices = null;
            cnlNumList = new CnlNumList(cnlNums);
            currentTable = null;
            updatedTable = null;
        }


        /// <summary>
        /// Gets the full path of the archive.
        /// </summary>
        private string GetArchivePath(PathOptions pathOptions)
        {
            string arcDir = options.IsCopy ? pathOptions.ArcCopyDir : pathOptions.ArcDir;
            return Path.Combine(arcDir, Code);
        }

        /// <summary>
        /// Gets the writing period in seconds.
        /// </summary>
        private int GetWritingPeriodInSec(ArchiveOptions options)
        {
            switch (options.WritingUnit)
            {
                case TimeUnit.Minute:
                    return options.WritingPeriod * 60;
                case TimeUnit.Hour:
                    return options.WritingPeriod * 1440;
                default: // TimeUnit.Second
                    return options.WritingPeriod;
            }
        }

        /// <summary>
        /// Gets the today's trend table, creating it if necessary.
        /// </summary>
        private TrendTable GetCurrentTrendTable(DateTime nowDT)
        {
            DateTime today = nowDT.Date;

            if (currentTable == null)
            {
                currentTable = new TrendTable(today, writingPeriod) { CnlNumList = cnlNumList };
                currentTable.SetDefaultMetadata();
            }
            else if (currentTable.TableDate != today) // current date is changed
            {
                tableCache.Add(currentTable.TableDate, currentTable);
                currentTable = new TrendTable(today, writingPeriod) { CnlNumList = cnlNumList };
                currentTable.SetDefaultMetadata();
            }

            return currentTable;
        }

        /// <summary>
        /// Gets the trend table from the cache, creating a table if necessary.
        /// </summary>
        private TrendTable GetTrendTable(DateTime timestamp)
        {
            DateTime tableDate = timestamp.Date;

            if (currentTable != null && currentTable.TableDate == tableDate)
            {
                return currentTable;
            }
            else if (updatedTable != null && updatedTable.TableDate == tableDate)
            {
                return updatedTable;
            }
            else
            {
                TrendTable trendTable = tableCache.Get(tableDate);

                if (trendTable == null)
                {
                    trendTable = new TrendTable(tableDate, writingPeriod) { CnlNumList = cnlNumList };
                    tableCache.Add(tableDate, trendTable);
                }

                return trendTable;
            }
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            // create the archive directory
            Directory.CreateDirectory(adapter.ParentDirectory);

            // check and update the today's trend table
            DateTime utcNow = DateTime.UtcNow;
            currentTable = GetCurrentTrendTable(utcNow);
            string tableDir = adapter.GetTablePath(currentTable);
            string metaFileName = adapter.GetMetaPath(currentTable);

            if (Directory.Exists(tableDir))
            {
                TrendTableMeta srcTableMeta = adapter.ReadMetadata(metaFileName);

                if (srcTableMeta == null)
                {
                    // the existing table is invalid and should be deleted
                    Directory.Delete(tableDir, true);
                }
                else if (srcTableMeta.Equals(currentTable.Metadata))
                {
                    if (currentTable.GetDataPosition(utcNow, PositionKind.Ceiling,
                        out TrendTablePage page, out int indexInPage))
                    {
                        string pageFileName = adapter.GetPagePath(page);
                        CnlNumList srcCnlNums = adapter.ReadCnlNums(pageFileName);

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
                            appLog.WriteAction(Locale.IsRussian ?
                                "Обновление номеров каналов страницы {0}" :
                                "Update channel numbers of the page {0}", pageFileName);
                            adapter.UpdatePageChannels(page, srcCnlNums);
                        }
                    }
                }
                else
                {
                    // updating the entire table structure would take too long, so just backup the table
                    appLog.WriteAction(Locale.IsRussian ?
                        "Резервное копирование таблицы {0}" :
                        "Backup the table {0}", tableDir);
                    adapter.BackupTable(currentTable);
                }
            }

            // create an empty table if it does not exist
            if (!Directory.Exists(tableDir))
            {
                adapter.WriteMetadata(metaFileName, currentTable.Metadata);
                currentTable.IsReady = true;
            }

            // add the archive channel list to the cache
            adapter.CnlNumCache.Add(cnlNumList.ListID, cnlNumList);
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public override void DeleteOutdatedData()
        {
            DirectoryInfo arcDirInfo = new DirectoryInfo(adapter.ParentDirectory);

            if (arcDirInfo.Exists)
            {
                DateTime minDT = DateTime.UtcNow.AddDays(-options.StoragePeriod);
                string minDirName = TrendTableAdapter.GetTableDirectory(Code, minDT);

                appLog.WriteAction(Locale.IsRussian ?
                    "Удаление устаревших данных из архива {0}, которые старше {1}" :
                    "Delete outdated data from the {0} archive older than {1}",
                    Code, minDT.ToLocalizedDateString());

                foreach (DirectoryInfo dirInfo in
                    arcDirInfo.EnumerateDirectories(Code + "*", SearchOption.TopDirectoryOnly))
                {
                    if (string.CompareOrdinal(dirInfo.Name, minDirName) < 0)
                        dirInfo.Delete(true);
                }
            }
        }

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        public override TrendBundle GetTrends(int[] cnlNums, DateTime startTime, DateTime endTime, bool endInclusive)
        {
            stopwatch.Restart();
            TrendBundle trendBundle;
            List<TrendBundle> bundles = new List<TrendBundle>();
            int totalCapacity = 0;

            foreach (DateTime date in EnumerateDates(startTime, endTime, endInclusive))
            {
                TrendTable trendTable = GetTrendTable(date);
                TrendBundle bundle = adapter.ReadTrends(trendTable, cnlNums, startTime, endTime, endInclusive);
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
            arcLog?.WriteAction(Locale.IsRussian ?
                "Чтение трендов длины {0} успешно завершено за {1} мс" :
                "Reading trends of length {0} completed successfully in {1} ms",
                trendBundle.Timestamps.Count, stopwatch.ElapsedMilliseconds);

            return trendBundle;
        }

        /// <summary>
        /// Gets the trend of the specified input channel.
        /// </summary>
        public override Trend GetTrend(int cnlNum, DateTime startTime, DateTime endTime, bool endInclusive)
        {
            stopwatch.Restart();
            Trend resultTrend;
            List<Trend> trends = new List<Trend>();
            int totalCapacity = 0;

            foreach (DateTime date in EnumerateDates(startTime, endTime, endInclusive))
            {
                TrendTable trendTable = GetTrendTable(date);
                Trend trend = adapter.ReadTrend(trendTable, cnlNum, startTime, endTime, endInclusive);
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
            arcLog?.WriteAction(Locale.IsRussian ?
                "Чтение тренда длины {0} успешно завершено за {1} мс" :
                "Reading a trend of length {0} completed successfully in {1} ms",
                resultTrend.Points.Count, stopwatch.ElapsedMilliseconds);

            return resultTrend;
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public override List<DateTime> GetTimestamps(DateTime startTime, DateTime endTime, bool endInclusive)
        {
            stopwatch.Restart();
            List<DateTime> resultTimestamps;
            List<List<DateTime>> listOfTimestamps = new List<List<DateTime>>();
            int totalCapacity = 0;

            foreach (DateTime date in EnumerateDates(startTime, endTime, endInclusive))
            {
                TrendTable trendTable = GetTrendTable(date);
                List<DateTime> timestamps = adapter.ReadTimestamps(trendTable, startTime, endTime, endInclusive);
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
            arcLog?.WriteAction(Locale.IsRussian ?
                "Чтение меток времени длины {0} успешно завершено за {1} мс" :
                "Reading timestamps of length {0} completed successfully in {1} ms",
                resultTimestamps.Count, stopwatch.ElapsedMilliseconds);

            return resultTimestamps;
        }

        /// <summary>
        /// Gets the slice of the specified input channels at the timestamp.
        /// </summary>
        public override Slice GetSlice(int[] cnlNums, DateTime timestamp)
        {
            stopwatch.Restart();
            Slice slice = adapter.ReadSlice(GetTrendTable(timestamp), cnlNums, timestamp);
            stopwatch.Stop();
            arcLog?.WriteAction(Locale.IsRussian ?
                "Чтение среза длины {0} успешно завершено за {1} мс" :
                "Reading a slice of length {0} completed successfully in {1} ms",
                slice.CnlNums.Length, stopwatch.ElapsedMilliseconds);
            return slice;
        }

        /// <summary>
        /// Gets the input channel data.
        /// </summary>
        public override CnlData GetCnlData(int cnlNum, DateTime timestamp)
        {
            return adapter.ReadCnlData(GetTrendTable(timestamp), cnlNum, timestamp);
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override bool ProcessData(ICurrentData curData)
        {
            if (options.WritingMode == WritingMode.Auto && nextWriteTime <= curData.Timestamp)
            {
                DateTime writeTime = GetClosestWriteTime(curData.Timestamp, writingPeriod);
                nextWriteTime = writeTime.AddSeconds(writingPeriod);

                stopwatch.Restart();
                TrendTable trendTable = GetCurrentTrendTable(writeTime);
                InitCnlIndices(curData, ref cnlIndices);
                CopyCnlData(curData, slice, cnlIndices);
                slice.Timestamp = writeTime;
                adapter.WriteSlice(trendTable, slice);

                stopwatch.Stop();
                arcLog?.WriteAction(Locale.IsRussian ?
                    "Запись среза длины {0} успешно завершена за {1} мс" :
                    "Writing a slice of length {0} completed successfully in {1} ms",
                    slice.CnlNums.Length, stopwatch.ElapsedMilliseconds);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Accepts or rejects data with the specified timestamp.
        /// </summary>
        public override bool AcceptData(DateTime timestamp)
        {
            return writingPeriod > 0 &&
                (int)Math.Round(timestamp.TimeOfDay.TotalMilliseconds) % (writingPeriod * 1000) == 0;
        }

        /// <summary>
        /// Maintains performance when data is written one at a time.
        /// </summary>
        public override void BeginUpdate(int deviceNum, DateTime timestamp)
        {
            stopwatch.Restart();
            updatedTable = GetTrendTable(timestamp);
        }

        /// <summary>
        /// Completes the update operation.
        /// </summary>
        public override void EndUpdate(int deviceNum, DateTime timestamp)
        {
            updatedTable = null;
            stopwatch.Stop();
            arcLog?.WriteAction(Locale.IsRussian ?
                "Обновление данных успешно завершено за {1} мс" :
                "Data update completed successfully in {0} ms",
                slice.CnlNums.Length, stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Writes the input channel data.
        /// </summary>
        public override void WriteCnlData(int cnlNum, DateTime timestamp, CnlData cnlData)
        {
            adapter.WriteCnlData(GetTrendTable(timestamp), cnlNum, timestamp, cnlData);
        }
    }
}
