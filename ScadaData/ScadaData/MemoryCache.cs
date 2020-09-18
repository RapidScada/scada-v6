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
 * Summary  : Represents an in-memory cache
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;

namespace Scada
{
    /// <summary>
    /// Represents an in-memory cache.
    /// <para>Представляет кэш в памяти.</para>
    /// </summary>
    public class MemoryCache<TKey, TValue>
    {
        /// <summary>
        /// Represents a cache item.
        /// </summary>
        protected class CacheItem
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public CacheItem(TKey key, TValue value, DateTime lastAccessTime)
            {
                Key = key;
                Value = value;
                LastAccessTime = lastAccessTime;
            }

            /// <summary>
            /// Gets the item key.
            /// </summary>
            public TKey Key { get; }
            /// <summary>
            /// Gets the item value.
            /// </summary>
            public TValue Value { get; set; }            
            /// <summary>
            /// Gets or sets the time (UTC) when the current item was last accessed.
            /// </summary>
            public DateTime LastAccessTime { get; set; }
        }


        /// <summary>
        /// The cache items.
        /// </summary>
        protected Dictionary<TKey, CacheItem> items;
        /// <summary>
        /// The time (UTC) when the outdated items were last removed.
        /// </summary>
        protected DateTime lastCleanupTime;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MemoryCache(TimeSpan slidingExpiration)
            : this(slidingExpiration, int.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MemoryCache(TimeSpan slidingExpiration, int capacity)
        {
            items = new Dictionary<TKey, CacheItem>();
            lastCleanupTime = DateTime.UtcNow;

            SlidingExpiration = slidingExpiration;
            Capacity = capacity;
        }


        /// <summary>
        /// Gets a value that indicates whether a cache entry should be evicted
        /// if it has not been accessed in a given span of time.
        /// </summary>
        public TimeSpan SlidingExpiration { get; }

        /// <summary>
        /// Gets the maximum number of entries that can be stored in the cache.
        /// </summary>
        public int Capacity { get; }


        /// <summary>
        /// Removes the outdated items.
        /// </summary>
        protected void RemoveOutdatedItems(DateTime nowDT)
        {
            lock (items)
            {
                // remove items that have not been accessed for a long time
                List<TKey> keysToRemove = new List<TKey>();

                foreach (KeyValuePair<TKey, CacheItem> pair in items)
                {
                    if (nowDT - pair.Value.LastAccessTime > SlidingExpiration)
                        keysToRemove.Add(pair.Key);
                }

                foreach (TKey key in keysToRemove)
                {
                    items.Remove(key);
                }

                // remove items if capacity is exceeded, taking into account access time
                int itemCnt = items.Count;

                if (itemCnt > Capacity)
                {
                    TKey[] keys = new TKey[itemCnt];
                    DateTime[] accessTimes = new DateTime[itemCnt];
                    int i = 0;

                    foreach (KeyValuePair<TKey, CacheItem> pair in items)
                    {
                        keys[i] = pair.Key;
                        accessTimes[i] = pair.Value.LastAccessTime;
                        i++;
                    }

                    Array.Sort(accessTimes, keys);
                    int delCnt = itemCnt - Capacity;

                    for (int j = 0; j < delCnt; j++)
                    {
                        items.Remove(keys[j]);
                    }
                }

                lastCleanupTime = nowDT;
            }
        }

        /// <summary>
        /// Adds a cache entry into the cache.
        /// </summary>
        public bool Add(TKey key, TValue value)
        {
            lock (items)
            {
                if (items.ContainsKey(key))
                {
                    return false;
                }
                else
                {
                    items.Add(key, new CacheItem(key, value, DateTime.UtcNow));
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets an entry from the cache, or null if the specified key is not found.
        /// </summary>
        public TValue Get(TKey key)
        {
            lock (items)
            {
                TValue itemValue = default(TValue);
                DateTime utcNow = DateTime.UtcNow;

                // get the item
                if (items.TryGetValue(key, out CacheItem item))
                {
                    item.LastAccessTime = utcNow;
                    itemValue = item.Value;
                }

                // cleanup the cache
                if (utcNow - lastCleanupTime > SlidingExpiration)
                    RemoveOutdatedItems(utcNow);

                return itemValue;
            }
        }
    }
}
