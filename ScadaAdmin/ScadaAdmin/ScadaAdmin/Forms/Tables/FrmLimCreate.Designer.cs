namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmLimCreate
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
            txtName = new System.Windows.Forms.TextBox();
            lblName = new System.Windows.Forms.Label();
            numLimID = new System.Windows.Forms.NumericUpDown();
            lblLimID = new System.Windows.Forms.Label();
            chkIsBoundToCnl = new System.Windows.Forms.CheckBox();
            chkIsShared = new System.Windows.Forms.CheckBox();
            txtHiHi = new System.Windows.Forms.TextBox();
            lblHiHi = new System.Windows.Forms.Label();
            txtHigh = new System.Windows.Forms.TextBox();
            lblHigh = new System.Windows.Forms.Label();
            txtLow = new System.Windows.Forms.TextBox();
            lblLow = new System.Windows.Forms.Label();
            txtLoLo = new System.Windows.Forms.TextBox();
            lblLoLo = new System.Windows.Forms.Label();
            txtDeadband = new System.Windows.Forms.TextBox();
            lblDeadband = new System.Windows.Forms.Label();
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numLimID).BeginInit();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new System.Drawing.Point(121, 27);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(312, 23);
            txtName.TabIndex = 3;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(118, 9);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(39, 15);
            lblName.TabIndex = 2;
            lblName.Text = "Name";
            // 
            // numLimID
            // 
            numLimID.Location = new System.Drawing.Point(15, 27);
            numLimID.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numLimID.Name = "numLimID";
            numLimID.Size = new System.Drawing.Size(100, 23);
            numLimID.TabIndex = 1;
            numLimID.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblLimID
            // 
            lblLimID.AutoSize = true;
            lblLimID.Location = new System.Drawing.Point(12, 9);
            lblLimID.Name = "lblLimID";
            lblLimID.Size = new System.Drawing.Size(18, 15);
            lblLimID.TabIndex = 0;
            lblLimID.Text = "ID";
            // 
            // chkIsBoundToCnl
            // 
            chkIsBoundToCnl.AutoSize = true;
            chkIsBoundToCnl.Location = new System.Drawing.Point(15, 56);
            chkIsBoundToCnl.Name = "chkIsBoundToCnl";
            chkIsBoundToCnl.Size = new System.Drawing.Size(125, 19);
            chkIsBoundToCnl.TabIndex = 4;
            chkIsBoundToCnl.Text = "Bound to channels";
            chkIsBoundToCnl.UseVisualStyleBackColor = true;
            // 
            // chkIsShared
            // 
            chkIsShared.AutoSize = true;
            chkIsShared.Location = new System.Drawing.Point(15, 81);
            chkIsShared.Name = "chkIsShared";
            chkIsShared.Size = new System.Drawing.Size(62, 19);
            chkIsShared.TabIndex = 5;
            chkIsShared.Text = "Shared";
            chkIsShared.UseVisualStyleBackColor = true;
            // 
            // txtHiHi
            // 
            txtHiHi.Location = new System.Drawing.Point(333, 121);
            txtHiHi.Name = "txtHiHi";
            txtHiHi.Size = new System.Drawing.Size(100, 23);
            txtHiHi.TabIndex = 13;
            // 
            // lblHiHi
            // 
            lblHiHi.AutoSize = true;
            lblHiHi.Location = new System.Drawing.Point(330, 103);
            lblHiHi.Name = "lblHiHi";
            lblHiHi.Size = new System.Drawing.Size(60, 15);
            lblHiHi.TabIndex = 12;
            lblHiHi.Text = "High high";
            // 
            // txtHigh
            // 
            txtHigh.Location = new System.Drawing.Point(227, 121);
            txtHigh.Name = "txtHigh";
            txtHigh.Size = new System.Drawing.Size(100, 23);
            txtHigh.TabIndex = 11;
            // 
            // lblHigh
            // 
            lblHigh.AutoSize = true;
            lblHigh.Location = new System.Drawing.Point(224, 103);
            lblHigh.Name = "lblHigh";
            lblHigh.Size = new System.Drawing.Size(33, 15);
            lblHigh.TabIndex = 10;
            lblHigh.Text = "High";
            // 
            // txtLow
            // 
            txtLow.Location = new System.Drawing.Point(121, 121);
            txtLow.Name = "txtLow";
            txtLow.Size = new System.Drawing.Size(100, 23);
            txtLow.TabIndex = 9;
            // 
            // lblLow
            // 
            lblLow.AutoSize = true;
            lblLow.Location = new System.Drawing.Point(118, 103);
            lblLow.Name = "lblLow";
            lblLow.Size = new System.Drawing.Size(29, 15);
            lblLow.TabIndex = 8;
            lblLow.Text = "Low";
            // 
            // txtLoLo
            // 
            txtLoLo.Location = new System.Drawing.Point(15, 121);
            txtLoLo.Name = "txtLoLo";
            txtLoLo.Size = new System.Drawing.Size(100, 23);
            txtLoLo.TabIndex = 7;
            // 
            // lblLoLo
            // 
            lblLoLo.AutoSize = true;
            lblLoLo.Location = new System.Drawing.Point(12, 103);
            lblLoLo.Name = "lblLoLo";
            lblLoLo.Size = new System.Drawing.Size(51, 15);
            lblLoLo.TabIndex = 6;
            lblLoLo.Text = "Low low";
            // 
            // txtDeadband
            // 
            txtDeadband.Location = new System.Drawing.Point(15, 165);
            txtDeadband.Name = "txtDeadband";
            txtDeadband.Size = new System.Drawing.Size(100, 23);
            txtDeadband.TabIndex = 15;
            // 
            // lblDeadband
            // 
            lblDeadband.AutoSize = true;
            lblDeadband.Location = new System.Drawing.Point(12, 147);
            lblDeadband.Name = "lblDeadband";
            lblDeadband.Size = new System.Drawing.Size(61, 15);
            lblDeadband.TabIndex = 14;
            lblDeadband.Text = "Deadband";
            // 
            // btnOK
            // 
            btnOK.Location = new System.Drawing.Point(277, 204);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 22;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(358, 204);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 23;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmLimCreate
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(445, 239);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(txtDeadband);
            Controls.Add(lblDeadband);
            Controls.Add(txtHiHi);
            Controls.Add(lblHiHi);
            Controls.Add(txtHigh);
            Controls.Add(lblHigh);
            Controls.Add(txtLow);
            Controls.Add(lblLow);
            Controls.Add(txtLoLo);
            Controls.Add(lblLoLo);
            Controls.Add(chkIsShared);
            Controls.Add(chkIsBoundToCnl);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Controls.Add(numLimID);
            Controls.Add(lblLimID);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLimCreate";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Create Limit";
            Load += FrmLimCreate_Load;
            ((System.ComponentModel.ISupportInitialize)numLimID).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown numLimID;
        private System.Windows.Forms.Label lblLimID;
        private System.Windows.Forms.CheckBox chkIsBoundToCnl;
        private System.Windows.Forms.CheckBox chkIsShared;
        private System.Windows.Forms.TextBox txtHiHi;
        private System.Windows.Forms.Label lblHiHi;
        private System.Windows.Forms.TextBox txtHigh;
        private System.Windows.Forms.Label lblHigh;
        private System.Windows.Forms.TextBox txtLow;
        private System.Windows.Forms.Label lblLow;
        private System.Windows.Forms.TextBox txtLoLo;
        private System.Windows.Forms.Label lblLoLo;
        private System.Windows.Forms.TextBox txtDeadband;
        private System.Windows.Forms.Label lblDeadband;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}