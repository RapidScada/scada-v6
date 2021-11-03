// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Project;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents a form for importing a configuration database table.
    /// <para>Представляет форму импорта таблицы базы конфигурации.</para>
    /// </summary>
    public partial class FrmTableImport : Form
    {
        /// <summary>
        /// Represents an item of the table list.
        private class TableItem
        {
            public IBaseTable BaseTable { get; init; }
            public override string ToString() => BaseTable.Title;
        }

        private readonly ILog log;              // the application log
        private readonly ConfigBase configBase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTableImport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableImport(ILog log, ConfigBase configBase)
            : this()
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.configBase = configBase ?? throw new ArgumentNullException(nameof(configBase));
            SelectedItemType = null;
        }


        /// <summary>
        /// Gets or sets the item type of the selected table.
        /// </summary>
        public Type SelectedItemType { get; set; }


        /// <summary>
        /// Fills the combo box with the tables.
        /// </summary>
        private void FillTableList()
        {
            try
            {
                cbTable.BeginUpdate();
                int selectedIndex = 0;
                int index = 0;

                foreach (IBaseTable baseTable in configBase.AllTables)
                {
                    if (baseTable.ItemType == SelectedItemType)
                        selectedIndex = index;

                    cbTable.Items.Add(new TableItem { BaseTable = baseTable });
                    index++;
                }

                cbTable.SelectedIndex = selectedIndex;
            }
            finally
            {
                cbTable.EndUpdate();
            }
        }

        /// <summary>
        /// Calculates the end destination ID.
        /// </summary>
        private void CalcDestEndID()
        {
            numDestEndID.SetValue(chkSrcEndID.Checked 
                ? chkDestStartID.Checked 
                    ? numSrcEndID.Value - numSrcStartID.Value + numDestStartID.Value 
                    : numSrcEndID.Value 
                : 0);
        }

        /// <summary>
        /// Imports the table.
        /// </summary>
        private bool Import(IBaseTable baseTable, BaseTableFormat format)
        {
            try
            {
                string srcfileName = openFileDialog.FileName;
                int srcStartID = chkSrcStartID.Checked ? Convert.ToInt32(numSrcStartID.Value) : 0;

                if (File.Exists(srcfileName))
                {
                    //new ImportExport().ImportBaseTable(srcfileName, format, baseTable,
                    //    srcStartID,
                    //    chkSrcEndID.Checked ? Convert.ToInt32(numSrcEndID.Value) : int.MaxValue,
                    //    chkDestStartID.Checked ? Convert.ToInt32(numDestStartID.Value) : srcStartID,
                    //    out int affectedRows);
                    //ScadaUiUtils.ShowInfo(string.Format(ExtensionPhrases.ImportTableComplete, affectedRows));
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.FileNotFound);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.HandleError(ex, ExtensionPhrases.ImportTableError);
                return false;
            }
        }


        private void FrmTableImport_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            openFileDialog.SetFilter(ExtensionPhrases.ImportTableFilter);
            FillTableList();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtSrcFile.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(txtSrcFile.Text);
                openFileDialog.FileName = Path.GetFileName(txtSrcFile.Text);
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtSrcFile.Text = openFileDialog.FileName;
        }

        private void chkSrcStartID_CheckedChanged(object sender, EventArgs e)
        {
            numSrcStartID.Enabled = chkSrcStartID.Checked;
            CalcDestEndID();
        }

        private void chkSrcEndID_CheckedChanged(object sender, EventArgs e)
        {
            numSrcEndID.Enabled = chkSrcEndID.Checked;
            CalcDestEndID();
        }

        private void chkDestStartID_CheckedChanged(object sender, EventArgs e)
        {
            numDestStartID.Enabled = chkDestStartID.Checked;
            CalcDestEndID();
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            CalcDestEndID();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (cbTable.SelectedItem is TableItem tableItem)
            {
                SelectedItemType = tableItem.BaseTable.ItemType;
                BaseTableFormat format = 
                    string.Equals(Path.GetExtension(txtSrcFile.Text), ".xml", StringComparison.OrdinalIgnoreCase) ?
                        BaseTableFormat.XML : BaseTableFormat.DAT;

                if (Import(tableItem.BaseTable, format))
                    DialogResult = DialogResult.OK;
            }
        }
    }
}
