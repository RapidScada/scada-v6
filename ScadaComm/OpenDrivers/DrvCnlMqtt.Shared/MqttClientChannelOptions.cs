// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;

namespace Scada.Comm.Drivers.DrvCnlMqtt
{
    /// <summary>
    /// Represents options for connecting to an MQTT broker.
    /// <para>Представляет параметры подключения к MQTT-брокеру.</para>
    /// </summary>
    internal class MqttClientChannelOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientChannelOptions(OptionList options)
        {
            Server = options.GetValueAsString("Server");
            Port = options.GetValueAsInt("Port", 1883);
            ClientID = options.GetValueAsString("ClientID");
            Username = options.GetValueAsString("Username");
            Password = ScadaUtils.Decrypt(options.GetValueAsString("Password"));
            Timeout = options.GetValueAsInt("Timeout", 10000);
            ProtocolVersion = options.GetValueAsEnum("ProtocolVersion", ProtocolVersion.Unknown);
        }


        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the TCP port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the unique client ID.
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the send and receive timeout, ms.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the protocol version.
        /// </summary>
        public ProtocolVersion ProtocolVersion { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["Server"] = Server;
            options["Port"] = Port.ToString();
            options["ClientID"] = ClientID;
            options["Username"] = Username;
            options["Password"] = ScadaUtils.Encrypt(Password);
            options["Timeout"] = Timeout.ToString();
            options["ProtocolVersion"] = ProtocolVersion.ToString();
        }
    }
}
