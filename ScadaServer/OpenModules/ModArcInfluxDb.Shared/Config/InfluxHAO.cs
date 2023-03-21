// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Server.Archives;

namespace Scada.Server.Modules.ModArcInfluxDb.Config
{
    /// <summary>
    /// Represents options of a historical data archive.
    /// <para>Представляет параметры архива исторических данных.</para>
    /// </summary>
    internal class InfluxHAO : HistoricalArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public InfluxHAO(OptionList options)
            : base(options)
        {
            Connection = options.GetValueAsString("Connection");
            BatchSize = options.GetValueAsInt("BatchSize", 1000);
            FlushInterval = options.GetValueAsInt("FlushInterval", 1000);
        }


        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Gets or sets the number of data points to write as a batch.
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the time interval before a batch is written, ms.
        /// </summary>
        public int FlushInterval { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public override void AddToOptionList(OptionList options)
        {
            base.AddToOptionList(options);
            options["Connection"] = Connection;
            options["BatchSize"] = BatchSize.ToString();
            options["FlushInterval"] = FlushInterval.ToString();
        }
    }
}
