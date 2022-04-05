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
 * Summary  : Represents an index of a configuration database table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2022
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents an index of a configuration database table.
    /// <para>Представляет индекс таблицы базы конфигурации.</para>
    /// </summary>
    public class TableIndex<TKey, TItem> : ITableIndex
    {
        /// <summary>
        /// Represents a group of table items with the same indexed value, accessed by item primary key.
        /// </summary>
        public class IndexGroup : SortedDictionary<int, TItem>
        {
        }


        /// <summary>
        /// The indexed property.
        /// </summary>
        protected readonly PropertyDescriptor indexProp;
        /// <summary>
        /// The empty value of the indexed property.
        /// </summary>
        protected object emptyValue;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableIndex(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException("Column name must not be empty.", nameof(columnName));

            indexProp = GetIndexProp(columnName);
            ColumnName = columnName;
            ItemGroups = new SortedDictionary<TKey, IndexGroup>();
            IsReady = false;
        }


        /// <summary>
        /// Gets the name of the indexed column.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gets the item groups referenced by the index values.
        /// </summary>
        public SortedDictionary<TKey, IndexGroup> ItemGroups { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the index is ready to use.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets a value indicating whether null values are allowed in the indexed column.
        /// </summary>
        public bool AllowNull { get; protected set; }


        /// <summary>
        /// Gets the indexed property.
        /// </summary>
        private PropertyDescriptor GetIndexProp(string columnName)
        {
            PropertyDescriptorCollection itemProps = TypeDescriptor.GetProperties(typeof(TItem));
            PropertyDescriptor prop = itemProps[columnName];

            if (prop == null)
                throw new ScadaException("The item type doesn't contain the required property.");

            AllowNull = prop.PropertyType.IsNullable();
            Type propType = AllowNull ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;

            if (prop.PropertyType != typeof(TKey))
                throw new ScadaException("Column type mismatch.");

            if (propType == typeof(int))
                emptyValue = 0;
            else if (propType == typeof(string))
                emptyValue = "";
            else
                throw new ScadaException("Column type is not supported.");

            return prop;
        }

        /// <summary>
        /// Gets the value of the indexed property.
        /// </summary>
        private TKey GetIndexKey(object item)
        {
            object indexKey = indexProp.GetValue(item);
            return indexKey == null ? (TKey)emptyValue : (TKey)indexKey;
        }

        /// <summary>
        /// Converts the specified object to an index key of the particular type.
        /// </summary>
        private TKey CastIndexKey(object indexKey)
        {
            if (indexKey == null)
                return (TKey)emptyValue;
            else if (indexKey is TKey key)
                return key;
            else
                throw new ArgumentException("Invalid key type.", nameof(indexKey));
        }


        /// <summary>
        /// Adds the item to the index.
        /// </summary>
        public void AddToIndex(object item, int itemKey)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            TKey indexKey = GetIndexKey(item);

            if (!ItemGroups.TryGetValue(indexKey, out IndexGroup group))
            {
                group = new IndexGroup();
                ItemGroups.Add(indexKey, group);
            }

            group[itemKey] = (TItem)item;
        }

        /// <summary>
        /// Adds the items to the index and marks the index ready to use.
        /// </summary>
        public void AddRangeToIndex<T>(IEnumerable<KeyValuePair<int, T>> items)
        {
            foreach (KeyValuePair<int, T> pair in items)
            {
                AddToIndex(pair.Value, pair.Key);
            }

            IsReady = true;
        }

        /// <summary>
        /// Removes the item from the index.
        /// </summary>
        public void RemoveFromIndex(object item, int itemKey)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            TKey indexKey = GetIndexKey(item);

            if (ItemGroups.TryGetValue(indexKey, out IndexGroup group))
            {
                group.Remove(itemKey);

                if (group.Count == 0)
                    ItemGroups.Remove(indexKey);
            }
        }

        /// <summary>
        /// Checks whether the specified key is represented in the index.
        /// </summary>
        public bool IndexKeyExists(object indexKey)
        {
            return ItemGroups.ContainsKey(CastIndexKey(indexKey));
        }

        /// <summary>
        /// Returns an enumerable collection of the distinct index keys.
        /// </summary>
        public IEnumerable EnumerateIndexKeys()
        {
            foreach (TKey key in ItemGroups.Keys)
            {
                yield return key;
            }
        }

        /// <summary>
        /// Selects items keys by the specified index key.
        /// </summary>
        public IEnumerable<int> SelectItemKeys(object indexKey)
        {
            if (ItemGroups.TryGetValue(CastIndexKey(indexKey), out IndexGroup group))
            {
                foreach (int key in group.Keys)
                {
                    yield return key;
                }
            }
        }

        /// <summary>
        /// Selects items by the specified index key.
        /// </summary>
        public IEnumerable SelectItems(object indexKey)
        {
            if (ItemGroups.TryGetValue(CastIndexKey(indexKey), out IndexGroup group))
            {
                foreach (object item in group.Values)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Resets the index to its initial state.
        /// </summary>
        public void Reset()
        {
            ItemGroups.Clear();
            IsReady = false;
        }
    }
}
