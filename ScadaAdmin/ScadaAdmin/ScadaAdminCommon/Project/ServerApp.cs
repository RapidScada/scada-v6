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
 * Summary  : Represents the Server application in a project
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Server.Config;
using System;
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
            ClearConfig();
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public ServerConfig Config { get; protected set; }


        /// <summary>
        /// Gets the application configuration file path.
        /// </summary>
        private string GetConfigPath()
        {
            return Path.Combine(GetConfigDir(), ServerConfig.DefaultFileName);
        }

        /// <summary>
        /// Gets the application configuration directory.
        /// </summary>
        public string GetConfigDir()
        {
            return Path.Combine(AppDir, "Config");
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        public bool LoadConfig(out string errMsg)
        {
            return Config.Load(GetConfigPath(), out errMsg);
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        public bool SaveConfig(out string errMsg)
        {
            return Config.Save(GetConfigPath(), out errMsg);
        }

        /// <summary>
        /// Creates configuration files required for the application.
        /// </summary>
        public override bool CreateConfigFiles(out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(GetConfigDir());
                return SaveConfig(out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, AdminPhrases.CreateAppConfigError, 
                    CommonPhrases.ServerAppName);
                return false;
            }
        }

        /// <summary>
        /// Deletes configuration files of the application.
        /// </summary>
        public override bool DeleteConfigFiles(out string errMsg)
        {
            try
            {
                Directory.Delete(AppDir, true);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, AdminPhrases.DeleteAppConfigError, 
                    CommonPhrases.ServerAppName);
                return false;
            }
        }

        /// <summary>
        /// Clears the application configuration.
        /// </summary>
        public override void ClearConfig()
        {
            Config = new ServerConfig();
        }

        /// <summary>
        /// Gets the application directory.
        /// </summary>
        public static string GetAppDir(string parentDir)
        {
            return Path.Combine(parentDir, "ScadaServer");
        }
    }
}
