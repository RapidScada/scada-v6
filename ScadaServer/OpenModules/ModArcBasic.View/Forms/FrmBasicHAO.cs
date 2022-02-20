// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Archives;
using Scada.Server.Config;
using System;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    /// <summary>
    /// Represents a form for editing historical archive options.
    /// <para>Представляет форму для редактирования параметров архива исторических данных.</para>
    /// </summary>
    public partial class FrmBasicHAO : Form
    {
        private readonly AppDirs appDirs;             // the application directories
        private readonly ArchiveConfig archiveConfig; // the archive configuration
        private readonly BasicHAO options;            // the archive options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmBasicHAO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBasicHAO(AppDirs appDirs, ArchiveConfig archiveConfig)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.archiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
            options = new BasicHAO(archiveConfig.CustomOptions);
        }
        

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            numWritingPeriod.SetValue(options.WritingPeriod);
            cbWritingMode.SelectedIndex = options.WritingMode switch
            {
                WritingMode.AutoWithPeriod => 0,
                WritingMode.OnDemandWithPeriod => 1,
                _ => -1
            };
            cbWritingUnit.SelectedIndex = (int)options.WritingUnit;
            numPullToPeriod.SetValue(options.PullToPeriod);
            numRetention.SetValue(options.Retention);
            chkLogEnabled.Checked = options.LogEnabled;
            chkUseCopyDir.Checked = options.UseCopyDir;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.WritingPeriod = Convert.ToInt32(numWritingPeriod.Value);
            options.WritingMode = cbWritingMode.SelectedIndex switch
            {
                0 => WritingMode.AutoWithPeriod,
                1 => WritingMode.OnDemandWithPeriod,
                _ => options.WritingMode // no change
            };
            options.WritingUnit = (TimeUnit)cbWritingUnit.SelectedIndex;
            options.PullToPeriod = Convert.ToInt32(numPullToPeriod.Value);
            options.Retention = Convert.ToInt32(numRetention.Value);
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
