// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;
using System.Dynamic;

namespace Scada.Web.Plugins.PlgMimic.Models
{
    /// <summary>
    /// Represents a packet containing an entire faceplate.
    /// <para>Представляет пакет, содержащий фейсплейт целиком.</para>
    /// </summary>
    public class FaceplatePacket : PacketBase
    {
        /// <summary>
        /// Gets or sets the faceplate document.
        /// </summary>
        public ExpandoObject Document { get; init; }

        /// <summary>
        /// Gets the components.
        /// </summary>
        public List<Component> Components { get; init; }

        /// <summary>
        /// Gets the images.
        /// </summary>
        public List<Image> Images { get; init; }
    }
}
