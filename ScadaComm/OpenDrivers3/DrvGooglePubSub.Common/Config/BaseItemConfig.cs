// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Config
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
            SubscriptionId = "";
            DisplayName = "";
            Parent = null;
        }


        /// <summary>
        /// Gets or sets the MQTT topic.
        /// </summary>
        [DisplayName, Category, Description]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [DisplayName, Category, Description]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        [NCM.Browsable(false)]
        [field: NonSerialized]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        [NCM.Browsable(false)]
        public IList Children => null;


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            SubscriptionId = xmlElem.GetAttrAsString("subscriptionId");
            DisplayName = xmlElem.GetAttrAsString("displayName");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("subscriptionId", SubscriptionId);
            xmlElem.SetAttribute("displayName", DisplayName);
        }
    }
}
