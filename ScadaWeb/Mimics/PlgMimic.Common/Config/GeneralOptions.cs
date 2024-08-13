// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Config
{
    /// <summary>
    /// Represents general options of mimic diagrams.
    /// <para>Представляет основные параметры мнемосхем.</para>
    /// </summary>
    public class GeneralOptions
    {
        /// <summary>
        /// Gets or sets the data refresh rate in milliseconds.
        /// </summary>
        public int RefreshRate { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the scale type.
        /// </summary>
        public ScaleType ScaleType { get; set; } = ScaleType.Numeric;

        /// <summary>
        /// Gets or sets the scale value.
        /// </summary>
        public double ScaleValue { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets a value indicating whether to remember last scheme scale.
        /// </summary>
        public bool RememberScale { get; set; } = true;
    }
}
