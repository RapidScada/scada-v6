/*
 * Copyright 2023 Rapid Software LLC
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
 * Module   : ScadaAdminCommon
 * Summary  : Represents Administrator configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Config;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Admin.Config
{
    /// <summary>
    /// Represents Administrator configuration.
    /// <para>Представляет конфигурацию Администратора.</para>
    /// </summary>
    public class AdminConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaAdminConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AdminConfig()
        {
            Clear();
        }


        /// <summary>
        /// Gets the codes of the enabled extensions.
        /// </summary>
        public List<string> ExtensionCodes { get; private set; }

        /// <summary>
        /// Gets the associations between file extensions and editors.
        /// </summary>
        /// <remarks>The period is not included.</remarks>
        public SortedList<string, string> FileAssociations { get; private set; }

        /// <summary>
        /// Gets the channel numbering options.
        /// </summary>
        public ChannelNumberingOptions ChannelNumberingOptions { get; private set; }

        /// <summary>
        /// Gets the groups of custom options.
        /// </summary>
        public SortedList<string, OptionList> CustomOptions { get; private set; }


        /// <summary>
        /// Clears the configuration.
        /// </summary>
        private void Clear()
        {
            ExtensionCodes = new List<string>();
            FileAssociations = new SortedList<string, string>();
            ChannelNumberingOptions = new ChannelNumberingOptions();
            CustomOptions = new SortedList<string, OptionList>();
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        public void SetToDefault(string instanceDir)
        {
            ExtensionCodes = new List<string> 
            {
                "ExtDepAgent",
                "ExtDepPostgreSql",
                "ExtServerConfig",
                "ExtCommConfig",
                "ExtWebConfig",
                "ExtProjectTools",
                "ExtTableEditor"
            };

            FileAssociations = new SortedList<string, string>
            {
                { "sch", Path.Combine(instanceDir, @"ScadaSchemeEditor\ScadaSchemeEditor.exe") }
            };

            CustomOptions = new SortedList<string, OptionList>();
        }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                Clear();

                XmlDocument xmlDoc = new();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                if (rootElem.SelectSingleNode("Extensions") is XmlNode modulesNode)
                {
                    HashSet<string> extensionCodes = new();

                    foreach (XmlElement moduleElem in modulesNode.SelectNodes("Extension"))
                    {
                        string moduleCode = ScadaUtils.RemoveFileNameSuffixes(moduleElem.GetAttribute("code"));
                        if (moduleCode == "ExtSubFolder") EnableSubFolder(true);
                        if (moduleCode == "ExtBitReader") EnableBitReader(true);
                        if (extensionCodes.Add(moduleCode.ToLowerInvariant())) // check uniqueness
                            ExtensionCodes.Add(moduleCode);
                    }
                }

                if (rootElem.SelectSingleNode("FileAssociations") is XmlNode fileAssociationsNode)
                {
                    foreach (XmlElement associationElem in fileAssociationsNode.SelectNodes("Association"))
                    {
                        string ext = associationElem.GetAttrAsString("ext").ToLowerInvariant();
                        string path = associationElem.GetAttrAsString("path");
                        FileAssociations[ext] = path;
                    }
                }

                if (rootElem.SelectSingleNode("ChannelNumberingOptions") is XmlNode channelNumberingOptionsNode)
                    ChannelNumberingOptions.LoadFromXml(channelNumberingOptionsNode);

                if (rootElem.SelectSingleNode("CustomOptions") is XmlNode customOptionsNode)
                {
                    foreach (XmlElement optionGroupElem in customOptionsNode.SelectNodes("OptionGroup"))
                    {
                        OptionList optionList = new();
                        optionList.LoadFromXml(optionGroupElem);
                        CustomOptions[optionGroupElem.GetAttrAsString("name")] = optionList;
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(CommonPhrases.LoadConfigError);
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
                XmlDocument xmlDoc = new();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaAdminConfig");
                xmlDoc.AppendChild(rootElem);

                XmlElement extensionsElem = rootElem.AppendElem("Extensions");
                foreach (string extensionCode in ExtensionCodes)
                {
                    extensionsElem.AppendElem("Extension").SetAttribute("code", extensionCode);
                }

                XmlElement fileAssociationsElem = rootElem.AppendElem("FileAssociations");
                foreach (KeyValuePair<string, string> pair in FileAssociations)
                {
                    XmlElement associationElem = fileAssociationsElem.AppendElem("Association");
                    associationElem.SetAttribute("ext", pair.Key);
                    associationElem.SetAttribute("path", pair.Value);
                }

                ChannelNumberingOptions.SaveToXml(rootElem.AppendElem("ChannelNumberingOptions"));

                XmlElement customOptionsElem = rootElem.AppendElem("CustomOptions");
                foreach (KeyValuePair<string, OptionList> pair in CustomOptions)
                {
                    XmlElement optionGroupElem = customOptionsElem.AppendElem("OptionGroup");
                    optionGroupElem.SetAttribute("name", pair.Key);
                    pair.Value.SaveToXml(optionGroupElem);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(CommonPhrases.SaveConfigError);
                return false;
            }
        }

        /// <summary>
        /// Gets the list of options by the specified group name, or an empty list if the group is missing.
        /// </summary>
        public OptionList GetOptions(string groupName, bool addIfMissing = false)
        {
            if (!CustomOptions.TryGetValue(groupName, out OptionList options))
            {
                options = new OptionList();

                if (addIfMissing)
                    CustomOptions.Add(groupName, options);
            }

            return options;
        }
        public bool subFolderEnabled { get; private set; } = false;

        public bool EnableSubFolder (bool activate)
        {
            subFolderEnabled = activate;

            return activate;
        }

        public bool bitReaderEnabled { get; private set; } = false;

        public bool EnableBitReader(bool activate)
        {
            bitReaderEnabled = activate;
            return activate;
        }
    }
}
