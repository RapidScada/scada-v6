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
            components = new System.ComponentModel.Container();
            gbGeneral = new GroupBox();
            cbDataKind = new ComboBox();
            lblDataKind = new Label();
            txtName = new TextBox();
            lblName = new Label();
            chkActive = new CheckBox();
            gbFilter = new GroupBox();
            btnSelectDeviceNum = new Button();
            btnEditDeviceNum = new Button();
            txtDeviceNum = new TextBox();
            lblDeviceNum = new Label();
            btnSelectObjNum = new Button();
            btnEditObjNum = new Button();
            txtObjNum = new TextBox();
            lblObjNum = new Label();
            btnSelectCnlNum = new Button();
            btnEditCnlNum = new Button();
            txtCnlNum = new TextBox();
            lblCnlNum = new Label();
            gbQuery = new GroupBox();
            txtSql = new TextBox();
            btnViewParameters = new Button();
            chkSingleQuery = new CheckBox();
            toolTip = new ToolTip(components);
            gbGeneral.SuspendLayout();
            gbFilter.SuspendLayout();
            gbQuery.SuspendLayout();
            SuspendLayout();
            // 
            // gbGeneral
            // 
            gbGeneral.Controls.Add(cbDataKind);
            gbGeneral.Controls.Add(lblDataKind);
            gbGeneral.Controls.Add(txtName);
            gbGeneral.Controls.Add(lblName);
            gbGeneral.Controls.Add(chkActive);
            gbGeneral.Dock = DockStyle.Top;
            gbGeneral.Location = new Point(0, 0);
            gbGeneral.Margin = new Padding(3, 3, 3, 10);
            gbGeneral.Name = "gbGeneral";
            gbGeneral.Padding = new Padding(10, 3, 10, 10);
            gbGeneral.Size = new Size(404, 98);
            gbGeneral.TabIndex = 0;
            gbGeneral.TabStop = false;
            gbGeneral.Text = "General";
            // 
            // cbDataKind
            // 
            cbDataKind.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbDataKind.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDataKind.FormattingEnabled = true;
            cbDataKind.Items.AddRange(new object[] { "Current data", "Historical data", "Event", "Event acknowledgement", "Command" });
            cbDataKind.Location = new Point(229, 62);
            cbDataKind.Margin = new Padding(3, 3, 3, 10);
            cbDataKind.Name = "cbDataKind";
            cbDataKind.Size = new Size(162, 23);
            cbDataKind.TabIndex = 5;
            cbDataKind.SelectedIndexChanged += cbDataKind_SelectedIndexChanged;
            // 
            // lblDataKind
            // 
            lblDataKind.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDataKind.AutoSize = true;
            lblDataKind.Location = new Point(226, 44);
            lblDataKind.Name = "lblDataKind";
            lblDataKind.Size = new Size(57, 15);
            lblDataKind.TabIndex = 4;
            lblDataKind.Text = "Data kind";
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(13, 62);
            txtName.Margin = new Padding(3, 3, 3, 10);
            txtName.Name = "txtName";
            txtName.Size = new Size(210, 23);
            txtName.TabIndex = 3;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(10, 44);
            lblName.Name = "lblName";
            lblName.Size = new Size(39, 15);
            lblName.TabIndex = 2;
            lblName.Text = "Name";
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Location = new Point(13, 22);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(59, 19);
            chkActive.TabIndex = 1;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            chkActive.CheckedChanged += chkActive_CheckedChanged;
            // 
            // gbFilter
            // 
            gbFilter.Controls.Add(btnSelectDeviceNum);
            gbFilter.Controls.Add(btnEditDeviceNum);
            gbFilter.Controls.Add(txtDeviceNum);
            gbFilter.Controls.Add(lblDeviceNum);
            gbFilter.Controls.Add(btnSelectObjNum);
            gbFilter.Controls.Add(btnEditObjNum);
            gbFilter.Controls.Add(txtObjNum);
            gbFilter.Controls.Add(lblObjNum);
            gbFilter.Controls.Add(btnSelectCnlNum);
            gbFilter.Controls.Add(btnEditCnlNum);
            gbFilter.Controls.Add(txtCnlNum);
            gbFilter.Controls.Add(lblCnlNum);
            gbFilter.Dock = DockStyle.Top;
            gbFilter.Location = new Point(0, 98);
            gbFilter.Margin = new Padding(3, 3, 3, 10);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(10, 3, 10, 10);
            gbFilter.Size = new Size(404, 161);
            gbFilter.TabIndex = 1;
            gbFilter.TabStop = false;
            gbFilter.Text = "Filter";
            // 
            // btnSelectDeviceNum
            // 
            btnSelectDeviceNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectDeviceNum.FlatStyle = FlatStyle.Popup;
            btnSelectDeviceNum.Image = Properties.Resources.find;
            btnSelectDeviceNum.Location = new Point(368, 124);
            btnSelectDeviceNum.Name = "btnSelectDeviceNum";
            btnSelectDeviceNum.Size = new Size(23, 24);
            btnSelectDeviceNum.TabIndex = 12;
            btnSelectDeviceNum.UseVisualStyleBackColor = true;
            btnSelectDeviceNum.Click += btnSelectDeviceNum_Click;
            // 
            // btnEditDeviceNum
            // 
            btnEditDeviceNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditDeviceNum.FlatStyle = FlatStyle.Popup;
            btnEditDeviceNum.Image = Properties.Resources.edit;
            btnEditDeviceNum.Location = new Point(339, 124);
            btnEditDeviceNum.Name = "btnEditDeviceNum";
            btnEditDeviceNum.Size = new Size(23, 24);
            btnEditDeviceNum.TabIndex = 11;
            btnEditDeviceNum.UseVisualStyleBackColor = true;
            btnEditDeviceNum.Click += btnEditDeviceNum_Click;
            // 
            // txtDeviceNum
            // 
            txtDeviceNum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDeviceNum.Location = new Point(13, 125);
            txtDeviceNum.Margin = new Padding(3, 3, 3, 10);
            txtDeviceNum.Name = "txtDeviceNum";
            txtDeviceNum.Size = new Size(320, 23);
            txtDeviceNum.TabIndex = 10;
            txtDeviceNum.TextChanged += txtDeviceNum_TextChanged;
            txtDeviceNum.Enter += txtDeviceNum_Enter;
            txtDeviceNum.KeyDown += txtDeviceNum_KeyDown;
            txtDeviceNum.Validating += txtDeviceNum_Validating;
            // 
            // lblDeviceNum
            // 
            lblDeviceNum.AutoSize = true;
            lblDeviceNum.Location = new Point(10, 107);
            lblDeviceNum.Name = "lblDeviceNum";
            lblDeviceNum.Size = new Size(92, 15);
            lblDeviceNum.TabIndex = 9;
            lblDeviceNum.Text = "Device numbers";
            // 
            // btnSelectObjNum
            // 
            btnSelectObjNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectObjNum.FlatStyle = FlatStyle.Popup;
            btnSelectObjNum.Image = Properties.Resources.find;
            btnSelectObjNum.Location = new Point(368, 80);
            btnSelectObjNum.Name = "btnSelectObjNum";
            btnSelectObjNum.Size = new Size(23, 24);
            btnSelectObjNum.TabIndex = 8;
            btnSelectObjNum.UseVisualStyleBackColor = true;
            btnSelectObjNum.Click += btnSelectObjNum_Click;
            // 
            // btnEditObjNum
            // 
            btnEditObjNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditObjNum.FlatStyle = FlatStyle.Popup;
            btnEditObjNum.Image = Properties.Resources.edit;
            btnEditObjNum.Location = new Point(339, 80);
            btnEditObjNum.Name = "btnEditObjNum";
            btnEditObjNum.Size = new Size(23, 24);
            btnEditObjNum.TabIndex = 7;
            btnEditObjNum.UseVisualStyleBackColor = true;
            btnEditObjNum.Click += btnEditObjNum_Click;
            // 
            // txtObjNum
            // 
            txtObjNum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtObjNum.Location = new Point(13, 81);
            txtObjNum.Name = "txtObjNum";
            txtObjNum.Size = new Size(320, 23);
            txtObjNum.TabIndex = 6;
            txtObjNum.TextChanged += txtObjNum_TextChanged;
            txtObjNum.Enter += txtObjNum_Enter;
            txtObjNum.KeyDown += txtObjNum_KeyDown;
            txtObjNum.Validating += txtObjNum_Validating;
            // 
            // lblObjNum
            // 
            lblObjNum.AutoSize = true;
            lblObjNum.Location = new Point(10, 63);
            lblObjNum.Name = "lblObjNum";
            lblObjNum.Size = new Size(92, 15);
            lblObjNum.TabIndex = 5;
            lblObjNum.Text = "Object numbers";
            // 
            // btnSelectCnlNum
            // 
            btnSelectCnlNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectCnlNum.FlatStyle = FlatStyle.Popup;
            btnSelectCnlNum.Image = Properties.Resources.find;
            btnSelectCnlNum.Location = new Point(368, 36);
            btnSelectCnlNum.Name = "btnSelectCnlNum";
            btnSelectCnlNum.Size = new Size(23, 24);
            btnSelectCnlNum.TabIndex = 4;
            btnSelectCnlNum.UseVisualStyleBackColor = true;
            btnSelectCnlNum.Click += btnSelectCnlNum_Click;
            // 
            // btnEditCnlNum
            // 
            btnEditCnlNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditCnlNum.FlatStyle = FlatStyle.Popup;
            btnEditCnlNum.Image = Properties.Resources.edit;
            btnEditCnlNum.Location = new Point(339, 36);
            btnEditCnlNum.Name = "btnEditCnlNum";
            btnEditCnlNum.Size = new Size(23, 24);
            btnEditCnlNum.TabIndex = 3;
            btnEditCnlNum.UseVisualStyleBackColor = true;
            btnEditCnlNum.Click += btnEditCnlNum_Click;
            // 
            // txtCnlNum
            // 
            txtCnlNum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCnlNum.Location = new Point(13, 37);
            txtCnlNum.Name = "txtCnlNum";
            txtCnlNum.Size = new Size(320, 23);
            txtCnlNum.TabIndex = 2;
            txtCnlNum.TextChanged += txtCnlNum_TextChanged;
            txtCnlNum.Enter += txtCnlNum_Enter;
            txtCnlNum.KeyDown += txtCnlNum_KeyDown;
            txtCnlNum.Validating += txtCnlNum_Validating;
            // 
            // lblCnlNum
            // 
            lblCnlNum.AutoSize = true;
            lblCnlNum.Location = new Point(10, 19);
            lblCnlNum.Name = "lblCnlNum";
            lblCnlNum.Size = new Size(101, 15);
            lblCnlNum.TabIndex = 1;
            lblCnlNum.Text = "Channel numbers";
            // 
            // gbQuery
            // 
            gbQuery.Controls.Add(txtSql);
            gbQuery.Controls.Add(btnViewParameters);
            gbQuery.Controls.Add(chkSingleQuery);
            gbQuery.Dock = DockStyle.Fill;
            gbQuery.Location = new Point(0, 259);
            gbQuery.Name = "gbQuery";
            gbQuery.Padding = new Padding(10, 3, 10, 10);
            gbQuery.Size = new Size(404, 203);
            gbQuery.TabIndex = 2;
            gbQuery.TabStop = false;
            gbQuery.Text = "Query";
            // 
            // txtSql
            // 
            txtSql.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSql.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            txtSql.Location = new Point(13, 47);
            txtSql.Multiline = true;
            txtSql.Name = "txtSql";
            txtSql.ScrollBars = ScrollBars.Both;
            txtSql.Size = new Size(378, 143);
            txtSql.TabIndex = 3;
            txtSql.WordWrap = false;
            txtSql.TextChanged += txtSql_TextChanged;
            // 
            // btnViewParameters
            // 
            btnViewParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnViewParameters.FlatStyle = FlatStyle.Popup;
            btnViewParameters.Image = Properties.Resources.parameters;
            btnViewParameters.Location = new Point(368, 17);
            btnViewParameters.Name = "btnViewParameters";
            btnViewParameters.Size = new Size(23, 24);
            btnViewParameters.TabIndex = 2;
            toolTip.SetToolTip(btnViewParameters, "Available parameters");
            btnViewParameters.UseVisualStyleBackColor = true;
            btnViewParameters.Click += btnViewParameters_Click;
            // 
            // chkSingleQuery
            // 
            chkSingleQuery.AutoSize = true;
            chkSingleQuery.Location = new Point(13, 22);
            chkSingleQuery.Name = "chkSingleQuery";
            chkSingleQuery.Size = new Size(227, 19);
            chkSingleQuery.TabIndex = 1;
            chkSingleQuery.Text = "Single query (input numbers required)";
            chkSingleQuery.UseVisualStyleBackColor = true;
            chkSingleQuery.CheckedChanged += chkSingleQuery_CheckedChanged;
            // 
            // CtrlQuery
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbQuery);
            Controls.Add(gbFilter);
            Controls.Add(gbGeneral);
            Margin = new Padding(3, 3, 3, 10);
            Name = "CtrlQuery";
            Size = new Size(404, 462);
            gbGeneral.ResumeLayout(false);
            gbGeneral.PerformLayout();
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            gbQuery.ResumeLayout(false);
            gbQuery.PerformLayout();
            ResumeLayout(false);
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
        private Button btnViewParameters;
        private Button btnSelectDeviceNum;
        private Button btnSelectObjNum;
        private Button btnSelectCnlNum;
        private ToolTip toolTip;
    }
}
