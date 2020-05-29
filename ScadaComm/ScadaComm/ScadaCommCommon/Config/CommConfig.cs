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
 * Module   : ScadaCommCommon
 * Summary  : Represents Communicator configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2020
 */

using Scada.Client;
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
        /// Gets the connection options.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; private set; }

        /// <summary>
        /// Gets the configuration of the lines.
        /// </summary>
        public List<LineConfig> Lines { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            GeneralOptions = new GeneralOptions();
            ConnectionOptions = new ConnectionOptions();
            Lines = new List<LineConfig>();
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

                if (rootElem.SelectSingleNode("ConnectionOptions") is XmlNode connectionOptionsNode)
                    ConnectionOptions.LoadFromXml(connectionOptionsNode);

                if (rootElem.SelectSingleNode("Lines") is XmlNode linesNode)
                {
                    foreach (XmlElement lineElem in linesNode.SelectNodes("Line"))
                    {
                        LineConfig lineConfig = new LineConfig();
                        lineConfig.LoadFromXml(lineElem);
                        Lines.Add(lineConfig);
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
                ConnectionOptions.SaveToXml(rootElem.AppendElem("ConnectionOptions"));

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
    }
}
