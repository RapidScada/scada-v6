
namespace Scada.Server.Modules.ModArcPostgreSql.View.Forms
{
    partial class FrmConnManager
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
            this.lbConn = new System.Windows.Forms.ListBox();
            this.btnNewConn = new System.Windows.Forms.Button();
            this.btnDeleteConn = new System.Windows.Forms.Button();
            this.gbConnOptions = new System.Windows.Forms.GroupBox();
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbConnList = new System.Windows.Forms.GroupBox();
            this.gbConnOptions.SuspendLayout();
            this.gbConnList.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbConn
            // 
            this.lbConn.HorizontalScrollbar = true;
            this.lbConn.IntegralHeight = false;
            this.lbConn.ItemHeight = 15;
            this.lbConn.Location = new System.Drawing.Point(13, 51);
            this.lbConn.Name = "lbConn";
            this.lbConn.Size = new System.Drawing.Size(194, 291);
            this.lbConn.Sorted = true;
            this.lbConn.TabIndex = 2;
            this.lbConn.SelectedIndexChanged += new System.EventHandler(this.lbConn_SelectedIndexChanged);
            // 
            // btnNewConn
            // 
            this.btnNewConn.Location = new System.Drawing.Point(13, 22);
            this.btnNewConn.Name = "btnNewConn";
            this.btnNewConn.Size = new System.Drawing.Size(94, 23);
            this.btnNewConn.TabIndex = 0;
            this.btnNewConn.Text = "New";
            this.btnNewConn.UseVisualStyleBackColor = true;
            this.btnNewConn.Click += new System.EventHandler(this.btnNewConn_Click);
            // 
            // btnDeleteConn
            // 
            this.btnDeleteConn.Location = new System.Drawing.Point(113, 22);
            this.btnDeleteConn.Name = "btnDeleteConn";
            this.btnDeleteConn.Size = new System.Drawing.Size(94, 23);
            this.btnDeleteConn.TabIndex = 1;
            this.btnDeleteConn.Text = "Delete";
            this.btnDeleteConn.UseVisualStyleBackColor = true;
            this.btnDeleteConn.Click += new System.EventHandler(this.btnDeleteConn_Click);
            // 
            // gbConnOptions
            // 
            this.gbConnOptions.Controls.Add(this.txtConnectionString);
            this.gbConnOptions.Controls.Add(this.chkConnectionString);
            this.gbConnOptions.Controls.Add(this.txtPassword);
            this.gbConnOptions.Controls.Add(this.lblPassword);
            this.gbConnOptions.Controls.Add(this.txtUsername);
            this.gbConnOptions.Controls.Add(this.lblUsername);
            this.gbConnOptions.Controls.Add(this.txtDatabase);
            this.gbConnOptions.Controls.Add(this.lblDatabase);
            this.gbConnOptions.Controls.Add(this.txtServer);
            this.gbConnOptions.Controls.Add(this.lblServer);
            this.gbConnOptions.Controls.Add(this.txtName);
            this.gbConnOptions.Controls.Add(this.lblName);
            this.gbConnOptions.Location = new System.Drawing.Point(238, 12);
            this.gbConnOptions.Name = "gbConnOptions";
            this.gbConnOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnOptions.Size = new System.Drawing.Size(300, 355);
            this.gbConnOptions.TabIndex = 1;
            this.gbConnOptions.TabStop = false;
            this.gbConnOptions.Text = "Connection Options";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(13, 267);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConnectionString.Size = new System.Drawing.Size(274, 75);
            this.txtConnectionString.TabIndex = 11;
            this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
            // 
            // chkConnectionString
            // 
            this.chkConnectionString.AutoSize = true;
            this.chkConnectionString.Location = new System.Drawing.Point(13, 242);
            this.chkConnectionString.Name = "chkConnectionString";
            this.chkConnectionString.Size = new System.Drawing.Size(121, 19);
            this.chkConnectionString.TabIndex = 10;
            this.chkConnectionString.Text = "Connection string";
            this.chkConnectionString.UseVisualStyleBackColor = true;
            this.chkConnectionString.CheckedChanged += new System.EventHandler(this.chkConnectionString_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(13, 213);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(274, 23);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 195);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(13, 169);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(274, 23);
            this.txtUsername.TabIndex = 7;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 151);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Username";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(13, 125);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(274, 23);
            this.txtDatabase.TabIndex = 5;
            this.txtDatabase.TextChanged += new System.EventHandler(this.txtDatabase_TextChanged);
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(10, 107);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(55, 15);
            this.lblDatabase.TabIndex = 4;
            this.lblDatabase.Text = "Database";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(13, 81);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(274, 23);
            this.txtServer.TabIndex = 3;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(10, 63);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(39, 15);
            this.lblServer.TabIndex = 2;
            this.lblServer.Text = "Server";
            // 
            // txtName
            // 
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
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(463, 383);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(382, 383);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbConnList
            // 
            this.gbConnList.Controls.Add(this.lbConn);
            this.gbConnList.Controls.Add(this.btnDeleteConn);
            this.gbConnList.Controls.Add(this.btnNewConn);
            this.gbConnList.Location = new System.Drawing.Point(12, 12);
            this.gbConnList.Name = "gbConnList";
            this.gbConnList.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnList.Size = new System.Drawing.Size(220, 355);
            this.gbConnList.TabIndex = 0;
            this.gbConnList.TabStop = false;
            this.gbConnList.Text = "Connections";
            // 
            // FrmConnManager
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(550, 418);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbConnOptions);
            this.Controls.Add(this.gbConnList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConnManager";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PostgreSQL Connections";
            this.Load += new System.EventHandler(this.FrmConnManager_Load);
            this.gbConnOptions.ResumeLayout(false);
            this.gbConnOptions.PerformLayout();
            this.gbConnList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbConn;
        private System.Windows.Forms.Button btnNewConn;
        private System.Windows.Forms.Button btnDeleteConn;
        private System.Windows.Forms.GroupBox gbConnOptions;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.CheckBox chkConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbConnList;
    }
}