// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Web.Plugins.PlgMain;

namespace Scada.Admin.Extensions.ExtTableEditor.Forms
{
    /// <summary>
    /// Represents a form for editing table view options.
    /// <para>Представляет форму для редактирования параметров табличного представления.</para>
    /// </summary>
    public partial class FrmTableOptions : Form
    {
        private readonly TableOptions tableOptions; // the table view options
        private ConfigDatabase configDatabase;      // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTableOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableOptions(TableOptions tableOptions, ConfigDatabase configDatabase)
            : this()
        {
            this.tableOptions = tableOptions ?? throw new ArgumentNullException(nameof(tableOptions));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            chkUseDefault.Checked = tableOptions.UseDefault;
            txtArchiveCode.Text = tableOptions.ArchiveCode;
            numPeriod.SetValue(tableOptions.Period);
            txtChartArgs.Text = tableOptions.ChartArgs;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            tableOptions.UseDefault = chkUseDefault.Checked;
            tableOptions.ArchiveCode = txtArchiveCode.Text;
            tableOptions.Period = Convert.ToInt32(numPeriod.Value);
            tableOptions.ChartArgs = txtChartArgs.Text;
        }


        private void FrmTableOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            OptionsToControls();
        }

        private void chkUseDefault_CheckedChanged(object sender, EventArgs e)
        {
            pnlOptions.Enabled = !chkUseDefault.Checked;
        }

        private void btnSelectArchiveCode_Click(object sender, EventArgs e)
        {
            // show a dialog to select archive
            int archiveID = (from a in configDatabase.ArchiveTable
                             where a.Code == txtArchiveCode.Text
                             select a.ArchiveID).FirstOrDefault();

            FrmEntitySelect frmEntitySelect = new(configDatabase.ArchiveTable) 
            { 
                MultiSelect = false,
                SelectedID = archiveID
            };

            if (frmEntitySelect.ShowDialog() == DialogResult.OK &&
                frmEntitySelect.SelectedItem is Archive archive)
            {
                txtArchiveCode.Text = archive.Code;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
