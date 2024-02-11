// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Dbms;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using Scada.Server.Modules.ModDbExport.Config;
using Scada.Server.Modules.ModDbExport.View.Forms;
using System.ComponentModel;

namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    /// <summary>
    /// Represents a control for editing query options.
    /// <para>Представляет элемент управления для редактирования параметров запросов.</para>
    /// </summary>
    public partial class CtrlQuery : UserControl
    {
        private QueryOptions queryOptions;
        private bool filterChanged;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlQuery()
        {
            InitializeComponent();

            queryOptions = null;
            filterChanged = false;
            ConfigDataset = null;
        }


        /// <summary>
        /// Gets or sets the general options for editing.
        /// </summary>
        internal QueryOptions QueryOptions
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
        /// Gets or sets editable target DbType.
        /// </summary>
        internal KnownDBMS DbmsType { get; set; }

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
        private void ShowOptions(QueryOptions options)
        {
            if (options == null)
            {
                chkActive.Checked = false;
                chkSingleQuery.Checked = false;
                cbDataKind.SelectedIndex = 0;
                txtName.Text = "";
                txtCnlNum.Text = "";
                txtObjNum.Text = "";
                txtDeviceNum.Text = "";
                txtSql.Text = "";
                gbFilter.Visible = true;
                chkSingleQuery.Enabled = true;
            }
            else
            {
                chkActive.Checked = options.Active;
                chkSingleQuery.Checked = options.SingleQuery;
                cbDataKind.SelectedIndex = (int)options.DataKind;
                txtName.Text = options.Name;
                txtCnlNum.Text = ScadaUtils.ToRangeString(options.Filter.CnlNums);
                txtObjNum.Text = ScadaUtils.ToRangeString(options.Filter.ObjNums);
                txtDeviceNum.Text = ScadaUtils.ToRangeString(options.Filter.DeviceNums);
                txtSql.Text = options.Sql.Replace("\n", Environment.NewLine);
                gbFilter.Visible = options.DataKind != DataKind.EventAck;
                chkSingleQuery.Enabled = options.DataKind == DataKind.Current ||
                    options.DataKind == DataKind.Historical;
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
                AllowEmpty = true
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
                textBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        /// <summary>
        /// Validates the filter textbox.
        /// </summary>       
        private void ValidateFilterTextBox(List<int> numList, TextBox textBox)
        {
            if (filterChanged)
            {
                if (ScadaUtils.ParseRange(textBox.Text, true, true, out IList<int> newRange))
                {
                    // update list
                    numList.Clear();

                    if (newRange.Count > 0)
                        numList.AddRange(newRange);

                    textBox.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    if (textBox.Text == "")
                    {
                        chkSingleQuery.Checked = false;
                        chkSingleQuery.Enabled = false;
                    }
                    else
                    {
                        chkSingleQuery.Enabled = true;
                    }

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
                queryOptions.DataKind = (DataKind)cbDataKind.SelectedIndex;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);

                gbFilter.Visible = queryOptions.DataKind != DataKind.EventAck;
                chkSingleQuery.Enabled = queryOptions.DataKind == DataKind.Current || 
                    queryOptions.DataKind == DataKind.Historical;
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

        private void chkSingleQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.SingleQuery = chkSingleQuery.Checked;
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
                    txtCnlNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                    OnObjectChanged(TreeUpdateTypes.None);
                }
            }
        }

        private void btnSelectObjNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null && ConfigDataset != null)
                SelectNums(queryOptions.Filter.ObjNums, txtObjNum, ConfigDataset.ObjTable);
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

        private void btnEditObjNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null)
                SelectRange(queryOptions.Filter.ObjNums, txtObjNum);
        }

        private void btnEditDeviceNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null)
                SelectRange(queryOptions.Filter.DeviceNums, txtDeviceNum);
        }

        private void txtCnlNum_Enter(object sender, EventArgs e)
        {
            filterChanged = false;
        }

        private void txtCnlNum_TextChanged(object sender, EventArgs e)
        {
            filterChanged = true;
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

        private void txtObjNum_Enter(object sender, EventArgs e)
        {
            filterChanged = false;
        }
        
        private void txtObjNum_TextChanged(object sender, EventArgs e)
        {
            filterChanged = true;
        }     

        private void txtObjNum_Validating(object sender, CancelEventArgs e)
        {
            if (queryOptions != null)
                ValidateFilterTextBox(queryOptions.Filter.ObjNums, txtObjNum);
        }

        private void txtObjNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtObjNum_Validating(null, null);
        }

        private void txtDeviceNum_Enter(object sender, EventArgs e)
        {
            filterChanged = false;
        }

        private void txtDeviceNum_TextChanged(object sender, EventArgs e)
        {
            filterChanged = true;
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

        private void txtSql_TextChanged(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                queryOptions.Sql = txtSql.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void btnViewParameters_Click(object sender, EventArgs e)
        {
            if (queryOptions != null)
            {
                new FrmQueryParametrs { QueryOptions = queryOptions, DBMS = DbmsType }
                .ShowDialog();
            }
        }
    }
}
