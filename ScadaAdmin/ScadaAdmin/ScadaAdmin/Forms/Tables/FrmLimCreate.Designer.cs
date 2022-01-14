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
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.numLimID = new System.Windows.Forms.NumericUpDown();
            this.lblLimID = new System.Windows.Forms.Label();
            this.chkIsBoundToCnl = new System.Windows.Forms.CheckBox();
            this.chkIsShared = new System.Windows.Forms.CheckBox();
            this.txtHiHi = new System.Windows.Forms.TextBox();
            this.lblHiHi = new System.Windows.Forms.Label();
            this.txtHigh = new System.Windows.Forms.TextBox();
            this.lblHigh = new System.Windows.Forms.Label();
            this.txtLow = new System.Windows.Forms.TextBox();
            this.lblLow = new System.Windows.Forms.Label();
            this.txtLoLo = new System.Windows.Forms.TextBox();
            this.lblLoLo = new System.Windows.Forms.Label();
            this.txtDeadband = new System.Windows.Forms.TextBox();
            this.lblDeadband = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numLimID)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(121, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(312, 23);
            this.txtName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(118, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // numLimID
            // 
            this.numLimID.Location = new System.Drawing.Point(15, 27);
            this.numLimID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLimID.Name = "numLimID";
            this.numLimID.Size = new System.Drawing.Size(100, 23);
            this.numLimID.TabIndex = 1;
            this.numLimID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblLimID
            // 
            this.lblLimID.AutoSize = true;
            this.lblLimID.Location = new System.Drawing.Point(12, 9);
            this.lblLimID.Name = "lblLimID";
            this.lblLimID.Size = new System.Drawing.Size(18, 15);
            this.lblLimID.TabIndex = 0;
            this.lblLimID.Text = "ID";
            // 
            // chkIsBoundToCnl
            // 
            this.chkIsBoundToCnl.AutoSize = true;
            this.chkIsBoundToCnl.Location = new System.Drawing.Point(15, 56);
            this.chkIsBoundToCnl.Name = "chkIsBoundToCnl";
            this.chkIsBoundToCnl.Size = new System.Drawing.Size(125, 19);
            this.chkIsBoundToCnl.TabIndex = 4;
            this.chkIsBoundToCnl.Text = "Bound to channels";
            this.chkIsBoundToCnl.UseVisualStyleBackColor = true;
            // 
            // chkIsShared
            // 
            this.chkIsShared.AutoSize = true;
            this.chkIsShared.Location = new System.Drawing.Point(15, 81);
            this.chkIsShared.Name = "chkIsShared";
            this.chkIsShared.Size = new System.Drawing.Size(62, 19);
            this.chkIsShared.TabIndex = 5;
            this.chkIsShared.Text = "Shared";
            this.chkIsShared.UseVisualStyleBackColor = true;
            // 
            // txtHiHi
            // 
            this.txtHiHi.Location = new System.Drawing.Point(333, 121);
            this.txtHiHi.Name = "txtHiHi";
            this.txtHiHi.Size = new System.Drawing.Size(100, 23);
            this.txtHiHi.TabIndex = 13;
            // 
            // lblHiHi
            // 
            this.lblHiHi.AutoSize = true;
            this.lblHiHi.Location = new System.Drawing.Point(330, 103);
            this.lblHiHi.Name = "lblHiHi";
            this.lblHiHi.Size = new System.Drawing.Size(86, 15);
            this.lblHiHi.TabIndex = 12;
            this.lblHiHi.Text = "Extremely high";
            // 
            // txtHigh
            // 
            this.txtHigh.Location = new System.Drawing.Point(227, 121);
            this.txtHigh.Name = "txtHigh";
            this.txtHigh.Size = new System.Drawing.Size(100, 23);
            this.txtHigh.TabIndex = 11;
            // 
            // lblHigh
            // 
            this.lblHigh.AutoSize = true;
            this.lblHigh.Location = new System.Drawing.Point(224, 103);
            this.lblHigh.Name = "lblHigh";
            this.lblHigh.Size = new System.Drawing.Size(33, 15);
            this.lblHigh.TabIndex = 10;
            this.lblHigh.Text = "High";
            // 
            // txtLow
            // 
            this.txtLow.Location = new System.Drawing.Point(121, 121);
            this.txtLow.Name = "txtLow";
            this.txtLow.Size = new System.Drawing.Size(100, 23);
            this.txtLow.TabIndex = 9;
            // 
            // lblLow
            // 
            this.lblLow.AutoSize = true;
            this.lblLow.Location = new System.Drawing.Point(118, 103);
            this.lblLow.Name = "lblLow";
            this.lblLow.Size = new System.Drawing.Size(29, 15);
            this.lblLow.TabIndex = 8;
            this.lblLow.Text = "Low";
            // 
            // txtLoLo
            // 
            this.txtLoLo.Location = new System.Drawing.Point(15, 121);
            this.txtLoLo.Name = "txtLoLo";
            this.txtLoLo.Size = new System.Drawing.Size(100, 23);
            this.txtLoLo.TabIndex = 7;
            // 
            // lblLoLo
            // 
            this.lblLoLo.AutoSize = true;
            this.lblLoLo.Location = new System.Drawing.Point(12, 103);
            this.lblLoLo.Name = "lblLoLo";
            this.lblLoLo.Size = new System.Drawing.Size(81, 15);
            this.lblLoLo.TabIndex = 6;
            this.lblLoLo.Text = "Extremely low";
            // 
            // txtDeadband
            // 
            this.txtDeadband.Location = new System.Drawing.Point(15, 165);
            this.txtDeadband.Name = "txtDeadband";
            this.txtDeadband.Size = new System.Drawing.Size(100, 23);
            this.txtDeadband.TabIndex = 15;
            // 
            // lblDeadband
            // 
            this.lblDeadband.AutoSize = true;
            this.lblDeadband.Location = new System.Drawing.Point(12, 147);
            this.lblDeadband.Name = "lblDeadband";
            this.lblDeadband.Size = new System.Drawing.Size(61, 15);
            this.lblDeadband.TabIndex = 14;
            this.lblDeadband.Text = "Deadband";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(277, 204);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 22;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(358, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmLimCreate
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(445, 239);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtDeadband);
            this.Controls.Add(this.lblDeadband);
            this.Controls.Add(this.txtHiHi);
            this.Controls.Add(this.lblHiHi);
            this.Controls.Add(this.txtHigh);
            this.Controls.Add(this.lblHigh);
            this.Controls.Add(this.txtLow);
            this.Controls.Add(this.lblLow);
            this.Controls.Add(this.txtLoLo);
            this.Controls.Add(this.lblLoLo);
            this.Controls.Add(this.chkIsShared);
            this.Controls.Add(this.chkIsBoundToCnl);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.numLimID);
            this.Controls.Add(this.lblLimID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLimCreate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Limit";
            this.Load += new System.EventHandler(this.FrmLimCreate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numLimID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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