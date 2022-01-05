// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;

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
        public TesterOptions(OptionList options)
        {
            ReadMode = options.GetValueAsEnum<ReadMode>("ReadMode");
            BufferLength = options.GetValueAsInt("BufferLength", 100);
            BinStopCode = (byte)options.GetValueAsInt("BinStopCode");
            StopEnding = options.GetValueAsString("StopEnding");
        }


        /// <summary>
        /// Gets or sets the reading mode.
        /// </summary>
        public ReadMode ReadMode { get; set; }

        /// <summary>
        /// Gets or sets the length of the buffer length for binary data.
        /// </summary>
        public int BufferLength { get; set; }

        /// <summary>
        /// Gets or sets the stop code for reading binary data. If zero, no stop condition is used.
        /// </summary>
        public byte BinStopCode { get; set; }

        /// <summary>
        /// Gets or sets the line end to stop reading text. If empty, one line is read.
        /// </summary>
        public string StopEnding { get; set; }
    }
}
