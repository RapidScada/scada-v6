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
    /// <para>Представляет словарь, в котором КП группируются по строковому адресу.</para>
    /// </summary>
    public class DeviceDictionary
    {
        /// <summary>
        /// Represents a list of device logic instances.
        /// </summary>
        public class DeviceList : List<DeviceLogic>
        {
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceDictionary()
        {
            DeviceMap = new Dictionary<string, DeviceList>();
        }


        /// <summary>
        /// Gets the devices accessed by string address.
        /// </summary>
        public Dictionary<string, DeviceList> DeviceMap { get; }


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
                    if (!DeviceMap.TryGetValue(deviceLogic.StrAddress, out DeviceList deviceList))
                    {
                        deviceList = new DeviceList();
                        DeviceMap.Add(deviceLogic.StrAddress, deviceList);
                    }

                    deviceList.Add(deviceLogic);
                }
            }
        }

        /// <summary>
        /// Gets the devices with the specified string address.
        /// </summary>
        public bool GetDevices(string strAddress, out DeviceList devices)
        {
            return DeviceMap.TryGetValue(strAddress, out devices);
        }
    }
}
