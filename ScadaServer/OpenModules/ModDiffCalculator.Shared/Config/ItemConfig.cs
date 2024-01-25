// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Server.Modules.ModDiffCalculator.Config
{
    /// <summary>
    /// Represents a configuration of a calculated item.
    /// <para>Представляет конфигурацию вычисляемого элемента.</para>
    /// </summary>
    [Serializable]
    internal class ItemConfig : ITreeNode
    {
        /// <summary>
        /// Gets or sets the source channel number from which data for calculation is taken.
        /// </summary>
        [DisplayName, Category, Description]
        public int SrcCnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the destination channel number into which a calculation result is written.
        /// </summary>
        [DisplayName, Category, Description]
        public int DestCnlNum { get; set; } = 0;

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
        public IList Children => null;


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            SrcCnlNum = xmlElem.GetAttrAsInt("srcCnlNum");
            DestCnlNum = xmlElem.GetAttrAsInt("destCnlNum");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("srcCnlNum", SrcCnlNum);
            xmlElem.SetAttribute("destCnlNum", DestCnlNum);
        }
    }
}
