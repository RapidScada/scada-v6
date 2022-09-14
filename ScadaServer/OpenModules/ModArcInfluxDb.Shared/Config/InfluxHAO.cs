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
        }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public override void AddToOptionList(OptionList options)
        {
            base.AddToOptionList(options);
            options["Connection"] = Connection;
        }
    }
}
