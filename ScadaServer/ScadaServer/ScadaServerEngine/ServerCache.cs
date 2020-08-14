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
 * Module   : ScadaServerEngine
 * Summary  : Represents a server level cache
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

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
        /// Determines how long a channel list is stored in the cache.
        /// </summary>
        private static readonly TimeSpan CnlListExpiration = TimeSpan.FromMinutes(1);

        private long maxCnlListID; // the maximum channel list ID


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerCache()
        {
            maxCnlListID = 0;
            CnlListCache = new MemoryCache<long, CnlListItem>(CnlListExpiration);
        }

        /// <summary>
        /// Gets the cache containing channel lists accessed by list IDs.
        /// </summary>
        public MemoryCache<long, CnlListItem> CnlListCache { get; }

        /// <summary>
        /// Gets the next channel list ID.
        /// </summary>
        public long GetNextCnlListID()
        {
            return ++maxCnlListID;
        }
    }
}
