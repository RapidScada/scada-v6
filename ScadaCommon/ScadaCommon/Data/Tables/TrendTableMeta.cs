/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaCommon
 * Summary  : Represents metadata about a trend table or page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents metadata about a trend table or page.
    /// <para>Представляет метаданные таблицы или страницы.</para>
    /// </summary>
    public class TrendTableMeta
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendTableMeta()
        {
            MinTimestamp = DateTime.MinValue;
            MaxTimestamp = DateTime.MinValue;
            WritingPeriod = 0;
            PageCapacity = 0;
        }


        /// <summary>
        /// Gets or sets the minumum timestamp.
        /// </summary>
        public DateTime MinTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the maximum timestamp.
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
        /// Determines whether two object instances are equal.
        /// </summary>
        public bool Equals(TrendTableMeta meta)
        {
            return meta == this ||
                meta != null &&
                meta.MinTimestamp == MinTimestamp &&
                meta.MaxTimestamp == MaxTimestamp &&
                meta.WritingPeriod == WritingPeriod &&
                meta.PageCapacity == PageCapacity;
        }
    }
}
