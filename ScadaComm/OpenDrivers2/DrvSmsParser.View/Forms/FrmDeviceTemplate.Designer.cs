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
            pgTags = new TabPage();
            lblDataTagsHelp = new Label();
            txtTags = new TextBox();
            pgScript = new TabPage();
            txtScript = new TextBox();
            pnlBottom = new Panel();
            btnCancel = new Button();
            btnOK = new Button();
            tabControl.SuspendLayout();
            pgTags.SuspendLayout();
            pgScript.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(pgTags);
            tabControl.Controls.Add(pgScript);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(484, 416);
            tabControl.TabIndex = 0;
            // 
            // pgTags
            // 
            pgTags.Controls.Add(lblDataTagsHelp);
            pgTags.Controls.Add(txtTags);
            pgTags.Location = new Point(4, 24);
            pgTags.Name = "pgTags";
            pgTags.Padding = new Padding(5);
            pgTags.Size = new Size(476, 388);
            pgTags.TabIndex = 0;
            pgTags.Text = "Tags";
            pgTags.UseVisualStyleBackColor = true;
            // 
            // lblDataTagsHelp
            // 
            lblDataTagsHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblDataTagsHelp.AutoSize = true;
            lblDataTagsHelp.ForeColor = SystemColors.GrayText;
            lblDataTagsHelp.Location = new Point(5, 368);
            lblDataTagsHelp.Name = "lblDataTagsHelp";
            lblDataTagsHelp.Size = new Size(168, 15);
            lblDataTagsHelp.TabIndex = 1;
            lblDataTagsHelp.Text = "One tag per line: [Code] Name";
            // 
            // txtTags
            // 
            txtTags.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtTags.Location = new Point(8, 8);
            txtTags.Multiline = true;
            txtTags.Name = "txtTags";
            txtTags.ScrollBars = ScrollBars.Vertical;
            txtTags.Size = new Size(460, 357);
            txtTags.TabIndex = 0;
            txtTags.WordWrap = false;
            // 
            // pgScript
            // 
            pgScript.Controls.Add(txtScript);
            pgScript.Location = new Point(4, 24);
            pgScript.Name = "pgScript";
            pgScript.Padding = new Padding(5);
            pgScript.Size = new Size(476, 388);
            pgScript.TabIndex = 1;
            pgScript.Text = "Script";
            pgScript.UseVisualStyleBackColor = true;
            // 
            // txtScript
            // 
            txtScript.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtScript.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtScript.Location = new Point(8, 8);
            txtScript.Multiline = true;
            txtScript.Name = "txtScript";
            txtScript.ScrollBars = ScrollBars.Both;
            txtScript.Size = new Size(460, 372);
            txtScript.TabIndex = 0;
            txtScript.WordWrap = false;
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
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.Location = new Point(316, 10);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
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
            MinimumSize = new Size(300, 200);
            Name = "FrmDeviceTemplate";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Device Template";
            Load += FrmDeviceTemplate_Load;
            tabControl.ResumeLayout(false);
            pgTags.ResumeLayout(false);
            pgTags.PerformLayout();
            pgScript.ResumeLayout(false);
            pgScript.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage pgTags;
        private TabPage pgScript;
        private Panel pnlBottom;
        private Button btnCancel;
        private Button btnOK;
        private TextBox txtTags;
        private Label lblDataTagsHelp;
        private TextBox txtScript;
    }
}