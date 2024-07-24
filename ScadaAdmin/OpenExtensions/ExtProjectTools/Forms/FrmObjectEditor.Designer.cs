namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    partial class FrmObjectEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmObjectEditor));
            toolStrip = new ToolStrip();
            btnAddObj = new ToolStripButton();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAddObj });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(800, 25);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip1";
            // 
            // btnAddObj
            // 
            btnAddObj.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddObj.Image = (Image)resources.GetObject("btnAddObj.Image");
            btnAddObj.ImageTransparentColor = Color.Magenta;
            btnAddObj.Name = "btnAddObj";
            btnAddObj.Size = new Size(23, 22);
            btnAddObj.Text = "toolStripButton1";
            // 
            // FrmObjectEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(toolStrip);
            Name = "FrmObjectEditor";
            Text = "FrmObjectEditor";
            FormClosed += FrmObjectEditor_FormClosed;
            Load += FrmObjectEditor_Load;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripButton btnAddObj;
    }
}