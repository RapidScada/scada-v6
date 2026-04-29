// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents a configuration of an item writing command.
    /// <para>Представляет конфигурацию команды записи элемента.</para>
    /// </summary>
    public class WriteItemCommandConfig : CommandConfig
    {
        /// <summary>
        /// Gets the command type.
        /// </summary>
        public override CommandType CmdType => CommandType.WriteItem;

        /// <summary>
        /// Gets or sets the OPC node ID.
        /// </summary>
        public string NodeID { get; set; } = "";

        /// <summary>
        /// Gets or sets the data type name of an OPC variable.
        /// </summary>
        public string DataTypeName { get; set; } = "";


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlElement xmlElem)
        {
            base.LoadFromXml(xmlElem);
            NodeID = xmlElem.GetAttrAsString("nodeID");
            DataTypeName = xmlElem.GetAttrAsString("dataType");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.SetAttribute("nodeID", NodeID);
            xmlElem.SetAttribute("dataType", DataTypeName);
        }
    }
}
