// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using System.Collections;
using System.Xml;

namespace Scada.Server.Modules.ModDeviceAlarm.Config
{
    /// <summary>
    /// Represents an export target configuration.
    /// <para>Представляет конфигурацию цели экспорта.</para>
    /// </summary>
    [Serializable]
    internal class ExportTargetConfig : ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExportTargetConfig() 
        {
            GeneralOptions = new GeneralOptions();
            Triggers = new TriggerOptionList();
            Parent = null;
        }


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; }

        /// <summary>
        /// Gets the list of query options.
        /// </summary>
        public TriggerOptionList Triggers { get; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        public IList Children
        {
            get
            {
                return Triggers;
            }
        }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));

            if (xmlElem.SelectSingleNode("GeneralOptions") is XmlNode generalOptionsNode)
                GeneralOptions.LoadFromXml(generalOptionsNode);

            if (xmlElem.SelectSingleNode("Triggers") is XmlNode queriesNode)
                Triggers.LoadFromXml(queriesNode);
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            GeneralOptions.SaveToXml(xmlElem.AppendElem("GeneralOptions"));
            Triggers.SaveToXml(xmlElem.AppendElem("Triggers"));
        }
    }
}
