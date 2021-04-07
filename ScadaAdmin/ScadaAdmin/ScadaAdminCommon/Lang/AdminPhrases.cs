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
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Project");
            CreateAppConfigError = dict.GetPhrase("CreateAppConfigError");
            DeleteAppConfigError = dict.GetPhrase("DeleteAppConfigError");

            dict = Locale.GetDictionary("Scada.Admin.Project.ConfigBase");
            LoadConfigBaseError = dict.GetPhrase("LoadConfigBaseError");
            SaveConfigBaseError = dict.GetPhrase("SaveConfigBaseError");
            LoadBaseTableError = dict.GetPhrase("LoadBaseTableError");
            SaveBaseTableError = dict.GetPhrase("SaveBaseTableError");

            dict = Locale.GetDictionary("Scada.Admin.Project.ProjectInstance");
            CreateInstanceFilesError = dict.GetPhrase("CreateInstanceFilesError");
            DeleteInstanceFilesError = dict.GetPhrase("DeleteInstanceFilesError");
            RenameInstanceError = dict.GetPhrase("RenameInstanceError");
            InstanceNameEmpty = dict.GetPhrase("InstanceNameEmpty");
            InstanceNameInvalid = dict.GetPhrase("InstanceNameInvalid");

            dict = Locale.GetDictionary("Scada.Admin.Project.ScadaProject");
            CreateProjectError = dict.GetPhrase("CreateProjectError");
            LoadProjectError = dict.GetPhrase("LoadProjectError");
            SaveProjectError = dict.GetPhrase("SaveProjectError");
            LoadProjectDescrError = dict.GetPhrase("LoadProjectDescrError");
            ProjectNameEmpty = dict.GetPhrase("ProjectNameEmpty");
            ProjectNameInvalid = dict.GetPhrase("ProjectNameInvalid");
            RenameProjectError = dict.GetPhrase("RenameProjectError");
            ProjectDirectoryExists = dict.GetPhrase("ProjectDirectoryExists");
        }
    }
}
