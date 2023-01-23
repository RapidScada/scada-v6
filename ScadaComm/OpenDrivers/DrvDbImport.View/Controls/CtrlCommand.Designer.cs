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
            this.components = new System.ComponentModel.Container();
            this.gbCmd = new System.Windows.Forms.GroupBox();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.pbHintInfo = new System.Windows.Forms.PictureBox();
            this.lblSql = new System.Windows.Forms.Label();
            this.pnlCmdCodeWarn = new System.Windows.Forms.Panel();
            this.lblCmdCodeWarn = new System.Windows.Forms.Label();
            this.pbCmdCodeWarn = new System.Windows.Forms.PictureBox();
            this.txtCmdCode = new System.Windows.Forms.TextBox();
            this.lblCmdCode = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHintInfo)).BeginInit();
            this.pnlCmdCodeWarn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCmdCodeWarn)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCmd
            // 
            this.gbCmd.Controls.Add(this.txtSql);
            this.gbCmd.Controls.Add(this.pbHintInfo);
            this.gbCmd.Controls.Add(this.lblSql);
            this.gbCmd.Controls.Add(this.pnlCmdCodeWarn);
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
            // txtSql
            // 
            this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSql.Location = new System.Drawing.Point(13, 128);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.Size = new System.Drawing.Size(378, 321);
            this.txtSql.TabIndex = 7;
            this.txtSql.WordWrap = false;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // pbHintInfo
            // 
            this.pbHintInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbHintInfo.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.info;
            this.pbHintInfo.Location = new System.Drawing.Point(375, 107);
            this.pbHintInfo.Name = "pbHintInfo";
            this.pbHintInfo.Size = new System.Drawing.Size(16, 16);
            this.pbHintInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbHintInfo.TabIndex = 12;
            this.pbHintInfo.TabStop = false;
            // 
            // lblSql
            // 
            this.lblSql.AutoSize = true;
            this.lblSql.Location = new System.Drawing.Point(10, 107);
            this.lblSql.Name = "lblSql";
            this.lblSql.Size = new System.Drawing.Size(28, 15);
            this.lblSql.TabIndex = 6;
            this.lblSql.Text = "SQL";
            // 
            // pnlCmdCodeWarn
            // 
            this.pnlCmdCodeWarn.Controls.Add(this.lblCmdCodeWarn);
            this.pnlCmdCodeWarn.Controls.Add(this.pbCmdCodeWarn);
            this.pnlCmdCodeWarn.Location = new System.Drawing.Point(153, 81);
            this.pnlCmdCodeWarn.Name = "pnlCmdCodeWarn";
            this.pnlCmdCodeWarn.Size = new System.Drawing.Size(134, 23);
            this.pnlCmdCodeWarn.TabIndex = 5;
            // 
            // lblCmdCodeWarn
            // 
            this.lblCmdCodeWarn.AutoSize = true;
            this.lblCmdCodeWarn.ForeColor = System.Drawing.Color.Red;
            this.lblCmdCodeWarn.Location = new System.Drawing.Point(19, 4);
            this.lblCmdCodeWarn.Name = "lblCmdCodeWarn";
            this.lblCmdCodeWarn.Size = new System.Drawing.Size(72, 15);
            this.lblCmdCodeWarn.TabIndex = 0;
            this.lblCmdCodeWarn.Text = "Fill out code";
            // 
            // pbCmdCodeWarn
            // 
            this.pbCmdCodeWarn.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.warning;
            this.pbCmdCodeWarn.Location = new System.Drawing.Point(0, 3);
            this.pbCmdCodeWarn.Name = "pbCmdCodeWarn";
            this.pbCmdCodeWarn.Size = new System.Drawing.Size(16, 16);
            this.pbCmdCodeWarn.TabIndex = 0;
            this.pbCmdCodeWarn.TabStop = false;
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
            ((System.ComponentModel.ISupportInitialize)(this.pbHintInfo)).EndInit();
            this.pnlCmdCodeWarn.ResumeLayout(false);
            this.pnlCmdCodeWarn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCmdCodeWarn)).EndInit();
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
        private PictureBox pbHintInfo;
        private ToolTip toolTip;
        private Panel pnlCmdCodeWarn;
        private Label lblCmdCodeWarn;
        private PictureBox pbCmdCodeWarn;
    }
}
