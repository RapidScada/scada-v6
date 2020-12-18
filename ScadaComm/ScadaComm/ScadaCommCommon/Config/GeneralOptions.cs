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
 * Summary  : Represents general Communicator options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Log;
using System;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents general Communicator options.
    /// <para>Представляет основные параметры Коммуникатора.</para>
    /// </summary>
    public class GeneralOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GeneralOptions()
        {
            InteractWithServer = true;
            SendModifiedData = true;
            SendAllDataPeriod = 60;
            CmdEnabled = true;
            FileCmdEnabled = true;
            ClientLogEnabled = false;
            MaxLogSize = LogFile.DefaultCapacity;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to interact with the server.
        /// </summary>
        public bool InteractWithServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to send only modified data of device tags.
        /// </summary>
        public bool SendModifiedData { get; set; }

        /// <summary>
        /// Gets or sets the period of sending all device tags, sec.
        /// </summary>
        public int SendAllDataPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether commands are enabled.
        /// </summary>
        public bool CmdEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether file commands are enabled.
        /// </summary>
        public bool FileCmdEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write client communication log.
        /// </summary>
        public bool ClientLogEnabled { get; set; }

        /// <summary>
        /// Gets or sets the maximum log file size.
        /// </summary>
        public int MaxLogSize { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            InteractWithServer = xmlNode.GetChildAsBool("InteractWithServer");
            SendModifiedData = xmlNode.GetChildAsBool("SendModifiedData");
            SendAllDataPeriod = xmlNode.GetChildAsInt("SendAllDataPeriod");
            CmdEnabled = xmlNode.GetChildAsBool("CmdEnabled");
            FileCmdEnabled = xmlNode.GetChildAsBool("FileCmdEnabled");
            ClientLogEnabled = xmlNode.GetChildAsBool("ClientLogEnabled");
            MaxLogSize = xmlNode.GetChildAsInt("MaxLogSize", MaxLogSize);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("InteractWithServer", InteractWithServer);
            xmlElem.AppendElem("SendModifiedData", SendModifiedData);
            xmlElem.AppendElem("SendAllDataPeriod", SendAllDataPeriod);
            xmlElem.AppendElem("CmdEnabled", CmdEnabled);
            xmlElem.AppendElem("FileCmdEnabled", FileCmdEnabled);
            xmlElem.AppendElem("ClientLogEnabled", ClientLogEnabled);
            xmlElem.AppendElem("MaxLogSize", MaxLogSize);
        }
    }
}
