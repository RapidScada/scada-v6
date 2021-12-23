namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    partial class CtrlSync1
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
            this.lblSyncDirection = new System.Windows.Forms.Label();
            this.rbBaseToComm = new System.Windows.Forms.RadioButton();
            this.rbCommToBase = new System.Windows.Forms.RadioButton();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSyncDirection
            // 
            this.lblSyncDirection.AutoSize = true;
            this.lblSyncDirection.Location = new System.Drawing.Point(-3, 0);
            this.lblSyncDirection.Name = "lblSyncDirection";
            this.lblSyncDirection.Size = new System.Drawing.Size(187, 15);
            this.lblSyncDirection.TabIndex = 0;
            this.lblSyncDirection.Text = "Choose synchronization direction:";
            // 
            // rbBaseToComm
            // 
            this.rbBaseToComm.AutoSize = true;
            this.rbBaseToComm.Checked = true;
            this.rbBaseToComm.Location = new System.Drawing.Point(0, 18);
            this.rbBaseToComm.Name = "rbBaseToComm";
            this.rbBaseToComm.Size = new System.Drawing.Size(247, 19);
            this.rbBaseToComm.TabIndex = 1;
            this.rbBaseToComm.TabStop = true;
            this.rbBaseToComm.Text = "Configuration database to Communicator";
            this.rbBaseToComm.UseVisualStyleBackColor = true;
            // 
            // rbCommToBase
            // 
            this.rbCommToBase.AutoSize = true;
            this.rbCommToBase.Location = new System.Drawing.Point(0, 43);
            this.rbCommToBase.Name = "rbCommToBase";
            this.rbCommToBase.Size = new System.Drawing.Size(245, 19);
            this.rbCommToBase.TabIndex = 2;
            this.rbCommToBase.Text = "Communicator to configuration database";
            this.rbCommToBase.UseVisualStyleBackColor = true;
            // 
            // pbInfo
            // 
            this.pbInfo.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.info;
            this.pbInfo.Location = new System.Drawing.Point(0, 78);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(16, 16);
            this.pbInfo.TabIndex = 3;
            this.pbInfo.TabStop = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(22, 75);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(227, 120);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "Missing lines and devices will be created.\r\nExisting lines and devices will be up" +
    "dated.\r\n\r\nAffected properties:\r\n- Communication line name\r\n- Device name\r\n- Devi" +
    "ce driver\r\n- Device address";
            // 
            // CtrlSync1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.rbCommToBase);
            this.Controls.Add(this.rbBaseToComm);
            this.Controls.Add(this.lblSyncDirection);
            this.Name = "CtrlSync1";
            this.Size = new System.Drawing.Size(350, 220);
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSyncDirection;
        private System.Windows.Forms.RadioButton rbBaseToComm;
        private System.Windows.Forms.RadioButton rbCommToBase;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.Label lblInfo;
    }
}
