
namespace Scada.Forms.Controls
{
    partial class CtrlClientConnection
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
            this.btnPaste = new System.Windows.Forms.Button();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.lblSecretKey = new System.Windows.Forms.Label();
            this.txtInstance = new System.Windows.Forms.TextBox();
            this.lblInstance = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbConnectionOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // gbConnectionOptions
            // 
            this.gbConnectionOptions.Controls.Add(this.btnPaste);
            this.gbConnectionOptions.Controls.Add(this.txtSecretKey);
            this.gbConnectionOptions.Controls.Add(this.lblSecretKey);
            this.gbConnectionOptions.Controls.Add(this.txtInstance);
            this.gbConnectionOptions.Controls.Add(this.lblInstance);
            this.gbConnectionOptions.Controls.Add(this.txtPassword);
            this.gbConnectionOptions.Controls.Add(this.lblPassword);
            this.gbConnectionOptions.Controls.Add(this.txtUsername);
            this.gbConnectionOptions.Controls.Add(this.lblUsername);
            this.gbConnectionOptions.Controls.Add(this.numTimeout);
            this.gbConnectionOptions.Controls.Add(this.lblTimeout);
            this.gbConnectionOptions.Controls.Add(this.numPort);
            this.gbConnectionOptions.Controls.Add(this.lblPort);
            this.gbConnectionOptions.Controls.Add(this.txtHost);
            this.gbConnectionOptions.Controls.Add(this.lblHost);
            this.gbConnectionOptions.Controls.Add(this.txtName);
            this.gbConnectionOptions.Controls.Add(this.lblName);
            this.gbConnectionOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConnectionOptions.Location = new System.Drawing.Point(0, 0);
            this.gbConnectionOptions.Name = "gbConnectionOptions";
            this.gbConnectionOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnectionOptions.Size = new System.Drawing.Size(300, 366);
            this.gbConnectionOptions.TabIndex = 0;
            this.gbConnectionOptions.TabStop = false;
            this.gbConnectionOptions.Text = "Connection Options";
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(13, 330);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(75, 23);
            this.btnPaste.TabIndex = 16;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSecretKey.Location = new System.Drawing.Point(13, 301);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(274, 23);
            this.txtSecretKey.TabIndex = 15;
            this.txtSecretKey.UseSystemPasswordChar = true;
            this.txtSecretKey.TextChanged += new System.EventHandler(this.txtSecretKey_TextChanged);
            this.txtSecretKey.Enter += new System.EventHandler(this.txtSecretKey_Enter);
            this.txtSecretKey.Leave += new System.EventHandler(this.txtSecretKey_Leave);
            this.txtSecretKey.Validating += new System.ComponentModel.CancelEventHandler(this.txtSecretKey_Validating);
            // 
            // lblSecretKey
            // 
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Location = new System.Drawing.Point(10, 283);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(60, 15);
            this.lblSecretKey.TabIndex = 14;
            this.lblSecretKey.Text = "Secret key";
            // 
            // txtInstance
            // 
            this.txtInstance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstance.Location = new System.Drawing.Point(13, 257);
            this.txtInstance.Name = "txtInstance";
            this.txtInstance.Size = new System.Drawing.Size(274, 23);
            this.txtInstance.TabIndex = 13;
            this.txtInstance.TextChanged += new System.EventHandler(this.txtInstance_TextChanged);
            // 
            // lblInstance
            // 
            this.lblInstance.AutoSize = true;
            this.lblInstance.Location = new System.Drawing.Point(10, 239);
            this.lblInstance.Name = "lblInstance";
            this.lblInstance.Size = new System.Drawing.Size(51, 15);
            this.lblInstance.TabIndex = 12;
            this.lblInstance.Text = "Instance";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(13, 213);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(274, 23);
            this.txtPassword.TabIndex = 11;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 195);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(13, 169);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(274, 23);
            this.txtUsername.TabIndex = 9;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 151);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 8;
            this.lblUsername.Text = "Username";
            // 
            // numTimeout
            // 
            this.numTimeout.Location = new System.Drawing.Point(139, 125);
            this.numTimeout.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(120, 23);
            this.numTimeout.TabIndex = 7;
            this.numTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTimeout.ValueChanged += new System.EventHandler(this.numTimeout_ValueChanged);
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(136, 107);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(51, 15);
            this.lblTimeout.TabIndex = 6;
            this.lblTimeout.Text = "Timeout";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(13, 125);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(120, 23);
            this.numPort.TabIndex = 5;
            this.numPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.ValueChanged += new System.EventHandler(this.numPort_ValueChanged);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(10, 107);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 15);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port";
            // 
            // txtHost
            // 
            this.txtHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHost.Location = new System.Drawing.Point(13, 81);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(274, 23);
            this.txtHost.TabIndex = 3;
            this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(10, 63);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(32, 15);
            this.lblHost.TabIndex = 2;
            this.lblHost.Text = "Host";
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
            // CtrlClientConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbConnectionOptions);
            this.Name = "CtrlClientConnection";
            this.Size = new System.Drawing.Size(300, 366);
            this.gbConnectionOptions.ResumeLayout(false);
            this.gbConnectionOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnectionOptions;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtInstance;
        private System.Windows.Forms.Label lblInstance;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label lblSecretKey;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.Button btnPaste;
    }
}
