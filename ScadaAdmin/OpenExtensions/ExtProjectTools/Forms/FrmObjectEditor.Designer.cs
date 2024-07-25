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
            btnDeleteObject = new ToolStripButton();
            btnRefreshData = new ToolStripButton();
            sep1 = new ToolStripSeparator();
            btnFind = new ToolStripButton();
            tvObj = new TreeView();
            pnlRight = new Panel();
            gbObj = new GroupBox();
            cbParentObj = new ComboBox();
            lblParentObj = new Label();
            txtCode = new TextBox();
            lblCode = new Label();
            txtName = new TextBox();
            lblName = new Label();
            numObjNum = new NumericUpDown();
            lblObjNum = new Label();
            toolStrip.SuspendLayout();
            pnlRight.SuspendLayout();
            gbObj.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numObjNum).BeginInit();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAddObject, btnDeleteObject, btnRefreshData, sep1, btnFind });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(784, 25);
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
            // btnDeleteObject
            // 
            btnDeleteObject.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDeleteObject.Image = Properties.Resources.delete;
            btnDeleteObject.ImageTransparentColor = Color.Magenta;
            btnDeleteObject.Name = "btnDeleteObject";
            btnDeleteObject.Size = new Size(23, 22);
            btnDeleteObject.ToolTipText = "Delete Object";
            // 
            // btnRefreshData
            // 
            btnRefreshData.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnRefreshData.Image = Properties.Resources.refresh;
            btnRefreshData.ImageTransparentColor = Color.Magenta;
            btnRefreshData.Name = "btnRefreshData";
            btnRefreshData.Size = new Size(23, 22);
            btnRefreshData.ToolTipText = "Refresh Data";
            // 
            // sep1
            // 
            sep1.Name = "sep1";
            sep1.Size = new Size(6, 25);
            // 
            // btnFind
            // 
            btnFind.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnFind.Image = Properties.Resources.find;
            btnFind.ImageTransparentColor = Color.Magenta;
            btnFind.Name = "btnFind";
            btnFind.Size = new Size(23, 22);
            btnFind.ToolTipText = "Find (Ctrl+F)";
            // 
            // tvObj
            // 
            tvObj.Dock = DockStyle.Fill;
            tvObj.Location = new Point(0, 25);
            tvObj.Name = "tvObj";
            tvObj.Size = new Size(484, 436);
            tvObj.TabIndex = 1;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(gbObj);
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Location = new Point(484, 25);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(12, 0, 12, 12);
            pnlRight.Size = new Size(300, 436);
            pnlRight.TabIndex = 2;
            // 
            // gbObj
            // 
            gbObj.Controls.Add(cbParentObj);
            gbObj.Controls.Add(lblParentObj);
            gbObj.Controls.Add(txtCode);
            gbObj.Controls.Add(lblCode);
            gbObj.Controls.Add(txtName);
            gbObj.Controls.Add(lblName);
            gbObj.Controls.Add(numObjNum);
            gbObj.Controls.Add(lblObjNum);
            gbObj.Dock = DockStyle.Fill;
            gbObj.Location = new Point(12, 0);
            gbObj.Name = "gbObj";
            gbObj.Padding = new Padding(10, 3, 10, 10);
            gbObj.Size = new Size(276, 424);
            gbObj.TabIndex = 0;
            gbObj.TabStop = false;
            gbObj.Text = "Object Properties";
            // 
            // cbParentObj
            // 
            cbParentObj.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParentObj.FormattingEnabled = true;
            cbParentObj.Location = new Point(13, 190);
            cbParentObj.Name = "cbParentObj";
            cbParentObj.Size = new Size(250, 23);
            cbParentObj.TabIndex = 7;
            // 
            // lblParentObj
            // 
            lblParentObj.AutoSize = true;
            lblParentObj.Location = new Point(10, 172);
            lblParentObj.Name = "lblParentObj";
            lblParentObj.Size = new Size(77, 15);
            lblParentObj.TabIndex = 6;
            lblParentObj.Text = "Parent object";
            // 
            // txtCode
            // 
            txtCode.Location = new Point(13, 139);
            txtCode.Margin = new Padding(3, 3, 3, 10);
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(250, 23);
            txtCode.TabIndex = 5;
            // 
            // lblCode
            // 
            lblCode.AutoSize = true;
            lblCode.Location = new Point(10, 121);
            lblCode.Name = "lblCode";
            lblCode.Size = new Size(35, 15);
            lblCode.TabIndex = 4;
            lblCode.Text = "Code";
            // 
            // txtName
            // 
            txtName.Location = new Point(13, 88);
            txtName.Margin = new Padding(3, 3, 3, 10);
            txtName.Name = "txtName";
            txtName.Size = new Size(250, 23);
            txtName.TabIndex = 3;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(10, 70);
            lblName.Name = "lblName";
            lblName.Size = new Size(39, 15);
            lblName.TabIndex = 2;
            lblName.Text = "Name";
            // 
            // numObjNum
            // 
            numObjNum.Location = new Point(13, 37);
            numObjNum.Margin = new Padding(3, 3, 3, 10);
            numObjNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numObjNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numObjNum.Name = "numObjNum";
            numObjNum.Size = new Size(120, 23);
            numObjNum.TabIndex = 1;
            numObjNum.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblObjNum
            // 
            lblObjNum.AutoSize = true;
            lblObjNum.Location = new Point(10, 19);
            lblObjNum.Name = "lblObjNum";
            lblObjNum.Size = new Size(51, 15);
            lblObjNum.TabIndex = 0;
            lblObjNum.Text = "Number";
            // 
            // FrmObjectEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(tvObj);
            Controls.Add(pnlRight);
            Controls.Add(toolStrip);
            KeyPreview = true;
            Name = "FrmObjectEditor";
            Text = "Object Editor";
            FormClosed += FrmObjectEditor_FormClosed;
            Load += FrmObjectEditor_Load;
            KeyDown += FrmObjectEditor_KeyDown;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            pnlRight.ResumeLayout(false);
            gbObj.ResumeLayout(false);
            gbObj.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numObjNum).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripButton btnAddObject;
        private ToolStripButton btnDeleteObject;
        private ToolStripButton btnFind;
        private ToolStripSeparator sep1;
        private TreeView tvObj;
        private Panel pnlRight;
        private GroupBox gbObj;
        private NumericUpDown numObjNum;
        private Label lblObjNum;
        private TextBox txtCode;
        private Label lblCode;
        private TextBox txtName;
        private Label lblName;
        private ComboBox cbParentObj;
        private Label lblParentObj;
        private ToolStripButton btnRefreshData;
    }
}