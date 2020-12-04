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
        private readonly List<DeviceTag> tags;                    // the list of all device tags
        private readonly Dictionary<string, DeviceTag> tagByCode; // the device tags accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceTags()
        {
            tags = new List<DeviceTag>();
            tagByCode = new Dictionary<string, DeviceTag>();
            TagGroups = new List<TagGroup>();
        }


        /// <summary>
        /// Gets the tag groups.
        /// </summary>
        public List<TagGroup> TagGroups { get; }

        /// <summary>
        /// Gets the device tag at the specified index.
        /// </summary>
        public DeviceTag this[int index]
        {
            get
            {
                return tags[index];
            }
        }

        /// <summary>
        /// Gets the device tag with the specified code.
        /// </summary>
        public DeviceTag this[string code]
        {
            get
            {
                return tagByCode[code];
            }
        }


        /// <summary>
        /// Adds the tag group and calculates the tag indexes.
        /// </summary>
        public void AddGroup(TagGroup tagGroup)
        {
            int tagIndex = tags.Count;

            foreach (DeviceTag deviceTag in tagGroup.DeviceTags)
            {
                deviceTag.Index = tagIndex++;
                tags.Add(deviceTag);

                if (!string.IsNullOrEmpty(deviceTag.Code) && !tagByCode.ContainsKey(deviceTag.Code))
                    tagByCode.Add(deviceTag.Code, deviceTag);
            }
        }
    }
}
