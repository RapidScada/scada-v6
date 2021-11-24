namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    partial class FrmDeviceData
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
            this.lbDeviceData = new System.Windows.Forms.ListBox();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.btnDeviceProperties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbDeviceData
            // 
            this.lbDeviceData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDeviceData.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbDeviceData.FormattingEnabled = true;
            this.lbDeviceData.HorizontalScrollbar = true;
            this.lbDeviceData.IntegralHeight = false;
            this.lbDeviceData.Location = new System.Drawing.Point(12, 41);
            this.lbDeviceData.Name = "lbDeviceData";
            this.lbDeviceData.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbDeviceData.Size = new System.Drawing.Size(660, 358);
            this.lbDeviceData.TabIndex = 2;
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.Location = new System.Drawing.Point(93, 12);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(75, 23);
            this.btnSendCommand.TabIndex = 1;
            this.btnSendCommand.Text = "Command";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Interval = 1000;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // btnDeviceProperties
            // 
            this.btnDeviceProperties.Location = new System.Drawing.Point(12, 12);
            this.btnDeviceProperties.Name = "btnDeviceProperties";
            this.btnDeviceProperties.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceProperties.TabIndex = 0;
            this.btnDeviceProperties.Text = "Properties";
            this.btnDeviceProperties.UseVisualStyleBackColor = true;
            this.btnDeviceProperties.Click += new System.EventHandler(this.btnDeviceProperties_Click);
            // 
            // FrmDeviceData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.lbDeviceData);
            this.Controls.Add(this.btnSendCommand);
            this.Controls.Add(this.btnDeviceProperties);
            this.Name = "FrmDeviceData";
            this.Text = "Device {0} Data";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDeviceData_FormClosed);
            this.Load += new System.EventHandler(this.FrmDeviceData_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmDeviceData_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbDeviceData;
        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.Button btnDeviceProperties;
    }
}