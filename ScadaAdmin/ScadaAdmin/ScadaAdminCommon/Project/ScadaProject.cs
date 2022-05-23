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
 * Summary  : Represents a Rapid SCADA project
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Admin.Deployment;
using Scada.Admin.Lang;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents a Rapid SCADA project.
    /// <para>Представляет проект Rapid SCADA.</para>
    /// </summary>
    public class ScadaProject
    {
        /// <summary>
        /// The default project name.
        /// </summary>
        public const string DefaultName = "NewProject";

        private string fileName; // the project file name.


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaProject()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the project file name.
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            protected set
            {
                fileName = value;

                if (string.IsNullOrEmpty(fileName))
                {
                    Name = DefaultName;
                    ConfigDatabase.BaseDir = "";
                    Views.ViewDir = "";
                    Instances.ForEach(i => i.InstanceDir = "");
                    DeploymentConfig.FileName = "";
                }
                else
                {
                    Name = Path.GetFileNameWithoutExtension(fileName);
                    string projectDir = Path.GetDirectoryName(fileName);
                    ConfigDatabase.BaseDir = ScadaUtils.NormalDir(Path.Combine(projectDir, "BaseXML"));
                    Views.ViewDir = ScadaUtils.NormalDir(Path.Combine(projectDir, "Views"));
                    Instances.ForEach(i => i.InstanceDir = Path.Combine(projectDir, "Instances", i.Name));
                    DeploymentConfig.FileName = Path.Combine(projectDir, DeploymentConfig.DefaultFileName);
                }
            }
        }

        /// <summary>
        /// Gets the project directory.
        /// </summary>
        public string ProjectDir
        {
            get
            {
                return Path.GetDirectoryName(FileName);
            }
        }

        /// <summary>
        /// Gets or sets the project version.
        /// </summary>
        public ProjectVersion Version { get; set; }

        /// <summary>
        /// Gets or sets the project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the configuration database.
        /// </summary>
        public ConfigDatabase ConfigDatabase { get; private set; }

        /// <summary>
        /// Gets the metadata of the views.
        /// </summary>
        public ProjectViews Views { get; private set; }

        /// <summary>
        /// Gets the instances, including the corresponding application settings.
        /// </summary>
        public List<ProjectInstance> Instances { get; private set; }

        /// <summary>
        /// Gets the deployment configuration.
        /// </summary>
        public DeploymentConfig DeploymentConfig { get; private set; }


        /// <summary>
        /// Sets the project properties to the defaults.
        /// </summary>
        private void SetToDefault()
        {
            fileName = "";

            Name = DefaultName;
            Version = ProjectVersion.Default;
            Description = "";
            ConfigDatabase = new ConfigDatabase();
            Views = new ProjectViews();
            Instances = new List<ProjectInstance>();
            DeploymentConfig = new DeploymentConfig();
        }

        /// <summary>
        /// Gets the maximum ID of existing instances.
        /// </summary>
        private int GetMaxInstanceID()
        {
            return Instances.Count > 0 ? Instances.Max(i => i.ID) : 0;
        }

        /// <summary>
        /// Gets a file name to store a project.
        /// </summary>
        private static string GetProjectFileName(string projectDir, string projectName)
        {
            return Path.Combine(projectDir, projectName + AdminUtils.ProjectExt);
        }

        /// <summary>
        /// Copies the content of the directory.
        /// </summary>
        private static void CopyDirectory(DirectoryInfo source, DirectoryInfo dest)
        {
            Directory.CreateDirectory(dest.FullName);

            foreach (DirectoryInfo sourceSubdir in source.GetDirectories())
            {
                DirectoryInfo destSubdir = dest.CreateSubdirectory(sourceSubdir.Name);
                CopyDirectory(sourceSubdir, destSubdir);
            }

            foreach (FileInfo fileInfo in source.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(dest.FullName, fileInfo.Name), true);
            }
        }


        /// <summary>
        /// Loads the project from the specified file.
        /// </summary>
        public void Load(string fileName)
        {
            SetToDefault();
            FileName = fileName;

            XmlDocument xmlDoc = new();
            xmlDoc.Load(fileName);

            XmlElement rootElem = xmlDoc.DocumentElement;
            Version = ProjectVersion.Parse(rootElem.GetChildAsString("ProjectVersion"));
            Description = rootElem.GetChildAsString("Description");

            // load instances
            if (rootElem.SelectSingleNode("Instances") is XmlNode instancesNode)
            {
                HashSet<int> instanceIDs = new();
                string projectDir = ProjectDir;

                foreach (XmlNode instanceNode in instancesNode.SelectNodes("Instance"))
                {
                    ProjectInstance instance = new();
                    instance.LoadFromXml(instanceNode);
                    instance.InstanceDir = Path.Combine(projectDir, "Instances", instance.Name);

                    if (instance.ID > 0 && instanceIDs.Add(instance.ID)) // check uniqueness
                        Instances.Add(instance);
                }
            }
        }

        /// <summary>
        /// Loads the project from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                Load(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.LoadProjectError);
                return false;
            }
        }

        /// <summary>
        /// Saves the project to the specified file.
        /// </summary>
        public void Save(string fileName)
        {
            FileName = fileName;

            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ScadaProject");
            rootElem.AppendElem("AdminVersion", AdminUtils.AppVersion);
            rootElem.AppendElem("ProjectVersion", Version);
            rootElem.AppendElem("Description", Description);
            xmlDoc.AppendChild(rootElem);

            // save intances
            XmlElement instancesElem = xmlDoc.CreateElement("Instances");
            rootElem.AppendChild(instancesElem);

            foreach (ProjectInstance instance in Instances)
            {
                XmlElement instanceElem = xmlDoc.CreateElement("Instance");
                instance.SaveToXml(instanceElem);
                instancesElem.AppendChild(instanceElem);
            }

            xmlDoc.Save(fileName);
        }

        /// <summary>
        /// Saves the project to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.SaveProjectError);
                return false;
            }
        }

        /// <summary>
        /// Renames the project.
        /// </summary>
        public bool Rename(string newName, out string errMsg)
        {
            try
            {
                if (string.IsNullOrEmpty(newName))
                    throw new ArgumentException(AdminPhrases.ProjectNameEmpty, nameof(newName));

                if (!AdminUtils.NameIsValid(newName))
                    throw new ArgumentException(AdminPhrases.ProjectNameInvalid, nameof(newName));

                string projectDir = ProjectDir;
                DirectoryInfo directoryInfo = new(projectDir);
                string newProjectDir = Path.Combine(directoryInfo.Parent.FullName, newName);

                if (Directory.Exists(newProjectDir))
                    throw new ScadaException(AdminPhrases.ProjectDirectoryExists);

                File.Move(FileName, GetProjectFileName(projectDir, newName));
                Directory.Move(projectDir, newProjectDir);
                FileName = GetProjectFileName(newProjectDir, newName);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.RenameProjectError);
                return false;
            }
        }

        /// <summary>
        /// Creates a new instance that is not attached to any project.
        /// </summary>
        public ProjectInstance CreateInstance(string name)
        {
            return new ProjectInstance
            {
                ID = GetMaxInstanceID() + 1,
                Name = name,
                InstanceDir = Path.Combine(ProjectDir, "Instances", name)
            };
        }

        /// <summary>
        /// Determines whether an instance is in the project.
        /// </summary>
        public bool ContainsInstance(string name)
        {
            return Instances.Any(i => string.Equals(i.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the existing instance names as a hashset.
        /// </summary>
        public HashSet<string> GetInstanceNames(bool lowercase, string exceptName = null)
        {
            HashSet<string> instanceNames = new();
            exceptName = exceptName == null ? null : (lowercase ? exceptName.ToLowerInvariant() : exceptName);

            foreach (ProjectInstance instance in Instances)
            {
                string instanceName = lowercase ? instance.Name.ToLowerInvariant() : instance.Name;
                if (exceptName == null || instanceName != exceptName)
                    instanceNames.Add(instanceName);
            }

            return instanceNames;
        }

        /// <summary>
        /// Gets the project info.
        /// </summary>
        public string GetInfo()
        {
            StringBuilder sbInfo = new();

            if (Locale.IsRussian)
            {
                sbInfo
                    .Append("Наименование проекта : ").AppendLine(Name)
                    .Append("Версия проекта       : ").AppendLine(Version.ToString())
                    .Append("Метка времени        : ").AppendLine(DateTime.Now.ToLocalizedString());
            }
            else
            {
                sbInfo
                    .Append("Project name    : ").AppendLine(Name)
                    .Append("Project version : ").AppendLine(Version.ToString())
                    .Append("Timestamp       : ").AppendLine(DateTime.Now.ToLocalizedString());
            }

            return sbInfo.ToString();
        }

        /// <summary>
        /// Creates a new project with the specified parameters.
        /// </summary>
        public static bool Create(string name, string location, string template,
            out ScadaProject project, out string errMsg)
        {
            try
            {
                // validate arguments
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException(AdminPhrases.ProjectNameEmpty, nameof(name));

                if (!AdminUtils.NameIsValid(name))
                    throw new ArgumentException(AdminPhrases.ProjectNameInvalid, nameof(name));

                // define project directory and file
                string projectDir = Path.Combine(location, name);
                string projectFileName = GetProjectFileName(projectDir, name);

                if (Directory.Exists(projectDir))
                    throw new ScadaException(AdminPhrases.ProjectDirectoryExists);

                // copy template
                if (File.Exists(template))
                {
                    FileInfo templateFileInfo = new(template);
                    CopyDirectory(templateFileInfo.Directory, new DirectoryInfo(projectDir));
                    File.Move(Path.Combine(projectDir, templateFileInfo.Name), projectFileName);
                }

                // create project
                project = new ScadaProject { Name = name };

                if (File.Exists(projectFileName))
                {
                    // load from template
                    project.Load(projectFileName);
                    project.Description = "";
                }
                else
                {
                    Directory.CreateDirectory(projectDir);
                    project.FileName = projectFileName;
                }

                project.Save(project.FileName);

                // create necessary directories
                Directory.CreateDirectory(project.ConfigDatabase.BaseDir);
                Directory.CreateDirectory(project.Views.ViewDir);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                project = null;
                errMsg = ex.BuildErrorMessage(AdminPhrases.CreateProjectError);
                return false;
            }
        }

        /// <summary>
        /// Loads the project description from the specified project file.
        /// </summary>
        public static bool LoadDescription(string fileName, out string description, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.Load(fileName);
                description = xmlDoc.DocumentElement.GetChildAsString("Description");
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                description = "";
                errMsg = ex.BuildErrorMessage(AdminPhrases.LoadProjectDescrError);
                return false;
            }
        }
    }
}
