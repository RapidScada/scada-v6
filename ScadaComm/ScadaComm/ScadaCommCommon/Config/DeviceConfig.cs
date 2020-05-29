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
 * Summary  : Represents a device configuration in a polling sequence
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
    /// Represents a device configuration in a polling sequence.
    /// <para>Представляет конфигурацию устройства в последовательности опроса.</para>
    /// </summary>
    public class DeviceConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceConfig()
        {
            Active = true;
            IsBound = true;
            DeviceNum = 0;
            Name = "";
            Dll = "";
            NumAddress = 0;
            StrAddress = "";
            PollingOptions = new PollingOptions();
        }


        /// <summary>
        /// Gets or sets a value indicating whether the line is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the line is bound to the server.
        /// </summary>
        public bool IsBound { get; set; }

        /// <summary>
        /// Gets or sets the device number.
        /// </summary>
        public int DeviceNum { get; set; }

        /// <summary>
        /// Gets or sets the device name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the DLL that implements the device protocol.
        /// </summary>
        public string Dll { get; set; }

        /// <summary>
        /// Gets or sets the numeric address.
        /// </summary>
        public int NumAddress { get; set; }

        /// <summary>
        /// Gets or sets the string address.
        /// </summary>
        public string StrAddress { get; set; }

        /// <summary>
        /// Gets the polling options.
        /// </summary>
        public PollingOptions PollingOptions { get; private set; }
        
        
        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            Active = xmlElem.GetAttrAsBool("active");
            IsBound = xmlElem.GetAttrAsBool("isBound");
            DeviceNum = xmlElem.GetAttrAsInt("number");
            Name = xmlElem.GetAttrAsString("name");
            Dll = xmlElem.GetAttrAsString("dll");
            NumAddress = xmlElem.GetAttrAsInt("numAddress");
            StrAddress = xmlElem.GetAttrAsString("strAddress");
            PollingOptions.LoadFromXml(xmlElem);
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("isBound", IsBound);
            xmlElem.SetAttribute("number", DeviceNum);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("dll", Dll);
            xmlElem.SetAttribute("numAddress", NumAddress);
            xmlElem.SetAttribute("strAddress", StrAddress);
            PollingOptions.SaveToXml(xmlElem);
        }
    }
}
