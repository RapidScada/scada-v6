// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents subscription options applied to a communication line.
    /// <para>Представляет параметры подписки, применяемые к линии связи.</para>
    /// </summary>
    public class LineSubscriptionOptions
    {
        /// <summary>
        /// Gets or sets the the subscription creation mode.
        /// </summary>
        public SubscriptionCreationMode CreationMode { get; set; } = SubscriptionCreationMode.Manual;

        /// <summary>
        /// Gets or sets the format string for building a node ID based on a channel tag.
        /// </summary>
        public string NodeIdFormat { get; set; } = "";

        /// <summary>
        /// Gets or sets the maximum number of items per subscription.
        /// </summary>
        public int MaxItemCount { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the tag naming mode.
        /// </summary>
        public TagNamingMode TagNamingMode { get; set; } = TagNamingMode.NodeID;


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            CreationMode = xmlNode.GetChildAsEnum("CreationMode", CreationMode);
            NodeIdFormat = xmlNode.GetChildAsString("NodeIdFormat", NodeIdFormat);
            MaxItemCount = xmlNode.GetChildAsInt("MaxItemCount", MaxItemCount);
            TagNamingMode = xmlNode.GetChildAsEnum("TagNamingMode", TagNamingMode);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            xmlNode.AppendElem("CreationMode", CreationMode);
            xmlNode.AppendElem("NodeIdFormat", NodeIdFormat);
            xmlNode.AppendElem("MaxItemCount", MaxItemCount);
            xmlNode.AppendElem("TagNamingMode", TagNamingMode);
        }
    }
}
