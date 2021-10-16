
namespace Scada.Comm.Drivers.DrvDsOpcUaServer.View.Forms
{
    partial class FrmOpcUaServerDSO
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
            this.chkAutoAccept = new System.Windows.Forms.CheckBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblConfigFileName = new System.Windows.Forms.Label();
            this.txtConfigFileName = new System.Windows.Forms.TextBox();
            this.btnCreateConfigWin = new System.Windows.Forms.Button();
            this.btnCreateConfigLinux = new System.Windows.Forms.Button();
            this.btnOpenConfig = new System.Windows.Forms.Button();
            this.lblDeviceFilter = new System.Windows.Forms.Label();
            this.txtDeviceFilter = new System.Windows.Forms.TextBox();
            this.btnSelectDevices = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkAutoAccept
            // 
            this.chkAutoAccept.AutoSize = true;
            this.chkAutoAccept.Location = new System.Drawing.Point(12, 12);
            this.chkAutoAccept.Name = "chkAutoAccept";
            this.chkAutoAccept.Size = new System.Drawing.Size(198, 19);
            this.chkAutoAccept.TabIndex = 0;
            this.chkAutoAccept.Text = "Automatically accept certificates";
            this.chkAutoAccept.UseVisualStyleBackColor = true;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(9, 34);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(12, 52);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(360, 23);
            this.txtUsername.TabIndex = 2;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 78);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 96);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(360, 23);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblConfigFileName
            // 
            this.lblConfigFileName.AutoSize = true;
            this.lblConfigFileName.Location = new System.Drawing.Point(9, 122);
            this.lblConfigFileName.Name = "lblConfigFileName";
            this.lblConfigFileName.Size = new System.Drawing.Size(133, 15);
            this.lblConfigFileName.TabIndex = 5;
            this.lblConfigFileName.Text = "Configuration file name";
            // 
            // txtConfigFileName
            // 
            this.txtConfigFileName.Location = new System.Drawing.Point(12, 140);
            this.txtConfigFileName.Name = "txtConfigFileName";
            this.txtConfigFileName.Size = new System.Drawing.Size(273, 23);
            this.txtConfigFileName.TabIndex = 6;
            // 
            // btnCreateConfigWin
            // 
            this.btnCreateConfigWin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreateConfigWin.Image = global::Scada.Comm.Drivers.DrvDsOpcUaServer.View.Properties.Resources.windows;
            this.btnCreateConfigWin.Location = new System.Drawing.Point(291, 140);
            this.btnCreateConfigWin.Name = "btnCreateConfigWin";
            this.btnCreateConfigWin.Size = new System.Drawing.Size(23, 23);
            this.btnCreateConfigWin.TabIndex = 7;
            this.btnCreateConfigWin.UseVisualStyleBackColor = true;
            // 
            // btnCreateConfigLinux
            // 
            this.btnCreateConfigLinux.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreateConfigLinux.Image = global::Scada.Comm.Drivers.DrvDsOpcUaServer.View.Properties.Resources.linux;
            this.btnCreateConfigLinux.Location = new System.Drawing.Point(320, 140);
            this.btnCreateConfigLinux.Name = "btnCreateConfigLinux";
            this.btnCreateConfigLinux.Size = new System.Drawing.Size(23, 23);
            this.btnCreateConfigLinux.TabIndex = 8;
            this.btnCreateConfigLinux.UseVisualStyleBackColor = true;
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpenConfig.Image = global::Scada.Comm.Drivers.DrvDsOpcUaServer.View.Properties.Resources.open;
            this.btnOpenConfig.Location = new System.Drawing.Point(349, 140);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(23, 23);
            this.btnOpenConfig.TabIndex = 9;
            this.btnOpenConfig.UseVisualStyleBackColor = true;
            // 
            // lblDeviceFilter
            // 
            this.lblDeviceFilter.AutoSize = true;
            this.lblDeviceFilter.Location = new System.Drawing.Point(9, 166);
            this.lblDeviceFilter.Name = "lblDeviceFilter";
            this.lblDeviceFilter.Size = new System.Drawing.Size(69, 15);
            this.lblDeviceFilter.TabIndex = 10;
            this.lblDeviceFilter.Text = "Device filter";
            // 
            // txtDeviceFilter
            // 
            this.txtDeviceFilter.Location = new System.Drawing.Point(12, 184);
            this.txtDeviceFilter.Name = "txtDeviceFilter";
            this.txtDeviceFilter.Size = new System.Drawing.Size(331, 23);
            this.txtDeviceFilter.TabIndex = 11;
            // 
            // btnSelectDevices
            // 
            this.btnSelectDevices.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectDevices.Image = global::Scada.Comm.Drivers.DrvDsOpcUaServer.View.Properties.Resources.find;
            this.btnSelectDevices.Location = new System.Drawing.Point(349, 184);
            this.btnSelectDevices.Name = "btnSelectDevices";
            this.btnSelectDevices.Size = new System.Drawing.Size(23, 23);
            this.btnSelectDevices.TabIndex = 12;
            this.btnSelectDevices.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 223);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmOpcUaServerDSO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 258);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSelectDevices);
            this.Controls.Add(this.txtDeviceFilter);
            this.Controls.Add(this.lblDeviceFilter);
            this.Controls.Add(this.btnOpenConfig);
            this.Controls.Add(this.btnCreateConfigLinux);
            this.Controls.Add(this.btnCreateConfigWin);
            this.Controls.Add(this.txtConfigFileName);
            this.Controls.Add(this.lblConfigFileName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.chkAutoAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOpcUaServerDSO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Source Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkAutoAccept;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblConfigFileName;
        private System.Windows.Forms.TextBox txtConfigFileName;
        private System.Windows.Forms.Button btnCreateConfigWin;
        private System.Windows.Forms.Button btnCreateConfigLinux;
        private System.Windows.Forms.Button btnOpenConfig;
        private System.Windows.Forms.Label lblDeviceFilter;
        private System.Windows.Forms.TextBox txtDeviceFilter;
        private System.Windows.Forms.Button btnSelectDevices;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}