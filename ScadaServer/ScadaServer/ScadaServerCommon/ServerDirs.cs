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
 * Module   : ScadaServerCommon
 * Summary  : Represents directories of the Server application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2022
 */

namespace Scada.Server
{
    /// <summary>
    /// Represents directories of the Server application.
    /// <para>Представляет директории приложения Сервер.</para>
    /// </summary>
    public class ServerDirs : AppDirs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerDirs()
            : base()
        {
            ModDir = "";
        }


        /// <summary>
        /// Gets the directory of modules.
        /// </summary>
        public string ModDir { get; protected set; }


        /// <summary>
        /// Initializes the directories based on the directory of the executable file.
        /// </summary>
        public override void Init(string exeDir)
        {
            base.Init(exeDir);
            ModDir = AppendDir(ExeDir, "Mod");
        }

        /// <summary>
        /// Gets the directories required for the application.
        /// </summary>
        public override string[] GetRequiredDirs()
        {
            return new string[] { LangDir, LogDir, ModDir };
        }

        /// <summary>
        /// Gets the directories to search for assemblies.
        /// </summary>
        public override string[] GetProbingDirs()
        {
            return new string[] { ExeDir, ModDir };
        }
    }
}
