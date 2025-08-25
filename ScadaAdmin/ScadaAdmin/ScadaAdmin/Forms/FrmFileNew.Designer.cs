namespace Scada.Admin.App.Forms
{
    partial class FrmFileNew
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
            txtFileName = new TextBox();
            lblFileName = new Label();
            lbFileType = new ListBox();
            btnCancel = new Button();
            btnOK = new Button();
            SuspendLayout();
            // 
            // txtFileName
            // 
            txtFileName.Location = new Point(12, 137);
            txtFileName.Name = "txtFileName";
            txtFileName.Size = new Size(360, 23);
            txtFileName.TabIndex = 2;
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Location = new Point(9, 119);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(58, 15);
            lblFileName.TabIndex = 1;
            lblFileName.Text = "File name";
            // 
            // lbFileType
            // 
            lbFileType.FormattingEnabled = true;
            lbFileType.IntegralHeight = false;
            lbFileType.ItemHeight = 15;
            lbFileType.Items.AddRange(new object[] { "Scheme Classic", "Mimic Diagram (experimental)", "Faceplate (experimental)", "Table View", "XML File", "Text File" });
            lbFileType.Location = new Point(12, 12);
            lbFileType.Name = "lbFileType";
            lbFileType.Size = new Size(360, 104);
            lbFileType.TabIndex = 0;
            lbFileType.SelectedIndexChanged += lbFileType_SelectedIndexChanged;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(297, 176);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 176);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // FrmFileNew
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 211);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(lbFileType);
            Controls.Add(txtFileName);
            Controls.Add(lblFileName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmFileNew";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "New File";
            Load += FrmFileNew_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.ListBox lbFileType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}