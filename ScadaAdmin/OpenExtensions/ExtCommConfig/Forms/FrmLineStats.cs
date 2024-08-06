// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Lang;
using Scada.Agent;
using Scada.Comm;
using Scada.Comm.Config;
using Scada.Forms;
using Scada.Protocol;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControls;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for displaying communication line stats.
    /// <para>Представляет форму для отображения статистики линии связи.</para>
    /// </summary>
    public partial class FrmLineStats : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly LineConfig lineConfig;      // the communication line configuration
        private readonly RemoteLogBox stateBox;      // updates state
        private readonly RemoteLogBox logBox;        // updates log
        private bool stateTabActive;                 // indicates that the State tab is currently active
        private bool isClosed;                       // indicates that the form is closed


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLineStats()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLineStats(IAdminContext adminContext, LineConfig lineConfig)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.lineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));
            stateBox = new RemoteLogBox(lbState) { FullLogView = true };
            logBox = new RemoteLogBox(lbLog, true) { AutoScroll = true };
            stateTabActive = true;
            isClosed = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Initializes the log boxes.
        /// </summary>
        private void InitLogBoxes()
        {
            UpdateAgentClients();
            UpdateLogPaths();
        }

        /// <summary>
        /// Updates the Agent clients of the log boxes.
        /// </summary>
        private void UpdateAgentClients()
        {
            IAgentClient agentClient = adminContext.MainForm.GetAgentClient(ChildFormTag?.TreeNode, false);
            stateBox.AgentClient = agentClient;
            logBox.AgentClient = agentClient;

            if (agentClient == null)
            {
                stateBox.SetFirstLine(AdminPhrases.AgentNotEnabled);
                logBox.SetFirstLine(AdminPhrases.AgentNotEnabled);
            }
            else
            {
                stateBox.SetFirstLine(AdminPhrases.FileLoading);
                logBox.SetFirstLine(AdminPhrases.FileLoading);
            }
        }

        /// <summary>
        /// Updates the paths of the log boxes.
        /// </summary>
        private void UpdateLogPaths()
        {
            stateBox.LogPath = new RelativePath(TopFolder.Comm, AppFolder.Log,
                CommUtils.GetLineLogFileName(lineConfig.CommLineNum, ".txt"));
            logBox.LogPath = new RelativePath(TopFolder.Comm, AppFolder.Log,
                CommUtils.GetLineLogFileName(lineConfig.CommLineNum, ".log"));
        }


        private void FrmLineStats_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            Text = string.Format(ExtensionPhrases.LineStatsTitle, lineConfig.CommLineNum);

            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
            lbTabs.SelectedIndex = 0;

            InitLogBoxes();
            tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
            tmrRefresh.Start();
        }

        private void FrmLineStats_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
            tmrRefresh.Stop();
        }

        private void FrmLineStats_VisibleChanged(object sender, EventArgs e)
        {
            tmrRefresh.Interval = Visible
                ? ScadaUiUtils.LogRemoteRefreshInterval
                : ScadaUiUtils.LogInactiveRefreshInterval;
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AdminMessage.UpdateFileName)
            {
                Text = string.Format(ExtensionPhrases.LineStatsTitle, lineConfig.CommLineNum);
                InitLogBoxes();
            }
            else if (e.Message == AdminMessage.UpdateAgentClient)
            {
                UpdateAgentClients();
            }
        }

        private void lbTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            lbTabs.DrawTabItem(e);
        }

        private void lbTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTabs.SelectedIndex == 0)
            {
                stateTabActive = true;
                chkPause.Enabled = false;
                chkPause.Checked = false;
                lbState.Visible = true;
                lbLog.Visible = false;
            }
            else
            {
                stateTabActive = false;
                chkPause.Enabled = true;
                lbState.Visible = false;
                lbLog.Visible = true;
            }
        }

        private async void tmrRefresh_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Stop();

                if (stateTabActive)
                    await Task.Run(() => stateBox.RefreshWithAgent());
                else if (!chkPause.Checked)
                    await Task.Run(() => logBox.RefreshWithAgent());

                if (!isClosed)
                    tmrRefresh.Start();
            }
        }
    }
}
