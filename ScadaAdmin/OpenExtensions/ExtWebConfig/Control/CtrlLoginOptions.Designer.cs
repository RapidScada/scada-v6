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
            this.gbLoginOptions = new System.Windows.Forms.GroupBox();
            this.txtAutoLoginPassword = new System.Windows.Forms.TextBox();
            this.txtAutoLoginUsername = new System.Windows.Forms.TextBox();
            this.lblAutoLoginPassword = new System.Windows.Forms.Label();
            this.lblAutoLoginUsername = new System.Windows.Forms.Label();
            this.lblRememberMeExpires = new System.Windows.Forms.Label();
            this.numRememberMeExpires = new System.Windows.Forms.NumericUpDown();
            this.chkAllowRememberMe = new System.Windows.Forms.CheckBox();
            this.lblAllowRememberMe = new System.Windows.Forms.Label();
            this.chkRequireCaptcha = new System.Windows.Forms.CheckBox();
            this.lblRequireCaptcha = new System.Windows.Forms.Label();
            this.gbLoginOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRememberMeExpires)).BeginInit();
            this.SuspendLayout();
            // 
            // gbLoginOptions
            // 
            this.gbLoginOptions.Controls.Add(this.txtAutoLoginPassword);
            this.gbLoginOptions.Controls.Add(this.txtAutoLoginUsername);
            this.gbLoginOptions.Controls.Add(this.lblAutoLoginPassword);
            this.gbLoginOptions.Controls.Add(this.lblAutoLoginUsername);
            this.gbLoginOptions.Controls.Add(this.lblRememberMeExpires);
            this.gbLoginOptions.Controls.Add(this.numRememberMeExpires);
            this.gbLoginOptions.Controls.Add(this.chkAllowRememberMe);
            this.gbLoginOptions.Controls.Add(this.lblAllowRememberMe);
            this.gbLoginOptions.Controls.Add(this.chkRequireCaptcha);
            this.gbLoginOptions.Controls.Add(this.lblRequireCaptcha);
            this.gbLoginOptions.Location = new System.Drawing.Point(0, 0);
            this.gbLoginOptions.Name = "gbLoginOptions";
            this.gbLoginOptions.Size = new System.Drawing.Size(500, 189);
            this.gbLoginOptions.TabIndex = 4;
            this.gbLoginOptions.TabStop = false;
            this.gbLoginOptions.Text = "Login Options";
            // 
            // txtAutoLoginPassword
            // 
            this.txtAutoLoginPassword.Location = new System.Drawing.Point(258, 141);
            this.txtAutoLoginPassword.Name = "txtAutoLoginPassword";
            this.txtAutoLoginPassword.Size = new System.Drawing.Size(227, 23);
            this.txtAutoLoginPassword.TabIndex = 22;
            // 
            // txtAutoLoginUsername
            // 
            this.txtAutoLoginUsername.Location = new System.Drawing.Point(258, 107);
            this.txtAutoLoginUsername.Name = "txtAutoLoginUsername";
            this.txtAutoLoginUsername.Size = new System.Drawing.Size(227, 23);
            this.txtAutoLoginUsername.TabIndex = 21;
            // 
            // lblAutoLoginPassword
            // 
            this.lblAutoLoginPassword.AutoSize = true;
            this.lblAutoLoginPassword.Location = new System.Drawing.Point(13, 144);
            this.lblAutoLoginPassword.Name = "lblAutoLoginPassword";
            this.lblAutoLoginPassword.Size = new System.Drawing.Size(162, 15);
            this.lblAutoLoginPassword.TabIndex = 20;
            this.lblAutoLoginPassword.Text = "Password for automatic login";
            // 
            // lblAutoLoginUsername
            // 
            this.lblAutoLoginUsername.AutoSize = true;
            this.lblAutoLoginUsername.Location = new System.Drawing.Point(10, 107);
            this.lblAutoLoginUsername.Name = "lblAutoLoginUsername";
            this.lblAutoLoginUsername.Size = new System.Drawing.Size(165, 15);
            this.lblAutoLoginUsername.TabIndex = 19;
            this.lblAutoLoginUsername.Text = "Username for automatic login";
            // 
            // lblRememberMeExpires
            // 
            this.lblRememberMeExpires.AutoSize = true;
            this.lblRememberMeExpires.Location = new System.Drawing.Point(10, 76);
            this.lblRememberMeExpires.Name = "lblRememberMeExpires";
            this.lblRememberMeExpires.Size = new System.Drawing.Size(138, 15);
            this.lblRememberMeExpires.TabIndex = 18;
            this.lblRememberMeExpires.Text = "User\'s login expires, days";
            // 
            // numRememberMeExpires
            // 
            this.numRememberMeExpires.Location = new System.Drawing.Point(385, 74);
            this.numRememberMeExpires.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numRememberMeExpires.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRememberMeExpires.Name = "numRememberMeExpires";
            this.numRememberMeExpires.Size = new System.Drawing.Size(100, 23);
            this.numRememberMeExpires.TabIndex = 17;
            this.numRememberMeExpires.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chkAllowRememberMe
            // 
            this.chkAllowRememberMe.AutoSize = true;
            this.chkAllowRememberMe.Location = new System.Drawing.Point(470, 50);
            this.chkAllowRememberMe.Name = "chkAllowRememberMe";
            this.chkAllowRememberMe.Size = new System.Drawing.Size(15, 14);
            this.chkAllowRememberMe.TabIndex = 11;
            this.chkAllowRememberMe.UseVisualStyleBackColor = true;
            // 
            // lblAllowRememberMe
            // 
            this.lblAllowRememberMe.AutoSize = true;
            this.lblAllowRememberMe.Location = new System.Drawing.Point(10, 50);
            this.lblAllowRememberMe.Name = "lblAllowRememberMe";
            this.lblAllowRememberMe.Size = new System.Drawing.Size(187, 15);
            this.lblAllowRememberMe.TabIndex = 10;
            this.lblAllowRememberMe.Text = "User is allowed to remember login";
            // 
            // chkRequireCaptcha
            // 
            this.chkRequireCaptcha.AutoSize = true;
            this.chkRequireCaptcha.Location = new System.Drawing.Point(470, 22);
            this.chkRequireCaptcha.Name = "chkRequireCaptcha";
            this.chkRequireCaptcha.Size = new System.Drawing.Size(15, 14);
            this.chkRequireCaptcha.TabIndex = 9;
            this.chkRequireCaptcha.UseVisualStyleBackColor = true;
            // 
            // lblRequireCaptcha
            // 
            this.lblRequireCaptcha.AutoSize = true;
            this.lblRequireCaptcha.Location = new System.Drawing.Point(10, 26);
            this.lblRequireCaptcha.Name = "lblRequireCaptcha";
            this.lblRequireCaptcha.Size = new System.Drawing.Size(144, 15);
            this.lblRequireCaptcha.TabIndex = 8;
            this.lblRequireCaptcha.Text = "Require a captcha at login";
            // 
            // CtrlLoginOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbLoginOptions);
            this.Name = "CtrlLoginOptions";
            this.Size = new System.Drawing.Size(550, 550);
            this.gbLoginOptions.ResumeLayout(false);
            this.gbLoginOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRememberMeExpires)).EndInit();
            this.ResumeLayout(false);

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
    }
}
