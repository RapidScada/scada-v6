// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet.Client.Options;
using MQTTnet.Formatter;
using Scada.Config;

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Represents options for connecting to an MQTT broker.
    /// <para>Представляет параметры подключения к MQTT-брокеру.</para>
    /// </summary>
    public class MqttConnectionOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttConnectionOptions()
        {
            Server = "";
            Port = 1883;
            ClientID = "";
            Username = "";
            Password = "";
            Timeout = 10000;
            ProtocolVersion = MqttProtocolVersion.Unknown;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttConnectionOptions(OptionList options)
        {
            Server = options.GetValueAsString("Server");
            Port = options.GetValueAsInt("Port", 1883);
            ClientID = options.GetValueAsString("ClientID");
            Username = options.GetValueAsString("Username");
            Password = ScadaUtils.Decrypt(options.GetValueAsString("Password"));
            Timeout = options.GetValueAsInt("Timeout", 10000);
            ProtocolVersion = options.GetValueAsEnum("ProtocolVersion", MqttProtocolVersion.Unknown);
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
        public MqttProtocolVersion ProtocolVersion { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options, bool clearList = true)
        {
            if (clearList)
                options.Clear();

            options["Server"] = Server;
            options["Port"] = Port.ToString();
            options["ClientID"] = ClientID;
            options["Username"] = Username;
            options["Password"] = ScadaUtils.Encrypt(Password);
            options["Timeout"] = Timeout.ToString();
            options["ProtocolVersion"] = ProtocolVersion.ToString();
        }

        /// <summary>
        /// Converts the connection options to client options.
        /// </summary>
        public IMqttClientOptions ToMqttClientOptions()
        {
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                .WithTcpServer(Server, Port > 0 ? Port : null);

            if (!string.IsNullOrEmpty(ClientID))
                builder.WithClientId(ClientID);

            if (!string.IsNullOrEmpty(Username))
                builder.WithCredentials(Username, Password);

            if (Timeout > 0)
                builder.WithCommunicationTimeout(TimeSpan.FromMilliseconds(Timeout));

            if (ProtocolVersion > MqttProtocolVersion.Unknown)
                builder.WithProtocolVersion(ProtocolVersion);

            return builder.Build();
        }
    }
}
