/*
 * Copyright 2021 Rapid Software LLC
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
 * Modified : 2021
 */

using System;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents a device configuration in a polling sequence.
    /// <para>Представляет конфигурацию устройства в последовательности опроса.</para>
    /// </summary>
    [Serializable]
    public class DeviceConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceConfig()
        {
            Active = false;
            IsBound = false;
            DeviceNum = 0;
            Name = "";
            Driver = "";
            NumAddress = 0;
            StrAddress = "";
            PollingOptions = new PollingOptions();
        }


        /// <summary>
        /// Gets or sets a value indicating whether the device is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the device is bound to the configuration database.
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
        /// Gets or sets the code of the driver that implements the device protocol.
        /// </summary>
        public string Driver { get; set; }

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
                throw new ArgumentNullException(nameof(xmlElem));

            Active = xmlElem.GetAttrAsBool("active");
            IsBound = xmlElem.GetAttrAsBool("isBound");
            DeviceNum = xmlElem.GetAttrAsInt("number");
            Name = xmlElem.GetAttrAsString("name");
            Driver = xmlElem.GetAttrAsString("driver");
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
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("isBound", IsBound);
            xmlElem.SetAttribute("number", DeviceNum);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("driver", Driver);
            xmlElem.SetAttribute("numAddress", NumAddress);
            xmlElem.SetAttribute("strAddress", StrAddress);
            PollingOptions.SaveToXml(xmlElem);
        }
    }
}
