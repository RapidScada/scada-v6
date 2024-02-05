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
 * Summary  : Represents directories of the Administrator application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2024
 */

namespace Scada.Admin
{
    /// <summary>
    /// Represents directories of the Administrator application.
    /// <para>Представляет директории приложения Администратор.</para>
    /// </summary>
    public class AdminDirs : AppDirs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AdminDirs()
            : base()
        {
            CommonDataDir = "";
            LibDir = "";
            TemplateDir = "";
        }

        /// <summary>
        /// Gets the common application data directory.
        /// </summary>
        public string CommonDataDir { get; protected set; }

        /// <summary>
        /// Gets the directory of modules, drivers and other external libraries.
        /// </summary>
        public string LibDir { get; protected set; }

        /// <summary>
        /// Gets the directory of project templates.
        /// </summary>
        public string TemplateDir { get; protected set; }


        /// <summary>
        /// Initializes the directories based on the executable file directory and common application data directory.
        /// </summary>
        public override void Init(string exeDir, string commonDataDir)
        {
            base.Init(exeDir, commonDataDir);
            CommonDataDir = commonDataDir;
            LibDir = AppendDir(exeDir, "Lib");
            TemplateDir = AppendDir(exeDir, "Templates");
        }

        /// <summary>
        /// Gets the application data directories.
        /// </summary>
        public override string[] GetDataDirs()
        {
            return new string[] { ConfigDir, LogDir, TempDir };
        }

        /// <summary>
        /// Gets the directories to search for assemblies.
        /// </summary>
        public override string[] GetProbingDirs()
        {
            return new string[] { ExeDir, LibDir };
        }

        /// <summary>
        /// Creates a directory object for a library view based on the current object.
        /// </summary>
        public AppDirs CreateDirsForView(string configDir)
        {
            AdminDirs appDirs = new();
            appDirs.Init(ExeDir, CommonDataDir);
            appDirs.ConfigDir = configDir;
            return appDirs;
        }
    }
}
