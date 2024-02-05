
namespace Scada.Forms.Forms
{
    partial class FrmEntitySelect
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
            lblFilter = new Label();
            txtFilter = new TextBox();
            btnApplyFilter = new Button();
            chkOnlySelected = new CheckBox();
            dataGridView = new DataGridView();
            colSelected = new DataGridViewCheckBoxColumn();
            colID = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colCode = new DataGridViewTextBoxColumn();
            colDescr = new DataGridViewTextBoxColumn();
            btnSelect = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Location = new Point(9, 9);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(33, 15);
            lblFilter.TabIndex = 0;
            lblFilter.Text = "Filter";
            // 
            // txtFilter
            // 
            txtFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFilter.Location = new Point(12, 27);
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new Size(374, 23);
            txtFilter.TabIndex = 1;
            txtFilter.KeyDown += txtFilter_KeyDown;
            // 
            // btnApplyFilter
            // 
            btnApplyFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnApplyFilter.Location = new Point(392, 27);
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Size = new Size(80, 23);
            btnApplyFilter.TabIndex = 2;
            btnApplyFilter.Text = "Apply";
            btnApplyFilter.UseVisualStyleBackColor = true;
            btnApplyFilter.Click += btnApplyFilter_Click;
            // 
            // chkOnlySelected
            // 
            chkOnlySelected.AutoSize = true;
            chkOnlySelected.Location = new Point(12, 56);
            chkOnlySelected.Name = "chkOnlySelected";
            chkOnlySelected.Size = new Size(155, 19);
            chkOnlySelected.TabIndex = 3;
            chkOnlySelected.Text = "Show only selected rows";
            chkOnlySelected.UseVisualStyleBackColor = true;
            chkOnlySelected.CheckedChanged += btnApplyFilter_Click;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { colSelected, colID, colName, colCode, colDescr });
            dataGridView.Location = new Point(12, 81);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new Size(460, 429);
            dataGridView.StandardTab = true;
            dataGridView.TabIndex = 4;
            dataGridView.CellMouseUp += dataGridView_CellMouseUp;
            // 
            // colSelected
            // 
            colSelected.DataPropertyName = "Selected";
            colSelected.HeaderText = "Selected";
            colSelected.Name = "colSelected";
            colSelected.Resizable = DataGridViewTriState.True;
            colSelected.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // colID
            // 
            colID.DataPropertyName = "ID";
            colID.HeaderText = "ID";
            colID.Name = "colID";
            colID.ReadOnly = true;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colCode
            // 
            colCode.DataPropertyName = "Code";
            colCode.HeaderText = "Code";
            colCode.Name = "colCode";
            colCode.ReadOnly = true;
            // 
            // colDescr
            // 
            colDescr.DataPropertyName = "Descr";
            colDescr.HeaderText = "Description";
            colDescr.Name = "colDescr";
            colDescr.ReadOnly = true;
            // 
            // btnSelect
            // 
            btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSelect.Location = new Point(316, 526);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(75, 23);
            btnSelect.TabIndex = 5;
            btnSelect.Text = "Select";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(397, 526);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmEntitySelect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(484, 561);
            Controls.Add(btnCancel);
            Controls.Add(btnSelect);
            Controls.Add(dataGridView);
            Controls.Add(chkOnlySelected);
            Controls.Add(btnApplyFilter);
            Controls.Add(txtFilter);
            Controls.Add(lblFilter);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(200, 300);
            Name = "FrmEntitySelect";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select from {0}";
            Load += FrmEntitySelect_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.CheckBox chkOnlySelected;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescr;
    }
}