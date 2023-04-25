using Scada.Data.Entities;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
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