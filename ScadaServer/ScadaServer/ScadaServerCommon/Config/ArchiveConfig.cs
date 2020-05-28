/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaServerCommon
 * Summary  : Represents an archive configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Server.Archives;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Server.Config
{
    /// <summary>
    /// Represents an archive configuration.
    /// <para>Представляет конфигурацию архива.</para>
    /// </summary>
    public class ArchiveConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveConfig()
        {
            Code = "";
            Name = "";
            Kind = ArchiveKind.Unspecified;
            Options = new SortedList<string, string>();
        }


        /// <summary>
        /// Gets or sets the archive code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the archive name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the archive kind.
        /// </summary>
        public ArchiveKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the name of the DLL that implementats the archive.
        /// </summary>
        public string Dll { get; set; }

        /// <summary>
        /// Gets the archive options accessed by name.
        /// </summary>
        public SortedList<string, string> Options { get; private set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            Code = xmlElem.GetAttrAsString("code");
            Name = xmlElem.GetAttrAsString("name");
            Kind = xmlElem.GetAttrAsEnum("kind", ArchiveKind.Unspecified);
            Dll = ScadaUtils.RemoveFileNameSuffixes(xmlElem.GetAttrAsString("dll"));

            foreach (XmlElement optionElem in xmlElem.SelectNodes("Option"))
            {
                Options[optionElem.GetAttrAsString("name")] = optionElem.GetAttrAsString("value");
            }
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.SetAttribute("code", Code);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("kind", Kind);
            xmlElem.SetAttribute("dll", Dll);

            foreach (KeyValuePair<string, string> pair in Options)
            {
                xmlElem.AppendOptionElem(pair.Key, pair.Value);
            }
        }
    }
}
