// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents an image of a mimic diagram.
    /// <para>Представляет изображение мнемосхемы.</para>
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or sets the image name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        public byte[] Data { get; set; } = null;
    }
}
