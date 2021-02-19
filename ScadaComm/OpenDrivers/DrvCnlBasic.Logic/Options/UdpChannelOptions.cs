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
 * Module   : DrvCnlBasic
 * Summary  : Represents UDP channel options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic.Options
{
    /// <summary>
    /// Represents UDP channel options.
    /// <para>Представляет параметры канала UDP.</para>
    /// </summary>
    public class UdpChannelOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UdpChannelOptions(OptionList options)
        {
            LocalUdpPort = options.GetValueAsInt("LocalUdpPort", 1);
            RemoteUdpPort = options.GetValueAsInt("RemoteUdpPort", 1);
            RemoteIpAddress = options.GetValueAsString("RemoteIpAddress");
            Behavior = options.GetValueAsEnum("Behavior", ChannelBehavior.Master);
            DeviceMapping = options.GetValueAsEnum("DeviceMapping", DeviceMapping.ByIPAddress);
        }


        /// <summary>
        /// Gets or sets the local UDP port.
        /// </summary>
        public int LocalUdpPort { get; set; }

        /// <summary>
        /// Gets or sets the remote UDP port.
        /// </summary>
        public int RemoteUdpPort { get; set; }

        /// <summary>
        /// Gets or sets the default remote IP address.
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the channel behavior.
        /// </summary>
        public ChannelBehavior Behavior { get; set; }

        /// <summary>
        /// Gets or sets the device mapping mode.
        /// </summary>
        public DeviceMapping DeviceMapping { get; set; }
    }
}
