// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtServerConfig.Code;
using Scada.Forms;
using Scada.Server;
using Scada.Server.Config;
using System;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    /// <summary>
    /// Represents a form for editing general options.
    /// <para>Форма для редактирования общих параметров.</para>
    /// </summary>
    public partial class FrmGeneralOptions : Form, IChildForm
    {
        private readonly ServerConfig serverConfig; // the server confiration
        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmGeneralOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmGeneralOptions(ServerConfig serverConfig)
            : this()
        {
            this.serverConfig = serverConfig ?? throw new ArgumentNullException(nameof(serverConfig));
            changing = false;
        }
        
        
        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        private void SettingsToControls()
        {
            changing = true;

            // connection
            ListenerOptions listenerOptions = serverConfig.ListenerOptions;
            numTcpPort.SetValue(listenerOptions.Port);
            //chkUseAD.Checked = settings.UseAD;
            //txtLdapPath.Text = settings.LdapPath;

            // directories
            PathOptions pathOptions = serverConfig.PathOptions;
            txtBaseDir.Text = pathOptions.BaseDir;
            txtViewDir.Text = pathOptions.ViewDir;
            txtArcDir.Text = pathOptions.ArcDir;
            txtArcCopyDir.Text = pathOptions.ArcCopyDir;

            // logging
            //chkDetailedLog.Checked = settings.DetailedLog;

            changing = false;
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        private void ControlsToSettings()
        {
            // connection
            ListenerOptions listenerOptions = serverConfig.ListenerOptions;
            listenerOptions.Port = decimal.ToInt32(numTcpPort.Value);
            //settings.UseAD = chkUseAD.Checked;
            //settings.LdapPath = txtLdapPath.Text;

            // directories
            PathOptions pathOptions = serverConfig.PathOptions;
            pathOptions.BaseDir = txtBaseDir.Text;
            pathOptions.ViewDir = txtViewDir.Text;
            pathOptions.ArcDir = txtArcDir.Text;
            pathOptions.ArcCopyDir = txtArcCopyDir.Text;

            // logging
            //settings.DetailedLog = chkDetailedLog.Checked;
        }

        /// <summary>
        /// Sets the default directories.
        /// </summary>
        private void SetDirectoriesToDefault(bool forWindows)
        {
            string scadaDir = forWindows ? @"C:\SCADA\" : "/opt/scada/";
            string sepChar = forWindows ? "\\" : "/";

            txtBaseDir.Text = scadaDir + "BaseDAT" + sepChar;
            txtViewDir.Text = scadaDir + "Views" + sepChar;
            txtArcDir.Text = scadaDir + "ArchiveDAT" + sepChar;
            txtArcCopyDir.Text = scadaDir + "ArchiveDATCopy" + sepChar;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            ControlsToSettings();

            if (ChildFormTag.SendMessage(this, ServerMessage.SaveSettings))
                ChildFormTag.Modified = false;
        }


        private void FrmCommonParams_Load(object sender, System.EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            SettingsToControls();
        }

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                ChildFormTag.Modified = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // choose a text box
            TextBox textBox = null;
            string descr = "";

            if (sender == btnBrowseBaseDATDir)
            {
                textBox = txtBaseDir;
                //descr = CommonPhrases.ChooseBaseDATDir;
            }
            else if (sender == btnBrowseItfDir)
            {
                textBox = txtViewDir;
                //descr = ExtensionPhrases.ChooseItfDir;
            }
            else if (sender == btnBrowseArcDir)
            {
                textBox = txtArcDir;
                //descr = ExtensionPhrases.ChooseArcDir;
            }
            else if (sender == btnBrowseArcCopyDir)
            {
                textBox = txtArcCopyDir;
                //descr = ExtensionPhrases.ChooseArcCopyDir;
            }

            // browse directory
            if (textBox != null)
            {
                fbdDir.SelectedPath = textBox.Text.Trim();
                fbdDir.Description = descr;

                if (fbdDir.ShowDialog() == DialogResult.OK)
                    textBox.Text = ScadaUtils.NormalDir(fbdDir.SelectedPath);
            }
        }

        private void btnSetToDefault_Click(object sender, EventArgs e)
        {
            SetDirectoriesToDefault(sender == btnSetToDefaultWin);
        }
    }
}
