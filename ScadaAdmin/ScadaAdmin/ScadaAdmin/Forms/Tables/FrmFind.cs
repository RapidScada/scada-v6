/*
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
 * Summary  : Represents a form for finding and replacing in a table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2021
 */

using System;
using System.Text;
using System.Windows.Forms;
using Scada.Admin.App.Code;
using Scada.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a form for finding and replacing in a table.
    /// <para>Представляет форму для поиска и замены в таблице.</para>
    /// </summary>
    public partial class FrmFind : Form
    {
        private readonly FrmBaseTable frmBaseTable;
        private readonly DataGridView dataGridView;
        private int startRowIndex;   // the starting point of the search
        private bool foundSomething; // at least one result found


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmFind()
        {
            InitializeComponent();

            cbFind.Top = txtFind.Top;
            cbReplaceWith.Top = txtReplaceWith.Top;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFind(FrmBaseTable frmBaseTable, DataGridView dataGridView)
            : this()
        {
            this.frmBaseTable = frmBaseTable ?? throw new ArgumentNullException(nameof(frmBaseTable));
            this.dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
            FormTranslator.Translate(this, GetType().FullName);
        }


        /// <summary>
        /// Fills the column list.
        /// </summary>
        private void FillColumnList()
        {
            try
            {
                cbColumn.BeginUpdate();
                int curColInd = dataGridView.CurrentCell == null ? -1 : dataGridView.CurrentCell.ColumnIndex;
                int selColInd = 0;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if ((column is DataGridViewTextBoxColumn || column is DataGridViewComboBoxColumn) &&
                        !column.ReadOnly)
                    {
                        ColumnInfo colInfo = new(column);
                        int index = cbColumn.Items.Add(colInfo);

                        if (column.Index == curColInd)
                            selColInd = index;
                    }
                }

                if (cbColumn.Items.Count > 0)
                    cbColumn.SelectedIndex = selColInd;
            }
            finally
            {
                cbColumn.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the default search condition.
        /// </summary>
        private void SetDefaultSearch(ColumnInfo columnInfo)
        {
            DataGridViewCell curCell = dataGridView.CurrentCell;

            if (curCell != null && columnInfo != null)
            {
                if (columnInfo.IsText)
                {
                    if (curCell.EditedFormattedValue != null)
                        txtFind.Text = curCell.EditedFormattedValue.ToString();
                }
                else if (curCell.IsInEditMode)
                {
                    if (dataGridView.EditingControl is ComboBox comboBox)
                        cbFind.SelectedValue = comboBox.SelectedValue;
                }
                else
                {
                    cbFind.SelectedValue = curCell.Value;
                }
            }
        }

        /// <summary>
        /// Adjusts the controls depending on the selected column.
        /// </summary>
        private void AdjustControls(ColumnInfo columnInfo)
        {
            if (columnInfo == null)
            {
                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = false;
            }
            else if (columnInfo.IsText)
            {
                txtFind.Visible = true;
                cbFind.Visible = false;
                txtReplaceWith.Visible = true;
                cbReplaceWith.Visible = false;
                chkCaseSensitive.Enabled = true;
                chkWholeCellOnly.Enabled = true;
                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = 
                    txtFind.Text != "";
            }
            else
            {
                txtFind.Visible = false;
                cbFind.Visible = true;
                txtReplaceWith.Visible = false;
                cbReplaceWith.Visible = true;
                chkCaseSensitive.Enabled = false;
                chkWholeCellOnly.Enabled = false;

                cbFind.DataSource = columnInfo.DataSource1;
                cbReplaceWith.DataSource = columnInfo.DataSource2;
                cbFind.DisplayMember = cbReplaceWith.DisplayMember = columnInfo.DisplayMember;
                cbFind.ValueMember = cbReplaceWith.ValueMember = columnInfo.ValueMember;

                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = 
                    cbFind.Items.Count > 0;
            }
        }

        /// <summary>
        /// Resets the starting point of the search
        /// </summary>
        private void ResetSearch()
        {
            startRowIndex = -1;
            foundSomething = false;
        }

        /// <summary>
        /// Finds the next match.
        /// </summary>
        private void FindNext(ColumnInfo columnInfo, bool showMsg, out bool found, out bool endReached)
        {
            bool isText = columnInfo.IsText;
            string findStr = isText ? txtFind.Text : "";
            object findObj = isText ? null : cbFind.SelectedValue;
            bool ignoreCase = !chkCaseSensitive.Checked;
            bool wholeCellOnly = chkWholeCellOnly.Checked;

            DataGridViewCell curCell = dataGridView.CurrentCell;
            int colInd = columnInfo.Column.Index;
            int rowInd = curCell == null ? 0 : curCell.RowIndex;
            int rowCnt = dataGridView.RowCount;

            if (curCell != null && curCell.ColumnIndex >= colInd)
                rowInd = rowInd == rowCnt - 1 ? 0 : rowInd + 1;

            if (startRowIndex < 0)
                startRowIndex = rowInd;

            do
            {
                DataGridViewCell cell = dataGridView[colInd, rowInd];
                found = isText ?
                    IsMatched(cell, findStr, ignoreCase, wholeCellOnly) :
                    IsMatched(cell, findObj);

                if (found)
                {
                    foundSomething = true;
                    try { dataGridView.CurrentCell = cell; }
                    catch { /* unable to leave the current cell because its value incorrect */ }
                }

                if (++rowInd >= rowCnt)
                    rowInd = 0;

                endReached = rowInd == startRowIndex;

            } while (!found && !endReached);

            if (!found || endReached)
            {
                if (showMsg)
                    ScadaUiUtils.ShowInfo(foundSomething ? AppPhrases.SearchCompleted : AppPhrases.ValueNotFound);

                ResetSearch();
            }
        }

        /// <summary>
        /// Replaces the cell value.
        /// </summary>
        private void ReplaceCell(ColumnInfo columnInfo, bool checkMatch, out bool replaced, out bool noError)
        {
            replaced = false;

            if (columnInfo.IsText)
            {
                if (!checkMatch ||
                    IsMatched(dataGridView.CurrentCell, txtFind.Text, !chkCaseSensitive.Checked, chkWholeCellOnly.Checked))
                {
                    // replace value of the text cell
                    dataGridView.BeginEdit(false);
                    if (dataGridView.EditingControl is TextBox textBox)
                    {
                        replaced = true;
                        textBox.Text = chkWholeCellOnly.Checked ? txtReplaceWith.Text :
                            Replace(textBox.Text, txtFind.Text, txtReplaceWith.Text, !chkCaseSensitive.Checked);
                    }
                }
            }
            else
            {
                if (!checkMatch || IsMatched(dataGridView.CurrentCell, cbFind.SelectedValue))
                {
                    // replace value of the combo box cell
                    dataGridView.BeginEdit(false);
                    if (dataGridView.EditingControl is ComboBox comboBox)
                    {
                        replaced = true;
                        // no exception is thrown if the specified value doesn't exist
                        comboBox.SelectedValue = cbReplaceWith.SelectedValue;
                    }
                }
            }

            noError = !replaced || frmBaseTable.EndEdit();
        }

        /// <summary>
        /// Replaces all cell values match the condition.
        /// </summary>
        private void ReplaceAll(ColumnInfo columnInfo)
        {
            // replace the current cell if it's matched the search condition
            int replacedCnt = 0;
            ReplaceCell(columnInfo, true, out bool replaced, out bool noError);

            if (replaced && noError)
                replacedCnt++;

            // replace other cells
            startRowIndex = dataGridView.CurrentCell == null ? 0 : dataGridView.CurrentCell.RowIndex;
            bool found, endReached;

            do
            {
                FindNext(columnInfo, false, out found, out endReached);

                if (found)
                {
                    ReplaceCell(columnInfo, false, out replaced, out noError);

                    if (replaced && noError)
                        replacedCnt++;
                }

            } while (found && !endReached);

            if (replacedCnt > 0)
                ScadaUiUtils.ShowInfo(AppPhrases.ReplaceCount, replacedCnt);
            else if (noError)
                ScadaUiUtils.ShowInfo(AppPhrases.ValueNotFound);
        }

        /// <summary>
        /// Checks that the cell is matched the search condition.
        /// </summary>
        private static bool IsMatched(DataGridViewCell cell, string value, bool ignoreCase, bool wholeCellOnly)
        {
            if (cell == null)
            {
                return false;
            }
            else
            {
                StringComparison comparison = ignoreCase ?
                    StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
                string cellVal = cell.EditedFormattedValue?.ToString() ?? "";

                return wholeCellOnly && string.Compare(cellVal, value, ignoreCase) == 0 ||
                    !wholeCellOnly && cellVal.Contains(value, comparison);
            }
        }

        /// <summary>
        /// Checks that the cell is matched the search condition.
        /// </summary>
        private static bool IsMatched(DataGridViewCell cell, object value)
        {
            if (cell == null)
            {
                return false;
            }
            else
            {
                object cellVal = cell.IsInEditMode ?
                    (cell.DataGridView.EditingControl as ComboBox)?.SelectedValue :
                    cell.Value;

                return cellVal == value || (cellVal is int val1) && (value is int val2) && (val1 == val2);
            }
        }

        /// <summary>
        /// Replaces part of the string.
        /// </summary>
        private static string Replace(string s, string find, string replaceWith, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(find))
                return s;

            StringComparison comparison = ignoreCase ?
                StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            StringBuilder result = new();
            int len = s.Length;
            int findLen = find.Length;
            int startInd = 0;
            int ind = 0;

            while (ind >= 0 && startInd < len)
            {
                ind = s.IndexOf(find, startInd, comparison);
                if (ind >= 0)
                {
                    result.Append(s[startInd..ind]);
                    result.Append(replaceWith);
                    startInd = ind + findLen;
                }
                else
                {
                    result.Append(s[startInd..]);
                }
            }

            return result.ToString();
        }


        private void FrmReplace_Load(object sender, EventArgs e)
        {
            FillColumnList();
            SetDefaultSearch(cbColumn.SelectedItem as ColumnInfo);
            ResetSearch();

            if (cbColumn.Items.Count == 0 || txtFind.Visible && txtFind.Text == "")
                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = false;
        }

        private void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustControls(cbColumn.SelectedItem as ColumnInfo);
            ResetSearch();
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            ResetSearch();

            if (txtFind.Visible)
                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = txtFind.Text != "";
        }

        private void cbFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void cbReplaceWith_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            if (cbColumn.SelectedItem is ColumnInfo columnInfo)
                FindNext(columnInfo, true, out _, out _);
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (cbColumn.SelectedItem is ColumnInfo columnInfo)
            {
                ReplaceCell(columnInfo, true, out bool _, out bool noError);

                if (noError)
                    FindNext(columnInfo, true, out _, out _);
            }
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            if (cbColumn.SelectedItem is ColumnInfo columnInfo)
                ReplaceAll(columnInfo);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
