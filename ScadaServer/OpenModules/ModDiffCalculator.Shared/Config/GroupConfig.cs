// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using Scada.Data.Models;
using Scada.Lang;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

#if WINFORMS
using Scada.Forms.ComponentModel;
using System.Drawing.Design;
#endif

namespace Scada.Server.Modules.ModDiffCalculator.Config
{
    /// <summary>
    /// Represents a configuration of a group of calculated items.
    /// <para>Представляет конфигурацию группы вычисляемых элементов.</para>
    /// </summary>
    [Serializable]
    internal class GroupConfig : ITreeNode, IConfigDatasetAccessor
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
        /// Gets the display name.
        /// </summary>
        [NCM.Browsable(false)]
        public string DisplayName => string.IsNullOrEmpty(Name)
            ? (Locale.IsRussian ? "Безымянная" : "Unnamed")
            : Name;

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
        /// Gets or sets the calculation time offset.
        /// </summary>
        [DisplayName, Category, Description]
        public TimeSpan Offset { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Gets or sets the delay before calculation when it is time to calculate, seconds.
        /// </summary>
        [DisplayName, Category, Description]
        public int Delay { get; set; } = 10;

        /// <summary>
        /// Gets or sets the bit number of the archive for reading and writing data.
        /// </summary>
        #region Attributes
        [DisplayName, Category, Description]
#if WINFORMS
        [NCM.Editor(typeof(ArchiveBitEditor), typeof(UITypeEditor))]
#endif
        #endregion
        public int ArchiveBit { get; set; } = 0;

        /// <summary>
        /// Gets the configuration of the calculated items.
        /// </summary>
        [DisplayName, Category, Description]
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
        /// Gets the configuration database.
        /// </summary>
        [NCM.Browsable(false)]
        public ConfigDataset ConfigDataset => ModuleUtils.ConfigDataset;


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Active = xmlElem.GetAttrAsBool("active");
            Name = xmlElem.GetAttrAsString("name");
            PeriodType = xmlElem.GetAttrAsEnum("periodType", PeriodType);
            CustomPeriod = xmlElem.GetAttrAsTimeSpan("customPeriod");
            Offset = xmlElem.GetAttrAsTimeSpan("offset");
            Delay = xmlElem.GetAttrAsInt("delay", Delay);
            ArchiveBit = xmlElem.GetAttrAsInt("archiveBit");

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
            xmlElem.SetAttribute("archiveBit", ArchiveBit);

            foreach (ItemConfig itemConfig in Items)
            {
                itemConfig.SaveToXml(xmlElem.AppendElem("Item"));
            }
        }
    }
}
