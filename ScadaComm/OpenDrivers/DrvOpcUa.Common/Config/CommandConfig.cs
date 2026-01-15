// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents a command configuration.
    /// <para>Представляет конфигурацию команды.</para>
    /// </summary>
    public abstract class CommandConfig
    {
        /// <summary>
        /// Gets the command type.
        /// </summary>
        public abstract CommandType CmdType { get; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// Gets or sets the command number.
        /// </summary>
        public int CmdNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the command code.
        /// </summary>
        public string CmdCode { get; set; } = "";


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            DisplayName = xmlElem.GetAttrAsString("displayName");
            CmdNum = xmlElem.GetAttrAsInt("cmdNum");
            CmdCode = xmlElem.GetAttrAsString("cmdCode");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("cmdType", CmdType);
            xmlElem.SetAttribute("displayName", DisplayName);
            xmlElem.SetAttribute("cmdNum", CmdNum);
            xmlElem.SetAttribute("cmdCode", CmdCode);
        }
    }
}
