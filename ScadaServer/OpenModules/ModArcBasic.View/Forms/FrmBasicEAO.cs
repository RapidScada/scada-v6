// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Config;
using System;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    /// <summary>
    /// Represents a form for editing event archive options.
    /// <para>Представляет форму для редактирования параметров архива событий.</para>
    /// </summary>
    public partial class FrmBasicEAO : Form
    {
        private readonly AppDirs appDirs;             // the application directories
        private readonly ArchiveConfig archiveConfig; // the archive configuration
        private readonly BasicEAO options;            // the archive options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmBasicEAO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBasicEAO(AppDirs appDirs, ArchiveConfig archiveConfig)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.archiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
            options = new BasicEAO(archiveConfig.CustomOptions);
        }
        

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            numRetention.SetValue(options.Retention);
            chkLogEnabled.Checked = options.LogEnabled;
            chkUseCopyDir.Checked = options.UseCopyDir;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.Retention = Convert.ToInt32(numRetention.Value);
            options.LogEnabled = chkLogEnabled.Checked;
            options.UseCopyDir = chkUseCopyDir.Checked;
            options.AddToOptionList(archiveConfig.CustomOptions);
        }


        private void FrmHAO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, toolTip);
            OptionsToControls();
        }

        private void btnShowDir_Click(object sender, EventArgs e)
        {
            new FrmArcDir(appDirs.ConfigDir).ShowDialog();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
