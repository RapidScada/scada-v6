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
 * Summary  : Represents the Server application in a project
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Config;
using Scada.Lang;
using Scada.Server.Config;
using System.IO;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the Server application in a project.
    /// <para>Представляет приложение Сервер в проекте.</para>
    /// </summary>
    public class ServerApp : ProjectApp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerApp()
            : base()
        {
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public ServerConfig AppConfig { get; private set; }

        /// <summary>
        /// Gets the corresponding service application.
        /// </summary>
        public override ServiceApp ServiceApp => ServiceApp.Server;

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public override IConfig Config => AppConfig;


        /// <summary>
        /// Recreates the application configuration.
        /// </summary>
        protected override void RenewConfig()
        {
            AppConfig = new ServerConfig();
        }

        /// <summary>
        /// Gets the application configuration file path.
        /// </summary>
        protected override string GetConfigPath()
        {
            return Path.Combine(ConfigDir, ServerConfig.DefaultFileName);
        }

        /// <summary>
        /// Initializes the application directory relative to the instance directory.
        /// </summary>
        public override void InitAppDir(string instanceDir)
        {
            AppDir = string.IsNullOrEmpty(instanceDir) 
                ? "" 
                : ScadaUtils.NormalDir(Path.Combine(instanceDir, "ScadaServer"));
        }
    }
}
