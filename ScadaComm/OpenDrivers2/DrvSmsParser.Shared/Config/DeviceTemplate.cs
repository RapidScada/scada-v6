// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada;
using Scada.Config;
using System.Xml;

namespace DrvSmsParser.Shared.Config
{
    /// <summary>
    /// Represents a device template.
    /// <para>Представляет шаблон устройства.</para>
    /// </summary>
    internal class DeviceTemplate : ConfigBase
    {
        /// <summary>
        /// Gets the full tag names.
        /// </summary>
        /// <remarks>The full tag name consists of a tag code and a tag name.</remarks>
        public List<string> Tags { get; private set; }

        /// <summary>
        /// Gets the JavaScript to process incoming messages.
        /// </summary>
        public string Script { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            Tags = [];
            Script = "";
        }

        /// <summary>
        /// Loads the configuration from the XML document.
        /// </summary>
        protected override void LoadFromXml(XmlDocument xmlDoc)
        {
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("Tags") is XmlNode tagsNode)
            {
                foreach (XmlNode tagNode in tagsNode.ChildNodes)
                {
                    Tags.Add(tagNode.InnerText);
                }
            }

            Script = rootElem.GetChildAsString("Script");
        }

        /// <summary>
        /// Saves the configuration into the XML document.
        /// </summary>
        protected override void SaveToXml(XmlDocument xmlDoc)
        {
            XmlElement rootElem = xmlDoc.DocumentElement;
            XmlElement tagsElem = rootElem.AppendElem("Tags");

            foreach (string tag in Tags)
            {
                tagsElem.AppendElem("Tag", tag);
            }

            rootElem.AppendElem("Script", Script);
        }
    }
}
