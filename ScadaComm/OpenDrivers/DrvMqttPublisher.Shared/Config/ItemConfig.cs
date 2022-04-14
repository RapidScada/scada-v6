// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Comm.Drivers.DrvMqttPublisher.Config
{
    /// <summary>
    /// Represents an item configuration.
    /// <para>Представляет конфигурацию элемента.</para>
    /// </summary>
    [Serializable]
    internal class ItemConfig : ITreeNode
    {
        /// <summary>
        /// Gets or sets the number of the published channel.
        /// </summary>
        [DisplayName, Category, Description]
        public int CnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the MQTT topic.
        /// </summary>
        [DisplayName, Category, Description]
        public string Topic { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether the item is used to publish channel values.
        /// </summary>
        [DisplayName, Category, Description, NCM.TypeConverter(typeof(BooleanConverter))]
        public bool Publish { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the item is used to receive commands.
        /// </summary>
        [DisplayName, Category, Description, NCM.TypeConverter(typeof(BooleanConverter))]
        public bool Subscribe { get; set; } = false;

        /// <summary>
        /// Gets or sets the quality of service level.
        /// </summary>
        [DisplayName, Category, Description]
        public int QosLevel { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether to set the retained flag when publishing.
        /// </summary>
        [DisplayName, Category, Description, NCM.TypeConverter(typeof(BooleanConverter))]
        public bool Retain { get; set; } = false;

        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        [NCM.Browsable(false)]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        [NCM.Browsable(false)]
        public IList Children => null;


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
