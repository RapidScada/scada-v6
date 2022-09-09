// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information

using Scada.Forms;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcInfluxDb.Config;

namespace Scada.Server.Modules.ModArcInfluxDb.View.Forms
{
    /// <summary>
    /// Represents a form for editing historical archive options.
    /// <para>Представляет форму для редактирования параметров архива исторических данных.</para>
    /// </summary>
    public partial class FrmInfluxHAO : Form
    {
        private readonly AppDirs appDirs;             // the application directories
        private readonly ArchiveConfig archiveConfig; // the archive configuration
        private readonly InfluxHAO options;          // the archive options

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInfluxHAO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInfluxHAO(AppDirs appDirs, ArchiveConfig archiveConfig)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.archiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
            options = new InfluxHAO(archiveConfig.CustomOptions);
        }

        // <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            // general options
            ctrlHistoricalArchiveOptions.ArchiveOptions = options;

            // database options
            cbConnection.Text = options.Connection;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            // general options
            ctrlHistoricalArchiveOptions.ControlsToOptions();

            // database options
            options.Connection = cbConnection.Text;
            options.AddToOptionList(archiveConfig.CustomOptions);
        }


        private void FrmInfluxHAO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlHistoricalArchiveOptions, ctrlHistoricalArchiveOptions.GetType().FullName);

            UiUtils.FillConnections(cbConnection, appDirs.ConfigDir);
            OptionsToControls();
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
