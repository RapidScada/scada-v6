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
 * Summary  : Represents a data source configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Config;
using System;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents a data source configuration.
    /// <para>Представляет конфигурацию источника данных.</para>
    /// </summary>
    [Serializable]
    public class DataSourceConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataSourceConfig()
        {
            Active = false;
            Code = "";
            Name = "";
            Driver = "";
            CustomOptions = new OptionList();
        }


        /// <summary>
        /// Gets or sets a value indicating whether the data source is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the data source code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the data source name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code of the driver that implements the data source.
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

            Active = xmlElem.GetAttrAsBool("active");
            Code = xmlElem.GetAttrAsString("code");
            Name = xmlElem.GetAttrAsString("name");
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

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("code", Code);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("driver", Driver);
            CustomOptions.SaveToXml(xmlElem);
        }
    }
}
