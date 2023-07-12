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
        public List<DataRow> newRows = new List<DataRow>();
        public DataTable newDt = new DataTable();
        public int numberOfLastChannel;

        private List<DataRow> _selectedRows = new List<DataRow>();
        private DataTable _dt = new DataTable();


        public FrmBitReader()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            newDt.Rows.Clear();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboBox1.Enabled = textBox2.Enabled = true;
                textBox1.Enabled = textBox3.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                comboBox1.Enabled = textBox2.Enabled = textBox3.Enabled = false;
                textBox1.Enabled = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 1;

            if (radioButton1.Checked)
            {
                foreach (DataRow line in _selectedRows)
                {
                    //double
                    if (line[3].ToString() == "0")
                    {
                        for (int i = 0; i < 32; i++)
                        {
                            DataRow newRow = line.Table.Clone().NewRow();

                            for (int j = 0; j < line.ItemArray.Count(); j++)
                            {
                                newRow[j] = line[j];
                            }

                            //number
                            newRow[0] = $"{numberOfLastChannel + count}";
                            //name
                            newRow[2] = $"{line[2]}_{i}";
                            //type
                            newRow[3] = DBNull.Value;
                            //data lenght
                            newRow[4] = DBNull.Value;
                            //tag_code
                            newRow[9] = $"{line[9]}_{i}";
                            //formula_enabled
                            newRow[10] = true;
                            //input_formula
                            newRow[11] = $"Getbit(Val({line[0]}),{i})";
                            newRows.Add(newRow);

                            count++;
                        }
                    }
                    //integer
                    else if (line[3].ToString() == "1")
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            DataRow newRow = line.Table.Clone().NewRow();

                            for (int j = 0; j < line.ItemArray.Count(); j++)
                                newRow[j] = line[j];

                            //number
                            newRow[0] = $"{numberOfLastChannel + count}";
                            //name
                            newRow[2] = $"{line[2]}_{i}";
                            //type
                            newRow[3] = DBNull.Value;
                            //data lenght
                            newRow[4] = DBNull.Value;
                            //tag_code
                            newRow[9] = $"{line[9]}_{i}";
                            //formula_enabled
                            newRow[10] = true;
                            //input_formula
                            newRow[11] = $"Getbit(Val({line[0]}),{i})";
                            newRows.Add(newRow);

                            count++;
                        }
                    }
                }
            }
            else if (radioButton2.Checked)
            {
                foreach (DataRow line in _selectedRows)
                {
                    //double
                    if (line[3].ToString() == "0")
                    {
                        for (int i = 0; i < 32; i++)
                        {
                            DataRow newRow = line.Table.Clone().NewRow();

                            for (int j = 0; j < line.ItemArray.Count(); j++)
                            {
                                newRow[j] = line[j];
                            }

                            //number
                            newRow[0] = $"{numberOfLastChannel + count}";
                            //name
                            string val = "";
                            if (comboBox1.SelectedItem != null)
                                val = comboBox1.SelectedItem.ToString() == "Name" ? line[2].ToString() : line[9].ToString();
                            newRow[2] = $"{val}{textBox2.Text}{i}";
                            //type
                            newRow[3] = DBNull.Value;
                            //data lenght
                            newRow[4] = DBNull.Value;
                            //tag_code
                            newRow[9] = $"{line[9]}{textBox2.Text}{i}";
                            //formula_enabled
                            newRow[10] = true;
                            //input_formula
                            newRow[11] = $"Getbit(Val({line[0]}),{i})";
                            newRows.Add(newRow);

                            count++;
                        }
                    }
                    //integer
                    else if (line[3].ToString() == "1")
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            DataRow newRow = line.Table.Clone().NewRow();

                            for (int j = 0; j < line.ItemArray.Count(); j++)
                                newRow[j] = line[j];

                            //number
                            newRow[0] = $"{numberOfLastChannel + count}";
                            //name
                            string val = "";
                            if (comboBox1.SelectedItem != null)
                                val = comboBox1.SelectedItem.ToString() == "Name" ? line[2].ToString() : line[9].ToString();
                            newRow[2] = $"{val}{textBox2.Text}{i}";
                            //type
                            newRow[3] = DBNull.Value;
                            //data lenght
                            newRow[4] = DBNull.Value;
                            //tag_code
                            newRow[9] = $"{line[9]}{textBox2.Text}{i}";
                            //formula_enabled
                            newRow[10] = true;
                            //input_formula
                            newRow[11] = $"Getbit(Val({line[0]}),{i})";
                            newRows.Add(newRow);

                            count++;
                        }
                    }
                }
            }
        }
    }
}
