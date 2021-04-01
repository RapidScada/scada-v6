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

using System;
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
        /// Gets or sets a value indicating whether the application is present in the instance.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the application directory in the project.
        /// </summary>
        public string AppDir { get; set; } = "";



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
        /// Creates configuration files required for the application.
        /// </summary>
        public abstract bool CreateConfigFiles(out string errMsg);

        /// <summary>
        /// Deletes configuration files of the application.
        /// </summary>
        public abstract bool DeleteConfigFiles(out string errMsg);

        /// <summary>
        /// Clears the application configuration in memory.
        /// </summary>
        public abstract void ClearConfig();
    }
}
