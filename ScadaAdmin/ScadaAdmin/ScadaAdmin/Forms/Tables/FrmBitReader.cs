using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    public partial class FrmBitReader : Form
    {
        private List<DataRow> _selectedRows = new List<DataRow>();

        private DataTable _dt = new DataTable();

        public FrmBitReader()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboBox1.Visible = textBox2.Visible = textBox3.Visible = true;
                textBox1.Visible = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                comboBox1.Visible = textBox2.Visible = textBox3.Visible = false;
                textBox1.Visible = true;
            }
        }

        private void dataTableCreation()
        {
            _dt.Columns.Add("Number", typeof(int));
            _dt.Columns.Add("Name", typeof(string));
            _dt.Columns.Add("Data Type", typeof(string));
            _dt.Columns.Add("Tag Number", typeof(string));
            _dt.Columns.Add("Tag Code", typeof(string));

            foreach (DataRow line in _selectedRows)
            {
                if (line[3].ToString() == "1" || line[3].ToString() == "0")
                    _dt.Rows.Add(line[0].ToString() == null ? "" : line[0], line[2].ToString() == null ? "" : line[2], line[3].ToString() == "1" ? "Integer" : "Double", line[8].ToString() == null ? "" : line[8], line[9].ToString() == null ? "" : line[9]);
            }
            dataGridView1.DataSource = _dt;
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 200;

            dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

        }

        public void setSelectedRows(List<DataRow> lstDR)
        {
            foreach (DataRow row in lstDR)
                _selectedRows.Add(row);

            dataTableCreation();
        }
    }
}
