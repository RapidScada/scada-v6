namespace Scada.Comm.Drivers.DrvSmsParser.View.Forms
{
    partial class FrmDeviceProperties
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
            gbCommLine = new GroupBox();
            numDataLifetime = new NumericUpDown();
            lblDataLifetime = new Label();
            gbDevice = new GroupBox();
            btnBrowseTemplate = new Button();
            btnEditTemplate = new Button();
            btnNewTemplate = new Button();
            txtTemplateFileName = new TextBox();
            lblTemplateFileName = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            toolTip = new ToolTip(components);
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            gbCommLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDataLifetime).BeginInit();
            gbDevice.SuspendLayout();
            SuspendLayout();
            // 
            // gbCommLine
            // 
            gbCommLine.Controls.Add(numDataLifetime);
            gbCommLine.Controls.Add(lblDataLifetime);
            gbCommLine.Location = new Point(12, 12);
            gbCommLine.Name = "gbCommLine";
            gbCommLine.Padding = new Padding(10, 3, 10, 10);
            gbCommLine.Size = new Size(410, 73);
            gbCommLine.TabIndex = 0;
            gbCommLine.TabStop = false;
            gbCommLine.Text = "Communication Line";
            // 
            // numDataLifetime
            // 
            numDataLifetime.Location = new Point(13, 37);
            numDataLifetime.Maximum = new decimal(new int[] { 86400, 0, 0, 0 });
            numDataLifetime.Name = "numDataLifetime";
            numDataLifetime.Size = new Size(120, 23);
            numDataLifetime.TabIndex = 1;
            // 
            // lblDataLifetime
            // 
            lblDataLifetime.AutoSize = true;
            lblDataLifetime.Location = new Point(10, 19);
            lblDataLifetime.Name = "lblDataLifetime";
            lblDataLifetime.Size = new Size(97, 15);
            lblDataLifetime.TabIndex = 0;
            lblDataLifetime.Text = "Data lifetime, sec";
            // 
            // gbDevice
            // 
            gbDevice.Controls.Add(btnBrowseTemplate);
            gbDevice.Controls.Add(btnEditTemplate);
            gbDevice.Controls.Add(btnNewTemplate);
            gbDevice.Controls.Add(txtTemplateFileName);
            gbDevice.Controls.Add(lblTemplateFileName);
            gbDevice.Location = new Point(12, 91);
            gbDevice.Name = "gbDevice";
            gbDevice.Padding = new Padding(10, 3, 10, 10);
            gbDevice.Size = new Size(410, 73);
            gbDevice.TabIndex = 1;
            gbDevice.TabStop = false;
            gbDevice.Text = "Device";
            // 
            // btnBrowseTemplate
            // 
            btnBrowseTemplate.FlatStyle = FlatStyle.Popup;
            btnBrowseTemplate.Image = Properties.Resources.open;
            btnBrowseTemplate.Location = new Point(374, 37);
            btnBrowseTemplate.Name = "btnBrowseTemplate";
            btnBrowseTemplate.Size = new Size(23, 23);
            btnBrowseTemplate.TabIndex = 4;
            btnBrowseTemplate.UseVisualStyleBackColor = true;
            btnBrowseTemplate.Click += btnBrowseTemplate_Click;
            // 
            // btnEditTemplate
            // 
            btnEditTemplate.FlatStyle = FlatStyle.Popup;
            btnEditTemplate.Image = Properties.Resources.edit;
            btnEditTemplate.Location = new Point(345, 37);
            btnEditTemplate.Name = "btnEditTemplate";
            btnEditTemplate.Size = new Size(23, 23);
            btnEditTemplate.TabIndex = 3;
            btnEditTemplate.UseVisualStyleBackColor = true;
            btnEditTemplate.Click += btnEditTemplate_Click;
            // 
            // btnNewTemplate
            // 
            btnNewTemplate.FlatStyle = FlatStyle.Popup;
            btnNewTemplate.Image = Properties.Resources.new_file;
            btnNewTemplate.Location = new Point(316, 37);
            btnNewTemplate.Name = "btnNewTemplate";
            btnNewTemplate.Size = new Size(23, 23);
            btnNewTemplate.TabIndex = 2;
            btnNewTemplate.UseVisualStyleBackColor = true;
            btnNewTemplate.Click += btnNewTemplate_Click;
            // 
            // txtTemplateFileName
            // 
            txtTemplateFileName.Location = new Point(13, 37);
            txtTemplateFileName.Name = "txtTemplateFileName";
            txtTemplateFileName.Size = new Size(297, 23);
            txtTemplateFileName.TabIndex = 1;
            // 
            // lblTemplateFileName
            // 
            lblTemplateFileName.AutoSize = true;
            lblTemplateFileName.Location = new Point(10, 19);
            lblTemplateFileName.Name = "lblTemplateFileName";
            lblTemplateFileName.Size = new Size(92, 15);
            lblTemplateFileName.TabIndex = 0;
            lblTemplateFileName.Text = "Device template";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(266, 180);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(347, 180);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            openFileDialog.DefaultExt = "*.xml";
            openFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "*.xml";
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            // 
            // FrmDeviceProperties
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(434, 215);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(gbDevice);
            Controls.Add(gbCommLine);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDeviceProperties";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Device {0} Properties";
            Load += FrmDeviceProperties_Load;
            gbCommLine.ResumeLayout(false);
            gbCommLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDataLifetime).EndInit();
            gbDevice.ResumeLayout(false);
            gbDevice.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbCommLine;
        private GroupBox gbDevice;
        private Button btnOK;
        private Button btnCancel;
        private NumericUpDown numDataLifetime;
        private Label lblDataLifetime;
        private Label lblTemplateFileName;
        private TextBox txtTemplateFileName;
        private Button btnBrowseTemplate;
        private Button btnEditTemplate;
        private Button btnNewTemplate;
        private ToolTip toolTip;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
    }
}