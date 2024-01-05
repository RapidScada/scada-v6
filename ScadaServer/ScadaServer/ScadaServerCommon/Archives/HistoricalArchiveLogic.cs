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
 * Summary  : Represents the base class for historical data archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Models;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents the base class for historical data archive logic.
    /// <para>Представляет базовый класс логики архива исторических данных.</para>
    /// </summary>
    /// <remarks>Descendants of this class must be thread-safe.</remarks>
    public abstract class HistoricalArchiveLogic : ArchiveLogic
    {
        /// <summary>
        /// Represents a method that determines whether two CnlData instances are the same.
        /// </summary>
        protected delegate bool CnlDataEqualsDelegate(CnlData x, CnlData y);


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistoricalArchiveLogic(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums)
            : base(archiveContext, archiveConfig, cnlNums)
        {
            CurrentUpdateContext = null;
        }


        /// <summary>
        /// Gets the archive options.
        /// </summary>
        protected virtual HistoricalArchiveOptions ArchiveOptions => null;

        /// <summary>
        /// Gets or sets the context of the current update operation.
        /// </summary>
        public UpdateContext CurrentUpdateContext { get; set; }


        /// <summary>
        /// Gets the trends one by one and merges them.
        /// </summary>
        protected TrendBundle MergeTrends(TimeRange timeRange, int[] cnlNums)
        {
            int cnlCnt = cnlNums.Length;
            Trend[] trends = new Trend[cnlCnt];

            for (int i = 0; i < cnlCnt; i++)
            {
                trends[i] = GetTrend(timeRange, cnlNums[i]);
            }

            return TrendHelper.MergeTrends(trends);
        }

        /// <summary>
        /// Gets the recently updated channel data.
        /// </summary>
        protected bool GetRecentCnlData(object lockObj, DateTime timestamp, int cnlNum, out CnlData cnlData)
        {
            if (Monitor.TryEnter(lockObj))
            {
                try
                {
                    if (CurrentUpdateContext != null &&
                        CurrentUpdateContext.Timestamp == timestamp &&
                        CurrentUpdateContext.UpdatedData.TryGetValue(cnlNum, out cnlData))
                    {
                        return true;
                    }
                }
                finally
                {
                    Monitor.Exit(lockObj);
                }
            }

            cnlData = CnlData.Empty;
            return false;
        }

        /// <summary>
        /// Initializes the previous channel data.
        /// </summary>
        protected void InitPrevCnlData(ICurrentData curData, int[] cnlIndexes, ref CnlData[] prevCnlData)
        {
            if (ArchiveOptions != null && ArchiveOptions.WriteOnChange && 
                cnlIndexes != null && prevCnlData == null)
            {
                int cnlCnt = CnlNums.Length;
                prevCnlData = new CnlData[cnlCnt];

                for (int i = 0; i < cnlCnt; i++)
                {
                    prevCnlData[i] = curData.CnlData[cnlIndexes[i]];
                }
            }
        }

        /// <summary>
        /// Determines whether two CnlData instances are the same.
        /// </summary>
        protected bool CnlDataEquals1(CnlData x, CnlData y)
        {
            return x == y;
        }

        /// <summary>
        /// Determines whether two CnlData instances are considered the same, 
        /// comparing absolute values and taking into account a deadband.
        /// </summary>
        protected bool CnlDataEquals2(CnlData x, CnlData y)
        {
            return ArchiveOptions != null && x.Stat == y.Stat && (
                x.Val.Equals(y.Val) || 
                Math.Abs(x.Val - y.Val) <= ArchiveOptions.Deadband);
        }

        /// <summary>
        /// Determines whether two CnlData instances are considered the same,
        /// comparing the ratio of values and taking into account a deadband.
        /// </summary>
        protected bool CnlDataEquals3(CnlData x, CnlData y)
        {
            return ArchiveOptions != null && x.Stat == y.Stat && (
                x.Val.Equals(y.Val) ||
                x.Val != 0 && Math.Abs((x.Val - y.Val) / x.Val / 100) <= ArchiveOptions.Deadband);
        }

        /// <summary>
        /// Selects a method for comparing channel data depending on the archive options.
        /// </summary>
        protected CnlDataEqualsDelegate SelectCnlDataEquals()
        {
            if (ArchiveOptions == null)
                throw new InvalidOperationException("ArchiveOptions must not be null.");

            if (ArchiveOptions.Deadband <= 0)
                return CnlDataEquals1;
            else if (ArchiveOptions.DeadbandUnit == DeadbandUnit.Absolute)
                return CnlDataEquals2;
            else
                return CnlDataEquals3;
        }

        /// <summary>
        /// Checks that the timestamp is inside the retention period.
        /// </summary>
        protected bool TimeInsideRetention(DateTime timestamp, DateTime now)
        {
            return ArchiveOptions != null && now.AddDays(-ArchiveOptions.Retention) <= timestamp;
        }

        /// <summary>
        /// Gets the time period in seconds.
        /// </summary>
        protected static int GetPeriodInSec(int period, TimeUnit timeUnit)
        {
            switch (timeUnit)
            {
                case TimeUnit.Minute:
                    return period * 60;
                case TimeUnit.Hour:
                    return period * 3600;
                default: // TimeUnit.Second
                    return period;
            }
        }

        /// <summary>
        /// Checks that the timestamp is a multiple of the period.
        /// </summary>
        protected static bool TimeIsMultipleOfPeriod(DateTime timestamp, int period)
        {
            return period > 0 && (int)Math.Round(timestamp.TimeOfDay.TotalMilliseconds) % (period * 1000) == 0;
        }

        /// <summary>
        /// Pulls a timestamp to the closest periodic timestamp within the specified range.
        /// </summary>
        protected static bool PullTimeToPeriod(ref DateTime timestamp, int period, int pullingRange)
        {
            DateTime closestTime = GetClosestWriteTime(timestamp, period);

            if ((timestamp - closestTime).TotalSeconds <= pullingRange)
            {
                timestamp = closestTime;
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Gets the trends of the specified channels.
        /// </summary>
        public abstract TrendBundle GetTrends(TimeRange timeRange, int[] cnlNums);

        /// <summary>
        /// Gets the trend of the specified channel.
        /// </summary>
        public abstract Trend GetTrend(TimeRange timeRange, int cnlNum);

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public abstract List<DateTime> GetTimestamps(TimeRange timeRange);

        /// <summary>
        /// Gets the slice of the specified channels at the timestamp.
        /// </summary>
        public abstract Slice GetSlice(DateTime timestamp, int[] cnlNums);

        /// <summary>
        /// Gets the channel data.
        /// </summary>
        public abstract CnlData GetCnlData(DateTime timestamp, int cnlNum);

        /// <summary>
        /// Processes new data.
        /// </summary>
        /// <remarks>Returns true if the data has been written to the archive.</remarks>
        public abstract void ProcessData(ICurrentData curData);

        /// <summary>
        /// Accepts or rejects data with the specified timestamp.
        /// </summary>
        /// <remarks>The timestamp can be adjusted by the archive.</remarks>
        public abstract bool AcceptData(ref DateTime timestamp);

        /// <summary>
        /// Maintains performance when data is written one at a time.
        /// </summary>
        public abstract void BeginUpdate(UpdateContext updateContext);

        /// <summary>
        /// Updates the channel data.
        /// </summary>
        public abstract void UpdateData(UpdateContext updateContext, int cnlNum, CnlData cnlData);

        /// <summary>
        /// Completes the update operation.
        /// </summary>
        public abstract void EndUpdate(UpdateContext updateContext);
    }
}
