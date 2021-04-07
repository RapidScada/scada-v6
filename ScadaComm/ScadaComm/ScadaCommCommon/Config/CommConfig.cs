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
 * Module   : ScadaCommCommon
 * Summary  : Represents Communicator configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2020
 */

using Scada.Client;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents Communicator configuration.
    /// <para>Представляет конфигурацию Коммуникатора.</para>
    /// </summary>
    public class CommConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaCommConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; private set; }

        /// <summary>
        /// Gets the connection options accessed by connection name.
        /// </summary>
        public SortedList<string, ConnectionOptions> Connections { get; private set; }

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
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            GeneralOptions = new GeneralOptions();
            Connections = new SortedList<string, ConnectionOptions>();
            DataSources = new List<DataSourceConfig>();
            Lines = new List<LineConfig>();
            DriverCodes = new List<string>();
        }

        /// <summary>
        /// Fills the list of driver codes.
        /// </summary>
        private void FillDriverCodes()
        {
            HashSet<string> driverCodes = new HashSet<string>();

            foreach (DataSourceConfig dataSourceConfig in DataSources)
            {
                if (dataSourceConfig.Active && driverCodes.Add(dataSourceConfig.Driver.ToLowerInvariant()))
                    DriverCodes.Add(dataSourceConfig.Driver);
            }

            foreach (LineConfig lineConfig in Lines)
            {
                foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
                {
                    if (deviceConfig.Active && driverCodes.Add(deviceConfig.Driver.ToLowerInvariant()))
                        DriverCodes.Add(deviceConfig.Driver);
                }
            }

            DriverCodes.Sort();
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

                if (xmlDoc.DocumentElement.SelectSingleNode("Connections") is XmlNode connectionsNode)
                {
                    foreach (XmlNode connectionNode in connectionsNode.SelectNodes("Connection"))
                    {
                        ConnectionOptions connectionOptions = new ConnectionOptions();
                        connectionOptions.LoadFromXml(connectionNode);
                        Connections[connectionOptions.Name] = connectionOptions;
                    }
                }

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
                        LineConfig lineConfig = new LineConfig();
                        lineConfig.LoadFromXml(lineElem);
                        Lines.Add(lineConfig);
                    }
                }

                FillDriverCodes();
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

                XmlElement connectionsElem = rootElem.AppendElem("Connections");
                foreach (ConnectionOptions connectionOptions in Connections.Values)
                {
                    connectionOptions.SaveToXml(connectionsElem.AppendElem("Connection"));
                }

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

        /// <summary>
        /// Loads the communication line configuration from the specified file.
        /// </summary>
        public static bool LoadLineConfig(string fileName, int commLineNum, out LineConfig lineConfig, out string errMsg)
        {
            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
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
                errMsg = CommonPhrases.LoadAppConfigError + ": " + ex.Message;
                return false;
            }
        }
    }
}
