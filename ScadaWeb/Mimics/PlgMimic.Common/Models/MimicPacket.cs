// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;
using System.Dynamic;

namespace Scada.Web.Plugins.PlgMimic.Models
{
    /// <summary>
    /// Represents a packet containing mimic properties.
    /// <para>Представляет пакет, содержащий свойства мнемосхемы.</para>
    /// </summary>
    public class MimicPacket : PacketBase
    {
        /// <summary>
        /// Gets the mimic dependencies.
        /// </summary>
        public List<FaceplateMeta> Dependencies { get; init; }

        /// <summary>
        /// Gets or sets the mimic document.
        /// </summary>
        public ExpandoObject Document { get; init; }
    }
}
