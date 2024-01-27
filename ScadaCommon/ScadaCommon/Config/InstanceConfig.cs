/*
 * Copyright 2024 Rapid Software LLC
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
 * Modified : 2023
 */

using Scada.Dbms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Config
{
    /// <summary>
    /// Represents an instance configuration.
    /// <para>Представляет конфигурацию экземпляра.</para>
    /// </summary>
    public class InstanceConfig : IConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaInstanceConfig.xml";
        /// <summary>
        /// The default storage code.
        /// </summary>
        public const string DefaultStorageCode = "FileStorage";


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
        /// Gets the root directory of log files.
        /// </summary>
        /// <remarks>An empty value is equivalent to the instance directory.</remarks>
        public string LogDir { get; set; }

        /// <summary>
        /// Gets or sets the code of the active storage.
        /// </summary>
        public string ActiveStorage { get; set; }

        /// <summary>
        /// Gets the default database connection options.
        /// </summary>
        public DbConnectionOptions Connection { get; private set; }

        /// <summary>
        /// Gets the storage configurations accessed by storage code.
        /// </summary>
        public SortedList<string, XmlElement> Storages { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            Culture = Locale.DefaultCulture.Name;
            LogDir = "";
            ActiveStorage = DefaultStorageCode;
            Connection = new DbConnectionOptions();
            Storages = new SortedList<string, XmlElement>();
        }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;
                
                Culture = rootElem.GetChildAsString("Culture");
                LogDir = rootElem.GetChildAsString("LogDir");
                ActiveStorage = rootElem.GetChildAsString("ActiveStorage");

                if (rootElem.SelectSingleNode("Connection") is XmlNode connectionNode)
                    Connection.LoadFromXml(connectionNode);

                if (rootElem.SelectSingleNode("Storages") is XmlNode storagesNode)
                {
                    foreach (XmlElement storageElem in storagesNode.SelectNodes("Storage"))
                    {
                        string storageCode = storageElem.GetAttrAsString("code");
                        Storages[storageCode] = storageElem;
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
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
                rootElem.AppendElem("LogDir", LogDir);
                rootElem.AppendElem("ActiveStorage", ActiveStorage);
                Connection.SaveToXml(rootElem.AppendElem("Connection"));

                XmlElement storagesElem = rootElem.AppendElem("Storages");
                foreach (XmlElement storageElem in Storages.Values)
                {
                    XmlNode newStorageNode = xmlDoc.ImportNode(storageElem, true);
                    storagesElem.AppendChild(newStorageNode);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при сохранении конфигурации экземпляра" :
                    "Error saving instance configuration");
                return false;
            }
        }

        /// <summary>
        /// Gets the XML node corresponding to the active storage.
        /// </summary>
        public XmlElement GetActiveStorageXml()
        {
            return Storages.TryGetValue(ActiveStorage, out XmlElement xmlElement) 
                ? xmlElement 
                : new XmlDocument().CreateElement("Storage");
        }

        /// <summary>
        /// Gets the instance configuration file name.
        /// </summary>
        public static string GetConfigFileName(string instanceDir)
        {
            return Path.Combine(instanceDir, "Config", DefaultFileName);
        }
    }
}
