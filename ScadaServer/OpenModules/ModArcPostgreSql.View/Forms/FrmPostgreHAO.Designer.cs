﻿
namespace Scada.Server.Modules.ModArcPostgreSql.View.Forms
{
    partial class FrmPostgreHAO
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
            ctrlHistoricalArchiveOptions = new Server.Forms.Controls.CtrlHistoricalArchiveOptions();
            btnManageConn = new Button();
            btnCancel = new Button();
            btnOK = new Button();
            ctrlDatabaseOptions = new Controls.CtrlDatabaseOptions();
            tabControl = new TabControl();
            pageGeneral = new TabPage();
            pageDatabase = new TabPage();
            numCacheSizeRatio = new NumericUpDown();
            lblCacheSizeRatio = new Label();
            chkUseMemoryCache = new CheckBox();
            lblUseMemoryCache = new Label();
            panel1 = new Panel();
            tabControl.SuspendLayout();
            pageGeneral.SuspendLayout();
            pageDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCacheSizeRatio).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // ctrlHistoricalArchiveOptions
            // 
            ctrlHistoricalArchiveOptions.ArchiveOptions = null;
            ctrlHistoricalArchiveOptions.Location = new Point(8, 8);
            ctrlHistoricalArchiveOptions.Name = "ctrlHistoricalArchiveOptions";
            ctrlHistoricalArchiveOptions.Size = new Size(360, 309);
            ctrlHistoricalArchiveOptions.TabIndex = 0;
            // 
            // btnManageConn
            // 
            btnManageConn.Location = new Point(12, 6);
            btnManageConn.Name = "btnManageConn";
            btnManageConn.Size = new Size(140, 23);
            btnManageConn.TabIndex = 0;
            btnManageConn.Text = "Manage Connections";
            btnManageConn.UseVisualStyleBackColor = true;
            btnManageConn.Click += btnManageConn_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 6);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // ctrlDatabaseOptions
            // 
            ctrlDatabaseOptions.Location = new Point(8, 8);
            ctrlDatabaseOptions.Name = "ctrlDatabaseOptions";
            ctrlDatabaseOptions.Size = new Size(360, 136);
            ctrlDatabaseOptions.TabIndex = 0;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(pageGeneral);
            tabControl.Controls.Add(pageDatabase);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(384, 353);
            tabControl.TabIndex = 0;
            // 
            // pageGeneral
            // 
            pageGeneral.Controls.Add(ctrlHistoricalArchiveOptions);
            pageGeneral.Location = new Point(4, 24);
            pageGeneral.Name = "pageGeneral";
            pageGeneral.Padding = new Padding(5);
            pageGeneral.Size = new Size(376, 325);
            pageGeneral.TabIndex = 0;
            pageGeneral.Text = "General";
            pageGeneral.UseVisualStyleBackColor = true;
            // 
            // pageDatabase
            // 
            pageDatabase.Controls.Add(numCacheSizeRatio);
            pageDatabase.Controls.Add(lblCacheSizeRatio);
            pageDatabase.Controls.Add(chkUseMemoryCache);
            pageDatabase.Controls.Add(lblUseMemoryCache);
            pageDatabase.Controls.Add(ctrlDatabaseOptions);
            pageDatabase.Location = new Point(4, 24);
            pageDatabase.Name = "pageDatabase";
            pageDatabase.Padding = new Padding(5);
            pageDatabase.Size = new Size(376, 325);
            pageDatabase.TabIndex = 1;
            pageDatabase.Text = "Database";
            pageDatabase.UseVisualStyleBackColor = true;
            // 
            // numCacheSizeRatio
            // 
            numCacheSizeRatio.DecimalPlaces = 2;
            numCacheSizeRatio.Location = new Point(217, 179);
            numCacheSizeRatio.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numCacheSizeRatio.Name = "numCacheSizeRatio";
            numCacheSizeRatio.Size = new Size(151, 23);
            numCacheSizeRatio.TabIndex = 4;
            numCacheSizeRatio.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblCacheSizeRatio
            // 
            lblCacheSizeRatio.AutoSize = true;
            lblCacheSizeRatio.Location = new Point(5, 183);
            lblCacheSizeRatio.Name = "lblCacheSizeRatio";
            lblCacheSizeRatio.Size = new Size(89, 15);
            lblCacheSizeRatio.TabIndex = 3;
            lblCacheSizeRatio.Text = "Cache size ratio";
            // 
            // chkUseMemoryCache
            // 
            chkUseMemoryCache.AutoSize = true;
            chkUseMemoryCache.Location = new Point(353, 154);
            chkUseMemoryCache.Name = "chkUseMemoryCache";
            chkUseMemoryCache.Size = new Size(15, 14);
            chkUseMemoryCache.TabIndex = 2;
            chkUseMemoryCache.UseVisualStyleBackColor = true;
            // 
            // lblUseMemoryCache
            // 
            lblUseMemoryCache.AutoSize = true;
            lblUseMemoryCache.Location = new Point(5, 154);
            lblUseMemoryCache.Name = "lblUseMemoryCache";
            lblUseMemoryCache.Size = new Size(123, 15);
            lblUseMemoryCache.TabIndex = 1;
            lblUseMemoryCache.Text = "Use in-memory cache";
            // 
            // panel1
            // 
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnManageConn);
            panel1.Controls.Add(btnOK);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 353);
            panel1.Name = "panel1";
            panel1.Size = new Size(384, 41);
            panel1.TabIndex = 1;
            // 
            // FrmPostgreHAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 394);
            Controls.Add(tabControl);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPostgreHAO";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Historical Archive Options";
            Load += FrmPostgreHAO_Load;
            tabControl.ResumeLayout(false);
            pageGeneral.ResumeLayout(false);
            pageDatabase.ResumeLayout(false);
            pageDatabase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCacheSizeRatio).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnManageConn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Scada.Server.Forms.Controls.CtrlHistoricalArchiveOptions ctrlHistoricalArchiveOptions;
        private Controls.CtrlDatabaseOptions ctrlDatabaseOptions;
        private TabControl tabControl;
        private TabPage pageGeneral;
        private TabPage pageDatabase;
        private Panel panel1;
        private CheckBox chkUseMemoryCache;
        private Label lblUseMemoryCache;
        private Label lblCacheSizeRatio;
        private NumericUpDown numCacheSizeRatio;
    }
}