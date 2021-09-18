// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a record containing current channel data.
    /// <para>Представляет запись, содержащую текущие данные канала.</para>
    /// </summary>
    public struct CurDataRecord
    {
        /// <summary>
        /// Gets or sets the numeric channel data.
        /// </summary>
        public CurDataPoint D { get; set; }

        /// <summary>
        /// Gets or sets the formatted channel data.
        /// </summary>
        public CnlDataFormatted Df { get; set; }
    }
}
