/*
 * Copyright 2020 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModArcInfluxDb
 * Summary  : Represents options of a historical data archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;
using Scada.Server.Archives;

namespace Scada.Server.Modules.ModArcInfluxDb.Logic.Config
{
    /// <summary>
    /// Represents options of a historical data archive.
    /// <para>Представляет параметры архива исторических данных.</para>
    /// </summary>
    internal class ArchiveOptions : HistoricalArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveOptions(OptionList options)
            : base(options)
        {
            Connection = options.GetValueAsString("Connection");
        }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Connection { get; set; }
    }
}
