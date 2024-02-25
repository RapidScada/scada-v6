// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Modules.ModDiffCalculator.Config;

namespace Scada.Server.Modules.ModDiffCalculator.Logic
{
    /// <summary>
    /// Represents a group of channels.
    /// <para>Представляет группу каналов.</para>
    /// </summary>
    internal class ChannelGroup
    {
        private readonly bool periodIsMonth; // indicates that a monthly period is used
        private readonly double period;      // period in seconds, except month
        private readonly double offset;      // offset in seconds
        private readonly double delay;       // delay in seconds

        private DateTime time1;              // the previous calculation time
        private DateTime time2;              // the upcoming calculation time
        private DateTime delayedCalcTime;    // the upcoming calculation time with the delay


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelGroup(GroupConfig groupConfig)
        {
            periodIsMonth = groupConfig.PeriodType == PeriodType.Month;
            period = groupConfig.PeriodType switch
            {
                PeriodType.Minute => 60,
                PeriodType.Hour => 3600,
                PeriodType.Day => TimeSpan.FromDays(1).TotalSeconds,
                PeriodType.Month => 0,
                _ => groupConfig.CustomPeriod.TotalSeconds // PeriodType.Custom
            };
            offset = groupConfig.Offset.TotalSeconds;
            delay = groupConfig.Delay;

            time1 = DateTime.MinValue;
            time2 = DateTime.MinValue;
            delayedCalcTime = DateTime.MinValue;

            GroupConfig = groupConfig ?? throw new ArgumentNullException(nameof(groupConfig));
            SrcCnlNums = groupConfig.Items.Select(i => i.SrcCnlNum).ToArray();
            DestCnlNums = groupConfig.Items.Select(i => i.DestCnlNum).ToArray();
        }


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
        /// Initializes timing calculation.
        /// </summary>
        public void InitTime(DateTime utcNow)
        {
            time1 = GetCalculationTime(utcNow);
            time2 = GetNextCalculationTime(time1);
            delayedCalcTime = time2.AddSeconds(delay);
        }

        /// <summary>
        /// Checks if the specified time is the time to calculate differences.
        /// </summary>
        public bool IsTimeToCalculate(DateTime utcNow, out DateTime timestamp1, out DateTime timestamp2)
        {
            if (delayedCalcTime <= utcNow)
            {
                timestamp1 = time1;
                timestamp2 = time2;
                time1 = time2;
                time2 = GetNextCalculationTime(time2);
                delayedCalcTime = time2.AddSeconds(delay);
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
            if (periodIsMonth)
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
            return periodIsMonth 
                ? timestamp.AddMonths(1) 
                : timestamp.AddSeconds(period);
        }
    }
}
