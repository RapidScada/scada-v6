namespace Scada.Comm.Drivers.DrvDbImport.View.Controls
{
    partial class CtrlCommand
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
            this.gbCmd = new System.Windows.Forms.GroupBox();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.lblSql = new System.Windows.Forms.Label();
            this.txtCmdCode = new System.Windows.Forms.TextBox();
            this.lblCmdCode = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbCmd.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCmd
            // 
            this.gbCmd.Controls.Add(this.pnlInfo);
            this.gbCmd.Controls.Add(this.txtSql);
            this.gbCmd.Controls.Add(this.lblSql);
            this.gbCmd.Controls.Add(this.txtCmdCode);
            this.gbCmd.Controls.Add(this.lblCmdCode);
            this.gbCmd.Controls.Add(this.txtName);
            this.gbCmd.Controls.Add(this.lblName);
            this.gbCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCmd.Location = new System.Drawing.Point(0, 0);
            this.gbCmd.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbCmd.Name = "gbCmd";
            this.gbCmd.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCmd.Size = new System.Drawing.Size(404, 462);
            this.gbCmd.TabIndex = 0;
            this.gbCmd.TabStop = false;
            this.gbCmd.Text = "Command Parameters";
            // 
            // pnlInfo
            // 
            this.pnlInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInfo.Controls.Add(this.lblInfo);
            this.pnlInfo.Controls.Add(this.pbInfo);
            this.pnlInfo.Location = new System.Drawing.Point(13, 410);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(378, 39);
            this.pnlInfo.TabIndex = 7;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblInfo.Location = new System.Drawing.Point(25, 3);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(238, 15);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Avaliable parameters: @cmdVal, @cmdData\r\n";
            // 
            // pbInfo
            // 
            this.pbInfo.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.info;
            this.pbInfo.Location = new System.Drawing.Point(3, 3);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(16, 16);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbInfo.TabIndex = 12;
            this.pbInfo.TabStop = false;
            // 
            // txtSql
            // 
            this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSql.Location = new System.Drawing.Point(13, 125);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.Size = new System.Drawing.Size(378, 279);
            this.txtSql.TabIndex = 6;
            this.txtSql.WordWrap = false;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // lblSql
            // 
            this.lblSql.AutoSize = true;
            this.lblSql.Location = new System.Drawing.Point(10, 107);
            this.lblSql.Name = "lblSql";
            this.lblSql.Size = new System.Drawing.Size(28, 15);
            this.lblSql.TabIndex = 5;
            this.lblSql.Text = "SQL";
            // 
            // txtCmdCode
            // 
            this.txtCmdCode.Location = new System.Drawing.Point(13, 81);
            this.txtCmdCode.Name = "txtCmdCode";
            this.txtCmdCode.Size = new System.Drawing.Size(134, 23);
            this.txtCmdCode.TabIndex = 4;
            this.txtCmdCode.TextChanged += new System.EventHandler(this.txtCmdCode_TextChanged);
            // 
            // lblCmdCode
            // 
            this.lblCmdCode.AutoSize = true;
            this.lblCmdCode.Location = new System.Drawing.Point(10, 63);
            this.lblCmdCode.Name = "lblCmdCode";
            this.lblCmdCode.Size = new System.Drawing.Size(93, 15);
            this.lblCmdCode.TabIndex = 3;
            this.lblCmdCode.Text = "Command code";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(13, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(378, 23);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 19);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // CtrlCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCmd);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.Name = "CtrlCommand";
            this.Size = new System.Drawing.Size(404, 462);
            this.gbCmd.ResumeLayout(false);
            this.gbCmd.PerformLayout();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbCmd;
        private TextBox txtName;
        private Label lblName;
        private TextBox txtCmdCode;
        private Label lblCmdCode;
        private Label lblSql;
        private TextBox txtSql;
        private PictureBox pbInfo;
        private Panel pnlInfo;
        private Label lblInfo;
    }
}
