// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
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
        /// The parent list containing this command.
        /// </summary>
        [NonSerialized]
        private CommandList parentList;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommandConfig()
            : base()
        {
            CmdCode = "";
        }


        /// <summary>
        /// Gets or sets the command code.
        /// </summary>
        public string CmdCode { get; set; }

        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        public ITreeNode Parent
        {
            get
            {
                return parentList;
            }
            set
            {
                parentList = value == null ? null : (CommandList)value;
            }
        }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        public IList Children
        {
            get
            {
                return null;
            }
        }


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
