namespace Scada.Admin.Forms
{
    partial class FrmLogs
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
            this.components = new System.ComponentModel.Container();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblLoadFileList = new System.Windows.Forms.Label();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.chkPause = new System.Windows.Forms.CheckBox();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lblLoadFileList);
            this.pnlLeft.Controls.Add(this.lbFiles);
            this.pnlLeft.Controls.Add(this.cbFilter);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(250, 411);
            this.pnlLeft.TabIndex = 0;
            // 
            // lblLoadFileList
            // 
            this.lblLoadFileList.BackColor = System.Drawing.SystemColors.Window;
            this.lblLoadFileList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLoadFileList.Location = new System.Drawing.Point(0, 23);
            this.lblLoadFileList.Name = "lblLoadFileList";
            this.lblLoadFileList.Size = new System.Drawing.Size(250, 25);
            this.lblLoadFileList.TabIndex = 3;
            this.lblLoadFileList.Text = "Loading file list...";
            this.lblLoadFileList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoadFileList.Visible = false;
            // 
            // lbFiles
            // 
            this.lbFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.IntegralHeight = false;
            this.lbFiles.ItemHeight = 25;
            this.lbFiles.Location = new System.Drawing.Point(0, 23);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(250, 388);
            this.lbFiles.TabIndex = 1;
            this.lbFiles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbFiles_DrawItem);
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // cbFilter
            // 
            this.cbFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Location = new System.Drawing.Point(0, 0);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(250, 23);
            this.cbFilter.TabIndex = 0;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // chkPause
            // 
            this.chkPause.AutoSize = true;
            this.chkPause.Location = new System.Drawing.Point(262, 12);
            this.chkPause.Name = "chkPause";
            this.chkPause.Size = new System.Drawing.Size(57, 19);
            this.chkPause.TabIndex = 2;
            this.chkPause.Text = "Pause";
            this.chkPause.UseVisualStyleBackColor = true;
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbLog.FormattingEnabled = true;
            this.lbLog.HorizontalScrollbar = true;
            this.lbLog.IntegralHeight = false;
            this.lbLog.Location = new System.Drawing.Point(262, 37);
            this.lbLog.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.lbLog.Name = "lbLog";
            this.lbLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbLog.Size = new System.Drawing.Size(410, 362);
            this.lbLog.TabIndex = 4;
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Interval = 1000;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // FrmLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.chkPause);
            this.Controls.Add(this.pnlLeft);
            this.Name = "FrmLogs";
            this.Text = "Logs";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLogs_FormClosed);
            this.Load += new System.EventHandler(this.FrmLogs_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmLogs_VisibleChanged);
            this.pnlLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.CheckBox chkPause;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.Label lblLoadFileList;
        private System.Windows.Forms.ComboBox cbFilter;
    }
}