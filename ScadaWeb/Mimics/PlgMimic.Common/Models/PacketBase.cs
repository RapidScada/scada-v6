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
        /// Gets or sets the stamp of the mimic that ensures data integrity.
        /// </summary>
        [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public long MimicStamp { get; set; } = 0;
    }
}
