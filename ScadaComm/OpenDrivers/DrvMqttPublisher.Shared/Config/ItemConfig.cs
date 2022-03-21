// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvMqttPublisher.Config
{
    /// <summary>
    /// Represents an item configuration.
    /// <para>Представляет конфигурацию элемента.</para>
    /// </summary>
    internal class ItemConfig
    {
        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the MQTT topic.
        /// </summary>
        public string Topic { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether the item is used to publish channel values.
        /// </summary>
        public bool Publish { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the item is used to receive commands.
        /// </summary>
        public bool Subscribe { get; set; } = false;

        /// <summary>
        /// Gets or sets the quality of service level.
        /// </summary>
        public int QosLevel { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether to set the retained flag when the item published.
        /// </summary>
        public bool Retain { get; set; } = false;
        
        
        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            CnlNum = xmlElem.GetAttrAsInt("cnlNum");
            Topic = xmlElem.GetAttrAsString("topic");
            Publish = xmlElem.GetAttrAsBool("publish");
            Subscribe = xmlElem.GetAttrAsBool("subscribe");
            QosLevel = xmlElem.GetAttrAsInt("qosLevel");
            Retain = xmlElem.GetAttrAsBool("retain");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("cnlNum", CnlNum);
            xmlElem.SetAttribute("topic", Topic);
            xmlElem.SetAttribute("publish", Publish);
            xmlElem.SetAttribute("subscribe", Subscribe);
            xmlElem.SetAttribute("qosLevel", QosLevel);
            xmlElem.SetAttribute("retain", Retain);
        }
    }
}
