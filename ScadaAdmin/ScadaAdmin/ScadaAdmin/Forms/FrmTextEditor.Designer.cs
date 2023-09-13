namespace Scada.Admin.App.Forms
{
    partial class FrmTextEditor
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
            richTextBox = new System.Windows.Forms.RichTextBox();
            toolStrip = new System.Windows.Forms.ToolStrip();
            btnReload = new System.Windows.Forms.ToolStripButton();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox
            // 
            richTextBox.AcceptsTab = true;
            richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            richTextBox.Location = new System.Drawing.Point(0, 25);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new System.Drawing.Size(584, 336);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            richTextBox.WordWrap = false;
            richTextBox.TextChanged += richTextBox_TextChanged;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnReload });
            toolStrip.Location = new System.Drawing.Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new System.Drawing.Size(584, 25);
            toolStrip.TabIndex = 1;
            // 
            // btnReload
            // 
            btnReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnReload.Image = Properties.Resources.refresh;
            btnReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnReload.Name = "btnReload";
            btnReload.Size = new System.Drawing.Size(23, 22);
            btnReload.ToolTipText = "Reload File";
            btnReload.Click += btnReload_Click;
            // 
            // FrmTextEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(584, 361);
            Controls.Add(richTextBox);
            Controls.Add(toolStrip);
            Name = "FrmTextEditor";
            Text = "FrmTextEditor";
            Load += FrmTextEditor_Load;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnReload;
    }
}