namespace Scada.Server.Modules.ModArcInfluxDb.View.Forms
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
            this.gbConnList = new System.Windows.Forms.GroupBox();
            this.lvConn = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.btnDeleteConn = new System.Windows.Forms.Button();
            this.btnNewConn = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.txtOrg = new System.Windows.Forms.TextBox();
            this.lblOrg = new System.Windows.Forms.Label();
            this.txtBucket = new System.Windows.Forms.TextBox();
            this.lblBucket = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbConnList.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConnList
            // 
            this.gbConnList.Controls.Add(this.lvConn);
            this.gbConnList.Controls.Add(this.btnDeleteConn);
            this.gbConnList.Controls.Add(this.btnNewConn);
            this.gbConnList.Location = new System.Drawing.Point(12, 12);
            this.gbConnList.Name = "gbConnList";
            this.gbConnList.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnList.Size = new System.Drawing.Size(220, 399);
            this.gbConnList.TabIndex = 0;
            this.gbConnList.TabStop = false;
            this.gbConnList.Text = "Connections";
            // 
            // lvConn
            // 
            this.lvConn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
            this.lvConn.FullRowSelect = true;
            this.lvConn.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvConn.Location = new System.Drawing.Point(13, 51);
            this.lvConn.MultiSelect = false;
            this.lvConn.Name = "lvConn";
            this.lvConn.ShowGroups = false;
            this.lvConn.ShowItemToolTips = true;
            this.lvConn.Size = new System.Drawing.Size(194, 335);
            this.lvConn.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvConn.TabIndex = 2;
            this.lvConn.UseCompatibleStateImageBehavior = false;
            this.lvConn.View = System.Windows.Forms.View.Details;
            this.lvConn.SelectedIndexChanged += new System.EventHandler(this.lvConn_SelectedIndexChanged);
            // 
            // colName
            // 
            this.colName.Width = 186;
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
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(463, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(382, 427);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.txtOrg);
            this.gbOptions.Controls.Add(this.lblOrg);
            this.gbOptions.Controls.Add(this.txtBucket);
            this.gbOptions.Controls.Add(this.lblBucket);
            this.gbOptions.Controls.Add(this.txtPassword);
            this.gbOptions.Controls.Add(this.lblPassword);
            this.gbOptions.Controls.Add(this.txtUsername);
            this.gbOptions.Controls.Add(this.lblUsername);
            this.gbOptions.Controls.Add(this.txtToken);
            this.gbOptions.Controls.Add(this.lblToken);
            this.gbOptions.Controls.Add(this.txtUrl);
            this.gbOptions.Controls.Add(this.lblUrl);
            this.gbOptions.Controls.Add(this.txtName);
            this.gbOptions.Controls.Add(this.lblName);
            this.gbOptions.Location = new System.Drawing.Point(238, 12);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(300, 399);
            this.gbOptions.TabIndex = 1;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Connection Options";
            // 
            // txtOrg
            // 
            this.txtOrg.Location = new System.Drawing.Point(13, 301);
            this.txtOrg.Name = "txtOrg";
            this.txtOrg.Size = new System.Drawing.Size(274, 23);
            this.txtOrg.TabIndex = 13;
            this.txtOrg.TextChanged += new System.EventHandler(this.txtOrg_TextChanged);
            // 
            // lblOrg
            // 
            this.lblOrg.AutoSize = true;
            this.lblOrg.Location = new System.Drawing.Point(10, 283);
            this.lblOrg.Name = "lblOrg";
            this.lblOrg.Size = new System.Drawing.Size(75, 15);
            this.lblOrg.TabIndex = 12;
            this.lblOrg.Text = "Organization";
            // 
            // txtBucket
            // 
            this.txtBucket.Location = new System.Drawing.Point(13, 257);
            this.txtBucket.Name = "txtBucket";
            this.txtBucket.Size = new System.Drawing.Size(274, 23);
            this.txtBucket.TabIndex = 11;
            this.txtBucket.TextChanged += new System.EventHandler(this.txtBucket_TextChanged);
            // 
            // lblBucket
            // 
            this.lblBucket.AutoSize = true;
            this.lblBucket.Location = new System.Drawing.Point(10, 239);
            this.lblBucket.Name = "lblBucket";
            this.lblBucket.Size = new System.Drawing.Size(43, 15);
            this.lblBucket.TabIndex = 10;
            this.lblBucket.Text = "Bucket";
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
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(13, 125);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(274, 23);
            this.txtToken.TabIndex = 5;
            this.txtToken.UseSystemPasswordChar = true;
            this.txtToken.TextChanged += new System.EventHandler(this.txtToken_TextChanged);
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(10, 107);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(38, 15);
            this.lblToken.TabIndex = 4;
            this.lblToken.Text = "Token";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(13, 81);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(274, 23);
            this.txtUrl.TabIndex = 3;
            this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(10, 63);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(28, 15);
            this.lblUrl.TabIndex = 2;
            this.lblUrl.Text = "URL";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(13, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(274, 23);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
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
            // FrmConnManager
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(550, 462);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbConnList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConnManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InfluxDB Connections";
            this.Load += new System.EventHandler(this.FrmConnManager_Load);
            this.gbConnList.ResumeLayout(false);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbConnList;
        private ListView lvConn;
        private ColumnHeader colName;
        private Button btnDeleteConn;
        private Button btnNewConn;
        private Button btnCancel;
        private Button btnOK;
        private GroupBox gbOptions;
        private Label lblName;
        private TextBox txtName;
        private Label lblUrl;
        private TextBox txtUrl;
        private Label lblToken;
        private Label lblUsername;
        private TextBox txtToken;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblPassword;
        private Label lblBucket;
        private TextBox txtBucket;
        private TextBox txtOrg;
        private Label lblOrg;
    }
}