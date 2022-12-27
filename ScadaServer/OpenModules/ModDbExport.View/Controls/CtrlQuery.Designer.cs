namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    partial class CtrlQuery
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
            this.gbName = new System.Windows.Forms.GroupBox();
            this.lblDataKind = new System.Windows.Forms.Label();
            this.cbDataKind = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSelectDeviceNum = new System.Windows.Forms.Button();
            this.btnSelectObjNum = new System.Windows.Forms.Button();
            this.btnSelectCnlNum = new System.Windows.Forms.Button();
            this.btnEditDeviceNum = new System.Windows.Forms.Button();
            this.txtDeviceNum = new System.Windows.Forms.TextBox();
            this.lblDeviceNum = new System.Windows.Forms.Label();
            this.btnEditObjNum = new System.Windows.Forms.Button();
            this.txtObjNum = new System.Windows.Forms.TextBox();
            this.lblObjNum = new System.Windows.Forms.Label();
            this.btnEditCnlNum = new System.Windows.Forms.Button();
            this.txtCnlNum = new System.Windows.Forms.TextBox();
            this.lblCnlNum = new System.Windows.Forms.Label();
            this.gbQuery = new System.Windows.Forms.GroupBox();
            this.btnEditParametrs = new System.Windows.Forms.Button();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.chkSingleQuery = new System.Windows.Forms.CheckBox();
            this.gbName.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbQuery.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbName
            // 
            this.gbName.Controls.Add(this.lblDataKind);
            this.gbName.Controls.Add(this.cbDataKind);
            this.gbName.Controls.Add(this.txtName);
            this.gbName.Controls.Add(this.lblName);
            this.gbName.Controls.Add(this.chkActive);
            this.gbName.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbName.Location = new System.Drawing.Point(0, 0);
            this.gbName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbName.Name = "gbName";
            this.gbName.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbName.Size = new System.Drawing.Size(404, 99);
            this.gbName.TabIndex = 0;
            this.gbName.TabStop = false;
            this.gbName.Text = " General Options";
            // 
            // lblDataKind
            // 
            this.lblDataKind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataKind.AutoSize = true;
            this.lblDataKind.Location = new System.Drawing.Point(256, 51);
            this.lblDataKind.Name = "lblDataKind";
            this.lblDataKind.Size = new System.Drawing.Size(57, 15);
            this.lblDataKind.TabIndex = 8;
            this.lblDataKind.Text = "Data kind";
            // 
            // cbDataKind
            // 
            this.cbDataKind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDataKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataKind.FormattingEnabled = true;
            this.cbDataKind.Items.AddRange(new object[] {
            "Current",
            "Historical",
            "Event",
            "EventAck",
            "Command"});
            this.cbDataKind.Location = new System.Drawing.Point(253, 69);
            this.cbDataKind.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbDataKind.Name = "cbDataKind";
            this.cbDataKind.Size = new System.Drawing.Size(138, 23);
            this.cbDataKind.TabIndex = 7;
            this.cbDataKind.SelectedIndexChanged += new System.EventHandler(this.cbDataKind_SelectedIndexChanged);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(10, 69);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(237, 23);
            this.txtName.TabIndex = 6;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 51);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(13, 22);
            this.chkActive.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(59, 19);
            this.chkActive.TabIndex = 1;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSelectDeviceNum);
            this.gbFilter.Controls.Add(this.btnSelectObjNum);
            this.gbFilter.Controls.Add(this.btnSelectCnlNum);
            this.gbFilter.Controls.Add(this.btnEditDeviceNum);
            this.gbFilter.Controls.Add(this.txtDeviceNum);
            this.gbFilter.Controls.Add(this.lblDeviceNum);
            this.gbFilter.Controls.Add(this.btnEditObjNum);
            this.gbFilter.Controls.Add(this.txtObjNum);
            this.gbFilter.Controls.Add(this.lblObjNum);
            this.gbFilter.Controls.Add(this.btnEditCnlNum);
            this.gbFilter.Controls.Add(this.txtCnlNum);
            this.gbFilter.Controls.Add(this.lblCnlNum);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFilter.Location = new System.Drawing.Point(0, 99);
            this.gbFilter.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbFilter.Size = new System.Drawing.Size(404, 173);
            this.gbFilter.TabIndex = 1;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnSelectDeviceNum
            // 
            this.btnSelectDeviceNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectDeviceNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectDeviceNum.Location = new System.Drawing.Point(368, 138);
            this.btnSelectDeviceNum.Name = "btnSelectDeviceNum";
            this.btnSelectDeviceNum.Size = new System.Drawing.Size(23, 24);
            this.btnSelectDeviceNum.TabIndex = 17;
            this.btnSelectDeviceNum.UseVisualStyleBackColor = true;
            this.btnSelectDeviceNum.Click += new System.EventHandler(this.btnSelectDeviceNum_Click);
            // 
            // btnSelectObjNum
            // 
            this.btnSelectObjNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectObjNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectObjNum.Location = new System.Drawing.Point(368, 87);
            this.btnSelectObjNum.Name = "btnSelectObjNum";
            this.btnSelectObjNum.Size = new System.Drawing.Size(23, 24);
            this.btnSelectObjNum.TabIndex = 16;
            this.btnSelectObjNum.UseVisualStyleBackColor = true;
            this.btnSelectObjNum.Click += new System.EventHandler(this.btnSelectObjNum_Click);
            // 
            // btnSelectCnlNum
            // 
            this.btnSelectCnlNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectCnlNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectCnlNum.Location = new System.Drawing.Point(368, 40);
            this.btnSelectCnlNum.Name = "btnSelectCnlNum";
            this.btnSelectCnlNum.Size = new System.Drawing.Size(23, 24);
            this.btnSelectCnlNum.TabIndex = 15;
            this.btnSelectCnlNum.UseVisualStyleBackColor = true;
            this.btnSelectCnlNum.Click += new System.EventHandler(this.btnSelectCnlNum_Click);
            // 
            // btnEditDeviceNum
            // 
            this.btnEditDeviceNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditDeviceNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.edit;
            this.btnEditDeviceNum.Location = new System.Drawing.Point(339, 138);
            this.btnEditDeviceNum.Name = "btnEditDeviceNum";
            this.btnEditDeviceNum.Size = new System.Drawing.Size(23, 24);
            this.btnEditDeviceNum.TabIndex = 14;
            this.btnEditDeviceNum.UseVisualStyleBackColor = true;
            this.btnEditDeviceNum.Click += new System.EventHandler(this.btnEditDeviceNum_Click);
            // 
            // txtDeviceNum
            // 
            this.txtDeviceNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeviceNum.Location = new System.Drawing.Point(10, 138);
            this.txtDeviceNum.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtDeviceNum.Name = "txtDeviceNum";
            this.txtDeviceNum.Size = new System.Drawing.Size(323, 23);
            this.txtDeviceNum.TabIndex = 13;
            this.txtDeviceNum.TextChanged += new System.EventHandler(this.txtDeviceNum_TextChanged);
            this.txtDeviceNum.Enter += new System.EventHandler(this.txtDeviceNum_Enter);
            this.txtDeviceNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDeviceNum_KeyDown);
            this.txtDeviceNum.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeviceNum_Validating);
            // 
            // lblDeviceNum
            // 
            this.lblDeviceNum.AutoSize = true;
            this.lblDeviceNum.Location = new System.Drawing.Point(10, 120);
            this.lblDeviceNum.Name = "lblDeviceNum";
            this.lblDeviceNum.Size = new System.Drawing.Size(92, 15);
            this.lblDeviceNum.TabIndex = 12;
            this.lblDeviceNum.Text = "Device numbers";
            // 
            // btnEditObjNum
            // 
            this.btnEditObjNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditObjNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.edit;
            this.btnEditObjNum.Location = new System.Drawing.Point(339, 87);
            this.btnEditObjNum.Name = "btnEditObjNum";
            this.btnEditObjNum.Size = new System.Drawing.Size(23, 24);
            this.btnEditObjNum.TabIndex = 11;
            this.btnEditObjNum.UseVisualStyleBackColor = true;
            this.btnEditObjNum.Click += new System.EventHandler(this.btnEditObjNum_Click);
            // 
            // txtObjNum
            // 
            this.txtObjNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjNum.Location = new System.Drawing.Point(10, 87);
            this.txtObjNum.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtObjNum.Name = "txtObjNum";
            this.txtObjNum.Size = new System.Drawing.Size(323, 23);
            this.txtObjNum.TabIndex = 10;
            this.txtObjNum.TextChanged += new System.EventHandler(this.txtObjNum_TextChanged);
            this.txtObjNum.Enter += new System.EventHandler(this.txtObjNum_Enter);
            this.txtObjNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtObjNum_KeyDown);
            this.txtObjNum.Validating += new System.ComponentModel.CancelEventHandler(this.txtObjNum_Validating);
            // 
            // lblObjNum
            // 
            this.lblObjNum.AutoSize = true;
            this.lblObjNum.Location = new System.Drawing.Point(10, 73);
            this.lblObjNum.Name = "lblObjNum";
            this.lblObjNum.Size = new System.Drawing.Size(92, 15);
            this.lblObjNum.TabIndex = 9;
            this.lblObjNum.Text = "Object numbers";
            // 
            // btnEditCnlNum
            // 
            this.btnEditCnlNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditCnlNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.edit;
            this.btnEditCnlNum.Location = new System.Drawing.Point(339, 40);
            this.btnEditCnlNum.Name = "btnEditCnlNum";
            this.btnEditCnlNum.Size = new System.Drawing.Size(23, 24);
            this.btnEditCnlNum.TabIndex = 8;
            this.btnEditCnlNum.UseVisualStyleBackColor = true;
            this.btnEditCnlNum.Click += new System.EventHandler(this.btnEditCnlNum_Click);
            // 
            // txtCnlNum
            // 
            this.txtCnlNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCnlNum.Location = new System.Drawing.Point(10, 40);
            this.txtCnlNum.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtCnlNum.Name = "txtCnlNum";
            this.txtCnlNum.Size = new System.Drawing.Size(323, 23);
            this.txtCnlNum.TabIndex = 7;
            this.txtCnlNum.TextChanged += new System.EventHandler(this.txtCnlNum_TextChanged);
            this.txtCnlNum.Enter += new System.EventHandler(this.txtCnlNum_Enter);
            this.txtCnlNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCnlNum_KeyDown);
            this.txtCnlNum.Validating += new System.ComponentModel.CancelEventHandler(this.txtCnlNum_Validating);
            // 
            // lblCnlNum
            // 
            this.lblCnlNum.AutoSize = true;
            this.lblCnlNum.Location = new System.Drawing.Point(10, 22);
            this.lblCnlNum.Name = "lblCnlNum";
            this.lblCnlNum.Size = new System.Drawing.Size(85, 15);
            this.lblCnlNum.TabIndex = 6;
            this.lblCnlNum.Text = "Input channels";
            // 
            // gbQuery
            // 
            this.gbQuery.Controls.Add(this.btnEditParametrs);
            this.gbQuery.Controls.Add(this.txtSql);
            this.gbQuery.Controls.Add(this.chkSingleQuery);
            this.gbQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbQuery.Location = new System.Drawing.Point(0, 272);
            this.gbQuery.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbQuery.Name = "gbQuery";
            this.gbQuery.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbQuery.Size = new System.Drawing.Size(404, 190);
            this.gbQuery.TabIndex = 2;
            this.gbQuery.TabStop = false;
            this.gbQuery.Text = "Query";
            // 
            // btnEditParametrs
            // 
            this.btnEditParametrs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditParametrs.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.edit;
            this.btnEditParametrs.Location = new System.Drawing.Point(368, 22);
            this.btnEditParametrs.Name = "btnEditParametrs";
            this.btnEditParametrs.Size = new System.Drawing.Size(23, 24);
            this.btnEditParametrs.TabIndex = 15;
            this.btnEditParametrs.UseVisualStyleBackColor = true;
            // 
            // txtSql
            // 
            this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSql.Location = new System.Drawing.Point(13, 52);
            this.txtSql.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSql.Size = new System.Drawing.Size(378, 127);
            this.txtSql.TabIndex = 7;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // chkSingleQuery
            // 
            this.chkSingleQuery.AutoSize = true;
            this.chkSingleQuery.Location = new System.Drawing.Point(10, 22);
            this.chkSingleQuery.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.chkSingleQuery.Name = "chkSingleQuery";
            this.chkSingleQuery.Size = new System.Drawing.Size(227, 19);
            this.chkSingleQuery.TabIndex = 2;
            this.chkSingleQuery.Text = "Single query (input channels required)";
            this.chkSingleQuery.UseVisualStyleBackColor = true;
            this.chkSingleQuery.CheckedChanged += new System.EventHandler(this.chkSingleQuery_CheckedChanged);
            // 
            // CtrlQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbQuery);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.gbName);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.Name = "CtrlQuery";
            this.Size = new System.Drawing.Size(404, 462);
            this.gbName.ResumeLayout(false);
            this.gbName.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.gbQuery.ResumeLayout(false);
            this.gbQuery.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbName;
        private CheckBox chkActive;
        private TextBox txtName;
        private Label lblName;
        private ComboBox cbDataKind;
        private Label lblDataKind;
        private GroupBox gbFilter;
        private GroupBox gbQuery;
        private TextBox txtCnlNum;
        private Label lblCnlNum;
        private Button btnEditCnlNum;
        private Button btnEditDeviceNum;
        private TextBox txtDeviceNum;
        private Label lblDeviceNum;
        private Button btnEditObjNum;
        private TextBox txtObjNum;
        private Label lblObjNum;
        private CheckBox chkSingleQuery;
        private TextBox txtSql;
        private Button btnEditParametrs;
        private Button btnSelectDeviceNum;
        private Button btnSelectObjNum;
        private Button btnSelectCnlNum;
    }
}
