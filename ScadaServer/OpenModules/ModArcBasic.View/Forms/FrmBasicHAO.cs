// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcBasic.Config;

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
            chkLogEnabled.Checked = options.LogEnabled;
            numRetention.SetValue(options.Retention);
            chkWriteWithPeriod.Checked = options.WriteWithPeriod;
            numWritingPeriod.SetValue(options.WritingPeriod);
            cbWritingPeriodUnit.SelectedIndex = (int)options.WritingPeriodUnit;
            numPullToPeriod.SetValue(options.PullToPeriod);

            chkUseCopyDir.Checked = options.UseCopyDir;
            numMaxQueueSize.SetValue(options.MaxQueueSize);
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.LogEnabled = chkLogEnabled.Checked;
            options.Retention = Convert.ToInt32(numRetention.Value);
            options.WriteWithPeriod = chkWriteWithPeriod.Checked;
            options.WritingPeriod = Convert.ToInt32(numWritingPeriod.Value);
            options.WritingPeriodUnit = (TimeUnit)cbWritingPeriodUnit.SelectedIndex;
            options.PullToPeriod = Convert.ToInt32(numPullToPeriod.Value);

            options.UseCopyDir = chkUseCopyDir.Checked;
            options.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);

            options.AddToOptionList(archiveConfig.CustomOptions);
        }


        private void FrmHAO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
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
