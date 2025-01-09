
using ModifySystemAccount;

namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmPassword
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
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnShowPassword = new System.Windows.Forms.Button();
            this.btnHidePassword = new System.Windows.Forms.Button();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblUserID = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.pnlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 53);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(84, 15);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "New password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 71);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(279, 23);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // btnShowPassword
            // 
            this.btnShowPassword.Location = new System.Drawing.Point(297, 71);
            this.btnShowPassword.Name = "btnShowPassword";
            this.btnShowPassword.Size = new System.Drawing.Size(75, 23);
            this.btnShowPassword.TabIndex = 4;
            this.btnShowPassword.Text = "Show";
            this.btnShowPassword.UseVisualStyleBackColor = true;
            this.btnShowPassword.Click += new System.EventHandler(this.btnShowHidePassword_Click);
            // 
            // btnHidePassword
            // 
            this.btnHidePassword.Location = new System.Drawing.Point(297, 87);
            this.btnHidePassword.Name = "btnHidePassword";
            this.btnHidePassword.Size = new System.Drawing.Size(75, 23);
            this.btnHidePassword.TabIndex = 5;
            this.btnHidePassword.Text = "Hide";
            this.btnHidePassword.UseVisualStyleBackColor = true;
            this.btnHidePassword.Visible = false;
            this.btnHidePassword.Click += new System.EventHandler(this.btnShowHidePassword_Click);
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.pbInfo);
            this.pnlInfo.Controls.Add(this.lblInfo);
            this.pnlInfo.Location = new System.Drawing.Point(12, 100);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(360, 21);
            this.pnlInfo.TabIndex = 6;
            // 
            // pbInfo
            // 
            this.pbInfo.Image = Resources.info;
            this.pbInfo.Location = new System.Drawing.Point(0, 2);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(16, 16);
            this.pbInfo.TabIndex = 0;
            this.pbInfo.TabStop = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblInfo.Location = new System.Drawing.Point(22, 3);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(269, 15);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Changing the user ID will invalidate the password.";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 127);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(9, 9);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(44, 15);
            this.lblUserID.TabIndex = 0;
            this.lblUserID.Text = "User ID";
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(12, 27);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(100, 23);
            this.txtUserID.TabIndex = 1;
            // 
            // FrmPasswordSet
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 162);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.btnHidePassword);
            this.Controls.Add(this.btnShowPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPasswordSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Password";
            this.Load += new System.EventHandler(this.FrmPasswordSet_Load);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnShowPassword;
        private System.Windows.Forms.Button btnHidePassword;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.TextBox txtUserID;
    }
}