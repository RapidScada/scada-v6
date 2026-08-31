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
            gbCommLine = new GroupBox();
            numDataLifetime = new NumericUpDown();
            lblDataLifetime = new Label();
            gbDevice = new GroupBox();
            btnOK = new Button();
            btnCancel = new Button();
            txtTemplateFileName = new Label();
            textBox1 = new TextBox();
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
            gbDevice.Controls.Add(textBox1);
            gbDevice.Controls.Add(txtTemplateFileName);
            gbDevice.Location = new Point(12, 91);
            gbDevice.Name = "gbDevice";
            gbDevice.Padding = new Padding(10, 3, 10, 10);
            gbDevice.Size = new Size(410, 73);
            gbDevice.TabIndex = 1;
            gbDevice.TabStop = false;
            gbDevice.Text = "Device";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(266, 180);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
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
            // txtTemplateFileName
            // 
            txtTemplateFileName.AutoSize = true;
            txtTemplateFileName.Location = new Point(13, 19);
            txtTemplateFileName.Name = "txtTemplateFileName";
            txtTemplateFileName.Size = new Size(92, 15);
            txtTemplateFileName.TabIndex = 0;
            txtTemplateFileName.Text = "Device template";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(13, 37);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 23);
            textBox1.TabIndex = 1;
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
            StartPosition = FormStartPosition.CenterParent;
            Text = "Device {0} Properties";
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
        private Label txtTemplateFileName;
        private TextBox textBox1;
    }
}