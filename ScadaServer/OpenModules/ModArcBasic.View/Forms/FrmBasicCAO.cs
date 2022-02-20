// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Config;
using System;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    /// <summary>
    /// Represents a form for editing current archive options.
    /// <para>Представляет форму для редактирования параметров архива текущих данных.</para>
    /// </summary>
    public partial class FrmBasicCAO : Form
    {
        private readonly AppDirs appDirs;             // the application directories
        private readonly ArchiveConfig archiveConfig; // the archive configuration
        private readonly BasicCAO options;            // the archive options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmBasicCAO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBasicCAO(AppDirs appDirs, ArchiveConfig archiveConfig)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.archiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
            options = new BasicCAO(archiveConfig.CustomOptions);
        }
        

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            numWritingPeriod.SetValue(options.WritingPeriod);
            chkLogEnabled.Checked = options.LogEnabled;
            chkUseCopyDir.Checked = options.UseCopyDir;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.WritingPeriod = Convert.ToInt32(numWritingPeriod.Value);
            options.LogEnabled = chkLogEnabled.Checked;
            options.UseCopyDir = chkUseCopyDir.Checked;
            options.AddToOptionList(archiveConfig.CustomOptions);
        }


        private void FrmHAO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, new FormTranslatorOptions { ToolTip = toolTip });
            OptionsToControls();
        }

        private void btnShowDir_Click(object sender, EventArgs e)
        {
            new FrmArcDir(appDirs).ShowDialog();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
