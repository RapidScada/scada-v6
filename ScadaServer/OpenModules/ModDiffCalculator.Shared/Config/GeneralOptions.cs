// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Xml;

namespace Scada.Server.Modules.ModDiffCalculator.Config
{
    /// <summary>
    /// Represents general options of the module.
    /// <para>Представляет основные параметры модуля.</para>
    /// </summary>
    [Serializable]
    internal class GeneralOptions
    {
        /// <summary>
        /// Gets or sets the command code to control the module.
        /// </summary>
        [DisplayName, Category, Description]
        public string CmdCode { get; set; } = "";
        
        
        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            CmdCode = xmlNode.GetChildAsString("CmdCode");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("CmdCode", CmdCode);
        }
    }
}
