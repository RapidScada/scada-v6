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
 * Module   : ScadaCommCommon
 * Summary  : Represents Communicator configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2022
 */

using Scada.Client;
using Scada.Config;
using Scada.Lang;
using Scada.Storages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents Communicator configuration.
    /// <para>Представляет конфигурацию Коммуникатора.</para>
    /// </summary>
    [Serializable]
    public class CommConfig : ConfigBase, ITreeNode
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaCommConfig.xml";


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; private set; }

        /// <summary>
        /// Gets the connection options.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; private set; }

        /// <summary>
        /// Gets the configuration of the data sources.
        /// </summary>
        public List<DataSourceConfig> DataSources { get; private set; }

        /// <summary>
        /// Gets the configuration of the lines.
        /// </summary>
        public List<LineConfig> Lines { get; private set; }
        
        /// <summary>
        /// Gets the codes of the drivers used.
        /// </summary>
        public List<string> DriverCodes { get; private set; }
        
        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return null;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        IList ITreeNode.Children
        {
            get
            {
                return Lines;
            }
        }


        /// <summary>
        /// Fills the list of driver codes.
        /// </summary>
        private void FillDriverCodes()
        {
            HashSet<string> driverCodeSet = new HashSet<string>();

            void AddDriverCode(string driverCode)
            {
                if (!string.IsNullOrEmpty(driverCode) && driverCodeSet.Add(driverCode.ToLowerInvariant()))
                    DriverCodes.Add(driverCode);
            }

            foreach (DataSourceConfig dataSourceConfig in DataSources)
            {
                if (dataSourceConfig.Active)
                    AddDriverCode(dataSourceConfig.Driver);
            }

            foreach (LineConfig lineConfig in Lines)
            {
                if (lineConfig.Active)
                {
                    AddDriverCode(lineConfig.Channel.Driver);

                    foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
                    {
                        if (deviceConfig.Active)
                            AddDriverCode(deviceConfig.Driver);
                    }
                }
            }

            DriverCodes.Sort();
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            GeneralOptions = new GeneralOptions();
            ConnectionOptions = new ConnectionOptions();
            DataSources = new List<DataSourceConfig>();
            Lines = new List<LineConfig>();
            DriverCodes = new List<string>();
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

            if (rootElem.SelectSingleNode("ConnectionOptions") is XmlNode connectionOptionsNode)
                ConnectionOptions.LoadFromXml(connectionOptionsNode);

            if (rootElem.SelectSingleNode("DataSources") is XmlNode dataSourcesNode)
            {
                foreach (XmlElement dataSourceElem in dataSourcesNode.SelectNodes("DataSource"))
                {
                    DataSourceConfig dataSourceConfig = new DataSourceConfig();
                    dataSourceConfig.LoadFromXml(dataSourceElem);
                    DataSources.Add(dataSourceConfig);
                }
            }

            if (rootElem.SelectSingleNode("Lines") is XmlNode linesNode)
            {
                foreach (XmlElement lineElem in linesNode.SelectNodes("Line"))
                {
                    LineConfig lineConfig = new LineConfig { Parent = this };
                    lineConfig.LoadFromXml(lineElem);
                    Lines.Add(lineConfig);
                }
            }

            FillDriverCodes();
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ScadaCommConfig");
            xmlDoc.AppendChild(rootElem);

            GeneralOptions.SaveToXml(rootElem.AppendElem("GeneralOptions"));
            ConnectionOptions.SaveToXml(rootElem.AppendElem("ConnectionOptions"));

            XmlElement dataSourcesElem = rootElem.AppendElem("DataSources");
            foreach (DataSourceConfig dataSourceConfig in DataSources)
            {
                dataSourceConfig.SaveToXml(dataSourcesElem.AppendElem("DataSource"));
            }

            XmlElement linesElem = rootElem.AppendElem("Lines");
            foreach (LineConfig lineConfig in Lines)
            {
                lineConfig.SaveToXml(linesElem.AppendElem("Line"));
            }

            xmlDoc.Save(writer);
        }

        /// <summary>
        /// Loads the communication line configuration from the specified storage.
        /// </summary>
        public static bool LoadLineConfig(IStorage storage, string fileName, int commLineNum, 
            out LineConfig lineConfig, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(new StringReader(storage.ReadText(DataCategory.Config, fileName)));
                lineConfig = null;

                if (xmlDoc.DocumentElement.SelectSingleNode("Lines") is XmlNode linesNode)
                {
                    foreach (XmlElement lineElem in linesNode.SelectNodes($"Line[@number={commLineNum}]"))
                    {
                        lineConfig = new LineConfig();
                        lineConfig.LoadFromXml(lineElem);
                        break;
                    }
                }

                if (lineConfig == null)
                {
                    errMsg = string.Format(Locale.IsRussian ?
                        "Конфигурация линии связи {0} не найдена" :
                        "Communication line {0} configuration not found", commLineNum);
                    return false;
                }
                else
                {
                    errMsg = "";
                    return true;
                }
            }
            catch (Exception ex)
            {
                lineConfig = null;
                errMsg = ex.BuildErrorMessage(CommonPhrases.LoadConfigError);
                return false;
            }
        }
    }
}
