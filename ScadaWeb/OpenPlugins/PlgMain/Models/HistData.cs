// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a packet containing historical data.
    /// <para>Представляет пакет, содержащий исторические данные.</para>
    /// </summary>
    public class HistData
    {
        /// <summary>
        /// Represents a list of historical records.
        /// </summary>
        public class RecordList : List<HistDataRecord>
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public RecordList(int capacity)
                : base(capacity)
            {
            }
        }


        /// <summary>
        /// Gets or sets the channel numbers.
        /// </summary>
        public IEnumerable<int> CnlNums { get; set; }

        /// <summary>
        /// Gets or sets the ordered timestamps common for all trends.
        /// </summary>
        public IEnumerable<TimeRecord> Timestamps { get; set; }

        /// <summary>
        /// Gets or sets the trends having the same number of points.
        /// </summary>
        public IEnumerable<RecordList> Trends { get; set; }
    }
}
