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

using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represent a cache item that contains a channel list.
    /// <para>Представляет элемент кэша, который содержит список каналов.</para>
    /// </summary>
    internal class CnlListItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlListItem(long cnlListID, int capacity)
        {
            CnlListID = cnlListID;
            CnlTags = new List<CnlTag>(capacity);
        }


        /// <summary>
        /// Gets the channel list ID.
        /// </summary>
        public long CnlListID { get; }

        /// <summary>
        /// Gets the channel tags.
        /// </summary>
        public List<CnlTag> CnlTags { get; }
    }
}
