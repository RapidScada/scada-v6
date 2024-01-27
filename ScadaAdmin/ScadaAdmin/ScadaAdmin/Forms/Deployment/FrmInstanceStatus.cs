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
 * Summary  : Represents a form for displaying an instance status
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2023
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Agent.Client;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for displaying an instance status.
    /// <para>Представляет форму для отображающая статуса экземпляра.</para>
    /// </summary>
    public partial class FrmInstanceStatus : Form, IDeploymentForm
    {
        /// <summary>
        /// The services which status is displayed.
        /// </summary>
        private static readonly ServiceApp[] ServiceApps = new ServiceApp[] { ServiceApp.Server, ServiceApp.Comm };

        private readonly AppData appData;          // the common data of the application
        private readonly ScadaProject project;     // the project under development
        private readonly ProjectInstance instance; // the affected instance
        private DeploymentProfile initialProfile;  // the initial deployment profile
        private IAgentClient agentClient;          // the Agent client
        private volatile bool connected;           // indicates that status is polled


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmInstanceStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInstanceStatus(AppData appData, ScadaProject project, ProjectInstance instance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            initialProfile = null;
            agentClient = null;
            connected = false;

            ProfileChanged = false;
            ConnectionModified = false;
        }


        /// <summary>
        /// Gets a value indicating whether the form is available to a user.
        /// </summary>
        private bool FormAvailable => Visible && !IsDisposed;

        /// <summary>
        /// Gets a value indicating whether the selected profile changed.
        /// </summary>
        public bool ProfileChanged { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the Agent connection options were modified.
        /// </summary>
        public bool ConnectionModified { get; private set; }


        /// <summary>
        /// Connects to a remote server.
        /// </summary>
        private void Connect()
        {
            if (!connected)
            {
                connected = true;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                gbStatus.Enabled = true;
                timer_Tick(null, null);
            }
        }

        /// <summary>
        /// Disconnects from the remote server.
        /// </summary>
        private void Disconnect()
        {
            connected = false;
            timer.Stop();
            agentClient = null;
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            gbStatus.Enabled = false;
            txtServerStatus.Text = "";
            txtCommStatus.Text = "";
            txtUpdateTime.Text = "";
        }

        /// <summary>
        /// Enables or disables actions.
        /// </summary>
        private void SetActionEnabled()
        {
            if (ctrlProfileSelector.SelectedProfile != null &&
                ctrlProfileSelector.SelectedProfile.AgentEnabled)
            {
                gbAction.Enabled = true;
                txtServerStatus.Text = "";
                txtCommStatus.Text = "";
            }
            else
            {
                gbAction.Enabled = false;
                txtServerStatus.Text = AppPhrases.AgentDisabled;
                txtCommStatus.Text = AppPhrases.AgentDisabled;
            }
        }

        /// <summary>
        /// Gets the current service statuses.
        /// </summary>
        private async Task GetServiceStatusAsync(IAgentClient client)
        {
            if (client == null)
                return;

            await Task.Run(() =>
            {
                try
                {
                    lock (client)
                    {
                        ServiceStatus[] statuses = client.GetServiceStatus(ServiceApps);

                        if (connected)
                        {
                            txtServerStatus.Text = statuses[0].ToString(Locale.IsRussian);
                            txtCommStatus.Text = statuses[1].ToString(Locale.IsRussian);
                            txtUpdateTime.Text = DateTime.Now.ToLocalizedString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (connected)
                    {
                        txtServerStatus.Text = CommonPhrases.UndefinedSign;
                        txtCommStatus.Text = CommonPhrases.UndefinedSign;
                        txtUpdateTime.Text = ex.Message;
                    }
                }
            });
        }

        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        private async Task ControlServiceAsync(IAgentClient client, ServiceApp serviceApp, ServiceCommand command)
        {
            if (client == null)
                return;

            await Task.Run(() =>
            {
                try
                {
                    bool result;

                    lock (client)
                    {
                        result = client.ControlService(serviceApp, command, 0);
                    }

                    if (!result && FormAvailable)
                        ScadaUiUtils.ShowError(AppPhrases.UnableControlService);
                }
                catch (Exception ex)
                {
                    if (FormAvailable)
                        ScadaUiUtils.ShowError(ex.BuildErrorMessage(AppPhrases.ControlServiceError));
                }
            });
        }


        private void FrmInstanceStatus_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlProfileSelector, ctrlProfileSelector.GetType().FullName);

            ctrlProfileSelector.Init(appData, project.DeploymentConfig, instance);
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile != null)
            {
                initialProfile = profile.DeepClone();
                if (profile.AgentEnabled)
                    Connect();
            }
        }

        private void FrmInstanceStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            Disconnect();
            ConnectionModified = IDeploymentForm.ConnectionsDifferent(
                initialProfile, ctrlProfileSelector.SelectedProfile);
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            Disconnect();
            SetActionEnabled();
        }

        private void ctrlProfileSelector_ProfileEdited(object sender, EventArgs e)
        {
            Disconnect();
            SetActionEnabled();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile != null && profile.AgentEnabled)
            {
                instance.DeploymentProfile = profile.Name;
                ProfileChanged = IDeploymentForm.ProfilesDifferent(initialProfile, profile);
                Connect();
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private async void btnControlService_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonName = button.Name;
            ServiceCommand? serviceCommand = null;
            ServiceApp? serviceApp = null;

            // get command
            if (buttonName.StartsWith("btnStart"))
                serviceCommand = ServiceCommand.Start;
            else if (buttonName.StartsWith("btnStop"))
                serviceCommand = ServiceCommand.Stop;
            else if (buttonName.StartsWith("btnRestart"))
                serviceCommand = ServiceCommand.Restart;

            // get application
            if (buttonName.EndsWith("Server"))
                serviceApp = ServiceApp.Server;
            else if (buttonName.EndsWith("Comm"))
                serviceApp = ServiceApp.Comm;
            else if (buttonName.EndsWith("Web"))
                serviceApp = ServiceApp.Web;

            // send command to application
            if (serviceApp != null && serviceCommand != null)
            {
                button.DisplayWait();
                await ControlServiceAsync(agentClient, serviceApp.Value, serviceCommand.Value);
            }
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            // initialize client
            if (agentClient == null &&
                ctrlProfileSelector.SelectedProfile != null &&
                ctrlProfileSelector.SelectedProfile.AgentEnabled)
            {
                agentClient = new AgentClient(ctrlProfileSelector.SelectedProfile.AgentConnectionOptions);
            }

            // request statuses
            await GetServiceStatusAsync(agentClient);

            if (connected)
                timer.Start();
        }
    }
}
