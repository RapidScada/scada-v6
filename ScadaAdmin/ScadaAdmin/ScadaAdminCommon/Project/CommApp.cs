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
 * Summary  : Represents the Communicator application in a project
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Comm.Config;
using Scada.Config;
using Scada.Lang;
using System.IO;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the Communicator application in a project.
    /// <para>Представляет приложение Коммуникатор в проекте.</para>
    /// </summary>
    public class CommApp : ProjectApp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommApp()
            : base()
        {
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public CommConfig AppConfig { get; private set; }

        /// <summary>
        /// Gets the corresponding service application.
        /// </summary>
        public override ServiceApp ServiceApp => ServiceApp.Comm;

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public override IConfig Config => AppConfig;


        /// <summary>
        /// Recreates the application configuration.
        /// </summary>
        protected override void RenewConfig()
        {
            AppConfig = new CommConfig();
        }

        /// <summary>
        /// Gets the application configuration file path.
        /// </summary>
        protected override string GetConfigPath()
        {
            return Path.Combine(ConfigDir, CommConfig.DefaultFileName);
        }

        /// <summary>
        /// Initializes the application directory relative to the instance directory.
        /// </summary>
        public override void InitAppDir(string instanceDir)
        {
            AppDir = string.IsNullOrEmpty(instanceDir) 
                ? "" 
                : ScadaUtils.NormalDir(Path.Combine(instanceDir, "ScadaComm"));
        }
    }
}
