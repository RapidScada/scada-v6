// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for preview created channels.
    /// <para>Представляет форму для предварительного просмотра созданных каналов.</para>
    /// </summary>
    public partial class FrmCnlPreview : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCnlPreview()
        {
            InitializeComponent();
            dataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlPreview(List<Cnl> cnls)
            : this()
        {
            bindingSource.DataSource = cnls ?? throw new ArgumentNullException(nameof(cnls));
        }


        private void FrmCnlPreview_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            dataGridView.AutoSizeColumns();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // delete selected rows
            DataGridViewSelectedRowCollection selectedRows = dataGridView.SelectedRows;

            if (selectedRows.Count > 0)
            {
                for (int i = selectedRows.Count - 1; i >= 0; i--)
                {
                    dataGridView.Rows.RemoveAt(selectedRows[i].Index);
                }
            }
            else if (dataGridView.CurrentRow != null)
            {
                dataGridView.Rows.RemoveAt(dataGridView.CurrentRow.Index);
            }
        }

        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            btnAdd.Enabled = dataGridView.Rows.Count > 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
