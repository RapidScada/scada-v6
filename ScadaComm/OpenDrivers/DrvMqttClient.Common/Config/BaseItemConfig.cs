// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.ComponentModel;
using System.Xml;

namespace Scada.Comm.Drivers.DrvMqttClient.Config
{
    /// <summary>
    /// Represents an item configuration, common to subscription and command.
    /// <para>Представляет конфигурацию элемента, общую для подписки и команды.</para>
    /// </summary>
    [Serializable]
    public class BaseItemConfig : ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseItemConfig()
        {
            Topic = "";
            DisplayName = "";
            QosLevel = 0;
            Retain = false;
            Parent = null;
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
        /// Gets or sets the quality of service level.
        /// </summary>
        public int QosLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to set the retained flag when publishing.
        /// </summary>
        public bool Retain { get; set; }

        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        [Browsable(false)]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        [Browsable(false)]
        public IList Children => null;


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Topic = xmlElem.GetAttrAsString("topic");
            DisplayName = xmlElem.GetAttrAsString("displayName");
            QosLevel = xmlElem.GetAttrAsInt("qosLevel");
            Retain = xmlElem.GetAttrAsBool("retain");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("topic", Topic);
            xmlElem.SetAttribute("displayName", DisplayName);
            xmlElem.SetAttribute("qosLevel", QosLevel);
            xmlElem.SetAttribute("retain", Retain);
        }
    }
}
