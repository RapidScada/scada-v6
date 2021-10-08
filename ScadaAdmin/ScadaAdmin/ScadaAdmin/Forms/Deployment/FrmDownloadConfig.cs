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
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for downloading configuration.
    /// <para>Представляет форму для скачивания конфигурации.</para>
    /// </summary>
    public partial class FrmDownloadConfig : Form, IDeploymentForm
    {
        private readonly AppData appData;                   // the common data of the application
        private readonly ScadaProject project;              // the project under development
        private readonly ProjectInstance instance;          // the affected instance
        private readonly string initialProfileName;         // the initial instance profile name
        private ConnectionOptions initialConnectionOptions; // the copy of the initial Agent connection options
        private bool transferOptionsModified;               // the selected upload options were modified


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
        public FrmDownloadConfig(AppData appData, ScadaProject project, ProjectInstance instance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            initialProfileName = instance.DeploymentProfile;
            initialConnectionOptions = null;
            transferOptionsModified = false;

            ProfileChanged = false;
            ConnectionModified = false;
            BaseModified = false;
            ViewModified = false;
            InstanceModified = false;
        }


        /// <summary>
        /// Gets a value indicating whether the selected profile changed.
        /// </summary>
        public bool ProfileChanged { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the Agent connection options were modified.
        /// </summary>
        public bool ConnectionModified { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the configuration database was modified.
        /// </summary>
        public bool BaseModified { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the views were modified.
        /// </summary>
        public bool ViewModified { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the instance was modified
        /// </summary>
        public bool InstanceModified { get; private set; }


        /// <summary>
        /// Saves the deployment configuration.
        /// </summary>
        private void SaveDeploymentConfig()
        {
            if (!project.DeploymentConfig.Save(out string errMsg))
                appData.ErrLog.HandleError(errMsg);
        }

        private void FrmDownloadConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlProfileSelector, ctrlProfileSelector.GetType().FullName);
            FormTranslator.Translate(ctrlTransferOptions, ctrlTransferOptions.GetType().FullName);

            ctrlTransferOptions.Init(null, false);
            ctrlProfileSelector.Init(appData, project.DeploymentConfig, instance);

            if (ctrlProfileSelector.SelectedProfile?.AgentConnectionOptions is ConnectionOptions connectionOptions)
                initialConnectionOptions = connectionOptions.DeepClone();
        }

        private void FrmDownloadConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionModified = !ConnectionOptions.Equals(
                initialConnectionOptions, ctrlProfileSelector.SelectedProfile?.AgentConnectionOptions);
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            // display selected download options
            DeploymentProfile deploymentProfile = ctrlProfileSelector.SelectedProfile;

            if (deploymentProfile == null)
            {
                ctrlTransferOptions.Disable();
                btnDownload.Enabled = false;
            }
            else
            {
                ctrlTransferOptions.OptionsToControls(deploymentProfile.UploadOptions);
                btnDownload.Enabled = true;
            }

            transferOptionsModified = false;
        }

        private void ctrlTransferOptions_OptionsChanged(object sender, EventArgs e)
        {
            transferOptionsModified = true;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // validate options and download
            if (ctrlProfileSelector.SelectedProfile is DeploymentProfile profile &&
                ctrlTransferOptions.ValidateControls())
            {
                // save changed transfer options
                if (transferOptionsModified)
                {
                    ctrlTransferOptions.ControlsToOptions(profile.UploadOptions);
                    SaveDeploymentConfig();
                }

                // download
                instance.DeploymentProfile = profile.Name;
                ProfileChanged = initialProfileName != profile.Name;
                BaseModified = profile.UploadOptions.IncludeBase;
                ViewModified = profile.UploadOptions.IncludeView;
                InstanceModified = profile.UploadOptions.IncludeInstance;
                FrmTransfer frmTransfer = new(appData, project, instance, profile);

                if (frmTransfer.DownloadConfig())
                    DialogResult = DialogResult.OK;
            }
        }
    }
}
