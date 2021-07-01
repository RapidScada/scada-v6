// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a package containing current data without formatting.
    /// <para>Представляет пакет, содержащий текущие данные без форматирования.</para>
    /// </summary>
    public class CurDataLight
    {
        /// <summary>
        /// Gets or sets the data points.
        /// </summary>
        public List<CurDataPoint> Points { get; set; }

        /// <summary>
        /// Gets or sets the ID of the cached channel list on the server.
        /// </summary>
        public int CnlListID { get; set; }
    }
}
