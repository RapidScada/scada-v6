// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;

namespace Scada.Web.Plugins.PlgScheme.Code
{
    /// <summary>
    /// Represents scheme options.
    /// <para>Представляет параметры схемы.</para>
    /// </summary>
    public class SchemeOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SchemeOptions(OptionList options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            ScaleType = options.GetValueAsEnum("ScaleType", ScaleType.Numeric);
            ScaleValue = options.GetValueAsDouble("ScaleValue", 100) / 100;
            RememberScale = options.GetValueAsBool("RememberScale", true);
        }


        /// <summary>
        /// Gets or sets the scale type.
        /// </summary>
        public ScaleType ScaleType { get; set; }

        /// <summary>
        /// Gets or sets the scale value.
        /// </summary>
        public double ScaleValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remember last scheme scale.
        /// </summary>
        public bool RememberScale { get; set; }
    }
}
