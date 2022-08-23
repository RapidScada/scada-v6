// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Comm;
using Scada.Comm.Config;
using Scada.Forms;
using Scada.Protocol;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for displaying device data.
    /// <para>Представляет форму для отображения данных устройства.</para>
    /// </summary>
    public partial class FrmDeviceData : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly CommApp commApp;            // the Communicator application in a project
        private readonly DeviceConfig deviceConfig;  // the device configuration
        private readonly RemoteLogBox dataBox;       // updates device data
        private FrmDeviceCommand frmDeviceCommand;   // the form for sending commands
        private bool isClosed;                       // indicates that the form is closed


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceData(IAdminContext adminContext, CommApp commApp, DeviceConfig deviceConfig) : 
            this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.commApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
            this.deviceConfig = deviceConfig ?? throw new ArgumentNullException(nameof(deviceConfig));
            dataBox = new RemoteLogBox(lbDeviceData) { FullLogView = true };
            frmDeviceCommand = null;
            isClosed = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Initializes the log box.
        /// </summary>
        private void InitLogBox()
        {
            UpdateAgentClient();
            UpdateLogPath();
        }

        /// <summary>
        /// Updates the Agent client of the log box.
        /// </summary>
        private void UpdateAgentClient()
        {
            IAgentClient agentClient = adminContext.MainForm.GetAgentClient(ChildFormTag?.TreeNode, false);
            dataBox.AgentClient = agentClient;

            if (agentClient == null)
            {
                dataBox.SetFirstLine(AdminPhrases.AgentNotEnabled);
                btnSendCommand.Enabled = false;
            }
            else
            {
                dataBox.SetFirstLine(AdminPhrases.FileLoading);
                btnSendCommand.Enabled = true;
            }
        }

        /// <summary>
        /// Updates the path of the log box.
        /// </summary>
        private void UpdateLogPath()
        {
            dataBox.LogPath = new RelativePath(TopFolder.Comm, AppFolder.Log,
                CommUtils.GetDeviceLogFileName(deviceConfig.DeviceNum, ".txt"));
        }

        /// <summary>
        /// Saves the changes of the child form data.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void FrmDeviceData_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            Text = string.Format(Text, deviceConfig.DeviceNum);

            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;

            InitLogBox();
            tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
            tmrRefresh.Start();
        }

        private void FrmDeviceData_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
            tmrRefresh.Stop();
        }

        private void FrmDeviceData_VisibleChanged(object sender, EventArgs e)
        {
            tmrRefresh.Interval = Visible
                ? ScadaUiUtils.LogRemoteRefreshInterval
                : ScadaUiUtils.LogInactiveRefreshInterval;
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AdminMessage.UpdateAgentClient)
                UpdateAgentClient();
        }

        private async void tmrRefresh_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Stop();
                await Task.Run(() => dataBox.RefreshWithAgent());

                if (!isClosed)
                    tmrRefresh.Start();
            }
        }

        private void btnDeviceProperties_Click(object sender, EventArgs e)
        {
            // show device properties
            ExtensionUtils.ShowDeviceProperties(adminContext, commApp, deviceConfig, ChildFormTag?.TreeNode);
        }

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            // show device command form
            if (dataBox.AgentClient is IAgentClient agentClient)
            {
                frmDeviceCommand ??= new FrmDeviceCommand(adminContext, deviceConfig);
                frmDeviceCommand.AgentClient = agentClient;
                frmDeviceCommand.ShowDialog();
            }
        }
    }
}
