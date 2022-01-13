// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using System;
using System.Xml;

namespace Scada.Web.Plugins.PlgMain
{
    /// <summary>
    /// Represents a table view item.
    /// <para>Представляет элемент табличного представления.</para>
    /// </summary>
    public class TableItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableItem()
        {
            CnlNum = 0;
            DeviceNum = 0;
            Hidden = false;
            AutoText = false;
            Text = "";
            Cnl = null;
        }


        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets or sets the device number.
        /// </summary>
        public int DeviceNum { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is hidden, but affects event filtering.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the entity name as the item text.
        /// </summary>
        public bool AutoText { get; set; }

        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the channel properties.
        /// </summary>
        public Cnl Cnl { get; set; }


        /// <summary>
        /// Loads the item from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            CnlNum = xmlElem.GetAttrAsInt("cnlNum");
            DeviceNum = xmlElem.GetAttrAsInt("deviceNum");
            Hidden = xmlElem.GetAttrAsBool("hidden");
            AutoText = xmlElem.GetAttrAsBool("autoText");
            Text = xmlElem.InnerText;
        }

        /// <summary>
        /// Saves the item into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            
            if (CnlNum > 0)
                xmlElem.SetAttribute("cnlNum", CnlNum);

            if (DeviceNum > 0)
                xmlElem.SetAttribute("deviceNum", DeviceNum);

            if (Hidden)
                xmlElem.SetAttribute("hidden", Hidden);

            if (AutoText)
                xmlElem.SetAttribute("autoText", AutoText);
            else if (!string.IsNullOrEmpty(Text))
                xmlElem.InnerText = Text;
        }
    }
}
