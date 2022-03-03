/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents a deployment configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Admin.Lang;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Represents a deployment configuration.
    /// <para>Представляет конфигурацию развёртывания.</para>
    /// </summary>
    [Serializable]
    public class DeploymentConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "Deployment.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeploymentConfig()
        {
            Profiles = new SortedList<string, DeploymentProfile>();
            FileName = "";
            Loaded = false;
        }


        /// <summary>
        /// Gets the deployment profiles.
        /// </summary>
        public SortedList<string, DeploymentProfile> Profiles { get; private set; }

        /// <summary>
        /// Gets or sets the configuration file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets a value indicating whether the configuration is loaded.
        /// </summary>
        public bool Loaded { get; private set; }


        /// <summary>
        /// Loads the deployment configuration if needed.
        /// </summary>
        public bool Load(out string errMsg)
        {
            try
            {
                Profiles.Clear();
                Loaded = false;

                XmlDocument xmlDoc = new();
                xmlDoc.Load(FileName);

                foreach (XmlNode profileNode in xmlDoc.DocumentElement.SelectNodes("DeploymentProfile"))
                {
                    DeploymentProfile profile = new();
                    profile.LoadFromXml(profileNode);
                    Profiles[profile.Name] = profile;
                }

                Loaded = true;
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.LoadDeploymentConfigError);
                return false;
            }
        }

        /// <summary>
        /// Saves the deployment settings to the project directory.
        /// </summary>
        public bool Save(out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("DeploymentConfig");
                xmlDoc.AppendChild(rootElem);

                foreach (DeploymentProfile profile in Profiles.Values)
                {
                    profile.SaveToXml(rootElem.AppendElem("DeploymentProfile"));
                }

                xmlDoc.Save(FileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.SaveDeploymentConfigError);
                return false;
            }
        }

        /// <summary>
        /// Gets the existing profile names.
        /// </summary>
        public HashSet<string> GetExistingProfileNames(string exceptName = null)
        {
            HashSet<string> existingNames = new();

            foreach (DeploymentProfile profile in Profiles.Values)
            {
                if (exceptName == null || profile.Name != exceptName)
                    existingNames.Add(profile.Name);
            }

            return existingNames;
        }

        /// <summary>
        /// Updates the instance name of the Agent connection options for profiles belong to the specified instance.
        /// </summary>
        /// <returns>Returns true if profiles are found and updated; otherwise, false.</returns>
        public bool UpdateInstanceName(int instanceID, string instanceName)
        {
            bool profilesAffected = false;

            foreach (DeploymentProfile profile in Profiles.Values)
            {
                if (profile.InstanceID == instanceID)
                {
                    profile.AgentConnectionOptions.Instance = instanceName;
                    profilesAffected = true;
                }
            }

            return profilesAffected;
        }

        /// <summary>
        /// Removes profiles belong to the specified instance.
        /// </summary>
        /// <returns>Returns true if profiles are found and removed; otherwise, false.</returns>
        public bool RemoveProfilesByInstance(int instanceID)
        {
            bool profilesAffected = false;

            for (int i = Profiles.Count - 1; i >= 0; i--)
            {
                if (Profiles.Values[i].InstanceID == instanceID)
                {
                    Profiles.RemoveAt(i);
                    profilesAffected = true;
                }
            }

            return profilesAffected;
        }
    }
}
