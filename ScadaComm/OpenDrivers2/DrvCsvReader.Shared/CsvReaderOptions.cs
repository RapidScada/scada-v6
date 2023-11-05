// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvCsvReader
{
    /// <summary>
    /// Represents CSV reader options.
    /// <para>Представляет параметры считывателя CSV.</para>
    /// </summary>
    internal class CsvReaderOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CsvReaderOptions()
        {
            TagCount = 1;
            FileName = "";
            DecimalSeparator = ".";
            FieldDelimiter = ",";
            ReadMode = ReadMode.RealTime;
            DemoPeriod = DemoPeriod.OneHour;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CsvReaderOptions(OptionList options)
            : this()
        {
            TagCount = options.GetValueAsInt("TagCount", TagCount);
            FileName = options.GetValueAsString("FileName", FileName);
            DecimalSeparator = options.GetValueAsString("DecimalSeparator", DecimalSeparator);
            FieldDelimiter = options.GetValueAsString("FieldDelimiter", FieldDelimiter);
            ReadMode = options.GetValueAsEnum("ReadMode", ReadMode);
            DemoPeriod = options.GetValueAsEnum("DemoPeriod", DemoPeriod);
        }


        [Description("The number of device tags.")]
        public int TagCount { get; set; }

        [Description("The name of the CSV file containing data to read.")]
        public string FileName { get; set; }

        [Description("The string used as the decimal separator in numeric values.")]
        public string DecimalSeparator { get; set; }

        [Description("The delimiter used to separate fields.")]
        public string FieldDelimiter { get; set; }

        [Description("The reading mode. " + 
            "In RealTime mode, the driver reads data according to the current time. " +
            "In Demo mode, the driver reads data cyclically.")]
        public ReadMode ReadMode { get; set; }

        [Description("The data period defining the reading loop in demo mode.")]
        public DemoPeriod DemoPeriod { get; set; }
        
        
        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["TagCount"] = TagCount.ToString();
            options["FileName"] = FileName;
            options["DecimalSeparator"] = DecimalSeparator;
            options["FieldDelimiter"] = FieldDelimiter;
            options["ReadMode"] = ReadMode.ToString();
            options["DemoPeriod"] = DemoPeriod.ToString();
        }
    }
}
