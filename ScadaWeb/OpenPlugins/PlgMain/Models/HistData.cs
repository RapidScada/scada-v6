// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Api;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a package containing formatted historical data.
    /// <para>Представляет пакет, содержащий отформатированные исторические данные.</para>
    /// </summary>
    public class HistData
    {
        /// <summary>
        /// Gets or sets the data records.
        /// </summary>
        public List<HistDataRecord> Records { get; set; }
    }
}
