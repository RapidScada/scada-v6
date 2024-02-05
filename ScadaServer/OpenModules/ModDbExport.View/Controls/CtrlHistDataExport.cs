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
    /// Represents a control for editing historical data export options.
    /// <para>Представляет элемент управления для редактирования параметров экспорта исторических данных.</para>
    /// </summary>
    public partial class CtrlHistDataExport : UserControl
    {
        private HistDataExportOptions histDataExportOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlHistDataExport()
        {
            InitializeComponent();

            histDataExportOptions = null;
            ConfigDataset = null;
        }


        /// <summary>
        /// Gets or sets the current data transfer options for editing.
        /// </summary>
        internal HistDataExportOptions HistDataExportOptions
        {
            get
            {
                return histDataExportOptions;
            }
            set
            {
                histDataExportOptions = null;
                ShowOptions(value);
                histDataExportOptions = value;
            }
        }

        /// <summary>
        /// Gets or sets the configuration database.
        /// </summary>
        public ConfigDataset ConfigDataset { get; set; }


        /// <summary>
        /// Shows the options.
        /// </summary>
        private void ShowOptions(HistDataExportOptions options)
        {
            if (options == null)
            {
                chkIncludeCalculated.Checked = false;
                numExportCalculatedDelay.Value = numExportCalculatedDelay.Minimum;
                numHistArchiveBit.Value = numHistArchiveBit.Minimum;
            }
            else
            {
                chkIncludeCalculated.Checked = options.IncludeCalculated;
                numExportCalculatedDelay.SetValue(options.ExportCalculatedDelay);
                numHistArchiveBit.SetValue(options.HistArchiveBit);
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(histDataExportOptions, changeArgument));
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void chkIncludeCalculated_CheckedChanged(object sender, EventArgs e)
        {
            if (histDataExportOptions != null)
            {
                histDataExportOptions.IncludeCalculated = chkIncludeCalculated.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
                numExportCalculatedDelay.Enabled = numHistArchiveBit.Enabled = btnSelectHistArchiveBit.Enabled =
                    chkIncludeCalculated.Checked;
            }
        }

        private void numExportCalculatedDelay_ValueChanged(object sender, EventArgs e)
        {
            if (histDataExportOptions != null)
            {
                histDataExportOptions.ExportCalculatedDelay = Convert.ToInt32(numExportCalculatedDelay.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numHistArchiveBit_ValueChanged(object sender, EventArgs e)
        {
            if (histDataExportOptions != null)
            {
                histDataExportOptions.HistArchiveBit = Convert.ToInt32(numHistArchiveBit.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void btnSelectHistArchiveBit_Click(object sender, EventArgs e)
        {
            if (histDataExportOptions != null && ConfigDataset != null)
            {
                FrmBitSelect frmBitSelect = new()
                {
                    SelectedBit = histDataExportOptions.HistArchiveBit,
                    Bits = BitItemCollection.Create(ConfigDataset.ArchiveTable)
                };

                if (frmBitSelect.ShowDialog() == DialogResult.OK)
                    numHistArchiveBit.Value = frmBitSelect.SelectedBit;
            }
        }
    }
}
