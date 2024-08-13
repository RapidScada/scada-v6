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
            components = new System.ComponentModel.Container();
            toolStrip = new ToolStrip();
            btnAddObject = new ToolStripButton();
            btnDeleteObject = new ToolStripButton();
            tvObj = new TreeView();
            cmsTree = new ContextMenuStrip(components);
            miCollapseAll = new ToolStripMenuItem();
            miOpenChannels = new ToolStripMenuItem();
            ilTree = new ImageList(components);
            pnlRight = new Panel();
            gbObject = new GroupBox();
            txtDescr = new TextBox();
            lblDescr = new Label();
            cbParentObj = new ComboBox();
            lblParentObj = new Label();
            txtCode = new TextBox();
            lblCode = new Label();
            txtName = new TextBox();
            lblName = new Label();
            numObjNum = new NumericUpDown();
            lblObjNum = new Label();
            toolStrip.SuspendLayout();
            cmsTree.SuspendLayout();
            pnlRight.SuspendLayout();
            gbObject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numObjNum).BeginInit();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAddObject, btnDeleteObject });
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
            btnAddObject.Click += btnAddObject_Click;
            // 
            // btnDeleteObject
            // 
            btnDeleteObject.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDeleteObject.Image = Properties.Resources.delete;
            btnDeleteObject.ImageTransparentColor = Color.Magenta;
            btnDeleteObject.Name = "btnDeleteObject";
            btnDeleteObject.Size = new Size(23, 22);
            btnDeleteObject.ToolTipText = "Delete Object";
            btnDeleteObject.Click += btnDeleteObject_Click;
            // 
            // tvObj
            // 
            tvObj.ContextMenuStrip = cmsTree;
            tvObj.Dock = DockStyle.Fill;
            tvObj.HideSelection = false;
            tvObj.ImageIndex = 0;
            tvObj.ImageList = ilTree;
            tvObj.Location = new Point(0, 25);
            tvObj.Name = "tvObj";
            tvObj.SelectedImageIndex = 0;
            tvObj.Size = new Size(484, 436);
            tvObj.TabIndex = 1;
            tvObj.AfterSelect += tvObj_AfterSelect;
            tvObj.NodeMouseClick += tvObj_NodeMouseClick;
            // 
            // cmsTree
            // 
            cmsTree.Items.AddRange(new ToolStripItem[] { miCollapseAll, miOpenChannels });
            cmsTree.Name = "cmsTree";
            cmsTree.Size = new Size(156, 48);
            cmsTree.Opening += cmsTree_Opening;
            // 
            // miCollapseAll
            // 
            miCollapseAll.Image = Properties.Resources.collapse_all;
            miCollapseAll.Name = "miCollapseAll";
            miCollapseAll.Size = new Size(155, 22);
            miCollapseAll.Text = "Collapse All";
            miCollapseAll.Click += miCollapseAll_Click;
            // 
            // miOpenChannels
            // 
            miOpenChannels.Image = Properties.Resources.table;
            miOpenChannels.Name = "miOpenChannels";
            miOpenChannels.Size = new Size(155, 22);
            miOpenChannels.Text = "Open Channels";
            miOpenChannels.Click += miOpenChannels_Click;
            // 
            // ilTree
            // 
            ilTree.ColorDepth = ColorDepth.Depth32Bit;
            ilTree.ImageSize = new Size(16, 16);
            ilTree.TransparentColor = Color.Transparent;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(gbObject);
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Location = new Point(484, 25);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(12, 0, 12, 12);
            pnlRight.Size = new Size(300, 436);
            pnlRight.TabIndex = 2;
            // 
            // gbObject
            // 
            gbObject.Controls.Add(txtDescr);
            gbObject.Controls.Add(lblDescr);
            gbObject.Controls.Add(cbParentObj);
            gbObject.Controls.Add(lblParentObj);
            gbObject.Controls.Add(txtCode);
            gbObject.Controls.Add(lblCode);
            gbObject.Controls.Add(txtName);
            gbObject.Controls.Add(lblName);
            gbObject.Controls.Add(numObjNum);
            gbObject.Controls.Add(lblObjNum);
            gbObject.Dock = DockStyle.Fill;
            gbObject.Location = new Point(12, 0);
            gbObject.Name = "gbObject";
            gbObject.Padding = new Padding(10, 3, 10, 10);
            gbObject.Size = new Size(276, 424);
            gbObject.TabIndex = 0;
            gbObject.TabStop = false;
            gbObject.Text = "Object Properties";
            // 
            // txtDescr
            // 
            txtDescr.Location = new Point(13, 241);
            txtDescr.Margin = new Padding(3, 3, 3, 10);
            txtDescr.Name = "txtDescr";
            txtDescr.Size = new Size(250, 23);
            txtDescr.TabIndex = 9;
            txtDescr.TextChanged += txtDescr_TextChanged;
            // 
            // lblDescr
            // 
            lblDescr.AutoSize = true;
            lblDescr.Location = new Point(10, 223);
            lblDescr.Name = "lblDescr";
            lblDescr.Size = new Size(67, 15);
            lblDescr.TabIndex = 8;
            lblDescr.Text = "Description";
            // 
            // cbParentObj
            // 
            cbParentObj.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParentObj.FormattingEnabled = true;
            cbParentObj.Location = new Point(13, 190);
            cbParentObj.Margin = new Padding(3, 3, 3, 10);
            cbParentObj.Name = "cbParentObj";
            cbParentObj.Size = new Size(250, 23);
            cbParentObj.TabIndex = 7;
            cbParentObj.SelectedIndexChanged += cbParentObj_SelectedIndexChanged;
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
            txtCode.TextChanged += txtCode_TextChanged;
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
            txtName.TextChanged += txtName_TextChanged;
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
            numObjNum.ValueChanged += numObjNum_ValueChanged;
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
            Name = "FrmObjectEditor";
            Text = "Object Editor";
            Load += FrmObjectEditor_Load;
            VisibleChanged += FrmObjectEditor_VisibleChanged;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            cmsTree.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            gbObject.ResumeLayout(false);
            gbObject.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numObjNum).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripButton btnAddObject;
        private ToolStripButton btnDeleteObject;
        private TreeView tvObj;
        private Panel pnlRight;
        private GroupBox gbObject;
        private NumericUpDown numObjNum;
        private Label lblObjNum;
        private TextBox txtCode;
        private Label lblCode;
        private TextBox txtName;
        private Label lblName;
        private ComboBox cbParentObj;
        private Label lblParentObj;
        private ImageList ilTree;
        private ContextMenuStrip cmsTree;
        private ToolStripMenuItem miCollapseAll;
        private TextBox txtDescr;
        private Label lblDescr;
        private ToolStripMenuItem miOpenChannels;
    }
}