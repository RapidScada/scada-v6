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
 * Module   : ScadaCommCommon
 * Summary  : Represents a dictionary that groups devices by string address
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents a dictionary that groups devices by string address.
    /// <para>Представляет словарь, в котором устройства группируются по строковому адресу.</para>
    /// </summary>
    public class DeviceDictionary
    {
        /// <summary>
        /// Represents a group of devics with the same string address.
        /// </summary>
        public class DeviceGroup : List<DeviceLogic>
        {
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceDictionary()
        {
            DeviceGroups = new Dictionary<string, DeviceGroup>();
        }


        /// <summary>
        /// Gets the device groups accessed by string address.
        /// </summary>
        public Dictionary<string, DeviceGroup> DeviceGroups { get; }


        /// <summary>
        /// Adds and groups the specified devices.
        /// </summary>
        public void AddRange(IEnumerable<DeviceLogic> devices)
        {
            if (devices == null)
                throw new ArgumentNullException(nameof(devices));

            foreach (DeviceLogic deviceLogic in devices)
            {
                if (!string.IsNullOrEmpty(deviceLogic.StrAddress))
                {
                    if (!DeviceGroups.TryGetValue(deviceLogic.StrAddress, out DeviceGroup deviceList))
                    {
                        deviceList = new DeviceGroup();
                        DeviceGroups.Add(deviceLogic.StrAddress, deviceList);
                    }

                    deviceList.Add(deviceLogic);
                }
            }
        }

        /// <summary>
        /// Gets the device group with the specified string address.
        /// </summary>
        public bool GetDeviceGroup(string strAddress, out DeviceGroup deviceGroup)
        {
            return DeviceGroups.TryGetValue(strAddress, out deviceGroup);
        }

        /// <summary>
        /// Selects the device groups.
        /// </summary>
        public IEnumerable<DeviceGroup> SelectDeviceGroups()
        {
            foreach (DeviceGroup deviceGroup in DeviceGroups.Values)
            {
                yield return deviceGroup;
            }
        }
    }
}
