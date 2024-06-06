// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimic.Models
{
    /// <summary>
    /// Represents a packet containing mimic images.
    /// <para>Представляет пакет, содержащий изображения схемы.</para>
    /// </summary>
    public class ImagePacket : PacketBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether all images have been read.
        /// </summary>
        public bool EndOfImages { get; set; } = false;

        /// <summary>
        /// Gets the images.
        /// </summary>
        public List<Image> Images { get; init; }
    }
}
