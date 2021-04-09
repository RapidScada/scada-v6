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
 * Summary  : Represents an instance configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Lang;
using System;
using System.IO;
using System.Xml;

namespace Scada.Config
{
    /// <summary>
    /// Represents an instance configuration.
    /// <para>Представляет конфигурацию экземпляра.</para>
    /// </summary>
    public class InstanceConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaInstanceConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public InstanceConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the culture name.
        /// </summary>
        public string Culture { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            Culture = Locale.DefaultCulture.Name;
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
                Culture = xmlDoc.DocumentElement.GetChildAsString("Culture");

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, Locale.IsRussian ?
                    "Ошибка при загрузке конфигурации экземпляра" :
                    "Error loading instance configuration");
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

                XmlElement rootElem = xmlDoc.CreateElement("ScadaInstanceConfig");
                xmlDoc.AppendChild(rootElem);
                rootElem.AppendElem("Culture", Culture);

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, Locale.IsRussian ?
                    "Ошибка при сохранении конфигурации экземпляра" :
                    "Error saving instance configuration");
                return false;
            }
        }
    }
}
