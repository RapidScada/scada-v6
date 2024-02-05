// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcPostgreSql.Config;

namespace Scada.Server.Modules.ModArcPostgreSql.View.Forms
{
    /// <summary>
    /// Represents a form for editing current archive options.
    /// <para>Представляет форму для редактирования параметров архива текущих данных.</para>
    /// </summary>
    public partial class FrmPostgreCAO : Form
    {
        private readonly AppDirs appDirs;             // the application directories
        private readonly ArchiveConfig archiveConfig; // the archive configuration
        private readonly PostgreCAO options;          // the archive options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPostgreCAO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPostgreCAO(AppDirs appDirs, ArchiveConfig archiveConfig)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.archiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
            options = new PostgreCAO(archiveConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            // general options
            ctrlCurrentArchiveOptions.ArchiveOptions = options;

            // database options
            chkUseDefaultConn.Checked = options.UseDefaultConn;
            cbConnection.Text = options.Connection;
            numMaxQueueSize.SetValue(options.MaxQueueSize);
            numBatchSize.SetValue(options.BatchSize);
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            // general options
            ctrlCurrentArchiveOptions.ControlsToOptions();

            // database options
            options.UseDefaultConn = chkUseDefaultConn.Checked;
            options.Connection = cbConnection.Text;
            options.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);
            options.BatchSize = Convert.ToInt32(numBatchSize.Value);

            options.AddToOptionList(archiveConfig.CustomOptions);
        }


        private void FrmPostgreHAO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlCurrentArchiveOptions, ctrlCurrentArchiveOptions.GetType().FullName);

            OptionsToControls();
            UiUtils.FillConnections(cbConnection, appDirs.ConfigDir);
        }

        private void chkUseDefaultConn_CheckedChanged(object sender, EventArgs e)
        {
            cbConnection.Enabled = !chkUseDefaultConn.Checked;
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
