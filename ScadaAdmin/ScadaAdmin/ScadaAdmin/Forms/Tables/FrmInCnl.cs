/*
 * Copyright 2021 Rapid Software LLC
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
 * Summary  : Represents a form for editing input channel properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.App.Controls.Tables;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a form for editing input channel properties.
    /// <para>Представляет форму для редактирования свойств входного канала.</para>
    /// </summary>
    public partial class FrmInCnl : Form
    {
        private readonly DataGridView dataGridView;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmInCnl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInCnl(DataGridView dataGridView)
            : this()
        {
            this.dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
        }


        /// <summary>
        /// Sets the bit mask value according to the cell value.
        /// </summary>
        public static void SetValue(CtrlBitMask ctrlBitMask, DataGridViewCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));

            ctrlBitMask.MaskValue = cell.Value is int intVal ? intVal : 0;

            if (cell.OwningColumn.Tag is ColumnOptions columnOptions)
                ctrlBitMask.MaskBits = columnOptions.DataSource;

            ctrlBitMask.ShowMask();
        }

        /// <summary>
        /// Shows the properties of the current item.
        /// </summary>
        private void ShowItemProps()
        {
            if (dataGridView.CurrentRow == null)
            {
                btnOK.Enabled = false;
            }
            else
            {
                DataGridViewCellCollection cells = dataGridView.CurrentRow.Cells;
                chkActive.SetChecked(cells["Active"]);
                txtCnlNum.SetText(cells["CnlNum"]);
                txtName.SetText(cells["Name"]);
                cbDataType.SetValue(cells["DataTypeID"]);
                txtDataLen.SetText(cells["DataLen"]);
                cbCnlType.SetValue(cells["CnlTypeID"]);
                cbObj.SetValue(cells["ObjNum"]);
                cbDevice.SetValue(cells["DeviceNum"]);
                txtTagNum.SetText(cells["TagNum"]);
                txtTagCode.SetText(cells["TagCode"]);
                chkFormulaEnabled.SetChecked(cells["FormulaEnabled"]);
                txtFormula.SetText(cells["Formula"]);
                cbFormat.SetValue(cells["FormatID"]);
                cbQuantity.SetValue(cells["QuantityID"]);
                cbUnit.SetValue(cells["UnitID"]);
                cbLim.SetValue(cells["LimID"]);
                SetValue(bmArchive, cells["ArchiveMask"]);
                SetValue(bmEvent, cells["EventMask"]);
            }
        }
        
        /// <summary>
        /// Validates and applies the property changes.
        /// </summary>
        private bool ApplyChanges()
        {
            // validate changes
            StringBuilder sbError = new();

            if (!(int.TryParse(txtCnlNum.Text, out int cnlNum) &&
                ConfigBase.MinID <= cnlNum && cnlNum <= ConfigBase.MaxID))
            {
                sbError.AppendError(lblCnlNum, CommonPhrases.IntegerInRangeRequired, ConfigBase.MinID, ConfigBase.MaxID);
            }

            int dataLen = -1;
            if (txtDataLen.Text != "" && !int.TryParse(txtDataLen.Text, out dataLen))
                sbError.AppendError(lblDataLen, CommonPhrases.IntegerRequired);

            if (cbCnlType.SelectedValue == null)
                sbError.AppendError(lblCnlType, CommonPhrases.NonemptyRequired);

            int tagNum = -1;
            if (txtTagNum.Text != "" && !int.TryParse(txtTagNum.Text, out tagNum))
                sbError.AppendError(lblTagNum, CommonPhrases.IntegerRequired);

            if (sbError.Length > 0)
            {
                ScadaUiUtils.ShowError(AppPhrases.CorrectErrors + Environment.NewLine + sbError);
                return false;
            }
            else if (dataGridView.CurrentRow == null)
            {
                return false;
            }
            else
            {
                // apply changes
                DataGridViewCellCollection cells = dataGridView.CurrentRow.Cells;
                cells["Active"].Value = chkActive.Checked;
                cells["CnlNum"].Value = cnlNum;
                cells["Name"].Value = txtName.Text;
                cells["DataTypeID"].Value = cbDataType.SelectedValue ?? DBNull.Value;
                cells["DataLen"].Value = dataLen > 0 ? dataLen : DBNull.Value;
                cells["CnlTypeID"].Value = cbCnlType.SelectedValue ?? DBNull.Value;
                cells["ObjNum"].Value = cbObj.SelectedValue ?? DBNull.Value;
                cells["DeviceNum"].Value = cbDevice.SelectedValue ?? DBNull.Value;
                cells["TagNum"].Value = tagNum > 0 ? tagNum : DBNull.Value;
                cells["TagCode"].Value = txtTagCode.Text;
                cells["FormulaEnabled"].Value = chkFormulaEnabled.Checked;
                cells["Formula"].Value = txtFormula.Text;
                cells["FormatID"].Value = cbFormat.SelectedValue ?? DBNull.Value;
                cells["QuantityID"].Value = cbQuantity.SelectedValue ?? DBNull.Value;
                cells["UnitID"].Value = cbUnit.SelectedValue ?? DBNull.Value;
                cells["LimID"].Value = cbLim.SelectedValue ?? DBNull.Value;
                cells["ArchiveMask"].Value = bmArchive.MaskValue > 0 ? bmArchive.MaskValue : DBNull.Value;
                cells["EventMask"].Value = bmEvent.MaskValue > 0 ? bmEvent.MaskValue : DBNull.Value;
                return true;
            }
        }


        private void FrmInCnl_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(bmArchive, bmArchive.GetType().FullName);
            FormTranslator.Translate(bmEvent, bmEvent.GetType().FullName);

            ShowItemProps();
        }

        private void FrmInCnl_FormClosed(object sender, FormClosedEventArgs e)
        {
            // restore limit filter
            if (cbLim.DataSource is DataTable limTable)
                limTable.DefaultView.RowFilter = "";
        }

        private void cbObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtObjNum.Text = cbObj.SelectedValue is int intVal ? intVal.ToString() : "";
        }

        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDeviceNum.Text = cbDevice.SelectedValue is int intVal ? intVal.ToString() : "";
        }

        private void cbLim_SelectedIndexChanged(object sender, EventArgs e)
        {
            // show details of the selected limit
            if (cbLim.SelectedItem is DataRowView rowView && 
                rowView["LimID"] is int limID && limID > 0)
            {
                bool isBoundToCnl = (bool)rowView["IsBoundToCnl"];

                string LimToStr(string columnName)
                {
                    if (isBoundToCnl)
                    {
                        return rowView[columnName] is double doubleVal && doubleVal > 0
                            ? "#" + (int)doubleVal
                            : "";
                    }
                    else
                    {
                        return Convert.ToString(rowView[columnName]);
                    }
                }

                txtLoLo.Text = LimToStr("LoLo");
                txtLow.Text = LimToStr("Low");
                txtHigh.Text = LimToStr("High");
                txtHiHi.Text = LimToStr("HiHi");
                txtDeadband.Text = Convert.ToString(rowView["Deadband"]);
            }
            else
            {
                txtLoLo.Text = "";
                txtLow.Text = "";
                txtHigh.Text = "";
                txtHiHi.Text = "";
                txtDeadband.Text = "";
            }
        }

        private void chkShared_CheckedChanged(object sender, EventArgs e)
        {
            // filter limits
            if (cbLim.DataSource is DataTable limTable)
            {
                object selVal = cbLim.SelectedValue;
                cbLim.DataSource = null;

                limTable.DefaultView.RowFilter = chkShared.Checked
                    ? "IsShared = true OR LimID IS NULL" + (selVal is int intVal ? " OR LimID = " + intVal : "")
                    : "";

                cbLim.DisplayMember = "Name";
                cbLim.ValueMember = "LimID";
                cbLim.DataSource = limTable;
                cbLim.SelectedValue = selVal;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ApplyChanges())
                DialogResult = DialogResult.OK;
        }
    }
}
