
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
            this.gbConnectionOptions = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.chkConnectionString = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.cbDbms = new System.Windows.Forms.ComboBox();
            this.lblDbms = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbConnectionOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConnectionOptions
            // 
            this.gbConnectionOptions.Controls.Add(this.txtConnectionString);
            this.gbConnectionOptions.Controls.Add(this.chkConnectionString);
            this.gbConnectionOptions.Controls.Add(this.txtPassword);
            this.gbConnectionOptions.Controls.Add(this.lblPassword);
            this.gbConnectionOptions.Controls.Add(this.txtUsername);
            this.gbConnectionOptions.Controls.Add(this.lblUsername);
            this.gbConnectionOptions.Controls.Add(this.txtDatabase);
            this.gbConnectionOptions.Controls.Add(this.lblDatabase);
            this.gbConnectionOptions.Controls.Add(this.txtServer);
            this.gbConnectionOptions.Controls.Add(this.lblServer);
            this.gbConnectionOptions.Controls.Add(this.cbDbms);
            this.gbConnectionOptions.Controls.Add(this.lblDbms);
            this.gbConnectionOptions.Controls.Add(this.txtName);
            this.gbConnectionOptions.Controls.Add(this.lblName);
            this.gbConnectionOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConnectionOptions.Location = new System.Drawing.Point(0, 0);
            this.gbConnectionOptions.Name = "gbConnectionOptions";
            this.gbConnectionOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnectionOptions.Size = new System.Drawing.Size(300, 399);
            this.gbConnectionOptions.TabIndex = 0;
            this.gbConnectionOptions.TabStop = false;
            this.gbConnectionOptions.Text = "Connection Options";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(13, 311);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConnectionString.Size = new System.Drawing.Size(274, 75);
            this.txtConnectionString.TabIndex = 13;
            this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
            // 
            // chkConnectionString
            // 
            this.chkConnectionString.AutoSize = true;
            this.chkConnectionString.Location = new System.Drawing.Point(13, 286);
            this.chkConnectionString.Name = "chkConnectionString";
            this.chkConnectionString.Size = new System.Drawing.Size(121, 19);
            this.chkConnectionString.TabIndex = 12;
            this.chkConnectionString.Text = "Connection string";
            this.chkConnectionString.UseVisualStyleBackColor = true;
            this.chkConnectionString.CheckedChanged += new System.EventHandler(this.chkConnectionString_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(13, 257);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(274, 23);
            this.txtPassword.TabIndex = 11;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 239);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(13, 213);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(274, 23);
            this.txtUsername.TabIndex = 9;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 195);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 8;
            this.lblUsername.Text = "Username";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(13, 169);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(274, 23);
            this.txtDatabase.TabIndex = 7;
            this.txtDatabase.TextChanged += new System.EventHandler(this.txtDatabase_TextChanged);
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(10, 151);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(55, 15);
            this.lblDatabase.TabIndex = 6;
            this.lblDatabase.Text = "Database";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(13, 125);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(274, 23);
            this.txtServer.TabIndex = 5;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(10, 107);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(39, 15);
            this.lblServer.TabIndex = 4;
            this.lblServer.Text = "Server";
            // 
            // cbDbms
            // 
            this.cbDbms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDbms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDbms.FormattingEnabled = true;
            this.cbDbms.Items.AddRange(new object[] {
            "Undefined",
            "PostgreSQL",
            "MySQL",
            "MSSQL",
            "Oracle",
            "OLEDB"});
            this.cbDbms.Location = new System.Drawing.Point(13, 81);
            this.cbDbms.Name = "cbDbms";
            this.cbDbms.Size = new System.Drawing.Size(274, 23);
            this.cbDbms.TabIndex = 3;
            this.cbDbms.SelectedIndexChanged += new System.EventHandler(this.cbDbms_SelectedIndexChanged);
            // 
            // lblDbms
            // 
            this.lblDbms.AutoSize = true;
            this.lblDbms.Location = new System.Drawing.Point(10, 63);
            this.lblDbms.Name = "lblDbms";
            this.lblDbms.Size = new System.Drawing.Size(39, 15);
            this.lblDbms.TabIndex = 2;
            this.lblDbms.Text = "DBMS";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(13, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(274, 23);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.Validated += new System.EventHandler(this.txtName_Validated);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 19);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // CtrlDbConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbConnectionOptions);
            this.Name = "CtrlDbConnection";
            this.Size = new System.Drawing.Size(300, 399);
            this.gbConnectionOptions.ResumeLayout(false);
            this.gbConnectionOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnectionOptions;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.CheckBox chkConnectionString;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cbDbms;
        private System.Windows.Forms.Label lblDbms;
    }
}
