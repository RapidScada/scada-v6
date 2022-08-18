// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Server.Archives;

namespace Scada.Server.Modules.ModArcBasic.Config
{
    /// <summary>
    /// Represents options of a historical data archive.
    /// <para>Представляет параметры архива исторических данных.</para>
    /// </summary>
    public class BasicHAO : HistoricalArchiveOptions2
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicHAO(OptionList options)
            : base(options)
        {
            UseCopyDir = options.GetValueAsBool("UseCopyDir");
        }

        /// <summary>
        /// Gets or sets a value indicating whether to store data in the archive copy directory.
        /// </summary>
        public bool UseCopyDir { get; set; }

        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public override void AddToOptionList(OptionList options)
        {
            base.AddToOptionList(options);
            options["UseCopyDir"] = UseCopyDir.ToLowerString();
        }
    }
}
