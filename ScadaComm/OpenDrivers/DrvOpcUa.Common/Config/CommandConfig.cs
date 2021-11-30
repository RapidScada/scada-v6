// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents a command configuration.
    /// <para>Представляет конфигурацию команды.</para>
    /// </summary>
    public class CommandConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommandConfig()
        {
            NodeID = "";
            DisplayName = "";
            DataTypeName = "";
            CmdNum = 0;
        }


        /// <summary>
        /// Gets or sets the OPC UA node ID.
        /// </summary>
        public string NodeID { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the data type name.
        /// </summary>
        public string DataTypeName { get; set; }

        /// <summary>
        /// Gets or sets the command number.
        /// </summary>
        public int CmdNum { get; set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            NodeID = xmlElem.GetAttrAsString("nodeID");
            DisplayName = xmlElem.GetAttrAsString("displayName");
            DataTypeName = xmlElem.GetAttrAsString("dataType");
            CmdNum = xmlElem.GetAttrAsInt("cmdNum");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("nodeID", NodeID);
            xmlElem.SetAttribute("displayName", DisplayName);
            xmlElem.SetAttribute("dataType", DataTypeName);
            xmlElem.SetAttribute("cmdNum", CmdNum);
        }
    }
}
