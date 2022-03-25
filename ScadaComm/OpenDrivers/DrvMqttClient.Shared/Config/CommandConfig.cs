// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvMqttClient.Config
{
    /// <summary>
    /// Represents a command configuration.
    /// <para>Представляет конфигурацию команды.</para>
    /// </summary>
    internal class CommandConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommandConfig()
        {
            Topic = "";
            DisplayName = "";
            CmdCode = "";
            QosLevel = 0;
            Retain = false;
        }


        /// <summary>
        /// Gets or sets the MQTT topic.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the command code.
        /// </summary>
        public string CmdCode { get; set; }

        /// <summary>
        /// Gets or sets the quality of service level.
        /// </summary>
        public int QosLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to set the retained flag.
        /// </summary>
        public bool Retain { get; set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Topic = xmlElem.GetAttrAsString("topic");
            DisplayName = xmlElem.GetAttrAsString("displayName");
            CmdCode = xmlElem.GetAttrAsString("cmdCode");
            QosLevel = xmlElem.GetAttrAsInt("qosLevel");
            Retain = xmlElem.GetAttrAsBool("retain");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("topic", Topic);
            xmlElem.SetAttribute("displayName", DisplayName);
            xmlElem.SetAttribute("cmdCode", CmdCode);
            xmlElem.SetAttribute("qosLevel", QosLevel);
            xmlElem.SetAttribute("retain", Retain);
        }
    }
}
