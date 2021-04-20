
namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmBitMask
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
            this.lblMaskValue = new System.Windows.Forms.Label();
            this.txtMaskValue = new System.Windows.Forms.TextBox();
            this.lblMaskBits = new System.Windows.Forms.Label();
            this.lbMaskBits = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnResetMask = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMaskValue
            // 
            this.lblMaskValue.AutoSize = true;
            this.lblMaskValue.Location = new System.Drawing.Point(9, 9);
            this.lblMaskValue.Name = "lblMaskValue";
            this.lblMaskValue.Size = new System.Drawing.Size(81, 15);
            this.lblMaskValue.TabIndex = 0;
            this.lblMaskValue.Text = "Decimal value";
            // 
            // txtMaskValue
            // 
            this.txtMaskValue.Location = new System.Drawing.Point(12, 27);
            this.txtMaskValue.Name = "txtMaskValue";
            this.txtMaskValue.ReadOnly = true;
            this.txtMaskValue.Size = new System.Drawing.Size(279, 23);
            this.txtMaskValue.TabIndex = 1;
            // 
            // lblMaskBits
            // 
            this.lblMaskBits.AutoSize = true;
            this.lblMaskBits.Location = new System.Drawing.Point(9, 53);
            this.lblMaskBits.Name = "lblMaskBits";
            this.lblMaskBits.Size = new System.Drawing.Size(26, 15);
            this.lblMaskBits.TabIndex = 3;
            this.lblMaskBits.Text = "Bits";
            // 
            // lbMaskBits
            // 
            this.lbMaskBits.CheckOnClick = true;
            this.lbMaskBits.FormattingEnabled = true;
            this.lbMaskBits.IntegralHeight = false;
            this.lbMaskBits.Location = new System.Drawing.Point(12, 71);
            this.lbMaskBits.Name = "lbMaskBits";
            this.lbMaskBits.Size = new System.Drawing.Size(360, 349);
            this.lbMaskBits.TabIndex = 4;
            this.lbMaskBits.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbMaskBits_ItemCheck);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 426);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnResetMask
            // 
            this.btnResetMask.Location = new System.Drawing.Point(297, 27);
            this.btnResetMask.Name = "btnResetMask";
            this.btnResetMask.Size = new System.Drawing.Size(75, 23);
            this.btnResetMask.TabIndex = 2;
            this.btnResetMask.Text = "Reset";
            this.btnResetMask.UseVisualStyleBackColor = true;
            this.btnResetMask.Click += new System.EventHandler(this.btnResetMask_Click);
            // 
            // FrmBitMask
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.btnResetMask);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbMaskBits);
            this.Controls.Add(this.lblMaskBits);
            this.Controls.Add(this.txtMaskValue);
            this.Controls.Add(this.lblMaskValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBitMask";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bit Mask";
            this.Load += new System.EventHandler(this.FrmBitMask_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMaskValue;
        private System.Windows.Forms.TextBox txtMaskValue;
        private System.Windows.Forms.Label lblMaskBits;
        private System.Windows.Forms.CheckedListBox lbMaskBits;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnResetMask;
    }
}