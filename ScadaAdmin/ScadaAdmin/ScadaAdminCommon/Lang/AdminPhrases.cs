﻿/*
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
 * Summary  : The phrases used by the Administrator application and its extensions
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
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

        // Scada.Admin.Extensions
        public static string AgentNotEnabled { get; private set; }
        public static string DbNotEnabled { get; private set; }
        public static string UploadConfig { get; private set; }
        public static string UploadConfigCompleted { get; private set; }
        public static string DownloadConfig { get; private set; }
        public static string DownloadConfigCompleted { get; private set; }
        public static string DownloadBase { get; private set; }
        public static string DownloadViews { get; private set; }
        public static string DownloadAppConfig { get; private set; }
        public static string FileCount { get; private set; }
        public static string FileLoading { get; private set; }
        public static string StartNamedService { get; private set; }
        public static string StopNamedService { get; private set; }
        public static string RestartNamedService { get; private set; }
        public static string ServiceCommandCompleted { get; private set; }
        public static string ServiceCommandFailed { get; private set; }
        public static string EmptyDevice { get; private set; }
        public static string EmptyObject { get; private set; }

        // Scada.Admin.Forms.FrmLogs
        public static string AllFilesFilter { get; private set; }

        // Scada.Admin.Forms.FrmRegistration
        public static string LoadRegKeyError { get; private set; }
        public static string SaveRegKeyError { get; private set; }

        // Scada.Admin.Project
        public static string CreateAppConfigError { get; private set; }
        public static string DeleteAppConfigError { get; private set; }

        // Scada.Admin.Project.ConfigDatabase
        public static string LoadConfigDatabaseError { get; private set; }
        public static string SaveConfigDatabaseError { get; private set; }
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
            LoadDeploymentConfigError = dict[nameof(LoadDeploymentConfigError)];
            SaveDeploymentConfigError = dict[nameof(SaveDeploymentConfigError)];

            dict = Locale.GetDictionary("Scada.Admin.Extensions");
            AgentNotEnabled = dict[nameof(AgentNotEnabled)];
            DbNotEnabled = dict[nameof(DbNotEnabled)];
            UploadConfig = dict[nameof(UploadConfig)];
            UploadConfigCompleted = dict[nameof(UploadConfigCompleted)];
            DownloadConfig = dict[nameof(DownloadConfig)];
            DownloadConfigCompleted = dict[nameof(DownloadConfigCompleted)];
            DownloadBase = dict[nameof(DownloadBase)];
            DownloadViews = dict[nameof(DownloadViews)];
            DownloadAppConfig = dict[nameof(DownloadAppConfig)];
            FileCount = dict[nameof(FileCount)];
            FileLoading = dict[nameof(FileLoading)];
            StartNamedService = dict[nameof(StartNamedService)];
            StopNamedService = dict[nameof(StopNamedService)];
            RestartNamedService = dict[nameof(RestartNamedService)];
            ServiceCommandCompleted = dict[nameof(ServiceCommandCompleted)];
            ServiceCommandFailed = dict[nameof(ServiceCommandFailed)];
            EmptyDevice = dict[nameof(EmptyDevice)];
            EmptyObject = dict[nameof(EmptyObject)];

            dict = Locale.GetDictionary("Scada.Admin.Forms.FrmLogs");
            AllFilesFilter = dict[nameof(AllFilesFilter)];

            dict = Locale.GetDictionary("Scada.Admin.Forms.FrmRegistration");
            LoadRegKeyError = dict[nameof(LoadRegKeyError)];
            SaveRegKeyError = dict[nameof(SaveRegKeyError)];

            dict = Locale.GetDictionary("Scada.Admin.Project");
            CreateAppConfigError = dict[nameof(CreateAppConfigError)];
            DeleteAppConfigError = dict[nameof(DeleteAppConfigError)];

            dict = Locale.GetDictionary("Scada.Admin.Project.ConfigDatabase");
            LoadConfigDatabaseError = dict[nameof(LoadConfigDatabaseError)];
            SaveConfigDatabaseError = dict[nameof(SaveConfigDatabaseError)];
            LoadBaseTableError = dict[nameof(LoadBaseTableError)];
            SaveBaseTableError = dict[nameof(SaveBaseTableError)];

            dict = Locale.GetDictionary("Scada.Admin.Project.ProjectInstance");
            CreateInstanceFilesError = dict[nameof(CreateInstanceFilesError)];
            DeleteInstanceFilesError = dict[nameof(DeleteInstanceFilesError)];
            RenameInstanceError = dict[nameof(RenameInstanceError)];
            InstanceNameEmpty = dict[nameof(InstanceNameEmpty)];
            InstanceNameInvalid = dict[nameof(InstanceNameInvalid)];

            dict = Locale.GetDictionary("Scada.Admin.Project.ScadaProject");
            CreateProjectError = dict[nameof(CreateProjectError)];
            LoadProjectError = dict[nameof(LoadProjectError)];
            SaveProjectError = dict[nameof(SaveProjectError)];
            LoadProjectDescrError = dict[nameof(LoadProjectDescrError)];
            ProjectNameEmpty = dict[nameof(ProjectNameEmpty)];
            ProjectNameInvalid = dict[nameof(ProjectNameInvalid)];
            RenameProjectError = dict[nameof(RenameProjectError)];
            ProjectDirectoryExists = dict[nameof(ProjectDirectoryExists)];
        }
    }
}
