namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmBaseTable
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
            bindingNavigator = new System.Windows.Forms.BindingNavigator(components);
            btnAddNew = new System.Windows.Forms.ToolStripButton();
            bindingSource = new System.Windows.Forms.BindingSource(components);
            lblCount = new System.Windows.Forms.ToolStripLabel();
            btnMoveFirst = new System.Windows.Forms.ToolStripButton();
            btnMovePrevious = new System.Windows.Forms.ToolStripButton();
            sep1 = new System.Windows.Forms.ToolStripSeparator();
            txtPosition = new System.Windows.Forms.ToolStripTextBox();
            sep2 = new System.Windows.Forms.ToolStripSeparator();
            btnMoveNext = new System.Windows.Forms.ToolStripButton();
            btnMoveLast = new System.Windows.Forms.ToolStripButton();
            sep3 = new System.Windows.Forms.ToolStripSeparator();
            btnApplyEdit = new System.Windows.Forms.ToolStripButton();
            btnCancelEdit = new System.Windows.Forms.ToolStripButton();
            btnRefresh = new System.Windows.Forms.ToolStripButton();
            btnDelete = new System.Windows.Forms.ToolStripButton();
            btnClear = new System.Windows.Forms.ToolStripButton();
            sep4 = new System.Windows.Forms.ToolStripSeparator();
            btnCut = new System.Windows.Forms.ToolStripButton();
            btnCopy = new System.Windows.Forms.ToolStripButton();
            btnPaste = new System.Windows.Forms.ToolStripButton();
            sep5 = new System.Windows.Forms.ToolStripSeparator();
            btnFind = new System.Windows.Forms.ToolStripButton();
            btnFilter = new System.Windows.Forms.ToolStripButton();
            btnAutoSizeColumns = new System.Windows.Forms.ToolStripButton();
            btnProperties = new System.Windows.Forms.ToolStripButton();
            sep6 = new System.Windows.Forms.ToolStripSeparator();
            btnChangeObject = new System.Windows.Forms.ToolStripButton();
            btnBitReader = new System.Windows.Forms.ToolStripButton();
            dataGridView = new System.Windows.Forms.DataGridView();
            pnlError = new System.Windows.Forms.Panel();
            btnCloseError = new System.Windows.Forms.Button();
            lblError = new System.Windows.Forms.Label();
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            cmsTable = new System.Windows.Forms.ContextMenuStrip(components);
            miProperties = new System.Windows.Forms.ToolStripMenuItem();
            cmsChangeObject = new System.Windows.Forms.ContextMenuStrip(components);
            miComboBoxObject = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)bindingNavigator).BeginInit();
            bindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            pnlError.SuspendLayout();
            cmsTable.SuspendLayout();
            cmsChangeObject.SuspendLayout();
            SuspendLayout();
            // 
            // bindingNavigator
            // 
            bindingNavigator.AddNewItem = btnAddNew;
            bindingNavigator.BindingSource = bindingSource;
            bindingNavigator.CountItem = lblCount;
            bindingNavigator.DeleteItem = null;
            bindingNavigator.ImageScalingSize = new System.Drawing.Size(20, 20);
            bindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnMoveFirst, btnMovePrevious, sep1, txtPosition, lblCount, sep2, btnMoveNext, btnMoveLast, sep3, btnApplyEdit, btnCancelEdit, btnRefresh, btnAddNew, btnDelete, btnClear, sep4, btnCut, btnCopy, btnPaste, sep5, btnFind, btnFilter, btnAutoSizeColumns, btnProperties, sep6, btnChangeObject, btnBitReader });
            bindingNavigator.Location = new System.Drawing.Point(0, 53);
            bindingNavigator.MoveFirstItem = btnMoveFirst;
            bindingNavigator.MoveLastItem = btnMoveLast;
            bindingNavigator.MoveNextItem = btnMoveNext;
            bindingNavigator.MovePreviousItem = btnMovePrevious;
            bindingNavigator.Name = "bindingNavigator";
            bindingNavigator.PositionItem = txtPosition;
            bindingNavigator.Size = new System.Drawing.Size(667, 27);
            bindingNavigator.TabIndex = 1;
            // 
            // btnAddNew
            // 
            btnAddNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnAddNew.Image = Properties.Resources.add;
            btnAddNew.Name = "btnAddNew";
            btnAddNew.RightToLeftAutoMirrorImage = true;
            btnAddNew.Size = new System.Drawing.Size(29, 24);
            btnAddNew.Text = "Add New";
            // 
            // lblCount
            // 
            lblCount.Name = "lblCount";
            lblCount.Size = new System.Drawing.Size(50, 24);
            lblCount.Text = "sur {0}";
            lblCount.ToolTipText = "Total Number of Items";
            // 
            // btnMoveFirst
            // 
            btnMoveFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnMoveFirst.Image = Properties.Resources.move_first;
            btnMoveFirst.Name = "btnMoveFirst";
            btnMoveFirst.RightToLeftAutoMirrorImage = true;
            btnMoveFirst.Size = new System.Drawing.Size(29, 24);
            btnMoveFirst.Text = "Move First";
            // 
            // btnMovePrevious
            // 
            btnMovePrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnMovePrevious.Image = Properties.Resources.move_prev;
            btnMovePrevious.Name = "btnMovePrevious";
            btnMovePrevious.RightToLeftAutoMirrorImage = true;
            btnMovePrevious.Size = new System.Drawing.Size(29, 24);
            btnMovePrevious.Text = "Move Previous";
            // 
            // sep1
            // 
            sep1.Name = "sep1";
            sep1.Size = new System.Drawing.Size(6, 27);
            // 
            // txtPosition
            // 
            txtPosition.AccessibleName = "Position";
            txtPosition.AutoSize = false;
            txtPosition.Name = "txtPosition";
            txtPosition.Size = new System.Drawing.Size(57, 27);
            txtPosition.Text = "0";
            txtPosition.ToolTipText = "Current Position";
            // 
            // sep2
            // 
            sep2.Name = "sep2";
            sep2.Size = new System.Drawing.Size(6, 27);
            // 
            // btnMoveNext
            // 
            btnMoveNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnMoveNext.Image = Properties.Resources.move_next;
            btnMoveNext.Name = "btnMoveNext";
            btnMoveNext.RightToLeftAutoMirrorImage = true;
            btnMoveNext.Size = new System.Drawing.Size(29, 24);
            btnMoveNext.Text = "Move Next";
            // 
            // btnMoveLast
            // 
            btnMoveLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnMoveLast.Image = Properties.Resources.move_last;
            btnMoveLast.Name = "btnMoveLast";
            btnMoveLast.RightToLeftAutoMirrorImage = true;
            btnMoveLast.Size = new System.Drawing.Size(29, 24);
            btnMoveLast.Text = "Move Last";
            // 
            // sep3
            // 
            sep3.Name = "sep3";
            sep3.Size = new System.Drawing.Size(6, 27);
            // 
            // btnApplyEdit
            // 
            btnApplyEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnApplyEdit.Image = Properties.Resources.apply_edit;
            btnApplyEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnApplyEdit.Name = "btnApplyEdit";
            btnApplyEdit.Size = new System.Drawing.Size(29, 24);
            btnApplyEdit.Text = "Apply Edit Operation";
            btnApplyEdit.Click += btnApplyEdit_Click;
            // 
            // btnCancelEdit
            // 
            btnCancelEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnCancelEdit.Image = Properties.Resources.cancel_edit;
            btnCancelEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnCancelEdit.Name = "btnCancelEdit";
            btnCancelEdit.Size = new System.Drawing.Size(29, 24);
            btnCancelEdit.Text = "Cancel Edit Operation";
            btnCancelEdit.Click += btnCancelEdit_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnRefresh.Image = Properties.Resources.refresh;
            btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(29, 24);
            btnRefresh.Text = "Refresh Data";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnDelete
            // 
            btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnDelete.Image = Properties.Resources.delete;
            btnDelete.Name = "btnDelete";
            btnDelete.RightToLeftAutoMirrorImage = true;
            btnDelete.Size = new System.Drawing.Size(29, 24);
            btnDelete.Text = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnClear
            // 
            btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnClear.Image = Properties.Resources.clear;
            btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(29, 24);
            btnClear.Text = "Clear Table";
            btnClear.Click += btnClear_Click;
            // 
            // sep4
            // 
            sep4.Name = "sep4";
            sep4.Size = new System.Drawing.Size(6, 27);
            // 
            // btnCut
            // 
            btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnCut.Image = Properties.Resources.cut;
            btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnCut.Name = "btnCut";
            btnCut.Size = new System.Drawing.Size(29, 24);
            btnCut.Text = "Cut (Ctrl+X)";
            btnCut.Click += btnCut_Click;
            // 
            // btnCopy
            // 
            btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnCopy.Image = Properties.Resources.copy;
            btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(29, 24);
            btnCopy.Text = "Copy (Ctrl+C)";
            btnCopy.Click += btnCopy_Click;
            // 
            // btnPaste
            // 
            btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnPaste.Image = Properties.Resources.paste;
            btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(29, 24);
            btnPaste.Text = "Paste (Ctrl+V)";
            btnPaste.Click += btnPaste_Click;
            // 
            // sep5
            // 
            sep5.Name = "sep5";
            sep5.Size = new System.Drawing.Size(6, 27);
            // 
            // btnFind
            // 
            btnFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnFind.Image = Properties.Resources.find;
            btnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnFind.Name = "btnFind";
            btnFind.Size = new System.Drawing.Size(29, 24);
            btnFind.Text = "Find and Replace (Ctrl+F)";
            btnFind.Click += btnFind_Click;
            // 
            // btnFilter
            // 
            btnFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnFilter.Image = Properties.Resources.filter;
            btnFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new System.Drawing.Size(29, 24);
            btnFilter.Text = "Filter";
            btnFilter.Click += btnFilter_Click;
            // 
            // btnAutoSizeColumns
            // 
            btnAutoSizeColumns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnAutoSizeColumns.Image = Properties.Resources.resize;
            btnAutoSizeColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnAutoSizeColumns.Name = "btnAutoSizeColumns";
            btnAutoSizeColumns.Size = new System.Drawing.Size(29, 24);
            btnAutoSizeColumns.Text = "Autofit Column Widths";
            btnAutoSizeColumns.Click += btnAutoSizeColumns_Click;
            // 
            // btnProperties
            // 
            btnProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnProperties.Image = Properties.Resources.properties;
            btnProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnProperties.Name = "btnProperties";
            btnProperties.Size = new System.Drawing.Size(29, 24);
            btnProperties.Text = "Item Properties";
            btnProperties.Click += btnProperties_Click;
            // 
            // sep6
            // 
            sep6.Name = "sep6";
            sep6.Size = new System.Drawing.Size(6, 27);
            // 
            // btnChangeObject
            // 
            btnChangeObject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnChangeObject.Image = Properties.Resources.folder_closed;
            btnChangeObject.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnChangeObject.Name = "btnChangeObject";
            btnChangeObject.Size = new System.Drawing.Size(29, 24);
            btnChangeObject.Text = "Change the object of the selected item";
            btnChangeObject.Click += btnChangeObject_Click;
            // 
            // btnBitReader
            // 
            btnBitReader.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnBitReader.Enabled = false;
            btnBitReader.Image = Properties.Resources.instances;
            btnBitReader.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnBitReader.Name = "btnBitReader";
            btnBitReader.Size = new System.Drawing.Size(29, 24);
            btnBitReader.Text = "Bit reader";
            btnBitReader.Visible = false;
            btnBitReader.Click += btnBitReader_Click;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToOrderColumns = true;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnHeadersHeight = 29;
            dataGridView.DataSource = bindingSource;
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(0, 80);
            dataGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 51;
            dataGridView.ShowCellToolTips = false;
            dataGridView.Size = new System.Drawing.Size(667, 401);
            dataGridView.TabIndex = 2;
            dataGridView.CellClick += dataGridView_CellClick;
            dataGridView.CellFormatting += dataGridView_CellFormatting;
            dataGridView.CellMouseClick += dataGridView_CellMouseClick;
            dataGridView.CellMouseDoubleClick += dataGridView_CellMouseDoubleClick;
            dataGridView.CellValidating += dataGridView_CellValidating;
            dataGridView.DataError += dataGridView_DataError;
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.RowValidating += dataGridView_RowValidating;
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;
            // 
            // pnlError
            // 
            pnlError.AutoSize = true;
            pnlError.BackColor = System.Drawing.Color.FromArgb(242, 222, 222);
            pnlError.Controls.Add(btnCloseError);
            pnlError.Controls.Add(lblError);
            pnlError.Dock = System.Windows.Forms.DockStyle.Top;
            pnlError.ForeColor = System.Drawing.Color.FromArgb(169, 68, 66);
            pnlError.Location = new System.Drawing.Point(0, 0);
            pnlError.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlError.Name = "pnlError";
            pnlError.Size = new System.Drawing.Size(667, 53);
            pnlError.TabIndex = 0;
            pnlError.Visible = false;
            // 
            // btnCloseError
            // 
            btnCloseError.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCloseError.Location = new System.Drawing.Point(568, 11);
            btnCloseError.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnCloseError.Name = "btnCloseError";
            btnCloseError.Size = new System.Drawing.Size(86, 31);
            btnCloseError.TabIndex = 1;
            btnCloseError.Text = "Close";
            btnCloseError.UseVisualStyleBackColor = true;
            btnCloseError.Click += btnCloseError_Click;
            // 
            // lblError
            // 
            lblError.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblError.Location = new System.Drawing.Point(0, 0);
            lblError.Name = "lblError";
            lblError.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            lblError.Size = new System.Drawing.Size(547, 53);
            lblError.TabIndex = 0;
            lblError.Text = "Error message";
            lblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmsTable
            // 
            cmsTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmsTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miProperties });
            cmsTable.Name = "cmsTable";
            cmsTable.Size = new System.Drawing.Size(150, 30);
            // 
            // miProperties
            // 
            miProperties.Image = Properties.Resources.properties;
            miProperties.Name = "miProperties";
            miProperties.Size = new System.Drawing.Size(149, 26);
            miProperties.Text = "Properties";
            miProperties.Click += btnProperties_Click;
            // 
            // cmsChangeObject
            // 
            cmsChangeObject.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmsChangeObject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miComboBoxObject });
            cmsChangeObject.Name = "cmsChangeObject";
            cmsChangeObject.Size = new System.Drawing.Size(182, 36);
            // 
            // miComboBoxObject
            // 
            miComboBoxObject.Name = "miComboBoxObject";
            miComboBoxObject.Size = new System.Drawing.Size(121, 28);
            // 
            // FrmBaseTable
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(667, 481);
            Controls.Add(dataGridView);
            Controls.Add(bindingNavigator);
            Controls.Add(pnlError);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "FrmBaseTable";
            Text = "FrmBaseTable";
            Load += FrmBaseTable_Load;
            Shown += FrmBaseTable_Shown;
            VisibleChanged += FrmBaseTable_VisibleChanged;
            KeyDown += FrmBaseTable_KeyDown;
            ((System.ComponentModel.ISupportInitialize)bindingNavigator).EndInit();
            bindingNavigator.ResumeLayout(false);
            bindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            pnlError.ResumeLayout(false);
            cmsTable.ResumeLayout(false);
            cmsChangeObject.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator;
        private System.Windows.Forms.ToolStripButton btnAddNew;
        private System.Windows.Forms.ToolStripLabel lblCount;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnMoveFirst;
        private System.Windows.Forms.ToolStripButton btnMovePrevious;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripTextBox txtPosition;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripButton btnMoveNext;
        private System.Windows.Forms.ToolStripButton btnMoveLast;
        private System.Windows.Forms.ToolStripSeparator sep3;
        private System.Windows.Forms.Panel pnlError;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnCloseError;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripSeparator sep4;
        private System.Windows.Forms.ToolStripButton btnAutoSizeColumns;
        private System.Windows.Forms.ToolStripButton btnApplyEdit;
        private System.Windows.Forms.ToolStripButton btnCancelEdit;
        private System.Windows.Forms.ToolStripSeparator sep6;
        private System.Windows.Forms.ToolStripButton btnChangeObject;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripButton btnCut;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripButton btnPaste;
        private System.Windows.Forms.ToolStripSeparator sep5;
        private System.Windows.Forms.ToolStripButton btnFind;
        private System.Windows.Forms.ToolStripButton btnProperties;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ContextMenuStrip cmsTable;
        private System.Windows.Forms.ToolStripMenuItem miProperties;
        private System.Windows.Forms.ToolStripButton btnFilter;
        private System.Windows.Forms.ContextMenuStrip cmsChangeObject;
        private System.Windows.Forms.ToolStripComboBox miComboBoxObject;
        private System.Windows.Forms.ToolStripButton btnBitReader;
    }
}