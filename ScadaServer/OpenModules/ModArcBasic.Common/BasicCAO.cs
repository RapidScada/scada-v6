// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Server.Archives;

namespace Scada.Server.Modules.ModArcBasic
{
    /// <summary>
    /// Represents options of a current data archive.
    /// <para>Представляет параметры архива текущих данных.</para>
    /// </summary>
    public class BasicCAO : CurrentArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicCAO(OptionList options)
            : base(options)
        {
            IsCopy = options.GetValueAsBool("IsCopy");
        }

        /// <summary>
        /// Gets or sets a value indicating whether the archive stores a copy of the data.
        /// </summary>
        public bool IsCopy { get; set; }
    }
}
