// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a package containing events.
    /// <para>Представляет пакет, содержащий события.</para>
    /// </summary>
    public class EventPacket
    {
        /// <summary>
        /// Gets or sets the event records.
        /// </summary>
        public IEnumerable<EventRecord> Records { get; set; }

        /// <summary>
        /// Gets or sets the ID of the event filter on the server.
        /// </summary>
        /// <remarks>String is used instead of long because JavaScript unable to decode long.</remarks>
        public string FilterID { get; set; }
    }
}
