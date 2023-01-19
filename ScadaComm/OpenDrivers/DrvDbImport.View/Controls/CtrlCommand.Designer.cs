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
            this.pbParametersHint = new System.Windows.Forms.PictureBox();
            this.lblSql = new System.Windows.Forms.Label();
            this.txtCmdCode = new System.Windows.Forms.TextBox();
            this.lblCmdCode = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbParametersHint)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCmd
            // 
            this.gbCmd.Controls.Add(this.txtSql);
            this.gbCmd.Controls.Add(this.pbParametersHint);
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
            // txtSql
            // 
            this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSql.Location = new System.Drawing.Point(13, 139);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.Size = new System.Drawing.Size(378, 310);
            this.txtSql.TabIndex = 5;
            this.txtSql.WordWrap = false;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // pbParametersHint
            // 
            this.pbParametersHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbParametersHint.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.info;
            this.pbParametersHint.Location = new System.Drawing.Point(375, 117);
            this.pbParametersHint.Name = "pbParametersHint";
            this.pbParametersHint.Size = new System.Drawing.Size(16, 16);
            this.pbParametersHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbParametersHint.TabIndex = 12;
            this.pbParametersHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbParametersHint, "Avaliable parameters:\r\n@cmdVal, @cmdData");
            // 
            // lblSql
            // 
            this.lblSql.AutoSize = true;
            this.lblSql.Location = new System.Drawing.Point(10, 121);
            this.lblSql.Name = "lblSql";
            this.lblSql.Size = new System.Drawing.Size(23, 15);
            this.lblSql.TabIndex = 4;
            this.lblSql.Text = "Sql";
            // 
            // txtCmdCode
            // 
            this.txtCmdCode.Location = new System.Drawing.Point(13, 88);
            this.txtCmdCode.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtCmdCode.Name = "txtCmdCode";
            this.txtCmdCode.Size = new System.Drawing.Size(134, 23);
            this.txtCmdCode.TabIndex = 3;
            this.txtCmdCode.TextChanged += new System.EventHandler(this.txtCmdCode_TextChanged);
            // 
            // lblCmdCode
            // 
            this.lblCmdCode.AutoSize = true;
            this.lblCmdCode.Location = new System.Drawing.Point(10, 70);
            this.lblCmdCode.Name = "lblCmdCode";
            this.lblCmdCode.Size = new System.Drawing.Size(93, 15);
            this.lblCmdCode.TabIndex = 2;
            this.lblCmdCode.Text = "Command code";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(13, 37);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(378, 23);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 19);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
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
            ((System.ComponentModel.ISupportInitialize)(this.pbParametersHint)).EndInit();
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
        private PictureBox pbParametersHint;
        private ToolTip toolTip;
    }
}
