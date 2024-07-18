// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Archives;
using System.ComponentModel;

namespace Scada.Server.Forms.Controls
{
    /// <summary>
    /// Represents a control for editing event archive options.
    /// <para>Представляет элемент управления для редактирования параметров архива событий.</para>
    /// </summary>
    public partial class CtrlEventArchiveOptions : UserControl
    {
        private EventArchiveOptions options;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlEventArchiveOptions()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the archive options being edited.
        /// </summary>
        [Browsable(false)]
        public EventArchiveOptions ArchiveOptions
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
                Enabled = false;
                chkReadOnly.Checked = false;
                chkLogEnabled.Checked = false;
                numRetention.Value = 1;
            }
            else
            {
                Enabled = true;
                chkReadOnly.Checked = options.ReadOnly;
                chkLogEnabled.Checked = options.LogEnabled;
                numRetention.SetValue(options.Retention);
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
            }
        }


        private void chkReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            lblRetention.Enabled = numRetention.Enabled = txtRetentionUnit.Enabled =
                !chkReadOnly.Checked;
        }
    }
}
