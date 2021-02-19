/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaServerEngine
 * Summary  : Represents a server level cache
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Tables;
using System;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents a server level cache.
    /// <para>Представляет кэш уровня сервера.</para>
    /// </summary>
    internal class ServerCache
    {
        /// <summary>
        /// Determines how long an item is stored in the cache.
        /// </summary>
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(1);

        private long maxItemID; // the maximum cache item ID


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerCache()
        {
            maxItemID = 0;
            CnlListCache = new MemoryCache<long, CnlListItem>(CacheExpiration);
            DataFilterCache = new MemoryCache<long, DataFilter>(CacheExpiration);
        }


        /// <summary>
        /// Gets the cache containing channel lists accessed by list ID.
        /// </summary>
        public MemoryCache<long, CnlListItem> CnlListCache { get; }

        /// <summary>
        /// Gets the cache containing data filters.
        /// </summary>
        public MemoryCache<long, DataFilter> DataFilterCache { get; }


        /// <summary>
        /// Gets the next cache item ID.
        /// </summary>
        public long GetNextID()
        {
            return ++maxItemID;
        }
    }
}
