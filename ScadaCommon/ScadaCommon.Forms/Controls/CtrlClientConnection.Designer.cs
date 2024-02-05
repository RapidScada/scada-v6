
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
            gbConnectionOptions = new GroupBox();
            pnlConnectionOptions = new Panel();
            btnPaste = new Button();
            lblName = new Label();
            txtSecretKey = new TextBox();
            txtName = new TextBox();
            lblSecretKey = new Label();
            txtHost = new TextBox();
            lblHost = new Label();
            txtInstance = new TextBox();
            lblPort = new Label();
            lblInstance = new Label();
            numPort = new NumericUpDown();
            txtPassword = new TextBox();
            lblTimeout = new Label();
            lblPassword = new Label();
            numTimeout = new NumericUpDown();
            txtUsername = new TextBox();
            lblUsername = new Label();
            gbConnectionOptions.SuspendLayout();
            pnlConnectionOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTimeout).BeginInit();
            SuspendLayout();
            // 
            // gbConnectionOptions
            // 
            gbConnectionOptions.Controls.Add(pnlConnectionOptions);
            gbConnectionOptions.Dock = DockStyle.Fill;
            gbConnectionOptions.Location = new Point(0, 0);
            gbConnectionOptions.Name = "gbConnectionOptions";
            gbConnectionOptions.Padding = new Padding(10, 3, 10, 10);
            gbConnectionOptions.Size = new Size(300, 366);
            gbConnectionOptions.TabIndex = 0;
            gbConnectionOptions.TabStop = false;
            gbConnectionOptions.Text = "Connection Options";
            // 
            // pnlConnectionOptions
            // 
            pnlConnectionOptions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlConnectionOptions.Controls.Add(btnPaste);
            pnlConnectionOptions.Controls.Add(lblName);
            pnlConnectionOptions.Controls.Add(txtSecretKey);
            pnlConnectionOptions.Controls.Add(txtName);
            pnlConnectionOptions.Controls.Add(lblSecretKey);
            pnlConnectionOptions.Controls.Add(txtHost);
            pnlConnectionOptions.Controls.Add(lblHost);
            pnlConnectionOptions.Controls.Add(txtInstance);
            pnlConnectionOptions.Controls.Add(lblPort);
            pnlConnectionOptions.Controls.Add(lblInstance);
            pnlConnectionOptions.Controls.Add(numPort);
            pnlConnectionOptions.Controls.Add(txtPassword);
            pnlConnectionOptions.Controls.Add(lblTimeout);
            pnlConnectionOptions.Controls.Add(lblPassword);
            pnlConnectionOptions.Controls.Add(numTimeout);
            pnlConnectionOptions.Controls.Add(txtUsername);
            pnlConnectionOptions.Controls.Add(lblUsername);
            pnlConnectionOptions.Location = new Point(13, 22);
            pnlConnectionOptions.Name = "pnlConnectionOptions";
            pnlConnectionOptions.Size = new Size(274, 331);
            pnlConnectionOptions.TabIndex = 1;
            // 
            // btnPaste
            // 
            btnPaste.Location = new Point(0, 308);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(75, 23);
            btnPaste.TabIndex = 16;
            btnPaste.Text = "Paste";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
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
            // txtSecretKey
            // 
            txtSecretKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSecretKey.Location = new Point(0, 279);
            txtSecretKey.Name = "txtSecretKey";
            txtSecretKey.Size = new Size(274, 23);
            txtSecretKey.TabIndex = 15;
            txtSecretKey.UseSystemPasswordChar = true;
            txtSecretKey.TextChanged += txtSecretKey_TextChanged;
            txtSecretKey.Enter += txtSecretKey_Enter;
            txtSecretKey.Leave += txtSecretKey_Leave;
            txtSecretKey.Validating += txtSecretKey_Validating;
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
            // lblSecretKey
            // 
            lblSecretKey.AutoSize = true;
            lblSecretKey.Location = new Point(-3, 261);
            lblSecretKey.Name = "lblSecretKey";
            lblSecretKey.Size = new Size(60, 15);
            lblSecretKey.TabIndex = 14;
            lblSecretKey.Text = "Secret key";
            // 
            // txtHost
            // 
            txtHost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtHost.Location = new Point(0, 59);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(274, 23);
            txtHost.TabIndex = 3;
            txtHost.TextChanged += txtHost_TextChanged;
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new Point(-3, 41);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(32, 15);
            lblHost.TabIndex = 2;
            lblHost.Text = "Host";
            // 
            // txtInstance
            // 
            txtInstance.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtInstance.Location = new Point(0, 235);
            txtInstance.Name = "txtInstance";
            txtInstance.Size = new Size(274, 23);
            txtInstance.TabIndex = 13;
            txtInstance.TextChanged += txtInstance_TextChanged;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(-3, 85);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(29, 15);
            lblPort.TabIndex = 4;
            lblPort.Text = "Port";
            // 
            // lblInstance
            // 
            lblInstance.AutoSize = true;
            lblInstance.Location = new Point(-3, 217);
            lblInstance.Name = "lblInstance";
            lblInstance.Size = new Size(51, 15);
            lblInstance.TabIndex = 12;
            lblInstance.Text = "Instance";
            // 
            // numPort
            // 
            numPort.Location = new Point(0, 103);
            numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new Size(120, 23);
            numPort.TabIndex = 5;
            numPort.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.ValueChanged += numPort_ValueChanged;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPassword.Location = new Point(0, 191);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(274, 23);
            txtPassword.TabIndex = 11;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // lblTimeout
            // 
            lblTimeout.AutoSize = true;
            lblTimeout.Location = new Point(123, 85);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new Size(51, 15);
            lblTimeout.TabIndex = 6;
            lblTimeout.Text = "Timeout";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(-3, 173);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 10;
            lblPassword.Text = "Password";
            // 
            // numTimeout
            // 
            numTimeout.Location = new Point(126, 103);
            numTimeout.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numTimeout.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTimeout.Name = "numTimeout";
            numTimeout.Size = new Size(120, 23);
            numTimeout.TabIndex = 7;
            numTimeout.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            numTimeout.ValueChanged += numTimeout_ValueChanged;
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUsername.Location = new Point(0, 147);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(274, 23);
            txtUsername.TabIndex = 9;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(-3, 129);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(60, 15);
            lblUsername.TabIndex = 8;
            lblUsername.Text = "Username";
            // 
            // CtrlClientConnection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbConnectionOptions);
            Name = "CtrlClientConnection";
            Size = new Size(300, 366);
            gbConnectionOptions.ResumeLayout(false);
            pnlConnectionOptions.ResumeLayout(false);
            pnlConnectionOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTimeout).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbConnectionOptions;
        private TextBox txtName;
        private Label lblName;
        private TextBox txtHost;
        private Label lblHost;
        private NumericUpDown numPort;
        private Label lblPort;
        private TextBox txtPassword;
        private Label lblPassword;
        private TextBox txtUsername;
        private Label lblUsername;
        private TextBox txtInstance;
        private Label lblInstance;
        private Label lblTimeout;
        private NumericUpDown numTimeout;
        private Label lblSecretKey;
        private TextBox txtSecretKey;
        private Button btnPaste;
        private Panel pnlConnectionOptions;
    }
}
