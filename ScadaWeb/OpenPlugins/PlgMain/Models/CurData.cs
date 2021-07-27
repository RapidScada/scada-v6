// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a packet containing current data.
    /// <para>Представляет пакет, содержащий текущие данные.</para>
    /// </summary>
    public class CurData
    {
        /// <summary>
        /// Gets or sets the server current time.
        /// </summary>
        public TimeRecord ServerTime { get; set; }

        /// <summary>
        /// Gets or sets the data records.
        /// </summary>
        public IEnumerable<CurDataRecord> Records { get; set; }

        /// <summary>
        /// Gets or sets the ID of the cached channel list on the server.
        /// </summary>
        /// <remarks>String is used instead of long because JavaScript unable to decode long.</remarks>
        public string CnlListID { get; set; }
    }
}
