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
 * Module   : ScadaCommon
 * Summary  : Represents application directories
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using Scada.Lang;
using System;
using System.IO;

namespace Scada
{
    /// <summary>
    /// Represents application directories.
    /// <para>Представляет директории приложения.</para>
    /// </summary>
    public class AppDirs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppDirs()
        {
            InstanceDir = "";
            ExeDir = "";
            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            StorageDir = "";
            TempDir = "";
        }


        /// <summary>
        /// Gets the directory of the current instance.
        /// </summary>
        public string InstanceDir { get; protected set; }

        /// <summary>
        /// Gets the directory of the executable file.
        /// </summary>
        public string ExeDir { get; protected set; }

        /// <summary>
        /// Gets the command directory.
        /// </summary>
        /// <remarks>This directory is used by the console application.</remarks>
        public string CmdDir { get; protected set; }

        /// <summary>
        /// Gets the directory of the application configuration.
        /// </summary>
        public string ConfigDir { get; protected set; }

        /// <summary>
        /// Gets the directory of language files.
        /// </summary>
        public string LangDir { get; protected set; }

        /// <summary>
        /// Gets the directory of log files.
        /// </summary>
        public string LogDir { get; protected set; }

        /// <summary>
        /// Gets the storage directory.
        /// </summary>
        public string StorageDir { get; protected set; }

        /// <summary>
        /// Gets the directory of temporary files.
        /// </summary>
        public string TempDir { get; protected set; }

        /// <summary>
        /// Checks that the directories exist.
        /// </summary>
        public bool Exist
        {
            get
            {
                foreach (string dir in GetRequiredDirs())
                {
                    if (!Directory.Exists(dir))
                        return false;
                }

                return true;
            }
        }


        /// <summary>
        /// Checks that the directories exist.
        /// </summary>
        public bool CheckExistence(out string errMsg)
        {
            if (Exist)
            {
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = string.Format(Locale.IsRussian ?
                    "Директории приложения не существуют:{0}{1}" :
                    "Application directories do not exist:{0}{1}",
                    Environment.NewLine, string.Join(Environment.NewLine, GetRequiredDirs()));
                return false;
            }
        }

        /// <summary>
        /// Initializes the directories based on the directory of the executable file.
        /// </summary>
        public virtual void Init(string exeDir)
        {
            ExeDir = ScadaUtils.NormalDir(exeDir);
            InstanceDir = ScadaUtils.NormalDir(Directory.GetParent(ExeDir).FullName);
            CmdDir = ExeDir + "Cmd" + Path.DirectorySeparatorChar;
            ConfigDir = ExeDir + "Config" + Path.DirectorySeparatorChar;
            LangDir = ExeDir + "Lang" + Path.DirectorySeparatorChar;
            LogDir = ExeDir + "Log" + Path.DirectorySeparatorChar;
            StorageDir = ExeDir + "Storage" + Path.DirectorySeparatorChar;
            TempDir = ExeDir + "Temp" + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Gets the directories required for the application.
        /// </summary>
        public virtual string[] GetRequiredDirs()
        {
            return Array.Empty<string>();
        }
    }
}
