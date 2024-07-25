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
            toolStrip = new ToolStrip();
            btnAddObject = new ToolStripButton();
            btnMoveUp = new ToolStripButton();
            btnMoveDown = new ToolStripButton();
            btnMoveOut = new ToolStripButton();
            btnMoveInto = new ToolStripButton();
            btnDeleteObject = new ToolStripButton();
            btnFind = new ToolStripButton();
            separator1 = new ToolStripSeparator();
            separator2 = new ToolStripSeparator();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAddObject, btnDeleteObject, separator1, btnMoveUp, btnMoveDown, btnMoveOut, btnMoveInto, separator2, btnFind });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(800, 25);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip1";
            // 
            // btnAddObject
            // 
            btnAddObject.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddObject.Image = Properties.Resources.add;
            btnAddObject.ImageTransparentColor = Color.Magenta;
            btnAddObject.Name = "btnAddObject";
            btnAddObject.Size = new Size(23, 22);
            btnAddObject.ToolTipText = "Add Object";
            // 
            // btnMoveUp
            // 
            btnMoveUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveUp.Image = Properties.Resources.move_up;
            btnMoveUp.ImageTransparentColor = Color.Magenta;
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(23, 22);
            btnMoveUp.ToolTipText = "Move Up";
            // 
            // btnMoveDown
            // 
            btnMoveDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveDown.Image = Properties.Resources.move_down;
            btnMoveDown.ImageTransparentColor = Color.Magenta;
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(23, 22);
            btnMoveDown.ToolTipText = "Move Down";
            // 
            // btnMoveOut
            // 
            btnMoveOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveOut.Image = Properties.Resources.move_out;
            btnMoveOut.ImageTransparentColor = Color.Magenta;
            btnMoveOut.Name = "btnMoveOut";
            btnMoveOut.Size = new Size(23, 22);
            btnMoveOut.ToolTipText = "Move Out of Current Parent";
            // 
            // btnMoveInto
            // 
            btnMoveInto.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveInto.Image = Properties.Resources.move_into;
            btnMoveInto.ImageTransparentColor = Color.Magenta;
            btnMoveInto.Name = "btnMoveInto";
            btnMoveInto.Size = new Size(23, 22);
            btnMoveInto.ToolTipText = "Move Into Next Parent";
            // 
            // btnDeleteObject
            // 
            btnDeleteObject.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDeleteObject.Image = Properties.Resources.delete;
            btnDeleteObject.ImageTransparentColor = Color.Magenta;
            btnDeleteObject.Name = "btnDeleteObject";
            btnDeleteObject.Size = new Size(23, 22);
            btnDeleteObject.ToolTipText = "Delete Object";
            // 
            // btnFind
            // 
            btnFind.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnFind.Image = Properties.Resources.find;
            btnFind.ImageTransparentColor = Color.Magenta;
            btnFind.Name = "btnFind";
            btnFind.Size = new Size(23, 22);
            btnFind.ToolTipText = "Find";
            // 
            // separator1
            // 
            separator1.Name = "separator1";
            separator1.Size = new Size(6, 25);
            // 
            // separator2
            // 
            separator2.Name = "separator2";
            separator2.Size = new Size(6, 25);
            // 
            // FrmObjectEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(toolStrip);
            Name = "FrmObjectEditor";
            Text = "Object Editor";
            FormClosed += FrmObjectEditor_FormClosed;
            Load += FrmObjectEditor_Load;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripButton btnAddObject;
        private ToolStripButton btnDeleteObject;
        private ToolStripButton btnMoveUp;
        private ToolStripButton btnMoveDown;
        private ToolStripButton btnMoveOut;
        private ToolStripButton btnMoveInto;
        private ToolStripButton btnFind;
        private ToolStripSeparator separator1;
        private ToolStripSeparator separator2;
    }
}