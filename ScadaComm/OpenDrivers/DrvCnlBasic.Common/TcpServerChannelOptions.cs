// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Config;

namespace Scada.Comm.Drivers.DrvCnlBasic
{
    /// <summary>
    /// Represents TCP server channel options.
    /// <para>Представляет параметры канала TCP-сервер.</para>
    /// </summary>
    public class TcpServerChannelOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TcpServerChannelOptions(OptionList options)
        {
            TcpPort = options.GetValueAsInt("TcpPort", 502); // Modbus port
            ClientLifetime = options.GetValueAsInt("ClientLifetime", 60);
            Behavior = options.GetValueAsEnum("Behavior", ChannelBehavior.Master);
            ConnectionMode = options.GetValueAsEnum("ConnectionMode", ConnectionMode.Individual);
            DeviceMapping = options.GetValueAsEnum("DeviceMapping", DeviceMapping.ByIPAddress);
        }


        /// <summary>
        /// Gets or sets the TCP port for incoming connections.
        /// </summary>
        public int TcpPort { get; set; }

        /// <summary>
        /// Gets or sets the time after which an inactive client is disconnected, s.
        /// </summary>
        public int ClientLifetime { get; set; }

        /// <summary>
        /// Gets or sets the channel behavior.
        /// </summary>
        public ChannelBehavior Behavior { get; set; }

        /// <summary>
        /// Gets or sets the connection mode.
        /// </summary>
        public ConnectionMode ConnectionMode { get; set; }

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
            options["TcpPort"] = TcpPort.ToString();
            options["ClientLifetime"] = ClientLifetime.ToString();
            options["Behavior"] = Behavior.ToString();
            options["ConnectionMode"] = ConnectionMode.ToString();
            options["DeviceMapping"] = DeviceMapping.ToString();
        }
    }
}
