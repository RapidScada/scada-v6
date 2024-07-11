// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Scada.Web.Plugins.PlgMimic.Models
{
    /// <summary>
    /// Represents a packet to transfer a mimic part.
    /// <para>Представляет пакет для передачи части мнемосхемы.</para>
    /// </summary>
    public abstract class PacketBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PacketBase()
        {
            MimicKey = 0;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PacketBase(long mimicKey)
        {
            MimicKey = mimicKey;
        }

        /// <summary>
        /// Gets the mimic key that ensures data integrity.
        /// </summary>
        [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public long MimicKey { get; init; }
    }
}
