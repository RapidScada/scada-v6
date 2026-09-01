namespace Scada.Comm.Drivers.DrvSmsParser.View.Forms
{
    partial class FrmDeviceTemplate
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
            tabControl = new TabControl();
            pgDataTags = new TabPage();
            pgScript = new TabPage();
            pnlBottom = new Panel();
            btnOK = new Button();
            btnCancel = new Button();
            txtDataTags = new TextBox();
            txtDataTagsHelp = new Label();
            textBox1 = new TextBox();
            tabControl.SuspendLayout();
            pgDataTags.SuspendLayout();
            pgScript.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(pgDataTags);
            tabControl.Controls.Add(pgScript);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(484, 416);
            tabControl.TabIndex = 0;
            // 
            // pgDataTags
            // 
            pgDataTags.Controls.Add(txtDataTagsHelp);
            pgDataTags.Controls.Add(txtDataTags);
            pgDataTags.Location = new Point(4, 24);
            pgDataTags.Name = "pgDataTags";
            pgDataTags.Padding = new Padding(5);
            pgDataTags.Size = new Size(476, 388);
            pgDataTags.TabIndex = 0;
            pgDataTags.Text = "Data Tags";
            pgDataTags.UseVisualStyleBackColor = true;
            // 
            // pgScript
            // 
            pgScript.Controls.Add(textBox1);
            pgScript.Location = new Point(4, 24);
            pgScript.Name = "pgScript";
            pgScript.Padding = new Padding(5);
            pgScript.Size = new Size(476, 388);
            pgScript.TabIndex = 1;
            pgScript.Text = "Script";
            pgScript.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 416);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(484, 45);
            pnlBottom.TabIndex = 1;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.Location = new Point(316, 10);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(397, 10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtDataTags
            // 
            txtDataTags.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtDataTags.Location = new Point(8, 8);
            txtDataTags.Multiline = true;
            txtDataTags.Name = "txtDataTags";
            txtDataTags.ScrollBars = ScrollBars.Vertical;
            txtDataTags.Size = new Size(460, 357);
            txtDataTags.TabIndex = 0;
            txtDataTags.WordWrap = false;
            // 
            // txtDataTagsHelp
            // 
            txtDataTagsHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtDataTagsHelp.AutoSize = true;
            txtDataTagsHelp.ForeColor = SystemColors.GrayText;
            txtDataTagsHelp.Location = new Point(5, 368);
            txtDataTagsHelp.Name = "txtDataTagsHelp";
            txtDataTagsHelp.Size = new Size(168, 15);
            txtDataTagsHelp.TabIndex = 1;
            txtDataTagsHelp.Text = "One tag per line: [Code] Name";
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox1.Location = new Point(8, 8);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(460, 372);
            textBox1.TabIndex = 0;
            textBox1.WordWrap = false;
            // 
            // FrmDeviceTemplate
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(484, 461);
            Controls.Add(tabControl);
            Controls.Add(pnlBottom);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDeviceTemplate";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Device Template";
            tabControl.ResumeLayout(false);
            pgDataTags.ResumeLayout(false);
            pgDataTags.PerformLayout();
            pgScript.ResumeLayout(false);
            pgScript.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage pgDataTags;
        private TabPage pgScript;
        private Panel pnlBottom;
        private Button btnCancel;
        private Button btnOK;
        private TextBox txtDataTags;
        private Label txtDataTagsHelp;
        private TextBox textBox1;
    }
}