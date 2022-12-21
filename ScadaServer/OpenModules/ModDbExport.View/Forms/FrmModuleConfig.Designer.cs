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
            this.btnOracle = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPostgreSql = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMySql = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddCurTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnAddHistTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnAddEventTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnAddAckTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnAddCommand = new System.Windows.Forms.ToolStripButton();
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
            this.pnlBottom.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(647, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(566, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(485, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddbAdd,
            this.btnAddCurTrigger,
            this.btnAddHistTrigger,
            this.btnAddEventTrigger,
            this.btnAddAckTrigger,
            this.btnAddCommand,
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
            this.toolStrip.TabIndex = 5;
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
            // 
            // btnOracle
            // 
            this.btnOracle.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.db_oracle;
            this.btnOracle.Name = "btnOracle";
            this.btnOracle.Size = new System.Drawing.Size(184, 22);
            this.btnOracle.Text = "Oracle";
            // 
            // btnPostgreSql
            // 
            this.btnPostgreSql.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.db_postgresql;
            this.btnPostgreSql.Name = "btnPostgreSql";
            this.btnPostgreSql.Size = new System.Drawing.Size(184, 22);
            this.btnPostgreSql.Text = "PostgreSQL";
            // 
            // btnMySql
            // 
            this.btnMySql.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.db_mysql;
            this.btnMySql.Name = "btnMySql";
            this.btnMySql.Size = new System.Drawing.Size(184, 22);
            this.btnMySql.Text = "MySQL";
            // 
            // btnAddCurTrigger
            // 
            this.btnAddCurTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCurTrigger.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_cur;
            this.btnAddCurTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCurTrigger.Name = "btnAddCurTrigger";
            this.btnAddCurTrigger.Size = new System.Drawing.Size(23, 22);
            this.btnAddCurTrigger.Text = "Add current data trigger";
            // 
            // btnAddHistTrigger
            // 
            this.btnAddHistTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddHistTrigger.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_hist;
            this.btnAddHistTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddHistTrigger.Name = "btnAddHistTrigger";
            this.btnAddHistTrigger.Size = new System.Drawing.Size(23, 22);
            this.btnAddHistTrigger.Text = "toolStripButton5";
            // 
            // btnAddEventTrigger
            // 
            this.btnAddEventTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddEventTrigger.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_event;
            this.btnAddEventTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddEventTrigger.Name = "btnAddEventTrigger";
            this.btnAddEventTrigger.Size = new System.Drawing.Size(23, 22);
            this.btnAddEventTrigger.Text = "Add event trigger";
            // 
            // btnAddAckTrigger
            // 
            this.btnAddAckTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddAckTrigger.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_ack;
            this.btnAddAckTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddAckTrigger.Name = "btnAddAckTrigger";
            this.btnAddAckTrigger.Size = new System.Drawing.Size(23, 22);
            this.btnAddAckTrigger.Text = "toolStripButton4";
            // 
            // btnAddCommand
            // 
            this.btnAddCommand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCommand.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.query_cmd;
            this.btnAddCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCommand.Name = "btnAddCommand";
            this.btnAddCommand.Size = new System.Drawing.Size(23, 22);
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
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tableLayoutPanel);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 25);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(734, 471);
            this.pnlMain.TabIndex = 6;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.91553F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.08447F));
            this.tableLayoutPanel.Controls.Add(this.pnlTree, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pnlInfo, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(734, 471);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pnlTree
            // 
            this.pnlTree.Controls.Add(this.gbTarget);
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTree.Location = new System.Drawing.Point(0, 0);
            this.pnlTree.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Size = new System.Drawing.Size(315, 471);
            this.pnlTree.TabIndex = 0;
            // 
            // gbTarget
            // 
            this.gbTarget.Controls.Add(this.tvTargets);
            this.gbTarget.Location = new System.Drawing.Point(12, 3);
            this.gbTarget.Name = "gbTarget";
            this.gbTarget.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbTarget.Size = new System.Drawing.Size(300, 462);
            this.gbTarget.TabIndex = 1;
            this.gbTarget.TabStop = false;
            this.gbTarget.Text = "Export Targets";
            // 
            // tvTargets
            // 
            this.tvTargets.ContextMenuStrip = this.cmsTree;
            this.tvTargets.HideSelection = false;
            this.tvTargets.ImageIndex = 0;
            this.tvTargets.ImageList = this.ilTree;
            this.tvTargets.Location = new System.Drawing.Point(13, 22);
            this.tvTargets.Name = "tvTargets";
            this.tvTargets.SelectedImageIndex = 0;
            this.tvTargets.Size = new System.Drawing.Size(274, 427);
            this.tvTargets.TabIndex = 0;
            // 
            // cmsTree
            // 
            this.cmsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCollapseAll});
            this.cmsTree.Name = "cmsTree";
            this.cmsTree.Size = new System.Drawing.Size(181, 48);
            // 
            // miCollapseAll
            // 
            this.miCollapseAll.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.collapse_all;
            this.miCollapseAll.Name = "miCollapseAll";
            this.miCollapseAll.Size = new System.Drawing.Size(180, 22);
            this.miCollapseAll.Text = "Collapse All";
            // 
            // ilTree
            // 
            this.ilTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ilTree.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblHint);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfo.Location = new System.Drawing.Point(315, 0);
            this.pnlInfo.Margin = new System.Windows.Forms.Padding(0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(419, 471);
            this.pnlInfo.TabIndex = 1;
            // 
            // lblHint
            // 
            this.lblHint.BackColor = System.Drawing.SystemColors.Control;
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(0, 352);
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
            this.ClientSize = new System.Drawing.Size(734, 541);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmModuleConfig";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export to DB";
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
        private ToolStripButton btnAddCurTrigger;
        private ToolStripButton btnAddCommand;
        private ToolStripButton btnAddEventTrigger;
        private ToolStripButton btnAddHistTrigger;
        private ToolStripButton btnAddAckTrigger;
        private Label lblHint;
        private ContextMenuStrip cmsTree;
        private ToolStripMenuItem miCollapseAll;
        private ImageList ilTree;
    }
}