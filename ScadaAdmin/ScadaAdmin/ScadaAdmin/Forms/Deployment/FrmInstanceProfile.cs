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

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Client;
using Scada.Forms;
using Scada.Log;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for selecting an instance deployment profile.
    /// <para>Представляет форму для выбора профиля развёртывания экземпляра.</para>
    /// </summary>
    public partial class FrmInstanceProfile : Form
    {
        private readonly AppData appData;                   // the common data of the application
        private readonly ScadaProject project;              // the project under development
        private readonly ProjectInstance projectInstance;   // the affected instance
        private ConnectionOptions initialConnectionOptions; // the copy of the initial Agent connection options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInstanceProfile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInstanceProfile(AppData appData, ScadaProject project, ProjectInstance projectInstance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.projectInstance = projectInstance ?? throw new ArgumentNullException(nameof(projectInstance));
            initialConnectionOptions = null;

            ProfileChanged = false;
            ConnectionModified = false;
        }


        /// <summary>
        /// Gets a value indicating whether the selected profile changed.
        /// </summary>
        public bool ProfileChanged { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the Agent connection options were modified.
        /// </summary>
        public bool ConnectionModified { get; protected set; }


        /// <summary>
        /// Tests the connection of the selected profile.
        /// </summary>
        private void TestConnection()
        {
            ScadaUiUtils.ShowError("Not implemented.");
            /*try
            {
                Cursor = Cursors.WaitCursor;
                DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

                if (profile != null)
                {
                    ConnectionSettings connSettings = profile.ConnectionSettings.Clone();
                    connSettings.ScadaInstance = instance.Name;
                    IAgentClient agentClient = new AgentWcfClient(connSettings);

                    bool testResult = agentClient.TestConnection(out string errMsg);
                    Cursor = Cursors.Default;

                    if (testResult)
                        ScadaUiUtils.ShowInfo(AppPhrases.ConnectionOK);
                    else
                        ScadaUiUtils.ShowError(errMsg);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                appData.ProcError(ex, AppPhrases.TestConnectionError);
            }*/
        }


        private void FrmInstanceProfile_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlProfileSelector, ctrlProfileSelector.GetType().FullName);

            ctrlProfileSelector.Init(appData, project.DeploymentConfig, projectInstance);

            if (ctrlProfileSelector.SelectedProfile?.AgentConnectionOptions is ConnectionOptions connectionOptions)
                initialConnectionOptions = connectionOptions.DeepClone();
        }

        private void FrmInstanceProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionModified = !ConnectionOptions.Equals(
                initialConnectionOptions, ctrlProfileSelector.SelectedProfile?.AgentConnectionOptions);
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            btnTest.Enabled = ctrlProfileSelector.SelectedProfile != null;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TestConnection();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // set instance profile
            string selectedProfileName = ctrlProfileSelector.SelectedProfile?.Name ?? "";
            ProfileChanged = projectInstance.DeploymentProfile != selectedProfileName;
            projectInstance.DeploymentProfile = selectedProfileName;
            DialogResult = DialogResult.OK;
        }
    }
}
