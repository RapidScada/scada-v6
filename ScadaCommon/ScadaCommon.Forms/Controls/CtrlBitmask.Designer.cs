
namespace Scada.Forms.Controls
{
    partial class CtrlBitmask
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
            btnResetMask = new Button();
            lbMaskBits = new CheckedListBox();
            lblMaskBits = new Label();
            txtMaskValue = new TextBox();
            lblMaskValue = new Label();
            pnlBitMask = new Panel();
            pnlBitMask.SuspendLayout();
            SuspendLayout();
            // 
            // btnResetMask
            // 
            btnResetMask.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnResetMask.Location = new Point(285, 18);
            btnResetMask.Name = "btnResetMask";
            btnResetMask.Size = new Size(75, 23);
            btnResetMask.TabIndex = 7;
            btnResetMask.Text = "Reset";
            btnResetMask.UseVisualStyleBackColor = true;
            btnResetMask.Click += btnResetMask_Click;
            // 
            // lbMaskBits
            // 
            lbMaskBits.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lbMaskBits.CheckOnClick = true;
            lbMaskBits.FormattingEnabled = true;
            lbMaskBits.IntegralHeight = false;
            lbMaskBits.Location = new Point(0, 62);
            lbMaskBits.Name = "lbMaskBits";
            lbMaskBits.Size = new Size(360, 350);
            lbMaskBits.TabIndex = 9;
            lbMaskBits.ItemCheck += lbMaskBits_ItemCheck;
            // 
            // lblMaskBits
            // 
            lblMaskBits.AutoSize = true;
            lblMaskBits.Location = new Point(-3, 44);
            lblMaskBits.Name = "lblMaskBits";
            lblMaskBits.Size = new Size(26, 15);
            lblMaskBits.TabIndex = 8;
            lblMaskBits.Text = "Bits";
            // 
            // txtMaskValue
            // 
            txtMaskValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtMaskValue.Location = new Point(0, 18);
            txtMaskValue.Name = "txtMaskValue";
            txtMaskValue.ReadOnly = true;
            txtMaskValue.Size = new Size(279, 23);
            txtMaskValue.TabIndex = 6;
            // 
            // lblMaskValue
            // 
            lblMaskValue.AutoSize = true;
            lblMaskValue.Location = new Point(-3, 0);
            lblMaskValue.Name = "lblMaskValue";
            lblMaskValue.Size = new Size(81, 15);
            lblMaskValue.TabIndex = 5;
            lblMaskValue.Text = "Decimal value";
            // 
            // pnlBitMask
            // 
            pnlBitMask.Controls.Add(lbMaskBits);
            pnlBitMask.Controls.Add(lblMaskBits);
            pnlBitMask.Controls.Add(btnResetMask);
            pnlBitMask.Controls.Add(txtMaskValue);
            pnlBitMask.Controls.Add(lblMaskValue);
            pnlBitMask.Dock = DockStyle.Fill;
            pnlBitMask.Location = new Point(0, 0);
            pnlBitMask.Name = "pnlBitMask";
            pnlBitMask.Size = new Size(360, 412);
            pnlBitMask.TabIndex = 10;
            // 
            // CtrlBitmask
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlBitMask);
            Margin = new Padding(3, 0, 3, 3);
            Name = "CtrlBitmask";
            Size = new Size(360, 412);
            pnlBitMask.ResumeLayout(false);
            pnlBitMask.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnResetMask;
        private System.Windows.Forms.CheckedListBox lbMaskBits;
        private System.Windows.Forms.Label lblMaskBits;
        private System.Windows.Forms.TextBox txtMaskValue;
        private System.Windows.Forms.Label lblMaskValue;
        private System.Windows.Forms.Panel pnlBitMask;
    }
}
