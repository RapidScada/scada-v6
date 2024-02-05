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
 * Module   : ScadaAdminCommon
 * Summary  : Represents channel numbering options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2023
 */

using System.Xml;

namespace Scada.Admin.Config
{
    /// <summary>
    /// Represents channel numbering options.
    /// <para>Представляет параметры нумерации каналов.</para>
    /// </summary>
    public class ChannelNumberingOptions : NumberingOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelNumberingOptions()
            : base()
        {
            PrependDeviceName = true;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to prepend a device name in channel names.
        /// </summary>
        public bool PrependDeviceName { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            PrependDeviceName = xmlNode.GetChildAsBool("PrependDeviceName", PrependDeviceName);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("PrependDeviceName", PrependDeviceName);
        }
    }
}
