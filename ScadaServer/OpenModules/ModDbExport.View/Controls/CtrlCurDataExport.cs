// Copyright (c) Rapid Software LLC. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Forms;
using Scada.Server.Modules.ModDbExport.Config;

namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    /// <summary>
    /// Represents a control for editing current data export options.
    /// <para>Представляет элемент управления для редактирования параметров экспортирования текущих данных.</para>
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
        internal CurDataExportOptions CurDataTransferOptions
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
                numTimePeriod.Value = options.TimerPeriod;
                numAllDataPeriod.Value = options.AllDataPeriod;

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
    }
}
