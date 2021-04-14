namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmTextDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblLength = new System.Windows.Forms.Label();
            this.txtPlainText = new System.Windows.Forms.RichTextBox();
            this.lblLine = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(616, 526);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(697, 526);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblLength
            // 
            this.lblLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLength.AutoSize = true;
            this.lblLength.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLength.Location = new System.Drawing.Point(100, 530);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(97, 15);
            this.lblLength.TabIndex = 1;
            this.lblLength.Text = "Length: 0 of 1000";
            // 
            // txtPlainText
            // 
            this.txtPlainText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPlainText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPlainText.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPlainText.Location = new System.Drawing.Point(12, 12);
            this.txtPlainText.Name = "txtPlainText";
            this.txtPlainText.Size = new System.Drawing.Size(760, 508);
            this.txtPlainText.TabIndex = 0;
            this.txtPlainText.Text = "";
            this.txtPlainText.WordWrap = false;
            this.txtPlainText.SelectionChanged += new System.EventHandler(this.txtPlainText_SelectionChanged);
            this.txtPlainText.TextChanged += new System.EventHandler(this.txtPlainText_TextChanged);
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLine.Location = new System.Drawing.Point(12, 530);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(41, 15);
            this.lblLine.TabIndex = 4;
            this.lblLine.Text = "Line: 0";
            // 
            // FrmTextDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.txtPlainText);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FrmTextDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Text Editor";
            this.Load += new System.EventHandler(this.FrmEditSource_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.RichTextBox txtPlainText;
        private System.Windows.Forms.Label lblLine;
    }
}