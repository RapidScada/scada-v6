// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;
using System.Reflection.Metadata;

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

            int currentIndex = 0;
            Components = [];
            EndOfComponents = true;

            foreach (Component component in mimic.EnumerateComponents())
            {
                if (currentIndex++ >= index)
                {
                    if (Components.Count < count)
                        Components.Add(component);

                    if (Components.Count >= count)
                    {
                        EndOfComponents = false;
                        break;
                    }
                }
            }
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
