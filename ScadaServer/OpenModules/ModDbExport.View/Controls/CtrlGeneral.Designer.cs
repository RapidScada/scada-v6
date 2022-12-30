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
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.numDataLifetime = new System.Windows.Forms.NumericUpDown();
            this.lblDataLifetime = new System.Windows.Forms.Label();
            this.numMaxQueueSize = new System.Windows.Forms.NumericUpDown();
            this.lblMaxQueueSize = new System.Windows.Forms.Label();
            this.btnSelectCnlStat = new System.Windows.Forms.Button();
            this.numStatusCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblStatusCnlNum = new System.Windows.Forms.Label();
            this.txtCmdCode = new System.Windows.Forms.TextBox();
            this.lblCmdCode = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtTargetID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.gbGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLifetime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStatusCnlNum)).BeginInit();
            this.SuspendLayout();
            // 
            // gbGeneral
            // 
            this.gbGeneral.Controls.Add(this.numDataLifetime);
            this.gbGeneral.Controls.Add(this.lblDataLifetime);
            this.gbGeneral.Controls.Add(this.numMaxQueueSize);
            this.gbGeneral.Controls.Add(this.lblMaxQueueSize);
            this.gbGeneral.Controls.Add(this.btnSelectCnlStat);
            this.gbGeneral.Controls.Add(this.numStatusCnlNum);
            this.gbGeneral.Controls.Add(this.lblStatusCnlNum);
            this.gbGeneral.Controls.Add(this.txtCmdCode);
            this.gbGeneral.Controls.Add(this.lblCmdCode);
            this.gbGeneral.Controls.Add(this.txtName);
            this.gbGeneral.Controls.Add(this.lblName);
            this.gbGeneral.Controls.Add(this.txtTargetID);
            this.gbGeneral.Controls.Add(this.lblID);
            this.gbGeneral.Controls.Add(this.chkActive);
            this.gbGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGeneral.Location = new System.Drawing.Point(0, 0);
            this.gbGeneral.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneral.Size = new System.Drawing.Size(404, 462);
            this.gbGeneral.TabIndex = 0;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "General Options";
            // 
            // numDataLifetime
            // 
            this.numDataLifetime.Location = new System.Drawing.Point(13, 273);
            this.numDataLifetime.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numDataLifetime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numDataLifetime.Name = "numDataLifetime";
            this.numDataLifetime.Size = new System.Drawing.Size(120, 23);
            this.numDataLifetime.TabIndex = 14;
            this.numDataLifetime.Value = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numDataLifetime.ValueChanged += new System.EventHandler(this.numDataLifetime_ValueChanged);
            // 
            // lblDataLifetime
            // 
            this.lblDataLifetime.AutoSize = true;
            this.lblDataLifetime.Location = new System.Drawing.Point(10, 255);
            this.lblDataLifetime.Name = "lblDataLifetime";
            this.lblDataLifetime.Size = new System.Drawing.Size(146, 15);
            this.lblDataLifetime.TabIndex = 13;
            this.lblDataLifetime.Text = "Data lifetime in queue, sec";
            // 
            // numMaxQueueSize
            // 
            this.numMaxQueueSize.Location = new System.Drawing.Point(13, 222);
            this.numMaxQueueSize.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numMaxQueueSize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numMaxQueueSize.Name = "numMaxQueueSize";
            this.numMaxQueueSize.Size = new System.Drawing.Size(120, 23);
            this.numMaxQueueSize.TabIndex = 12;
            this.numMaxQueueSize.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMaxQueueSize.ValueChanged += new System.EventHandler(this.numMaxQueueSize_ValueChanged);
            // 
            // lblMaxQueueSize
            // 
            this.lblMaxQueueSize.AutoSize = true;
            this.lblMaxQueueSize.Location = new System.Drawing.Point(10, 204);
            this.lblMaxQueueSize.Name = "lblMaxQueueSize";
            this.lblMaxQueueSize.Size = new System.Drawing.Size(120, 15);
            this.lblMaxQueueSize.TabIndex = 11;
            this.lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // btnSelectCnlStat
            // 
            this.btnSelectCnlStat.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectCnlStat.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectCnlStat.Location = new System.Drawing.Point(139, 170);
            this.btnSelectCnlStat.Name = "btnSelectCnlStat";
            this.btnSelectCnlStat.Size = new System.Drawing.Size(23, 24);
            this.btnSelectCnlStat.TabIndex = 10;
            this.btnSelectCnlStat.UseVisualStyleBackColor = true;
            this.btnSelectCnlStat.Click += new System.EventHandler(this.btnSelectCnlStat_Click);
            // 
            // numStatusCnlNum
            // 
            this.numStatusCnlNum.Location = new System.Drawing.Point(13, 170);
            this.numStatusCnlNum.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numStatusCnlNum.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numStatusCnlNum.Name = "numStatusCnlNum";
            this.numStatusCnlNum.Size = new System.Drawing.Size(120, 23);
            this.numStatusCnlNum.TabIndex = 9;
            this.numStatusCnlNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStatusCnlNum.ValueChanged += new System.EventHandler(this.numStatusCnlNum_ValueChanged);
            // 
            // lblStatusCnlNum
            // 
            this.lblStatusCnlNum.AutoSize = true;
            this.lblStatusCnlNum.Location = new System.Drawing.Point(10, 153);
            this.lblStatusCnlNum.Name = "lblStatusCnlNum";
            this.lblStatusCnlNum.Size = new System.Drawing.Size(129, 15);
            this.lblStatusCnlNum.TabIndex = 8;
            this.lblStatusCnlNum.Text = "Status channel number";
            // 
            // txtCmdCode
            // 
            this.txtCmdCode.Location = new System.Drawing.Point(13, 120);
            this.txtCmdCode.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtCmdCode.Name = "txtCmdCode";
            this.txtCmdCode.Size = new System.Drawing.Size(120, 23);
            this.txtCmdCode.TabIndex = 7;
            this.txtCmdCode.TextChanged += new System.EventHandler(this.txtCmdCode_TextChanged);
            // 
            // lblCmdCode
            // 
            this.lblCmdCode.AutoSize = true;
            this.lblCmdCode.Location = new System.Drawing.Point(10, 102);
            this.lblCmdCode.Name = "lblCmdCode";
            this.lblCmdCode.Size = new System.Drawing.Size(93, 15);
            this.lblCmdCode.TabIndex = 6;
            this.lblCmdCode.Text = "Command code";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(77, 69);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(314, 23);
            this.txtName.TabIndex = 5;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(74, 51);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name";
            // 
            // txtTargetID
            // 
            this.txtTargetID.Location = new System.Drawing.Point(13, 69);
            this.txtTargetID.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtTargetID.Name = "txtTargetID";
            this.txtTargetID.ReadOnly = true;
            this.txtTargetID.Size = new System.Drawing.Size(58, 23);
            this.txtTargetID.TabIndex = 4;
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(10, 51);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(53, 15);
            this.lblID.TabIndex = 2;
            this.lblID.Text = "Target ID";
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
            // CtrlGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbGeneral);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.Name = "CtrlGeneral";
            this.Size = new System.Drawing.Size(404, 462);
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLifetime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStatusCnlNum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbGeneral;
        private CheckBox chkActive;
        private TextBox txtName;
        private Label lblName;
        private TextBox txtTargetID;
        private Label lblID;
        private TextBox txtCmdCode;
        private Label lblCmdCode;
        private NumericUpDown numDataLifetime;
        private Label lblDataLifetime;
        private NumericUpDown numMaxQueueSize;
        private Label lblMaxQueueSize;
        private Label lblStatusCnlNum;
        private Button btnSelectCnlStat;
        private NumericUpDown numStatusCnlNum;
    }
}
