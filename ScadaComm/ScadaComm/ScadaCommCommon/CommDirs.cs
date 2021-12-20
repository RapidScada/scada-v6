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
 * Module   : ScadaCommCommon
 * Summary  : Represents directories of the Communicator application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using System.IO;

namespace Scada.Comm
{
    /// <summary>
    /// Represents directories of the Communicator application.
    /// <para>Представляет директории приложения Коммуникатор.</para>
    /// </summary>
    public class CommDirs : AppDirs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommDirs()
            : base()
        {
            DrvDir = "";
        }


        /// <summary>
        /// Gets the directory of drivers.
        /// </summary>
        public string DrvDir { get; protected set; }


        /// <summary>
        /// Initializes the directories based on the directory of the executable file.
        /// </summary>
        public override void Init(string exeDir)
        {
            base.Init(exeDir);
            DrvDir = AppendDir(ExeDir, "Drv");
        }

        /// <summary>
        /// Gets the directories required for the application.
        /// </summary>
        public override string[] GetRequiredDirs()
        {
            return new string[] { LangDir, LogDir, DrvDir };
        }

        /// <summary>
        /// Finds an assembly in the application directories.
        /// </summary>
        public bool FindAssembly(string simpleName, out string path)
        {
            foreach (string basePath in new string[] { ExeDir, DrvDir })
            {
                path = Path.Combine(basePath, simpleName + ".dll");

                if (File.Exists(path))
                    return true;
            }

            path = "";
            return false;
        }
    }
}
