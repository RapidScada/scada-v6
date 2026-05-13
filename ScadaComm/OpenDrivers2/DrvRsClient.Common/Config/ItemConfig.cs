// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvRsClient.Config
{
    /// <summary>
    /// Represents an item configuration.
    /// <para>Представляет конфигурацию элемента.</para>
    /// </summary>
    public class ItemConfig
    {
        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether commands are disabled for this item.
        /// </summary>
        public bool ReadOnly { get; set; } = false;


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            CnlNum = xmlElem.GetAttrAsInt("cnlNum");
            Name = xmlElem.GetAttrAsString("name");
            ReadOnly = xmlElem.GetAttrAsBool("readOnly");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("cnlNum", CnlNum);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("readOnly", ReadOnly);
        }
    }
}
