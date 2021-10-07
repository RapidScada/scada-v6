/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Summary  : Represents a form for downloading configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Client;
using Scada.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for downloading configuration.
    /// <para>Представляет форму для скачивания конфигурации.</para>
    /// </summary>
    public partial class FrmDownloadConfig : Form
    {
        private readonly AppData appData;                 // the common data of the application
        private readonly ScadaProject project;            // the project under development
        private readonly ProjectInstance projectInstance; // the affected instance
        private DeploymentProfile initialProfile;         // the initial deployment profile
        //private ConnectionOptions initialConnSettings;  // copy of the initial connection settings
        private bool downloadSettingsModified;            // the selected download settings are modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDownloadConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDownloadConfig(AppData appData, ScadaProject project, ProjectInstance projectInstance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.projectInstance = projectInstance ?? throw new ArgumentNullException(nameof(projectInstance));
        }


        /// <summary>
        /// Gets a value indicating whether the selected profile changed.
        /// </summary>
        public bool ProfileChanged { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the connection settings were modified.
        /// </summary>
        public bool ConnSettingsModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the configuration database was modified.
        /// </summary>
        public bool BaseModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the interface files were modified.
        /// </summary>
        public bool InterfaceModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the instance was modified.
        /// </summary>
        public bool InstanceModified { get; protected set; }
        
        
        /// <summary>
        /// Saves the deployment configuration.
        /// </summary>
        private void SaveDeploymentConfig()
        {
            if (!project.DeploymentConfig.Save(out string errMsg))
                appData.ErrLog.HandleError(errMsg);
        }

        /// <summary>
        /// Gets a name for a temporary file.
        /// </summary>
        private string GetTempFileName()
        {
            return Path.Combine(appData.AppDirs.TempDir,
                string.Format("download-config_{0}.zip", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
        }

        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        private bool DownloadConfig(DeploymentProfile profile)
        {
            return false;
            /*string configFileName = GetTempFileName();

            try
            {
                Cursor = Cursors.WaitCursor;
                DateTime t0 = DateTime.UtcNow;

                // download the configuration
                ConnectionSettings connSettings = profile.ConnectionSettings.Clone();
                connSettings.ScadaInstance = projectInstance.Name;
                IAgentClient agentClient = new AgentWcfClient(connSettings);
                agentClient.DownloadConfig(configFileName, profile.DownloadSettings.ToConfigOpions());

                // import the configuration
                ImportExport importExport = new ImportExport();
                importExport.ImportArchive(configFileName, project, projectInstance, out ConfigParts foundConfigParts);
                FileInfo configFileInfo = new FileInfo(configFileName);
                long configFileSize = configFileInfo.Length;

                // set the modification flags
                BaseModified = foundConfigParts.HasFlag(ConfigParts.Base);
                InterfaceModified = foundConfigParts.HasFlag(ConfigParts.Interface);
                InstanceModified = foundConfigParts.HasFlag(ConfigParts.Server) ||
                    foundConfigParts.HasFlag(ConfigParts.Comm) || foundConfigParts.HasFlag(ConfigParts.Web);

                // show result
                Cursor = Cursors.Default;
                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.DownloadConfigComplete,
                    Math.Round((DateTime.UtcNow - t0).TotalSeconds), configFileSize));
                return true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                appData.ProcError(ex, AppPhrases.DownloadConfigError);
                return false;
            }
            finally
            {
                // delete temporary file
                try { File.Delete(configFileName); }
                catch { }
            }*/
        }


        private void FrmDownloadConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlProfileSelector, ctrlProfileSelector.GetType().FullName);
            FormTranslator.Translate(ctrlTransferSettings, ctrlTransferSettings.GetType().FullName);

            ProfileChanged = false;
            ConnSettingsModified = false;
            BaseModified = false;
            InterfaceModified = false;
            InstanceModified = false;

            ctrlTransferSettings.Disable();
            ctrlProfileSelector.Init(appData, project.DeploymentConfig, projectInstance);

            initialProfile = ctrlProfileSelector.SelectedProfile;
            //initialConnSettings = initialProfile?.AgentConnectionOptions.Clone();
            downloadSettingsModified = false;
        }

        private void FrmDownloadConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ConnSettingsModified = !ProfileChanged &&
            //    !ConnectionSettings.Equals(initialConnSettings, initialProfile?.ConnectionSettings);
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            // display download settings of the selected profile
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile == null)
            {
                ctrlTransferSettings.Disable();
                btnDownload.Enabled = false;
            }
            else
            {
                ctrlTransferSettings.OptionsToControls(profile.DownloadOptions);
                btnDownload.Enabled = true;
            }

            downloadSettingsModified = false;
        }

        private void ctrlTransferSettings_SettingsChanged(object sender, EventArgs e)
        {
            downloadSettingsModified = true;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // validate settings and download
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile != null && ctrlTransferSettings.ValidateControls())
            {
                // save the settings changes
                if (downloadSettingsModified)
                {
                    ctrlTransferSettings.ControlsToOptions(profile.DownloadOptions);
                    SaveDeploymentConfig();
                }

                // download
                projectInstance.DeploymentProfile = profile.Name;
                if (DownloadConfig(profile))
                {
                    ProfileChanged = initialProfile != profile;
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
