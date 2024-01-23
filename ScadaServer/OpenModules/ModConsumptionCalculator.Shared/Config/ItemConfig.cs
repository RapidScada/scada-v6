// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Server.Modules.ModConsumptionCalculator.Config
{
    /// <summary>
    /// Represents a configuration of a calculated item.
    /// <para>Представляет конфигурацию вычисляемого элемента.</para>
    /// </summary>
    [Serializable]
    internal class ItemConfig
    {
        /// <summary>
        /// Gets or sets the source channel number.
        /// </summary>
        public int SrcCnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the destination channel number.
        /// </summary>
        public int DestCnlNum { get; set; } = 0;


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
