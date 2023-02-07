// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvTester
{
    /// <summary>
    /// Represents tester options.
    /// <para>Представляет параметры тестировщика.</para>
    /// </summary>
    internal class TesterOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TesterOptions()
            : this(new OptionList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TesterOptions(OptionList options)
        {
            ReadMode = options.GetValueAsEnum<ReadMode>("ReadMode");
            BufferLength = options.GetValueAsInt("BufferLength", 100);
            BinStopCode = (byte)options.GetValueAsInt("BinStopCode");
            StopEnding = options.GetValueAsString("StopEnding");
        }


        [Description("The reading mode.")]
        public ReadMode ReadMode { get; set; }

        [Description("The length of the input buffer for binary data.")]
        public int BufferLength { get; set; }

        [Description("The stop code for reading binary data. If zero, no stop condition is used.")]
        public byte BinStopCode { get; set; }

        [Description("The line end to stop reading text. If empty, one line is read.")]
        public string StopEnding { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["ReadMode"] = ReadMode.ToString();
            options["BufferLength"] = BufferLength.ToString();
            options["BinStopCode"] = BinStopCode.ToString();
            options["StopEnding"] = StopEnding;
        }
    }
}
