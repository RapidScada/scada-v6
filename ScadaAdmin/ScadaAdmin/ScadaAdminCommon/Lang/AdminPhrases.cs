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
 * Summary  : The phrases used by the Administrator application and its extensions
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;

namespace Scada.Admin.Lang
{
    /// <summary>
    /// The phrases used by the Administrator application and its extensions.
    /// <para>Фразы, используемые приложением Администратор и его расширениями.</para>
    /// </summary>
    public static class AdminPhrases
    {
        // Locale specific phrases
        public static string ErrorInExtension { get; private set; }
        public static string ExtensionMessage { get; private set; }

        // Scada.Admin.Deployment.DeploymentConfig
        public static string LoadDeploymentConfigError { get; private set; }
        public static string SaveDeploymentConfigError { get; private set; }

        // Scada.Admin.Extenstions
        public static string AgentDisabled { get; private set; }
        public static string FileLoading { get; private set; }

        // Scada.Admin.Project
        public static string CreateAppConfigError { get; private set; }
        public static string DeleteAppConfigError { get; private set; }

        // Scada.Admin.Project.ConfigBase
        public static string LoadConfigBaseError { get; private set; }
        public static string SaveConfigBaseError { get; private set; }
        public static string LoadBaseTableError { get; private set; }
        public static string SaveBaseTableError { get; private set; }

        // Scada.Admin.Project.ProjectInstance
        public static string CreateInstanceFilesError { get; private set; }
        public static string DeleteInstanceFilesError { get; private set; }
        public static string RenameInstanceError { get; private set; }
        public static string InstanceNameEmpty { get; private set; }
        public static string InstanceNameInvalid { get; private set; }

        // Scada.Admin.Project.ScadaProject
        public static string CreateProjectError { get; private set; }
        public static string LoadProjectError { get; private set; }
        public static string SaveProjectError { get; private set; }
        public static string LoadProjectDescrError { get; private set; }
        public static string ProjectNameEmpty { get; private set; }
        public static string ProjectNameInvalid { get; private set; }
        public static string RenameProjectError { get; private set; }
        public static string ProjectDirectoryExists { get; private set; }

        public static void Init()
        {
            // set phrases depending on the locale
            if (Locale.IsRussian)
            {
                ErrorInExtension = "Ошибка при вызове метода {0} расширения {1}";
                ExtensionMessage = "Расширение {0}: {1}";
            }
            else
            {
                ErrorInExtension = "Error calling the {0} method of the {1} extension";
                ExtensionMessage = "Extension {0}: {1}";
            }

            // load phrases from dictionaries
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Deployment.DeploymentConfig");
            LoadDeploymentConfigError = dict["LoadDeploymentConfigError"];
            SaveDeploymentConfigError = dict["SaveDeploymentConfigError"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions");
            AgentDisabled = dict["AgentDisabled"];
            FileLoading = dict["FileLoading"];

            dict = Locale.GetDictionary("Scada.Admin.Project");
            CreateAppConfigError = dict["CreateAppConfigError"];
            DeleteAppConfigError = dict["DeleteAppConfigError"];

            dict = Locale.GetDictionary("Scada.Admin.Project.ConfigBase");
            LoadConfigBaseError = dict["LoadConfigBaseError"];
            SaveConfigBaseError = dict["SaveConfigBaseError"];
            LoadBaseTableError = dict["LoadBaseTableError"];
            SaveBaseTableError = dict["SaveBaseTableError"];

            dict = Locale.GetDictionary("Scada.Admin.Project.ProjectInstance");
            CreateInstanceFilesError = dict["CreateInstanceFilesError"];
            DeleteInstanceFilesError = dict["DeleteInstanceFilesError"];
            RenameInstanceError = dict["RenameInstanceError"];
            InstanceNameEmpty = dict["InstanceNameEmpty"];
            InstanceNameInvalid = dict["InstanceNameInvalid"];

            dict = Locale.GetDictionary("Scada.Admin.Project.ScadaProject");
            CreateProjectError = dict["CreateProjectError"];
            LoadProjectError = dict["LoadProjectError"];
            SaveProjectError = dict["SaveProjectError"];
            LoadProjectDescrError = dict["LoadProjectDescrError"];
            ProjectNameEmpty = dict["ProjectNameEmpty"];
            ProjectNameInvalid = dict["ProjectNameInvalid"];
            RenameProjectError = dict["RenameProjectError"];
            ProjectDirectoryExists = dict["ProjectDirectoryExists"];
        }
    }
}
