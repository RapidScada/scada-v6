
namespace Scada.Forms.Forms
{
    partial class FrmBitmask
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
            btnOK = new Button();
            btnCancel = new Button();
            ctrlBitmask = new Controls.CtrlBitmask();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 437);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 437);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // ctrlBitmask
            // 
            ctrlBitmask.Location = new Point(12, 9);
            ctrlBitmask.Margin = new Padding(3, 0, 3, 3);
            ctrlBitmask.MaskBits = null;
            ctrlBitmask.MaskValue = 0;
            ctrlBitmask.Name = "ctrlBitmask";
            ctrlBitmask.Size = new Size(360, 412);
            ctrlBitmask.TabIndex = 0;
            // 
            // FrmBitmask
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 472);
            Controls.Add(ctrlBitmask);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBitmask";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Bit Mask";
            Load += FrmBitmask_Load;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Scada.Forms.Controls.CtrlBitmask ctrlBitmask;
    }
}