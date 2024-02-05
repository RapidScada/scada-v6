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
 * Summary  : Represents display options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using System.Xml;

namespace Scada.Web.Config
{
    /// <summary>
    /// Represents display options.
    /// <para>Представляет параметры отображения.</para>
    /// </summary>
    public class DisplayOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DisplayOptions()
        {
            ShowHeader = true;
            ShowMainMenu = true;
            ShowViewExplorer = true;
            RefreshRate = 1000;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to show a page header.
        /// </summary>
        public bool ShowHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the main menu.
        /// </summary>
        public bool ShowMainMenu { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the view explorer.
        /// </summary>
        public bool ShowViewExplorer { get; set; }

        /// <summary>
        /// Gets or sets the data refresh rate in milliseconds.
        /// </summary>
        public int RefreshRate { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ShowHeader = xmlNode.GetChildAsBool("ShowHeader", ShowHeader);
            ShowMainMenu = xmlNode.GetChildAsBool("ShowMainMenu", ShowMainMenu);
            ShowViewExplorer = xmlNode.GetChildAsBool("ShowViewExplorer", ShowViewExplorer);
            RefreshRate = xmlNode.GetChildAsInt("RefreshRate", RefreshRate);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("ShowHeader", ShowHeader);
            xmlElem.AppendElem("ShowMainMenu", ShowMainMenu);
            xmlElem.AppendElem("ShowViewExplorer", ShowViewExplorer);
            xmlElem.AppendElem("RefreshRate", RefreshRate);
        }
    }
}
