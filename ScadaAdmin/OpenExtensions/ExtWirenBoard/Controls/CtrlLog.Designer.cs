namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    partial class CtrlLog
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
            this.pnlLogHolder = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.pnlLogHolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLogHolder
            // 
            this.pnlLogHolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLogHolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLogHolder.Controls.Add(this.txtLog);
            this.pnlLogHolder.Location = new System.Drawing.Point(12, 12);
            this.pnlLogHolder.Name = "pnlLogHolder";
            this.pnlLogHolder.Size = new System.Drawing.Size(476, 176);
            this.pnlLogHolder.TabIndex = 1;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(474, 174);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            this.txtLog.WordWrap = false;
            // 
            // CtrlLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlLogHolder);
            this.Name = "CtrlLog";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.Size = new System.Drawing.Size(500, 200);
            this.pnlLogHolder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlLogHolder;
        private RichTextBox txtLog;
    }
}
