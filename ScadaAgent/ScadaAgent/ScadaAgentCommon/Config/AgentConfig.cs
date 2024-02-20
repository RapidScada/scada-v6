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
 * Summary  : Represents Agent configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Lang;
using Scada.Server;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Agent.Config
{
    /// <summary>
    /// Represents Agent configuration.
    /// <para>Представляет конфигурацию Агента.</para>
    /// </summary>
    public class AgentConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaAgentConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AgentConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the listener options.
        /// </summary>
        public ListenerOptions ListenerOptions { get; private set; }

        /// <summary>
        /// Gets the reverse connection options.
        /// </summary>
        public ReverseConnectionOptions ReverseConnectionOptions { get; private set; }
        
        /// <summary>
        /// Gets the options of the instances.
        /// </summary>
        public List<InstanceOptions> Instances { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            ListenerOptions = new ListenerOptions();
            ReverseConnectionOptions = new ReverseConnectionOptions();
            Instances = new List<InstanceOptions>();
        }


        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                if (rootElem.SelectSingleNode("ListenerOptions") is XmlNode listenerOptionsNode)
                    ListenerOptions.LoadFromXml(listenerOptionsNode);

                if (rootElem.SelectSingleNode("ReverseConnection") is XmlElement reverseConnectionElem)
                {
                    ReverseConnectionOptions.Enabled = reverseConnectionElem.GetAttrAsBool("enabled");
                    ReverseConnectionOptions.LoadFromXml(reverseConnectionElem);
                }

                if (rootElem.SelectSingleNode("Instances") is XmlNode instancesNode)
                {
                    foreach (XmlElement instanceElem in instancesNode.SelectNodes("Instance"))
                    {
                        InstanceOptions instanceConfig = new InstanceOptions();
                        instanceConfig.LoadFromXml(instanceElem);
                        Instances.Add(instanceConfig);
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(CommonPhrases.LoadConfigError);
                return false;
            }
        }
    }
}
