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
    internal class BasicHAO : HistoricalArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicHAO(OptionList options)
            : base(options)
        {
            ReadOnly = false;
            IsPeriodic = true;
            UseCopyDir = options.GetValueAsBool("UseCopyDir");
            MaxQueueSize = options.GetValueAsInt("MaxQueueSize", 1000);
        }


        /// <summary>
        /// Gets or sets a value indicating whether to store data in the archive copy directory.
        /// </summary>
        public bool UseCopyDir { get; set; }

        /// <summary>
        /// Gets or sets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public override void AddToOptionList(OptionList options)
        {
            base.AddToOptionList(options);
            options["UseCopyDir"] = UseCopyDir.ToLowerString();
            options["MaxQueueSize"] = MaxQueueSize.ToString();
        }
    }
}
