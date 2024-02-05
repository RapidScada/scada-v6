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
    /// <remarks>The class is thread-safe.</remarks>
    public class MemoryCache<TKey, TValue>
    {
        /// <summary>
        /// Represents a cache entry.
        /// </summary>
        protected class CacheEntry
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public CacheEntry(TKey key, TValue value, DateTime lastAccessTime)
            {
                Key = key;
                Value = value;
                LastAccessTime = lastAccessTime;
            }

            /// <summary>
            /// Gets the entry key.
            /// </summary>
            public TKey Key { get; }
            /// <summary>
            /// Gets the entry value.
            /// </summary>
            public TValue Value { get; set; }
            /// <summary>
            /// Gets or sets the time (UTC) when the current entry was last accessed.
            /// </summary>
            public DateTime LastAccessTime { get; set; }
        }


        /// <summary>
        /// The cache entries.
        /// </summary>
        protected readonly Dictionary<TKey, CacheEntry> entries;
        /// <summary>
        /// The period of removing the outdated cache entries.
        /// </summary>
        protected readonly TimeSpan cleanupPeriod;
        /// <summary>
        /// The time (UTC) when the outdated entries were last removed.
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
            entries = new Dictionary<TKey, CacheEntry>();
            cleanupPeriod = TimeSpan.FromTicks(slidingExpiration.Ticks / 2);
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
        /// Removes the outdated entries.
        /// </summary>
        protected void RemoveOutdatedItems(DateTime nowDT)
        {
            if (nowDT - lastCleanupTime > cleanupPeriod)
            {
                // remove items that have not been accessed for a long time
                List<TKey> keysToRemove = new List<TKey>();

                foreach (KeyValuePair<TKey, CacheEntry> pair in entries)
                {
                    if (nowDT - pair.Value.LastAccessTime > SlidingExpiration)
                        keysToRemove.Add(pair.Key);
                }

                foreach (TKey key in keysToRemove)
                {
                    entries.Remove(key);
                }

                // remove items if capacity is exceeded, taking into account access time
                int itemCnt = entries.Count;

                if (itemCnt > Capacity)
                {
                    TKey[] keys = new TKey[itemCnt];
                    DateTime[] accessTimes = new DateTime[itemCnt];
                    int i = 0;

                    foreach (KeyValuePair<TKey, CacheEntry> pair in entries)
                    {
                        keys[i] = pair.Key;
                        accessTimes[i] = pair.Value.LastAccessTime;
                        i++;
                    }

                    Array.Sort(accessTimes, keys);
                    int delCnt = itemCnt - Capacity;

                    for (int j = 0; j < delCnt; j++)
                    {
                        entries.Remove(keys[j]);
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
            lock (entries)
            {
                DateTime utcNow = DateTime.UtcNow;
                bool added = false;

                if (!entries.ContainsKey(key))
                {
                    entries.Add(key, new CacheEntry(key, value, DateTime.UtcNow));
                    added = true;
                }

                RemoveOutdatedItems(utcNow);
                return added;
            }
        }

        /// <summary>
        /// Gets an entry from the cache, or null if the specified key is not found.
        /// </summary>
        public TValue Get(TKey key)
        {
            lock (entries)
            {
                DateTime utcNow = DateTime.UtcNow;
                TValue value = default;

                if (entries.TryGetValue(key, out CacheEntry entry))
                {
                    entry.LastAccessTime = utcNow;
                    value = entry.Value;
                }

                RemoveOutdatedItems(utcNow);
                return value;
            }
        }

        /// <summary>
        /// Gets an entry from the cache, or creates a new one and adds it to the cache.
        /// </summary>
        public TValue GetOrCreate(TKey key, Func<TValue> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            lock (entries)
            {
                DateTime utcNow = DateTime.UtcNow;
                TValue value;

                if (entries.ContainsKey(key))
                {
                    CacheEntry entry = entries[key];
                    entry.LastAccessTime = utcNow;
                    value = entry.Value;
                }
                else
                {
                    value = factory();
                    entries.Add(key, new CacheEntry(key, value, utcNow));
                }

                RemoveOutdatedItems(utcNow);
                return value;
            }
        }

        /// <summary>
        /// Gets the snapshot of the cache values.
        /// </summary>
        public TValue[] GetSnapshot()
        {
            lock (entries)
            {
                RemoveOutdatedItems(DateTime.UtcNow);
                TValue[] values = new TValue[entries.Count];
                int i = 0;

                foreach (CacheEntry entry in entries.Values)
                {
                    values[i++] = entry.Value;
                }

                return values;
            }
        }
    }
}
