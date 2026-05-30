namespace Scada.Comm.Drivers.DrvRsClient.View.Forms
{
    partial class FrmLineConfig
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
            chkUseDefaultConnection = new CheckBox();
            ctrlClientConnection = new Scada.Forms.Controls.CtrlClientConnection();
            btnOK = new Button();
            btnCancel = new Button();
            pnlLineInfo = new Panel();
            lblLineInfo = new Label();
            pbConnectionInfo = new PictureBox();
            pnlLineInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbConnectionInfo).BeginInit();
            SuspendLayout();
            // 
            // chkUseDefaultConnection
            // 
            chkUseDefaultConnection.AutoSize = true;
            chkUseDefaultConnection.Location = new Point(12, 12);
            chkUseDefaultConnection.Name = "chkUseDefaultConnection";
            chkUseDefaultConnection.Size = new Size(148, 19);
            chkUseDefaultConnection.TabIndex = 0;
            chkUseDefaultConnection.Text = "Use default connection";
            chkUseDefaultConnection.UseVisualStyleBackColor = true;
            chkUseDefaultConnection.CheckedChanged += chkUseDefaultConnection_CheckedChanged;
            // 
            // ctrlClientConnection
            // 
            ctrlClientConnection.ConnectionOptions = null;
            ctrlClientConnection.InstanceEnabled = false;
            ctrlClientConnection.Location = new Point(12, 37);
            ctrlClientConnection.Name = "ctrlClientConnection";
            ctrlClientConnection.NameEnabled = false;
            ctrlClientConnection.Size = new Size(360, 366);
            ctrlClientConnection.TabIndex = 1;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 446);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 446);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlLineInfo
            // 
            pnlLineInfo.Controls.Add(lblLineInfo);
            pnlLineInfo.Controls.Add(pbConnectionInfo);
            pnlLineInfo.Location = new Point(12, 409);
            pnlLineInfo.Name = "pnlLineInfo";
            pnlLineInfo.Size = new Size(360, 21);
            pnlLineInfo.TabIndex = 2;
            // 
            // lblLineInfo
            // 
            lblLineInfo.AutoSize = true;
            lblLineInfo.ForeColor = SystemColors.GrayText;
            lblLineInfo.Location = new Point(22, 3);
            lblLineInfo.Name = "lblLineInfo";
            lblLineInfo.Size = new Size(264, 15);
            lblLineInfo.TabIndex = 0;
            lblLineInfo.Text = "Options are common to the communication line";
            // 
            // pbConnectionInfo
            // 
            pbConnectionInfo.Image = Properties.Resources.info;
            pbConnectionInfo.Location = new Point(0, 2);
            pbConnectionInfo.Name = "pbConnectionInfo";
            pbConnectionInfo.Size = new Size(16, 16);
            pbConnectionInfo.TabIndex = 12;
            pbConnectionInfo.TabStop = false;
            // 
            // FrmLineConfig
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 481);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(pnlLineInfo);
            Controls.Add(ctrlClientConnection);
            Controls.Add(chkUseDefaultConnection);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLineConfig";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Connection Options";
            Load += FrmLineConfig_Load;
            pnlLineInfo.ResumeLayout(false);
            pnlLineInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbConnectionInfo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkUseDefaultConnection;
        private Scada.Forms.Controls.CtrlClientConnection ctrlClientConnection;
        private Button btnOK;
        private Button btnCancel;
        private Panel pnlLineInfo;
        private Label lblLineInfo;
        private PictureBox pbConnectionInfo;
    }
}