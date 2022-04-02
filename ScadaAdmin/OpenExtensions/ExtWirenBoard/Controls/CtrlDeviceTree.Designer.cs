namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    partial class CtrlDeviceTree
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView = new System.Windows.Forms.TreeView();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.pnlTopLeft = new System.Windows.Forms.Panel();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.pnlTopLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.CheckBoxes = true;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.ilTree;
            this.treeView.Location = new System.Drawing.Point(3, 32);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(232, 241);
            this.treeView.TabIndex = 2;
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // ilTree
            // 
            this.ilTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilTree.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.propertyGrid, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.treeView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.pnlTopLeft, 0, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(476, 276);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(241, 32);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(232, 241);
            this.propertyGrid.TabIndex = 3;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // pnlTopLeft
            // 
            this.pnlTopLeft.Controls.Add(this.btnSelectNone);
            this.pnlTopLeft.Controls.Add(this.btnSelectAll);
            this.pnlTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopLeft.Location = new System.Drawing.Point(3, 3);
            this.pnlTopLeft.Name = "pnlTopLeft";
            this.pnlTopLeft.Size = new System.Drawing.Size(232, 23);
            this.pnlTopLeft.TabIndex = 1;
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(96, 0);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(90, 23);
            this.btnSelectNone.TabIndex = 1;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(0, 0);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(90, 23);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // CtrlDeviceTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "CtrlDeviceTree";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.Size = new System.Drawing.Size(500, 300);
            this.tableLayoutPanel.ResumeLayout(false);
            this.pnlTopLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private TreeView treeView;
        private TableLayoutPanel tableLayoutPanel;
        private Panel pnlTopLeft;
        private Button btnSelectAll;
        private Button btnSelectNone;
        private PropertyGrid propertyGrid;
        private ImageList ilTree;
    }
}
