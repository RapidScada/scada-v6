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
 * Summary  : Represents communication line options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents communication line options.
    /// <para>Представляет параметры линии связи.</para>
    /// </summary>
    [Serializable]
    public class LineOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LineOptions()
        {
            ReqRetries = 3;
            CycleDelay = 0;
            CmdEnabled = true;
            PollAfterCmd = true;
            DetailedLog = true;
        }


        /// <summary>
        /// Gets or sets the number of retries of the request in case of an error.
        /// </summary>
        public int ReqRetries { get; set; }

        /// <summary>
        /// Gets or sets the delay after polling cycle, ms
        /// </summary>
        public int CycleDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether commands are enabled.
        /// </summary>
        public bool CmdEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to poll a device after a command.
        /// </summary>
        public bool PollAfterCmd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write detailed information to the log.
        /// </summary>
        public bool DetailedLog { get; set; }
        
        
        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            ReqRetries = xmlNode.GetChildAsInt("ReqRetries", ReqRetries);
            CycleDelay = xmlNode.GetChildAsInt("CycleDelay", CycleDelay);
            CmdEnabled = xmlNode.GetChildAsBool("CmdEnabled", CmdEnabled);
            PollAfterCmd = xmlNode.GetChildAsBool("PollAfterCmd", PollAfterCmd);
            DetailedLog = xmlNode.GetChildAsBool("DetailedLog", DetailedLog);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("ReqRetries", ReqRetries);
            xmlElem.AppendElem("CycleDelay", CycleDelay);
            xmlElem.AppendElem("CmdEnabled", CmdEnabled);
            xmlElem.AppendElem("PollAfterCmd", PollAfterCmd);
            xmlElem.AppendElem("DetailedLog", DetailedLog);
        }
    }
}
