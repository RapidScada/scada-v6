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
using System.Text;

namespace Scada
{
    /// <summary>
    /// Represents an in-memory cache.
    /// <para>Представляет кэш в памяти.</para>
    /// </summary>
    public class MemoryCache<TKey, TValue>
    {
        /// <summary>
        /// Adds a cache entry into the cache.
        /// </summary>
        public bool Add(TKey key, TValue value)
        {
            return false;
        }

        /// <summary>
        /// Gets an entry from the cache.
        /// </summary>
        public TValue Get(TKey key)
        {
            return default(TValue);
        }
    }
}
