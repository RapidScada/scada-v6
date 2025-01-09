namespace Scada.Server.Modules.ModDeviceAlarm.View.Forms
{
    partial class FrmModuleConfig
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
            pnlBottom = new Panel();
            btnClose = new Button();
            btnCancel = new Button();
            btnSave = new Button();
            toolStrip = new ToolStrip();
            btnAdd = new ToolStripButton();
            btnAddStatusTrigger = new ToolStripButton();
            btnAddValueUnchangedTrigger = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnMoveUp = new ToolStripButton();
            btnMoveDown = new ToolStripButton();
            btnDelete = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnCut = new ToolStripButton();
            btnCopy = new ToolStripButton();
            btnPaste = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            btnSmtpConfig = new ToolStripButton();
            pnlMain = new Panel();
            pnlInfo = new Panel();
            ctrlGeneral = new Controls.CtrlGeneral();
            lblHint = new Label();
            ctrlQuery = new Controls.CtrlQuery();
            gbTarget = new GroupBox();
            tvTargets = new TreeView();
            cmsTree = new ContextMenuStrip(components);
            miCollapseAll = new ToolStripMenuItem();
            ilTree = new ImageList(components);
            pnlBottom.SuspendLayout();
            toolStrip.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlInfo.SuspendLayout();
            gbTarget.SuspendLayout();
            cmsTree.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnClose);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 562);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(734, 51);
            pnlBottom.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(647, 11);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 26);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.Location = new Point(566, 11);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 26);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(485, 11);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 26);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnAddStatusTrigger, btnAddValueUnchangedTrigger, toolStripSeparator1, btnMoveUp, btnMoveDown, btnDelete, toolStripSeparator2, btnCut, btnCopy, btnPaste, toolStripSeparator3, btnSmtpConfig });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(734, 25);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "Add export target";
            // 
            // btnAdd
            // 
            btnAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAdd.Image = Properties.Resources.add_db;
            btnAdd.ImageTransparentColor = Color.Magenta;
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(23, 22);
            btnAdd.Text = "Add trigger";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnAddStatusTrigger
            // 
            btnAddStatusTrigger.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddStatusTrigger.Image = Properties.Resources.query_cur;
            btnAddStatusTrigger.ImageTransparentColor = Color.Magenta;
            btnAddStatusTrigger.Name = "btnAddStatusTrigger";
            btnAddStatusTrigger.Size = new Size(23, 22);
            btnAddStatusTrigger.Text = "Add Status Trigger";
            btnAddStatusTrigger.Click += btnAddQuery_Click;
            // 
            // btnAddValueUnchangedTrigger
            // 
            btnAddValueUnchangedTrigger.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddValueUnchangedTrigger.Image = Properties.Resources.query_event;
            btnAddValueUnchangedTrigger.ImageTransparentColor = Color.Magenta;
            btnAddValueUnchangedTrigger.Name = "btnAddValueUnchangedTrigger";
            btnAddValueUnchangedTrigger.Size = new Size(23, 22);
            btnAddValueUnchangedTrigger.Text = "Add Value Unchanged trigger";
            btnAddValueUnchangedTrigger.Click += btnAddQuery_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // btnMoveUp
            // 
            btnMoveUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveUp.Image = Properties.Resources.move_up;
            btnMoveUp.ImageTransparentColor = Color.Magenta;
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(23, 22);
            btnMoveUp.Text = "Move Up";
            btnMoveUp.ToolTipText = "Move Up";
            btnMoveUp.Click += btnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveDown.Image = Properties.Resources.move_down;
            btnMoveDown.ImageTransparentColor = Color.Magenta;
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(23, 22);
            btnMoveDown.Text = "Move Down";
            btnMoveDown.ToolTipText = "Move Down";
            btnMoveDown.Click += btnMoveDown_Click;
            // 
            // btnDelete
            // 
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDelete.Image = Properties.Resources.delete;
            btnDelete.ImageTransparentColor = Color.Magenta;
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(23, 22);
            btnDelete.Text = "Delete";
            btnDelete.ToolTipText = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // btnCut
            // 
            btnCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnCut.Image = Properties.Resources.cut;
            btnCut.ImageTransparentColor = Color.Magenta;
            btnCut.Name = "btnCut";
            btnCut.Size = new Size(23, 22);
            btnCut.Text = "Cut";
            btnCut.ToolTipText = "Cut";
            btnCut.Click += btnCut_Click;
            // 
            // btnCopy
            // 
            btnCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnCopy.Image = Properties.Resources.copy;
            btnCopy.ImageTransparentColor = Color.Magenta;
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(23, 22);
            btnCopy.Text = "Copy";
            btnCopy.ToolTipText = "Copy";
            btnCopy.Click += btnCopy_Click;
            // 
            // btnPaste
            // 
            btnPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnPaste.Image = Properties.Resources.paste;
            btnPaste.ImageTransparentColor = Color.Magenta;
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(23, 22);
            btnPaste.Text = "Paste";
            btnPaste.ToolTipText = "Paste";
            btnPaste.Click += btnPaste_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
            // 
            // btnSmtpConfig
            // 
            btnSmtpConfig.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSmtpConfig.Image = Properties.Resources.export_options;
            btnSmtpConfig.ImageTransparentColor = Color.Magenta;
            btnSmtpConfig.Name = "btnSmtpConfig";
            btnSmtpConfig.Size = new Size(23, 22);
            btnSmtpConfig.Text = "Smtp config";
            btnSmtpConfig.Click += btnSmtpConfig_Click;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(pnlInfo);
            pnlMain.Controls.Add(gbTarget);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 25);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(734, 537);
            pnlMain.TabIndex = 1;
            // 
            // pnlInfo
            // 
            pnlInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlInfo.Controls.Add(ctrlGeneral);
            pnlInfo.Controls.Add(lblHint);
            pnlInfo.Controls.Add(ctrlQuery);
            pnlInfo.Location = new Point(318, 3);
            pnlInfo.Margin = new Padding(0);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new Size(404, 527);
            pnlInfo.TabIndex = 1;
            // 
            // ctrlGeneral
            // 
            ctrlGeneral.ConfigDataset = null;
            ctrlGeneral.Dock = DockStyle.Fill;
            ctrlGeneral.Location = new Point(0, 0);
            ctrlGeneral.Margin = new Padding(3, 3, 3, 10);
            ctrlGeneral.Name = "ctrlGeneral";
            ctrlGeneral.Size = new Size(404, 527);
            ctrlGeneral.TabIndex = 2;
            ctrlGeneral.ObjectChanged += ctrlGeneral_ObjectChanged;
            // 
            // lblHint
            // 
            lblHint.BackColor = SystemColors.Control;
            lblHint.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblHint.ForeColor = SystemColors.GrayText;
            lblHint.Location = new Point(0, 0);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(434, 74);
            lblHint.TabIndex = 0;
            lblHint.Text = "Add tagret";
            lblHint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ctrlQuery
            // 
            ctrlQuery.ConfigDataset = null;
            ctrlQuery.Dock = DockStyle.Fill;
            ctrlQuery.Location = new Point(0, 0);
            ctrlQuery.Margin = new Padding(3, 3, 3, 10);
            ctrlQuery.Name = "ctrlQuery";
            ctrlQuery.Size = new Size(404, 527);
            ctrlQuery.TabIndex = 1;
            ctrlQuery.ObjectChanged += ctrlQuery_ObjectChanged;
            // 
            // gbTarget
            // 
            gbTarget.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            gbTarget.Controls.Add(tvTargets);
            gbTarget.Location = new Point(12, 3);
            gbTarget.Name = "gbTarget";
            gbTarget.Padding = new Padding(10, 3, 10, 11);
            gbTarget.Size = new Size(300, 527);
            gbTarget.TabIndex = 0;
            gbTarget.TabStop = false;
            gbTarget.Text = "Export Targets";
            // 
            // tvTargets
            // 
            tvTargets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tvTargets.ContextMenuStrip = cmsTree;
            tvTargets.HideSelection = false;
            tvTargets.ImageIndex = 0;
            tvTargets.ImageList = ilTree;
            tvTargets.Location = new Point(13, 25);
            tvTargets.Name = "tvTargets";
            tvTargets.SelectedImageIndex = 0;
            tvTargets.Size = new Size(274, 486);
            tvTargets.TabIndex = 0;
            tvTargets.AfterSelect += tvTargets_AfterSelect;
            // 
            // cmsTree
            // 
            cmsTree.Items.AddRange(new ToolStripItem[] { miCollapseAll });
            cmsTree.Name = "cmsTree";
            cmsTree.Size = new Size(145, 26);
            // 
            // miCollapseAll
            // 
            miCollapseAll.Image = Properties.Resources.collapse_all;
            miCollapseAll.Name = "miCollapseAll";
            miCollapseAll.Size = new Size(144, 22);
            miCollapseAll.Text = "Collapse All";
            miCollapseAll.Click += miCollapseAll_Click;
            // 
            // ilTree
            // 
            ilTree.ColorDepth = ColorDepth.Depth24Bit;
            ilTree.ImageSize = new Size(16, 16);
            ilTree.TransparentColor = Color.Transparent;
            // 
            // FrmModuleConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(734, 613);
            Controls.Add(pnlMain);
            Controls.Add(toolStrip);
            Controls.Add(pnlBottom);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(500, 335);
            Name = "FrmModuleConfig";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Export to DB";
            FormClosing += FrmModuleConfig_FormClosing;
            Load += FrmModuleConfig_Load;
            pnlBottom.ResumeLayout(false);
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlInfo.ResumeLayout(false);
            gbTarget.ResumeLayout(false);
            cmsTree.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlBottom;
        private Button btnClose;
        private Button btnCancel;
        private Button btnSave;
        private ToolStrip toolStrip;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnMoveUp;
        private ToolStripButton btnMoveDown;
        private ToolStripButton btnDelete;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton btnCut;
        private ToolStripButton btnCopy;
        private ToolStripButton btnPaste;
        private Panel pnlMain;
        private Panel pnlInfo;
        private ToolStripButton btnAddStatusTrigger;
        private ToolStripButton btnAddValueUnchangedTrigger;
        private Label lblHint;
        private ContextMenuStrip cmsTree;
        private ToolStripMenuItem miCollapseAll;
        private ImageList ilTree;
        private Controls.CtrlQuery ctrlQuery;
        private Controls.CtrlGeneral ctrlGeneral;
        private GroupBox gbTarget;
        private TreeView tvTargets;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton btnSmtpConfig;
        private ToolStripButton btnAdd;
    }
}