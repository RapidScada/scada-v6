namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    partial class CtrlPluginAssignment
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
            this.gbPluginAssignment = new System.Windows.Forms.GroupBox();
            this.cbNotificationFeature = new System.Windows.Forms.ComboBox();
            this.lblNotificationFeature = new System.Windows.Forms.Label();
            this.cbUserManagementFeature = new System.Windows.Forms.ComboBox();
            this.lblUserManagementFeature = new System.Windows.Forms.Label();
            this.cbEventAckFeature = new System.Windows.Forms.ComboBox();
            this.lblEventAckFeature = new System.Windows.Forms.Label();
            this.cbCommandFeature = new System.Windows.Forms.ComboBox();
            this.lblCommandFeature = new System.Windows.Forms.Label();
            this.cbChartFeature = new System.Windows.Forms.ComboBox();
            this.lblChartFeature = new System.Windows.Forms.Label();
            this.gbPluginAssignment.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPluginAssignment
            // 
            this.gbPluginAssignment.Controls.Add(this.cbNotificationFeature);
            this.gbPluginAssignment.Controls.Add(this.lblNotificationFeature);
            this.gbPluginAssignment.Controls.Add(this.cbUserManagementFeature);
            this.gbPluginAssignment.Controls.Add(this.lblUserManagementFeature);
            this.gbPluginAssignment.Controls.Add(this.cbEventAckFeature);
            this.gbPluginAssignment.Controls.Add(this.lblEventAckFeature);
            this.gbPluginAssignment.Controls.Add(this.cbCommandFeature);
            this.gbPluginAssignment.Controls.Add(this.lblCommandFeature);
            this.gbPluginAssignment.Controls.Add(this.cbChartFeature);
            this.gbPluginAssignment.Controls.Add(this.lblChartFeature);
            this.gbPluginAssignment.Location = new System.Drawing.Point(0, 0);
            this.gbPluginAssignment.Name = "gbPluginAssignment";
            this.gbPluginAssignment.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbPluginAssignment.Size = new System.Drawing.Size(500, 184);
            this.gbPluginAssignment.TabIndex = 0;
            this.gbPluginAssignment.TabStop = false;
            this.gbPluginAssignment.Text = "Plugin Assignment";
            // 
            // cbNotificationFeature
            // 
            this.cbNotificationFeature.FormattingEnabled = true;
            this.cbNotificationFeature.Location = new System.Drawing.Point(287, 150);
            this.cbNotificationFeature.Name = "cbNotificationFeature";
            this.cbNotificationFeature.Size = new System.Drawing.Size(200, 23);
            this.cbNotificationFeature.TabIndex = 9;
            this.cbNotificationFeature.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            this.cbNotificationFeature.TextUpdate += new System.EventHandler(this.control_Changed);
            // 
            // lblNotificationFeature
            // 
            this.lblNotificationFeature.AutoSize = true;
            this.lblNotificationFeature.Location = new System.Drawing.Point(10, 154);
            this.lblNotificationFeature.Name = "lblNotificationFeature";
            this.lblNotificationFeature.Size = new System.Drawing.Size(181, 15);
            this.lblNotificationFeature.TabIndex = 8;
            this.lblNotificationFeature.Text = "Notification management plugin";
            // 
            // cbUserManagementFeature
            // 
            this.cbUserManagementFeature.FormattingEnabled = true;
            this.cbUserManagementFeature.Location = new System.Drawing.Point(287, 119);
            this.cbUserManagementFeature.Name = "cbUserManagementFeature";
            this.cbUserManagementFeature.Size = new System.Drawing.Size(200, 23);
            this.cbUserManagementFeature.TabIndex = 7;
            this.cbUserManagementFeature.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            this.cbUserManagementFeature.TextUpdate += new System.EventHandler(this.control_Changed);
            // 
            // lblUserManagementFeature
            // 
            this.lblUserManagementFeature.AutoSize = true;
            this.lblUserManagementFeature.Location = new System.Drawing.Point(10, 123);
            this.lblUserManagementFeature.Name = "lblUserManagementFeature";
            this.lblUserManagementFeature.Size = new System.Drawing.Size(141, 15);
            this.lblUserManagementFeature.TabIndex = 6;
            this.lblUserManagementFeature.Text = "User management plugin";
            // 
            // cbEventAckFeature
            // 
            this.cbEventAckFeature.FormattingEnabled = true;
            this.cbEventAckFeature.Location = new System.Drawing.Point(287, 88);
            this.cbEventAckFeature.Name = "cbEventAckFeature";
            this.cbEventAckFeature.Size = new System.Drawing.Size(200, 23);
            this.cbEventAckFeature.TabIndex = 5;
            this.cbEventAckFeature.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            this.cbEventAckFeature.TextUpdate += new System.EventHandler(this.control_Changed);
            // 
            // lblEventAckFeature
            // 
            this.lblEventAckFeature.AutoSize = true;
            this.lblEventAckFeature.Location = new System.Drawing.Point(10, 92);
            this.lblEventAckFeature.Name = "lblEventAckFeature";
            this.lblEventAckFeature.Size = new System.Drawing.Size(192, 15);
            this.lblEventAckFeature.TabIndex = 4;
            this.lblEventAckFeature.Text = "Plugin for event acknowledgement";
            // 
            // cbCommandFeature
            // 
            this.cbCommandFeature.FormattingEnabled = true;
            this.cbCommandFeature.Location = new System.Drawing.Point(287, 57);
            this.cbCommandFeature.Name = "cbCommandFeature";
            this.cbCommandFeature.Size = new System.Drawing.Size(200, 23);
            this.cbCommandFeature.TabIndex = 3;
            this.cbCommandFeature.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            this.cbCommandFeature.TextUpdate += new System.EventHandler(this.control_Changed);
            // 
            // lblCommandFeature
            // 
            this.lblCommandFeature.AutoSize = true;
            this.lblCommandFeature.Location = new System.Drawing.Point(10, 61);
            this.lblCommandFeature.Name = "lblCommandFeature";
            this.lblCommandFeature.Size = new System.Drawing.Size(167, 15);
            this.lblCommandFeature.TabIndex = 2;
            this.lblCommandFeature.Text = "Plugin for sending commands";
            // 
            // cbChartFeature
            // 
            this.cbChartFeature.FormattingEnabled = true;
            this.cbChartFeature.Location = new System.Drawing.Point(287, 26);
            this.cbChartFeature.Name = "cbChartFeature";
            this.cbChartFeature.Size = new System.Drawing.Size(200, 23);
            this.cbChartFeature.TabIndex = 1;
            this.cbChartFeature.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            this.cbChartFeature.TextUpdate += new System.EventHandler(this.control_Changed);
            // 
            // lblChartFeature
            // 
            this.lblChartFeature.AutoSize = true;
            this.lblChartFeature.Location = new System.Drawing.Point(10, 30);
            this.lblChartFeature.Name = "lblChartFeature";
            this.lblChartFeature.Size = new System.Drawing.Size(106, 15);
            this.lblChartFeature.TabIndex = 0;
            this.lblChartFeature.Text = "Plugin for charting";
            // 
            // CtrlPluginAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbPluginAssignment);
            this.Name = "CtrlPluginAssignment";
            this.Size = new System.Drawing.Size(550, 550);
            this.Load += new System.EventHandler(this.CtrlPluginAssignment_Load);
            this.gbPluginAssignment.ResumeLayout(false);
            this.gbPluginAssignment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbPluginAssignment;
        private Label lblCommandFeature;
        private Label lblChartFeature;
        private Label lblNotificationFeature;
        private Label lblUserManagementFeature;
        private Label lblEventAckFeature;
        private ComboBox cbNotificationFeature;
        private ComboBox cbUserManagementFeature;
        private ComboBox cbEventAckFeature;
        private ComboBox cbCommandFeature;
        private ComboBox cbChartFeature;
    }
}
