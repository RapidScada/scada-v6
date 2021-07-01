// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Api;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a package containing formatted current data.
    /// <para>Представляет пакет, содержащий форматированные текущие данные.</para>
    /// </summary>
    public class CurData
    {
        /// <summary>
        /// Gets or sets the data records.
        /// </summary>
        public List<CurDataRecord> Records { get; set; }

        /// <summary>
        /// Gets or sets the ID of the cached channel list on the server.
        /// </summary>
        public int CnlListID { get; set; }
    }
}
