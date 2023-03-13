
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
            gbConnectionOptions = new System.Windows.Forms.GroupBox();
            btnPaste = new System.Windows.Forms.Button();
            txtSecretKey = new System.Windows.Forms.TextBox();
            lblSecretKey = new System.Windows.Forms.Label();
            txtInstance = new System.Windows.Forms.TextBox();
            lblInstance = new System.Windows.Forms.Label();
            txtPassword = new System.Windows.Forms.TextBox();
            lblPassword = new System.Windows.Forms.Label();
            txtUsername = new System.Windows.Forms.TextBox();
            lblUsername = new System.Windows.Forms.Label();
            numTimeout = new System.Windows.Forms.NumericUpDown();
            lblTimeout = new System.Windows.Forms.Label();
            numPort = new System.Windows.Forms.NumericUpDown();
            lblPort = new System.Windows.Forms.Label();
            txtHost = new System.Windows.Forms.TextBox();
            lblHost = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            lblName = new System.Windows.Forms.Label();
            gbConnectionOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            SuspendLayout();
            // 
            // gbConnectionOptions
            // 
            gbConnectionOptions.Controls.Add(btnPaste);
            gbConnectionOptions.Controls.Add(txtSecretKey);
            gbConnectionOptions.Controls.Add(lblSecretKey);
            gbConnectionOptions.Controls.Add(txtInstance);
            gbConnectionOptions.Controls.Add(lblInstance);
            gbConnectionOptions.Controls.Add(txtPassword);
            gbConnectionOptions.Controls.Add(lblPassword);
            gbConnectionOptions.Controls.Add(txtUsername);
            gbConnectionOptions.Controls.Add(lblUsername);
            gbConnectionOptions.Controls.Add(numTimeout);
            gbConnectionOptions.Controls.Add(lblTimeout);
            gbConnectionOptions.Controls.Add(numPort);
            gbConnectionOptions.Controls.Add(lblPort);
            gbConnectionOptions.Controls.Add(txtHost);
            gbConnectionOptions.Controls.Add(lblHost);
            gbConnectionOptions.Controls.Add(txtName);
            gbConnectionOptions.Controls.Add(lblName);
            gbConnectionOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            gbConnectionOptions.Location = new System.Drawing.Point(0, 0);
            gbConnectionOptions.Name = "gbConnectionOptions";
            gbConnectionOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            gbConnectionOptions.Size = new System.Drawing.Size(300, 366);
            gbConnectionOptions.TabIndex = 0;
            gbConnectionOptions.TabStop = false;
            gbConnectionOptions.Text = "Connection Options";
            // 
            // btnPaste
            // 
            btnPaste.Location = new System.Drawing.Point(13, 330);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(75, 23);
            btnPaste.TabIndex = 16;
            btnPaste.Text = "Paste";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
            // 
            // txtSecretKey
            // 
            txtSecretKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSecretKey.Location = new System.Drawing.Point(13, 301);
            txtSecretKey.Name = "txtSecretKey";
            txtSecretKey.Size = new System.Drawing.Size(274, 23);
            txtSecretKey.TabIndex = 15;
            txtSecretKey.UseSystemPasswordChar = true;
            txtSecretKey.TextChanged += txtSecretKey_TextChanged;
            txtSecretKey.Enter += txtSecretKey_Enter;
            txtSecretKey.Leave += txtSecretKey_Leave;
            txtSecretKey.Validating += txtSecretKey_Validating;
            // 
            // lblSecretKey
            // 
            lblSecretKey.AutoSize = true;
            lblSecretKey.Location = new System.Drawing.Point(10, 283);
            lblSecretKey.Name = "lblSecretKey";
            lblSecretKey.Size = new System.Drawing.Size(60, 15);
            lblSecretKey.TabIndex = 14;
            lblSecretKey.Text = "Secret key";
            // 
            // txtInstance
            // 
            txtInstance.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtInstance.Location = new System.Drawing.Point(13, 257);
            txtInstance.Name = "txtInstance";
            txtInstance.Size = new System.Drawing.Size(274, 23);
            txtInstance.TabIndex = 13;
            txtInstance.TextChanged += txtInstance_TextChanged;
            // 
            // lblInstance
            // 
            lblInstance.AutoSize = true;
            lblInstance.Location = new System.Drawing.Point(10, 239);
            lblInstance.Name = "lblInstance";
            lblInstance.Size = new System.Drawing.Size(51, 15);
            lblInstance.TabIndex = 12;
            lblInstance.Text = "Instance";
            // 
            // txtPassword
            // 
            txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPassword.Location = new System.Drawing.Point(13, 213);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(274, 23);
            txtPassword.TabIndex = 11;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(10, 195);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(57, 15);
            lblPassword.TabIndex = 10;
            lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            txtUsername.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtUsername.Location = new System.Drawing.Point(13, 169);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(274, 23);
            txtUsername.TabIndex = 9;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new System.Drawing.Point(10, 151);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(60, 15);
            lblUsername.TabIndex = 8;
            lblUsername.Text = "Username";
            // 
            // numTimeout
            // 
            numTimeout.Location = new System.Drawing.Point(139, 125);
            numTimeout.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numTimeout.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTimeout.Name = "numTimeout";
            numTimeout.Size = new System.Drawing.Size(120, 23);
            numTimeout.TabIndex = 7;
            numTimeout.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            numTimeout.ValueChanged += numTimeout_ValueChanged;
            // 
            // lblTimeout
            // 
            lblTimeout.AutoSize = true;
            lblTimeout.Location = new System.Drawing.Point(136, 107);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new System.Drawing.Size(51, 15);
            lblTimeout.TabIndex = 6;
            lblTimeout.Text = "Timeout";
            // 
            // numPort
            // 
            numPort.Location = new System.Drawing.Point(13, 125);
            numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new System.Drawing.Size(120, 23);
            numPort.TabIndex = 5;
            numPort.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.ValueChanged += numPort_ValueChanged;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new System.Drawing.Point(10, 107);
            lblPort.Name = "lblPort";
            lblPort.Size = new System.Drawing.Size(29, 15);
            lblPort.TabIndex = 4;
            lblPort.Text = "Port";
            // 
            // txtHost
            // 
            txtHost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtHost.Location = new System.Drawing.Point(13, 81);
            txtHost.Name = "txtHost";
            txtHost.Size = new System.Drawing.Size(274, 23);
            txtHost.TabIndex = 3;
            txtHost.TextChanged += txtHost_TextChanged;
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new System.Drawing.Point(10, 63);
            lblHost.Name = "lblHost";
            lblHost.Size = new System.Drawing.Size(32, 15);
            lblHost.TabIndex = 2;
            lblHost.Text = "Host";
            // 
            // txtName
            // 
            txtName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtName.Location = new System.Drawing.Point(13, 37);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(274, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += txtName_TextChanged;
            txtName.Validated += txtName_Validated;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(10, 19);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(39, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Name";
            // 
            // CtrlClientConnection
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gbConnectionOptions);
            Name = "CtrlClientConnection";
            Size = new System.Drawing.Size(300, 366);
            gbConnectionOptions.ResumeLayout(false);
            gbConnectionOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            ResumeLayout(false);
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
