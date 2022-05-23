/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents Server configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2021
 */

using Scada.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Server.Config
{
    /// <summary>
    /// Represents Server configuration.
    /// <para>Представляет конфигурацию Сервера.</para>
    /// </summary>
    [Serializable]
    public class ServerConfig : ConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaServerConfig.xml";


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; private set; }

        /// <summary>
        /// Gets the listener options.
        /// </summary>
        public ListenerOptions ListenerOptions { get; private set; }

        /// <summary>
        /// Gets the codes of the enabled modules.
        /// </summary>
        public List<string> ModuleCodes { get; private set; }

        /// <summary>
        /// Gets the configuration of the archives.
        /// </summary>
        public List<ArchiveConfig> Archives { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            GeneralOptions = new GeneralOptions();
            ListenerOptions = new ListenerOptions();
            ModuleCodes = new List<string>();
            Archives = new List<ArchiveConfig>();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("GeneralOptions") is XmlNode generalOptionsNode)
                GeneralOptions.LoadFromXml(generalOptionsNode);

            if (rootElem.SelectSingleNode("ListenerOptions") is XmlNode listenerOptionsNode)
                ListenerOptions.LoadFromXml(listenerOptionsNode);

            HashSet<string> moduleCodeSet = new HashSet<string>();

            void AddModuleCode(string moduleCode)
            {
                if (!string.IsNullOrEmpty(moduleCode) && moduleCodeSet.Add(moduleCode.ToLowerInvariant()))
                    ModuleCodes.Add(moduleCode);
            }

            if (rootElem.SelectSingleNode("Modules") is XmlNode modulesNode)
            {
                foreach (XmlElement moduleElem in modulesNode.SelectNodes("Module"))
                {
                    string moduleCode = ScadaUtils.RemoveFileNameSuffixes(moduleElem.GetAttribute("code"));
                    AddModuleCode(moduleCode);
                }
            }

            if (rootElem.SelectSingleNode("Archives") is XmlNode archivesNode)
            {
                foreach (XmlElement archiveElem in archivesNode.SelectNodes("Archive"))
                {
                    ArchiveConfig archiveConfig = new ArchiveConfig();
                    archiveConfig.LoadFromXml(archiveElem);
                    Archives.Add(archiveConfig);

                    if (archiveConfig.Active)
                        AddModuleCode(archiveConfig.Module);
                }
            }
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ScadaServerConfig");
            xmlDoc.AppendChild(rootElem);

            GeneralOptions.SaveToXml(rootElem.AppendElem("GeneralOptions"));
            ListenerOptions.SaveToXml(rootElem.AppendElem("ListenerOptions"));

            XmlElement modulesElem = rootElem.AppendElem("Modules");
            foreach (string moduleCode in ModuleCodes)
            {
                modulesElem.AppendElem("Module").SetAttribute("code", moduleCode);
            }

            XmlElement archivesElem = rootElem.AppendElem("Archives");
            foreach (ArchiveConfig archiveConfig in Archives)
            {
                archiveConfig.SaveToXml(archivesElem.AppendElem("Archive"));
            }

            xmlDoc.Save(writer);
        }
    }
}
