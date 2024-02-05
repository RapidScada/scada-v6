// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Modules.ModDbExport.Config;
using System.ComponentModel;

namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    /// <summary>
    /// Represents a control for editing current data export options.
    /// <para>Представляет элемент управления для редактирования параметров экспорта текущих данных.</para>
    /// </summary>
    public partial class CtrlCurDataExport : UserControl
    {
        private CurDataExportOptions curDataExportOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCurDataExport()
        {
            InitializeComponent();
            curDataExportOptions = null;
        }


        /// <summary>
        /// Gets or sets the current data transfer options for editing.
        /// </summary>
        internal CurDataExportOptions CurDataExportOptions
        {
            get
            {
                return curDataExportOptions;
            }
            set
            {
                curDataExportOptions = null;
                ShowOptions(value);
                curDataExportOptions = value;
            }
        }


        /// <summary>
        /// Shows the options.
        /// </summary>
        private void ShowOptions(CurDataExportOptions options)
        {
            if (options == null)
            {
                cbTrigger.SelectedIndex = 0;
                chkSkipUnchanged.Checked = false;
                chkIncludeCalculated.Checked = false;
                numTimePeriod.Value = numTimePeriod.Minimum;
                numAllDataPeriod.Value = numAllDataPeriod.Minimum;
                numTimePeriod.Enabled = false;
                numAllDataPeriod.Enabled = false;
            }
            else
            {
                cbTrigger.SelectedIndex = (int)options.Trigger;
                chkSkipUnchanged.Checked = options.SkipUnchanged;
                chkIncludeCalculated.Checked = options.IncludeCalculated;
                numTimePeriod.SetValue(options.TimerPeriod);
                numAllDataPeriod.SetValue(options.AllDataPeriod);
                numTimePeriod.Enabled = numAllDataPeriod.Enabled = cbTrigger.SelectedIndex > 0;
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(curDataExportOptions, changeArgument));
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void cbTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (curDataExportOptions != null)
            {
                curDataExportOptions.Trigger = (ExportTrigger)cbTrigger.SelectedIndex;
                OnObjectChanged(TreeUpdateTypes.None);
                numTimePeriod.Enabled = numAllDataPeriod.Enabled = 
                    cbTrigger.SelectedIndex > 0;
            }
        }

        private void numTimePeriod_ValueChanged(object sender, EventArgs e)
        {
            if (curDataExportOptions != null)
            {
                curDataExportOptions.TimerPeriod = Convert.ToInt32(numTimePeriod.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numAllDataPeriod_ValueChanged(object sender, EventArgs e)
        {
            if (curDataExportOptions != null)
            {
                curDataExportOptions.AllDataPeriod = Convert.ToInt32(numAllDataPeriod.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void chkSkipUnchanged_CheckedChanged(object sender, EventArgs e)
        {
            if (curDataExportOptions != null)
            {
                curDataExportOptions.SkipUnchanged = chkSkipUnchanged.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void chkIncludeCalculated_CheckedChanged(object sender, EventArgs e)
        {
            if (curDataExportOptions != null)
            {
                curDataExportOptions.IncludeCalculated = chkIncludeCalculated.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
