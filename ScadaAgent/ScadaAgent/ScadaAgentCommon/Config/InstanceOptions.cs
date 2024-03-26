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
 * Module   : ScadaAgentCommon
 * Summary  : Represents instance options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Xml;

namespace Scada.Agent.Config
{
    /// <summary>
    /// Represents instance options.
    /// <para>Представляет параметры экземпляра.</para>
    /// </summary>
    public class InstanceOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public InstanceOptions()
        {
            Active = true;
            Name = "";
            ProxyMode = false;
            Directory = "";
            AdminUser = new UserCredentials();
            AgentUser = new UserCredentials();
        }


        /// <summary>
        /// Gets or sets a value indicating whether the instance is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the instance name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the instance is deployed on another host.
        /// </summary>
        public bool ProxyMode { get; set; }

        /// <summary>
        /// Gets or sets the installation directory of the instance.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Gets the user credentials to connect as an administrator.
        /// </summary>
        public UserCredentials AdminUser { get; private set; }

        /// <summary>
        /// Gets the user credentials to connect as a remote agent.
        /// </summary>
        public UserCredentials AgentUser { get; private set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Active = xmlElem.GetAttrAsBool("active");
            Name = xmlElem.GetAttrAsString("name");
            ProxyMode = xmlElem.GetChildAsBool("ProxyMode");
            Directory = xmlElem.GetChildAsString("Directory");

            if (xmlElem.SelectSingleNode("AdminUser") is XmlNode adminUserNode)
                AdminUser.LoadFromXml(adminUserNode);

            if (xmlElem.SelectSingleNode("AgentUser") is XmlNode agentUserNode)
                AgentUser.LoadFromXml(agentUserNode);
        }
    }
}
