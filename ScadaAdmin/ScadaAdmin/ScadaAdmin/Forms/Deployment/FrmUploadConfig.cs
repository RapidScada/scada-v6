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
 * Summary  : Represents a form for uploading configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Client;
using Scada.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for uploading configuration.
    /// <para>Представляет форму для передачи конфигурации.</para>
    /// </summary>
    public partial class FrmUploadConfig : Form
    {
        private readonly AppData appData;                 // the common data of the application
        private readonly ScadaProject project;            // the project under development
        private readonly ProjectInstance projectInstance; // the affected instance
        private DeploymentProfile initialProfile;         // the initial deployment profile
        //private ConnectionOptions initialConnSettings;  // copy of the initial connection settings
        private bool uploadSettingsModified;              // the selected upload settings were modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmUploadConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmUploadConfig(AppData appData, ScadaProject project, ProjectInstance projectInstance)
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
        /// Gets a value indicating whether the configuration database was modified
        /// </summary>
        public bool BaseModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the interface files were modified
        /// </summary>
        public bool InterfaceModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the instance was modified
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
                string.Format("upload-config_{0}.zip", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
        }

        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        private bool UploadConfig(DeploymentProfile profile)
        {
            return false;
            /*string configFileName = GetTempFileName();

            try
            {
                Cursor = Cursors.WaitCursor;
                DateTime t0 = DateTime.UtcNow;

                // prepare an archive
                ImportExport importExport = new ImportExport();
                importExport.ExportToArchive(configFileName, project, projectInstance, profile.UploadSettings);
                FileInfo configFileInfo = new FileInfo(configFileName);
                long configFileSize = configFileInfo.Length;

                // upload the configuration
                ConnectionSettings connSettings = profile.ConnectionSettings.Clone();
                connSettings.ScadaInstance = projectInstance.Name;
                IAgentClient agentClient = new AgentWcfClient(connSettings);
                agentClient.UploadConfig(configFileName, profile.UploadSettings.ToConfigOpions());

                // restart the services
                if (profile.UploadSettings.RestartServer &&
                    (profile.UploadSettings.IncludeBase || profile.UploadSettings.IncludeServer))
                {
                    agentClient.ControlService(ServiceApp.Server, ServiceCommand.Restart);
                }

                if (profile.UploadSettings.RestartComm &&
                    (profile.UploadSettings.IncludeBase || profile.UploadSettings.IncludeComm))
                {
                    agentClient.ControlService(ServiceApp.Comm, ServiceCommand.Restart);
                }

                // show result
                Cursor = Cursors.Default;
                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.UploadConfigComplete,
                    Math.Round((DateTime.UtcNow - t0).TotalSeconds), configFileSize));
                return true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                appData.ProcError(ex, AppPhrases.UploadConfigError);
                return false;
            }
            finally
            {
                // delete temporary file
                try { File.Delete(configFileName); }
                catch { }
            }*/
        }


        private void FrmUploadConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlProfileSelector, ctrlProfileSelector.GetType().FullName);
            FormTranslator.Translate(ctrlTransferSettings, ctrlTransferSettings.GetType().FullName);

            ProfileChanged = false;
            ConnSettingsModified = false;

            ctrlTransferSettings.Init(project.ConfigBase);
            ctrlTransferSettings.Disable();
            ctrlProfileSelector.Init(appData, project.DeploymentConfig, projectInstance);

            initialProfile = ctrlProfileSelector.SelectedProfile;
            //initialConnSettings = initialProfile?.AgentConnectionOptions.Clone();
            uploadSettingsModified = false;
        }

        private void FrmUploadConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ConnSettingsModified = !ProfileChanged &&
            //    !ConnectionSettings.Equals(initialConnSettings, initialProfile?.ConnectionSettings);
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            // display upload settings of the selected profile
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile == null)
            {
                ctrlTransferSettings.Disable();
                btnUpload.Enabled = false;
            }
            else
            {
                ctrlTransferSettings.OptionsToControls(profile.UploadOptions);
                btnUpload.Enabled = true;
            }

            uploadSettingsModified = false;
        }

        private void ctrlTransferSettings_SettingsChanged(object sender, EventArgs e)
        {
            uploadSettingsModified = true;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            // validate settings and upload
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile != null && ctrlTransferSettings.ValidateControls())
            {
                // save the settings changes
                if (uploadSettingsModified)
                {
                    ctrlTransferSettings.ControlsToOptions(profile.UploadOptions);
                    SaveDeploymentConfig();
                }

                // upload
                projectInstance.DeploymentProfile = profile.Name;
                if (UploadConfig(profile))
                {
                    ProfileChanged = initialProfile != profile;
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
