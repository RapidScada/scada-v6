﻿/*
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
 * Module   : ScadaServerCommon
 * Summary  : Represents server configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2021
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Server.Config
{
    /// <summary>
    /// Represents server configuration.
    /// <para>Представляет конфигурацию сервера.</para>
    /// </summary>
    public class ServerConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaServerConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; private set; }

        /// <summary>
        /// Gets the listener options.
        /// </summary>
        public ListenerOptions ListenerOptions { get; private set; }

        /// <summary>
        /// Gets the path options.
        /// </summary>
        public PathOptions PathOptions { get; private set; }

        /// <summary>
        /// Gets the configuration of the archives.
        /// </summary>
        public List<ArchiveConfig> Archives { get; private set; }

        /// <summary>
        /// Gets the codes of the enabled modules.
        /// </summary>
        public List<string> ModuleCodes { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            GeneralOptions = new GeneralOptions();
            ListenerOptions = new ListenerOptions();
            PathOptions = new PathOptions();
            Archives = new List<ArchiveConfig>();
            ModuleCodes = new List<string>();
        }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                if (rootElem.SelectSingleNode("GeneralOptions") is XmlNode generalOptionsNode)
                    GeneralOptions.LoadFromXml(generalOptionsNode);

                if (rootElem.SelectSingleNode("ListenerOptions") is XmlNode listenerOptionsNode)
                    ListenerOptions.LoadFromXml(listenerOptionsNode);

                if (rootElem.SelectSingleNode("PathOptions") is XmlNode pathOptionsNode)
                    PathOptions.LoadFromXml(pathOptionsNode);

                HashSet<string> moduleCodes = new HashSet<string>();

                if (rootElem.SelectSingleNode("Modules") is XmlNode modulesNode)
                {
                    foreach (XmlElement moduleElem in modulesNode.SelectNodes("Module"))
                    {
                        string moduleCode = ScadaUtils.RemoveFileNameSuffixes(moduleElem.GetAttribute("code"));

                        if (moduleCodes.Add(moduleCode.ToLowerInvariant())) // check uniqueness
                            ModuleCodes.Add(moduleCode);
                    }
                }

                if (rootElem.SelectSingleNode("Archives") is XmlNode archivesNode)
                {
                    foreach (XmlElement archiveElem in archivesNode.SelectNodes("Archive"))
                    {
                        ArchiveConfig archiveConfig = new ArchiveConfig();
                        archiveConfig.LoadFromXml(archiveElem);
                        Archives.Add(archiveConfig);

                        if (archiveConfig.Active && moduleCodes.Add(archiveConfig.Module.ToLowerInvariant()))
                            ModuleCodes.Add(archiveConfig.Module);
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.LoadAppConfigError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the configuration to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaServerConfig");
                xmlDoc.AppendChild(rootElem);

                GeneralOptions.SaveToXml(rootElem.AppendElem("GeneralOptions"));
                ListenerOptions.SaveToXml(rootElem.AppendElem("ListenerOptions"));
                PathOptions.SaveToXml(rootElem.AppendElem("PathOptions"));

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

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.SaveAppConfigError + ": " + ex.Message;
                return false;
            }
        }
    }
}
