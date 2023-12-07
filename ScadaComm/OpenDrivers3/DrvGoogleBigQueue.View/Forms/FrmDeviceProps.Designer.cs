namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View.Forms
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
            gbDevice = new GroupBox();
            btnBrowseTemplate = new Button();
            btnEditTemplate = new Button();
            txtTemplateFileName = new TextBox();
            lblTemplateFileName = new Label();
            gbCommLine = new GroupBox();
            lblLocation = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            openFileDialog = new OpenFileDialog();
            txtLocation = new TextBox();
            gbDevice.SuspendLayout();
            gbCommLine.SuspendLayout();
            SuspendLayout();
            // 
            // gbDevice
            // 
            gbDevice.Controls.Add(btnBrowseTemplate);
            gbDevice.Controls.Add(btnEditTemplate);
            gbDevice.Controls.Add(txtTemplateFileName);
            gbDevice.Controls.Add(lblTemplateFileName);
            gbDevice.Location = new Point(12, 103);
            gbDevice.Name = "gbDevice";
            gbDevice.Padding = new Padding(10, 3, 10, 11);
            gbDevice.Size = new Size(410, 83);
            gbDevice.TabIndex = 1;
            gbDevice.TabStop = false;
            gbDevice.Text = "Device";
            // 
            // btnBrowseTemplate
            // 
            btnBrowseTemplate.Location = new Point(322, 42);
            btnBrowseTemplate.Name = "btnBrowseTemplate";
            btnBrowseTemplate.Size = new Size(75, 26);
            btnBrowseTemplate.TabIndex = 3;
            btnBrowseTemplate.Text = "Browse...";
            btnBrowseTemplate.UseVisualStyleBackColor = true;
            btnBrowseTemplate.Click += btnBrowse_Click;
            // 
            // btnEditTemplate
            // 
            btnEditTemplate.Location = new Point(241, 42);
            btnEditTemplate.Name = "btnEditTemplate";
            btnEditTemplate.Size = new Size(75, 26);
            btnEditTemplate.TabIndex = 2;
            btnEditTemplate.Text = "Edit";
            btnEditTemplate.UseVisualStyleBackColor = true;
            btnEditTemplate.Click += btnEdit_Click;
            // 
            // txtTemplateFileName
            // 
            txtTemplateFileName.Location = new Point(13, 42);
            txtTemplateFileName.Name = "txtTemplateFileName";
            txtTemplateFileName.Size = new Size(222, 23);
            txtTemplateFileName.TabIndex = 1;
            // 
            // lblTemplateFileName
            // 
            lblTemplateFileName.AutoSize = true;
            lblTemplateFileName.Location = new Point(10, 22);
            lblTemplateFileName.Name = "lblTemplateFileName";
            lblTemplateFileName.Size = new Size(101, 17);
            lblTemplateFileName.TabIndex = 0;
            lblTemplateFileName.Text = "Device template";
            // 
            // gbCommLine
            // 
            gbCommLine.Controls.Add(txtLocation);
            gbCommLine.Controls.Add(lblLocation);
            gbCommLine.Location = new Point(12, 14);
            gbCommLine.Name = "gbCommLine";
            gbCommLine.Padding = new Padding(10, 3, 10, 11);
            gbCommLine.Size = new Size(410, 83);
            gbCommLine.TabIndex = 0;
            gbCommLine.TabStop = false;
            gbCommLine.Text = "Communication Line";
            // 
            // lblLocation
            // 
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(10, 22);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(57, 17);
            lblLocation.TabIndex = 0;
            lblLocation.Text = "Location";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(266, 204);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 26);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(347, 204);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 26);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            openFileDialog.DefaultExt = "*.xml";
            openFileDialog.Filter = "Template Files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            // 
            // txtLocation
            // 
            txtLocation.Location = new Point(13, 42);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(384, 23);
            txtLocation.TabIndex = 4;
            // 
            // FrmDeviceProps
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(434, 244);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(gbDevice);
            Controls.Add(gbCommLine);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDeviceProps";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Device {0} Properties";
            Load += FrmDevProps_Load;
            gbDevice.ResumeLayout(false);
            gbDevice.PerformLayout();
            gbCommLine.ResumeLayout(false);
            gbCommLine.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.TextBox txtTemplateFileName;
        private System.Windows.Forms.Label lblTemplateFileName;
        private System.Windows.Forms.GroupBox gbCommLine;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.Button btnEditTemplate;
        private TextBox txtLocation;
    }
}