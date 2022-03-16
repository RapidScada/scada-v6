// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvMqttClient.Config
{
    /// <summary>
    /// Represents options for connecting to an MQTT broker.
    /// <para>Представляет параметры подключения к MQTT-брокеру.</para>
    /// </summary>
    internal class MqttConnectionOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttConnectionOptions()
        {
            Name = "";
            Server = "";
            Port = 1883;
            ClientID = "";
            Username = "";
            Password = "";
            Timeout = 10000;
            ProtocolVersion = 0;
        }


        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Name { get; set; }

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
        public int ProtocolVersion { get; set; }
        
        
        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            Name = xmlNode.GetChildAsString("Name");
            Server = xmlNode.GetChildAsString("Server");
            Port = xmlNode.GetChildAsInt("Port", Port);
            ClientID = xmlNode.GetChildAsString("ClientID");
            Username = xmlNode.GetChildAsString("Username");
            Password = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
            Timeout = xmlNode.GetChildAsInt("Timeout", Timeout);
            ProtocolVersion = xmlNode.GetChildAsInt("ProtocolVersion");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));

            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("Server", Server);
            xmlElem.AppendElem("Port", Port);
            xmlElem.AppendElem("ClientID", ClientID);
            xmlElem.AppendElem("Username", Username);
            xmlElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
            xmlElem.AppendElem("Timeout", Timeout);
            xmlElem.AppendElem("ProtocolVersion", ProtocolVersion);
        }
    }
}
