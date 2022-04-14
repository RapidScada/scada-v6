// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Comm.Drivers.DrvMqttPublisher.Config
{
    /// <summary>
    /// Represents device options.
    /// <para>Представляет параметры устройства.</para>
    /// </summary>
    [Serializable]
    internal class DeviceOptions
    {
        /// <summary>
        /// Gets or sets the root topic used as a prefix for all device topics.
        /// </summary>
        [DisplayName, Category, Description]
        public string RootTopic { get; set; } = "";

        /// <summary>
        /// Gets or sets the payload to send if channel value is undefined.
        /// </summary>
        [DisplayName, Category, Description]
        public string UndefinedValue { get; set; } = "NaN";

        /// <summary>
        /// Gets or sets the format of published channel data.
        /// </summary>
        [DisplayName, Category, Description]
        public string PublishFormat { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether to send channel data when changed.
        /// </summary>
        [DisplayName, Category, Description, NCM.TypeConverter(typeof(BooleanConverter))]
        public bool PublishOnChange { get; set; } = true;

        /// <summary>
        /// Gets or sets the publishing period for all device items, sec.
        /// </summary>
        [DisplayName, Category, Description]
        public int PublishPeriod { get; set; } = 60;


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            RootTopic = xmlNode.GetChildAsString("RootTopic", RootTopic);
            UndefinedValue = xmlNode.GetChildAsString("UndefinedValue", UndefinedValue);
            PublishFormat = xmlNode.GetChildAsString("PublishFormat", PublishFormat);
            PublishOnChange = xmlNode.GetChildAsBool("PublishOnChange", PublishOnChange);
            PublishPeriod = xmlNode.GetChildAsInt("PublishPeriod", PublishPeriod);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("RootTopic", RootTopic);
            xmlElem.AppendElem("UndefinedValue", UndefinedValue);
            xmlElem.AppendElem("PublishFormat", PublishFormat);
            xmlElem.AppendElem("PublishOnChange", PublishOnChange);
            xmlElem.AppendElem("PublishPeriod", PublishPeriod);
        }
    }
}
