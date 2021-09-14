// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtServerConfig.Code;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules;
using System;
using System.IO;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    /// <summary>
    /// Represents a form for editing archives.
    /// <para>Представляет форму для редактирования архивов.</para>
    /// </summary>
    public partial class FrmArchives : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly ServerApp serverApp;        // the server application in a project
        private readonly ServerConfig serverConfig;  // the server configuration
        private bool changing;                       // controls are being changed programmatically
        private ArchiveConfig archiveClipboard;      // contains the copied archive


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmArchives()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmArchives(IAdminContext adminContext, ServerApp serverApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.serverApp = serverApp ?? throw new ArgumentNullException(nameof(serverApp));
            serverConfig = serverApp.Config;
            changing = false;
            archiveClipboard = null;
            SetColumnNames();
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Sets the column names as needed for translation.
        /// </summary>
        private void SetColumnNames()
        {
            colOrder.Name = nameof(colOrder);
            colActive.Name = nameof(colActive);
            colCode.Name = nameof(colCode);
            colName.Name = nameof(colName);
            colKind.Name = nameof(colKind);
            colModule.Name = nameof(colModule);
        }

        /// <summary>
        /// Fills the combo box that contains archive kinds.
        /// </summary>
        private void FillKindComboBox()
        {
            cbKind.BeginUpdate();
            cbKind.Items.Add(ServerPhrases.UnspecifiedArchiveKind);
            cbKind.Items.Add(ServerPhrases.CurrentArchiveKind);
            cbKind.Items.Add(ServerPhrases.HistoricalArchiveKind);
            cbKind.Items.Add(ServerPhrases.EventsArchiveKind);
            cbKind.EndUpdate();
        }

        /// <summary>
        /// Fills the combo box by the modules that support archives.
        /// </summary>
        private void FillModuleComboBox()
        {
            try
            {
                cbModule.BeginUpdate();
                cbModule.Items.Clear();
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);

                foreach (FileInfo fileInfo in
                    dirInfo.EnumerateFiles("ModArc*.View.dll", SearchOption.TopDirectoryOnly))
                {
                    cbModule.Items.Add(ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name));
                }
            }
            finally
            {
                cbModule.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            try
            {
                lvArchive.BeginUpdate();
                lvArchive.Items.Clear();
                int index = 0;

                foreach (ArchiveConfig archiveConfig in serverConfig.Archives)
                {
                    lvArchive.Items.Add(CreateArchiveItem(archiveConfig.DeepClone(), ref index));
                }

                if (lvArchive.Items.Count > 0)
                    lvArchive.Items[0].Selected = true;
            }
            finally
            {
                lvArchive.EndUpdate();
            }
        }
        
        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            serverConfig.Archives.Clear();

            foreach (ListViewItem item in lvArchive.Items)
            {
                serverConfig.Archives.Add((ArchiveConfig)item.Tag);
            }
        }

        /// <summary>
        /// Enables or disables the controls.
        /// </summary>
        private void SetControlsEnabled()
        {
            if (lvArchive.SelectedItems.Count > 0)
            {
                int index = lvArchive.SelectedIndices[0];
                btnMoveUpArchive.Enabled = index > 0;
                btnMoveDownArchive.Enabled = index < lvArchive.Items.Count - 1;
                btnDeleteArchive.Enabled = true;
                btnCutArchive.Enabled = true;
                btnCopyArchive.Enabled = true;
                gbArchive.Enabled = true;
            }
            else
            {
                btnMoveUpArchive.Enabled = false;
                btnMoveDownArchive.Enabled = false;
                btnDeleteArchive.Enabled = false;
                btnCutArchive.Enabled = false;
                btnCopyArchive.Enabled = false;
                gbArchive.Enabled = false;
            }
        }

        /// <summary>
        /// Gets the selected list view item and the corresponding archive configuration.
        /// </summary>
        private bool GetSelectedItem(out ListViewItem item, out ArchiveConfig archiveConfig)
        {
            if (lvArchive.SelectedItems.Count > 0)
            {
                item = lvArchive.SelectedItems[0];
                archiveConfig = (ArchiveConfig)item.Tag;
                return true;
            }
            else
            {
                item = null;
                archiveConfig = null;
                return false;
            }
        }

        /// <summary>
        /// Displays the specified archive properties.
        /// </summary>
        private void DisplayArchive(ArchiveConfig archiveConfig)
        {
            if (archiveConfig == null)
            {
                chkActive.Checked = false;
                txtCode.Text = "";
                txtName.Text = "";
                cbKind.SelectedIndex = -1;
                cbModule.Text = "";
                txtOptions.Text = "";
            }
            else
            {
                chkActive.Checked = archiveConfig.Active;
                txtCode.Text = archiveConfig.Code;
                txtName.Text = archiveConfig.Name;
                cbKind.SelectedIndex = (int)archiveConfig.Kind;
                cbModule.Text = archiveConfig.Module;
                txtOptions.Text = archiveConfig.CustomOptions.ToString();
            }
        }

        /// <summary>
        /// Adds an item to the list view according to the specified archive.
        /// </summary>
        private void AddArchiveItem(ArchiveConfig archiveConfig)
        {
            int index = 0;
            lvArchive.InsertItem(CreateArchiveItem(archiveConfig, ref index), true);
            ChildFormTag.Modified = true;
        }

        /// <summary>
        /// Creates a new list view item that represents the specified archive.
        /// </summary>
        private static ListViewItem CreateArchiveItem(ArchiveConfig archiveConfig, ref int index)
        {
            return new ListViewItem(new string[]
            {
                (++index).ToString(),
                archiveConfig.Active ? "V" : " ",
                archiveConfig.Code,
                archiveConfig.Name,
                TranslateArchiveKind(archiveConfig.Kind),
                archiveConfig.Module
            })
            {
                Tag = archiveConfig
            };
        }

        /// <summary>
        /// Translates the specified archive kind.
        /// </summary>
        private static string TranslateArchiveKind(ArchiveKind archiveKind)
        {
            return archiveKind switch
            {
                ArchiveKind.Current => ServerPhrases.CurrentArchiveKind,
                ArchiveKind.Historical => ServerPhrases.HistoricalArchiveKind,
                ArchiveKind.Events => ServerPhrases.EventsArchiveKind,
                _ => ServerPhrases.UnspecifiedArchiveKind
            };
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            ControlsToConfig();

            if (serverApp.SaveConfig(out string errMsg))
                ChildFormTag.Modified = false;
            else
                adminContext.ErrLog.HandleError(errMsg);
        }


        private void FrmArchives_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillKindComboBox();
            FillModuleComboBox();
            SetControlsEnabled();
            ConfigToControls();
            btnPasteArchive.Enabled = false;
        }

        private void btnAddArchive_Click(object sender, EventArgs e)
        {
            AddArchiveItem(new ArchiveConfig());
        }

        private void btnMoveUpArchive_Click(object sender, EventArgs e)
        {
            if (lvArchive.MoveUpSelectedItem(true))
                ChildFormTag.Modified = true;
        }

        private void btnMoveDownArchive_Click(object sender, EventArgs e)
        {
            if (lvArchive.MoveDownSelectedItem(true))
                ChildFormTag.Modified = true;
        }

        private void btnDeleteArchive_Click(object sender, EventArgs e)
        {
            if (lvArchive.RemoveSelectedItem(true))
                ChildFormTag.Modified = true;
        }

        private void btnCutArchive_Click(object sender, EventArgs e)
        {
            // cut the selected archive
            btnCopyArchive_Click(null, null);
            btnDeleteArchive_Click(null, null);
        }

        private void btnCopyArchive_Click(object sender, EventArgs e)
        {
            // copy the selected archive
            if (GetSelectedItem(out _, out ArchiveConfig archiveConfig))
            {
                btnPasteArchive.Enabled = true;
                archiveClipboard = archiveConfig.DeepClone();
            }

            lvArchive.Focus();
        }

        private void btnPasteArchive_Click(object sender, EventArgs e)
        {
            // paste the copied archive
            if (archiveClipboard == null)
                lvArchive.Focus();
            else
                AddArchiveItem(archiveClipboard.DeepClone());
        }

        private void lvArchive_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display the selected item properties
            changing = true;
            GetSelectedItem(out _, out ArchiveConfig archiveConfig);
            DisplayArchive(archiveConfig);
            SetControlsEnabled();
            changing = false;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out ArchiveConfig archiveConfig))
            {
                archiveConfig.Active = chkActive.Checked;
                item.SubItems[1].Text = chkActive.Checked ? "V" : " ";
                ChildFormTag.Modified = true;
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out ArchiveConfig archiveConfig))
            {
                archiveConfig.Code = txtCode.Text;
                item.SubItems[2].Text = txtCode.Text;
                ChildFormTag.Modified = true;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out ArchiveConfig archiveConfig))
            {
                archiveConfig.Name = txtName.Text;
                item.SubItems[3].Text = txtName.Text;
                ChildFormTag.Modified = true;
            }
        }

        private void cbKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out ArchiveConfig archiveConfig))
            {
                archiveConfig.Kind = (ArchiveKind)cbKind.SelectedIndex;
                item.SubItems[4].Text = TranslateArchiveKind(archiveConfig.Kind);
                ChildFormTag.Modified = true;
            }
        }

        private void cbModule_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out ArchiveConfig archiveConfig))
            {
                archiveConfig.Module = cbModule.Text;
                item.SubItems[5].Text = cbModule.Text;
                ChildFormTag.Modified = true;
            }
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            // show archive properties
            if (GetSelectedItem(out _, out ArchiveConfig archiveConfig))
            {
                if (string.IsNullOrEmpty(archiveConfig.Module))
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.ModuleNotSpecified);
                }
                else if (!ModuleFactory.GetModuleView(adminContext.AppDirs.LibDir, archiveConfig.Module,
                    out ModuleView moduleView, out string message))
                {
                    ScadaUiUtils.ShowError(message);
                }
                else if (!moduleView.CanCreateArchive(archiveConfig.Kind))
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.ArchiveNotSupported, 
                        TranslateArchiveKind(archiveConfig.Kind));
                }
                else if (moduleView.CreateArchiveView(archiveConfig) is not ArchiveView archiveView)
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.UnableCreateArchiveView);
                }
                else if (!archiveView.CanShowProperties)
                {
                    ScadaUiUtils.ShowInfo(ExtensionPhrases.NoArchiveView);
                }
                else
                {
                    archiveView.BaseDataSet = adminContext.CurrentProject.ConfigBase;
                    archiveView.AppDirs = adminContext.AppDirs.CreateDirsForView(serverApp.ConfigDir);
                    archiveView.AppConfig = serverConfig;
                    archiveView.ArchiveConfig = archiveConfig;
                    
                    if (archiveView.ShowProperties())
                    {
                        DisplayArchive(archiveConfig);
                        ChildFormTag.Modified = true;
                    }
                }
            }
        }
    }
}
