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
 * Summary  : Represents a form for selecting an instance deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2021
 */

using Scada.Admin.Deployment;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for editing a deployment profile.
    /// <para>Представляет форму для редактирования профиля развёртывания.</para>
    /// </summary>
    public partial class FrmProfileEdit : Form
    {
        private readonly DeploymentProfile deploymentProfile;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmProfileEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmProfileEdit(DeploymentProfile deploymentProfile)
            : this()
        {
            this.deploymentProfile = deploymentProfile ?? throw new ArgumentNullException(nameof(deploymentProfile));
        }


        /// <summary>
        /// Gets or sets the names of the existing profiles.
        /// </summary>
        public HashSet<string> ExistingProfileNames { get; set; }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            // general
            txtName.Text = deploymentProfile.Name;
            cbExtension.Text = deploymentProfile.Extension;
            txtWebUrl.Text = deploymentProfile.WebUrl;

            // Agent connection
            chkAgentEnabled.Checked = deploymentProfile.AgentEnabled;

            // DB connection
            chkDbEnabled.Checked = deploymentProfile.DbEnabled;
            ctrlDbConnection.Enabled = deploymentProfile.DbEnabled;
            ctrlDbConnection.ConnectionOptions = deploymentProfile.DbConnectionOptions;
            ctrlDbConnection.NameEnabled = false;
            ctrlDbConnection.DbmsEnabled = true;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {

        }


        private void FrmProfileEdit_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlDbConnection, ctrlDbConnection.GetType().FullName);
            ConfigToControls();
        }

        private void chkAgentEnabled_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkDbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ctrlDbConnection.Enabled = chkDbEnabled.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToConfig();
            DialogResult = DialogResult.OK;
        }
    }
}
