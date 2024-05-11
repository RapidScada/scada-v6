// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a font available a mimic diagram.
    /// <para>Представляет шрифт, доступный на мнемосхеме.</para>
    /// </summary>
    public class Font
    {
        /// <summary>
        /// Gets or sets the family name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the stylesheet URL.
        /// </summary>
        public string Url { get; set; } = "";
    }
}
