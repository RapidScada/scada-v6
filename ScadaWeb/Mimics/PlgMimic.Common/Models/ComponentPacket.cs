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
        /// Initializes a new instance of the class.
        /// </summary>
        public ComponentPacket(long mimicKey, Mimic mimic, int index, int count)
            : base(mimicKey)
        {
            ArgumentNullException.ThrowIfNull(mimic, nameof(mimic));
            Components = mimic.EnumerateComponents().Skip(index).Take(count).ToList();
            EndOfComponents = Components.Count < count;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ComponentPacket(long mimicKey, Mimic mimic, int index, int count,
            Func<Component, bool> checkRightsFunc) : base(mimicKey)
        {
            ArgumentNullException.ThrowIfNull(mimic, nameof(mimic));
            ArgumentNullException.ThrowIfNull(checkRightsFunc, nameof(checkRightsFunc));

            Components = mimic.EnumerateComponents(checkRightsFunc).Skip(index).Take(count).ToList();
            EndOfComponents = Components.Count < count;
        }


        /// <summary>
        /// Gets a value indicating whether all components have been read.
        /// </summary>
        public bool EndOfComponents { get; }

        /// <summary>
        /// Gets the components.
        /// </summary>
        public List<Component> Components { get; }
    }
}
