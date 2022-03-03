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
 * Summary  : Represents an instance that includes of one or more applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Admin.Lang;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents an instance that includes of one or more applications.
    /// <para>Представляет экземпляр, включающий из одно или несколько приложений.</para>
    /// </summary>
    public class ProjectInstance
    {
        /// <summary>
        /// The default instance name.
        /// </summary>
        public const string DefaultName = "Default";

        private string instanceDir; // the instance directory inside the project


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProjectInstance()
        {
            ID = 0;
            Name = "";
            ServerApp = new ServerApp();
            CommApp = new CommApp();
            WebApp = new WebApp();
            AllApps = new ProjectApp[] { ServerApp, CommApp, WebApp };
            DeploymentProfile = "";
            InstanceDir = "";
        }


        /// <summary>
        /// Gets or sets the instance identifier.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the instance.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the Server application in the project.
        /// </summary>
        public ServerApp ServerApp { get; }

        /// <summary>
        /// Gets the Communicator application in the project.
        /// </summary>
        public CommApp CommApp { get; }

        /// <summary>
        /// Gets the Webstation application in the project.
        /// </summary>
        public WebApp WebApp { get; }

        /// <summary>
        /// Gets all applications in the project.
        /// </summary>
        public ProjectApp[] AllApps { get; }

        /// <summary>
        /// Gets or sets the name of the deployment profile.
        /// </summary>
        public string DeploymentProfile { get; set; }

        /// <summary>
        /// Gets or sets the instance directory inside the project.
        /// </summary>
        public string InstanceDir
        {
            get
            {
                return instanceDir;
            }
            set
            {
                instanceDir = ScadaUtils.NormalDir(value);
                Array.ForEach(AllApps, app => app.InitAppDir(instanceDir));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether all the application configurations are loaded.
        /// </summary>
        public bool ConfigLoaded
        {
            get
            {
                return AllApps.All(app => !app.Enabled || app.ConfigLoaded);
            }
            set
            {
                foreach (ProjectApp app in AllApps)
                {
                    app.ConfigLoaded = value;
                }
            }
        }


        /// <summary>
        /// Loads the instance configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            ID = xmlNode.GetChildAsInt("ID", 1);
            Name = xmlNode.GetChildAsString("Name");

            if (xmlNode.SelectSingleNode("ServerApp") is XmlElement serverAppElem)
                ServerApp.LoadFromXml(serverAppElem);

            if (xmlNode.SelectSingleNode("CommApp") is XmlElement commAppElem)
                CommApp.LoadFromXml(commAppElem);

            if (xmlNode.SelectSingleNode("WebApp") is XmlElement webAppElem)
                WebApp.LoadFromXml(webAppElem);

            DeploymentProfile = xmlNode.GetChildAsString("DeploymentProfile");
        }

        /// <summary>
        /// Saves the instance configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("ID", ID);
            xmlElem.AppendElem("Name", Name);
            ServerApp.SaveToXml(xmlElem.AppendElem("ServerApp"));
            CommApp.SaveToXml(xmlElem.AppendElem("CommApp"));
            WebApp.SaveToXml(xmlElem.AppendElem("WebApp"));
            xmlElem.AppendElem("DeploymentProfile", DeploymentProfile);
        }

        /// <summary>
        /// Loads configuration of all the applications if needed.
        /// </summary>
        public bool LoadAppConfig(out string errMsg)
        {
            foreach (ProjectApp app in AllApps)
            {
                if (app.Enabled && !(app.ConfigLoaded || app.LoadConfig(out errMsg)))
                    return false;
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Creates all project files required for the instance.
        /// </summary>
        public bool CreateInstanceFiles(out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(InstanceDir);

                foreach (ProjectApp app in AllApps)
                {
                    if (app.Enabled && !app.CreateConfigFiles(out errMsg))
                        return false;
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.CreateInstanceFilesError);
                return false;
            }
        }

        /// <summary>
        /// Deletes all project files of the instance.
        /// </summary>
        public bool DeleteInstanceFiles(out string errMsg)
        {
            try
            {
                if (Directory.Exists(InstanceDir))
                    Directory.Delete(InstanceDir, true);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.DeleteInstanceFilesError);
                return false;
            }
        }

        /// <summary>
        /// Renames the instance.
        /// </summary>
        public bool Rename(string newName, out string errMsg)
        {
            try
            {
                if (string.IsNullOrEmpty(newName))
                    throw new ArgumentException(AdminPhrases.InstanceNameEmpty, nameof(newName));

                if (!AdminUtils.NameIsValid(newName))
                    throw new ArgumentException(AdminPhrases.InstanceNameInvalid, nameof(newName));

                DirectoryInfo directoryInfo = new(InstanceDir);
                string newInstanceDir = Path.Combine(directoryInfo.Parent.FullName, newName);
                directoryInfo.MoveTo(newInstanceDir);
                InstanceDir = newInstanceDir;
                Name = newName;

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.RenameInstanceError);
                return false;
            }
        }
    }
}
