namespace Scada.Forms.Forms
{
    partial class FrmCnlSelect
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
            chkOnlySelected = new CheckBox();
            btnApplyFilter = new Button();
            txtFilter = new TextBox();
            lblFilter = new Label();
            lblObj = new Label();
            cbObj = new ComboBox();
            lblDevice = new Label();
            cbDevice = new ComboBox();
            dataGridView = new DataGridView();
            colSelected = new DataGridViewCheckBoxColumn();
            colCnlNum = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            btnCancel = new Button();
            btnSelect = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // chkOnlySelected
            // 
            chkOnlySelected.AutoSize = true;
            chkOnlySelected.Location = new Point(12, 100);
            chkOnlySelected.Name = "chkOnlySelected";
            chkOnlySelected.Size = new Size(155, 19);
            chkOnlySelected.TabIndex = 7;
            chkOnlySelected.Text = "Show only selected rows";
            chkOnlySelected.UseVisualStyleBackColor = true;
            chkOnlySelected.CheckedChanged += btnApplyFilter_Click;
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
            // txtFilter
            // 
            txtFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFilter.Location = new Point(12, 27);
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new Size(374, 23);
            txtFilter.TabIndex = 1;
            txtFilter.KeyDown += txtFilter_KeyDown;
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
            // lblObj
            // 
            lblObj.AutoSize = true;
            lblObj.Location = new Point(9, 53);
            lblObj.Name = "lblObj";
            lblObj.Size = new Size(42, 15);
            lblObj.TabIndex = 3;
            lblObj.Text = "Object";
            // 
            // cbObj
            // 
            cbObj.DropDownStyle = ComboBoxStyle.DropDownList;
            cbObj.FormattingEnabled = true;
            cbObj.Location = new Point(12, 71);
            cbObj.Name = "cbObj";
            cbObj.Size = new Size(184, 23);
            cbObj.TabIndex = 4;
            // 
            // lblDevice
            // 
            lblDevice.AutoSize = true;
            lblDevice.Location = new Point(199, 53);
            lblDevice.Name = "lblDevice";
            lblDevice.Size = new Size(42, 15);
            lblDevice.TabIndex = 5;
            lblDevice.Text = "Device";
            // 
            // cbDevice
            // 
            cbDevice.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDevice.FormattingEnabled = true;
            cbDevice.Location = new Point(202, 71);
            cbDevice.Name = "cbDevice";
            cbDevice.Size = new Size(184, 23);
            cbDevice.TabIndex = 6;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { colSelected, colCnlNum, colName });
            dataGridView.Location = new Point(12, 125);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new Size(460, 385);
            dataGridView.StandardTab = true;
            dataGridView.TabIndex = 8;
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
            // colCnlNum
            // 
            colCnlNum.DataPropertyName = "CnlNum";
            colCnlNum.HeaderText = "Number";
            colCnlNum.Name = "colCnlNum";
            colCnlNum.ReadOnly = true;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.Width = 180;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(397, 526);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSelect.Location = new Point(316, 526);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(75, 23);
            btnSelect.TabIndex = 9;
            btnSelect.Text = "Select";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // FrmCnlSelect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(484, 561);
            Controls.Add(btnCancel);
            Controls.Add(btnSelect);
            Controls.Add(dataGridView);
            Controls.Add(chkOnlySelected);
            Controls.Add(cbDevice);
            Controls.Add(lblDevice);
            Controls.Add(cbObj);
            Controls.Add(lblObj);
            Controls.Add(btnApplyFilter);
            Controls.Add(txtFilter);
            Controls.Add(lblFilter);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCnlSelect";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Channels";
            Load += FrmCnlSelect_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox chkOnlySelected;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Label lblObj;
        private System.Windows.Forms.ComboBox cbObj;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCnlNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    }
}