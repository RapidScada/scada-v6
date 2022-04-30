// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using System;

namespace Scada.Web.Plugins.PlgChart
{
    /// <summary>
    /// Represents chart data builder options.
    /// <para>Представляет параметры построителя данных графика.</para>
    /// </summary>
    public class ChartDataBuilderOptions
    {
        /// <summary>
        /// Gets or sets the channel numbers.
        /// </summary>
        public int[] CnlNums { get; set; }

        /// <summary>
        /// Gets or sets the chart time range.
        /// </summary>
        public TimeRange TimeRange { get; set; }

        /// <summary>
        /// Gets or sets the bit number of the chart data archive.
        /// </summary>
        public int ArchiveBit { get; set; }

        /// <summary>
        /// Gets or sets the user's time zone.
        /// </summary>
        public TimeZoneInfo TimeZone { get; set; }


        /// <summary>
        /// Validates the options.
        /// </summary>
        public void Validate()
        {
            if (CnlNums == null)
                throw new ScadaException("CnlNums must not be null.");

            if (TimeZone == null)
                throw new ScadaException("TimeZone must not be null.");
        }
    }
}
