// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Lang;
using Scada.Agent;
using Scada.Forms;
using Scada.Lang;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for displaying logs.
    /// <para>Представляет форму для отображения журналов.</para>
    /// </summary>
    public partial class FrmLogs : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly RemoteLogBox logBox;        // updates log
        private IAgentClient agentClient;            // interacts with Agent
        private bool fileNamesLoaded;                // indicates that file names are loaded
        private bool isClosed;                       // indicates that the form is closed


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLogs()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLogs(IAdminContext adminContext)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            logBox = new RemoteLogBox(lbLog) { AutoScroll = true };
            agentClient = null;
            fileNamesLoaded = false;
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
            UpdateAgentClient(false);
            UpdateLogPath();
        }

        /// <summary>
        /// Updates the Agent client of the log box.
        /// </summary>
        private void UpdateAgentClient(bool setFirstLine)
        {
            agentClient = adminContext.MainForm.GetAgentClient(ChildFormTag?.TreeNode, false);
            logBox.AgentClient = agentClient;

            lbFiles.Items.Clear();
            fileNamesLoaded = false;

            if (setFirstLine)
                SetFirstLine();
        }

        /// <summary>
        /// Updates the path of the log box.
        /// </summary>
        private void UpdateLogPath()
        {
            logBox.LogPath = new RelativePath(TopFolder.Comm, AppFolder.Log, 
                lbFiles.SelectedItem == null ? "" : lbFiles.SelectedItem.ToString());
            SetFirstLine();
        }

        /// <summary>
        /// Sets the initial text of the log box.
        /// </summary>
        private void SetFirstLine()
        {
            if (logBox.AgentClient == null)
                logBox.SetFirstLine(AdminPhrases.AgentNotEnabled);
            else if (string.IsNullOrEmpty(logBox.LogPath.Path))
                logBox.SetFirstLine(CommonPhrases.NoData);
            else
                logBox.SetFirstLine(AdminPhrases.FileLoading);
        }

        /// <summary>
        /// Clears the list of file names.
        /// </summary>
        private void ClearFileList()
        {
            lbFiles.Items.Clear();
            UpdateLogPath();
        }

        /// <summary>
        /// Loads the list of file names.
        /// </summary>
        private void LoadFileList()
        {
            if (agentClient is not IAgentClient localAgentClient)
                return;

            try
            {
                lbFiles.BeginUpdate();
                lbFiles.Items.Clear();
                ICollection<string> fileNames;

                lock (localAgentClient.SyncRoot)
                {
                    fileNames = localAgentClient.GetFileList(
                        new RelativePath(TopFolder.Comm, AppFolder.Log), GetSearchPattern());
                }

                lbFiles.Items.AddRange(fileNames.ToArray());
                fileNamesLoaded = true;

                if (lbFiles.Items.Count > 0)
                    lbFiles.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logBox.SetFirstLine(ex.Message);
            }
            finally
            {
                lbFiles.EndUpdate();
            }
        }

        /// <summary>
        /// Gets the search pattern prefix according to the file filter.
        /// </summary>
        private string GetSearchPattern()
        {
            return cbFilter.SelectedIndex switch
            {
                0 => "ScadaComm.*",
                1 => "line*.*",
                2 => "device*.*",
                _ => "*"
            };
        }

        /// <summary>
        /// Saves the changes of the child form data.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void FrmLogs_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
            cbFilter.SelectedIndex = 0;

            InitLogBox();
            tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
            tmrRefresh.Start();
        }

        private void FrmLogs_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
            tmrRefresh.Stop();
        }

        private void FrmLogs_VisibleChanged(object sender, EventArgs e)
        {
            tmrRefresh.Interval = Visible
                ? ScadaUiUtils.LogRemoteRefreshInterval
                : ScadaUiUtils.LogInactiveRefreshInterval;
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AdminMessage.UpdateAgentClient)
                UpdateAgentClient(true);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkPause.Checked = false;
            fileNamesLoaded = false;
            ClearFileList();
        }

        private void lbFiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            lbFiles.DrawTabItem(e);
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkPause.Checked = false;
            UpdateLogPath();
        }

        private async void tmrRefresh_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Stop();

                if (fileNamesLoaded)
                {
                    if (!chkPause.Checked && !string.IsNullOrEmpty(logBox.LogPath.Path))
                        await Task.Run(() => logBox.RefreshWithAgent());
                }
                else
                {
                    lblLoadFileList.Visible = true;
                    await Task.Run(() => LoadFileList());

                    if (fileNamesLoaded)
                        lblLoadFileList.Visible = false;
                }

                if (!isClosed)
                    tmrRefresh.Start();
            }
        }
    }
}
