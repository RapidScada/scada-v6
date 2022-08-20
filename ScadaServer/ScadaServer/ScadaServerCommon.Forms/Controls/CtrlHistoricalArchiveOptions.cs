// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Archives;
using System.ComponentModel;

namespace Scada.Server.Forms.Controls
{
    /// <summary>
    /// Represents a control for editing historical archive options.
    /// <para>Представляет элемент управления для редактирования параметров исторического архива.</para>
    /// </summary>
    public partial class CtrlHistoricalArchiveOptions : UserControl
    {
        private HistoricalArchiveOptions options;
        private bool changing;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlHistoricalArchiveOptions()
        {
            InitializeComponent();

            options = null;
            changing = false;
        }


        /// <summary>
        /// Gets or sets the archive options being edited.
        /// </summary>
        [Browsable(false)]
        public HistoricalArchiveOptions ArchiveOptions
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
                OptionsToControls();
                SetEnabled();
            }
        }
        
        
        /// <summary>
        /// Sets the Enabled property of the controls.
        /// </summary>
        private void DoSetEnabled()
        {
            if (chkReadOnly.Checked)
            {
                lblRetention.Enabled = numRetention.Enabled = txtRetentionUnit.Enabled =
                    lblIsPeriodic.Enabled = chkIsPeriodic.Enabled =
                    lblWriteWithPeriod.Enabled = chkWriteWithPeriod.Enabled =
                    lblWritingPeriod.Enabled = numWritingPeriod.Enabled = cbPeriodUnit.Enabled =
                    lblPullToPeriod.Enabled = numPullToPeriod.Enabled = txtPullToPeriodUnit.Enabled =
                    lblWriteOnChange.Enabled = chkWriteOnChange.Enabled =
                    lblDeadband.Enabled = numDeadband.Enabled = cbDeadbandUnit.Enabled = false;
            }
            else
            {
                lblRetention.Enabled = numRetention.Enabled = txtRetentionUnit.Enabled = 
                    lblIsPeriodic.Enabled = chkIsPeriodic.Enabled =
                    lblWriteWithPeriod.Enabled = chkWriteWithPeriod.Enabled = true;

                lblWritingPeriod.Enabled = numWritingPeriod.Enabled = cbPeriodUnit.Enabled = 
                    chkIsPeriodic.Checked || chkWriteWithPeriod.Checked;

                lblPullToPeriod.Enabled = numPullToPeriod.Enabled = txtPullToPeriodUnit.Enabled =
                    chkIsPeriodic.Checked;

                lblWriteOnChange.Enabled = chkWriteOnChange.Enabled =
                    !chkIsPeriodic.Checked;

                lblDeadband.Enabled = numDeadband.Enabled = cbDeadbandUnit.Enabled =
                    !chkIsPeriodic.Checked && chkWriteOnChange.Checked;
            }
        }

        /// <summary>
        /// Sets the Enabled property of the controls if a change is not in progress.
        /// </summary>
        private void SetEnabled()
        {
            if (!changing)
            {
                changing = true;
                DoSetEnabled();
                changing = false;
            }
        }

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            if (options == null)
            {
                gbOptions.Enabled = false;
                chkReadOnly.Checked = false;
                chkLogEnabled.Checked = false;
                numRetention.Value = 1;
                chkIsPeriodic.Checked = false;
                chkWriteWithPeriod.Checked = false;
                numWritingPeriod.Value = 1;
                cbPeriodUnit.SelectedIndex = 0;
                numPullToPeriod.Value = 0;
                chkWriteOnChange.Checked = false;
                numDeadband.Value = 0;
                cbDeadbandUnit.SelectedIndex = 0;
            }
            else
            {
                gbOptions.Enabled = true;
                chkReadOnly.Checked = options.ReadOnly;
                chkLogEnabled.Checked = options.LogEnabled;
                numRetention.SetValue(options.Retention);
                chkIsPeriodic.Checked = options.IsPeriodic;
                chkWriteWithPeriod.Checked = options.WriteWithPeriod;
                numWritingPeriod.SetValue(options.WritingPeriod);
                cbPeriodUnit.SelectedIndex = (int)options.PeriodUnit;
                numPullToPeriod.SetValue(options.PullToPeriod);
                chkWriteOnChange.Checked = options.WriteOnChange;
                numDeadband.SetValue(Convert.ToDecimal(options.Deadband));
                cbDeadbandUnit.SelectedIndex = (int)options.DeadbandUnit;
            }
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        public void ControlsToOptions()
        {
            if (options != null)
            {
                options.ReadOnly = chkReadOnly.Checked;
                options.LogEnabled = chkLogEnabled.Checked;
                options.Retention = Convert.ToInt32(numRetention.Value);
                options.IsPeriodic = chkIsPeriodic.Checked;
                options.WriteWithPeriod = chkWriteWithPeriod.Checked;
                options.WritingPeriod = Convert.ToInt32(numWritingPeriod.Value);
                options.PeriodUnit = (TimeUnit)cbPeriodUnit.SelectedIndex;
                options.PullToPeriod = Convert.ToInt32(numPullToPeriod.Value);
                options.WriteOnChange = chkWriteOnChange.Checked;
                options.Deadband = Convert.ToDouble(numDeadband.Value);
                options.DeadbandUnit = (DeadbandUnit)cbDeadbandUnit.SelectedIndex;
            }
        }


        private void chkReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            SetEnabled();
        }

        private void chkIsPeriodic_CheckedChanged(object sender, EventArgs e)
        {
            SetEnabled();
        }

        private void chkWriteWithPeriod_CheckedChanged(object sender, EventArgs e)
        {
            SetEnabled();
        }

        private void chkWriteOnChange_CheckedChanged(object sender, EventArgs e)
        {
            SetEnabled();
        }
    }
}
