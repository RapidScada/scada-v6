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
 * Summary  : Represents a form for editing output channel properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a form for editing output channel properties.
    /// <para>Представляет форму для редактирования свойств канала управления.</para>
    /// </summary>
    public partial class FrmOutCnl : Form
    {
        private readonly DataGridView dataGridView;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmOutCnl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmOutCnl(DataGridView dataGridView)
            : this()
        {
            this.dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
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
                txtOutCnlNum.SetText(cells["OutCnlNum"]);
                txtName.SetText(cells["Name"]);
                cbCmdType.SetValue(cells["CmdTypeID"]);
                cbObj.SetValue(cells["ObjNum"]);
                cbDevice.SetValue(cells["DeviceNum"]);
                txtCmdNum.SetText(cells["CmdNum"]);
                txtCmdCode.SetText(cells["CmdCode"]);
                chkFormulaEnabled.SetChecked(cells["FormulaEnabled"]);
                txtFormula.SetText(cells["Formula"]);
                cbFormat.SetValue(cells["FormatID"]);
                chkEventEnabled.SetChecked(cells["EventEnabled"]);
            }
        }

        /// <summary>
        /// Validates and applies the property changes.
        /// </summary>
        private bool ApplyChanges()
        {
            // validate changes
            StringBuilder sbError = new();

            if (!(int.TryParse(txtOutCnlNum.Text, out int outCnlNum) &&
                ConfigBase.MinID <= outCnlNum && outCnlNum <= ConfigBase.MaxID))
            {
                sbError.AppendError(lblOutCnlNum, CommonPhrases.IntegerInRangeRequired, ConfigBase.MinID, ConfigBase.MaxID);
            }

            if (cbCmdType.SelectedValue == null)
                sbError.AppendError(lblCmdType, CommonPhrases.NonemptyRequired);

            int cmdNum = -1;
            if (txtCmdNum.Text != "" && !int.TryParse(txtCmdNum.Text, out cmdNum))
                sbError.AppendError(lblCmdNum, CommonPhrases.IntegerRequired);

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
                cells["OutCnlNum"].Value = outCnlNum;
                cells["Name"].Value = txtName.Text;
                cells["CmdTypeID"].Value = cbCmdType.SelectedValue ?? DBNull.Value;
                cells["ObjNum"].Value = cbObj.SelectedValue ?? DBNull.Value;
                cells["DeviceNum"].Value = cbDevice.SelectedValue ?? DBNull.Value;
                cells["CmdNum"].Value = cmdNum > 0 ? cmdNum : DBNull.Value;
                cells["CmdCode"].Value = txtCmdCode.Text;
                cells["FormulaEnabled"].Value = chkFormulaEnabled.Checked;
                cells["Formula"].Value = txtFormula.Text;
                cells["FormatID"].Value = cbFormat.SelectedValue ?? DBNull.Value;
                cells["EventEnabled"].Value = chkEventEnabled.Checked;
                return true;
            }
        }


        private void FrmInCnlProps_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ShowItemProps();
        }

        private void cbObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtObjNum.Text = cbObj.SelectedValue is int intVal ? intVal.ToString() : "";
        }

        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDeviceNum.Text = cbDevice.SelectedValue is int intVal ? intVal.ToString() : "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ApplyChanges())
                DialogResult = DialogResult.OK;
        }
    }
}
