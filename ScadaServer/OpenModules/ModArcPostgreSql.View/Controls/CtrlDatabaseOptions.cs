// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Modules.ModArcPostgreSql.Config;
using System.ComponentModel;

namespace Scada.Server.Modules.ModArcPostgreSql.View.Controls
{
    /// <summary>
    /// Represents a control for editing database options.
    /// <para>Представляет элемент управления для редактирования параметров базы данных.</para>
    /// </summary>
    public partial class CtrlDatabaseOptions : UserControl
    {
        private IDatabaseOptions options;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlDatabaseOptions()
        {
            InitializeComponent();
            options = null;
        }


        /// <summary>
        /// Gets or sets the database options being edited.
        /// </summary>
        [Browsable(false)]
        internal IDatabaseOptions DatabaseOptions
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
        /// Gets the combo box that contains a connection list.
        /// </summary>
        internal ComboBox ConnectionComboBox => cbConnection;


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            if (options == null)
            {
                chkUseDefaultConn.Checked = false;
                cbConnection.Text = "";
                cbPartitionSize.SelectedIndex = 0;
                numMaxQueueSize.Value = numMaxQueueSize.Minimum;
                numBatchSize.Value = numBatchSize.Minimum;
            }
            else
            {
                chkUseDefaultConn.Checked = options.UseDefaultConn;
                cbConnection.Text = options.Connection;
                cbPartitionSize.SelectedIndex = (int)options.PartitionSize;
                numMaxQueueSize.SetValue(options.MaxQueueSize);
                numBatchSize.SetValue(options.BatchSize);
            }
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        public void ControlsToOptions()
        {
            if (options != null)
            {
                options.UseDefaultConn = chkUseDefaultConn.Checked;
                options.Connection = cbConnection.Text;
                options.PartitionSize = (PartitionSize)cbPartitionSize.SelectedIndex;
                options.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);
                options.BatchSize = Convert.ToInt32(numBatchSize.Value);
            }
        }


        private void chkUseDefaultConn_CheckedChanged(object sender, EventArgs e)
        {
            cbConnection.Enabled = !chkUseDefaultConn.Checked;
        }
    }
}
