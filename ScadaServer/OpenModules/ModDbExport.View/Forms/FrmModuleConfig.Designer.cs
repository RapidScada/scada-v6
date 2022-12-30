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
            this.components = new System.ComponentModel.Container();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ddbAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnSqlServer = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMySql = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOracle = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPostgreSql = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddCurrentQuery = new System.Windows.Forms.ToolStripButton();
            this.btnAddHistoricalQuery = new System.Windows.Forms.ToolStripButton();
            this.btnAddEventQuery = new System.Windows.Forms.ToolStripButton();
            this.btnAddEventAckQuery = new System.Windows.Forms.ToolStripButton();
            this.btnAddCommandQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCut = new System.Windows.Forms.ToolStripButton();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnPaste = new System.Windows.Forms.ToolStripButton();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTree = new System.Windows.Forms.Panel();
            this.gbTarget = new System.Windows.Forms.GroupBox();
            this.tvTargets = new System.Windows.Forms.TreeView();
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.ctrlDbConnection = new Scada.Forms.Controls.CtrlDbConnection();
            this.ctrlArcReplication = new Scada.Server.Modules.ModDbExport.View.Controls.CtrlArcReplication();
            this.ctrlCurDataExport = new Scada.Server.Modules.ModDbExport.View.Controls.CtrlCurDataExport();
            this.ctrlGeneral = new Scada.Server.Modules.ModDbExport.View.Controls.CtrlGeneral();
            this.ctrlQuery = new Scada.Server.Modules.ModDbExport.View.Controls.CtrlQuery();
            this.lblHint = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.pnlTree.SuspendLayout();
            this.gbTarget.SuspendLayout();
            this.cmsTree.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 496);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(734, 45);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(647, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(566, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(485, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddbAdd,
            this.btnAddCurrentQuery,
            this.btnAddHistoricalQuery,
            this.btnAddEventQuery,
            this.btnAddEventAckQuery,
            this.btnAddCommandQuery,
            this.toolStripSeparator1,
            this.btnMoveUp,
            this.btnMoveDown,
            this.btnDelete,
            this.toolStripSeparator2,
            this.btnCut,
            this.btnCopy,
            this.btnPaste});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(734, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "Add export target";
            // 
            // ddbAdd
            // 
            this.ddbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ddbAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSqlServer,
            this.btnMySql,
            this.btnOracle,
            this.btnPostgreSql});
            this.ddbAdd.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.add_db;
            this.ddbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddbAdd.Name = "ddbAdd";
            this.ddbAdd.Size = new System.Drawing.Size(29, 22);
            this.ddbAdd.Text = "Add export target";
            this.ddbAdd.ToolTipText = "Add export target";
            // 
            // btnSqlServer
            // 
            this.btnSqlServer.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.db_mssql;
            this.btnSqlServer.Name = "btnSqlServer";
            this.btnSqlServer.Size = new System.Drawing.Size(184, 22);
            this.btnSqlServer.Text = "Microsoft SQL Server";
            this.btnSqlServer.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnMySql
            // 
            this.btnMySql.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.db_mysql;
            this.btnMySql.Name = "btnMySql";
            this.btnMySql.Size = new System.Drawing.Size(184, 22);
            this.btnMySql.Text = "MySQL";
            this.btnMySql.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnOracle
            // 
            this.btnOracle.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.db_oracle;
            this.btnOracle.Name = "btnOracle";
            this.btnOracle.Size = new System.Drawing.Size(184, 22);
            this.btnOracle.Text = "Oracle";
            this.btnOracle.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnPostgreSql
            // 
            this.btnPostgreSql.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.db_postgresql;
            this.btnPostgreSql.Name = "btnPostgreSql";
            this.btnPostgreSql.Size = new System.Drawing.Size(184, 22);
            this.btnPostgreSql.Text = "PostgreSQL";
            this.btnPostgreSql.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnAddCurrentQuery
            // 
            this.btnAddCurrentQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCurrentQuery.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_cur;
            this.btnAddCurrentQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCurrentQuery.Name = "btnAddCurrentQuery";
            this.btnAddCurrentQuery.Size = new System.Drawing.Size(23, 22);
            this.btnAddCurrentQuery.Text = "Add current query";
            this.btnAddCurrentQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
            // 
            // btnAddHistoricalQuery
            // 
            this.btnAddHistoricalQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddHistoricalQuery.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_hist;
            this.btnAddHistoricalQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddHistoricalQuery.Name = "btnAddHistoricalQuery";
            this.btnAddHistoricalQuery.Size = new System.Drawing.Size(23, 22);
            this.btnAddHistoricalQuery.Text = "Add historical query";
            this.btnAddHistoricalQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
            // 
            // btnAddEventQuery
            // 
            this.btnAddEventQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddEventQuery.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_event;
            this.btnAddEventQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddEventQuery.Name = "btnAddEventQuery";
            this.btnAddEventQuery.Size = new System.Drawing.Size(23, 22);
            this.btnAddEventQuery.Text = "Add event query";
            this.btnAddEventQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
            // 
            // btnAddEventAckQuery
            // 
            this.btnAddEventAckQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddEventAckQuery.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_ack;
            this.btnAddEventAckQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddEventAckQuery.Name = "btnAddEventAckQuery";
            this.btnAddEventAckQuery.Size = new System.Drawing.Size(23, 22);
            this.btnAddEventAckQuery.Text = "Add event acknowledgement query";
            this.btnAddEventAckQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
            // 
            // btnAddCommandQuery
            // 
            this.btnAddCommandQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCommandQuery.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_cmd;
            this.btnAddCommandQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCommandQuery.Name = "btnAddCommandQuery";
            this.btnAddCommandQuery.Size = new System.Drawing.Size(23, 22);
            this.btnAddCommandQuery.Text = "Add command query";
            this.btnAddCommandQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.move_up;
            this.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.ToolTipText = "Move Up";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.move_down;
            this.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.ToolTipText = "Move Down";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.ToolTipText = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCut
            // 
            this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCut.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.cut;
            this.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(23, 22);
            this.btnCut.Text = "Cut";
            this.btnCut.ToolTipText = "Cut";
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopy.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.copy;
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(23, 22);
            this.btnCopy.Text = "Copy";
            this.btnCopy.ToolTipText = "Copy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPaste.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.paste;
            this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(23, 22);
            this.btnPaste.Text = "Paste";
            this.btnPaste.ToolTipText = "Paste";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tableLayoutPanel);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 25);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(734, 471);
            this.pnlMain.TabIndex = 1;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.pnlTree, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pnlInfo, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(734, 471);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pnlTree
            // 
            this.pnlTree.Controls.Add(this.gbTarget);
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTree.Location = new System.Drawing.Point(0, 0);
            this.pnlTree.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Size = new System.Drawing.Size(315, 471);
            this.pnlTree.TabIndex = 0;
            // 
            // gbTarget
            // 
            this.gbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbTarget.Controls.Add(this.tvTargets);
            this.gbTarget.Location = new System.Drawing.Point(12, 3);
            this.gbTarget.Name = "gbTarget";
            this.gbTarget.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbTarget.Size = new System.Drawing.Size(300, 462);
            this.gbTarget.TabIndex = 0;
            this.gbTarget.TabStop = false;
            this.gbTarget.Text = "Export Targets";
            // 
            // tvTargets
            // 
            this.tvTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvTargets.ContextMenuStrip = this.cmsTree;
            this.tvTargets.HideSelection = false;
            this.tvTargets.ImageIndex = 0;
            this.tvTargets.ImageList = this.ilTree;
            this.tvTargets.Location = new System.Drawing.Point(13, 22);
            this.tvTargets.Name = "tvTargets";
            this.tvTargets.SelectedImageIndex = 0;
            this.tvTargets.Size = new System.Drawing.Size(274, 427);
            this.tvTargets.TabIndex = 0;
            this.tvTargets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTargets_AfterSelect);
            // 
            // cmsTree
            // 
            this.cmsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCollapseAll});
            this.cmsTree.Name = "cmsTree";
            this.cmsTree.Size = new System.Drawing.Size(137, 26);
            // 
            // miCollapseAll
            // 
            this.miCollapseAll.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.collapse_all;
            this.miCollapseAll.Name = "miCollapseAll";
            this.miCollapseAll.Size = new System.Drawing.Size(136, 22);
            this.miCollapseAll.Text = "Collapse All";
            this.miCollapseAll.Click += new System.EventHandler(this.miCollapseAll_Click);
            // 
            // ilTree
            // 
            this.ilTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ilTree.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInfo.Controls.Add(this.ctrlDbConnection);
            this.pnlInfo.Controls.Add(this.ctrlArcReplication);
            this.pnlInfo.Controls.Add(this.ctrlCurDataExport);
            this.pnlInfo.Controls.Add(this.ctrlGeneral);
            this.pnlInfo.Controls.Add(this.ctrlQuery);
            this.pnlInfo.Controls.Add(this.lblHint);
            this.pnlInfo.Location = new System.Drawing.Point(315, 0);
            this.pnlInfo.Margin = new System.Windows.Forms.Padding(0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(419, 471);
            this.pnlInfo.TabIndex = 1;
            // 
            // ctrlDbConnection
            // 
            this.ctrlDbConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlDbConnection.BuildConnectionStringFunc = null;
            this.ctrlDbConnection.ConnectionOptions = null;
            this.ctrlDbConnection.DbmsEnabled = true;
            this.ctrlDbConnection.Location = new System.Drawing.Point(3, 3);
            this.ctrlDbConnection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ctrlDbConnection.Name = "ctrlDbConnection";
            this.ctrlDbConnection.NameEnabled = true;
            this.ctrlDbConnection.Size = new System.Drawing.Size(300, 399);
            this.ctrlDbConnection.TabIndex = 7;
            this.ctrlDbConnection.ConnectionOptionsChanged += new System.EventHandler(this.ctrlDbConnection_ConnectionOptionsChanged);
            // 
            // ctrlArcReplication
            // 
            this.ctrlArcReplication.ConfigDataset = null;
            this.ctrlArcReplication.Location = new System.Drawing.Point(3, 3);
            this.ctrlArcReplication.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ctrlArcReplication.Name = "ctrlArcReplication";
            this.ctrlArcReplication.Size = new System.Drawing.Size(404, 462);
            this.ctrlArcReplication.TabIndex = 4;
            this.ctrlArcReplication.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrl_ObjectChanged);
            // 
            // ctrlCurDataExport
            // 
            this.ctrlCurDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlCurDataExport.Location = new System.Drawing.Point(3, 3);
            this.ctrlCurDataExport.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ctrlCurDataExport.Name = "ctrlCurDataExport";
            this.ctrlCurDataExport.Size = new System.Drawing.Size(404, 462);
            this.ctrlCurDataExport.TabIndex = 3;
            this.ctrlCurDataExport.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrl_ObjectChanged);
            // 
            // ctrlGeneral
            // 
            this.ctrlGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlGeneral.ConfigDataset = null;
            this.ctrlGeneral.Location = new System.Drawing.Point(3, 3);
            this.ctrlGeneral.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ctrlGeneral.Name = "ctrlGeneral";
            this.ctrlGeneral.Size = new System.Drawing.Size(404, 462);
            this.ctrlGeneral.TabIndex = 2;
            this.ctrlGeneral.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlGeneral_ObjectChanged);
            // 
            // ctrlQuery
            // 
            this.ctrlQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlQuery.ConfigDataset = null;
            this.ctrlQuery.Location = new System.Drawing.Point(3, 3);
            this.ctrlQuery.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ctrlQuery.Name = "ctrlQuery";
            this.ctrlQuery.Size = new System.Drawing.Size(404, 462);
            this.ctrlQuery.TabIndex = 1;
            this.ctrlQuery.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlQuery_ObjectChanged);
            // 
            // lblHint
            // 
            this.lblHint.BackColor = System.Drawing.SystemColors.Control;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(0, 0);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(404, 100);
            this.lblHint.TabIndex = 0;
            this.lblHint.Text = "Add tagret";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmModuleConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(734, 541);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(460, 280);
            this.Name = "FrmModuleConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export to DB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmModuleConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmModuleConfig_Load);
            this.pnlBottom.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.pnlTree.ResumeLayout(false);
            this.gbTarget.ResumeLayout(false);
            this.cmsTree.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private TableLayoutPanel tableLayoutPanel;
        private Panel pnlTree;
        private Panel pnlInfo;
        private GroupBox gbTarget;
        private TreeView tvTargets;
        private ToolStripDropDownButton ddbAdd;
        private ToolStripMenuItem btnSqlServer;
        private ToolStripMenuItem btnOracle;
        private ToolStripMenuItem btnPostgreSql;
        private ToolStripMenuItem btnMySql;
        private ToolStripButton btnAddCurrentQuery;
        private ToolStripButton btnAddCommandQuery;
        private ToolStripButton btnAddEventQuery;
        private ToolStripButton btnAddHistoricalQuery;
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
    }
}