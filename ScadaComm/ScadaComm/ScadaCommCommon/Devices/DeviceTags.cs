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
 * Module   : ScadaCommCommon
 * Summary  : Represents a set of device tags
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using System.Collections;
using System.Collections.Generic;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents a set of device tags.
    /// <para>Представляет набор тегов устройства.</para>
    /// </summary>
    public class DeviceTags : IEnumerable<DeviceTag>
    {
        private readonly List<DeviceTag> deviceTags;              // the list of all device tags
        private readonly Dictionary<string, DeviceTag> tagByCode; // the device tags accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceTags()
        {
            deviceTags = new List<DeviceTag>();
            tagByCode = new Dictionary<string, DeviceTag>();

            TagGroups = new List<TagGroup>();
            FlattenGroups = false;
            UseStatusTag = true;
            StatusTag = null;
        }


        /// <summary>
        /// Gets the device tag at the specified index, or throws an exception if the specified index is out of range.
        /// </summary>
        public DeviceTag this[int index]
        {
            get
            {
                return deviceTags[index];
            }
        }

        /// <summary>
        /// Gets the device tag with the specified code, or throws an exception if the specified key is not found.
        /// </summary>
        public DeviceTag this[string code]
        {
            get
            {
                return tagByCode[code];
            }
        }

        /// <summary>
        /// Gets the tag groups.
        /// </summary>
        public List<TagGroup> TagGroups { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to flatten the tag groups for display.
        /// </summary>
        public bool FlattenGroups { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a special status tag should be added automatically.
        /// </summary>
        public bool UseStatusTag { get; set; }

        /// <summary>
        /// Gets the tag corresponding to the device status.
        /// </summary>
        public DeviceTag StatusTag { get; private set; }

        /// <summary>
        /// Gets the number of device tags.
        /// </summary>
        public int Count
        {
            get
            {
                return deviceTags.Count;
            }
        }


        /// <summary>
        /// Adds the tag group and calculates the tag indexes.
        /// </summary>
        public void AddGroup(TagGroup tagGroup)
        {
            int tagIndex = deviceTags.Count;
            TagGroups.Add(tagGroup);

            foreach (DeviceTag deviceTag in tagGroup.DeviceTags)
            {
                deviceTag.Index = tagIndex++;
                deviceTags.Add(deviceTag);

                if (!string.IsNullOrEmpty(deviceTag.Code) && !tagByCode.ContainsKey(deviceTag.Code))
                    tagByCode.Add(deviceTag.Code, deviceTag);
            }
        }

        /// <summary>
        /// Adds the status tag.
        /// </summary>
        public void AddStatusTag()
        {
            TagGroup tagGroup = new TagGroup(CommUtils.StatusTagCode) { Hidden = true };
            StatusTag = tagGroup.AddTag(CommUtils.StatusTagCode, CommUtils.StatusTagCode);
            AddGroup(tagGroup);
        }

        /// <summary>
        /// Determines whether a tag with the specified code exists.
        /// </summary>
        public bool ContainsTag(string code)
        {
            return tagByCode.ContainsKey(code);
        }

        /// <summary>
        /// Gets the device tag at the specified index.
        /// </summary>
        public bool TryGetTag(int index, out DeviceTag deviceTag)
        {
            if (0 <= index && index < deviceTags.Count)
            {
                deviceTag = deviceTags[index];
                return true;
            }
            else
            {
                deviceTag = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the device tag with the specified code.
        /// </summary>
        public bool TryGetTag(string code, out DeviceTag deviceTag)
        {
            return tagByCode.TryGetValue(code, out deviceTag);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the device tags.
        /// </summary>
        public IEnumerator<DeviceTag> GetEnumerator()
        {
            return deviceTags.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the device tags.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return deviceTags.GetEnumerator();
        }
    }
}
