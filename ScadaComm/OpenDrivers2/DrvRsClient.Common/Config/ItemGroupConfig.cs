// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Comm.Drivers.DrvRsClient.Config
{
    /// <summary>
    /// Represents an item group configuration.
    /// <para>Представляет конфигурацию группы элементов.</para>
    /// </summary>
    [Serializable]
    public class ItemGroupConfig : ITreeNode
    {
        /// <summary>
        /// Gets or sets a value indicating whether the group is active.
        /// </summary>
        [DisplayName, Category, Description, NCM.TypeConverter(typeof(BooleanConverter))]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        [DisplayName, Category, Description]
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets the items.
        /// </summary>
        [NCM.Browsable(false)]
        public List<ItemConfig> Items { get; } = [];

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        [NCM.Browsable(false)]
        [field: NonSerialized]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        [NCM.Browsable(false)]
        public IList Children => Items;


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Active = xmlElem.GetAttrAsBool("active");
            Name = xmlElem.GetAttrAsString("name");

            foreach (XmlElement itemElem in xmlElem.SelectNodes("Item"))
            {
                ItemConfig itemConfig = new() { Parent = this };
                itemConfig.LoadFromXml(itemElem);
                Items.Add(itemConfig);
            }
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("name", Name);

            foreach (ItemConfig itemConfig in Items)
            {
                itemConfig.SaveToXml(xmlElem.AppendElem("Item"));
            }
        }
    }
}
