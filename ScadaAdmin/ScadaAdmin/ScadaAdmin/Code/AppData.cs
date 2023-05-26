/*
 * Copyright 2023 Rapid Software LLC
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
using Scada.Admin.Extensions;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Comm.Lang;
using Scada.Config;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Lang;
using System;
using System.IO;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Contains the common application data.
    /// <para>Содержит общие данные приложения.</para>
    /// </summary>
    public sealed class AppData : IAdminContext
    {
        private ScadaProject currentProject; // the project currently open


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppData()
        {
            AppConfig = new AdminConfig();
            State = new AppState();
            AppDirs = new AdminDirs();
            ErrLog = LogStub.Instance;
            ExtensionHolder = null;
            CurrentProject = null;
            MainForm = null;
        }

        public bool ExtSubFolder { get; private set; }
        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public AdminConfig AppConfig { get; }

        /// <summary>
        /// Gets the application state.
        /// </summary>
        public AppState State { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AdminDirs AppDirs { get; }

        /// <summary>
        /// Gets the application error log.
        /// </summary>
        /// <remarks>
        /// Use the log only for writing errors because many instances of the application can be open simultaneously.
        /// </remarks>
        public ILog ErrLog { get; private set; }

        /// <summary>
        /// Gets the extension holder.
        /// </summary>
        public ExtensionHolder ExtensionHolder { get; private set; }
        
        /// <summary>
        /// Gets or sets the project currently open.
        /// </summary>
        public ScadaProject CurrentProject
        {
            get
            {
                return currentProject;
            }
            set
            {
                ScadaProject prevProject = currentProject;
                currentProject = value;

                if (currentProject != prevProject)
                    OnCurrentProjectChanged();
            }
        }

        /// <summary>
        /// Gets or sets the referense to the main form.
        /// </summary>
        public IMainForm MainForm { get; set; }


        /// <summary>
        /// Localizes the application.
        /// </summary>
        private void LocalizeApp()
        {
            // load instance culture
            if (!Locale.LoadCulture(GetInstanceConfigFileName(), out string errMsg))
                ErrLog.WriteError(errMsg);

            // load dictionaries
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaCommon", out errMsg))
                ErrLog.WriteError(errMsg);

            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaAdmin", out errMsg))
                ErrLog.WriteError(errMsg);

            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaServer", out errMsg))
                ErrLog.WriteError(errMsg);

            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaComm", out errMsg))
                ErrLog.WriteError(errMsg);

            // read phrases from the dictionaries
            CommonPhrases.Init();
            AdminPhrases.Init();
            AppPhrases.Init();
            ServerPhrases.Init();
            CommPhrases.Init();
        }

        /// <summary>
        /// Loads the application configuration.
        /// </summary>
        private void LoadAppConfig()
        {
            string fileName = Path.Combine(AppDirs.ConfigDir, AdminConfig.DefaultFileName);

            if (File.Exists(fileName))
            {
                // load existing configuration
                if (!AppConfig.Load(fileName, out string errMsg))
                    ErrLog.WriteError(errMsg);
            }
            else
            {
                // use default configuration
                AppConfig.SetToDefault(AppDirs.InstanceDir);

                if (!AppConfig.Save(fileName, out string errMsg))
                    ErrLog.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Loads extenstions.
        /// </summary>
        private void LoadExtensions()
        {
            ExtensionHolder = new(ErrLog);

            foreach (string extensionCode in AppConfig.ExtensionCodes)
            {
                if (ExtensionFactory.GetExtensionLogic(AppDirs.LibDir, extensionCode, this,
                    out ExtensionLogic extensionLogic, out string message))
                {
                    ExtensionHolder.AddExtension(extensionLogic);
                }
                else
                {
                    ErrLog.WriteError(message);
                }
            }

            ExtensionHolder.LoadDictionaries();
            ExtensionHolder.LoadConfig();
        }

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
                ErrLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при очистке директории временных файлов" :
                    "Error cleaning the directory of temporary files");
            }
        }

        /// <summary>
        /// Raises a CurrentProjectChanged event.
        /// </summary>
        private void OnCurrentProjectChanged()
        {
            CurrentProjectChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Initializes the common application data.
        /// </summary>
        public void Init(string exeDir)
        {
            AppDirs.Init(exeDir, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            AppDirs.CreateDataDirs(out _);

            ErrLog = new LogFile(LogFormat.Full)
            {
                FileName = Path.Combine(AppDirs.LogDir, AdminUtils.LogFileName)
            };

            LocalizeApp();
            LoadAppConfig();
            LoadExtensions();
        }

        /// <summary>
        /// Makes finalization steps.
        /// </summary>
        public void FinalizeApp()
        {
            ClearTempDir();
        }

        /// <summary>
        /// Gets the full file name of the instance configuration.
        /// </summary>
        public string GetInstanceConfigFileName()
        {
            return Path.Combine(AppDirs.ExeDir, "..", "Config", InstanceConfig.DefaultFileName);
        }

        /// <summary>
        /// Sends the message to the extensions.
        /// </summary>
        public bool MessageToExtensions(MessageEventArgs e)
        {
            MessageToExtension?.Invoke(this, e);
            return e != null && e.BoolResult;
        }


        
        
        /// <summary>
        /// Occurs when the current project changes.
        /// </summary>
        public event EventHandler CurrentProjectChanged;

        /// <summary>
        /// Occurs when some extension sends a message to other extensions.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageToExtension;
    }
}
