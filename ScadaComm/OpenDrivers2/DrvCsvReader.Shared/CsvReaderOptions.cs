// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvCsvReader
{
    /// <summary>
    /// Represents CSV reader options.
    /// <para>Представляет параметры считывателя CSV.</para>
    /// </summary>
    internal class CsvReaderOptions
    {
        [Description("The number of device tags.")]
        public int TagCount { get; set; } = 1;

        [Description("The name of the CSV file containing data to read.")]
        public string FileName { get; set; }

        [Description("The string used as the decimal separator in numeric values.")]
        public string DecimalSeparator { get; set; } = ".";

        [Description("The delimiter used to separate fields.")]
        public string FieldDelimiter { get; set; } = ",";

        [Description("The reading mode. " + 
            "In RealTime mode, the driver reads data according to the current time. " +
            "In Demo mode, the driver reads data cyclically.")]
        public ReadMode ReadMode { get; set; } = ReadMode.RealTime;

        [Description("The data period defining the reading loop in demo mode.")]
        public DemoPeriod DemoPeriod { get; set; } = DemoPeriod.OneHour;
    }
}
