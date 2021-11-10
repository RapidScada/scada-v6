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
            this.components = new System.ComponentModel.Container();
            this.ctrlProfileSelector = new Scada.Admin.App.Controls.Deployment.CtrlProfileSelector();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.txtUpdateTime = new System.Windows.Forms.TextBox();
            this.lblUpdateTime = new System.Windows.Forms.Label();
            this.btnRestartWeb = new System.Windows.Forms.Button();
            this.btnStopWeb = new System.Windows.Forms.Button();
            this.btnStartWeb = new System.Windows.Forms.Button();
            this.txtWebStatus = new System.Windows.Forms.TextBox();
            this.lblWebStatus = new System.Windows.Forms.Label();
            this.btnRestartComm = new System.Windows.Forms.Button();
            this.btnStopComm = new System.Windows.Forms.Button();
            this.btnStartComm = new System.Windows.Forms.Button();
            this.txtCommStatus = new System.Windows.Forms.TextBox();
            this.lblCommStatus = new System.Windows.Forms.Label();
            this.btnRestartServer = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.txtServerStatus = new System.Windows.Forms.TextBox();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.gbAction = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.gbStatus.SuspendLayout();
            this.gbAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctrlProfileSelector
            // 
            this.ctrlProfileSelector.Location = new System.Drawing.Point(12, 12);
            this.ctrlProfileSelector.Name = "ctrlProfileSelector";
            this.ctrlProfileSelector.Size = new System.Drawing.Size(469, 113);
            this.ctrlProfileSelector.TabIndex = 0;
            this.ctrlProfileSelector.SelectedProfileChanged += new System.EventHandler(this.ctrlProfileSelector_SelectedProfileChanged);
            this.ctrlProfileSelector.ProfileEdited += new System.EventHandler(this.ctrlProfileSelector_ProfileEdited);
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.txtUpdateTime);
            this.gbStatus.Controls.Add(this.lblUpdateTime);
            this.gbStatus.Controls.Add(this.btnRestartWeb);
            this.gbStatus.Controls.Add(this.btnStopWeb);
            this.gbStatus.Controls.Add(this.btnStartWeb);
            this.gbStatus.Controls.Add(this.txtWebStatus);
            this.gbStatus.Controls.Add(this.lblWebStatus);
            this.gbStatus.Controls.Add(this.btnRestartComm);
            this.gbStatus.Controls.Add(this.btnStopComm);
            this.gbStatus.Controls.Add(this.btnStartComm);
            this.gbStatus.Controls.Add(this.txtCommStatus);
            this.gbStatus.Controls.Add(this.lblCommStatus);
            this.gbStatus.Controls.Add(this.btnRestartServer);
            this.gbStatus.Controls.Add(this.btnStopServer);
            this.gbStatus.Controls.Add(this.btnStartServer);
            this.gbStatus.Controls.Add(this.txtServerStatus);
            this.gbStatus.Controls.Add(this.lblServerStatus);
            this.gbStatus.Location = new System.Drawing.Point(12, 195);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbStatus.Size = new System.Drawing.Size(469, 145);
            this.gbStatus.TabIndex = 2;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Status";
            // 
            // txtUpdateTime
            // 
            this.txtUpdateTime.Location = new System.Drawing.Point(108, 109);
            this.txtUpdateTime.Name = "txtUpdateTime";
            this.txtUpdateTime.ReadOnly = true;
            this.txtUpdateTime.Size = new System.Drawing.Size(150, 23);
            this.txtUpdateTime.TabIndex = 16;
            // 
            // lblUpdateTime
            // 
            this.lblUpdateTime.AutoSize = true;
            this.lblUpdateTime.Location = new System.Drawing.Point(13, 113);
            this.lblUpdateTime.Name = "lblUpdateTime";
            this.lblUpdateTime.Size = new System.Drawing.Size(72, 15);
            this.lblUpdateTime.TabIndex = 15;
            this.lblUpdateTime.Text = "Update time";
            // 
            // btnRestartWeb
            // 
            this.btnRestartWeb.Location = new System.Drawing.Point(396, 80);
            this.btnRestartWeb.Name = "btnRestartWeb";
            this.btnRestartWeb.Size = new System.Drawing.Size(60, 23);
            this.btnRestartWeb.TabIndex = 14;
            this.btnRestartWeb.Text = "Restart";
            this.btnRestartWeb.UseVisualStyleBackColor = true;
            this.btnRestartWeb.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // btnStopWeb
            // 
            this.btnStopWeb.Location = new System.Drawing.Point(330, 80);
            this.btnStopWeb.Name = "btnStopWeb";
            this.btnStopWeb.Size = new System.Drawing.Size(60, 23);
            this.btnStopWeb.TabIndex = 13;
            this.btnStopWeb.Text = "Stop";
            this.btnStopWeb.UseVisualStyleBackColor = true;
            this.btnStopWeb.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // btnStartWeb
            // 
            this.btnStartWeb.Location = new System.Drawing.Point(264, 80);
            this.btnStartWeb.Name = "btnStartWeb";
            this.btnStartWeb.Size = new System.Drawing.Size(60, 23);
            this.btnStartWeb.TabIndex = 12;
            this.btnStartWeb.Text = "Start";
            this.btnStartWeb.UseVisualStyleBackColor = true;
            this.btnStartWeb.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // txtWebStatus
            // 
            this.txtWebStatus.Location = new System.Drawing.Point(108, 80);
            this.txtWebStatus.Name = "txtWebStatus";
            this.txtWebStatus.ReadOnly = true;
            this.txtWebStatus.Size = new System.Drawing.Size(150, 23);
            this.txtWebStatus.TabIndex = 11;
            this.txtWebStatus.Text = "Status not available";
            // 
            // lblWebStatus
            // 
            this.lblWebStatus.AutoSize = true;
            this.lblWebStatus.Location = new System.Drawing.Point(13, 84);
            this.lblWebStatus.Name = "lblWebStatus";
            this.lblWebStatus.Size = new System.Drawing.Size(67, 15);
            this.lblWebStatus.TabIndex = 10;
            this.lblWebStatus.Text = "Webstation";
            // 
            // btnRestartComm
            // 
            this.btnRestartComm.Location = new System.Drawing.Point(396, 51);
            this.btnRestartComm.Name = "btnRestartComm";
            this.btnRestartComm.Size = new System.Drawing.Size(60, 23);
            this.btnRestartComm.TabIndex = 9;
            this.btnRestartComm.Text = "Restart";
            this.btnRestartComm.UseVisualStyleBackColor = true;
            this.btnRestartComm.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // btnStopComm
            // 
            this.btnStopComm.Location = new System.Drawing.Point(330, 51);
            this.btnStopComm.Name = "btnStopComm";
            this.btnStopComm.Size = new System.Drawing.Size(60, 23);
            this.btnStopComm.TabIndex = 8;
            this.btnStopComm.Text = "Stop";
            this.btnStopComm.UseVisualStyleBackColor = true;
            this.btnStopComm.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // btnStartComm
            // 
            this.btnStartComm.Location = new System.Drawing.Point(264, 51);
            this.btnStartComm.Name = "btnStartComm";
            this.btnStartComm.Size = new System.Drawing.Size(60, 23);
            this.btnStartComm.TabIndex = 7;
            this.btnStartComm.Text = "Start";
            this.btnStartComm.UseVisualStyleBackColor = true;
            this.btnStartComm.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // txtCommStatus
            // 
            this.txtCommStatus.Location = new System.Drawing.Point(108, 51);
            this.txtCommStatus.Name = "txtCommStatus";
            this.txtCommStatus.ReadOnly = true;
            this.txtCommStatus.Size = new System.Drawing.Size(150, 23);
            this.txtCommStatus.TabIndex = 6;
            // 
            // lblCommStatus
            // 
            this.lblCommStatus.AutoSize = true;
            this.lblCommStatus.Location = new System.Drawing.Point(13, 55);
            this.lblCommStatus.Name = "lblCommStatus";
            this.lblCommStatus.Size = new System.Drawing.Size(88, 15);
            this.lblCommStatus.TabIndex = 5;
            this.lblCommStatus.Text = "Communicator";
            // 
            // btnRestartServer
            // 
            this.btnRestartServer.Location = new System.Drawing.Point(396, 22);
            this.btnRestartServer.Name = "btnRestartServer";
            this.btnRestartServer.Size = new System.Drawing.Size(60, 23);
            this.btnRestartServer.TabIndex = 4;
            this.btnRestartServer.Text = "Restart";
            this.btnRestartServer.UseVisualStyleBackColor = true;
            this.btnRestartServer.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.Location = new System.Drawing.Point(330, 22);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(60, 23);
            this.btnStopServer.TabIndex = 3;
            this.btnStopServer.Text = "Stop";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(264, 22);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(60, 23);
            this.btnStartServer.TabIndex = 2;
            this.btnStartServer.Text = "Start";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnControlService_Click);
            // 
            // txtServerStatus
            // 
            this.txtServerStatus.Location = new System.Drawing.Point(108, 22);
            this.txtServerStatus.Name = "txtServerStatus";
            this.txtServerStatus.ReadOnly = true;
            this.txtServerStatus.Size = new System.Drawing.Size(150, 23);
            this.txtServerStatus.TabIndex = 1;
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Location = new System.Drawing.Point(13, 26);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(39, 15);
            this.lblServerStatus.TabIndex = 0;
            this.lblServerStatus.Text = "Server";
            // 
            // gbAction
            // 
            this.gbAction.Controls.Add(this.btnDisconnect);
            this.gbAction.Controls.Add(this.btnConnect);
            this.gbAction.Location = new System.Drawing.Point(12, 131);
            this.gbAction.Name = "gbAction";
            this.gbAction.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbAction.Size = new System.Drawing.Size(469, 58);
            this.gbAction.TabIndex = 1;
            this.gbAction.TabStop = false;
            this.gbAction.Text = "Actions";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(119, 22);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 23);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(13, 22);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(406, 356);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // FrmInstanceStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(493, 391);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.gbAction);
            this.Controls.Add(this.ctrlProfileSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInstanceStatus";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instance Status";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmInstanceStatus_FormClosed);
            this.Load += new System.EventHandler(this.FrmInstanceStatus_Load);
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.gbAction.ResumeLayout(false);
            this.ResumeLayout(false);

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