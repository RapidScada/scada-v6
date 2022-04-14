// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Xml;

namespace Scada.Comm.Drivers.DrvMqttClient.Config
{
    /// <summary>
    /// Represents a command configuration.
    /// <para>Представляет конфигурацию команды.</para>
    /// </summary>
    [Serializable]
    public class CommandConfig : BaseItemConfig, ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommandConfig()
            : base()
        {
            CmdCode = "";
        }


        /// <summary>
        /// Gets or sets the command code associated with the topic.
        /// </summary>
        [DisplayName, Category, Description]
        public string CmdCode { get; set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlElement xmlElem)
        {
            base.LoadFromXml(xmlElem);
            CmdCode = xmlElem.GetAttrAsString("cmdCode");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.SetAttribute("cmdCode", CmdCode);
        }
    }
}
