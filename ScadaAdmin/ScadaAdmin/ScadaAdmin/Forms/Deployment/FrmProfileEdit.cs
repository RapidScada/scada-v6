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
 * Module   : Administrator
 * Summary  : Represents a form for selecting an instance deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for editing a deployment profile.
    /// <para>Представляет форму для редактирования профиля развёртывания.</para>
    /// </summary>
    public partial class FrmProfileEdit : Form
    {
        private readonly AppData appData;                     // the common data of the application
        private readonly DeploymentProfile deploymentProfile; // the edited deployment profile


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
        public FrmProfileEdit(AppData appData, DeploymentProfile deploymentProfile)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.deploymentProfile = deploymentProfile ?? throw new ArgumentNullException(nameof(deploymentProfile));
        }


        /// <summary>
        /// Gets or sets the names of the existing profiles.
        /// </summary>
        public HashSet<string> ExistingProfileNames { get; set; }


        /// <summary>
        /// Fills the combo box by the extensions that support deployment.
        /// </summary>
        private void FillExtensionComboBox()
        {
            try
            {
                cbExtension.BeginUpdate();
                cbExtension.Items.Clear();

                foreach (string extensionCode in appData.AppConfig.ExtensionCodes)
                {
                    if (extensionCode.StartsWith("ExtDep"))
                        cbExtension.Items.Add(extensionCode);
                }
            }
            finally
            {
                cbExtension.EndUpdate();
            }
        }

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
            ctrlAgentConnection.Enabled = deploymentProfile.AgentEnabled;
            ctrlAgentConnection.ConnectionOptions = deploymentProfile.AgentConnectionOptions;
            ctrlAgentConnection.NameEnabled = false;
            ctrlAgentConnection.InstanceEnabled = false;

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
            deploymentProfile.Name = txtName.Text;
            deploymentProfile.Extension = cbExtension.Text;
            deploymentProfile.WebUrl = txtWebUrl.Text;
            deploymentProfile.AgentEnabled = chkAgentEnabled.Checked;
            deploymentProfile.DbEnabled = chkDbEnabled.Checked;
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            StringBuilder sbError = new();

            if (string.IsNullOrWhiteSpace(txtName.Text))
                sbError.AppendError(lblName, CommonPhrases.NonemptyRequired);

            if (string.IsNullOrWhiteSpace(cbExtension.Text))
                sbError.AppendError(lblExtension, CommonPhrases.NonemptyRequired);

            if (!string.IsNullOrWhiteSpace(txtWebUrl.Text) && !ScadaUtils.IsValidUrl(txtWebUrl.Text))
                sbError.AppendError(lblWebUrl, CommonPhrases.ValidUrlRequired);

            if (sbError.Length > 0)
            {
                ScadaUiUtils.ShowError(CommonPhrases.CorrectErrors + Environment.NewLine + sbError);
                return false;
            }

            if (ExistingProfileNames != null && ExistingProfileNames.Contains(txtName.Text.Trim()))
            {
                ScadaUiUtils.ShowError(AppPhrases.ProfileNameDuplicated);
                return false;
            }

            return true;
        }


        private void FrmProfileEdit_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlAgentConnection, ctrlAgentConnection.GetType().FullName);
            FormTranslator.Translate(ctrlDbConnection, ctrlDbConnection.GetType().FullName);
            FillExtensionComboBox();
            ConfigToControls();
        }

        private void chkAgentEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ctrlAgentConnection.Enabled = chkAgentEnabled.Checked;
        }

        private void chkDbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ctrlDbConnection.Enabled = chkDbEnabled.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                ControlsToConfig();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
