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
 * Summary  : Represents a communication channel configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Config;
using System;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents a communication channel configuration.
    /// <para>Представляет конфигурацию канала связи.</para>
    /// </summary>
    [Serializable]
    public class ChannelConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelConfig()
        {
            TypeCode = "";
            Driver = "";
            CustomOptions = new OptionList();
        }

        
        /// <summary>
        /// Gets or sets the channel type code.
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// Gets or sets the code of the driver that implements the channel.
        /// </summary>
        public string Driver { get; set; }

        /// <summary>
        /// Gets the custom options.
        /// </summary>
        public OptionList CustomOptions { get; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            TypeCode = xmlElem.GetAttrAsString("type");
            Driver = xmlElem.GetAttrAsString("driver");
            CustomOptions.LoadFromXml(xmlElem);
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("type", TypeCode);
            xmlElem.SetAttribute("driver", Driver);

            if (!string.IsNullOrEmpty(Driver))
                CustomOptions.SaveToXml(xmlElem);
        }
    }
}
