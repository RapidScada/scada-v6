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
 * Module   : Administrator
 * Summary  : Contains the common application data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.Config;
using Scada.Config;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.IO;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Contains the common application data.
    /// <para>Содержит общие данные приложения.</para>
    /// </summary>
    public sealed class AppData
    {
        /// <summary>
        /// The short name of the application error log file.
        /// </summary>
        private const string ErrFileName = "ScadaAdmin.err";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppData()
        {
            AppDirs = new AdminDirs();
            ErrLog = LogStub.Instance;
            Config = new AdminConfig();
            State = new AppState();
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AdminDirs AppDirs { get; }

        /// <summary>
        /// Gets the application error log.
        /// </summary>
        public ILog ErrLog { get; private set; }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public AdminConfig Config { get; }

        /// <summary>
        /// Gets the application state.
        /// </summary>
        public AppState State { get; }


        /// <summary>
        /// Clears the temporary directory.
        /// </summary>
        private void ClearTempDir()
        {
            try
            {
                DirectoryInfo directoryInfo = new(AppDirs.TempDir);

                if (directoryInfo.Exists)
                {
                    foreach (DirectoryInfo subdirInfo in directoryInfo.EnumerateDirectories())
                    {
                        subdirInfo.Delete(true);
                    }

                    foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles())
                    {
                        fileInfo.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrLog.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при очистке директории временных файлов" :
                    "Error cleaning the directory of temporary files");
            }
        }

        /// <summary>
        /// Initializes the common application data.
        /// </summary>
        public void Init(string exeDir)
        {
            AppDirs.Init(exeDir);

            ErrLog = new LogFile(LogFormat.Full)
            {
                FileName = Path.Combine(AppDirs.LogDir, ErrFileName)
            };
        }

        /// <summary>
        /// Makes finalization steps.
        /// </summary>
        public void FinalizeApp()
        {
            ClearTempDir();
        }

        /// <summary>
        /// Writes the error to the log and displays a error message.
        /// </summary>
        public void ProcError(string text)
        {
            ErrLog.WriteError(text);
            ScadaUiUtils.ShowError(text);
        }

        /// <summary>
        /// Writes the error to the log and displays a error message.
        /// </summary>
        public void ProcError(Exception ex, string text = "", params object[] args)
        {
            string msg = ScadaUtils.BuildErrorMessage(ex, text, args);
            ErrLog.WriteMessage(msg, LogMessageType.Exception);
            ScadaUiUtils.ShowError(msg);
        }

        /// <summary>
        /// Gets the full file name of the instance configuration.
        /// </summary>
        public string GetInstanceConfigFileName()
        {
            return Path.Combine(AppDirs.ExeDir, "..", "Config", InstanceConfig.DefaultFileName);
        }
    }
}
