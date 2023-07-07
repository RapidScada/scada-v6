using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    public partial class FrmBitReader : Form
    {
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
    }
}
