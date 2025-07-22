/*
 * Copyright 2025 Rapid Software LLC
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
 * Summary  : Provides helper methods for archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2025
 * Modified : 2025
 */

using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Lang;
using System;
using System.Collections.Generic;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Provides helper methods for archive logic.
    /// <para>Предоставляет вспомогательные методы для логики архивов.</para>
    /// </summary>
    public static class ArchiveUtils
    {
        /// <summary>
        /// Gets the channel indexes in the specified array of channel numbers.
        /// </summary>
        public static Dictionary<int, int> GetCnlIndexes(int[] cnlNums)
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
        /// Initializes the indexes that map the archive channels to all channels.
        /// </summary>
        public static void InitCnlIndexes(ICurrentData curData, int[] cnlNums, ref int[] cnlIndexes)
        {
            if (cnlIndexes == null)
            {
                int cnlCnt = cnlNums.Length;
                cnlIndexes = new int[cnlCnt];

                for (int i = 0; i < cnlCnt; i++)
                {
                    cnlIndexes[i] = curData.GetCnlIndex(cnlNums[i]);
                }
            }
        }

        /// <summary>
        /// Initializes the previous channel data.
        /// </summary>
        public static void InitPrevCnlData(ICurrentData curData, int[] cnlNums, int[] cnlIndexes,
            ref CnlData[] prevCnlData)
        {
            if (prevCnlData == null)
            {
                int cnlCnt = cnlNums.Length;
                prevCnlData = new CnlData[cnlCnt];

                for (int i = 0; i < cnlCnt; i++)
                {
                    prevCnlData[i] = curData.CnlData[cnlIndexes[i]];
                }
            }
        }

        /// <summary>
        /// Copies the current data to the target slice that contains the archive channels.
        /// </summary>
        public static void CopyCnlData(ICurrentData curData, Slice targetSlice, int[] cnlNums, int[] cnlIndexes)
        {
            if (targetSlice.CnlNums == cnlNums)
            {
                for (int i = 0, cnlCnt = cnlNums.Length; i < cnlCnt; i++)
                {
                    targetSlice.CnlData[i] = curData.CnlData[cnlIndexes[i]];
                }
            }
            else
            {
                throw new ScadaException("Inappropriate target slice.");
            }
        }

        /// <summary>
        /// Gets the archive status, including queue statistics.
        /// </summary>
        public static string GetStatusText(bool isReady, QueueStats queueStats, int? queueSize)
        {
            if (!isReady)
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
        /// Converts the specified value to a TimeSpan according to its unit.
        /// It is assumed that one month consists of 31 days.
        /// </summary>
        public static TimeSpan ConvertToTimeSpan(int value, TimeUnit timeUnit)
        {
            switch (timeUnit)
            {
                case TimeUnit.Minute:
                    return TimeSpan.FromMinutes(value);
                case TimeUnit.Hour:
                    return TimeSpan.FromHours(value);
                case TimeUnit.Day:
                    return TimeSpan.FromDays(value);
                case TimeUnit.Month:
                    return TimeSpan.FromDays(31);
                default: // TimeUnit.Second
                    return TimeSpan.FromSeconds(value);
            }
        }

        /// <summary>
        /// Adds the specified value to the timestamp, taking the unit into account.
        /// </summary>
        public static DateTime AddValue(this DateTime timestamp, int value, TimeUnit timeUnit)
        {
            switch (timeUnit)
            {
                case TimeUnit.Minute:
                    return timestamp.AddMinutes(value);
                case TimeUnit.Hour:
                    return timestamp.AddHours(value);
                case TimeUnit.Day:
                    return timestamp.AddDays(value);
                case TimeUnit.Month:
                    return timestamp.AddMonths(value);
                default: // TimeUnit.Second
                    return timestamp.AddSeconds(value);
            }
        }

        /// <summary>
        /// Adds the writing period to the specified timestamp.
        /// </summary>
        public static DateTime AddWritingPeriod(this DateTime timestamp, HistoricalArchiveOptions options)
        {
            return timestamp.AddValue(options.WritingPeriod, options.WritingPeriodUnit);
        }

        /// <summary>
        /// Adds the writing offset to the specified timestamp.
        /// </summary>
        public static DateTime AddWritingOffset(this DateTime timestamp, HistoricalArchiveOptions options)
        {
            return timestamp.AddValue(options.WritingOffset, options.WritingOffsetUnit);
        }

        /// <summary>
        /// Adjusts the timestamp for daylight saving time, if specified by the options.
        /// </summary>
        public static DateTime AdjustForDst(this DateTime timestamp, HistoricalArchiveOptions options)
        {
            return options.AdjustForDst && TimeZoneInfo.Local.IsDaylightSavingTime(timestamp)
                ? timestamp.AddHours(1)
                : timestamp;
        }

        /// <summary>
        /// Gets the closest time to write data to the archive, less than or equal to the specified timestamp.
        /// </summary>
        public static DateTime GetClosestWriteTime(DateTime timestamp, TimeSpan period, TimeSpan offset)
        {
            if (period <= TimeSpan.Zero)
                return timestamp;

            if (offset < TimeSpan.Zero)
                offset = TimeSpan.Zero;
            else if (offset >= period)
                offset = TimeSpan.FromTicks(offset.Ticks % period.Ticks);

            DateTime startDate = period.TotalDays <= 1
                ? timestamp.Date
                : new DateTime(timestamp.Year, 1, 1, 0, 0, 0, timestamp.Kind);
            TimeSpan timeSpan = timestamp - startDate;
            return timeSpan < offset
                ? startDate.Add(-period).Add(offset)
                : startDate.AddTicks((timeSpan - offset).Ticks / period.Ticks * period.Ticks + offset.Ticks);
        }

        /// <summary>
        /// Gets the closest time to write data to the archive, less than or equal to the specified timestamp.
        /// </summary>
        public static DateTime GetClosestWriteTime(DateTime timestamp, HistoricalArchiveOptions options)
        {
            if (options.WritingPeriod <= 0)
                return timestamp;

            if (options.WritingPeriodUnit == TimeUnit.Month)
            {
                DateTime startTime = new DateTime(timestamp.Year, 1, 1, 0, 0, 0, timestamp.Kind)
                    .AddWritingOffset(options);

                if (timestamp < startTime)
                {
                    return startTime.AddMonths(-options.WritingPeriod);
                }
                else
                {
                    DateTime writeTime = startTime;
                    DateTime prevWriteTime = startTime;

                    while (writeTime < timestamp)
                    {
                        prevWriteTime = writeTime;
                        writeTime = writeTime.AddMonths(options.WritingPeriod);
                    }

                    return writeTime <= timestamp ? writeTime : prevWriteTime;
                }
            }
            else
            {
                return GetClosestWriteTime(
                    timestamp,
                    ConvertToTimeSpan(options.WritingPeriod, options.WritingPeriodUnit),
                    ConvertToTimeSpan(options.WritingOffset, options.WritingOffsetUnit));
            }
        }

        /// <summary>
        /// Gets the next time to write data to the archive, greater than or equal to the specified timestamp.
        /// </summary>
        public static DateTime GetNextWriteTime(DateTime timestamp, TimeSpan period, TimeSpan offset)
        {
            return period > TimeSpan.Zero
                ? GetClosestWriteTime(timestamp, period, offset).Add(period)
                : timestamp;
        }

        /// <summary>
        /// Gets the next time to write data to the archive, greater than or equal to the specified timestamp.
        /// </summary>
        public static DateTime GetNextWriteTime(DateTime timestamp, HistoricalArchiveOptions options)
        {
            return options.WritingPeriod > 0
                ? GetClosestWriteTime(timestamp, options).AddWritingPeriod(options)
                : timestamp;
        }

        /// <summary>
        /// Checks that the timestamp is a multiple of the period.
        /// </summary>
        public static bool TimeIsMultipleOfPeriod(DateTime timestamp, TimeSpan period, TimeSpan offset)
        {
            if (period <= TimeSpan.Zero)
                return false;

            if (period.TotalDays <= 1)
            {
                return (int)timestamp.TimeOfDay.TotalMilliseconds % (int)period.TotalMilliseconds ==
                    (int)offset.TotalMilliseconds;
            }

            DateTime startDate = new DateTime(timestamp.Year, 1, 1, 0, 0, 0, timestamp.Kind);
            return (int)(timestamp - startDate).TotalSeconds % (int)period.TotalSeconds == (int)offset.TotalSeconds;
        }

        /// <summary>
        /// Pulls a timestamp to the closest periodic timestamp within the specified range.
        /// </summary>
        public static bool PullTimeToPeriod(ref DateTime timestamp, TimeSpan period, TimeSpan offset,
            TimeSpan pullingRange)
        {
            DateTime closestTime = GetClosestWriteTime(timestamp, period, offset);

            if (timestamp - closestTime <= pullingRange)
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
        /// Returns an enumerable collection of dates in the specified time interval.
        /// </summary>
        public static IEnumerable<DateTime> EnumerateDates(TimeRange timeRange)
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
    }
}
