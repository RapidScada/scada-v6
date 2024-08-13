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
            bindingNavigator = new BindingNavigator(components);
            btnAddNew = new ToolStripButton();
            bindingSource = new BindingSource(components);
            lblCount = new ToolStripLabel();
            btnMoveFirst = new ToolStripButton();
            btnMovePrevious = new ToolStripButton();
            sep1 = new ToolStripSeparator();
            txtPosition = new ToolStripTextBox();
            sep2 = new ToolStripSeparator();
            btnMoveNext = new ToolStripButton();
            btnMoveLast = new ToolStripButton();
            sep3 = new ToolStripSeparator();
            btnApplyEdit = new ToolStripButton();
            btnCancelEdit = new ToolStripButton();
            btnDelete = new ToolStripButton();
            btnClear = new ToolStripButton();
            sep4 = new ToolStripSeparator();
            btnCut = new ToolStripButton();
            btnCopy = new ToolStripButton();
            btnPaste = new ToolStripButton();
            sep5 = new ToolStripSeparator();
            btnFilter = new ToolStripButton();
            btnAutoSizeColumns = new ToolStripButton();
            btnProperties = new ToolStripButton();
            dataGridView = new DataGridView();
            pnlError = new Panel();
            btnCloseError = new Button();
            lblError = new Label();
            openFileDialog = new OpenFileDialog();
            folderBrowserDialog = new FolderBrowserDialog();
            cmsTable = new ContextMenuStrip(components);
            miProperties = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)bindingNavigator).BeginInit();
            bindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            pnlError.SuspendLayout();
            cmsTable.SuspendLayout();
            SuspendLayout();
            // 
            // bindingNavigator
            // 
            bindingNavigator.AddNewItem = btnAddNew;
            bindingNavigator.BindingSource = bindingSource;
            bindingNavigator.CountItem = lblCount;
            bindingNavigator.DeleteItem = null;
            bindingNavigator.Items.AddRange(new ToolStripItem[] { btnMoveFirst, btnMovePrevious, sep1, txtPosition, lblCount, sep2, btnMoveNext, btnMoveLast, sep3, btnApplyEdit, btnCancelEdit, btnAddNew, btnDelete, btnClear, sep4, btnCut, btnCopy, btnPaste, sep5, btnFilter, btnAutoSizeColumns, btnProperties });
            bindingNavigator.Location = new Point(0, 40);
            bindingNavigator.MoveFirstItem = btnMoveFirst;
            bindingNavigator.MoveLastItem = btnMoveLast;
            bindingNavigator.MoveNextItem = btnMoveNext;
            bindingNavigator.MovePreviousItem = btnMovePrevious;
            bindingNavigator.Name = "bindingNavigator";
            bindingNavigator.PositionItem = txtPosition;
            bindingNavigator.Size = new Size(584, 25);
            bindingNavigator.TabIndex = 1;
            // 
            // btnAddNew
            // 
            btnAddNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddNew.Image = Properties.Resources.add;
            btnAddNew.Name = "btnAddNew";
            btnAddNew.RightToLeftAutoMirrorImage = true;
            btnAddNew.Size = new Size(23, 22);
            btnAddNew.Text = "Add New";
            // 
            // lblCount
            // 
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(35, 22);
            lblCount.Text = "of {0}";
            lblCount.ToolTipText = "Total Number of Items";
            // 
            // btnMoveFirst
            // 
            btnMoveFirst.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveFirst.Image = Properties.Resources.move_first;
            btnMoveFirst.Name = "btnMoveFirst";
            btnMoveFirst.RightToLeftAutoMirrorImage = true;
            btnMoveFirst.Size = new Size(23, 22);
            btnMoveFirst.Text = "Move First";
            // 
            // btnMovePrevious
            // 
            btnMovePrevious.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMovePrevious.Image = Properties.Resources.move_prev;
            btnMovePrevious.Name = "btnMovePrevious";
            btnMovePrevious.RightToLeftAutoMirrorImage = true;
            btnMovePrevious.Size = new Size(23, 22);
            btnMovePrevious.Text = "Move Previous";
            // 
            // sep1
            // 
            sep1.Name = "sep1";
            sep1.Size = new Size(6, 25);
            // 
            // txtPosition
            // 
            txtPosition.AccessibleName = "Position";
            txtPosition.AutoSize = false;
            txtPosition.Name = "txtPosition";
            txtPosition.Size = new Size(50, 23);
            txtPosition.Text = "0";
            txtPosition.ToolTipText = "Current Position";
            // 
            // sep2
            // 
            sep2.Name = "sep2";
            sep2.Size = new Size(6, 25);
            // 
            // btnMoveNext
            // 
            btnMoveNext.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveNext.Image = Properties.Resources.move_next;
            btnMoveNext.Name = "btnMoveNext";
            btnMoveNext.RightToLeftAutoMirrorImage = true;
            btnMoveNext.Size = new Size(23, 22);
            btnMoveNext.Text = "Move Next";
            // 
            // btnMoveLast
            // 
            btnMoveLast.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveLast.Image = Properties.Resources.move_last;
            btnMoveLast.Name = "btnMoveLast";
            btnMoveLast.RightToLeftAutoMirrorImage = true;
            btnMoveLast.Size = new Size(23, 22);
            btnMoveLast.Text = "Move Last";
            // 
            // sep3
            // 
            sep3.Name = "sep3";
            sep3.Size = new Size(6, 25);
            // 
            // btnApplyEdit
            // 
            btnApplyEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnApplyEdit.Image = Properties.Resources.apply_edit;
            btnApplyEdit.ImageTransparentColor = Color.Magenta;
            btnApplyEdit.Name = "btnApplyEdit";
            btnApplyEdit.Size = new Size(23, 22);
            btnApplyEdit.Text = "Apply Edit Operation";
            btnApplyEdit.Click += btnApplyEdit_Click;
            // 
            // btnCancelEdit
            // 
            btnCancelEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnCancelEdit.Image = Properties.Resources.cancel_edit;
            btnCancelEdit.ImageTransparentColor = Color.Magenta;
            btnCancelEdit.Name = "btnCancelEdit";
            btnCancelEdit.Size = new Size(23, 22);
            btnCancelEdit.Text = "Cancel Edit Operation";
            btnCancelEdit.Click += btnCancelEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDelete.Image = Properties.Resources.delete;
            btnDelete.Name = "btnDelete";
            btnDelete.RightToLeftAutoMirrorImage = true;
            btnDelete.Size = new Size(23, 22);
            btnDelete.Text = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnClear
            // 
            btnClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnClear.Image = Properties.Resources.clear;
            btnClear.ImageTransparentColor = Color.Magenta;
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(23, 22);
            btnClear.Text = "Clear Table";
            btnClear.Click += btnClear_Click;
            // 
            // sep4
            // 
            sep4.Name = "sep4";
            sep4.Size = new Size(6, 25);
            // 
            // btnCut
            // 
            btnCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnCut.Image = Properties.Resources.cut;
            btnCut.ImageTransparentColor = Color.Magenta;
            btnCut.Name = "btnCut";
            btnCut.Size = new Size(23, 22);
            btnCut.Text = "Cut (Ctrl+X)";
            btnCut.Click += btnCut_Click;
            // 
            // btnCopy
            // 
            btnCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnCopy.Image = Properties.Resources.copy;
            btnCopy.ImageTransparentColor = Color.Magenta;
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(23, 22);
            btnCopy.Text = "Copy (Ctrl+C)";
            btnCopy.Click += btnCopy_Click;
            // 
            // btnPaste
            // 
            btnPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnPaste.Image = Properties.Resources.paste;
            btnPaste.ImageTransparentColor = Color.Magenta;
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(23, 22);
            btnPaste.Text = "Paste (Ctrl+V)";
            btnPaste.Click += btnPaste_Click;
            // 
            // sep5
            // 
            sep5.Name = "sep5";
            sep5.Size = new Size(6, 25);
            // 
            // btnFilter
            // 
            btnFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnFilter.Image = Properties.Resources.filter;
            btnFilter.ImageTransparentColor = Color.Magenta;
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(23, 22);
            btnFilter.Text = "Filter";
            btnFilter.Click += btnFilter_Click;
            // 
            // btnAutoSizeColumns
            // 
            btnAutoSizeColumns.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAutoSizeColumns.Image = Properties.Resources.resize;
            btnAutoSizeColumns.ImageTransparentColor = Color.Magenta;
            btnAutoSizeColumns.Name = "btnAutoSizeColumns";
            btnAutoSizeColumns.Size = new Size(23, 22);
            btnAutoSizeColumns.Text = "Autofit Column Widths";
            btnAutoSizeColumns.Click += btnAutoSizeColumns_Click;
            // 
            // btnProperties
            // 
            btnProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnProperties.Image = Properties.Resources.properties;
            btnProperties.ImageTransparentColor = Color.Magenta;
            btnProperties.Name = "btnProperties";
            btnProperties.Size = new Size(23, 22);
            btnProperties.Text = "Item Properties";
            btnProperties.Click += btnProperties_Click;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToOrderColumns = true;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = bindingSource;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Location = new Point(0, 65);
            dataGridView.Name = "dataGridView";
            dataGridView.ShowCellToolTips = false;
            dataGridView.Size = new Size(584, 296);
            dataGridView.TabIndex = 2;
            dataGridView.CellClick += dataGridView_CellClick;
            dataGridView.CellFormatting += dataGridView_CellFormatting;
            dataGridView.CellMouseClick += dataGridView_CellMouseClick;
            dataGridView.CellMouseDoubleClick += dataGridView_CellMouseDoubleClick;
            dataGridView.CellValidating += dataGridView_CellValidating;
            dataGridView.DataError += dataGridView_DataError;
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.RowValidating += dataGridView_RowValidating;
            // 
            // pnlError
            // 
            pnlError.AutoSize = true;
            pnlError.BackColor = Color.FromArgb(242, 222, 222);
            pnlError.Controls.Add(btnCloseError);
            pnlError.Controls.Add(lblError);
            pnlError.Dock = DockStyle.Top;
            pnlError.ForeColor = Color.FromArgb(169, 68, 66);
            pnlError.Location = new Point(0, 0);
            pnlError.Name = "pnlError";
            pnlError.Size = new Size(584, 40);
            pnlError.TabIndex = 0;
            pnlError.Visible = false;
            // 
            // btnCloseError
            // 
            btnCloseError.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCloseError.Location = new Point(497, 8);
            btnCloseError.Name = "btnCloseError";
            btnCloseError.Size = new Size(75, 23);
            btnCloseError.TabIndex = 1;
            btnCloseError.Text = "Close";
            btnCloseError.UseVisualStyleBackColor = true;
            btnCloseError.Click += btnCloseError_Click;
            // 
            // lblError
            // 
            lblError.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblError.Location = new Point(0, 0);
            lblError.Name = "lblError";
            lblError.Padding = new Padding(5);
            lblError.Size = new Size(479, 40);
            lblError.TabIndex = 0;
            lblError.Text = "Error message";
            lblError.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmsTable
            // 
            cmsTable.Items.AddRange(new ToolStripItem[] { miProperties });
            cmsTable.Name = "cmsTable";
            cmsTable.Size = new Size(128, 26);
            // 
            // miProperties
            // 
            miProperties.Image = Properties.Resources.properties;
            miProperties.Name = "miProperties";
            miProperties.Size = new Size(127, 22);
            miProperties.Text = "Properties";
            miProperties.Click += btnProperties_Click;
            // 
            // FrmBaseTable
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 361);
            Controls.Add(dataGridView);
            Controls.Add(bindingNavigator);
            Controls.Add(pnlError);
            KeyPreview = true;
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
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripSeparator sep4;
        private System.Windows.Forms.ToolStripButton btnAutoSizeColumns;
        private System.Windows.Forms.ToolStripButton btnApplyEdit;
        private System.Windows.Forms.ToolStripButton btnCancelEdit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripButton btnCut;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripButton btnPaste;
        private System.Windows.Forms.ToolStripSeparator sep5;
        private System.Windows.Forms.ToolStripButton btnProperties;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ContextMenuStrip cmsTable;
        private System.Windows.Forms.ToolStripMenuItem miProperties;
        private System.Windows.Forms.ToolStripButton btnFilter;
    }
}