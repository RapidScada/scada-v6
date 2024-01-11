namespace Scada.Forms.Forms
{
    partial class FrmRangeEdit
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
            btnCancel = new Button();
            btnOK = new Button();
            lblRange = new Label();
            txtRange = new TextBox();
            lblExample = new Label();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(247, 176);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.Location = new Point(166, 176);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Location = new Point(9, 9);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(144, 15);
            lblRange.TabIndex = 0;
            lblRange.Text = "Range of integer numbers";
            // 
            // txtRange
            // 
            txtRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtRange.Location = new Point(12, 27);
            txtRange.Multiline = true;
            txtRange.Name = "txtRange";
            txtRange.ScrollBars = ScrollBars.Vertical;
            txtRange.Size = new Size(310, 128);
            txtRange.TabIndex = 1;
            // 
            // lblExample
            // 
            lblExample.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblExample.AutoSize = true;
            lblExample.ForeColor = SystemColors.GrayText;
            lblExample.Location = new Point(9, 158);
            lblExample.Name = "lblExample";
            lblExample.Size = new Size(119, 15);
            lblExample.TabIndex = 2;
            lblExample.Text = "For example: 1 - 5, 10";
            // 
            // FrmRangeEdit
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(334, 211);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(lblExample);
            Controls.Add(txtRange);
            Controls.Add(lblRange);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(300, 200);
            Name = "FrmRangeEdit";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Range";
            Load += FrmRangeEdit_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.TextBox txtRange;
        private System.Windows.Forms.Label lblExample;
    }
}