﻿/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Represents a form for setting a table filter
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Data.Tables;
using Scada.Forms;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a form for setting a table filter.
    /// <para>Представляет форму для установки фильтра по таблице.</para>
    /// </summary>
    public partial class FrmFilter : Form
    {
        /// <summary>
        /// Represents an extended filter.
        /// </summary>
        private class FilterExtended : TableFilter
        {
            private static readonly string[] MathOperations = { "=", "<>", "<", "<=", ">", ">=" };
            public int StringOperation { get; set; }
            public int MathOperation { get; set; }

            public string GetRowFilter()
            {
                if (Argument == null)
                {
                    return "";
                }
                else if (Argument is string)
                {
                    return string.Format(StringOperation == 0 ?
                        "{0} = '{1}'" :
                        "{0} like '%{1}%'", ColumnName, Argument);
                }
                else
                {
                    string valStr = Argument is double valDbl ?
                        valDbl.ToString(CultureInfo.InvariantCulture) :
                        Argument.ToString();

                    return string.Format("{0} {1} {2}", ColumnName, MathOperations[MathOperation], valStr);
                }
            }
        }

        private readonly DataGridView dataGridView;
        private readonly DataTable dataTable;
        private FilterExtended currentFilter;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmFilter()
        {
            InitializeComponent();

            cbMathOperation.Top = cbStringOperation.Top;
            cbValue.Top = txtValue.Top;
            cbBoolean.Top = txtValue.Top;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFilter(DataGridView dataGridView, DataTable dataTable)
            : this()
        {
            this.dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
            this.dataTable = dataTable ?? throw new ArgumentNullException(nameof(dataTable));
            currentFilter = null;
            FormTranslator.Translate(this, GetType().FullName);
        }


        /// <summary>
        /// Gets information of the selected column.
        /// </summary>
        private ColumnInfo SelectedColumnInfo
        {
            get
            {
                return cbColumn.SelectedItem as ColumnInfo;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the filter is not set.
        /// </summary>
        public bool FilterIsEmpty
        {
            get
            {
                return currentFilter == null;
            }
        }
        
        
        /// <summary>
        /// Fills the column list.
        /// </summary>
        private void FillColumnList(string selectedColumnName)
        {
            try
            {
                cbColumn.BeginUpdate();
                cbColumn.Items.Clear();
                int selectedIndex = 0;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column is DataGridViewTextBoxColumn || 
                        column is DataGridViewComboBoxColumn ||
                        column is DataGridViewCheckBoxColumn)
                    {
                        ColumnInfo columnInfo = new(column);
                        int index = cbColumn.Items.Add(columnInfo);

                        if (column.Name == selectedColumnName)
                            selectedIndex = index;
                   }
                }

                if (cbColumn.Items.Count > 0)
                    cbColumn.SelectedIndex = selectedIndex;
            }
            finally
            {
                cbColumn.EndUpdate();
            }
        }

        /// <summary>
        /// Adjusts the controls depending on the selected column.
        /// </summary>
        private void AdjustControls(ColumnInfo columnInfo)
        {
            cbStringOperation.Visible = false;
            cbStringOperation.SelectedIndex = 0;

            cbMathOperation.Visible = false;
            cbMathOperation.Enabled = true;
            cbMathOperation.SelectedIndex = 0;

            txtValue.Visible = false;
            txtValue.Text = "";

            cbValue.Visible = false;
            cbValue.DataSource = null;

            cbBoolean.Visible = false;
            cbBoolean.SelectedIndex = 0;

            if (columnInfo != null)
            {
                if (columnInfo.IsText)
                {
                    if (columnInfo.IsNumber)
                        cbMathOperation.Visible = true;
                    else
                        cbStringOperation.Visible = true;

                    txtValue.Visible = true;
                }
                else if (columnInfo.IsComboBox)
                {
                    cbMathOperation.Visible = true;
                    cbMathOperation.Enabled = false;

                    cbValue.Visible = true;
                    cbValue.DataSource = columnInfo.DataSource1;
                    cbValue.DisplayMember = columnInfo.DisplayMember;
                    cbValue.ValueMember = columnInfo.ValueMember;
                }
                else if (columnInfo.IsCheckBox)
                {
                    cbMathOperation.Visible = true;
                    cbMathOperation.Enabled = false;
                    cbBoolean.Visible = true;
                }
            }
        }

        /// <summary>
        /// Sets the default filter.
        /// </summary>
        private void SetDefaultFilter(ColumnInfo columnInfo)
        {
            DataGridViewCell curCell = dataGridView.CurrentCell;

            if (columnInfo != null && curCell != null)
            {
                if (columnInfo.IsText)
                {
                    txtValue.Text = curCell.Value?.ToString() ?? "";
                }
                else if (columnInfo.IsComboBox)
                {
                    cbValue.SelectedValue = curCell.Value;
                }
                else if (columnInfo.IsCheckBox)
                {
                    cbBoolean.SelectedIndex = (bool)curCell.Value ? 1 : 0;
                }
            }
        }

        /// <summary>
        /// Sets the controls according to the current filter.
        /// </summary>
        private void DisplayCurrentFilter(ColumnInfo columnInfo)
        {
            if (columnInfo != null)
            {
                if (columnInfo.IsText)
                {
                    cbStringOperation.SelectedIndex = currentFilter.StringOperation;
                    cbMathOperation.SelectedIndex = currentFilter.MathOperation;
                    txtValue.Text = currentFilter.Argument?.ToString() ?? "";
                }
                else if (columnInfo.IsComboBox)
                {
                    cbStringOperation.SelectedIndex = 0;
                    try { cbValue.SelectedValue = (int)currentFilter.Argument; }
                    catch { }
                }
                else if (columnInfo.IsCheckBox)
                {
                    cbStringOperation.SelectedIndex = 0;
                    cbBoolean.SelectedIndex = currentFilter.Argument is bool val && val ? 1 : 0;
                }
            }
        }

        /// <summary>
        /// Creates a new filter according to the controls.
        /// </summary>
        private FilterExtended CreateFilter(ColumnInfo columnInfo)
        {
            FilterExtended filter = new()
            {
                ColumnName = columnInfo.Column.Name,
                StringOperation = cbStringOperation.SelectedIndex,
                MathOperation = cbMathOperation.SelectedIndex
            };

            if (columnInfo.IsText)
                filter.Argument = columnInfo.IsNumber ? ScadaUtils.ParseDouble(txtValue.Text) : txtValue.Text;
            else if (columnInfo.IsComboBox)
                filter.Argument = (cbValue.SelectedValue is int val) ? val : -1;
            else if (columnInfo.IsCheckBox)
                filter.Argument = cbBoolean.SelectedIndex > 0;

            return filter;
        }


        private void FrmFilter_Load(object sender, EventArgs e)
        {
            ActiveControl = cbColumn;

            if (currentFilter == null)
            {
                FillColumnList(dataGridView.CurrentCell?.OwningColumn.Name ?? "");
                SetDefaultFilter(SelectedColumnInfo);
            }
            else
            {
                FillColumnList(currentFilter.ColumnName);
                DisplayCurrentFilter(SelectedColumnInfo);
            }
        }

        private void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustControls(SelectedColumnInfo);
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            currentFilter = null;
            dataTable.DefaultView.RowFilter = "";
            DialogResult = DialogResult.OK;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                currentFilter = CreateFilter(SelectedColumnInfo);
                dataTable.DefaultView.RowFilter = currentFilter == null ? "" : currentFilter.GetRowFilter();
                DialogResult = DialogResult.OK;
            }
            catch
            {
                ScadaUiUtils.ShowError(AppPhrases.IncorrectTableFilter);
            }
        }
    }
}
