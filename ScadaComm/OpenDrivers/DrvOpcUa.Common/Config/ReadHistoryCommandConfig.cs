// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents a configuration of a history reading command.
    /// <para>Представляет конфигурацию команды чтения истории.</para>
    /// </summary>
    public class ReadHistoryCommandConfig : CommandConfig
    {
        /// <summary>
        /// Gets the command type.
        /// </summary>
        public override CommandType CmdType => CommandType.ReadHistory;

        /// <summary>
        /// Gets or sets the number of values per node in a read operation.
        /// </summary>
        public int ValuesPerNode { get; set; } = 1000;

        /// <summary>
        /// Gets the nodes to read.
        /// </summary>
        public List<string> NodeIDs { get; } = [];


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlElement xmlElem)
        {
            base.LoadFromXml(xmlElem);
            ValuesPerNode = xmlElem.GetAttrAsInt("valuesPerNode", ValuesPerNode);

            foreach (XmlElement itemElem in xmlElem.SelectNodes("Item"))
            {
                string nodeID = itemElem.GetAttrAsString("nodeID");

                if (!string.IsNullOrEmpty(nodeID))
                    NodeIDs.Add(nodeID);
            }
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.SetAttribute("valuesPerNode", ValuesPerNode);

            foreach (string nodeID in NodeIDs)
            {
                xmlElem.AppendElem("Item").SetAttribute("nodeID", nodeID);
            }
        }
    }
}
