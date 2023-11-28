// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Config
{
    /// <summary>
    /// Represents device options.
    /// <para>Представляет параметры устройства.</para>
    /// </summary>
    [Serializable]
    public class DeviceOptions
    {
        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        [DisplayName, Category, Description]
        public string ProjectId { get; set; } = "";

        /// <summary>
        /// 报文最长时限
        /// </summary>
        [DisplayName, Category, Description]
        public int MaximumTimeLimit { get; set; } = 5;


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ProjectId = xmlNode.GetChildAsString("ProjectId");
            MaximumTimeLimit = xmlNode.GetChildAsInt("MaximumTimeLimit");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("ProjectId", ProjectId);
            xmlElem.AppendElem("MaximumTimeLimit", MaximumTimeLimit);
        }
    }
}
