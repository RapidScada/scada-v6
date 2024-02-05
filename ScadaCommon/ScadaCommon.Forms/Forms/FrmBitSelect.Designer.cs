namespace Scada.Forms.Forms
{
    partial class FrmBitSelect
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
            lbBits = new CheckedListBox();
            btnCancel = new Button();
            btnOK = new Button();
            SuspendLayout();
            // 
            // lbBits
            // 
            lbBits.CheckOnClick = true;
            lbBits.FormattingEnabled = true;
            lbBits.IntegralHeight = false;
            lbBits.Location = new Point(12, 12);
            lbBits.Name = "lbBits";
            lbBits.Size = new Size(360, 350);
            lbBits.TabIndex = 0;
            lbBits.ItemCheck += lbBits_ItemCheck;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 378);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 378);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // FrmBitSelect
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 413);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(lbBits);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBitSelect";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Bit";
            Load += FrmBitSelect_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.CheckedListBox lbBits;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}