namespace Scada.Admin.App.Forms.Deployment
{
    partial class FrmInstanceStatus
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
            components = new System.ComponentModel.Container();
            ctrlProfileSelector = new Controls.Deployment.CtrlProfileSelector();
            gbStatus = new System.Windows.Forms.GroupBox();
            txtUpdateTime = new System.Windows.Forms.TextBox();
            lblUpdateTime = new System.Windows.Forms.Label();
            btnRestartWeb = new System.Windows.Forms.Button();
            btnStopWeb = new System.Windows.Forms.Button();
            btnStartWeb = new System.Windows.Forms.Button();
            txtWebStatus = new System.Windows.Forms.TextBox();
            lblWebStatus = new System.Windows.Forms.Label();
            btnRestartComm = new System.Windows.Forms.Button();
            btnStopComm = new System.Windows.Forms.Button();
            btnStartComm = new System.Windows.Forms.Button();
            txtCommStatus = new System.Windows.Forms.TextBox();
            lblCommStatus = new System.Windows.Forms.Label();
            btnRestartServer = new System.Windows.Forms.Button();
            btnStopServer = new System.Windows.Forms.Button();
            btnStartServer = new System.Windows.Forms.Button();
            txtServerStatus = new System.Windows.Forms.TextBox();
            lblServerStatus = new System.Windows.Forms.Label();
            gbAction = new System.Windows.Forms.GroupBox();
            btnDisconnect = new System.Windows.Forms.Button();
            btnConnect = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            timer = new System.Windows.Forms.Timer(components);
            gbStatus.SuspendLayout();
            gbAction.SuspendLayout();
            SuspendLayout();
            // 
            // ctrlProfileSelector
            // 
            ctrlProfileSelector.Location = new System.Drawing.Point(12, 12);
            ctrlProfileSelector.Name = "ctrlProfileSelector";
            ctrlProfileSelector.Size = new System.Drawing.Size(469, 113);
            ctrlProfileSelector.TabIndex = 0;
            ctrlProfileSelector.SelectedProfileChanged += ctrlProfileSelector_SelectedProfileChanged;
            ctrlProfileSelector.ProfileEdited += ctrlProfileSelector_ProfileEdited;
            // 
            // gbStatus
            // 
            gbStatus.Controls.Add(txtUpdateTime);
            gbStatus.Controls.Add(lblUpdateTime);
            gbStatus.Controls.Add(btnRestartWeb);
            gbStatus.Controls.Add(btnStopWeb);
            gbStatus.Controls.Add(btnStartWeb);
            gbStatus.Controls.Add(txtWebStatus);
            gbStatus.Controls.Add(lblWebStatus);
            gbStatus.Controls.Add(btnRestartComm);
            gbStatus.Controls.Add(btnStopComm);
            gbStatus.Controls.Add(btnStartComm);
            gbStatus.Controls.Add(txtCommStatus);
            gbStatus.Controls.Add(lblCommStatus);
            gbStatus.Controls.Add(btnRestartServer);
            gbStatus.Controls.Add(btnStopServer);
            gbStatus.Controls.Add(btnStartServer);
            gbStatus.Controls.Add(txtServerStatus);
            gbStatus.Controls.Add(lblServerStatus);
            gbStatus.Location = new System.Drawing.Point(12, 195);
            gbStatus.Name = "gbStatus";
            gbStatus.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            gbStatus.Size = new System.Drawing.Size(469, 145);
            gbStatus.TabIndex = 2;
            gbStatus.TabStop = false;
            gbStatus.Text = "Status";
            // 
            // txtUpdateTime
            // 
            txtUpdateTime.Location = new System.Drawing.Point(108, 109);
            txtUpdateTime.Name = "txtUpdateTime";
            txtUpdateTime.ReadOnly = true;
            txtUpdateTime.Size = new System.Drawing.Size(150, 23);
            txtUpdateTime.TabIndex = 16;
            // 
            // lblUpdateTime
            // 
            lblUpdateTime.AutoSize = true;
            lblUpdateTime.Location = new System.Drawing.Point(13, 113);
            lblUpdateTime.Name = "lblUpdateTime";
            lblUpdateTime.Size = new System.Drawing.Size(72, 15);
            lblUpdateTime.TabIndex = 15;
            lblUpdateTime.Text = "Update time";
            // 
            // btnRestartWeb
            // 
            btnRestartWeb.Location = new System.Drawing.Point(396, 80);
            btnRestartWeb.Name = "btnRestartWeb";
            btnRestartWeb.Size = new System.Drawing.Size(60, 23);
            btnRestartWeb.TabIndex = 14;
            btnRestartWeb.Text = "Restart";
            btnRestartWeb.UseVisualStyleBackColor = true;
            btnRestartWeb.Click += btnControlService_Click;
            // 
            // btnStopWeb
            // 
            btnStopWeb.Location = new System.Drawing.Point(330, 80);
            btnStopWeb.Name = "btnStopWeb";
            btnStopWeb.Size = new System.Drawing.Size(60, 23);
            btnStopWeb.TabIndex = 13;
            btnStopWeb.Text = "Stop";
            btnStopWeb.UseVisualStyleBackColor = true;
            btnStopWeb.Click += btnControlService_Click;
            // 
            // btnStartWeb
            // 
            btnStartWeb.Location = new System.Drawing.Point(264, 80);
            btnStartWeb.Name = "btnStartWeb";
            btnStartWeb.Size = new System.Drawing.Size(60, 23);
            btnStartWeb.TabIndex = 12;
            btnStartWeb.Text = "Start";
            btnStartWeb.UseVisualStyleBackColor = true;
            btnStartWeb.Click += btnControlService_Click;
            // 
            // txtWebStatus
            // 
            txtWebStatus.Enabled = false;
            txtWebStatus.Location = new System.Drawing.Point(108, 80);
            txtWebStatus.Name = "txtWebStatus";
            txtWebStatus.ReadOnly = true;
            txtWebStatus.Size = new System.Drawing.Size(150, 23);
            txtWebStatus.TabIndex = 11;
            txtWebStatus.Text = "Status not supported";
            // 
            // lblWebStatus
            // 
            lblWebStatus.AutoSize = true;
            lblWebStatus.Location = new System.Drawing.Point(13, 84);
            lblWebStatus.Name = "lblWebStatus";
            lblWebStatus.Size = new System.Drawing.Size(67, 15);
            lblWebStatus.TabIndex = 10;
            lblWebStatus.Text = "Webstation";
            // 
            // btnRestartComm
            // 
            btnRestartComm.Location = new System.Drawing.Point(396, 51);
            btnRestartComm.Name = "btnRestartComm";
            btnRestartComm.Size = new System.Drawing.Size(60, 23);
            btnRestartComm.TabIndex = 9;
            btnRestartComm.Text = "Restart";
            btnRestartComm.UseVisualStyleBackColor = true;
            btnRestartComm.Click += btnControlService_Click;
            // 
            // btnStopComm
            // 
            btnStopComm.Location = new System.Drawing.Point(330, 51);
            btnStopComm.Name = "btnStopComm";
            btnStopComm.Size = new System.Drawing.Size(60, 23);
            btnStopComm.TabIndex = 8;
            btnStopComm.Text = "Stop";
            btnStopComm.UseVisualStyleBackColor = true;
            btnStopComm.Click += btnControlService_Click;
            // 
            // btnStartComm
            // 
            btnStartComm.Location = new System.Drawing.Point(264, 51);
            btnStartComm.Name = "btnStartComm";
            btnStartComm.Size = new System.Drawing.Size(60, 23);
            btnStartComm.TabIndex = 7;
            btnStartComm.Text = "Start";
            btnStartComm.UseVisualStyleBackColor = true;
            btnStartComm.Click += btnControlService_Click;
            // 
            // txtCommStatus
            // 
            txtCommStatus.Location = new System.Drawing.Point(108, 51);
            txtCommStatus.Name = "txtCommStatus";
            txtCommStatus.ReadOnly = true;
            txtCommStatus.Size = new System.Drawing.Size(150, 23);
            txtCommStatus.TabIndex = 6;
            // 
            // lblCommStatus
            // 
            lblCommStatus.AutoSize = true;
            lblCommStatus.Location = new System.Drawing.Point(13, 55);
            lblCommStatus.Name = "lblCommStatus";
            lblCommStatus.Size = new System.Drawing.Size(88, 15);
            lblCommStatus.TabIndex = 5;
            lblCommStatus.Text = "Communicator";
            // 
            // btnRestartServer
            // 
            btnRestartServer.Location = new System.Drawing.Point(396, 22);
            btnRestartServer.Name = "btnRestartServer";
            btnRestartServer.Size = new System.Drawing.Size(60, 23);
            btnRestartServer.TabIndex = 4;
            btnRestartServer.Text = "Restart";
            btnRestartServer.UseVisualStyleBackColor = true;
            btnRestartServer.Click += btnControlService_Click;
            // 
            // btnStopServer
            // 
            btnStopServer.Location = new System.Drawing.Point(330, 22);
            btnStopServer.Name = "btnStopServer";
            btnStopServer.Size = new System.Drawing.Size(60, 23);
            btnStopServer.TabIndex = 3;
            btnStopServer.Text = "Stop";
            btnStopServer.UseVisualStyleBackColor = true;
            btnStopServer.Click += btnControlService_Click;
            // 
            // btnStartServer
            // 
            btnStartServer.Location = new System.Drawing.Point(264, 22);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new System.Drawing.Size(60, 23);
            btnStartServer.TabIndex = 2;
            btnStartServer.Text = "Start";
            btnStartServer.UseVisualStyleBackColor = true;
            btnStartServer.Click += btnControlService_Click;
            // 
            // txtServerStatus
            // 
            txtServerStatus.Location = new System.Drawing.Point(108, 22);
            txtServerStatus.Name = "txtServerStatus";
            txtServerStatus.ReadOnly = true;
            txtServerStatus.Size = new System.Drawing.Size(150, 23);
            txtServerStatus.TabIndex = 1;
            // 
            // lblServerStatus
            // 
            lblServerStatus.AutoSize = true;
            lblServerStatus.Location = new System.Drawing.Point(13, 26);
            lblServerStatus.Name = "lblServerStatus";
            lblServerStatus.Size = new System.Drawing.Size(39, 15);
            lblServerStatus.TabIndex = 0;
            lblServerStatus.Text = "Server";
            // 
            // gbAction
            // 
            gbAction.Controls.Add(btnDisconnect);
            gbAction.Controls.Add(btnConnect);
            gbAction.Location = new System.Drawing.Point(12, 131);
            gbAction.Name = "gbAction";
            gbAction.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            gbAction.Size = new System.Drawing.Size(469, 58);
            gbAction.TabIndex = 1;
            gbAction.TabStop = false;
            gbAction.Text = "Actions";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new System.Drawing.Point(119, 22);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new System.Drawing.Size(100, 23);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new System.Drawing.Point(13, 22);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new System.Drawing.Size(100, 23);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnClose
            // 
            btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnClose.Location = new System.Drawing.Point(406, 356);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(75, 23);
            btnClose.TabIndex = 3;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            // 
            // FrmInstanceStatus
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new System.Drawing.Size(493, 391);
            Controls.Add(btnClose);
            Controls.Add(gbStatus);
            Controls.Add(gbAction);
            Controls.Add(ctrlProfileSelector);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmInstanceStatus";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Instance Status";
            FormClosed += FrmInstanceStatus_FormClosed;
            Load += FrmInstanceStatus_Load;
            gbStatus.ResumeLayout(false);
            gbStatus.PerformLayout();
            gbAction.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Controls.Deployment.CtrlProfileSelector ctrlProfileSelector;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.TextBox txtUpdateTime;
        private System.Windows.Forms.Label lblUpdateTime;
        private System.Windows.Forms.Button btnRestartComm;
        private System.Windows.Forms.TextBox txtCommStatus;
        private System.Windows.Forms.Label lblCommStatus;
        private System.Windows.Forms.Button btnRestartServer;
        private System.Windows.Forms.TextBox txtServerStatus;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.GroupBox gbAction;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnStopComm;
        private System.Windows.Forms.Button btnStartComm;
        private System.Windows.Forms.Button btnStopWeb;
        private System.Windows.Forms.Button btnStartWeb;
        private System.Windows.Forms.Button btnRestartWeb;
        private System.Windows.Forms.TextBox txtWebStatus;
        private System.Windows.Forms.Label lblWebStatus;
    }
}