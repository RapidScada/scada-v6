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
 * Module   : ScadaAdminCommon
 * Summary  : Represents an instance that includes of one or more applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.Lang;
using System;
using System.IO;
using System.Xml;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents an application in a project.
    /// <para>Представляет приложение в проекте.</para>
    /// </summary>
    public abstract class ProjectApp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProjectApp()
        {
            Enabled = true;
            AppDir = "";
            ClearConfig();
        }


        /// <summary>
        /// Gets or sets a value indicating whether the application is present in the instance.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the application directory in the project.
        /// </summary>
        public string AppDir { get; set; }

        /// <summary>
        /// Gets a value indicating whether the application configuration is loaded.
        /// </summary>
        public bool ConfigLoaded { get; protected set; }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        public abstract string AppName { get; }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public abstract object AppConfig { get; }


        /// <summary>
        /// Loads the application options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Enabled = xmlElem.GetAttrAsBool("enabled");
        }

        /// <summary>
        /// Saves the application options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("enabled", Enabled);
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        public abstract bool LoadConfig(out string errMsg);

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        public abstract bool SaveConfig(out string errMsg);

        /// <summary>
        /// Gets the application configuration directory.
        /// </summary>
        public virtual string GetConfigDir()
        {
            return string.IsNullOrEmpty(AppDir) ? "" : Path.Combine(AppDir, "Config");
        }

        /// <summary>
        /// Creates configuration files required for the application.
        /// </summary>
        public virtual bool CreateConfigFiles(out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(GetConfigDir());
                return SaveConfig(out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, AdminPhrases.CreateAppConfigError, AppName);
                return false;
            }
        }

        /// <summary>
        /// Deletes configuration files of the application.
        /// </summary>
        public virtual bool DeleteConfigFiles(out string errMsg)
        {
            try
            {
                Directory.Delete(AppDir, true);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, AdminPhrases.DeleteAppConfigError, AppName);
                return false;
            }
        }

        /// <summary>
        /// Clears the application configuration in memory.
        /// </summary>
        public virtual void ClearConfig()
        {
            ConfigLoaded = false;
        }
    }
}
