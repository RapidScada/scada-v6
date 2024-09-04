namespace Scada.Admin.Extensions.ExtExternalTools.Forms
{
    partial class FrmExtensionConfig
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
            lvTool = new ListView();
            btnAdd = new Button();
            btnMoveUp = new Button();
            btnMoveDown = new Button();
            btnDelete = new Button();
            gbTool = new GroupBox();
            btnBrowseDir = new Button();
            txtWorkingDirectory = new TextBox();
            lblWorkingDirectory = new Label();
            btnAddArgument = new Button();
            txtArguments = new TextBox();
            lblArguments = new Label();
            btnBrowseFile = new Button();
            txtFileName = new TextBox();
            lblFileName = new Label();
            txtTitle = new TextBox();
            lblTitle = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            gbTool.SuspendLayout();
            SuspendLayout();
            // 
            // lvTool
            // 
            lvTool.HeaderStyle = ColumnHeaderStyle.None;
            lvTool.Location = new Point(12, 12);
            lvTool.MultiSelect = false;
            lvTool.Name = "lvTool";
            lvTool.Size = new Size(274, 200);
            lvTool.TabIndex = 0;
            lvTool.UseCompatibleStateImageBehavior = false;
            lvTool.View = View.Details;
            lvTool.SelectedIndexChanged += lvTool_SelectedIndexChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(292, 13);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(80, 23);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnMoveUp
            // 
            btnMoveUp.Location = new Point(292, 41);
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(80, 23);
            btnMoveUp.TabIndex = 2;
            btnMoveUp.Text = "Move Up";
            btnMoveUp.UseVisualStyleBackColor = true;
            btnMoveUp.Click += btnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.Location = new Point(292, 70);
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(80, 23);
            btnMoveDown.TabIndex = 3;
            btnMoveDown.Text = "Move Down";
            btnMoveDown.UseVisualStyleBackColor = true;
            btnMoveDown.Click += btnMoveDown_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(292, 99);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(80, 23);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // gbTool
            // 
            gbTool.Controls.Add(btnBrowseDir);
            gbTool.Controls.Add(txtWorkingDirectory);
            gbTool.Controls.Add(lblWorkingDirectory);
            gbTool.Controls.Add(btnAddArgument);
            gbTool.Controls.Add(txtArguments);
            gbTool.Controls.Add(lblArguments);
            gbTool.Controls.Add(btnBrowseFile);
            gbTool.Controls.Add(txtFileName);
            gbTool.Controls.Add(lblFileName);
            gbTool.Controls.Add(txtTitle);
            gbTool.Controls.Add(lblTitle);
            gbTool.Location = new Point(12, 218);
            gbTool.Name = "gbTool";
            gbTool.Padding = new Padding(10, 3, 10, 10);
            gbTool.Size = new Size(360, 205);
            gbTool.TabIndex = 5;
            gbTool.TabStop = false;
            gbTool.Text = "Selected Tool";
            // 
            // btnBrowseDir
            // 
            btnBrowseDir.FlatStyle = FlatStyle.Popup;
            btnBrowseDir.Location = new Point(324, 169);
            btnBrowseDir.Name = "btnBrowseDir";
            btnBrowseDir.Size = new Size(23, 23);
            btnBrowseDir.TabIndex = 10;
            btnBrowseDir.Text = "…";
            btnBrowseDir.UseVisualStyleBackColor = true;
            btnBrowseDir.Click += btnBrowseDir_Click;
            // 
            // txtWorkingDirectory
            // 
            txtWorkingDirectory.Location = new Point(13, 169);
            txtWorkingDirectory.Name = "txtWorkingDirectory";
            txtWorkingDirectory.Size = new Size(305, 23);
            txtWorkingDirectory.TabIndex = 9;
            txtWorkingDirectory.TextChanged += txtWorkingDirectory_TextChanged;
            // 
            // lblWorkingDirectory
            // 
            lblWorkingDirectory.AutoSize = true;
            lblWorkingDirectory.Location = new Point(10, 151);
            lblWorkingDirectory.Name = "lblWorkingDirectory";
            lblWorkingDirectory.Size = new Size(102, 15);
            lblWorkingDirectory.TabIndex = 8;
            lblWorkingDirectory.Text = "Working directory";
            // 
            // btnAddArgument
            // 
            btnAddArgument.FlatStyle = FlatStyle.Popup;
            btnAddArgument.Location = new Point(324, 125);
            btnAddArgument.Name = "btnAddArgument";
            btnAddArgument.Size = new Size(23, 23);
            btnAddArgument.TabIndex = 7;
            btnAddArgument.Text = "+";
            btnAddArgument.UseVisualStyleBackColor = true;
            btnAddArgument.Click += btnAddArgument_Click;
            // 
            // txtArguments
            // 
            txtArguments.Location = new Point(13, 125);
            txtArguments.Name = "txtArguments";
            txtArguments.Size = new Size(305, 23);
            txtArguments.TabIndex = 6;
            txtArguments.TextChanged += txtArguments_TextChanged;
            // 
            // lblArguments
            // 
            lblArguments.AutoSize = true;
            lblArguments.Location = new Point(10, 107);
            lblArguments.Name = "lblArguments";
            lblArguments.Size = new Size(66, 15);
            lblArguments.TabIndex = 5;
            lblArguments.Text = "Arguments";
            // 
            // btnBrowseFile
            // 
            btnBrowseFile.FlatStyle = FlatStyle.Popup;
            btnBrowseFile.Location = new Point(324, 81);
            btnBrowseFile.Name = "btnBrowseFile";
            btnBrowseFile.Size = new Size(23, 23);
            btnBrowseFile.TabIndex = 4;
            btnBrowseFile.Text = "…";
            btnBrowseFile.UseVisualStyleBackColor = true;
            btnBrowseFile.Click += btnBrowseFile_Click;
            // 
            // txtFileName
            // 
            txtFileName.Location = new Point(13, 81);
            txtFileName.Name = "txtFileName";
            txtFileName.Size = new Size(305, 23);
            txtFileName.TabIndex = 3;
            txtFileName.TextChanged += txtFileName_TextChanged;
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Location = new Point(10, 63);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(58, 15);
            lblFileName.TabIndex = 2;
            lblFileName.Text = "File name";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(13, 37);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(334, 23);
            txtTitle.TabIndex = 1;
            txtTitle.TextChanged += txtTitle_TextChanged;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(10, 19);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(29, 15);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Title";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 439);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 6;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 439);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmExtensionConfig
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 474);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(gbTool);
            Controls.Add(btnDelete);
            Controls.Add(btnMoveDown);
            Controls.Add(btnMoveUp);
            Controls.Add(btnAdd);
            Controls.Add(lvTool);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmExtensionConfig";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "External Tools";
            Load += FrmExtensionConfig_Load;
            gbTool.ResumeLayout(false);
            gbTool.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListView lvTool;
        private Button btnAdd;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnDelete;
        private GroupBox gbTool;
        private Label lblTitle;
        private TextBox txtTitle;
        private TextBox txtFileName;
        private Label lblFileName;
        private Button btnBrowseFile;
        private TextBox txtArguments;
        private Label lblArguments;
        private Button btnAddArgument;
        private Label lblWorkingDirectory;
        private Button btnBrowseDir;
        private TextBox txtWorkingDirectory;
        private Button btnOK;
        private Button btnCancel;
    }
}