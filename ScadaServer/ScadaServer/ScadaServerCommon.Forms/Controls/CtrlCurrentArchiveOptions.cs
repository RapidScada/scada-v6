// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Archives;
using System.ComponentModel;

namespace Scada.Server.Forms.Controls
{
    /// <summary>
    /// Represents a control for editing current archive options.
    /// <para>Представляет элемент управления для редактирования параметров текущего архива.</para>
    /// </summary>
    public partial class CtrlCurrentArchiveOptions : UserControl
    {
        private CurrentArchiveOptions options;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCurrentArchiveOptions()
        {
            InitializeComponent();
            options = null;
        }


        /// <summary>
        /// Gets or sets the archive options being edited.
        /// </summary>
        [Browsable(false)]
        public CurrentArchiveOptions ArchiveOptions
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
                OptionsToControls();
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
                numFlushPeriod.Value = 1;
            }
            else
            {
                gbOptions.Enabled = true;
                chkReadOnly.Checked = options.ReadOnly;
                chkLogEnabled.Checked = options.LogEnabled;
                numFlushPeriod.SetValue(options.FlushPeriod);
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
                options.FlushPeriod = Convert.ToInt32(numFlushPeriod.Value);
            }
        }


        private void chkReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            lblFlushPeriod.Enabled = numFlushPeriod.Enabled = txtFlushPeriodUnit.Enabled =
                !chkReadOnly.Checked;
        }
    }
}
