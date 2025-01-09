// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Dbms;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using Scada.Server.Config;
using Scada.Server.Modules.ModDeviceAlarm.Config;
using Scada.Server.Modules.ModDeviceAlarm.View.Forms;
using System.ComponentModel;

namespace Scada.Server.Modules.ModDeviceAlarm.View.Controls
{
    /// <summary>
    /// Represents a control for editing query options.
    /// <para>Представляет элемент управления для редактирования параметров запросов.</para>
    /// </summary>
    public partial class CtrlQuery : UserControl
    {
        private const int DefaultCnlNum = 1;

        private TriggerOptions queryOptions;
        private bool cnlNumChanged;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlQuery()
        {
            InitializeComponent();

            queryOptions = null;
            cnlNumChanged = false;
            ConfigDataset = null;
        }


        /// <summary>
        /// Gets or sets the general options for editing.
        /// </summary>
        internal TriggerOptions QueryOptions
        {
            get
            {
                return queryOptions;
            }
            set
            {
                queryOptions = null;
                ShowOptions(value);
                queryOptions = value;
            }
        }

        /// <summary>
        /// Gets the tool tip to be accessed on the main form.
        /// </summary>
        internal ToolTip ToolTip
        {
            get
            {
                return toolTip;
            }
            set
            {
                toolTip = value;
            }
        }

        /// <summary>
        /// Gets or sets the configuration database.
        /// </summary>
        public ConfigDataset ConfigDataset { get; set; }


        /// <summary>
        /// Shows the options.
        /// </summary>
        private void ShowOptions(TriggerOptions options)
        {
            if (options == null)
            {
                chkActive.Checked = false;
                cbTriggerKind.SelectedIndex = 0;
                txtName.Text = "";
                txtCnlNum.Text = "";
                txtDeviceNum.Text = "";
                gbFilter.Visible = true;
                numStatusCnlNum.Enabled = true;
                numDataPeriod.Enabled = false;
                numStatusCnlNum.Value = 0;
                numStatusPeriod.Value = 60;
                numDataPeriod.Value = 60;
                numDataUnchangedNumber.Value = 5;
            }
            else
            {
                chkActive.Checked = options.Active;
                cbTriggerKind.SelectedIndex = (int)options.TriggerKind;
                txtName.Text = options.Name;
                txtCnlNum.Text = ScadaUtils.ToRangeString(options.Filter.CnlNums);
                txtDeviceNum.Text = ScadaUtils.ToRangeString(options.Filter.DeviceNums);

                numStatusCnlNum.Value = options.StatusCnlNum;
                numStatusPeriod.Value = options.StatusPeriod;
                numDataPeriod.Value = options.DataUnchangedPeriod;
                numDataUnchangedNumber.Value = options.DataUnchangedNumber;

                numStatusCnlNum.Enabled = numStatusPeriod.Enabled = options.TriggerKind == TriggerKind.Status;
                numDataPeriod.Enabled = numDataUnchangedNumber.Enabled = options.TriggerKind == TriggerKind.ValueUnchange;
            }
        }

