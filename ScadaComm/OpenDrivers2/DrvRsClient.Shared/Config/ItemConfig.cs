// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Comm.Drivers.DrvRsClient.Config
{
    /// <summary>
    /// Represents an item configuration.
    /// <para>Представляет конфигурацию элемента.</para>
    /// </summary>
    [Serializable]
    internal class ItemConfig : ITreeNode
    {
        /// <summary>
        /// Gets or sets the channel number corresponding to the item.
        /// </summary>
        [DisplayName, Category, Description]
        public int CnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        [DisplayName, Category, Description]
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether commands are disabled for this item.
        /// </summary>
        [DisplayName, Category, Description, NCM.TypeConverter(typeof(BooleanConverter))]
        public bool ReadOnly { get; set; } = false;

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
