
namespace Scada.Admin.App.Controls.Tables
{
    partial class CtrlBitMask
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
            this.btnResetMask = new System.Windows.Forms.Button();
            this.lbMaskBits = new System.Windows.Forms.CheckedListBox();
            this.lblMaskBits = new System.Windows.Forms.Label();
            this.txtMaskValue = new System.Windows.Forms.TextBox();
            this.lblMaskValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnResetMask
            // 
            this.btnResetMask.Location = new System.Drawing.Point(285, 18);
            this.btnResetMask.Name = "btnResetMask";
            this.btnResetMask.Size = new System.Drawing.Size(75, 23);
            this.btnResetMask.TabIndex = 7;
            this.btnResetMask.Text = "Reset";
            this.btnResetMask.UseVisualStyleBackColor = true;
            this.btnResetMask.Click += new System.EventHandler(this.btnResetMask_Click);
            // 
            // lbMaskBits
            // 
            this.lbMaskBits.CheckOnClick = true;
            this.lbMaskBits.FormattingEnabled = true;
            this.lbMaskBits.IntegralHeight = false;
            this.lbMaskBits.Location = new System.Drawing.Point(0, 62);
            this.lbMaskBits.Name = "lbMaskBits";
            this.lbMaskBits.Size = new System.Drawing.Size(360, 349);
            this.lbMaskBits.TabIndex = 9;
            this.lbMaskBits.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbMaskBits_ItemCheck);
            // 
            // lblMaskBits
            // 
            this.lblMaskBits.AutoSize = true;
            this.lblMaskBits.Location = new System.Drawing.Point(-3, 44);
            this.lblMaskBits.Name = "lblMaskBits";
            this.lblMaskBits.Size = new System.Drawing.Size(26, 15);
            this.lblMaskBits.TabIndex = 8;
            this.lblMaskBits.Text = "Bits";
            // 
            // txtMaskValue
            // 
            this.txtMaskValue.Location = new System.Drawing.Point(0, 18);
            this.txtMaskValue.Name = "txtMaskValue";
            this.txtMaskValue.ReadOnly = true;
            this.txtMaskValue.Size = new System.Drawing.Size(279, 23);
            this.txtMaskValue.TabIndex = 6;
            // 
            // lblMaskValue
            // 
            this.lblMaskValue.AutoSize = true;
            this.lblMaskValue.Location = new System.Drawing.Point(-3, 0);
            this.lblMaskValue.Name = "lblMaskValue";
            this.lblMaskValue.Size = new System.Drawing.Size(81, 15);
            this.lblMaskValue.TabIndex = 5;
            this.lblMaskValue.Text = "Decimal value";
            // 
            // CtrlBitMask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnResetMask);
            this.Controls.Add(this.lbMaskBits);
            this.Controls.Add(this.lblMaskBits);
            this.Controls.Add(this.txtMaskValue);
            this.Controls.Add(this.lblMaskValue);
            this.Name = "CtrlBitMask";
            this.Size = new System.Drawing.Size(360, 411);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnResetMask;
        private System.Windows.Forms.CheckedListBox lbMaskBits;
        private System.Windows.Forms.Label lblMaskBits;
        private System.Windows.Forms.TextBox txtMaskValue;
        private System.Windows.Forms.Label lblMaskValue;
    }
}
