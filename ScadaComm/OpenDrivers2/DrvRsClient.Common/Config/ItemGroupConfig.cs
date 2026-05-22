// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvRsClient.Config
{
    public class ItemGroupConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the group is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets the items.
        /// </summary>
        public List<ItemConfig> Items { get; } = [];


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
                ItemConfig itemConfig = new();
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
