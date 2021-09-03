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
 * Module   : ScadaAdminCommon
 * Summary  : Represents Administrator configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

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
            SetToDefault();
        }


        /// <summary>
        /// Gets the codes of the enabled extensions.
        /// </summary>
        public List<string> ExtensionCodes { get; private set; }

        /// <summary>
        /// Gets the associations between file extensions and editors.
        /// </summary>
        public SortedList<string, string> FileAssociations { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            ExtensionCodes = new List<string>();
            FileAssociations = new SortedList<string, string>
            {
                { "sch", @"C:\SCADA\ScadaSchemeEditor\ScadaSchemeEditor.exe" },
                { "tbl", @"C:\SCADA\ScadaTableEditor\ScadaTableEditor.exe" }
            };
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

                XmlDocument xmlDoc = new();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                HashSet<string> extensionCodes = new();

                if (rootElem.SelectSingleNode("Extensions") is XmlNode modulesNode)
                {
                    foreach (XmlElement moduleElem in modulesNode.SelectNodes("Extension"))
                    {
                        string moduleCode = ScadaUtils.RemoveFileNameSuffixes(moduleElem.GetAttribute("code"));

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

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, CommonPhrases.LoadAppConfigError);
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

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, CommonPhrases.SaveAppConfigError);
                return false;
            }
        }
    }
}
