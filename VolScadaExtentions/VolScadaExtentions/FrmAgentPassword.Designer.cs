
using ModifySystemAccount;

namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmAgentPassword
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
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnShowPassword = new Button();
            btnHidePassword = new Button();
            btnOK = new Button();
            btnCancel = new Button();
            lblUserID = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(9, 60);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(95, 17);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "New password";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(12, 80);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(279, 23);
            txtPassword.TabIndex = 3;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // btnShowPassword
            // 
            btnShowPassword.Location = new Point(297, 80);
            btnShowPassword.Name = "btnShowPassword";
            btnShowPassword.Size = new Size(75, 26);
            btnShowPassword.TabIndex = 4;
            btnShowPassword.Text = "Show";
            btnShowPassword.UseVisualStyleBackColor = true;
            btnShowPassword.Click += btnShowHidePassword_Click;
            // 
            // btnHidePassword
            // 
            btnHidePassword.Location = new Point(297, 99);
            btnHidePassword.Name = "btnHidePassword";
            btnHidePassword.Size = new Size(75, 26);
            btnHidePassword.TabIndex = 5;
            btnHidePassword.Text = "Hide";
            btnHidePassword.UseVisualStyleBackColor = true;
            btnHidePassword.Visible = false;
            btnHidePassword.Click += btnShowHidePassword_Click;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 144);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 26);
            btnOK.TabIndex = 7;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 144);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 26);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblUserID
            // 
            lblUserID.AutoSize = true;
            lblUserID.Location = new Point(9, 10);
            lblUserID.Name = "lblUserID";
            lblUserID.Size = new Size(35, 17);
            lblUserID.TabIndex = 0;
            lblUserID.Text = "User";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 34);
            label1.Name = "label1";
            label1.Size = new Size(77, 17);
            label1.TabIndex = 9;
            label1.Text = "ScadaAgent";
            // 
            // FrmAgentPassword
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 184);
            Controls.Add(label1);
            Controls.Add(lblUserID);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnHidePassword);
            Controls.Add(btnShowPassword);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAgentPassword";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Set Password";
            Load += FrmPasswordSet_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnShowPassword;
        private System.Windows.Forms.Button btnHidePassword;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblUserID;
        private Label label1;
    }
}