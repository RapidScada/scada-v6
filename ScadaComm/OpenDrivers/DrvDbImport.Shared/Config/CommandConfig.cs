// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvDbImport.Config
{
    /// <summary>
    /// Represents a command configuration.
    /// <para>Представляет конфигурацию команды.</para>
    /// </summary>
    [Serializable]
    public class CommandConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommandConfig()
        {
            Name = "";
            CmdCode = "";
            Sql = "";
        }


        /// <summary>
        /// Gets or sets the command name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the command code.
        /// </summary>
        public string CmdCode { get; set; }

        /// <summary>
        /// Gets or sets the SQL request.
        /// </summary>
        public string Sql { get; set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Name = xmlElem.GetAttrAsString("name");
            CmdCode = xmlElem.GetAttrAsString("cmdCode");
            Sql = xmlElem.GetChildAsString("Sql");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("cmdCode", CmdCode);
            xmlElem.AppendElem("Sql", Sql);
        }
    }
}
