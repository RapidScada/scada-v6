namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    partial class CtrlGeneral
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
            gbGeneral = new GroupBox();
            numDataLifetime = new NumericUpDown();
            lblDataLifetime = new Label();
            numMaxQueueSize = new NumericUpDown();
            lblMaxQueueSize = new Label();
            btnSelectCnlStatus = new Button();
            numStatusCnlNum = new NumericUpDown();
            lblStatusCnlNum = new Label();
            txtCmdCode = new TextBox();
            lblCmdCode = new Label();
            txtName = new TextBox();
            lblName = new Label();
            txtTargetID = new TextBox();
            lblTargetID = new Label();
            chkActive = new CheckBox();
            gbGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDataLifetime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStatusCnlNum).BeginInit();
            SuspendLayout();
            // 
            // gbGeneral
            // 
            gbGeneral.Controls.Add(numDataLifetime);
            gbGeneral.Controls.Add(lblDataLifetime);
            gbGeneral.Controls.Add(numMaxQueueSize);
            gbGeneral.Controls.Add(lblMaxQueueSize);
            gbGeneral.Controls.Add(btnSelectCnlStatus);
            gbGeneral.Controls.Add(numStatusCnlNum);
            gbGeneral.Controls.Add(lblStatusCnlNum);
            gbGeneral.Controls.Add(txtCmdCode);
            gbGeneral.Controls.Add(lblCmdCode);
            gbGeneral.Controls.Add(txtName);
            gbGeneral.Controls.Add(lblName);
            gbGeneral.Controls.Add(txtTargetID);
            gbGeneral.Controls.Add(lblTargetID);
            gbGeneral.Controls.Add(chkActive);
            gbGeneral.Dock = DockStyle.Fill;
            gbGeneral.Location = new Point(0, 0);
            gbGeneral.Margin = new Padding(3, 3, 3, 10);
            gbGeneral.Name = "gbGeneral";
            gbGeneral.Padding = new Padding(10, 3, 10, 10);
            gbGeneral.Size = new Size(404, 462);
            gbGeneral.TabIndex = 0;
            gbGeneral.TabStop = false;
            gbGeneral.Text = "General Options";
            // 
            // numDataLifetime
            // 
            numDataLifetime.Location = new Point(13, 273);
            numDataLifetime.Margin = new Padding(3, 3, 3, 10);
            numDataLifetime.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numDataLifetime.Name = "numDataLifetime";
            numDataLifetime.Size = new Size(120, 23);
            numDataLifetime.TabIndex = 14;
            numDataLifetime.Value = new decimal(new int[] { 3600, 0, 0, 0 });
            numDataLifetime.ValueChanged += numDataLifetime_ValueChanged;
            // 
            // lblDataLifetime
            // 
            lblDataLifetime.AutoSize = true;
            lblDataLifetime.Location = new Point(10, 255);
            lblDataLifetime.Name = "lblDataLifetime";
            lblDataLifetime.Size = new Size(146, 15);
            lblDataLifetime.TabIndex = 13;
            lblDataLifetime.Text = "Data lifetime in queue, sec";
            // 
            // numMaxQueueSize
            // 
            numMaxQueueSize.Location = new Point(13, 222);
            numMaxQueueSize.Margin = new Padding(3, 3, 3, 10);
            numMaxQueueSize.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numMaxQueueSize.Name = "numMaxQueueSize";
            numMaxQueueSize.Size = new Size(120, 23);
            numMaxQueueSize.TabIndex = 12;
            numMaxQueueSize.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            numMaxQueueSize.ValueChanged += numMaxQueueSize_ValueChanged;
            // 
            // lblMaxQueueSize
            // 
            lblMaxQueueSize.AutoSize = true;
            lblMaxQueueSize.Location = new Point(10, 204);
            lblMaxQueueSize.Name = "lblMaxQueueSize";
            lblMaxQueueSize.Size = new Size(120, 15);
            lblMaxQueueSize.TabIndex = 11;
            lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // btnSelectCnlStatus
            // 
            btnSelectCnlStatus.FlatStyle = FlatStyle.Popup;
            btnSelectCnlStatus.Image = Properties.Resources.find;
            btnSelectCnlStatus.Location = new Point(139, 169);
            btnSelectCnlStatus.Name = "btnSelectCnlStatus";
            btnSelectCnlStatus.Size = new Size(23, 24);
            btnSelectCnlStatus.TabIndex = 10;
            btnSelectCnlStatus.UseVisualStyleBackColor = true;
            btnSelectCnlStatus.Click += btnSelectCnlStatus_Click;
            // 
            // numStatusCnlNum
            // 
            numStatusCnlNum.Location = new Point(13, 170);
            numStatusCnlNum.Margin = new Padding(3, 3, 3, 10);
            numStatusCnlNum.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numStatusCnlNum.Name = "numStatusCnlNum";
            numStatusCnlNum.Size = new Size(120, 23);
            numStatusCnlNum.TabIndex = 9;
            numStatusCnlNum.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numStatusCnlNum.ValueChanged += numStatusCnlNum_ValueChanged;
            // 
            // lblStatusCnlNum
            // 
            lblStatusCnlNum.AutoSize = true;
            lblStatusCnlNum.Location = new Point(10, 153);
            lblStatusCnlNum.Name = "lblStatusCnlNum";
            lblStatusCnlNum.Size = new Size(129, 15);
            lblStatusCnlNum.TabIndex = 8;
            lblStatusCnlNum.Text = "Status channel number";
            // 
            // txtCmdCode
            // 
            txtCmdCode.Location = new Point(13, 120);
            txtCmdCode.Margin = new Padding(3, 3, 3, 10);
            txtCmdCode.Name = "txtCmdCode";
            txtCmdCode.Size = new Size(120, 23);
            txtCmdCode.TabIndex = 7;
            txtCmdCode.TextChanged += txtCmdCode_TextChanged;
            // 
            // lblCmdCode
            // 
            lblCmdCode.AutoSize = true;
            lblCmdCode.Location = new Point(10, 102);
            lblCmdCode.Name = "lblCmdCode";
            lblCmdCode.Size = new Size(93, 15);
            lblCmdCode.TabIndex = 6;
            lblCmdCode.Text = "Command code";
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(79, 69);
            txtName.Margin = new Padding(3, 3, 3, 10);
            txtName.Name = "txtName";
            txtName.Size = new Size(312, 23);
            txtName.TabIndex = 5;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(76, 51);
            lblName.Name = "lblName";
            lblName.Size = new Size(39, 15);
            lblName.TabIndex = 3;
            lblName.Text = "Name";
            // 
            // txtTargetID
            // 
            txtTargetID.Location = new Point(13, 69);
            txtTargetID.Margin = new Padding(3, 3, 3, 10);
            txtTargetID.Name = "txtTargetID";
            txtTargetID.ReadOnly = true;
            txtTargetID.Size = new Size(60, 23);
            txtTargetID.TabIndex = 4;
            // 
            // lblTargetID
            // 
            lblTargetID.AutoSize = true;
            lblTargetID.Location = new Point(10, 51);
            lblTargetID.Name = "lblTargetID";
            lblTargetID.Size = new Size(53, 15);
            lblTargetID.TabIndex = 2;
            lblTargetID.Text = "Target ID";
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Location = new Point(13, 22);
            chkActive.Margin = new Padding(3, 3, 3, 10);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(59, 19);
            chkActive.TabIndex = 1;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            chkActive.CheckedChanged += chkActive_CheckedChanged;
            // 
            // CtrlGeneral
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbGeneral);
            Margin = new Padding(3, 3, 3, 10);
            Name = "CtrlGeneral";
            Size = new Size(404, 462);
            gbGeneral.ResumeLayout(false);
            gbGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDataLifetime).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStatusCnlNum).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbGeneral;
        private CheckBox chkActive;
        private TextBox txtName;
        private Label lblName;
        private TextBox txtTargetID;
        private Label lblTargetID;
        private TextBox txtCmdCode;
        private Label lblCmdCode;
        private NumericUpDown numDataLifetime;
        private Label lblDataLifetime;
        private NumericUpDown numMaxQueueSize;
        private Label lblMaxQueueSize;
        private Label lblStatusCnlNum;
        private Button btnSelectCnlStatus;
        private NumericUpDown numStatusCnlNum;
    }
}
