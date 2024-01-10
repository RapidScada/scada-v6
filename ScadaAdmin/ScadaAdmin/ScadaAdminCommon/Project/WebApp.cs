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
 * Summary  : Represents the Webstation application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Config;
using Scada.Lang;
using Scada.Web.Config;
using System.IO;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the Webstation application
    /// <para>Представляет приложение Вебстанция</para>
    /// </summary>
    public class WebApp : ProjectApp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebApp()
            : base()
        {
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public WebConfig AppConfig { get; private set; }

        /// <summary>
        /// Gets the corresponding service application.
        /// </summary>
        public override ServiceApp ServiceApp => ServiceApp.Web;

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public override IConfig Config => AppConfig;

        /// <summary>
        /// Gets the application configuration directory.
        /// </summary>
        public override string ConfigDir
        {
            get
            {
                return string.IsNullOrEmpty(AppDir) 
                    ? "" 
                    : ScadaUtils.NormalDir(Path.Combine(AppDir, "config"));
            }
        }


        /// <summary>
        /// Recreates the application configuration.
        /// </summary>
        protected override void RenewConfig()
        {
            AppConfig = new WebConfig();
        }

        /// <summary>
        /// Gets the application configuration file path.
        /// </summary>
        protected override string GetConfigPath()
        {
            return Path.Combine(ConfigDir, WebConfig.DefaultFileName);
        }

        /// <summary>
        /// Initializes the application directory relative to the instance directory.
        /// </summary>
        public override void InitAppDir(string instanceDir)
        {
            AppDir = string.IsNullOrEmpty(instanceDir) 
                ? "" 
                : ScadaUtils.NormalDir(Path.Combine(instanceDir, "ScadaWeb"));
        }
    }
}
