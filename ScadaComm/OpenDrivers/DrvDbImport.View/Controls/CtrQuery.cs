// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using Scada.Comm.Drivers.DrvDbImport.Config;
using Scada.Forms;
using static System.Windows.Forms.Design.AxImporter;

namespace Scada.Comm.Drivers.DrvDbImport.View.Controls
{
    /// <summary>
    /// Represents a control for editing query options.
    /// <para>Представляет элемент управления для редактирования параметров запросов.</para>
    /// </summary>
    public partial class CtrQuery : UserControl
    {
        private QueryConfig queryConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the edited query.
        /// </summary>
        internal QueryConfig QueryConfig
        {
            get
            {
                return queryConfig;
            }
            set
            {
                queryConfig = null;
                ShowCommandProps(value);
                queryConfig = value;
            }
        }

        /// <summary>
        /// Shows the command properties.
        /// </summary>
        private void ShowCommandProps(QueryConfig queryConfig)
        {
            if (queryConfig == null)
            {
                chkActive.Checked = false;
                txtName.Text = "";
                txtTags.Text = "";
                chkSingleRow.Checked = false;
                txtSql.Text = "";
            }
            else
            {
                chkActive.Checked = queryConfig.Active;
                txtName.Text = queryConfig.Name;

                txtTags.Clear();
                foreach (string tag in queryConfig.TagCodes)
                {
                    txtTags.AppendText(tag);
                    txtTags.AppendText(Environment.NewLine);
                }

                chkSingleRow.Checked = queryConfig.SingleRow;
                txtSql.Clear();
                txtSql.AppendText(queryConfig.Sql.Replace("\n", Environment.NewLine));
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(queryConfig, changeArgument));
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
            if (queryConfig != null)
            {
                queryConfig.Active = chkActive.Checked;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (queryConfig != null)
            {
                queryConfig.Name = txtName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtTags_TextChanged(object sender, EventArgs e)
        {
            if (queryConfig != null)
            {
                queryConfig.TagCodes.Clear();
                
                for (int i = 0; i < txtTags.Lines.Count(); i++)
                {
                    queryConfig.TagCodes.Add(txtTags.Lines[i]);
                }

                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void chkSingleRow_CheckedChanged(object sender, EventArgs e)
        {
            if (queryConfig != null)
            {
                queryConfig.SingleRow = chkActive.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtSql_TextChanged(object sender, EventArgs e)
        {
            if (queryConfig != null)
            {
                queryConfig.Sql = txtSql.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
