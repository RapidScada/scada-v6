// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents a configuration of a method calling command.
    /// <para>Представляет конфигурацию команды вызова метода.</para>
    /// </summary>
    public class CallMethodCommandConfig : CommandConfig
    {
        /// <summary>
        /// Gets the command type.
        /// </summary>
        public override CommandType CmdType => CommandType.CallMethod;

        /// <summary>
        /// Gets or sets the OPC node ID.
        /// </summary>
        public string NodeID { get; set; } = "";

        /// <summary>
        /// Gets or sets the ID of the parent OPC node.
        /// </summary>
        public string ParentNodeID { get; set; } = "";


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlElement xmlElem)
        {
            base.LoadFromXml(xmlElem);
            NodeID = xmlElem.GetAttrAsString("nodeID");
            ParentNodeID = xmlElem.GetAttrAsString("parentNodeID");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.SetAttribute("nodeID", NodeID);
            xmlElem.SetAttribute("parentNodeID", ParentNodeID);
        }
    }
}
