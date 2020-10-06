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
 * Module   : ScadaServerCommon
 * Summary  : Represents the base class for archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents the base class for archive logic.
    /// <para>Представляет базовый класс логики архива.</para>
    /// </summary>
    public abstract class ArchiveLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected ArchiveLogic(ArchiveConfig archiveConfig, int[] cnlNums)
        {
            ArchiveConfig = archiveConfig ?? throw new ArgumentNullException("archiveConfig");
            CnlNums = cnlNums ?? throw new ArgumentNullException("cnlNums");
            IsReady = false;
            LastWriteTime = DateTime.MinValue;
            LastCleanupTime = DateTime.MinValue;
            CleanupPeriod = TimeSpan.FromDays(1);
        }


        /// <summary>
        /// Gets the archive configuration.
        /// </summary>
        protected ArchiveConfig ArchiveConfig { get; }

        /// <summary>
        /// Gets the input channel numbers processed by the archive.
        /// </summary>
        /// <remarks>Channel numbers are sorted and unique.</remarks>
        protected int[] CnlNums { get; }

        /// <summary>
        /// Gets the archive code.
        /// </summary>
        public string Code
        {
            get
            {
                return ArchiveConfig.Code;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the archive is ready for reading and writing.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets or sets the time (UTC) when the archive was last written to.
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets or sets the time (UTC) when the archive was last cleaned up.
        /// </summary>
        public DateTime LastCleanupTime { get; set; }

        /// <summary>
        /// Gets the required cleanup period for outdated data.
        /// </summary>
        public TimeSpan CleanupPeriod { get; protected set; }


        /// <summary>
        /// Gets the closest time to write data to the archive, less than or equal to the specified timestamp.
        /// </summary>
        protected DateTime GetClosestWriteTime(DateTime timestamp, int writingPeriod)
        {
            return writingPeriod > 0 ?
                timestamp.Date.AddSeconds((int)timestamp.TimeOfDay.TotalSeconds / writingPeriod * writingPeriod) :
                timestamp;
        }

        /// <summary>
        /// Gets the next time to write data to the archive, greater than or equal to the specified timestamp.
        /// </summary>
        protected DateTime GetNextWriteTime(DateTime timestamp, int writingPeriod)
        {
            return writingPeriod > 0 ?
                timestamp.Date.AddSeconds(((int)timestamp.TimeOfDay.TotalSeconds / writingPeriod + 1) * writingPeriod) :
                timestamp;
        }

        /// <summary>
        /// Initializes the indices that map the archive input channels to all channels.
        /// </summary>
        protected void InitCnlIndices(ICurrentData curData, ref int[] cnlIndices)
        {
            if (cnlIndices == null)
            {
                int cnlCnt = CnlNums.Length;
                cnlIndices = new int[cnlCnt];

                for (int i = 0; i < cnlCnt; i++)
                {
                    cnlIndices[i] = curData.GetCnlIndex(CnlNums[i]);
                }
            }
        }

        /// <summary>
        /// Copies the current data to the slice that contains the archive input channels.
        /// </summary>
        protected void CopyCnlData(ICurrentData curData, Slice slice, int[] cnlIndices)
        {
            if (slice.CnlNums == CnlNums)
            {
                slice.Timestamp = curData.Timestamp;

                for (int i = 0, cnlCnt = CnlNums.Length; i < cnlCnt; i++)
                {
                    slice.CnlData[i] = curData.CnlData[cnlIndices[i]];
                }
            }
            else
            {
                throw new ScadaException("Inappropriate slice.");
            }
        }

        /// <summary>
        /// Returns an enumerable collection of dates in the specified time interval.
        /// </summary>
        protected IEnumerable<DateTime> EnumerateDates(DateTime startTime, DateTime endTime, bool endInclusive)
        {
            if (endInclusive)
            {
                for (DateTime date = startTime.Date; date <= endTime; date = date.AddDays(1.0))
                {
                    yield return date;
                }
            }
            else
            {
                for (DateTime date = startTime.Date; date < endTime; date = date.AddDays(1.0))
                {
                    yield return date;
                }
            }
        }

        /// <summary>
        /// Acquires an exclusive lock on the archive.
        /// </summary>
        public virtual void Lock()
        {
            Monitor.Enter(this);
        }

        /// <summary>
        /// Releases an exclusive lock on the archive.
        /// </summary>
        public virtual void Unlock()
        {
            Monitor.Exit(this);
        }

        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public virtual void MakeReady()
        {
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public virtual void DeleteOutdatedData()
        {
        }

        /// <summary>
        /// Appends information about the archive to the string builder.
        /// </summary>
        public virtual void AppendInfo(StringBuilder sb)
        {
            sb.Append("[").Append(ArchiveConfig.Code).Append("] ").Append(ArchiveConfig.Name).Append(" : ");

            if (Locale.IsRussian)
                sb.Append(IsReady ? "готовность" : "не готов");
            else
                sb.Append(IsReady ? "Ready" : "Not Ready");

            sb.AppendLine();
        }
    }
}
