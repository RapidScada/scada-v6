// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Archives;
using Scada.Server.Config;
using System;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcPostgreSql.View.Forms
{
    /// <summary>
    /// Represents a form for editing historical archive options.
    /// <para>Представляет форму для редактирования параметров архива исторических данных.</para>
    /// </summary>
    public partial class FrmPostgreHAO : Form
    {
        private readonly AppDirs appDirs;             // the application directories
        private readonly ArchiveConfig archiveConfig; // the archive configuration
        private readonly PostgreHAO options;          // the archive options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPostgreHAO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPostgreHAO(AppDirs appDirs, ArchiveConfig archiveConfig)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.archiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
            options = new PostgreHAO(archiveConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            // general options
            numWritingPeriod.SetValue(options.WritingPeriod);
            cbWritingMode.SelectedIndex = (int)options.WritingMode;
            cbWritingUnit.SelectedIndex = (int)options.WritingUnit;
            numPullToPeriod.SetValue(options.PullToPeriod);
            numRetention.SetValue(options.Retention);
            chkLogEnabled.Checked = options.LogEnabled;

            // database options
            chkUseStorageConn.Checked = options.UseStorageConn;
            cbConnection.Text = options.Connection;
            numMaxQueueSize.SetValue(options.MaxQueueSize);
            cbPartitionSize.SelectedIndex = (int)options.PartitionSize;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            // general options
            options.WritingPeriod = Convert.ToInt32(numWritingPeriod.Value);
            options.WritingMode = (WritingMode)cbWritingMode.SelectedIndex;
            options.WritingUnit = (TimeUnit)cbWritingUnit.SelectedIndex;
            options.PullToPeriod = Convert.ToInt32(numPullToPeriod.Value);
            options.Retention = Convert.ToInt32(numRetention.Value);
            options.LogEnabled = chkLogEnabled.Checked;

            // database options
            options.UseStorageConn = chkUseStorageConn.Checked;
            options.Connection = cbConnection.Text;
            options.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);
            options.PartitionSize = (PartitionSize)cbPartitionSize.SelectedIndex;

            options.AddToOptionList(archiveConfig.CustomOptions);
        }


        private void FrmPostgreHAO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            OptionsToControls();
            UiUtils.FillConnections(cbConnection, appDirs.ConfigDir);
        }

        private void cbWritingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            WritingMode writingMode = (WritingMode)cbWritingMode.SelectedIndex;
            lblWritingPeriod.Enabled = numWritingPeriod.Enabled =
                lblWritingUnit.Enabled = cbWritingUnit.Enabled =
                lblPullToPeriod.Enabled = numPullToPeriod.Enabled =
                writingMode == WritingMode.AutoWithPeriod || writingMode == WritingMode.OnDemandWithPeriod;
        }

        private void chkUseStorageConn_CheckedChanged(object sender, EventArgs e)
        {
            cbConnection.Enabled = !chkUseStorageConn.Checked;
        }

        private void btnManageConn_Click(object sender, EventArgs e)
        {
            UiUtils.EditConnections(cbConnection, appDirs.ConfigDir);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
