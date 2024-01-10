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
 * Summary  : Represents a deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Client;
using Scada.Dbms;
using System;
using System.Xml;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Represents a deployment profile.
    /// <para>Представляет профиль развёртывания.</para>
    /// </summary>
    [Serializable]
    public class DeploymentProfile
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeploymentProfile()
        {
            InstanceID = 0;
            Name = "";
            Extension = "";
            WebUrl = "";
            AgentEnabled = true;
            DbEnabled = false;
            AgentConnectionOptions = new ConnectionOptions
            {
                Port = 10002,
                Username = "ScadaAdmin",
                Password = ""
            };
            DbConnectionOptions = new DbConnectionOptions();
            DownloadOptions = new DownloadOptions();
            UploadOptions = new UploadOptions();
        }


        /// <summary>
        /// Gets or set the reference to the instance to which the profile belongs.
        /// </summary>
        public int InstanceID { get; set; }

        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the code of the extension that provides data transfer.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the web application address.
        /// </summary>
        public string WebUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Agent connection is enabled.
        /// </summary>
        public bool AgentEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the database connection is enabled.
        /// </summary>
        public bool DbEnabled { get; set; }

        /// <summary>
        /// Gets the Agent connection options.
        /// </summary>
        public ConnectionOptions AgentConnectionOptions { get; private set; }

        /// <summary>
        /// Gets the database connection options.
        /// </summary>
        public DbConnectionOptions DbConnectionOptions { get; private set; }

        /// <summary>
        /// Gets the download options.
        /// </summary>
        public DownloadOptions DownloadOptions { get; private set; }

        /// <summary>
        /// Gets the upload options.
        /// </summary>
        public UploadOptions UploadOptions { get; private set; }
        
        
        /// <summary>
        /// Loads the settings from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            InstanceID = xmlNode.GetChildAsInt("InstanceID");
            Name = xmlNode.GetChildAsString("Name");
            Extension = xmlNode.GetChildAsString("Extension");
            WebUrl = xmlNode.GetChildAsString("WebUrl");
            AgentEnabled = xmlNode.GetChildAsBool("AgentEnabled");
            DbEnabled = xmlNode.GetChildAsBool("DbEnabled");

            if (xmlNode.SelectSingleNode("AgentConnectionOptions") is XmlNode agentConnectionOptionsNode)
                AgentConnectionOptions.LoadFromXml(agentConnectionOptionsNode);

            if (xmlNode.SelectSingleNode("DbConnectionOptions") is XmlNode dbConnectionOptionsNode)
                DbConnectionOptions.LoadFromXml(dbConnectionOptionsNode);

            if (xmlNode.SelectSingleNode("DownloadOptions") is XmlNode downloadOptionsNode)
                DownloadOptions.LoadFromXml(downloadOptionsNode);

            if (xmlNode.SelectSingleNode("UploadOptions") is XmlNode uploadOptionsNode)
                UploadOptions.LoadFromXml(uploadOptionsNode);
        }

        /// <summary>
        /// Saves the settings into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("InstanceID", InstanceID);
            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("Extension", Extension);
            xmlElem.AppendElem("WebUrl", WebUrl);
            xmlElem.AppendElem("AgentEnabled", AgentEnabled);
            xmlElem.AppendElem("DbEnabled", DbEnabled);
            AgentConnectionOptions.SaveToXml(xmlElem.AppendElem("AgentConnectionOptions"));
            DbConnectionOptions.SaveToXml(xmlElem.AppendElem("DbConnectionOptions"));
            DownloadOptions.SaveToXml(xmlElem.AppendElem("DownloadOptions"));
            UploadOptions.SaveToXml(xmlElem.AppendElem("UploadOptions"));
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
