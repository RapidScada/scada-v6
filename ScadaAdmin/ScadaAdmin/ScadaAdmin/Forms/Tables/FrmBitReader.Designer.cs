namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmBitReader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPageSplit = new System.Windows.Forms.TabPage();
            button1 = new System.Windows.Forms.Button();
            textBox3 = new System.Windows.Forms.TextBox();
            textBox2 = new System.Windows.Forms.TextBox();
            comboBox1 = new System.Windows.Forms.ComboBox();
            radioButton2 = new System.Windows.Forms.RadioButton();
            textBox1 = new System.Windows.Forms.TextBox();
            radioButton1 = new System.Windows.Forms.RadioButton();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            tabControl1.SuspendLayout();
            tabPageSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageSplit);
            tabControl1.Location = new System.Drawing.Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(776, 426);
            tabControl1.TabIndex = 0;
            // 
            // tabPageSplit
            // 
            tabPageSplit.Controls.Add(button1);
            tabPageSplit.Controls.Add(textBox3);
            tabPageSplit.Controls.Add(textBox2);
            tabPageSplit.Controls.Add(comboBox1);
            tabPageSplit.Controls.Add(radioButton2);
            tabPageSplit.Controls.Add(textBox1);
            tabPageSplit.Controls.Add(radioButton1);
            tabPageSplit.Controls.Add(label2);
            tabPageSplit.Controls.Add(label1);
            tabPageSplit.Controls.Add(dataGridView1);
            tabPageSplit.Location = new System.Drawing.Point(4, 29);
            tabPageSplit.Name = "tabPageSplit";
            tabPageSplit.Padding = new System.Windows.Forms.Padding(3);
            tabPageSplit.Size = new System.Drawing.Size(768, 393);
            tabPageSplit.TabIndex = 0;
            tabPageSplit.Text = "Split";
            tabPageSplit.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button1.Location = new System.Drawing.Point(658, 350);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(94, 29);
            button1.TabIndex = 9;
            button1.Text = "Apply";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox3
            // 
            textBox3.Enabled = false;
            textBox3.Location = new System.Drawing.Point(461, 351);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(125, 27);
            textBox3.TabIndex = 8;
            textBox3.Text = "[0;nbBit]";
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(330, 351);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "separator";
            textBox2.Size = new System.Drawing.Size(125, 27);
            textBox2.TabIndex = 7;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Name", "Tag Code" });
            comboBox1.Location = new System.Drawing.Point(173, 351);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(151, 28);
            comboBox1.TabIndex = 6;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(11, 352);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(156, 24);
            radioButton2.TabIndex = 5;
            radioButton2.TabStop = true;
            radioButton2.Text = "Custom generation";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new System.Drawing.Point(175, 310);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(125, 27);
            textBox1.TabIndex = 4;
            textBox1.Text = "Name_[0;nbBit]";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new System.Drawing.Point(11, 311);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(138, 24);
            radioButton1.TabIndex = 3;
            radioButton1.TabStop = true;
            radioButton1.Text = "Auto generation";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 279);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(105, 20);
            label2.TabIndex = 2;
            label2.Text = "Name format :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 24);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(78, 20);
            label1.TabIndex = 1;
            label1.Text = "You chose:";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new System.Drawing.Point(6, 57);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new System.Drawing.Size(756, 192);
            dataGridView1.TabIndex = 0;
            // 
            // FrmBitReader
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(797, 442);
            Controls.Add(tabControl1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "FrmBitReader";
            Text = "Bit Reader";
            tabControl1.ResumeLayout(false);
            tabPageSplit.ResumeLayout(false);
            tabPageSplit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSplit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_dt;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_tc;
    }
}