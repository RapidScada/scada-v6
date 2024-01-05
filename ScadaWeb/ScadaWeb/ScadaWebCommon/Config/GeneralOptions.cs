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
 * Module   : ScadaWebCommon
 * Summary  : Represents general web application options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Log;
using System.Xml;

namespace Scada.Web.Config
{
    /// <summary>
    /// Represents general web application options.
    /// <para>Представляет основные параметры веб-приложения.</para>
    /// </summary>
    public class GeneralOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GeneralOptions()
        {
            DefaultCulture = "";
            DefaultTimeZone = "";
            DefaultStartPage = "";
            EnableCommands = true;
            ShareStats = true;
            MaxLogSize = LogFile.DefaultCapacityMB;
        }


        /// <summary>
        /// Gets or sets the default culture name.
        /// </summary>
        public string DefaultCulture { get; set; }

        /// <summary>
        /// Gets or sets the default time zone identifier.
        /// </summary>
        /// <remarks>All the time zones are returned by the TimeZoneInfo.GetSystemTimeZones method.</remarks>
        public string DefaultTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the default start page that opens after user login.
        /// </summary>
        public string DefaultStartPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable telecontrol commands.
        /// </summary>
        public bool EnableCommands { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to share depersonalized stats with the developers.
        /// </summary>
        public bool ShareStats { get; set; }

        /// <summary>
        /// Gets or sets the maximum log file size, megabytes.
        /// </summary>
        public int MaxLogSize { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            DefaultCulture = xmlNode.GetChildAsString("DefaultCulture", DefaultCulture);
            DefaultTimeZone = xmlNode.GetChildAsString("DefaultTimeZone", DefaultTimeZone);
            DefaultStartPage = xmlNode.GetChildAsString("DefaultStartPage", DefaultStartPage);
            EnableCommands = xmlNode.GetChildAsBool("EnableCommands", EnableCommands);
            ShareStats = xmlNode.GetChildAsBool("ShareStats", ShareStats);
            MaxLogSize = xmlNode.GetChildAsInt("MaxLogSize", MaxLogSize);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("DefaultCulture", DefaultCulture);
            xmlElem.AppendElem("DefaultTimeZone", DefaultTimeZone);
            xmlElem.AppendElem("DefaultStartPage", DefaultStartPage);
            xmlElem.AppendElem("EnableCommands", EnableCommands);
            xmlElem.AppendElem("ShareStats", ShareStats);
            xmlElem.AppendElem("MaxLogSize", MaxLogSize);
        }
    }
}
