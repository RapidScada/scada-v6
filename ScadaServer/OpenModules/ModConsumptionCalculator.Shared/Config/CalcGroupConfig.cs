// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Server.Modules.ModConsumptionCalculator.Config
{
    /// <summary>
    /// Represents a configuration of a group of calculated items.
    /// <para>Представляет конфигурацию группы вычисляемых элементов.</para>
    /// </summary>
    [Serializable]
    internal class CalcGroupConfig : ITreeNode
    {
        /// <summary>
        /// Gets or sets a value indicating whether the group is active.
        /// </summary>
        [DisplayName, Category, Description]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DisplayName, Category, Description]
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the calculation period type.
        /// </summary>
        [DisplayName, Category, Description]
        public PeriodType PeriodType { get; set; } = PeriodType.Custom;

        /// <summary>
        /// Gets or sets the custom calculation period.
        /// </summary>
        [DisplayName, Category, Description]
        public TimeSpan CustomPeriod { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Gets or sets the calculation offset.
        /// </summary>
        [DisplayName, Category, Description]
        public TimeSpan Offset { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Gets or sets the delay before calculation when it is time to calculate, seconds.
        /// </summary>
        [DisplayName, Category, Description]
        public int Delay { get; set; } = 10;

        /// <summary>
        /// Gets the configuration of the calculated items.
        /// </summary>
        [DisplayName, Category, Description]
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
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Active = xmlElem.GetAttrAsBool("active");
            Name = xmlElem.GetAttrAsString("name");
            PeriodType = xmlElem.GetChildAsEnum("periodType", PeriodType);
            CustomPeriod = xmlElem.GetAttrAsTimeSpan("customPeriod");
            Offset = xmlElem.GetAttrAsTimeSpan("offset");
            Delay = xmlElem.GetAttrAsInt("delay", Delay);

            foreach (XmlElement itemElem in xmlElem.SelectNodes("Item"))
            {
                ItemConfig itemConfig = new() { Parent = this };
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
            xmlElem.SetAttribute("periodType", PeriodType);

            if (PeriodType == PeriodType.Custom)
                xmlElem.SetAttribute("customPeriod", CustomPeriod);

            xmlElem.SetAttribute("offset", Offset);
            xmlElem.SetAttribute("delay", Delay);

            foreach (ItemConfig itemConfig in Items)
            {
                itemConfig.SaveToXml(xmlElem.AppendElem("Item"));
            }
        }
    }
}
