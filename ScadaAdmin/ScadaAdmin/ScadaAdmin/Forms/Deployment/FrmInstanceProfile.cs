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
 * Modified : 2022
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Extensions;
using Scada.Admin.Project;
using Scada.Agent.Client;
using Scada.Client;
using Scada.Dbms;
using Scada.Forms;
using System;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for selecting an instance deployment profile.
    /// <para>Представляет форму для выбора профиля развёртывания экземпляра.</para>
    /// </summary>
    public partial class FrmInstanceProfile : Form, IDeploymentForm
    {
        private readonly AppData appData;          // the common data of the application
        private readonly ScadaProject project;     // the project under development
        private readonly ProjectInstance instance; // the affected instance
        private DeploymentProfile initialProfile;  // the initial deployment profile


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
        public FrmInstanceProfile(AppData appData, ScadaProject project, ProjectInstance instance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            initialProfile = null;

            ProfileChanged = false;
            ConnectionModified = false;
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
        /// Tests an Agent connection.
        /// </summary>
        private static bool TestAgentConnection(ConnectionOptions connectionOptions, out string errMsg)
        {
            try
            {
                AgentClient agentClient = new(connectionOptions);
                bool testOK = agentClient.TestConnection(out errMsg);
                agentClient.TerminateSession();
                return testOK;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Tests a database connection.
        /// </summary>
        private bool TestDbConnection(string extensionCode, DbConnectionOptions connectionOptions, out string errMsg)
        {
            if (!appData.ExtensionHolder.GetExtension(extensionCode, out ExtensionLogic extensionLogic))
            {
                errMsg = string.Format(AppPhrases.ExtensionNotFound, extensionCode);
            }
            else if (!extensionLogic.CanDeploy)
            {
                errMsg = string.Format(AppPhrases.ExtensionCannotDeploy, extensionCode);
            }
            else
            {
                try
                {
                    return extensionLogic.TestDbConnection(connectionOptions, out errMsg);
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }
            }

            return false;
        }


        private void FrmInstanceProfile_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlProfileSelector, ctrlProfileSelector.GetType().FullName);

            ctrlProfileSelector.Init(appData, project.DeploymentConfig, instance);

            if (ctrlProfileSelector.SelectedProfile != null)
                initialProfile = ctrlProfileSelector.SelectedProfile.DeepClone();
        }

        private void FrmInstanceProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionModified = IDeploymentForm.ConnectionsDifferent(
                initialProfile, ctrlProfileSelector.SelectedProfile);
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            btnTest.Enabled = ctrlProfileSelector.SelectedProfile != null;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (ctrlProfileSelector.SelectedProfile is DeploymentProfile profile)
            {
                StringBuilder sbTestResult = new();
                bool agentOK = false;
                bool dbOK = false;

                if (profile.AgentEnabled || profile.DbEnabled)
                {
                    Cursor = Cursors.WaitCursor;

                    // test Agent connection
                    if (profile.AgentEnabled)
                    {
                        if (TestAgentConnection(profile.AgentConnectionOptions, out string errMsg))
                        {
                            sbTestResult.AppendLine(AppPhrases.AgentConnectionOK);
                            agentOK = true;
                        }
                        else
                        {
                            sbTestResult.AppendLine(AppPhrases.AgentConnectionError).AppendLine(errMsg);
                        }
                    }
                    else
                    {
                        agentOK = true;
                    }

                    // test database connection
                    if (profile.DbEnabled && agentOK)
                    {
                        if (TestDbConnection(profile.Extension, profile.DbConnectionOptions, out string errMsg))
                        {
                            sbTestResult.AppendLine(AppPhrases.DbConnectionOK);
                            dbOK = true;
                        }
                        else
                        {
                            sbTestResult.AppendLine(AppPhrases.DbConnectionError).AppendLine(errMsg);
                        }
                    }
                    else
                    {
                        dbOK = true;
                    }

                    Cursor = Cursors.Default;
                }
                else
                {
                    sbTestResult.Append(AppPhrases.NoProfileConnections);
                }

                if (agentOK && dbOK)
                    ScadaUiUtils.ShowInfo(sbTestResult.ToString());
                else
                    ScadaUiUtils.ShowError(sbTestResult.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // set instance profile
            instance.DeploymentProfile = ctrlProfileSelector.SelectedProfile?.Name ?? "";
            ProfileChanged = IDeploymentForm.ProfilesDifferent(initialProfile, ctrlProfileSelector.SelectedProfile);
            DialogResult = DialogResult.OK;
        }
    }
}
