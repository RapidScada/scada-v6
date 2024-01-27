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
 * Module   : ScadaCommCommon
 * Summary  : Represents general Communicator options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
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
    [Serializable]
    public class GeneralOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GeneralOptions()
        {
            IsBound = true;
            SendModifiedData = true;
            SendAllDataPeriod = 60;
            EnableCommands = true;
            EnableFileCommands = true;
            StartLinesOnCommand = false;
            StopWait = 10;
            MaxLogSize = LogFile.DefaultCapacityMB;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the application is bound to the configuration database.
        /// </summary>
        public bool IsBound { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to send only modified data of device tags.
        /// </summary>
        public bool SendModifiedData { get; set; }

        /// <summary>
        /// Gets or sets the period of sending data of all device tags, sec.
        /// </summary>
        public int SendAllDataPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable telecontrol commands.
        /// </summary>
        public bool EnableCommands { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to read telecontrol commands from files.
        /// </summary>
        public bool EnableFileCommands { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether communication lines start when an appropriate command is received.
        /// </summary>
        public bool StartLinesOnCommand { get; set; }

        /// <summary>
        /// The time to wait for the service to stop, seconds.
        /// </summary>
        public int StopWait { get; set; }

        /// <summary>
        /// Gets or sets the maximum log file size, megabytes.
        /// </summary>
        public int MaxLogSize { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            IsBound = xmlNode.GetChildAsBool("IsBound", IsBound);
            SendModifiedData = xmlNode.GetChildAsBool("SendModifiedData", SendModifiedData);
            SendAllDataPeriod = xmlNode.GetChildAsInt("SendAllDataPeriod", SendAllDataPeriod);
            EnableCommands = xmlNode.GetChildAsBool("EnableCommands", EnableCommands);
            EnableFileCommands = xmlNode.GetChildAsBool("EnableFileCommands", EnableFileCommands);
            StartLinesOnCommand = xmlNode.GetChildAsBool("StartLinesOnCommand", StartLinesOnCommand);
            StopWait = xmlNode.GetChildAsInt("StopWait", StopWait);
            MaxLogSize = xmlNode.GetChildAsInt("MaxLogSize", MaxLogSize);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("IsBound", IsBound);
            xmlElem.AppendElem("SendModifiedData", SendModifiedData);
            xmlElem.AppendElem("SendAllDataPeriod", SendAllDataPeriod);
            xmlElem.AppendElem("EnableCommands", EnableCommands);
            xmlElem.AppendElem("EnableFileCommands", EnableFileCommands);
            xmlElem.AppendElem("StartLinesOnCommand", StartLinesOnCommand);
            xmlElem.AppendElem("StopWait", StopWait);
            xmlElem.AppendElem("MaxLogSize", MaxLogSize);
        }
    }
}
