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
 * Module   : ScadaServerEngine
 * Summary  : Represents a server level cache
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
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
        /// Specifies how long a cache entry can be inactive before it will be removed.
        /// </summary>
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(1);


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerCache()
        {
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
    }
}
