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
            this.lblAutoLoginPassword = new System.Windows.Forms.Label();
            this.txtAutoLoginUsername = new System.Windows.Forms.TextBox();
            this.lblAutoLoginUsername = new System.Windows.Forms.Label();
            this.numRememberMeExpires = new System.Windows.Forms.NumericUpDown();
            this.lblRememberMeExpires = new System.Windows.Forms.Label();
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
            this.gbLoginOptions.Controls.Add(this.lblAutoLoginPassword);
            this.gbLoginOptions.Controls.Add(this.txtAutoLoginUsername);
            this.gbLoginOptions.Controls.Add(this.lblAutoLoginUsername);
            this.gbLoginOptions.Controls.Add(this.numRememberMeExpires);
            this.gbLoginOptions.Controls.Add(this.lblRememberMeExpires);
            this.gbLoginOptions.Controls.Add(this.chkAllowRememberMe);
            this.gbLoginOptions.Controls.Add(this.lblAllowRememberMe);
            this.gbLoginOptions.Controls.Add(this.chkRequireCaptcha);
            this.gbLoginOptions.Controls.Add(this.lblRequireCaptcha);
            this.gbLoginOptions.Location = new System.Drawing.Point(0, 0);
            this.gbLoginOptions.Margin = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLoginOptions.Name = "gbLoginOptions";
            this.gbLoginOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLoginOptions.Size = new System.Drawing.Size(500, 180);
            this.gbLoginOptions.TabIndex = 0;
            this.gbLoginOptions.TabStop = false;
            this.gbLoginOptions.Text = "Login Options";
            // 
            // txtAutoLoginPassword
            // 
            this.txtAutoLoginPassword.Location = new System.Drawing.Point(287, 144);
            this.txtAutoLoginPassword.Name = "txtAutoLoginPassword";
            this.txtAutoLoginPassword.Size = new System.Drawing.Size(200, 23);
            this.txtAutoLoginPassword.TabIndex = 9;
            this.txtAutoLoginPassword.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblAutoLoginPassword
            // 
            this.lblAutoLoginPassword.AutoSize = true;
            this.lblAutoLoginPassword.Location = new System.Drawing.Point(10, 148);
            this.lblAutoLoginPassword.Name = "lblAutoLoginPassword";
            this.lblAutoLoginPassword.Size = new System.Drawing.Size(162, 15);
            this.lblAutoLoginPassword.TabIndex = 9;
            this.lblAutoLoginPassword.Text = "Password for automatic login";
            // 
            // txtAutoLoginUsername
            // 
            this.txtAutoLoginUsername.Location = new System.Drawing.Point(287, 115);
            this.txtAutoLoginUsername.Name = "txtAutoLoginUsername";
            this.txtAutoLoginUsername.Size = new System.Drawing.Size(200, 23);
            this.txtAutoLoginUsername.TabIndex = 8;
            this.txtAutoLoginUsername.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblAutoLoginUsername
            // 
            this.lblAutoLoginUsername.AutoSize = true;
            this.lblAutoLoginUsername.Location = new System.Drawing.Point(10, 119);
            this.lblAutoLoginUsername.Name = "lblAutoLoginUsername";
            this.lblAutoLoginUsername.Size = new System.Drawing.Size(165, 15);
            this.lblAutoLoginUsername.TabIndex = 7;
            this.lblAutoLoginUsername.Text = "Username for automatic login";
            // 
            // numRememberMeExpires
            // 
            this.numRememberMeExpires.Location = new System.Drawing.Point(387, 82);
            this.numRememberMeExpires.Maximum = new decimal(new int[] {
            365,
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
            this.numRememberMeExpires.TabIndex = 6;
            this.numRememberMeExpires.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numRememberMeExpires.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblRememberMeExpires
            // 
            this.lblRememberMeExpires.AutoSize = true;
            this.lblRememberMeExpires.Location = new System.Drawing.Point(10, 86);
            this.lblRememberMeExpires.Name = "lblRememberMeExpires";
            this.lblRememberMeExpires.Size = new System.Drawing.Size(107, 15);
            this.lblRememberMeExpires.TabIndex = 5;
            this.lblRememberMeExpires.Text = "Login expires, days";
            // 
            // chkAllowRememberMe
            // 
            this.chkAllowRememberMe.AutoSize = true;
            this.chkAllowRememberMe.Location = new System.Drawing.Point(472, 55);
            this.chkAllowRememberMe.Name = "chkAllowRememberMe";
            this.chkAllowRememberMe.Size = new System.Drawing.Size(15, 14);
            this.chkAllowRememberMe.TabIndex = 4;
            this.chkAllowRememberMe.UseVisualStyleBackColor = true;
            this.chkAllowRememberMe.CheckedChanged += new System.EventHandler(this.chkAllowRememberMe_CheckedChanged);
            // 
            // lblAllowRememberMe
            // 
            this.lblAllowRememberMe.AutoSize = true;
            this.lblAllowRememberMe.Location = new System.Drawing.Point(10, 55);
            this.lblAllowRememberMe.Name = "lblAllowRememberMe";
            this.lblAllowRememberMe.Size = new System.Drawing.Size(139, 15);
            this.lblAllowRememberMe.TabIndex = 3;
            this.lblAllowRememberMe.Text = "Allow to remember login";
            // 
            // chkRequireCaptcha
            // 
            this.chkRequireCaptcha.AutoSize = true;
            this.chkRequireCaptcha.Location = new System.Drawing.Point(472, 26);
            this.chkRequireCaptcha.Name = "chkRequireCaptcha";
            this.chkRequireCaptcha.Size = new System.Drawing.Size(15, 14);
            this.chkRequireCaptcha.TabIndex = 2;
            this.chkRequireCaptcha.UseVisualStyleBackColor = true;
            this.chkRequireCaptcha.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblRequireCaptcha
            // 
            this.lblRequireCaptcha.AutoSize = true;
            this.lblRequireCaptcha.Location = new System.Drawing.Point(10, 26);
            this.lblRequireCaptcha.Name = "lblRequireCaptcha";
            this.lblRequireCaptcha.Size = new System.Drawing.Size(135, 15);
            this.lblRequireCaptcha.TabIndex = 1;
            this.lblRequireCaptcha.Text = "Require captcha at login";
            // 
            // CtrlLoginOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbLoginOptions);
            this.Name = "CtrlLoginOptions";
            this.Size = new System.Drawing.Size(550, 200);
            this.Load += new System.EventHandler(this.CtrlLoginOptions_Load);
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
