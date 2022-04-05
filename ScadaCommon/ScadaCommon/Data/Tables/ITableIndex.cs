/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Defines functionality of configuration database indexes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using System.Collections;
using System.Collections.Generic;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Defines functionality of configuration database indexes.
    /// <para>Определяет функциональность индексов базы конфигурации.</para>
    /// </summary>
    public interface ITableIndex
    {

        /// <summary>
        /// Gets a value indicating whether the index is ready to use.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Gets a value indicating whether null values are allowed in the indexed column.
        /// </summary>
        bool AllowNull { get; }


        /// <summary>
        /// Adds the item to the index.
        /// </summary>
        void AddToIndex(object item, int itemKey);

        /// <summary>
        /// Adds the items to the index and marks the index ready to use.
        /// </summary>
        void AddRangeToIndex<T>(IEnumerable<KeyValuePair<int, T>> items);

        /// <summary>
        /// Removes the item from the index.
        /// </summary>
        void RemoveFromIndex(object item, int itemKey);

        /// <summary>
        /// Checks whether the specified key is represented in the index.
        /// </summary>
        bool IndexKeyExists(object indexKey);

        /// <summary>
        /// Returns an enumerable collection of the distinct index keys.
        /// </summary>
        IEnumerable EnumerateIndexKeys();

        /// <summary>
        /// Selects items keys by the specified index key.
        /// </summary>
        IEnumerable<int> SelectItemKeys(object indexKey);

        /// <summary>
        /// Selects items by the specified index key.
        /// </summary>
        IEnumerable SelectItems(object indexKey);

        /// <summary>
        /// Resets the index to its initial state.
        /// </summary>
        void Reset();
    }
}
