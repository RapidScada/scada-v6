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
 * Module   : ScadaCommCommon
 * Summary  : Represents a set of device tags
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents a set of device tags.
    /// <para>Представляет набор тегов КП.</para>
    /// </summary>
    public class DeviceTags
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceTags()
        {
            TagGroups = new List<TagGroup>();
        }

        /// <summary>
        /// Gets the tag groups.
        /// </summary>
        public List<TagGroup> TagGroups { get; }

        /// <summary>
        /// Adds the tag group and calculates the tag indexes.
        /// </summary>
        public void AddGroup(TagGroup tagGroup)
        {

        }
    }
}
