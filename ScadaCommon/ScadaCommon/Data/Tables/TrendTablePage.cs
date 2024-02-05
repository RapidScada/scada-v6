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
 * Summary  : Represents a trend table page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
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
        public TrendTablePage(int pageNumber, TrendTable trendTable, TrendTableMeta meta)
        {
            IsReady = false;
            PageNumber = pageNumber;
            PageIndex = pageNumber - 1;
            TrendTable = trendTable ?? throw new ArgumentNullException(nameof(trendTable));
            Metadata = meta ?? throw new ArgumentNullException(nameof(meta));
            CnlNumList = null;
            Path = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether the page is ready for reading and writing.
        /// </summary>
        /// <remarks>This means that the object in memory and the files on disk match.</remarks>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets the page number in the table.
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the page index in the table.
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Gets the parent trend table that the page belongs to.
        /// </summary>
        public TrendTable TrendTable { get; }

        /// <summary>
        /// Gets the page metadata.
        /// </summary>
        public TrendTableMeta Metadata { get; }

        /// <summary>
        /// Gets or sets the list of channel numbers whose data is stored in the page.
        /// </summary>
        public CnlNumList CnlNumList { get; set; }

        /// <summary>
        /// Gets or sets the full path of the page file.
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// Gets the index of the specified channel.
        /// </summary>
        public bool GetCnlIndex(int cnlNum, out int cnlIndex)
        {
            if (CnlNumList == null)
            {
                cnlIndex = 0;
                return false;
            }
            else
            {
                return CnlNumList.CnlIndexes.TryGetValue(cnlNum, out cnlIndex);
            }
        }
    }
}
