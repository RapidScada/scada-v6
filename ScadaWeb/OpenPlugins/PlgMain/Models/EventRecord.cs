// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a record containing an event.
    /// <para>Представляет запись, содержащую событие.</para>
    /// </summary>
    public struct EventRecord
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        public Event E { get; set; }

        /// <summary>
        /// Gets or sets the formatted event.
        /// </summary>
        public EventFormatted Ef { get; set; }

        /// <summary>
        /// Gets or sets the event time.
        /// </summary>
        public TimeRecord T { get; set; }

        /// <summary>
        /// Gets or sets the event ID as string.
        /// </summary>
        /// <remarks>String is used instead of long because JavaScript unable to decode long.</remarks>
        public string Id { get; set; }
    }
}
