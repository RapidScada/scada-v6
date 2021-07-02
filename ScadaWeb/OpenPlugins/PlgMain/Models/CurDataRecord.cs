// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a record containing current data of an input channel.
    /// <para>Представляет запись, содержащую текущие данные входного канала.</para>
    /// </summary>
    public struct CurDataRecord
    {
        /// <summary>
        /// Gets or sets the numeric input channel data.
        /// </summary>
        public CurDataPoint Pt { get; set; }

        /// <summary>
        /// Gets or sets the formatted input channel data.
        /// </summary>
        public CnlDataFormatted Fd { get; set; }
    }
}
