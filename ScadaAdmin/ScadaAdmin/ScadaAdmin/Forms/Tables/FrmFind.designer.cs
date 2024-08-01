namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmFind
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
            btnReplaceAll = new Button();
            btnClose = new Button();
            lblColumn = new Label();
            cbColumn = new ComboBox();
            lblFind = new Label();
            txtFind = new TextBox();
            lblReplaceWith = new Label();
            txtReplaceWith = new TextBox();
            chkCaseSensitive = new CheckBox();
            chkWholeCellOnly = new CheckBox();
            cbFind = new ComboBox();
            cbReplaceWith = new ComboBox();
            btnFindNext = new Button();
            btnReplace = new Button();
            SuspendLayout();
            // 
            // btnReplaceAll
            // 
            btnReplaceAll.Location = new Point(282, 194);
            btnReplaceAll.Name = "btnReplaceAll";
            btnReplaceAll.Size = new Size(90, 23);
            btnReplaceAll.TabIndex = 12;
            btnReplaceAll.Text = "Replace All";
            btnReplaceAll.UseVisualStyleBackColor = true;
            btnReplaceAll.Click += btnReplaceAll_Click;
            // 
            // btnClose
            // 
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(282, 223);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 23);
            btnClose.TabIndex = 13;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // lblColumn
            // 
            lblColumn.AutoSize = true;
            lblColumn.Location = new Point(9, 9);
            lblColumn.Name = "lblColumn";
            lblColumn.Size = new Size(50, 15);
            lblColumn.TabIndex = 0;
            lblColumn.Text = "Column";
            // 
            // cbColumn
            // 
            cbColumn.DropDownStyle = ComboBoxStyle.DropDownList;
            cbColumn.FormattingEnabled = true;
            cbColumn.Location = new Point(12, 27);
            cbColumn.Name = "cbColumn";
            cbColumn.Size = new Size(360, 23);
            cbColumn.TabIndex = 1;
            cbColumn.SelectedIndexChanged += cbColumn_SelectedIndexChanged;
            // 
            // lblFind
            // 
            lblFind.AutoSize = true;
            lblFind.Location = new Point(9, 53);
            lblFind.Name = "lblFind";
            lblFind.Size = new Size(59, 15);
            lblFind.TabIndex = 2;
            lblFind.Text = "Find what";
            // 
            // txtFind
            // 
            txtFind.Location = new Point(12, 71);
            txtFind.Name = "txtFind";
            txtFind.Size = new Size(360, 23);
            txtFind.TabIndex = 3;
            txtFind.TextChanged += txtFind_TextChanged;
            // 
            // lblReplaceWith
            // 
            lblReplaceWith.AutoSize = true;
            lblReplaceWith.Location = new Point(9, 97);
            lblReplaceWith.Name = "lblReplaceWith";
            lblReplaceWith.Size = new Size(74, 15);
            lblReplaceWith.TabIndex = 5;
            lblReplaceWith.Text = "Replace with";
            // 
            // txtReplaceWith
            // 
            txtReplaceWith.Location = new Point(12, 115);
            txtReplaceWith.Name = "txtReplaceWith";
            txtReplaceWith.Size = new Size(360, 23);
            txtReplaceWith.TabIndex = 6;
            // 
            // chkCaseSensitive
            // 
            chkCaseSensitive.AutoSize = true;
            chkCaseSensitive.Location = new Point(12, 144);
            chkCaseSensitive.Name = "chkCaseSensitive";
            chkCaseSensitive.Size = new Size(86, 19);
            chkCaseSensitive.TabIndex = 8;
            chkCaseSensitive.Text = "Match case";
            chkCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // chkWholeCellOnly
            // 
            chkWholeCellOnly.AutoSize = true;
            chkWholeCellOnly.Checked = true;
            chkWholeCellOnly.CheckState = CheckState.Checked;
            chkWholeCellOnly.Location = new Point(12, 169);
            chkWholeCellOnly.Name = "chkWholeCellOnly";
            chkWholeCellOnly.Size = new Size(142, 19);
            chkWholeCellOnly.TabIndex = 9;
            chkWholeCellOnly.Text = "Match whole cell only";
            chkWholeCellOnly.UseVisualStyleBackColor = true;
            // 
            // cbFind
            // 
            cbFind.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFind.FormattingEnabled = true;
            cbFind.Location = new Point(12, 81);
            cbFind.Name = "cbFind";
            cbFind.Size = new Size(360, 23);
            cbFind.TabIndex = 4;
            cbFind.Visible = false;
            cbFind.SelectedIndexChanged += cbFind_SelectedIndexChanged;
            // 
            // cbReplaceWith
            // 
            cbReplaceWith.DropDownStyle = ComboBoxStyle.DropDownList;
            cbReplaceWith.FormattingEnabled = true;
            cbReplaceWith.Location = new Point(12, 125);
            cbReplaceWith.Name = "cbReplaceWith";
            cbReplaceWith.Size = new Size(360, 23);
            cbReplaceWith.TabIndex = 7;
            cbReplaceWith.Visible = false;
            cbReplaceWith.SelectedIndexChanged += cbReplaceWith_SelectedIndexChanged;
            // 
            // btnFindNext
            // 
            btnFindNext.Location = new Point(90, 194);
            btnFindNext.Name = "btnFindNext";
            btnFindNext.Size = new Size(90, 23);
            btnFindNext.TabIndex = 10;
            btnFindNext.Text = "Find Next";
            btnFindNext.UseVisualStyleBackColor = true;
            btnFindNext.Click += btnFindNext_Click;
            // 
            // btnReplace
            // 
            btnReplace.Location = new Point(186, 194);
            btnReplace.Name = "btnReplace";
            btnReplace.Size = new Size(90, 23);
            btnReplace.TabIndex = 11;
            btnReplace.Text = "Replace";
            btnReplace.UseVisualStyleBackColor = true;
            btnReplace.Click += btnReplace_Click;
            // 
            // FrmFind
            // 
            AcceptButton = btnFindNext;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(384, 258);
            Controls.Add(btnReplace);
            Controls.Add(btnFindNext);
            Controls.Add(cbReplaceWith);
            Controls.Add(cbFind);
            Controls.Add(chkWholeCellOnly);
            Controls.Add(chkCaseSensitive);
            Controls.Add(txtReplaceWith);
            Controls.Add(lblReplaceWith);
            Controls.Add(txtFind);
            Controls.Add(lblFind);
            Controls.Add(cbColumn);
            Controls.Add(lblColumn);
            Controls.Add(btnReplaceAll);
            Controls.Add(btnClose);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmFind";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Find and Replace";
            Load += FrmReplace_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblColumn;
        private System.Windows.Forms.ComboBox cbColumn;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label lblReplaceWith;
        private System.Windows.Forms.TextBox txtReplaceWith;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.CheckBox chkWholeCellOnly;
        private System.Windows.Forms.ComboBox cbFind;
        private System.Windows.Forms.ComboBox cbReplaceWith;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnReplace;
    }
}