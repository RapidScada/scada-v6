// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Config;

namespace Scada.Comm.Drivers.DrvCnlBasic
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


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["LocalUdpPort"] = LocalUdpPort.ToString();
            options["RemoteUdpPort"] = RemoteUdpPort.ToString();
            options["RemoteIpAddress"] = RemoteIpAddress;
            options["Behavior"] = Behavior.ToString();
            options["DeviceMapping"] = DeviceMapping.ToString();
        }
    }
}
