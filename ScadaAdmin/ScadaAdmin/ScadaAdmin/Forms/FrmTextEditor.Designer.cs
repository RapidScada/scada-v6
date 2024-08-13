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
            richTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // richTextBox
            // 
            richTextBox.AcceptsTab = true;
            richTextBox.BorderStyle = BorderStyle.None;
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Font = new Font("Consolas", 9.75F);
            richTextBox.Location = new Point(0, 0);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new Size(584, 361);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            richTextBox.WordWrap = false;
            richTextBox.TextChanged += richTextBox_TextChanged;
            // 
            // FrmTextEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 361);
            Controls.Add(richTextBox);
            Name = "FrmTextEditor";
            Text = "FrmTextEditor";
            Load += FrmTextEditor_Load;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox;
    }
}