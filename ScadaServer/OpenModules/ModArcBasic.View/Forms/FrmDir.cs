// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    /// <summary>
    /// Represents a form for editing archive directories.
    /// <para>Представляет форму для редактирования директорий архива.</para>
    /// </summary>
    public partial class FrmDir : Form
    {
        private readonly string configFileName;     // the module configuration file name
        private readonly ModuleConfig moduleConfig; // the module configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDir()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDir(string configDir)
            : this()
        {
            configFileName = Path.Combine(configDir, ModuleConfig.DefaultFileName);
            moduleConfig = new ModuleConfig();
        }


        /// <summary>
        /// Loads the module configuration.
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(configFileName) && !moduleConfig.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Saves the module configuration.
        /// </summary>
        private bool SaveConfig()
        {
            if (moduleConfig.Save(configFileName, out string errMsg))
            {
                return true;
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            chkUseDefaultDir.Checked = moduleConfig.UseDefaultDir;
            txtArcDir.Text = moduleConfig.ArcDir;
            txtArcCopyDir.Text = moduleConfig.ArcCopyDir;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfing()
        {
            moduleConfig.UseDefaultDir = chkUseDefaultDir.Checked;
            moduleConfig.ArcDir = txtArcDir.Text;
            moduleConfig.ArcCopyDir = txtArcCopyDir.Text;
        }

        /// <summary>
        /// Sets the default directories.
        /// </summary>
        private void SetToDefault(bool windows)
        {
            if (windows)
            {
                txtArcDir.Text = @"C:\SCADA\Archive\";
                txtArcCopyDir.Text = @"C:\SCADA\ArchiveCopy\";
            }
            else
            {
                txtArcDir.Text = "/opt/scada/Archive/";
                txtArcCopyDir.Text = "/opt/scada/ArchiveCopy/";
            }
        }


        private void FrmDir_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            LoadConfig();
            ConfigToControls();
        }

        private void chkUseDefaultDir_CheckedChanged(object sender, EventArgs e)
        {
            txtArcDir.Enabled = btnBrowseArcDir.Enabled =
                txtArcCopyDir.Enabled = btnBrowseArcCopyDir.Enabled = 
                btnSetToDefaultWin.Enabled = btnSetToDefaultLinux.Enabled = !chkUseDefaultDir.Checked;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // choose a text box
            TextBox textBox = null;

            if (sender == btnBrowseArcDir)
                textBox = txtArcDir;
            else if (sender == btnBrowseArcCopyDir)
                textBox = txtArcCopyDir;

            // browse directory
            if (textBox != null)
            {
                folderBrowserDialog.SelectedPath = textBox.Text.Trim();

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    textBox.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);
            }
        }

        private void btnSetToDefault_Click(object sender, EventArgs e)
        {
            SetToDefault(sender == btnSetToDefaultWin);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToConfing();

            if (SaveConfig())
                DialogResult = DialogResult.OK;
        }
    }
}
