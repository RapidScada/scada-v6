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
        }


        private readonly ILog log;                  // the application log
        private readonly ArchiveOptions options;    // the archive options
        private readonly MemoryCache<DateTime, TrendTable> tableCache; // the cache containing trend tables
        private readonly TrendTableAdapter adapter; // reads and writes historical data
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
        public BasicHAL(ArchiveConfig archiveConfig, int[] cnlNums, PathOptions pathOptions, ILog log)
            : base(archiveConfig, cnlNums)
        {
            this.log = log ?? throw new ArgumentNullException("log");
            options = new ArchiveOptions(archiveConfig.CustomOptions);
            tableCache = new MemoryCache<DateTime, TrendTable>(ModUtils.CacheExpiration, ModUtils.CacheCapacity);
            adapter = new TrendTableAdapter
            {
                ParentDirectory = options.IsCopy ? pathOptions.ArcCopyDir : pathOptions.ArcDir,
                ArchiveCode = Code,
                CnlNumCache = new MemoryCache<long, CnlNumList>(ModUtils.CacheExpiration, ModUtils.CacheCapacity)
            };
            slice = new Slice(DateTime.MinValue, cnlNums, new CnlData[cnlNums.Length]);
            writingPeriod = GetWritingPeriodInSec(options);
            nextWriteTime = options.WritingMode == WritingMode.Auto ? 
                GetNextWriteTime(DateTime.UtcNow, writingPeriod) : DateTime.MinValue;
            cnlIndices = null;
            cnlNumList = new CnlNumList(cnlNums);
            currentTable = null;
            updatedTable = null;
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
            }
            else if (currentTable.TableDate != today) // current date is changed
            {
                tableCache.Add(currentTable.TableDate, currentTable);
                currentTable = new TrendTable(today, writingPeriod) { CnlNumList = cnlNumList };
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
            // create archive directory
            Directory.CreateDirectory(adapter.ParentDirectory);

            // check and update structure of the today's trend table
            DateTime utcNow = DateTime.UtcNow;
            DateTime tableDate = utcNow.Date;
            string tableDirectory = Path.Combine(adapter.ParentDirectory, 
                TrendTableAdapter.GetTableDirectory(Code, tableDate));

            if (Directory.Exists(tableDirectory))
            {
                TrendTable trendTable = new TrendTable(tableDate, writingPeriod) { CnlNumList = cnlNumList };
                string metaFileName = adapter.GetMetaPath(trendTable);
                TrendTableMeta tableMeta = adapter.ReadMetadata(metaFileName);

                if (tableMeta == null)
                {
                    // existing table is invalid and should be deleted
                    Directory.Delete(tableDirectory, true);
                }
                else if (trendTable.Metadata.Equals(tableMeta))
                {
                    if (trendTable.GetPage(utcNow, out TrendTablePage page))
                    {
                        string pageFileName = adapter.GetPagePath(page);
                        CnlNumList cnlNums = adapter.ReadCnlNums(pageFileName);

                        if (cnlNums == null)
                        {
                            // make sure that there is no page file
                            File.Delete(pageFileName);
                        }
                        else if (cnlNums.Equals(cnlNumList))
                        {
                            // use existing list ID
                            cnlNumList = new CnlNumList(cnlNums.ListID, cnlNumList);
                        }
                        else
                        {
                            // update one page
                            adapter.UpdatePageStructure(page);
                        }
                    }
                }
                else
                {
                    // update whole table
                    adapter.UpdateTableStructure(trendTable);
                }

                currentTable = trendTable;
                adapter.CnlNumCache.Add(cnlNumList.ListID, cnlNumList);
            }
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

                log.WriteAction(string.Format(Locale.IsRussian ?
                    "Удаление устаревших данных из архива {0}, которые старше {1}" :
                    "Delete outdated data from the {0} archive older than {1}",
                    Code, minDT.ToLocalizedDateString()));

                foreach (DirectoryInfo dirInfo in
                    arcDirInfo.EnumerateDirectories(Code + "*", SearchOption.TopDirectoryOnly))
                {
                    if (string.CompareOrdinal(dirInfo.Name, minDirName) < 0)
                        dirInfo.Delete(true);
                }
            }
        }

        /// <summary>
        /// Gets the input channel data.
        /// </summary>
        public override CnlData GetCnlData(int cnlNum, DateTime timestamp)
        {
            return CnlData.Empty;
        }

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        public override TrendBundle GetTrends(int[] cnlNums, DateTime startTime, DateTime endTime)
        {
            return new TrendBundle(cnlNums, 0);
        }

        /// <summary>
        /// Gets the trend of the specified input channel.
        /// </summary>
        public override Trend GetTrend(int cnlNum, DateTime startTime, DateTime endTime)
        {
            return new Trend(cnlNum, 0);
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public override List<DateTime> GetTimestamps(DateTime startTime, DateTime endTime)
        {
            return new List<DateTime>();
        }

        /// <summary>
        /// Gets the slice of the specified input channels at the timestamp.
        /// </summary>
        public override Slice GetSlice(int[] cnlNums, DateTime timestamp)
        {
            return new Slice(timestamp, cnlNums, new CnlData[cnlNums.Length]);
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override bool ProcessData(ICurrentData curData)
        {
            if (options.WritingMode == WritingMode.Auto && nextWriteTime <= curData.Timestamp)
            {
                nextWriteTime = GetNextWriteTime(curData.Timestamp, writingPeriod);
                TrendTable trendTable = GetCurrentTrendTable(curData.Timestamp);
                InitCnlIndices(curData, ref cnlIndices);
                CopyCnlData(curData, slice, cnlIndices);
                adapter.WriteSlice(trendTable, slice);
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
            updatedTable = GetTrendTable(timestamp);
        }

        /// <summary>
        /// Completes the update operation.
        /// </summary>
        public override void EndUpdate(int deviceNum, DateTime timestamp)
        {
            updatedTable = null;
        }

        /// <summary>
        /// Writes the input channel data.
        /// </summary>
        public override void WriteCnlData(int cnlNum, DateTime timestamp, CnlData cnlData)
        {
            TrendTable trendTable = GetTrendTable(timestamp);
            adapter.WriteCnlData(trendTable, cnlNum, timestamp, cnlData);
        }
    }
}
