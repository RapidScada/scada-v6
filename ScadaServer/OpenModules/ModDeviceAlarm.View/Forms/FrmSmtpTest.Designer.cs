namespace Scada.Server.Modules.ModDeviceAlarm.View.Forms
{
    partial class FrmSmtpTest
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
            btnSend = new Button();
            txtSendTo = new TextBox();
            lblSendTo = new Label();
            txtContent = new TextBox();
            lblSubject = new Label();
            txtSubject = new TextBox();
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.Location = new Point(13, 13);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(67, 70);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtSendTo
            // 
            txtSendTo.Location = new Point(177, 15);
            txtSendTo.Name = "txtSendTo";
            txtSendTo.Size = new Size(407, 23);
            txtSendTo.TabIndex = 1;
            // 
            // lblSendTo
            // 
            lblSendTo.AutoSize = true;
            lblSendTo.Location = new Point(111, 18);
            lblSendTo.Name = "lblSendTo";
            lblSendTo.Size = new Size(52, 17);
            lblSendTo.TabIndex = 2;
            lblSendTo.Text = "SendTo";
            // 
            // txtContent
            // 
            txtContent.Location = new Point(14, 100);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.Size = new Size(570, 189);
            txtContent.TabIndex = 3;
            // 
            // lblSubject
            // 
            lblSubject.AutoSize = true;
            lblSubject.Location = new Point(111, 63);
            lblSubject.Name = "lblSubject";
            lblSubject.Size = new Size(50, 17);
            lblSubject.TabIndex = 5;
            lblSubject.Text = "Subject";
            // 
            // txtSubject
            // 
            txtSubject.Location = new Point(177, 60);
            txtSubject.Name = "txtSubject";
            txtSubject.Size = new Size(407, 23);
            txtSubject.TabIndex = 4;
            // 
            // FrmSmtpTest
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(596, 308);
            Controls.Add(lblSubject);
            Controls.Add(txtSubject);
            Controls.Add(txtContent);
            Controls.Add(lblSendTo);
            Controls.Add(txtSendTo);
            Controls.Add(btnSend);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSmtpTest";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmSmtpTest";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private TextBox txtSendTo;
        private Label lblSendTo;
        private TextBox txtContent;
        private Label lblSubject;
        private TextBox txtSubject;
    }
}