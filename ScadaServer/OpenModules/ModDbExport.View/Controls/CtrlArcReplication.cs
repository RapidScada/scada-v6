// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Server.Modules.ModDbExport.Config;
using System.ComponentModel;

namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    /// <summary>
    /// Represents a control for editing archive replication.
    /// <para>Представляет элемент управления для редактирования параметров репликации архивов.</para>
    /// </summary>
    public partial class CtrlArcReplication : UserControl
    {
        private ArcReplicationOptions arcReplicationOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlArcReplication()
        {
            InitializeComponent();
            arcReplicationOptions = null;
        }


        /// <summary>
        /// Gets or sets the archive replication options for editing.
        /// </summary>
        internal ArcReplicationOptions ArcReplicationOptions
        {
            get
            {
                return arcReplicationOptions;
            }
            set
            {
                arcReplicationOptions = null;
                ShowOptions(value);
                arcReplicationOptions = value;
            }
        }

        /// <summary>
        /// Gets or sets the configuration database.
        /// </summary>
        public ConfigDataset ConfigDataset { get; set; }


        /// <summary>
        /// Shows the options.
        /// </summary>
        private void ShowOptions(ArcReplicationOptions options)
        {
            if (options == null)
            {
                chkEnabled.Checked = false;
                chkAutoExport.Checked = false;
                numHistArchiveBit.Value = numHistArchiveBit.Minimum;
                numEventArchiveBit.Value = numEventArchiveBit.Minimum;                
                numMinDepth.Value = numMinDepth.Minimum;
                numMaxDepth.Value = numMaxDepth.Minimum;
                numReadingStep.Value = numReadingStep.Minimum;
            }
            else
            {
                chkEnabled.Checked = options.Enabled;
                chkAutoExport.Checked = options.AutoExport;
                numHistArchiveBit.Value = options.HistArchiveBit;
                numEventArchiveBit.Value = options.EventArchiveBit;                
                numMinDepth.Value = options.MinDepth;
                numMaxDepth.Value = options.MaxDepth;
                numReadingStep.Value = options.ReadingStep;
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(arcReplicationOptions, changeArgument));
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (arcReplicationOptions != null)
            {
                arcReplicationOptions.Enabled = chkEnabled.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void chkAutoExport_CheckedChanged(object sender, EventArgs e)
        {
            if (arcReplicationOptions != null)
            {
                arcReplicationOptions.AutoExport = chkAutoExport.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numMinDepth_ValueChanged(object sender, EventArgs e)
        {
            if (arcReplicationOptions != null)
            {
                arcReplicationOptions.MinDepth = Convert.ToInt32(numMinDepth.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numMaxDepth_ValueChanged(object sender, EventArgs e)
        {
            if (arcReplicationOptions != null)
            {
                arcReplicationOptions.MaxDepth = Convert.ToInt32(numMaxDepth.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numReadingStep_ValueChanged(object sender, EventArgs e)
        {

            if (arcReplicationOptions != null)
            {
                arcReplicationOptions.ReadingStep = Convert.ToInt32(numReadingStep.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numHistArchiveBit_ValueChanged(object sender, EventArgs e)
        {
            if (arcReplicationOptions != null)
            {
                arcReplicationOptions.HistArchiveBit = Convert.ToInt32(numHistArchiveBit.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numEventArchiveBit_ValueChanged(object sender, EventArgs e)
        {
            if (arcReplicationOptions != null)
            {
                arcReplicationOptions.EventArchiveBit = Convert.ToInt32(numEventArchiveBit.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void btnSelectHistArchiveBit_Click(object sender, EventArgs e)
        {
            if (arcReplicationOptions != null && ConfigDataset != null)
            {
                FrmBitSelect frmBitSelect = new()
                {
                    SelectedBit = arcReplicationOptions.HistArchiveBit,
                    Bits = BitItemCollection.Create(ConfigDataset.ArchiveTable)
                };

                if (frmBitSelect.ShowDialog() == DialogResult.OK)
                    numHistArchiveBit.Value = frmBitSelect.SelectedBit;
            }
        }

        private void btnSelectEventArchiveBit_Click(object sender, EventArgs e)
        {
            if (arcReplicationOptions != null && ConfigDataset != null)
            {
                FrmBitSelect frmBitSelect = new()
                {
                    SelectedBit = arcReplicationOptions.EventArchiveBit,
                    Bits = BitItemCollection.Create(ConfigDataset.ArchiveTable)
                };

                if (frmBitSelect.ShowDialog() == DialogResult.OK)
                    numEventArchiveBit.Value = frmBitSelect.SelectedBit;
            }
        }
    }
}
