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
 * Summary  : Represents a mechanism to read and write event tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Data;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read and write event tables.
    /// <para>Представляет механизм для чтения и записи таблиц событий.</para>
    /// </summary>
    public class EventTableAdapter
    {
        /// <summary>
        /// Fills the specified table by reading data from a file or stream.
        /// </summary>
        public void Fill(EventTable eventTable)
        {
            if (eventTable == null)
                throw new ArgumentNullException("eventTable");
        }

        /// <summary>
        /// Fills the specified table by reading data from a file or stream.
        /// </summary>
        public void Fill(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException("dataTable");
        }

        /// <summary>
        /// Appends the specified event to a file or stream.
        /// </summary>
        public void AppendEvent(Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException("ev");
        }

        /// <summary>
        /// Writes the event acknowledgement information in a file or stream.
        /// </summary>
        public void WriteEventAck(Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException("ev");
        }

        /// <summary>
        /// Gets the table file name corresponding to the specified archive and date.
        /// </summary>
        public static string GetTableFileName(string archiveCode, DateTime tableDate)
        {
            return archiveCode.ToLowerInvariant() + tableDate.ToString("yyyyMMdd") + ".dat";
        }
    }
}
