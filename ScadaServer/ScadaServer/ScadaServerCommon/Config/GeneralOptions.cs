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
 * Module   : ScadaServerCommon
 * Summary  : Represents general server options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Log;
using System;
using System.Xml;

namespace Scada.Server.Config
{
    /// <summary>
    /// Represents general server options.
    /// <para>Представляет основные параметры сервера.</para>
    /// </summary>
    public class GeneralOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GeneralOptions()
        {
            UnrelIfInactive = 300;
            MaxLogSize = LogFile.DefaultCapacity;
        }


        /// <summary>
        /// Gets or sets the time after which an inactive input channel is marked as unreliable, sec.
        /// </summary>
        public int UnrelIfInactive { get; set; }

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
                throw new ArgumentNullException("xmlNode");

            UnrelIfInactive = xmlNode.GetChildAsInt("UnrelIfInactive", UnrelIfInactive);
            MaxLogSize = xmlNode.GetChildAsInt("MaxLogSize", MaxLogSize);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("UnrelIfInactive", UnrelIfInactive);
            xmlElem.AppendElem("MaxLogSize", MaxLogSize);
        }
    }
}
