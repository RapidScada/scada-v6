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
 * Summary  : Represents a mechanism to read and write trend tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read and write trend tables.
    /// <para>Представляет механизм для чтения и записи таблиц трендов.</para>
    /// </summary>
    public class TrendTableAdapter : Adapter
    {
        /// <summary>
        /// The major version number.
        /// </summary>
        protected const ushort MajorVersion = 1;
        /// <summary>
        /// The minor version number.
        /// </summary>
        protected const ushort MinorVersion = 0;
        /// <summary>
        /// The header size in a file.
        /// </summary>
        protected const int HeaderSize = 50;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendTableAdapter()
            : base()
        {
            ParentDirectory = null;
            ArchiveCode = "";
        }


        /// <summary>
        /// Hides the FileName property.
        /// </summary>
        private new string FileName { get; set; }

        /// <summary>
        /// Hides the Stream property.
        /// </summary>
        private new Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the parent directory of the table.
        /// </summary>
        public string ParentDirectory { get; set; }

        /// <summary>
        /// Gets or sets the archive code.
        /// </summary>
        public string ArchiveCode { get; set;  }


        /// <summary>
        /// Gets the full path of the trend table.
        /// </summary>
        protected string GetTablePath(TrendTable trendTable)
        {
            if (string.IsNullOrEmpty(ParentDirectory) || string.IsNullOrEmpty(ArchiveCode))
                throw new ScadaException("Failed to get the trend table path.");

            return Path.Combine(ParentDirectory, GetTableDirectory(ArchiveCode, trendTable.TableDate));
        }

        /// <summary>
        /// Writes the specified slice to the trend table.
        /// </summary>
        public void WriteSlice(TrendTable trendTable, Slice slice)
        {
            if (trendTable == null)
                throw new ArgumentNullException("trendTable");
            if (slice == null)
                throw new ArgumentNullException("slice");


        }

        /// <summary>
        /// Writes the input channel data to the trend table.
        /// </summary>
        public void WriteCnlData(TrendTable trendTable, int cnlNum, CnlData cnlData)
        {
            if (trendTable == null)
                throw new ArgumentNullException("trendTable");
        }


        /// <summary>
        /// Gets the trend table directory corresponding to the specified archive and date.
        /// </summary>
        public static string GetTableDirectory(string archiveCode, DateTime tableDate)
        {
            return archiveCode + tableDate.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Gets the page file name corresponding to the specified archive, date and page number.
        /// </summary>
        public static string GetPageFileName(string archiveCode, DateTime tableDate, int pageNumber)
        {
            return archiveCode.ToLowerInvariant() + tableDate.ToString("yyyyMMdd") + 
                "_" + pageNumber.ToString("D4") + ".dat";
        }

        /// <summary>
        /// Gets the meta file name corresponding to the specified archive and date.
        /// </summary>
        public static string GetMetaFileName(string archiveCode, DateTime tableDate)
        {
            return archiveCode.ToLowerInvariant() + tableDate.ToString("yyyyMMdd") + "_meta.dat";
        }
    }
}
