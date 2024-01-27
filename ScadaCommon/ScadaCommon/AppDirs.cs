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
 * Module   : ScadaCommon
 * Summary  : Represents application directories
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2023
 */

using Scada.Lang;
using System;
using System.IO;
using System.Reflection;

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
            Lowercase = false;
            InstanceDir = "";
            ExeDir = "";
            CmdDir = "";
            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            StorageDir = "";
            TempDir = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether application directories should be lowercase.
        /// </summary>
        public bool Lowercase { get; set; }

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
        /// Checks that the required directories exist.
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
        /// Appends the child directory to the parent directory.
        /// </summary>
        protected virtual string AppendDir(string parentDir, string childDir)
        {
            return ScadaUtils.NormalDir(Path.Combine(parentDir, Lowercase ? childDir.ToLowerInvariant() : childDir));
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
            Init(exeDir, null);
        }

        /// <summary>
        /// Initializes the directories based on the executable file directory and common application data directory.
        /// </summary>
        /// <remarks>Common application data directory should be writable.</remarks>
        public virtual void Init(string exeDir, string commonDataDir)
        {
            if (string.IsNullOrEmpty(exeDir))
                throw new ArgumentException("Executable directory must not be empty.", nameof(exeDir));

            string dataDir = string.IsNullOrEmpty(commonDataDir)
                ? exeDir
                : Path.Combine(commonDataDir, new DirectoryInfo(exeDir).Name);

            ExeDir = ScadaUtils.NormalDir(exeDir);
            InstanceDir = ScadaUtils.NormalDir(Path.GetFullPath(Path.Combine(exeDir, "..")));
            CmdDir = AppendDir(dataDir, "Cmd");
            ConfigDir = AppendDir(dataDir, "Config");
            LangDir = AppendDir(exeDir, "Lang");
            LogDir = AppendDir(dataDir, "Log");
            StorageDir = AppendDir(dataDir, "Storage");
            TempDir = AppendDir(dataDir, "Temp");
        }

        /// <summary>
        /// Initializes the directories based on the assembly location.
        /// </summary>
        public virtual void Init(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            Init(Path.GetDirectoryName(assembly.Location));
        }

        /// <summary>
        /// Updates the log directory and creates it, if necessary.
        /// </summary>
        public virtual bool UpdateLogDir(string instanceLogDir, out string errMsg)
        {
            if (!string.IsNullOrEmpty(instanceLogDir))
            {
                DirectoryInfo exeDirInfo = new DirectoryInfo(ExeDir);
                LogDir = AppendDir(Path.Combine(instanceLogDir, exeDirInfo.Name), "Log");
            }

            if (string.IsNullOrEmpty(LogDir))
            {
                errMsg = "Log directory is empty";
                return false;
            }

            return CreateLogDir(out errMsg);
        }

        /// <summary>
        /// Updates the log directory, logging possible errors to the console.
        /// </summary>
        public virtual void UpdateLogDir(string instanceLogDir)
        {
            if (!UpdateLogDir(instanceLogDir, out string errMsg))
                Console.WriteLine(errMsg);
        }

        /// <summary>
        /// Creates the log directory if necessary.
        /// </summary>
        public virtual bool CreateLogDir(out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(LogDir);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при создании директории журналов" :
                    "Error creating log directory");
                return false;
            }
        }

        /// <summary>
        /// Creates the log directory, logging possible errors to the console.
        /// </summary>
        public virtual void CreateLogDir()
        {
            if (!CreateLogDir(out string errMsg))
                Console.WriteLine(errMsg);
        }

        /// <summary>
        /// Creates the application data directories if necessary.
        /// </summary>
        public virtual bool CreateDataDirs(out string errMsg)
        {
            try
            {
                foreach (string dir in GetDataDirs())
                {
                    Directory.CreateDirectory(dir);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при создании директорий данных" :
                    "Error creating data directories");
                return false;
            }
        }

        /// <summary>
        /// Gets the directories required for the application.
        /// </summary>
        public virtual string[] GetRequiredDirs()
        {
            return Array.Empty<string>();
        }

        /// <summary>
        /// Gets the application data directories.
        /// </summary>
        public virtual string[] GetDataDirs()
        {
            return Array.Empty<string>();
        }

        /// <summary>
        /// Gets the directories to search for assemblies.
        /// </summary>
        public virtual string[] GetProbingDirs()
        {
            return new string[] { ExeDir };
        }
    }
}
