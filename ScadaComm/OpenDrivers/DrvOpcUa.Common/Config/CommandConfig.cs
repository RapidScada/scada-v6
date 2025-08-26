// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
        /// Gets or sets the OPC node ID.
        /// </summary>
        public string NodeID { get; set; } = "";

        /// <summary>
        /// Gets or sets the ID of the parent OPC node.
        /// </summary>
        public string ParentNodeID { get; set; } = "";

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
        /// Gets or sets a value indicating whether the command calls an OPC method.
        /// </summary>
        public bool IsMethod { get; set; } = false;

        /// <summary>
        /// Gets or sets the data type name of an OPC variable.
        /// </summary>
        public string DataTypeName { get; set; } = "";


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            NodeID = xmlElem.GetAttrAsString("nodeID");
            ParentNodeID = xmlElem.GetAttrAsString("parentNodeID");
            DisplayName = xmlElem.GetAttrAsString("displayName");
            CmdNum = xmlElem.GetAttrAsInt("cmdNum");
            CmdCode = xmlElem.GetAttrAsString("cmdCode");
            IsMethod = xmlElem.GetAttrAsBool("isMethod");
            DataTypeName = xmlElem.GetAttrAsString("dataType");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("nodeID", NodeID);
            xmlElem.SetAttribute("parentNodeID", ParentNodeID);
            xmlElem.SetAttribute("displayName", DisplayName);
            xmlElem.SetAttribute("cmdNum", CmdNum);
            xmlElem.SetAttribute("cmdCode", CmdCode);
            xmlElem.SetAttribute("isMethod", IsMethod);

            if (!IsMethod)
                xmlElem.SetAttribute("dataType", DataTypeName);
        }
    }
}
