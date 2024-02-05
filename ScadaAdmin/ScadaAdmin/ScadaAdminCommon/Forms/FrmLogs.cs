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
 * Module   : ScadaAdminCommon
 * Summary  : Represents a form for displaying logs
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Admin.Extensions;
using Scada.Admin.Lang;
using Scada.Agent;
using Scada.Forms;
using Scada.Lang;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Forms
{
    /// <summary>
    /// Represents a form for displaying logs.
    /// <para>Представляет форму для отображения журналов.</para>
    /// </summary>
    public partial class FrmLogs : Form, IChildForm
    {
        /// <summary>
        /// Represents a file filter item.
        /// </summary>
        protected class FilterItem
        {
            /// <summary>
            /// Gets or sets the display name.
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Gets or sets the search pattern.
            /// </summary>
            public string SearchPattern { get; set; }
            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            public override string ToString() => Name;
        }

        /// <summary>
        /// The file extensions that should be displayed in full.
        /// </summary>
        private static readonly HashSet<string> ShowFullExtensions = new() { ".txt", ".cs" };

        private readonly IAdminContext adminContext; // the Administrator context
        private readonly RemoteLogBox logBox;        // updates log

        private IAgentClient agentClient; // interacts with Agent
        private bool fileNamesLoaded;     // indicates that file names are loaded
        private bool firstTick;           // indicates the first timer tick
        private bool isClosed;            // indicates that the form is closed


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLogs()
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
            firstTick = true;
            isClosed = false;

            ServiceApp = ServiceApp.Unknown;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLogs(IAdminContext adminContext, ServiceApp serviceApp)
            : this(adminContext)
        {
            ServiceApp = serviceApp;
        }


        /// <summary>
        /// Gets or sets a value indicating whether file names are loaded.
        /// </summary>
        private bool FileNamesLoaded
        {
            get
            {
                return fileNamesLoaded;
            }
            set
            {
                fileNamesLoaded = value;
                lblLoadFileList.Visible = !fileNamesLoaded && agentClient != null;
            }
        }

        /// <summary>
        /// Gets or sets the application which logs are displayed.
        /// </summary>
        protected ServiceApp ServiceApp { get; set; }

        /// <summary>
        /// Gets the file filter combo box.
        /// </summary>
        protected ComboBox FilterComboBox => cbFilter;

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
            FileNamesLoaded = false;

            if (setFirstLine)
                SetFirstLine();
        }

        /// <summary>
        /// Updates the path of the log box.
        /// </summary>
        private void UpdateLogPath()
        {
            string path = lbFiles.SelectedItem == null ? "" : lbFiles.SelectedItem.ToString();
            logBox.LogPath = new RelativePath(ServiceApp, AppFolder.Log, path);
            logBox.FullLogView = ShowFullExtensions.Contains(Path.GetExtension(path));
            logBox.AutoScroll = !logBox.FullLogView;
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
            FileNamesLoaded = false;
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
                        new RelativePath(ServiceApp, AppFolder.Log),
                        cbFilter.SelectedItem is FilterItem filterItem ? filterItem.SearchPattern : "*");
                }

                lbFiles.Items.AddRange(fileNames.ToArray());
                FileNamesLoaded = true;

                if (lbFiles.Items.Count > 0)
                    lbFiles.SelectedIndex = 0;
                else
                    logBox.SetFirstLine(CommonPhrases.NoData);
            }
            catch (Exception ex)
            {
                logBox.SetFirstLine(ex.Message);
            }
            finally
            {
                if (!lbFiles.IsDisposed)
                    lbFiles.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the file filter and selects the default item.
        /// </summary>
        protected virtual void FillFilter()
        {
            FilterComboBox.Items.Add(new FilterItem
            {
                Name = AdminPhrases.AllFilesFilter,
                SearchPattern = "*"
            });
            FilterComboBox.SelectedIndex = 0;
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
            FormTranslator.Translate(this, typeof(FrmLogs).FullName);
            
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
            
            FillFilter();
            InitLogBox();
            tmrRefresh.Start();
        }

        private void FrmLogs_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
            tmrRefresh.Stop();
        }

        private void FrmLogs_VisibleChanged(object sender, EventArgs e)
        {
            if (!firstTick)
            {
                tmrRefresh.Interval = Visible
                    ? ScadaUiUtils.LogRemoteRefreshInterval
                    : ScadaUiUtils.LogInactiveRefreshInterval;
            }
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AdminMessage.UpdateAgentClient)
                UpdateAgentClient(true);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkPause.Checked = false;
            ClearFileList();
        }

        private void lbFiles_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            SizeF textSize = e.Graphics.MeasureString("0", lbFiles.Font);
            e.ItemHeight = (int)(textSize.Height * 1.5);
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

                if (firstTick)
                {
                    firstTick = false;
                    tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
                }

                if (FileNamesLoaded)
                {
                    if (!chkPause.Checked && !string.IsNullOrEmpty(logBox.LogPath.Path))
                        await Task.Run(() => logBox.RefreshWithAgent());
                }
                else
                {
                    await Task.Run(() => LoadFileList());
                }

                if (!isClosed)
                    tmrRefresh.Start();
            }
        }
    }
}