        /// <summary>
        /// Opens a form for editing range.
        /// </summary>
        private void SelectRange(List<int> numsList, TextBox textBox)
        {
            FrmRangeEdit frmRangeEdit = new()
            {
                Range = numsList,
                AllowEmpty = false,
                DefaultValue = DefaultCnlNum
            };

            if (frmRangeEdit.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = numsList.ToRangeString();
                textBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        /// <summary>
        /// Opens a form for selecting numbers.
        /// </summary>
        private void SelectNums(List<int> numsList, TextBox textBox, IBaseTable baseTable)
        {
            FrmEntitySelect frmEntitySelect = new(baseTable)
            {
                SelectedIDs = numsList
            };

            if (frmEntitySelect.ShowDialog() == DialogResult.OK)
            {
                numsList.Clear();
                numsList.AddRange(frmEntitySelect.SelectedIDs);

                textBox.Text = numsList.ToRangeString();
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        /// <summary>
        /// Validates the textbox of the filter.
        /// </summary>       
        private void ValidateFilterTextBox(List<int> numList, TextBox textBox)
        {
            if (cnlNumChanged)
            {
                if (ScadaUtils.ParseRange(textBox.Text, true, true, out IList<int> newRange))
                {
                    // update channel list
                    numList.Clear();

                    if (newRange.Count > 0)
                        numList.AddRange(newRange);
                    else
                        numList.Add(DefaultCnlNum);

                    textBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    OnObjectChanged(TreeUpdateTypes.None);
                }
                else
                {
                    // show error
                    textBox.ForeColor = Color.Red;
                    ScadaUiUtils.ShowError(CommonPhrases.ValidRangeRequired);
                }
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(queryOptions, changeArgument));
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        public void SetFocus()
        {
            txtName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.Active = chkActive.Checked;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void cbDataKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.TriggerKind = (TriggerKind)cbTriggerKind.SelectedIndex;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);

                numStatusCnlNum.Enabled = numStatusPeriod.Enabled = queryOptions.TriggerKind == TriggerKind.Status;
                numDataPeriod.Enabled = numDataUnchangedNumber.Enabled = queryOptions.TriggerKind == TriggerKind.ValueUnchange;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.Name = txtName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void btnSelectCnlNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null && ConfigDataset != null)
            {
                FrmCnlSelect frmCnlSelect = new(ConfigDataset)
                {
                    SelectedCnlNums = queryOptions.Filter.CnlNums
                };

                if (frmCnlSelect.ShowDialog() == DialogResult.OK)
                {
                    queryOptions.Filter.CnlNums.Clear();
                    queryOptions.Filter.CnlNums.AddRange(frmCnlSelect.SelectedCnlNums);

                    txtCnlNum.Text = queryOptions.Filter.CnlNums.ToRangeString();
                    OnObjectChanged(TreeUpdateTypes.None);
                }
            }
        }

        private void btnSelectDeviceNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null && ConfigDataset != null)
                SelectNums(queryOptions.Filter.DeviceNums, txtDeviceNum, ConfigDataset.DeviceTable);
        }

        private void btnEditCnlNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null)
                SelectRange(queryOptions.Filter.CnlNums, txtCnlNum);
        }

        private void btnEditDeviceNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null)
                SelectRange(queryOptions.Filter.DeviceNums, txtDeviceNum);
        }

        private void txtCnlNum_Enter(object sender, EventArgs e)
        {
            cnlNumChanged = false;
        }

        private void txtCnlNum_TextChanged(object sender, EventArgs e)
        {
            cnlNumChanged = true;
        }

        private void txtCnlNum_Validating(object sender, CancelEventArgs e)
        {
            if (queryOptions != null)
                ValidateFilterTextBox(queryOptions.Filter.CnlNums, txtCnlNum);
        }

        private void txtCnlNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtCnlNum_Validating(null, null);
        }

        private void txtDeviceNum_Enter(object sender, EventArgs e)
        {
            cnlNumChanged = false;
        }

        private void txtDeviceNum_TextChanged(object sender, EventArgs e)
        {
            cnlNumChanged = true;
        }

        private void txtDeviceNum_Validating(object sender, CancelEventArgs e)
        {
            if (queryOptions != null)
                ValidateFilterTextBox(queryOptions.Filter.DeviceNums, txtDeviceNum);
        }

        private void txtDeviceNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtDeviceNum_Validating(null, null);
        }

        private void numStatusCnlNum_ValueChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.StatusCnlNum = Convert.ToInt32(numStatusCnlNum.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numStatusPeriod_ValueChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.StatusPeriod = Convert.ToInt32(numStatusPeriod.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numDataPeriod_ValueChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.DataUnchangedPeriod = Convert.ToInt32(numDataPeriod.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numDataUnchangedNumber_ValueChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.DataUnchangedNumber = Convert.ToInt32(numDataUnchangedNumber.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void btnSelectCnlStatus_Click(object sender, EventArgs e)
        {
            if (queryOptions != null && ConfigDataset != null)
            {
                FrmEntitySelect frmEntitySelect = new(ConfigDataset.CnlStatusTable)
                {
                    MultiSelect = false,
                    SelectedID = queryOptions.StatusCnlNum
                };

                if (frmEntitySelect.ShowDialog() == DialogResult.OK)
                {
                    numStatusCnlNum.Value = frmEntitySelect.SelectedID;
                    OnObjectChanged(TreeUpdateTypes.None);
                }
            }
        }
    }
}
