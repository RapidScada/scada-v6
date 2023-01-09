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
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.cbDataKind = new System.Windows.Forms.ComboBox();
            this.lblDataKind = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSelectDeviceNum = new System.Windows.Forms.Button();
            this.btnEditDeviceNum = new System.Windows.Forms.Button();
            this.txtDeviceNum = new System.Windows.Forms.TextBox();
            this.lblDeviceNum = new System.Windows.Forms.Label();
            this.btnSelectObjNum = new System.Windows.Forms.Button();
            this.btnEditObjNum = new System.Windows.Forms.Button();
            this.txtObjNum = new System.Windows.Forms.TextBox();
            this.lblObjNum = new System.Windows.Forms.Label();
            this.btnSelectCnlNum = new System.Windows.Forms.Button();
            this.btnEditCnlNum = new System.Windows.Forms.Button();
            this.txtCnlNum = new System.Windows.Forms.TextBox();
            this.lblCnlNum = new System.Windows.Forms.Label();
            this.gbQuery = new System.Windows.Forms.GroupBox();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.btnEditParametrs = new System.Windows.Forms.Button();
            this.chkSingleQuery = new System.Windows.Forms.CheckBox();
            this.gbGeneral.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbQuery.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbGeneral
            // 
            this.gbGeneral.Controls.Add(this.cbDataKind);
            this.gbGeneral.Controls.Add(this.lblDataKind);
            this.gbGeneral.Controls.Add(this.txtName);
            this.gbGeneral.Controls.Add(this.lblName);
            this.gbGeneral.Controls.Add(this.chkActive);
            this.gbGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbGeneral.Location = new System.Drawing.Point(0, 0);
            this.gbGeneral.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneral.Size = new System.Drawing.Size(404, 98);
            this.gbGeneral.TabIndex = 0;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "General Options";
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
            this.cbDataKind.Location = new System.Drawing.Point(269, 62);
            this.cbDataKind.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbDataKind.Name = "cbDataKind";
            this.cbDataKind.Size = new System.Drawing.Size(122, 23);
            this.cbDataKind.TabIndex = 5;
            this.cbDataKind.SelectedIndexChanged += new System.EventHandler(this.cbDataKind_SelectedIndexChanged);
            // 
            // lblDataKind
            // 
            this.lblDataKind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataKind.AutoSize = true;
            this.lblDataKind.Location = new System.Drawing.Point(266, 44);
            this.lblDataKind.Name = "lblDataKind";
            this.lblDataKind.Size = new System.Drawing.Size(57, 15);
            this.lblDataKind.TabIndex = 4;
            this.lblDataKind.Text = "Data kind";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(13, 62);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 23);
            this.txtName.TabIndex = 3;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(13, 22);
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
            this.gbFilter.Controls.Add(this.btnEditDeviceNum);
            this.gbFilter.Controls.Add(this.txtDeviceNum);
            this.gbFilter.Controls.Add(this.lblDeviceNum);
            this.gbFilter.Controls.Add(this.btnSelectObjNum);
            this.gbFilter.Controls.Add(this.btnEditObjNum);
            this.gbFilter.Controls.Add(this.txtObjNum);
            this.gbFilter.Controls.Add(this.lblObjNum);
            this.gbFilter.Controls.Add(this.btnSelectCnlNum);
            this.gbFilter.Controls.Add(this.btnEditCnlNum);
            this.gbFilter.Controls.Add(this.txtCnlNum);
            this.gbFilter.Controls.Add(this.lblCnlNum);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFilter.Location = new System.Drawing.Point(0, 98);
            this.gbFilter.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbFilter.Size = new System.Drawing.Size(404, 163);
            this.gbFilter.TabIndex = 1;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnSelectDeviceNum
            // 
            this.btnSelectDeviceNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectDeviceNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectDeviceNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectDeviceNum.Location = new System.Drawing.Point(368, 126);
            this.btnSelectDeviceNum.Name = "btnSelectDeviceNum";
            this.btnSelectDeviceNum.Size = new System.Drawing.Size(23, 24);
            this.btnSelectDeviceNum.TabIndex = 12;
            this.btnSelectDeviceNum.UseVisualStyleBackColor = true;
            this.btnSelectDeviceNum.Click += new System.EventHandler(this.btnSelectDeviceNum_Click);
            // 
            // btnEditDeviceNum
            // 
            this.btnEditDeviceNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditDeviceNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditDeviceNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.edit;
            this.btnEditDeviceNum.Location = new System.Drawing.Point(339, 126);
            this.btnEditDeviceNum.Name = "btnEditDeviceNum";
            this.btnEditDeviceNum.Size = new System.Drawing.Size(23, 24);
            this.btnEditDeviceNum.TabIndex = 11;
            this.btnEditDeviceNum.UseVisualStyleBackColor = true;
            this.btnEditDeviceNum.Click += new System.EventHandler(this.btnEditDeviceNum_Click);
            // 
            // txtDeviceNum
            // 
            this.txtDeviceNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeviceNum.Location = new System.Drawing.Point(13, 127);
            this.txtDeviceNum.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtDeviceNum.Name = "txtDeviceNum";
            this.txtDeviceNum.Size = new System.Drawing.Size(320, 23);
            this.txtDeviceNum.TabIndex = 10;
            this.txtDeviceNum.TextChanged += new System.EventHandler(this.txtDeviceNum_TextChanged);
            this.txtDeviceNum.Enter += new System.EventHandler(this.txtDeviceNum_Enter);
            this.txtDeviceNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDeviceNum_KeyDown);
            this.txtDeviceNum.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeviceNum_Validating);
            // 
            // lblDeviceNum
            // 
            this.lblDeviceNum.AutoSize = true;
            this.lblDeviceNum.Location = new System.Drawing.Point(10, 109);
            this.lblDeviceNum.Name = "lblDeviceNum";
            this.lblDeviceNum.Size = new System.Drawing.Size(92, 15);
            this.lblDeviceNum.TabIndex = 9;
            this.lblDeviceNum.Text = "Device numbers";
            // 
            // btnSelectObjNum
            // 
            this.btnSelectObjNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectObjNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectObjNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectObjNum.Location = new System.Drawing.Point(368, 80);
            this.btnSelectObjNum.Name = "btnSelectObjNum";
            this.btnSelectObjNum.Size = new System.Drawing.Size(23, 24);
            this.btnSelectObjNum.TabIndex = 8;
            this.btnSelectObjNum.UseVisualStyleBackColor = true;
            this.btnSelectObjNum.Click += new System.EventHandler(this.btnSelectObjNum_Click);
            // 
            // btnEditObjNum
            // 
            this.btnEditObjNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditObjNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditObjNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.edit;
            this.btnEditObjNum.Location = new System.Drawing.Point(339, 80);
            this.btnEditObjNum.Name = "btnEditObjNum";
            this.btnEditObjNum.Size = new System.Drawing.Size(23, 24);
            this.btnEditObjNum.TabIndex = 7;
            this.btnEditObjNum.UseVisualStyleBackColor = true;
            this.btnEditObjNum.Click += new System.EventHandler(this.btnEditObjNum_Click);
            // 
            // txtObjNum
            // 
            this.txtObjNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjNum.Location = new System.Drawing.Point(13, 81);
            this.txtObjNum.Name = "txtObjNum";
            this.txtObjNum.Size = new System.Drawing.Size(320, 23);
            this.txtObjNum.TabIndex = 6;
            this.txtObjNum.TextChanged += new System.EventHandler(this.txtObjNum_TextChanged);
            this.txtObjNum.Enter += new System.EventHandler(this.txtObjNum_Enter);
            this.txtObjNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtObjNum_KeyDown);
            this.txtObjNum.Validating += new System.ComponentModel.CancelEventHandler(this.txtObjNum_Validating);
            // 
            // lblObjNum
            // 
            this.lblObjNum.AutoSize = true;
            this.lblObjNum.Location = new System.Drawing.Point(10, 63);
            this.lblObjNum.Name = "lblObjNum";
            this.lblObjNum.Size = new System.Drawing.Size(92, 15);
            this.lblObjNum.TabIndex = 5;
            this.lblObjNum.Text = "Object numbers";
            // 
            // btnSelectCnlNum
            // 
            this.btnSelectCnlNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectCnlNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectCnlNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectCnlNum.Location = new System.Drawing.Point(368, 36);
            this.btnSelectCnlNum.Name = "btnSelectCnlNum";
            this.btnSelectCnlNum.Size = new System.Drawing.Size(23, 24);
            this.btnSelectCnlNum.TabIndex = 4;
            this.btnSelectCnlNum.UseVisualStyleBackColor = true;
            this.btnSelectCnlNum.Click += new System.EventHandler(this.btnSelectCnlNum_Click);
            // 
            // btnEditCnlNum
            // 
            this.btnEditCnlNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditCnlNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditCnlNum.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.edit;
            this.btnEditCnlNum.Location = new System.Drawing.Point(339, 36);
            this.btnEditCnlNum.Name = "btnEditCnlNum";
            this.btnEditCnlNum.Size = new System.Drawing.Size(23, 24);
            this.btnEditCnlNum.TabIndex = 3;
            this.btnEditCnlNum.UseVisualStyleBackColor = true;
            this.btnEditCnlNum.Click += new System.EventHandler(this.btnEditCnlNum_Click);
            // 
            // txtCnlNum
            // 
            this.txtCnlNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCnlNum.Location = new System.Drawing.Point(13, 37);
            this.txtCnlNum.Name = "txtCnlNum";
            this.txtCnlNum.Size = new System.Drawing.Size(320, 23);
            this.txtCnlNum.TabIndex = 2;
            this.txtCnlNum.TextChanged += new System.EventHandler(this.txtCnlNum_TextChanged);
            this.txtCnlNum.Enter += new System.EventHandler(this.txtCnlNum_Enter);
            this.txtCnlNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCnlNum_KeyDown);
            this.txtCnlNum.Validating += new System.ComponentModel.CancelEventHandler(this.txtCnlNum_Validating);
            // 
            // lblCnlNum
            // 
            this.lblCnlNum.AutoSize = true;
            this.lblCnlNum.Location = new System.Drawing.Point(10, 19);
            this.lblCnlNum.Name = "lblCnlNum";
            this.lblCnlNum.Size = new System.Drawing.Size(101, 15);
            this.lblCnlNum.TabIndex = 1;
            this.lblCnlNum.Text = "Channel numbers";
            // 
            // gbQuery
            // 
            this.gbQuery.Controls.Add(this.txtSql);
            this.gbQuery.Controls.Add(this.btnEditParametrs);
            this.gbQuery.Controls.Add(this.chkSingleQuery);
            this.gbQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbQuery.Location = new System.Drawing.Point(0, 261);
            this.gbQuery.Name = "gbQuery";
            this.gbQuery.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbQuery.Size = new System.Drawing.Size(404, 201);
            this.gbQuery.TabIndex = 2;
            this.gbQuery.TabStop = false;
            this.gbQuery.Text = "Query";
            // 
            // txtSql
            // 
            this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSql.Location = new System.Drawing.Point(13, 47);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSql.Size = new System.Drawing.Size(378, 141);
            this.txtSql.TabIndex = 3;
            this.txtSql.WordWrap = false;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // btnEditParametrs
            // 
            this.btnEditParametrs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditParametrs.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditParametrs.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.parameters;
            this.btnEditParametrs.Location = new System.Drawing.Point(368, 18);
            this.btnEditParametrs.Name = "btnEditParametrs";
            this.btnEditParametrs.Size = new System.Drawing.Size(23, 24);
            this.btnEditParametrs.TabIndex = 2;
            this.btnEditParametrs.UseVisualStyleBackColor = true;
            this.btnEditParametrs.Click += new System.EventHandler(this.btnEditParametrs_Click);
            // 
            // chkSingleQuery
            // 
            this.chkSingleQuery.AutoSize = true;
            this.chkSingleQuery.Location = new System.Drawing.Point(13, 22);
            this.chkSingleQuery.Name = "chkSingleQuery";
            this.chkSingleQuery.Size = new System.Drawing.Size(227, 19);
            this.chkSingleQuery.TabIndex = 1;
            this.chkSingleQuery.Text = "Single query (input numbers required)";
            this.chkSingleQuery.UseVisualStyleBackColor = true;
            this.chkSingleQuery.CheckedChanged += new System.EventHandler(this.chkSingleQuery_CheckedChanged);
            // 
            // CtrlQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbQuery);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.gbGeneral);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.Name = "CtrlQuery";
            this.Size = new System.Drawing.Size(404, 462);
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.gbQuery.ResumeLayout(false);
            this.gbQuery.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbGeneral;
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
