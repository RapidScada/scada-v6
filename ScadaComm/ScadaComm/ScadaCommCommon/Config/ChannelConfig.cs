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
 * Summary  : Represents a communication channel configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
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
    public class ChannelConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelConfig()
        {
            TypeName = "";
            Dll = "";
            CustomOptions = new CustomOptions();
        }

        
        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the DLL that implements the channel.
        /// </summary>
        public string Dll { get; set; }

        /// <summary>
        /// Gets the custom options.
        /// </summary>
        public CustomOptions CustomOptions { get; private set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            TypeName = xmlElem.GetAttrAsString("type");
            Dll = xmlElem.GetAttrAsString("dll");
            CustomOptions.LoadFromXml(xmlElem);
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.SetAttribute("type", TypeName);
            xmlElem.SetAttribute("dll", Dll);
            CustomOptions.SaveToXml(xmlElem);
        }
    }
}
