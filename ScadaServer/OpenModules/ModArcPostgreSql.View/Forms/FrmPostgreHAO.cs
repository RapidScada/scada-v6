// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcPostgreSql.Config;

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
        private FrmPostgreHAO()
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
            ctrlHistoricalArchiveOptions.ArchiveOptions = options;
            ctrlDatabaseOptions.DatabaseOptions = options;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            ctrlHistoricalArchiveOptions.ControlsToOptions();
            ctrlDatabaseOptions.ControlsToOptions();
            options.AddToOptionList(archiveConfig.CustomOptions);
        }


        private void FrmPostgreHAO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlHistoricalArchiveOptions, ctrlHistoricalArchiveOptions.GetType().FullName);
            FormTranslator.Translate(ctrlDatabaseOptions, ctrlDatabaseOptions.GetType().FullName);

            UiUtils.FillConnections(ctrlDatabaseOptions.ConnectionComboBox, appDirs.ConfigDir);
            OptionsToControls();
        }

        private void btnManageConn_Click(object sender, EventArgs e)
        {
            UiUtils.EditConnections(ctrlDatabaseOptions.ConnectionComboBox, appDirs.ConfigDir);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
