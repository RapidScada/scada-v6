// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimic.Models
{
    /// <summary>
    /// Represents a packet containing mimic components.
    /// <para>Представляет пакет, содержащий компоненты схемы.</para>
    /// </summary>
    public class ComponentPacket : PacketBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether all components have been read.
        /// </summary>
        public bool EndOfComponents { get; set; } = false;

        /// <summary>
        /// Gets the components.
        /// </summary>
        public List<Component> Components { get; init; }
    }
}
