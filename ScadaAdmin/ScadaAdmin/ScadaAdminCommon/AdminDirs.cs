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
 * Summary  : Represents directories of the Administrator application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using System.IO;

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
            LibDir = "";
            TemplateDir = "";
        }


        /// <summary>
        /// Gets the directory of modules, drivers and other external libraries.
        /// </summary>
        public string LibDir { get; protected set; }

        /// <summary>
        /// Gets the directory of project templates.
        /// </summary>
        public string TemplateDir { get; protected set; }


        /// <summary>
        /// Initializes the directories based on the directory of the executable file.
        /// </summary>
        public override void Init(string exeDir)
        {
            base.Init(exeDir);
            LibDir = ExeDir + "Lib" + Path.DirectorySeparatorChar;
            TemplateDir = ExeDir + "Templates" + Path.DirectorySeparatorChar;
        }
    }
}
