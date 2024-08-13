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
        /// Initializes a new instance of the class.
        /// </summary>
        public MimicPacket(long mimicKey, Mimic mimic)
            : base(mimicKey)
        {
            ArgumentNullException.ThrowIfNull(mimic, nameof(mimic));
            Dependencies = mimic.Dependencies;
            Document = mimic.Document;
        }


        /// <summary>
        /// Gets the mimic dependencies.
        /// </summary>
        public List<FaceplateMeta> Dependencies { get; }

        /// <summary>
        /// Gets or sets the mimic document.
        /// </summary>
        public ExpandoObject Document { get; }
    }
}
