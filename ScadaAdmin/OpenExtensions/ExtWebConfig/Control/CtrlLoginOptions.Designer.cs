namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    partial class CtrlLoginOptions
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
            gbLoginOptions = new GroupBox();
            numClientIpOrder = new NumericUpDown();
            lblClientIpOrder = new Label();
            txtAutoLoginPassword = new TextBox();
            lblAutoLoginPassword = new Label();
            txtAutoLoginUsername = new TextBox();
            lblAutoLoginUsername = new Label();
            numRememberMeExpires = new NumericUpDown();
            lblRememberMeExpires = new Label();
            chkAllowRememberMe = new CheckBox();
            lblAllowRememberMe = new Label();
            chkRequireCaptcha = new CheckBox();
            lblRequireCaptcha = new Label();
            gbGoogleSSO = new GroupBox();
            txtGoogleClientSecret = new TextBox();
            lblGoogleClientSecret = new Label();
            txtGoogleClientId = new TextBox();
            lblGoogleClientId = new Label();
            txtGoogleLoginUri = new TextBox();
            lblGoogleLoginUri = new Label();
            gbLoginOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numClientIpOrder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRememberMeExpires).BeginInit();
            gbGoogleSSO.SuspendLayout();
            SuspendLayout();
            // 
            // gbLoginOptions
            // 
            gbLoginOptions.Controls.Add(numClientIpOrder);
            gbLoginOptions.Controls.Add(lblClientIpOrder);
            gbLoginOptions.Controls.Add(txtAutoLoginPassword);
            gbLoginOptions.Controls.Add(lblAutoLoginPassword);
            gbLoginOptions.Controls.Add(txtAutoLoginUsername);
            gbLoginOptions.Controls.Add(lblAutoLoginUsername);
            gbLoginOptions.Controls.Add(numRememberMeExpires);
            gbLoginOptions.Controls.Add(lblRememberMeExpires);
            gbLoginOptions.Controls.Add(chkAllowRememberMe);
            gbLoginOptions.Controls.Add(lblAllowRememberMe);
            gbLoginOptions.Controls.Add(chkRequireCaptcha);
            gbLoginOptions.Controls.Add(lblRequireCaptcha);
            gbLoginOptions.Location = new Point(0, 0);
            gbLoginOptions.Margin = new Padding(10, 3, 10, 11);
            gbLoginOptions.Name = "gbLoginOptions";
            gbLoginOptions.Padding = new Padding(10, 3, 10, 11);
            gbLoginOptions.Size = new Size(500, 231);
            gbLoginOptions.TabIndex = 0;
            gbLoginOptions.TabStop = false;
            gbLoginOptions.Text = "Login Options";
            // 
            // numClientIpOrder
            // 
            numClientIpOrder.Location = new Point(387, 196);
            numClientIpOrder.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            numClientIpOrder.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numClientIpOrder.Name = "numClientIpOrder";
            numClientIpOrder.Size = new Size(100, 23);
            numClientIpOrder.TabIndex = 11;
            numClientIpOrder.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numClientIpOrder.ValueChanged += control_Changed;
            // 
            // lblClientIpOrder
            // 
            lblClientIpOrder.AutoSize = true;
            lblClientIpOrder.Location = new Point(10, 200);
            lblClientIpOrder.Name = "lblClientIpOrder";
            lblClientIpOrder.Size = new Size(198, 17);
            lblClientIpOrder.TabIndex = 10;
            lblClientIpOrder.Text = "Client Ip order(from right to left)";
            // 
            // txtAutoLoginPassword
            // 
            txtAutoLoginPassword.Location = new Point(287, 163);
            txtAutoLoginPassword.Name = "txtAutoLoginPassword";
            txtAutoLoginPassword.Size = new Size(200, 23);
            txtAutoLoginPassword.TabIndex = 9;
            txtAutoLoginPassword.UseSystemPasswordChar = true;
            txtAutoLoginPassword.TextChanged += control_Changed;
            // 
            // lblAutoLoginPassword
            // 
            lblAutoLoginPassword.AutoSize = true;
            lblAutoLoginPassword.Location = new Point(10, 168);
            lblAutoLoginPassword.Name = "lblAutoLoginPassword";
            lblAutoLoginPassword.Size = new Size(179, 17);
            lblAutoLoginPassword.TabIndex = 9;
            lblAutoLoginPassword.Text = "Password for automatic login";
            // 
            // txtAutoLoginUsername
            // 
            txtAutoLoginUsername.Location = new Point(287, 130);
            txtAutoLoginUsername.Name = "txtAutoLoginUsername";
            txtAutoLoginUsername.Size = new Size(200, 23);
            txtAutoLoginUsername.TabIndex = 8;
            txtAutoLoginUsername.TextChanged += control_Changed;
            // 
            // lblAutoLoginUsername
            // 
            lblAutoLoginUsername.AutoSize = true;
            lblAutoLoginUsername.Location = new Point(10, 135);
            lblAutoLoginUsername.Name = "lblAutoLoginUsername";
            lblAutoLoginUsername.Size = new Size(182, 17);
            lblAutoLoginUsername.TabIndex = 7;
            lblAutoLoginUsername.Text = "Username for automatic login";
            // 
            // numRememberMeExpires
            // 
            numRememberMeExpires.Location = new Point(387, 93);
            numRememberMeExpires.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            numRememberMeExpires.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRememberMeExpires.Name = "numRememberMeExpires";
            numRememberMeExpires.Size = new Size(100, 23);
            numRememberMeExpires.TabIndex = 6;
            numRememberMeExpires.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numRememberMeExpires.ValueChanged += control_Changed;
            // 
            // lblRememberMeExpires
            // 
            lblRememberMeExpires.AutoSize = true;
            lblRememberMeExpires.Location = new Point(10, 97);
            lblRememberMeExpires.Name = "lblRememberMeExpires";
            lblRememberMeExpires.Size = new Size(120, 17);
            lblRememberMeExpires.TabIndex = 5;
            lblRememberMeExpires.Text = "Login expires, days";
            // 
            // chkAllowRememberMe
            // 
            chkAllowRememberMe.AutoSize = true;
            chkAllowRememberMe.Location = new Point(472, 62);
            chkAllowRememberMe.Name = "chkAllowRememberMe";
            chkAllowRememberMe.Size = new Size(15, 14);
            chkAllowRememberMe.TabIndex = 4;
            chkAllowRememberMe.UseVisualStyleBackColor = true;
            chkAllowRememberMe.CheckedChanged += chkAllowRememberMe_CheckedChanged;
            // 
            // lblAllowRememberMe
            // 
            lblAllowRememberMe.AutoSize = true;
            lblAllowRememberMe.Location = new Point(10, 62);
            lblAllowRememberMe.Name = "lblAllowRememberMe";
            lblAllowRememberMe.Size = new Size(153, 17);
            lblAllowRememberMe.TabIndex = 3;
            lblAllowRememberMe.Text = "Allow to remember login";
            // 
            // chkRequireCaptcha
            // 
            chkRequireCaptcha.AutoSize = true;
            chkRequireCaptcha.Location = new Point(472, 29);
            chkRequireCaptcha.Name = "chkRequireCaptcha";
            chkRequireCaptcha.Size = new Size(15, 14);
            chkRequireCaptcha.TabIndex = 2;
            chkRequireCaptcha.UseVisualStyleBackColor = true;
            chkRequireCaptcha.CheckedChanged += control_Changed;
            // 
            // lblRequireCaptcha
            // 
            lblRequireCaptcha.AutoSize = true;
            lblRequireCaptcha.Location = new Point(10, 29);
            lblRequireCaptcha.Name = "lblRequireCaptcha";
            lblRequireCaptcha.Size = new Size(150, 17);
            lblRequireCaptcha.TabIndex = 1;
            lblRequireCaptcha.Text = "Require captcha at login";
            // 
            // gbGoogleSSO
            // 
            gbGoogleSSO.Controls.Add(txtGoogleClientSecret);
            gbGoogleSSO.Controls.Add(lblGoogleClientSecret);
            gbGoogleSSO.Controls.Add(txtGoogleClientId);
            gbGoogleSSO.Controls.Add(lblGoogleClientId);
            gbGoogleSSO.Controls.Add(txtGoogleLoginUri);
            gbGoogleSSO.Controls.Add(lblGoogleLoginUri);
            gbGoogleSSO.Location = new Point(0, 245);
            gbGoogleSSO.Margin = new Padding(10, 3, 10, 11);
            gbGoogleSSO.Name = "gbGoogleSSO";
            gbGoogleSSO.Padding = new Padding(10, 3, 10, 11);
            gbGoogleSSO.Size = new Size(500, 124);
            gbGoogleSSO.TabIndex = 10;
            gbGoogleSSO.TabStop = false;
            gbGoogleSSO.Text = "GoogleSSO";
            // 
            // txtGoogleClientSecret
            // 
            txtGoogleClientSecret.Location = new Point(283, 88);
            txtGoogleClientSecret.Name = "txtGoogleClientSecret";
            txtGoogleClientSecret.Size = new Size(200, 23);
            txtGoogleClientSecret.TabIndex = 10;
            txtGoogleClientSecret.UseSystemPasswordChar = true;
            txtGoogleClientSecret.TextChanged += control_Changed;
            // 
            // lblGoogleClientSecret
            // 
            lblGoogleClientSecret.AutoSize = true;
            lblGoogleClientSecret.Location = new Point(13, 93);
            lblGoogleClientSecret.Name = "lblGoogleClientSecret";
            lblGoogleClientSecret.Size = new Size(124, 17);
            lblGoogleClientSecret.TabIndex = 11;
            lblGoogleClientSecret.Text = "Google client secret";
            // 
            // txtGoogleClientId
            // 
            txtGoogleClientId.Location = new Point(283, 57);
            txtGoogleClientId.Name = "txtGoogleClientId";
            txtGoogleClientId.Size = new Size(200, 23);
            txtGoogleClientId.TabIndex = 9;
            txtGoogleClientId.UseSystemPasswordChar = true;
            txtGoogleClientId.TextChanged += control_Changed;
            // 
            // lblGoogleClientId
            // 
            lblGoogleClientId.AutoSize = true;
            lblGoogleClientId.Location = new Point(13, 62);
            lblGoogleClientId.Name = "lblGoogleClientId";
            lblGoogleClientId.Size = new Size(100, 17);
            lblGoogleClientId.TabIndex = 9;
            lblGoogleClientId.Text = "Google client id";
            // 
            // txtGoogleLoginUri
            // 
            txtGoogleLoginUri.Location = new Point(283, 24);
            txtGoogleLoginUri.Name = "txtGoogleLoginUri";
            txtGoogleLoginUri.Size = new Size(200, 23);
            txtGoogleLoginUri.TabIndex = 8;
            txtGoogleLoginUri.TextChanged += control_Changed;
            // 
            // lblGoogleLoginUri
            // 
            lblGoogleLoginUri.AutoSize = true;
            lblGoogleLoginUri.Location = new Point(13, 29);
            lblGoogleLoginUri.Name = "lblGoogleLoginUri";
            lblGoogleLoginUri.Size = new Size(59, 17);
            lblGoogleLoginUri.TabIndex = 7;
            lblGoogleLoginUri.Text = "Login uri";
            // 
            // CtrlLoginOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbGoogleSSO);
            Controls.Add(gbLoginOptions);
            Name = "CtrlLoginOptions";
            Size = new Size(521, 393);
            Load += CtrlLoginOptions_Load;
            gbLoginOptions.ResumeLayout(false);
            gbLoginOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numClientIpOrder).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRememberMeExpires).EndInit();
            gbGoogleSSO.ResumeLayout(false);
            gbGoogleSSO.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbLoginOptions;
        private TextBox txtAutoLoginPassword;
        private TextBox txtAutoLoginUsername;
        private Label lblAutoLoginPassword;
        private Label lblAutoLoginUsername;
        private Label lblRememberMeExpires;
        private NumericUpDown numRememberMeExpires;
        private CheckBox chkAllowRememberMe;
        private Label lblAllowRememberMe;
        private CheckBox chkRequireCaptcha;
        private Label lblRequireCaptcha;
        private GroupBox gbGoogleSSO;
        private TextBox txtGoogleClientSecret;
        private Label lblGoogleClientSecret;
        private TextBox txtGoogleClientId;
        private Label lblGoogleClientId;
        private TextBox txtGoogleLoginUri;
        private Label lblGoogleLoginUri;
        private NumericUpDown numClientIpOrder;
        private Label lblClientIpOrder;
    }
}
