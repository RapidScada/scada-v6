// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Server.Modules.ModDiffCalculator.Config;

namespace Scada.Server.Modules.ModDiffCalculator.Logic
{
    /// <summary>
    /// Represents a group of channels.
    /// <para>Представляет группу каналов.</para>
    /// </summary>
    internal class ChannelGroup
    {
        private readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(1);
        private const int CacheCapacity = 100;

        private readonly double period;   // period in seconds, except month
        private readonly double offset;   // offset in seconds
        private readonly double delay;    // delay in seconds
        private readonly MemoryCache<DateTime, Slice> cache; // the thread-safe group cache

        private DateTime time1;           // the previous calculation time, UTC
        private DateTime time2;           // the upcoming calculation time, UTC
        private DateTime delayedCalcTime; // the upcoming calculation time with the delay, UTC


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelGroup(GroupConfig groupConfig)
        {
            period = groupConfig.PeriodType switch
            {
                PeriodType.Minute => 60,
                PeriodType.Hour => 3600,
                PeriodType.Day => TimeSpan.FromDays(1).TotalSeconds,
                PeriodType.Month => TimeSpan.FromDays(31).TotalSeconds,
                _ => groupConfig.CustomPeriod.TotalSeconds // PeriodType.Custom
            };
            offset = groupConfig.Offset.TotalSeconds;
            delay = groupConfig.Delay;
            cache = new MemoryCache<DateTime, Slice>(CacheExpiration, CacheCapacity);

            time1 = DateTime.MinValue;
            time2 = DateTime.MinValue;
            delayedCalcTime = DateTime.MinValue;

            GroupConfig = groupConfig ?? throw new ArgumentNullException(nameof(groupConfig));
            SrcCnlNums = groupConfig.Items.Select(i => i.SrcCnlNum).ToArray();
            DestCnlNums = groupConfig.Items.Select(i => i.DestCnlNum).ToArray();
        }


        /// <summary>
        /// Gets a value indicating whether a monthly period is used.
        /// </summary>
        private bool PeriodIsMonth => GroupConfig.PeriodType == PeriodType.Month;

        /// <summary>
        /// Gets a value indicating whether the period is affected by the AdjustForDst option.
        /// </summary>
        private bool PeriodIsAdjustable => 
            GroupConfig.PeriodType == PeriodType.Day || GroupConfig.PeriodType == PeriodType.Month;


        /// <summary>
        /// Gets the group configuration.
        /// </summary>
        public GroupConfig GroupConfig { get; }

        /// <summary>
        /// Gets the source channel numbers.
        /// </summary>
        public int[] SrcCnlNums { get; }

        /// <summary>
        /// Gets the destination channel numbers corresponding to the source channels.
        /// </summary>
        public int[] DestCnlNums { get; }

        /// <summary>
        /// Gets the previous calculation time.
        /// </summary>
        public DateTime PrevCalcTime => time1;


        /// <summary>
        /// Initializes timing calculation.
        /// </summary>
        public void InitTime(DateTime utcNow)
        {
            time1 = GetCalculationTime(utcNow);
            time2 = GetNextCalculationTime(time1);
            delayedCalcTime = AdjustForDst(time2.AddSeconds(delay));
        }

        /// <summary>
        /// Checks if the specified time is the time to calculate differences.
        /// </summary>
        public bool IsTimeToCalculate(DateTime utcNow, out DateTime timestamp1, out DateTime timestamp2)
        {
            if (delayedCalcTime <= utcNow)
            {
                timestamp1 = AdjustForDst(time1);
                timestamp2 = AdjustForDst(time2);
                time1 = time2;
                time2 = GetNextCalculationTime(time2);
                delayedCalcTime = AdjustForDst(time2.AddSeconds(delay));
                return true;
            }
            else
            {
                timestamp1 = DateTime.MinValue;
                timestamp2 = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Gets a calculation time before the start time.
        /// </summary>
        public DateTime GetCalculationTime(DateTime startTime)
        {
            if (PeriodIsMonth)
            {
                DateTime calcTime = new DateTime(startTime.Year, startTime.Month, 1, 0, 0, 0, startTime.Kind)
                    .AddSeconds(offset);
                return calcTime < startTime ? calcTime : calcTime.AddMonths(-1);
            }
            else
            {
                DateTime yearStart = new(startTime.Year, 1, 1, 0, 0, 0, startTime.Kind);
                double interval = (startTime - yearStart).TotalSeconds + offset;
                DateTime calcTime = yearStart.AddSeconds(Math.Truncate(interval / period) * period + offset);
                return calcTime < startTime ? calcTime : calcTime.AddSeconds(-period);
            }
        }

        /// <summary>
        /// Gets the next calculation time, relative to the specified timestamp.
        /// </summary>
        public DateTime GetNextCalculationTime(DateTime timestamp)
        {
            return PeriodIsMonth
                ? timestamp.AddMonths(1)
                : timestamp.AddSeconds(period);
        }

        /// <summary>
        /// Adjusts the specified timestamp for daylight saving time, if specified by the group configuration.
        /// </summary>
        public DateTime AdjustForDst(DateTime timestamp)
        {
            // check summer time for local time zone
            return GroupConfig.AdjustForDst && PeriodIsAdjustable && 
                timestamp > DateTime.MinValue && timestamp.ToLocalTime().IsDaylightSavingTime()
                ? timestamp.AddHours(1)
                : timestamp;
        }

        /// <summary>
        /// Gets the historical slice from the group cache or from the server.
        /// </summary>
        public Slice GetSlice(IServerContext serverContext, DateTime timestamp)
        {
            return cache.GetOrCreate(timestamp, () =>
            {
                return serverContext.GetSlice(GroupConfig.ArchiveBit, timestamp, SrcCnlNums);
            });
        }
    }
}
