namespace Scada.Comm.Drivers.DrvModbus.View.Forms
{
    partial class FrmDeviceProps
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
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.btnBrowseTemplate = new System.Windows.Forms.Button();
            this.btnEditTemplate = new System.Windows.Forms.Button();
            this.txtTemplateFileName = new System.Windows.Forms.TextBox();
            this.lblTemplateFileName = new System.Windows.Forms.Label();
            this.gbCommLine = new System.Windows.Forms.GroupBox();
            this.cbTransMode = new System.Windows.Forms.ComboBox();
            this.lblTransMode = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.gbDevice.SuspendLayout();
            this.gbCommLine.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDevice
            // 
            this.gbDevice.Controls.Add(this.btnBrowseTemplate);
            this.gbDevice.Controls.Add(this.btnEditTemplate);
            this.gbDevice.Controls.Add(this.txtTemplateFileName);
            this.gbDevice.Controls.Add(this.lblTemplateFileName);
            this.gbDevice.Location = new System.Drawing.Point(12, 91);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevice.Size = new System.Drawing.Size(410, 73);
            this.gbDevice.TabIndex = 1;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Device";
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Location = new System.Drawing.Point(322, 37);
            this.btnBrowseTemplate.Name = "btnBrowseTemplate";
            this.btnBrowseTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTemplate.TabIndex = 3;
            this.btnBrowseTemplate.Text = "Browse...";
            this.btnBrowseTemplate.UseVisualStyleBackColor = true;
            this.btnBrowseTemplate.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnEditTemplate
            // 
            this.btnEditTemplate.Location = new System.Drawing.Point(241, 37);
            this.btnEditTemplate.Name = "btnEditTemplate";
            this.btnEditTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnEditTemplate.TabIndex = 2;
            this.btnEditTemplate.Text = "Edit";
            this.btnEditTemplate.UseVisualStyleBackColor = true;
            this.btnEditTemplate.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // txtTemplateFileName
            // 
            this.txtTemplateFileName.Location = new System.Drawing.Point(13, 37);
            this.txtTemplateFileName.Name = "txtTemplateFileName";
            this.txtTemplateFileName.Size = new System.Drawing.Size(222, 23);
            this.txtTemplateFileName.TabIndex = 1;
            // 
            // lblTemplateFileName
            // 
            this.lblTemplateFileName.AutoSize = true;
            this.lblTemplateFileName.Location = new System.Drawing.Point(10, 19);
            this.lblTemplateFileName.Name = "lblTemplateFileName";
            this.lblTemplateFileName.Size = new System.Drawing.Size(92, 15);
            this.lblTemplateFileName.TabIndex = 0;
            this.lblTemplateFileName.Text = "Device template";
            // 
            // gbCommLine
            // 
            this.gbCommLine.Controls.Add(this.cbTransMode);
            this.gbCommLine.Controls.Add(this.lblTransMode);
            this.gbCommLine.Location = new System.Drawing.Point(12, 12);
            this.gbCommLine.Name = "gbCommLine";
            this.gbCommLine.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommLine.Size = new System.Drawing.Size(410, 73);
            this.gbCommLine.TabIndex = 0;
            this.gbCommLine.TabStop = false;
            this.gbCommLine.Text = "Communication Line";
            // 
            // cbTransMode
            // 
            this.cbTransMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTransMode.FormattingEnabled = true;
            this.cbTransMode.Items.AddRange(new object[] {
            "Modbus RTU",
            "Modbus ASCII",
            "Modbus TCP"});
            this.cbTransMode.Location = new System.Drawing.Point(13, 37);
            this.cbTransMode.Name = "cbTransMode";
            this.cbTransMode.Size = new System.Drawing.Size(384, 23);
            this.cbTransMode.TabIndex = 1;
            // 
            // lblTransMode
            // 
            this.lblTransMode.AutoSize = true;
            this.lblTransMode.Location = new System.Drawing.Point(10, 19);
            this.lblTransMode.Name = "lblTransMode";
            this.lblTransMode.Size = new System.Drawing.Size(52, 15);
            this.lblTransMode.TabIndex = 0;
            this.lblTransMode.Text = "Protocol";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(266, 180);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(347, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.xml";
            this.openFileDialog.Filter = "Template Files (*.xml)|*.xml|All Files (*.*)|*.*";
            this.openFileDialog.FilterIndex = 0;
            // 
            // FrmDeviceProps
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 215);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbDevice);
            this.Controls.Add(this.gbCommLine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDeviceProps";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device {0} Properties";
            this.Load += new System.EventHandler(this.FrmDevProps_Load);
            this.gbDevice.ResumeLayout(false);
            this.gbDevice.PerformLayout();
            this.gbCommLine.ResumeLayout(false);
            this.gbCommLine.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.TextBox txtTemplateFileName;
        private System.Windows.Forms.Label lblTemplateFileName;
        private System.Windows.Forms.GroupBox gbCommLine;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbTransMode;
        private System.Windows.Forms.Label lblTransMode;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.Button btnEditTemplate;
    }
}