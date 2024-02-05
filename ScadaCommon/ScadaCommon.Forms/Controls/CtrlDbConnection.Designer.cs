
namespace Scada.Forms.Controls
{
    partial class CtrlDbConnection
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            gbConnectionOptions = new GroupBox();
            pnlConnectionOptions = new Panel();
            txtConnectionString = new TextBox();
            lblName = new Label();
            chkConnectionString = new CheckBox();
            txtName = new TextBox();
            txtPassword = new TextBox();
            lblDbms = new Label();
            lblPassword = new Label();
            cbDbms = new ComboBox();
            txtUsername = new TextBox();
            lblServer = new Label();
            lblUsername = new Label();
            txtServer = new TextBox();
            txtDatabase = new TextBox();
            lblDatabase = new Label();
            gbConnectionOptions.SuspendLayout();
            pnlConnectionOptions.SuspendLayout();
            SuspendLayout();
            // 
            // gbConnectionOptions
            // 
            gbConnectionOptions.Controls.Add(pnlConnectionOptions);
            gbConnectionOptions.Dock = DockStyle.Fill;
            gbConnectionOptions.Location = new Point(0, 0);
            gbConnectionOptions.Name = "gbConnectionOptions";
            gbConnectionOptions.Padding = new Padding(10, 3, 10, 10);
            gbConnectionOptions.Size = new Size(300, 399);
            gbConnectionOptions.TabIndex = 0;
            gbConnectionOptions.TabStop = false;
            gbConnectionOptions.Text = "Connection Options";
            // 
            // pnlConnectionOptions
            // 
            pnlConnectionOptions.Controls.Add(txtConnectionString);
            pnlConnectionOptions.Controls.Add(lblName);
            pnlConnectionOptions.Controls.Add(chkConnectionString);
            pnlConnectionOptions.Controls.Add(txtName);
            pnlConnectionOptions.Controls.Add(txtPassword);
            pnlConnectionOptions.Controls.Add(lblDbms);
            pnlConnectionOptions.Controls.Add(lblPassword);
            pnlConnectionOptions.Controls.Add(cbDbms);
            pnlConnectionOptions.Controls.Add(txtUsername);
            pnlConnectionOptions.Controls.Add(lblServer);
            pnlConnectionOptions.Controls.Add(lblUsername);
            pnlConnectionOptions.Controls.Add(txtServer);
            pnlConnectionOptions.Controls.Add(txtDatabase);
            pnlConnectionOptions.Controls.Add(lblDatabase);
            pnlConnectionOptions.Location = new Point(13, 22);
            pnlConnectionOptions.Name = "pnlConnectionOptions";
            pnlConnectionOptions.Size = new Size(274, 364);
            pnlConnectionOptions.TabIndex = 1;
            // 
            // txtConnectionString
            // 
            txtConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtConnectionString.Location = new Point(0, 289);
            txtConnectionString.Multiline = true;
            txtConnectionString.Name = "txtConnectionString";
            txtConnectionString.ScrollBars = ScrollBars.Vertical;
            txtConnectionString.Size = new Size(274, 75);
            txtConnectionString.TabIndex = 13;
            txtConnectionString.TextChanged += txtConnectionString_TextChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(-3, -3);
            lblName.Name = "lblName";
            lblName.Size = new Size(39, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Name";
            // 
            // chkConnectionString
            // 
            chkConnectionString.AutoSize = true;
            chkConnectionString.Location = new Point(0, 264);
            chkConnectionString.Name = "chkConnectionString";
            chkConnectionString.Size = new Size(121, 19);
            chkConnectionString.TabIndex = 12;
            chkConnectionString.Text = "Connection string";
            chkConnectionString.UseVisualStyleBackColor = true;
            chkConnectionString.CheckedChanged += chkConnectionString_CheckedChanged;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(0, 15);
            txtName.Name = "txtName";
            txtName.Size = new Size(274, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += txtName_TextChanged;
            txtName.Validated += txtName_Validated;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPassword.Location = new Point(0, 235);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(274, 23);
            txtPassword.TabIndex = 11;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // lblDbms
            // 
            lblDbms.AutoSize = true;
            lblDbms.Location = new Point(-3, 41);
            lblDbms.Name = "lblDbms";
            lblDbms.Size = new Size(39, 15);
            lblDbms.TabIndex = 2;
            lblDbms.Text = "DBMS";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(-3, 217);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 10;
            lblPassword.Text = "Password";
            // 
            // cbDbms
            // 
            cbDbms.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbDbms.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDbms.FormattingEnabled = true;
            cbDbms.Location = new Point(0, 59);
            cbDbms.Name = "cbDbms";
            cbDbms.Size = new Size(274, 23);
            cbDbms.TabIndex = 3;
            cbDbms.SelectedIndexChanged += cbDbms_SelectedIndexChanged;
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUsername.Location = new Point(0, 191);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(274, 23);
            txtUsername.TabIndex = 9;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(-3, 85);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(39, 15);
            lblServer.TabIndex = 4;
            lblServer.Text = "Server";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(-3, 173);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(60, 15);
            lblUsername.TabIndex = 8;
            lblUsername.Text = "Username";
            // 
            // txtServer
            // 
            txtServer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtServer.Location = new Point(0, 103);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(274, 23);
            txtServer.TabIndex = 5;
            txtServer.TextChanged += txtServer_TextChanged;
            // 
            // txtDatabase
            // 
            txtDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDatabase.Location = new Point(0, 147);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new Size(274, 23);
            txtDatabase.TabIndex = 7;
            txtDatabase.TextChanged += txtDatabase_TextChanged;
            // 
            // lblDatabase
            // 
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new Point(-3, 129);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new Size(55, 15);
            lblDatabase.TabIndex = 6;
            lblDatabase.Text = "Database";
            // 
            // CtrlDbConnection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbConnectionOptions);
            Name = "CtrlDbConnection";
            Size = new Size(300, 399);
            gbConnectionOptions.ResumeLayout(false);
            pnlConnectionOptions.ResumeLayout(false);
            pnlConnectionOptions.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbConnectionOptions;
        private TextBox txtConnectionString;
        private CheckBox chkConnectionString;
        private TextBox txtPassword;
        private Label lblPassword;
        private TextBox txtUsername;
        private Label lblUsername;
        private TextBox txtDatabase;
        private Label lblDatabase;
        private TextBox txtServer;
        private Label lblServer;
        private TextBox txtName;
        private Label lblName;
        private ComboBox cbDbms;
        private Label lblDbms;
        private Panel pnlConnectionOptions;
    }
}
