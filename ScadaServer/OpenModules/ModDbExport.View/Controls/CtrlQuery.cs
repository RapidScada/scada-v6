// Copyright (c) Rapid Software LLC. All rights reserved.

using Scada.Forms;
using Scada.Server.Config;
using Scada.Server.Modules.ModDbExport.Config;
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
        
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlQuery()
        {
            InitializeComponent();
            queryOptions = null;
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
                chkSingleQuery.Visible = true;
                btnEditParametrs.Visible = true;
            }
            else
            {
                chkActive.Checked = options.Active;
                chkSingleQuery.Checked = options.SingleQuery;
                cbDataKind.SelectedIndex = (int)options.DataKind;
                txtName.Text = options.Name;
                txtCnlNum.Text = ScadaUtils.ToRangeString(options.Filter.CnlNums);
                txtObjNum.Text = ScadaUtils.ToRangeString(options.Filter.ObjNums);
                txtDeviceNum.Text = ScadaUtils.ToRangeString (options.Filter.DeviceNums);
                txtSql.AppendText(options.Sql.Trim());
                gbFilter.Visible = options.DataKind != DataKind.EventAck;

                if (options.DataKind == DataKind.Current || options.DataKind == DataKind.Historical)
                    chkSingleQuery.Visible = btnEditParametrs.Visible = true;
                else
                    chkSingleQuery.Visible = btnEditParametrs.Visible = false;
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
                OnObjectChanged(TreeUpdateTypes.None);

                gbFilter.Visible = queryOptions.DataKind != DataKind.EventAck;
                
                if (queryOptions.DataKind == DataKind.Current || queryOptions.DataKind == DataKind.Historical)
                    chkSingleQuery.Visible = true;
                else
                    chkSingleQuery.Visible = false;
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
    }
}
