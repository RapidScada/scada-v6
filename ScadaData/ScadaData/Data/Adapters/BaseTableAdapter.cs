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
 * Summary  : Represents a mechanism to read and write the configuration database tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Tables;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read and write the configuration database tables.
    /// <para>Представляет механизм для чтения и записи таблиц базы конфигурации.</para>
    /// </summary>
    public class BaseTableAdapter : Adapter
    {
        /// <summary>
        /// Fills the specified table by reading data from the configuration database.
        /// </summary>
        public void Fill(IBaseTable baseTable)
        {
        }

        /// <summary>
        /// Updates the configuration database by writing data of the specified table.
        /// </summary>
        public void Update(IBaseTable baseTable)
        {
        }
    }
}
