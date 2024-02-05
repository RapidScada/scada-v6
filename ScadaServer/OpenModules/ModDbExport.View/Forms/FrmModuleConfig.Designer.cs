namespace Scada.Server.Modules.ModDbExport.View.Forms
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
            ddbAdd = new ToolStripDropDownButton();
            btnSqlServer = new ToolStripMenuItem();
            btnMySql = new ToolStripMenuItem();
            btnOracle = new ToolStripMenuItem();
            btnPostgreSql = new ToolStripMenuItem();
            btnAddCurrentDataQuery = new ToolStripButton();
            btnAddHistoricalDataQuery = new ToolStripButton();
            btnAddEventQuery = new ToolStripButton();
            btnAddEventAckQuery = new ToolStripButton();
            btnAddCommandQuery = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnMoveUp = new ToolStripButton();
            btnMoveDown = new ToolStripButton();
            btnDelete = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnCut = new ToolStripButton();
            btnCopy = new ToolStripButton();
            btnPaste = new ToolStripButton();
            pnlMain = new Panel();
            pnlInfo = new Panel();
            lblHint = new Label();
            ctrlGeneral = new Controls.CtrlGeneral();
            ctrlDbConnection = new Scada.Forms.Controls.CtrlDbConnection();
            ctrlCurDataExport = new Controls.CtrlCurDataExport();
            ctrlHistDataExport = new Controls.CtrlHistDataExport();
            ctrlArcReplication = new Controls.CtrlArcReplication();
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
            pnlBottom.Location = new Point(0, 496);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(734, 45);
            pnlBottom.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(647, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.Location = new Point(566, 10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(485, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { ddbAdd, btnAddCurrentDataQuery, btnAddHistoricalDataQuery, btnAddEventQuery, btnAddEventAckQuery, btnAddCommandQuery, toolStripSeparator1, btnMoveUp, btnMoveDown, btnDelete, toolStripSeparator2, btnCut, btnCopy, btnPaste });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(734, 25);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "Add export target";
            // 
            // ddbAdd
            // 
            ddbAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ddbAdd.DropDownItems.AddRange(new ToolStripItem[] { btnSqlServer, btnMySql, btnOracle, btnPostgreSql });
            ddbAdd.Image = Properties.Resources.add_db;
            ddbAdd.ImageTransparentColor = Color.Magenta;
            ddbAdd.Name = "ddbAdd";
            ddbAdd.Size = new Size(29, 22);
            ddbAdd.Text = "Add export target";
            ddbAdd.ToolTipText = "Add export target";
            // 
            // btnSqlServer
            // 
            btnSqlServer.Image = Properties.Resources.db_mssql;
            btnSqlServer.Name = "btnSqlServer";
            btnSqlServer.Size = new Size(184, 22);
            btnSqlServer.Text = "Microsoft SQL Server";
            btnSqlServer.Click += btnAddTarget_Click;
            // 
            // btnMySql
            // 
            btnMySql.Image = Properties.Resources.db_mysql;
            btnMySql.Name = "btnMySql";
            btnMySql.Size = new Size(184, 22);
            btnMySql.Text = "MySQL";
            btnMySql.Click += btnAddTarget_Click;
            // 
            // btnOracle
            // 
            btnOracle.Image = Properties.Resources.db_oracle;
            btnOracle.Name = "btnOracle";
            btnOracle.Size = new Size(184, 22);
            btnOracle.Text = "Oracle";
            btnOracle.Click += btnAddTarget_Click;
            // 
            // btnPostgreSql
            // 
            btnPostgreSql.Image = Properties.Resources.db_postgresql;
            btnPostgreSql.Name = "btnPostgreSql";
            btnPostgreSql.Size = new Size(184, 22);
            btnPostgreSql.Text = "PostgreSQL";
            btnPostgreSql.Click += btnAddTarget_Click;
            // 
            // btnAddCurrentDataQuery
            // 
            btnAddCurrentDataQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddCurrentDataQuery.Image = Properties.Resources.query_cur;
            btnAddCurrentDataQuery.ImageTransparentColor = Color.Magenta;
            btnAddCurrentDataQuery.Name = "btnAddCurrentDataQuery";
            btnAddCurrentDataQuery.Size = new Size(23, 22);
            btnAddCurrentDataQuery.Text = "Add Current Data Query";
            btnAddCurrentDataQuery.Click += btnAddQuery_Click;
            // 
            // btnAddHistoricalDataQuery
            // 
            btnAddHistoricalDataQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddHistoricalDataQuery.Image = Properties.Resources.query_hist;
            btnAddHistoricalDataQuery.ImageTransparentColor = Color.Magenta;
            btnAddHistoricalDataQuery.Name = "btnAddHistoricalDataQuery";
            btnAddHistoricalDataQuery.Size = new Size(23, 22);
            btnAddHistoricalDataQuery.Text = "Add Historical Data Query";
            btnAddHistoricalDataQuery.Click += btnAddQuery_Click;
            // 
            // btnAddEventQuery
            // 
            btnAddEventQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddEventQuery.Image = Properties.Resources.query_event;
            btnAddEventQuery.ImageTransparentColor = Color.Magenta;
            btnAddEventQuery.Name = "btnAddEventQuery";
            btnAddEventQuery.Size = new Size(23, 22);
            btnAddEventQuery.Text = "Add Event Query";
            btnAddEventQuery.Click += btnAddQuery_Click;
            // 
            // btnAddEventAckQuery
            // 
            btnAddEventAckQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddEventAckQuery.Image = Properties.Resources.query_ack;
            btnAddEventAckQuery.ImageTransparentColor = Color.Magenta;
            btnAddEventAckQuery.Name = "btnAddEventAckQuery";
            btnAddEventAckQuery.Size = new Size(23, 22);
            btnAddEventAckQuery.Text = "Add Event Acknowledgement Query";
            btnAddEventAckQuery.Click += btnAddQuery_Click;
            // 
            // btnAddCommandQuery
            // 
            btnAddCommandQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddCommandQuery.Image = Properties.Resources.query_cmd;
            btnAddCommandQuery.ImageTransparentColor = Color.Magenta;
            btnAddCommandQuery.Name = "btnAddCommandQuery";
            btnAddCommandQuery.Size = new Size(23, 22);
            btnAddCommandQuery.Text = "Add Command Query";
            btnAddCommandQuery.Click += btnAddQuery_Click;
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
            // pnlMain
            // 
            pnlMain.Controls.Add(pnlInfo);
            pnlMain.Controls.Add(gbTarget);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 25);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(734, 471);
            pnlMain.TabIndex = 1;
            // 
            // pnlInfo
            // 
            pnlInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlInfo.Controls.Add(lblHint);
            pnlInfo.Controls.Add(ctrlGeneral);
            pnlInfo.Controls.Add(ctrlDbConnection);
            pnlInfo.Controls.Add(ctrlCurDataExport);
            pnlInfo.Controls.Add(ctrlHistDataExport);
            pnlInfo.Controls.Add(ctrlArcReplication);
            pnlInfo.Controls.Add(ctrlQuery);
            pnlInfo.Location = new Point(318, 3);
            pnlInfo.Margin = new Padding(0);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new Size(404, 462);
            pnlInfo.TabIndex = 1;
            // 
            // lblHint
            // 
            lblHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblHint.BackColor = SystemColors.Control;
            lblHint.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblHint.ForeColor = SystemColors.GrayText;
            lblHint.Location = new Point(0, 0);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(404, 74);
            lblHint.TabIndex = 6;
            lblHint.Text = "Add tagret";
            lblHint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ctrlGeneral
            // 
            ctrlGeneral.ConfigDataset = null;
            ctrlGeneral.Dock = DockStyle.Fill;
            ctrlGeneral.Location = new Point(0, 0);
            ctrlGeneral.Margin = new Padding(3, 3, 3, 10);
            ctrlGeneral.Name = "ctrlGeneral";
            ctrlGeneral.Size = new Size(404, 462);
            ctrlGeneral.TabIndex = 5;
            ctrlGeneral.ObjectChanged += ctrlGeneral_ObjectChanged;
            // 
            // ctrlDbConnection
            // 
            ctrlDbConnection.BuildConnectionStringFunc = null;
            ctrlDbConnection.ConnectionOptions = null;
            ctrlDbConnection.DbmsEnabled = true;
            ctrlDbConnection.Dock = DockStyle.Fill;
            ctrlDbConnection.Location = new Point(0, 0);
            ctrlDbConnection.Margin = new Padding(3, 3, 3, 10);
            ctrlDbConnection.Name = "ctrlDbConnection";
            ctrlDbConnection.NameEnabled = true;
            ctrlDbConnection.Size = new Size(404, 462);
            ctrlDbConnection.TabIndex = 4;
            ctrlDbConnection.ConnectionOptionsChanged += ctrlDbConnection_ConnectionOptionsChanged;
            // 
            // ctrlCurDataExport
            // 
            ctrlCurDataExport.Dock = DockStyle.Fill;
            ctrlCurDataExport.Location = new Point(0, 0);
            ctrlCurDataExport.Margin = new Padding(3, 3, 3, 10);
            ctrlCurDataExport.Name = "ctrlCurDataExport";
            ctrlCurDataExport.Size = new Size(404, 462);
            ctrlCurDataExport.TabIndex = 3;
            ctrlCurDataExport.ObjectChanged += ctrl_ObjectChanged;
            // 
            // ctrlHistDataExport
            // 
            ctrlHistDataExport.ConfigDataset = null;
            ctrlHistDataExport.Dock = DockStyle.Fill;
            ctrlHistDataExport.Location = new Point(0, 0);
            ctrlHistDataExport.Margin = new Padding(3, 3, 3, 10);
            ctrlHistDataExport.Name = "ctrlHistDataExport";
            ctrlHistDataExport.Size = new Size(404, 462);
            ctrlHistDataExport.TabIndex = 2;
            ctrlHistDataExport.ObjectChanged += ctrl_ObjectChanged;
            // 
            // ctrlArcReplication
            // 
            ctrlArcReplication.ConfigDataset = null;
            ctrlArcReplication.Dock = DockStyle.Fill;
            ctrlArcReplication.Location = new Point(0, 0);
            ctrlArcReplication.Margin = new Padding(3, 3, 3, 10);
            ctrlArcReplication.Name = "ctrlArcReplication";
            ctrlArcReplication.Size = new Size(404, 462);
            ctrlArcReplication.TabIndex = 1;
            ctrlArcReplication.ObjectChanged += ctrl_ObjectChanged;
            // 
            // ctrlQuery
            // 
            ctrlQuery.ConfigDataset = null;
            ctrlQuery.Dock = DockStyle.Fill;
            ctrlQuery.Location = new Point(0, 0);
            ctrlQuery.Margin = new Padding(3, 3, 3, 10);
            ctrlQuery.Name = "ctrlQuery";
            ctrlQuery.Size = new Size(404, 462);
            ctrlQuery.TabIndex = 0;
            ctrlQuery.ObjectChanged += ctrlQuery_ObjectChanged;
            // 
            // gbTarget
            // 
            gbTarget.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            gbTarget.Controls.Add(tvTargets);
            gbTarget.Location = new Point(12, 3);
            gbTarget.Name = "gbTarget";
            gbTarget.Padding = new Padding(10, 3, 10, 10);
            gbTarget.Size = new Size(300, 462);
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
            tvTargets.Location = new Point(13, 22);
            tvTargets.Name = "tvTargets";
            tvTargets.SelectedImageIndex = 0;
            tvTargets.Size = new Size(274, 427);
            tvTargets.TabIndex = 0;
            tvTargets.AfterSelect += tvTargets_AfterSelect;
            // 
            // cmsTree
            // 
            cmsTree.Items.AddRange(new ToolStripItem[] { miCollapseAll });
            cmsTree.Name = "cmsTree";
            cmsTree.Size = new Size(137, 26);
            // 
            // miCollapseAll
            // 
            miCollapseAll.Image = Properties.Resources.collapse_all;
            miCollapseAll.Name = "miCollapseAll";
            miCollapseAll.Size = new Size(136, 22);
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
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(734, 541);
            Controls.Add(pnlMain);
            Controls.Add(toolStrip);
            Controls.Add(pnlBottom);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(500, 300);
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
        private ToolStripDropDownButton ddbAdd;
        private ToolStripMenuItem btnSqlServer;
        private ToolStripMenuItem btnOracle;
        private ToolStripMenuItem btnPostgreSql;
        private ToolStripMenuItem btnMySql;
        private ToolStripButton btnAddCurrentDataQuery;
        private ToolStripButton btnAddCommandQuery;
        private ToolStripButton btnAddEventQuery;
        private ToolStripButton btnAddHistoricalDataQuery;
        private ToolStripButton btnAddEventAckQuery;
        private Label lblHint;
        private ContextMenuStrip cmsTree;
        private ToolStripMenuItem miCollapseAll;
        private ImageList ilTree;
        private Controls.CtrlQuery ctrlQuery;
        private Controls.CtrlGeneral ctrlGeneral;
        private Controls.CtrlCurDataExport ctrlCurDataExport;
        private Controls.CtrlArcReplication ctrlArcReplication;
        private Scada.Forms.Controls.CtrlDbConnection ctrlDbConnection;
        private GroupBox gbTarget;
        private TreeView tvTargets;
        private Controls.CtrlHistDataExport ctrlHistDataExport;
    }
}