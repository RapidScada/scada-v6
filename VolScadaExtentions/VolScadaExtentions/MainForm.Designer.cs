namespace VolScadaExtentions
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            panel1 = new Panel();
            lbltips = new Label();
            panel2 = new Panel();
            dataGridView = new DataGridView();
            UserID = new DataGridViewTextBoxColumn();
            Names = new DataGridViewTextBoxColumn();
            Password = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewButtonColumn();
            btnSelectPath = new Button();
            label1 = new Label();
            txtDatPath = new TextBox();
            btnOpen = new Button();
            btnAgentPwd = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnAgentPwd);
            panel1.Controls.Add(lbltips);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnSelectPath);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtDatPath);
            panel1.Controls.Add(btnOpen);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(648, 367);
            panel1.TabIndex = 0;
            // 
            // lbltips
            // 
            lbltips.AutoSize = true;
            lbltips.Location = new Point(15, 342);
            lbltips.Name = "lbltips";
            lbltips.Size = new Size(32, 17);
            lbltips.TabIndex = 5;
            lbltips.Text = "提示";
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView);
            panel2.Location = new Point(12, 57);
            panel2.Name = "panel2";
            panel2.Size = new Size(627, 278);
            panel2.TabIndex = 4;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { UserID, Names, Password, Column1 });
            dataGridView.Location = new Point(4, 3);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowTemplate.Height = 25;
            dataGridView.Size = new Size(620, 268);
            dataGridView.TabIndex = 1;
            dataGridView.CellClick += dataGridView_CellClick;
            // 
            // UserID
            // 
            UserID.DataPropertyName = "UserID";
            UserID.HeaderText = "ID";
            UserID.Name = "UserID";
            UserID.ReadOnly = true;
            UserID.Width = 60;
            // 
            // Names
            // 
            Names.DataPropertyName = "Name";
            Names.HeaderText = "Name";
            Names.Name = "Names";
            Names.ReadOnly = true;
            // 
            // Password
            // 
            Password.DataPropertyName = "Password";
            Password.HeaderText = "Password";
            Password.Name = "Password";
            Password.ReadOnly = true;
            Password.Width = 249;
            // 
            // Column1
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "修改密码";
            Column1.DefaultCellStyle = dataGridViewCellStyle1;
            Column1.HeaderText = "ModifyPwd";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.True;
            Column1.SortMode = DataGridViewColumnSortMode.Automatic;
            Column1.Text = "修改";
            Column1.Width = 120;
            // 
            // btnSelectPath
            // 
            btnSelectPath.Location = new Point(367, 18);
            btnSelectPath.Name = "btnSelectPath";
            btnSelectPath.Size = new Size(77, 23);
            btnSelectPath.TabIndex = 3;
            btnSelectPath.Text = "选择路径";
            btnSelectPath.UseVisualStyleBackColor = true;
            btnSelectPath.Click += btnSelectPath_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 21);
            label1.Name = "label1";
            label1.Size = new Size(52, 17);
            label1.TabIndex = 2;
            label1.Text = "Dat路径";
            // 
            // txtDatPath
            // 
            txtDatPath.Location = new Point(71, 18);
            txtDatPath.Name = "txtDatPath";
            txtDatPath.Size = new Size(280, 23);
            txtDatPath.TabIndex = 1;
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(450, 18);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(77, 23);
            btnOpen.TabIndex = 0;
            btnOpen.Text = "打开";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;
            // 
            // btnAgentPwd
            // 
            btnAgentPwd.Location = new Point(533, 18);
            btnAgentPwd.Name = "btnAgentPwd";
            btnAgentPwd.Size = new Size(103, 23);
            btnAgentPwd.TabIndex = 6;
            btnAgentPwd.Text = "Agent密码加密";
            btnAgentPwd.UseVisualStyleBackColor = true;
            btnAgentPwd.Click += btnAgentPwd_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(648, 367);
            Controls.Add(panel1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            Text = "修改系统账号辅助工具";
            Load += MainForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnSelectPath;
        private Label label1;
        private TextBox txtDatPath;
        private Button btnOpen;
        private Panel panel2;
        private DataGridView dataGridView;
        private DataGridViewTextBoxColumn UserID;
        private DataGridViewTextBoxColumn Names;
        private DataGridViewTextBoxColumn Password;
        private DataGridViewButtonColumn Column1;
        private Label lbltips;
        private Button btnAgentPwd;
    }
}
