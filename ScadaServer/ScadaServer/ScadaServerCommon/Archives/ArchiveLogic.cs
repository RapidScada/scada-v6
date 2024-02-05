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
 * Module   : ScadaServerCommon
 * Summary  : Represents the base class for archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents the base class for archive logic.
    /// <para>Представляет базовый класс логики архива.</para>
    /// </summary>
    /// <remarks>Descendants of this class must be thread-safe.</remarks>
    public abstract class ArchiveLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected ArchiveLogic(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums)
        {
            ArchiveContext = archiveContext ?? throw new ArgumentNullException(nameof(archiveContext));
            ArchiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
            CnlNums = cnlNums ?? throw new ArgumentNullException(nameof(cnlNums));
            LastWriteTime = DateTime.UtcNow;
            Code = archiveConfig.Code;
            Title = ServerUtils.GetArchiveTitle(Code, archiveConfig.Name);
            IsReady = false;
            LastCleanupTime = DateTime.MinValue;
            CleanupPeriod = TimeSpan.FromDays(1);
        }


        /// <summary>
        /// Gets the archive context.
        /// </summary>
        protected IArchiveContext ArchiveContext { get; }

        /// <summary>
        /// Gets the archive configuration.
        /// </summary>
        protected ArchiveConfig ArchiveConfig { get; }

        /// <summary>
        /// Gets the channel numbers processed by the archive.
        /// </summary>
        /// <remarks>Channel numbers are sorted and unique.</remarks>
        protected int[] CnlNums { get; }

        /// <summary>
        /// Gets or sets the time (UTC) when the archive was last written to.
        /// </summary>
        protected DateTime LastWriteTime { get; set; }


        /// <summary>
        /// Gets the archive code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the archive title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the archive is ready for reading and writing.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets or sets the time (UTC) when the archive was last cleaned up.
        /// </summary>
        public DateTime LastCleanupTime { get; set; }

        /// <summary>
        /// Gets the required cleanup period for outdated data.
        /// </summary>
        public TimeSpan CleanupPeriod { get; protected set; }

        /// <summary>
        /// Gets the current archive status as text.
        /// </summary>
        public virtual string StatusText
        {
            get
            {
                return Locale.IsRussian ?
                    (IsReady ? "готов" : "не готов") :
                    (IsReady ? "Ready" : "Not Ready");
            }
        }


        /// <summary>
        /// Creates a log file of the archive.
        /// </summary>
        protected ILog CreateLog(string moduleCode)
        {
            return new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(ArchiveContext.AppDirs.LogDir, moduleCode + "_" + Code + ".log"),
                CapacityMB = ArchiveContext.AppConfig.GeneralOptions.MaxLogSize
            };
        }

        /// <summary>
        /// Initializes the indexes that map the archive channels to all channels.
        /// </summary>
        protected void InitCnlIndexes(ICurrentData curData, ref int[] cnlIndexes)
        {
            if (cnlIndexes == null)
            {
                int cnlCnt = CnlNums.Length;
                cnlIndexes = new int[cnlCnt];

                for (int i = 0; i < cnlCnt; i++)
                {
                    cnlIndexes[i] = curData.GetCnlIndex(CnlNums[i]);
                }
            }
        }

        /// <summary>
        /// Copies the current data to the slice that contains the archive channels.
        /// </summary>
        protected void CopyCnlData(ICurrentData curData, Slice slice, int[] cnlIndexes)
        {
            if (slice.CnlNums == CnlNums)
            {
                for (int i = 0, cnlCnt = CnlNums.Length; i < cnlCnt; i++)
                {
                    slice.CnlData[i] = curData.CnlData[cnlIndexes[i]];
                }
            }
            else
            {
                throw new ScadaException("Inappropriate slice.");
            }
        }

        /// <summary>
        /// Gets the archive status, including queue statistics.
        /// </summary>
        protected string GetStatusText(QueueStats queueStats, int? queueSize)
        {
            if (!IsReady)
            {
                return Locale.IsRussian ?
                    "не готов" :
                    "Not Ready";
            }
            else if (queueStats == null)
            {
                return Locale.IsRussian ?
                    "готов" :
                    "Ready";
            }
            else if (Locale.IsRussian)
            {
                return (queueStats.HasError ? "ошибка" : "готов") +
                    $", очередь {queueSize} из {queueStats.MaxQueueSize}, потеряно {queueStats.SkippedItems}";
            }
            else
            {
                return (queueStats.HasError ? "Error" : "Ready") +
                    $", queue {queueSize} of {queueStats.MaxQueueSize}, lost {queueStats.SkippedItems}";
            }
        }

        /// <summary>
        /// Gets the channel indexes in the specified array.
        /// </summary>
        protected static Dictionary<int, int> GetCnlIndexes(int[] cnlNums)
        {
            int cnlCnt = cnlNums.Length;
            Dictionary<int, int> indexes = new Dictionary<int, int>(cnlCnt);

            for (int i = 0; i < cnlCnt; i++)
            {
                indexes[cnlNums[i]] = i;
            }

            return indexes;
        }

        /// <summary>
        /// Gets the closest time to write data to the archive, less than or equal to the specified timestamp.
        /// </summary>
        protected static DateTime GetClosestWriteTime(DateTime timestamp, int period)
        {
            return period > 0 ?
                timestamp.Date.AddSeconds((int)timestamp.TimeOfDay.TotalSeconds / period * period) :
                timestamp;
        }

        /// <summary>
        /// Gets the next time to write data to the archive, greater than or equal to the specified timestamp.
        /// </summary>
        protected static DateTime GetNextWriteTime(DateTime timestamp, int period)
        {
            return period > 0 ?
                timestamp.Date.AddSeconds(((int)timestamp.TimeOfDay.TotalSeconds / period + 1) * period) :
                timestamp;
        }

        /// <summary>
        /// Returns an enumerable collection of dates in the specified time interval.
        /// </summary>
        protected static IEnumerable<DateTime> EnumerateDates(TimeRange timeRange)
        {
            if (timeRange.EndInclusive)
            {
                for (DateTime date = timeRange.StartTime.Date; date <= timeRange.EndTime; date = date.AddDays(1.0))
                {
                    yield return date;
                }
            }
            else
            {
                for (DateTime date = timeRange.StartTime.Date; date < timeRange.EndTime; date = date.AddDays(1.0))
                {
                    yield return date;
                }
            }
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public virtual void MakeReady()
        {
        }

        /// <summary>
        /// Closes the archive.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public virtual void DeleteOutdatedData()
        {
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        public virtual DateTime GetLastWriteTime()
        {
            return LastWriteTime;
        }
    }
}
