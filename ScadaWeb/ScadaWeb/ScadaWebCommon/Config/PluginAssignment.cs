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
 * Summary  : Represents a relationship between the application features and plugins
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using System.Xml;

namespace Scada.Web.Config
{
    /// <summary>
    /// Represents a relationship between the application features and plugins.
    /// <para>Представляет взаимосвязь между функциями приложения и плагинами.</para>
    /// </summary>
    public class PluginAssignment
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginAssignment()
        {
            ChartFeature = "";
            CommandFeature = "";
            EventAckFeature = "";
            UserManagementFeature = "";
            NotificationFeature = "";
        }


        /// <summary>
        /// Gets or sets the code of the plugin that provides charting.
        /// </summary>
        public string ChartFeature { get; set; }

        /// <summary>
        /// Gets or sets the code of the plugin that provides sending commands.
        /// </summary>
        public string CommandFeature { get; set; }

        /// <summary>
        /// Gets or sets the code of the plugin that provides event acknowledgement.
        /// </summary>
        public string EventAckFeature { get; set; }

        /// <summary>
        /// Gets or sets the code of the plugin that manages users.
        /// </summary>
        public string UserManagementFeature { get; set; }

        /// <summary>
        /// Gets or sets the code of the plugin that manages notifications.
        /// </summary>
        public string NotificationFeature { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ChartFeature = xmlNode.GetChildAsString("ChartFeature");
            CommandFeature = xmlNode.GetChildAsString("CommandFeature");
            EventAckFeature = xmlNode.GetChildAsString("EventAckFeature");
            UserManagementFeature = xmlNode.GetChildAsString("UserManagementFeature");
            NotificationFeature = xmlNode.GetChildAsString("NotificationFeature");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("ChartFeature", ChartFeature);
            xmlElem.AppendElem("CommandFeature", CommandFeature);
            xmlElem.AppendElem("EventAckFeature", EventAckFeature);
            xmlElem.AppendElem("UserManagementFeature", UserManagementFeature);
            xmlElem.AppendElem("NotificationFeature", NotificationFeature);
        }
    }
}
