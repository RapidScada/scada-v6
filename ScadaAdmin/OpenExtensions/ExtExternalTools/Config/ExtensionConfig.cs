// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.Xml;

namespace Scada.Admin.Extensions.ExtExternalTools.Config
{
    /// <summary>
    /// Represents an extension configuration.
    /// <para>Представляет конфигурацию расширения.</para>
    /// </summary>
    internal class ExtensionConfig : ConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ExtExternalTools.xml";

        /// <summary>
        /// Gets the configuration of the tool items.
        /// </summary>
        public List<ToolItemConfig> ToolItems { get; private set; }
        
        
        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            ToolItems = [];
        }

        /// <summary>
        /// Loads the configuration from the XML document.
        /// </summary>
        protected override void LoadFromXml(XmlDocument xmlDoc)
        {
            foreach (XmlNode toolItemNode in xmlDoc.DocumentElement.SelectNodes("ToolItem"))
            {
                ToolItemConfig toolItemConfig = new();
                toolItemConfig.LoadFromXml(toolItemNode);
                ToolItems.Add(toolItemConfig);
            }
        }

        /// <summary>
        /// Saves the configuration into the XML document.
        /// </summary>
        protected override void SaveToXml(XmlDocument xmlDoc)
        {
            XmlElement rootElem = xmlDoc.CreateElement("ExtExternalTools");
            xmlDoc.AppendChild(rootElem);

            foreach (ToolItemConfig toolItemConfig in ToolItems)
            {
                toolItemConfig.SaveToXml(rootElem.AppendElem("ToolItem"));
            }
        }
    }
}
