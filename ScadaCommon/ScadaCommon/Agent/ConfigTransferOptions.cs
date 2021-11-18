/*
 * Copyright 2021 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : Represents the configuration transfer options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Agent
{
    /// <summary>
    /// Represents the configuration transfer options.
    /// <para>Представляет параметры передачи конфигурации.</para>
    /// </summary>
    public class ConfigTransferOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigTransferOptions()
        {
            ConfigParts = ConfigParts.None;
            IgnoredPaths = new List<RelativePath>();
        }


        /// <summary>
        /// Gets or sets the configuration parts.
        /// </summary>
        public ConfigParts ConfigParts { get; set; }

        /// <summary>
        /// Gets the ignored paths.
        /// </summary>
        public ICollection<RelativePath> IgnoredPaths { get; }



        /// <summary>
        /// Loads the options from the specified reader.
        /// </summary>
        public void Load(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("ConfigParts") is XmlNode configPartsNode)
            {
                if (configPartsNode.GetChildAsBool("Base"))
                    ConfigParts |= ConfigParts.Base;

                if (configPartsNode.GetChildAsBool("View"))
                    ConfigParts |= ConfigParts.View;

                if (configPartsNode.GetChildAsBool("Server"))
                    ConfigParts |= ConfigParts.Server;

                if (configPartsNode.GetChildAsBool("Comm"))
                    ConfigParts |= ConfigParts.Comm;

                if (configPartsNode.GetChildAsBool("Web"))
                    ConfigParts |= ConfigParts.Web;
            }

            if (rootElem.SelectSingleNode("IgnoredPaths") is XmlNode ignoredPathsNode)
            {
                foreach (XmlElement pathElem in ignoredPathsNode.SelectNodes("Path"))
                {
                    IgnoredPaths.Add(new RelativePath
                    {
                        TopFolder = pathElem.GetAttrAsEnum<TopFolder>("topFolder"),
                        AppFolder = pathElem.GetAttrAsEnum<AppFolder>("appFolder"),
                        Path = pathElem.GetAttrAsString("path")
                    });
                }
            }
        }

        /// <summary>
        /// Saves the options to the specified writer.
        /// </summary>
        public void Save(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ConfigTransferOptions");
            xmlDoc.AppendChild(rootElem);

            XmlElement configPartsElem = rootElem.AppendElem("ConfigParts");
            configPartsElem.AppendElem("Base", ConfigParts.HasFlag(ConfigParts.Base));
            configPartsElem.AppendElem("View", ConfigParts.HasFlag(ConfigParts.View));
            configPartsElem.AppendElem("Server", ConfigParts.HasFlag(ConfigParts.Server));
            configPartsElem.AppendElem("Comm", ConfigParts.HasFlag(ConfigParts.Comm));
            configPartsElem.AppendElem("Web", ConfigParts.HasFlag(ConfigParts.Web));

            XmlElement ignoredPathsElem = rootElem.AppendElem("IgnoredPaths");
            foreach (RelativePath path in IgnoredPaths)
            {
                XmlElement pathElem = ignoredPathsElem.AppendElem("Path");
                pathElem.SetAttribute("topFolder", path.TopFolder);
                pathElem.SetAttribute("appFolder", path.AppFolder);
                pathElem.SetAttribute("path", path.Path);
            }

            xmlDoc.Save(writer);
        }
    }
}
