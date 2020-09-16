/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaData
 * Summary  : Represents a trend table page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a trend table page.
    /// <para>Представляет страницу таблицы трендов.</para>
    /// </summary>
    public class TrendTablePage
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendTablePage(TrendTable trendTable)
        {
            IsReady = false;
            TrendTable = trendTable ?? throw new ArgumentNullException("trendTable");
            PageNumber = 0;
            MinTimestamp = DateTime.MinValue;
            MaxTimestamp = DateTime.MinValue;
            WritingPeriod = 0;
            PageCapacity = 0;
            CnlNumList = null;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the page is ready for reading and writing.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets the parent trend table that the page belongs to.
        /// </summary>
        public TrendTable TrendTable { get; }

        /// <summary>
        /// Gets or sets the page number in the table.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the minumum timestamp of the page data.
        /// </summary>
        public DateTime MinTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the maximum timestamp of the page data.
        /// </summary>
        public DateTime MaxTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the writing period in seconds.
        /// </summary>
        public int WritingPeriod { get; set; }

        /// <summary>
        /// Gets or sets the page capacity.
        /// </summary>
        public int PageCapacity { get; set; }

        /// <summary>
        /// Gets or sets the list of input channel numbers whose data is stored in the page.
        /// </summary>
        public CnlNumList CnlNumList { get; set; }
    }
}
