namespace Scada.Server.Modules.ModDeviceAlarm.View.Controls
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
            numDataUnchangedNumber = new NumericUpDown();
            lblDataUnchangedNumber = new Label();
            numDataPeriod = new NumericUpDown();
            lblDataUnchangePeriod = new Label();
            btnSelectCnlStatus = new Button();
            numStatusCnlNum = new NumericUpDown();
            lblStatusCnlNum = new Label();
            cbTriggerKind = new ComboBox();
            lblTriggerKind = new Label();
            txtName = new TextBox();
            lblName = new Label();
            chkActive = new CheckBox();
            gbFilter = new GroupBox();
            btnSelectDeviceNum = new Button();
            btnEditDeviceNum = new Button();
            txtDeviceNum = new TextBox();
            lblDeviceNum = new Label();
            btnSelectCnlNum = new Button();
            btnEditCnlNum = new Button();
            txtCnlNum = new TextBox();
            lblCnlNum = new Label();
            toolTip = new ToolTip(components);
            numStatusPeriod = new NumericUpDown();
            lblStatusPeriod = new Label();
            gbGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDataUnchangedNumber).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDataPeriod).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStatusCnlNum).BeginInit();
            gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numStatusPeriod).BeginInit();
            SuspendLayout();
            // 
            // gbGeneral
            // 
            gbGeneral.Controls.Add(numStatusPeriod);
            gbGeneral.Controls.Add(lblStatusPeriod);
            gbGeneral.Controls.Add(numDataUnchangedNumber);
            gbGeneral.Controls.Add(lblDataUnchangedNumber);
            gbGeneral.Controls.Add(numDataPeriod);
            gbGeneral.Controls.Add(lblDataUnchangePeriod);
            gbGeneral.Controls.Add(btnSelectCnlStatus);
            gbGeneral.Controls.Add(numStatusCnlNum);
            gbGeneral.Controls.Add(lblStatusCnlNum);
            gbGeneral.Controls.Add(cbTriggerKind);
            gbGeneral.Controls.Add(lblTriggerKind);
            gbGeneral.Controls.Add(txtName);
            gbGeneral.Controls.Add(lblName);
            gbGeneral.Controls.Add(chkActive);
            gbGeneral.Dock = DockStyle.Top;
            gbGeneral.Location = new Point(0, 0);
            gbGeneral.Margin = new Padding(3, 3, 3, 11);
            gbGeneral.Name = "gbGeneral";
            gbGeneral.Padding = new Padding(10, 3, 10, 11);
            gbGeneral.Size = new Size(404, 229);
            gbGeneral.TabIndex = 0;
            gbGeneral.TabStop = false;
            gbGeneral.Text = "General";
            // 
            // numDataUnchangedNumber
            // 
            numDataUnchangedNumber.Location = new Point(203, 192);
            numDataUnchangedNumber.Margin = new Padding(3, 3, 3, 11);
            numDataUnchangedNumber.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numDataUnchangedNumber.Name = "numDataUnchangedNumber";
            numDataUnchangedNumber.Size = new Size(120, 23);
            numDataUnchangedNumber.TabIndex = 18;
            numDataUnchangedNumber.Value = new decimal(new int[] { 5, 0, 0, 0 });
            numDataUnchangedNumber.ValueChanged += numDataUnchangedNumber_ValueChanged;
            // 
            // lblDataUnchangedNumber
            // 
            lblDataUnchangedNumber.AutoSize = true;
            lblDataUnchangedNumber.Location = new Point(200, 171);
            lblDataUnchangedNumber.Name = "lblDataUnchangedNumber";
            lblDataUnchangedNumber.Size = new Size(170, 17);
            lblDataUnchangedNumber.TabIndex = 17;
            lblDataUnchangedNumber.Text = "Number of data unchanged";
            // 
            // numDataPeriod
            // 
            numDataPeriod.Location = new Point(13, 192);
            numDataPeriod.Margin = new Padding(3, 3, 3, 11);
            numDataPeriod.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numDataPeriod.Name = "numDataPeriod";
            numDataPeriod.Size = new Size(120, 23);
            numDataPeriod.TabIndex = 18;
            numDataPeriod.Value = new decimal(new int[] { 60, 0, 0, 0 });
            numDataPeriod.ValueChanged += numDataPeriod_ValueChanged;
            // 
            // lblDataUnchangePeriod
            // 
            lblDataUnchangePeriod.AutoSize = true;
            lblDataUnchangePeriod.Location = new Point(10, 171);
            lblDataUnchangePeriod.Name = "lblDataUnchangePeriod";
            lblDataUnchangePeriod.Size = new Size(186, 17);
            lblDataUnchangePeriod.TabIndex = 17;
            lblDataUnchangePeriod.Text = "Period of data unchanged, sec";
            // 
            // btnSelectCnlStatus
            // 
            btnSelectCnlStatus.FlatStyle = FlatStyle.Popup;
            btnSelectCnlStatus.Image = Properties.Resources.find;
            btnSelectCnlStatus.Location = new Point(139, 128);
            btnSelectCnlStatus.Name = "btnSelectCnlStatus";
            btnSelectCnlStatus.Size = new Size(23, 27);
            btnSelectCnlStatus.TabIndex = 13;
            btnSelectCnlStatus.UseVisualStyleBackColor = true;
            btnSelectCnlStatus.Click += btnSelectCnlStatus_Click;
            // 
            // numStatusCnlNum
            // 
            numStatusCnlNum.Location = new Point(13, 129);
            numStatusCnlNum.Margin = new Padding(3, 3, 3, 11);
            numStatusCnlNum.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numStatusCnlNum.Name = "numStatusCnlNum";
            numStatusCnlNum.Size = new Size(120, 23);
            numStatusCnlNum.TabIndex = 12;
            numStatusCnlNum.Click += numStatusCnlNum_ValueChanged;
            // 
            // lblStatusCnlNum
            // 
            lblStatusCnlNum.AutoSize = true;
            lblStatusCnlNum.Location = new Point(10, 109);
            lblStatusCnlNum.Name = "lblStatusCnlNum";
            lblStatusCnlNum.Size = new Size(140, 17);
            lblStatusCnlNum.TabIndex = 11;
            lblStatusCnlNum.Text = "Status channel number";
            // 
            // cbTriggerKind
            // 
            cbTriggerKind.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbTriggerKind.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTriggerKind.FormattingEnabled = true;
            cbTriggerKind.Items.AddRange(new object[] { "Status", "Value unchange" });
            cbTriggerKind.Location = new Point(229, 70);
            cbTriggerKind.Margin = new Padding(3, 3, 3, 11);
            cbTriggerKind.Name = "cbTriggerKind";
            cbTriggerKind.Size = new Size(162, 25);
            cbTriggerKind.TabIndex = 5;
            cbTriggerKind.SelectedIndexChanged += cbDataKind_SelectedIndexChanged;
            // 
            // lblTriggerKind
            // 
            lblTriggerKind.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTriggerKind.AutoSize = true;
            lblTriggerKind.Location = new Point(226, 50);
            lblTriggerKind.Name = "lblTriggerKind";
            lblTriggerKind.Size = new Size(80, 17);
            lblTriggerKind.TabIndex = 4;
            lblTriggerKind.Text = "Trigger kind";
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(13, 70);
            txtName.Margin = new Padding(3, 3, 3, 11);
            txtName.Name = "txtName";
            txtName.Size = new Size(210, 23);
            txtName.TabIndex = 3;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(10, 50);
            lblName.Name = "lblName";
            lblName.Size = new Size(43, 17);
            lblName.TabIndex = 2;
            lblName.Text = "Name";
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Location = new Point(13, 25);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(61, 21);
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
            gbFilter.Controls.Add(btnSelectCnlNum);
            gbFilter.Controls.Add(btnEditCnlNum);
            gbFilter.Controls.Add(txtCnlNum);
            gbFilter.Controls.Add(lblCnlNum);
            gbFilter.Dock = DockStyle.Top;
            gbFilter.Location = new Point(0, 229);
            gbFilter.Margin = new Padding(3, 3, 3, 11);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(10, 3, 10, 11);
            gbFilter.Size = new Size(404, 132);
            gbFilter.TabIndex = 1;
            gbFilter.TabStop = false;
            gbFilter.Text = "Filter";
            // 
            // btnSelectDeviceNum
            // 
            btnSelectDeviceNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectDeviceNum.FlatStyle = FlatStyle.Popup;
            btnSelectDeviceNum.Image = Properties.Resources.find;
            btnSelectDeviceNum.Location = new Point(368, 39);
            btnSelectDeviceNum.Name = "btnSelectDeviceNum";
            btnSelectDeviceNum.Size = new Size(23, 27);
            btnSelectDeviceNum.TabIndex = 12;
            btnSelectDeviceNum.UseVisualStyleBackColor = true;
            btnSelectDeviceNum.Click += btnSelectDeviceNum_Click;
            // 
            // btnEditDeviceNum
            // 
            btnEditDeviceNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditDeviceNum.FlatStyle = FlatStyle.Popup;
            btnEditDeviceNum.Image = Properties.Resources.edit;
            btnEditDeviceNum.Location = new Point(339, 39);
            btnEditDeviceNum.Name = "btnEditDeviceNum";
            btnEditDeviceNum.Size = new Size(23, 27);
            btnEditDeviceNum.TabIndex = 11;
            btnEditDeviceNum.UseVisualStyleBackColor = true;
            btnEditDeviceNum.Click += btnEditDeviceNum_Click;
            // 
            // txtDeviceNum
            // 
            txtDeviceNum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDeviceNum.Location = new Point(13, 40);
            txtDeviceNum.Margin = new Padding(3, 3, 3, 11);
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
            lblDeviceNum.Location = new Point(10, 19);
            lblDeviceNum.Name = "lblDeviceNum";
            lblDeviceNum.Size = new Size(101, 17);
            lblDeviceNum.TabIndex = 9;
            lblDeviceNum.Text = "Device numbers";
            // 
            // btnSelectCnlNum
            // 
            btnSelectCnlNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectCnlNum.FlatStyle = FlatStyle.Popup;
            btnSelectCnlNum.Image = Properties.Resources.find;
            btnSelectCnlNum.Location = new Point(368, 85);
            btnSelectCnlNum.Name = "btnSelectCnlNum";
            btnSelectCnlNum.Size = new Size(23, 27);
            btnSelectCnlNum.TabIndex = 4;
            btnSelectCnlNum.UseVisualStyleBackColor = true;
            btnSelectCnlNum.Click += btnSelectCnlNum_Click;
            // 
            // btnEditCnlNum
            // 
            btnEditCnlNum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditCnlNum.FlatStyle = FlatStyle.Popup;
            btnEditCnlNum.Image = Properties.Resources.edit;
            btnEditCnlNum.Location = new Point(339, 85);
            btnEditCnlNum.Name = "btnEditCnlNum";
            btnEditCnlNum.Size = new Size(23, 27);
            btnEditCnlNum.TabIndex = 3;
            btnEditCnlNum.UseVisualStyleBackColor = true;
            btnEditCnlNum.Click += btnEditCnlNum_Click;
            // 
            // txtCnlNum
            // 
            txtCnlNum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCnlNum.Location = new Point(13, 86);
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
            lblCnlNum.Location = new Point(10, 66);
            lblCnlNum.Name = "lblCnlNum";
            lblCnlNum.Size = new Size(109, 17);
            lblCnlNum.TabIndex = 1;
            lblCnlNum.Text = "Channel numbers";
            // 
            // numStatusPeriod
            // 
            numStatusPeriod.Location = new Point(203, 129);
            numStatusPeriod.Margin = new Padding(3, 3, 3, 11);
            numStatusPeriod.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numStatusPeriod.Name = "numStatusPeriod";
            numStatusPeriod.Size = new Size(120, 23);
            numStatusPeriod.TabIndex = 20;
            numStatusPeriod.Value = new decimal(new int[] { 60, 0, 0, 0 });
            numStatusPeriod.ValueChanged += numStatusPeriod_ValueChanged;
            // 
            // lblStatusPeriod
            // 
            lblStatusPeriod.AutoSize = true;
            lblStatusPeriod.Location = new Point(200, 108);
            lblStatusPeriod.Name = "lblStatusPeriod";
            lblStatusPeriod.Size = new Size(126, 17);
            lblStatusPeriod.TabIndex = 19;
            lblStatusPeriod.Text = "Period of status, sec";
            // 
            // CtrlQuery
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbFilter);
            Controls.Add(gbGeneral);
            Margin = new Padding(3, 3, 3, 11);
            Name = "CtrlQuery";
            Size = new Size(404, 524);
            gbGeneral.ResumeLayout(false);
            gbGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDataUnchangedNumber).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDataPeriod).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStatusCnlNum).EndInit();
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numStatusPeriod).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbGeneral;
        private CheckBox chkActive;
        private TextBox txtName;
        private Label lblName;
        private ComboBox cbTriggerKind;
        private Label lblTriggerKind;
        private GroupBox gbFilter;
        private TextBox txtCnlNum;
        private Label lblCnlNum;
        private Button btnEditCnlNum;
        private Button btnEditDeviceNum;
        private TextBox txtDeviceNum;
        private Label lblDeviceNum;
        private Button btnSelectDeviceNum;
        private Button btnSelectCnlNum;
        private ToolTip toolTip;
        private Button btnSelectCnlStatus;
        private NumericUpDown numStatusCnlNum;
        private Label lblStatusCnlNum;
        private NumericUpDown numDataPeriod;
        private Label lblDataUnchangePeriod;
        private NumericUpDown numDataUnchangedNumber;
        private Label lblDataUnchangedNumber;
        private NumericUpDown numStatusPeriod;
        private Label lblStatusPeriod;
    }
}
