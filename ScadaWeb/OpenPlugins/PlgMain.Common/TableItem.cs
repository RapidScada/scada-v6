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
            Text = "";
            Hidden = false;
            Cnl = null;
        }


        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is hidden, but affects event filtering.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets the channel properties.
        /// </summary>
        public Cnl Cnl { get; set; }


        /// <summary>
        /// Loads the item from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            CnlNum = xmlElem.GetAttrAsInt("cnlNum");
            Text = xmlElem.InnerText;
            Hidden = xmlElem.GetAttrAsBool("hidden");
        }

        /// <summary>
        /// Saves the item into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("cnlNum", CnlNum);
            xmlElem.InnerText = Text;
            xmlElem.SetAttribute("hidden", Hidden);
        }
    }
}
