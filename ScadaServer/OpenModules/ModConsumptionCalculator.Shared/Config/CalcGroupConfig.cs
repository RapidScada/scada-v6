// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Server.Modules.ModConsumptionCalculator.Config
{
    /// <summary>
    /// Represents a configuration of a group of calculated items.
    /// <para>Представляет конфигурацию группы вычисляемых элементов.</para>
    /// </summary>
    [Serializable]
    internal class CalcGroupConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the group is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the calculation period.
        /// </summary>
        public TimeSpan Period { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Gets or sets the calculation offset.
        /// </summary>
        public TimeSpan Offset { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Gets or sets the delay before calculation when it is time to calculate, seconds.
        /// </summary>
        public int Delay { get; set; } = 10;

        /// <summary>
        /// Gets the configuration of the calculated items.
        /// </summary>
        public List<ItemConfig> Items { get; } = [];


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Active = xmlElem.GetAttrAsBool("active");
            Name = xmlElem.GetAttrAsString("name");
            Period = xmlElem.GetAttrAsTimeSpan("period");
            Offset = xmlElem.GetAttrAsTimeSpan("offset");
            Delay = xmlElem.GetAttrAsInt("delay", Delay);

            foreach (XmlElement itemElem in xmlElem.SelectNodes("Item"))
            {
                ItemConfig itemConfig = new();
                itemConfig.LoadFromXml(itemElem);
                Items.Add(itemConfig);
            }
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("period", Period);
            xmlElem.SetAttribute("offset", Offset);
            xmlElem.SetAttribute("delay", Delay);

            foreach (ItemConfig itemConfig in Items)
            {
                itemConfig.SaveToXml(xmlElem.AppendElem("Item"));
            }
        }
    }
}
