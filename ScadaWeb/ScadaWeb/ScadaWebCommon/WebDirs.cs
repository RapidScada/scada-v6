/*
 * Copyright 2025 Rapid Software LLC
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
 * Module   : ScadaWebCommon
 * Summary  : Represents directories of the web application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2025
 */

namespace Scada.Web
{
    /// <summary>
    /// Represents directories of the web application.
    /// <para>Представляет директории веб-приложения.</para>
    /// </summary>
    public class WebDirs : AppDirs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebDirs()
            : base()
        {
            Lowercase = true;
            PluginDir = "";
        }


        /// <summary>
        /// Gets the directory of plugins.
        /// </summary>
        public string PluginDir { get; protected set; }


        /// <summary>
        /// Initializes the directories based on the directory of the executable file.
        /// </summary>
        public override void Init(string exeDir)
        {
            base.Init(exeDir);
            PluginDir = ScadaUtils.NormalDir(Path.Combine(ExeDir, "wwwroot", "plugins"));
        }

        /// <summary>
        /// Gets the directories to search for assemblies.
        /// </summary>
        public override string[] GetProbingDirs()
        {
            return [ExeDir, PluginDir];
        }
    }
}
