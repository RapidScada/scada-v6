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
 * Summary  : Provides data for receiving and processing an incoming request
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using System.Collections.Generic;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Provides data for receiving and processing an incoming request.
    /// <para>Предоставляет данные для приема и обработки входящего запроса.</para>
    /// </summary>
    public class IncomingRequestArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public IncomingRequestArgs()
        {
            TargetDevices = new List<DeviceLogic>();
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets a value that indicates whether an error occurred while receiving and processing the request.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Gets the devices that corresponds the request.
        /// </summary>
        public List<DeviceLogic> TargetDevices { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether execute the same method for the next device.
        /// </summary>
        public bool NextDevice { get; set; }

        /// <summary>
        /// Gets or sets the object that contains data related to the request.
        /// </summary>
        public object Tag { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        public void SetToDefault()
        {
            HasError = false;
            TargetDevices.Clear();
            NextDevice = false;
            Tag = null;
        }

        /// <summary>
        /// Gets the first target device, or null if there are no devices.
        /// </summary>
        public DeviceLogic GetFirstDevice()
        {
            return TargetDevices.Count > 0 ? TargetDevices[0] : null;
        }
    }
}
