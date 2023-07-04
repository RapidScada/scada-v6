// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Config;

namespace Scada.Comm.Drivers.DrvCnlBasic
{
    /// <summary>
    /// Represents TCP client channel options.
    /// <para>Представляет параметры канала TCP-клиент.</para>
    /// </summary>
    public class TcpClientChannelOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TcpClientChannelOptions(OptionList options)
        {
            Host = options.GetValueAsString("Host");
            TcpPort = options.GetValueAsInt("TcpPort", 502); // Modbus port
            ReconnectAfter = options.GetValueAsInt("ReconnectAfter", 5);
            StayConnected = options.GetValueAsBool("StayConnected", true);
            DisconnectOnError = options.GetValueAsBool("DisconnectOnError", false);
            Behavior = options.GetValueAsEnum("Behavior", ChannelBehavior.Master);
            ConnectionMode = options.GetValueAsEnum("ConnectionMode", ConnectionMode.Individual);
        }


        /// <summary>
        /// Gets or sets the remote host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the default remote TCP port.
        /// </summary>
        public int TcpPort { get; set; }

        /// <summary>
        /// Gets or sets the reconnect interval in seconds.
        /// </summary>
        public int ReconnectAfter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to keep a connection open after a session.
        /// </summary>
        public bool StayConnected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disconnect if a session fails.
        /// </summary>
        public bool DisconnectOnError { get; set; }

        /// <summary>
        /// Gets or sets the channel behavior.
        /// </summary>
        public ChannelBehavior Behavior { get; set; }

        /// <summary>
        /// Gets or sets the connection mode.
        /// </summary>
        public ConnectionMode ConnectionMode { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["Host"] = Host;
            options["TcpPort"] = TcpPort.ToString();
            options["ReconnectAfter"] = ReconnectAfter.ToString();
            options["StayConnected"] = StayConnected.ToLowerString();
            options["DisconnectOnError"] = DisconnectOnError.ToLowerString();
            options["Behavior"] = Behavior.ToString();
            options["ConnectionMode"] = ConnectionMode.ToString();
        }
    }
}
