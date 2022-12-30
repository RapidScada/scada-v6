// Copyright (c) Rapid Software LLC. All rights reserved.

using Scada.Data.Models;
using Scada.Dbms;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using Scada.Server.Config;
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
        private const int DefaultCnlNum = 1;

        private QueryOptions queryOptions;
        private bool cnlNumChanged;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlQuery()
        {
            InitializeComponent();
            queryOptions = null;
            
            ConfigDataset = null;
            cnlNumChanged = false;
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
                txtSql.Clear();
                txtSql.AppendText(options.Sql);
                gbFilter.Visible = options.DataKind != DataKind.EventAck;

                if (options.DataKind == DataKind.Current || options.DataKind == DataKind.Historical)
                    chkSingleQuery.Enabled = true;
                else
                    chkSingleQuery.Enabled = false;
            }
        }

        /// <summary>
        /// Calls the form for selecting list of channels.
        /// </summary>
        private void selectNumsList(List<int> numsList, TextBox textBox)
        {
            FrmCnlSelect frmCnlSelect = new(ConfigDataset)
            {
                SelectedCnlNums = numsList
            };

            if (frmCnlSelect.ShowDialog() == DialogResult.OK)
            {
                numsList.Clear();
                numsList.AddRange(frmCnlSelect.SelectedCnlNums);

                textBox.Text = numsList.ToRangeString();
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        /// <summary>
        /// Validations textbox.
        /// </summary>       
        private void txtValidation(List<int> numsList, TextBox textBox)
        {
            if (queryOptions != null && cnlNumChanged)
            {
                if (ScadaUtils.ParseRange(textBox.Text, true, true, out IList<int> newRange))
                {
                    // update channel list
                    numsList.Clear();

                    if (newRange.Count > 0)
                        numsList.AddRange(newRange);
                    else
                        numsList.Add(DefaultCnlNum);

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

                if (queryOptions.DataKind == DataKind.Current || queryOptions.DataKind == DataKind.Historical)
                    chkSingleQuery.Enabled = true;
                else
                    chkSingleQuery.Enabled = false;
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
                selectNumsList(queryOptions.Filter.CnlNums, txtCnlNum);
        }

        private void btnSelectObjNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null && ConfigDataset != null)
                selectNumsList(queryOptions.Filter.ObjNums, txtObjNum);
        }

        private void btnSelectDeviceNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null && ConfigDataset != null)
                selectNumsList(queryOptions.Filter.DeviceNums, txtDeviceNum);
        }

        private void btnEditCnlNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null &&
                new FrmRangeEdit { Range = queryOptions.Filter.CnlNums, AllowEmpty = false, DefaultValue = DefaultCnlNum }
                    .ShowDialog() == DialogResult.OK)
            {
                txtCnlNum.Text = queryOptions.Filter.CnlNums.ToRangeString();
                txtCnlNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void btnEditObjNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null &&
               new FrmRangeEdit { Range = queryOptions.Filter.ObjNums, AllowEmpty = false, DefaultValue = DefaultCnlNum }
                   .ShowDialog() == DialogResult.OK)
            {
                txtObjNum.Text = queryOptions.Filter.ObjNums.ToRangeString();
                txtObjNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void btnEditDeviceNum_Click(object sender, EventArgs e)
        {
            if (queryOptions != null &&
               new FrmRangeEdit { Range = queryOptions.Filter.DeviceNums, AllowEmpty = false, DefaultValue = DefaultCnlNum }
                   .ShowDialog() == DialogResult.OK)
            {
                txtDeviceNum.Text = queryOptions.Filter.DeviceNums.ToRangeString();
                txtDeviceNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                OnObjectChanged(TreeUpdateTypes.None);
            }
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
            txtValidation(queryOptions.Filter.CnlNums, txtCnlNum);
        }

        private void txtCnlNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtCnlNum_Validating(null, null);
        }

        private void txtObjNum_Enter(object sender, EventArgs e)
        {
            cnlNumChanged = false;
        }
        
        private void txtObjNum_TextChanged(object sender, EventArgs e)
        {
            cnlNumChanged = false;
        }     

        private void txtObjNum_Validating(object sender, CancelEventArgs e)
        {
            txtValidation(queryOptions.Filter.ObjNums, txtObjNum);
        }

        private void txtObjNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtObjNum_Validating(null, null);
        }

        private void txtDeviceNum_Enter(object sender, EventArgs e)
        {
            cnlNumChanged = false;
        }

        private void txtDeviceNum_TextChanged(object sender, EventArgs e)
        {
            cnlNumChanged = false;
        }

        private void txtDeviceNum_Validating(object sender, CancelEventArgs e)
        {
            txtValidation(queryOptions.Filter.DeviceNums, txtDeviceNum);
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

        private void btnEditParametrs_Click(object sender, EventArgs e)
        {
            if (queryOptions != null)
                _ = new FrmQueryParametrs { QueryOptions = queryOptions, DBMS = DbmsType, }
                .ShowDialog() == DialogResult.OK;
        }
    }
}
