// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Server.Modules.ModDiffCalculator.Config;

namespace Scada.Server.Modules.ModDiffCalculator.Logic
{
    /// <summary>
    /// Represents a group of channels.
    /// <para>Представляет группу каналов.</para>
    /// </summary>
    internal class ChannelGroup
    {
        private readonly double period; // period in seconds
        private readonly double offset; // offset in seconds
        private readonly double delay;  // delay in seconds


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
                PeriodType.Month => TimeSpan.FromDays(30).TotalSeconds, // !!!
                _ => groupConfig.CustomPeriod.TotalSeconds // PeriodType.Custom
            };
            offset = groupConfig.Offset.TotalSeconds;
            delay = groupConfig.Delay;

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
        /// Checks if the specified time is the time to calculate differences.
        /// </summary>
        public bool IsTimeToCalculate(DateTime utcNow, out DateTime timestamp1, out DateTime timestamp2)
        {
            timestamp1 = DateTime.MinValue;
            timestamp2 = DateTime.MinValue;
            return false;
        }
    }
}
