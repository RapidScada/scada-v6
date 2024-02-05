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
 * Module   : ScadaServerCommon
 * Summary  : Represents general server options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Log;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Server.Config
{
    /// <summary>
    /// Represents general server options.
    /// <para>Представляет основные параметры сервера.</para>
    /// </summary>
    [Serializable]
    public class GeneralOptions
    {
        private ICollection<int> enableFormulasObjNums;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GeneralOptions()
        {
            UnrelIfInactive = 300;
            MaxCurDataAge = 0;
            UseArchivalStatus = false;
            GenerateAckCmd = false;
            DisableFormulas = false;
            EnableFormulasObjNums = Array.Empty<int>();
            StopWait = 10;
            MaxLogSize = LogFile.DefaultCapacityMB;
        }


        /// <summary>
        /// Gets or sets the time after which an inactive channel is marked as unreliable, seconds.
        /// </summary>
        public int UnrelIfInactive { get; set; }

        /// <summary>
        /// Gets or sets the maximum time after which the current data is written as historical, seconds.
        /// </summary>
        public int MaxCurDataAge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to mark incoming historical data as archival.
        /// </summary>
        public bool UseArchivalStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate a command when an event is acknowledged.
        /// </summary>
        public bool GenerateAckCmd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether channel formula calculation is disabled.
        /// </summary>
        public bool DisableFormulas { get; set; }

        /// <summary>
        /// Gets the object numbers of the channels for which formulas are enabled, 
        /// even though formulas are generally disabled.
        /// </summary>
        public ICollection<int> EnableFormulasObjNums
        {
            get
            {
                return enableFormulasObjNums;
            }
            set
            {
                enableFormulasObjNums = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

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

            UnrelIfInactive = xmlNode.GetChildAsInt("UnrelIfInactive", UnrelIfInactive);
            MaxCurDataAge = xmlNode.GetChildAsInt("MaxCurDataAge", MaxCurDataAge);
            UseArchivalStatus = xmlNode.GetChildAsBool("UseArchivalStatus", UseArchivalStatus);
            GenerateAckCmd = xmlNode.GetChildAsBool("GenerateAckCmd", GenerateAckCmd);
            DisableFormulas = xmlNode.GetChildAsBool("DisableFormulas", DisableFormulas);
            EnableFormulasObjNums = ScadaUtils.ParseRange(
                xmlNode.GetChildAsString("EnableFormulasObjNums"), true, true);
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

            xmlElem.AppendElem("UnrelIfInactive", UnrelIfInactive);
            xmlElem.AppendElem("MaxCurDataAge", MaxCurDataAge);
            xmlElem.AppendElem("UseArchivalStatus", UseArchivalStatus);
            xmlElem.AppendElem("GenerateAckCmd", GenerateAckCmd);
            xmlElem.AppendElem("DisableFormulas", DisableFormulas);
            xmlElem.AppendElem("EnableFormulasObjNums", EnableFormulasObjNums.ToRangeString());
            xmlElem.AppendElem("StopWait", StopWait);
            xmlElem.AppendElem("MaxLogSize", MaxLogSize);
        }
    }
}
