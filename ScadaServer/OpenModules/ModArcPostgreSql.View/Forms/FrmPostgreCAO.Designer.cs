
namespace Scada.Server.Modules.ModArcPostgreSql.View.Forms
{
    partial class FrmPostgreCAO
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
            ctrlCurrentArchiveOptions = new Server.Forms.Controls.CtrlCurrentArchiveOptions();
            btnManageConn = new Button();
            btnCancel = new Button();
            btnOK = new Button();
            tabControl = new TabControl();
            pageGeneral = new TabPage();
            pageDatabase = new TabPage();
            ctrlDatabaseOptions = new Controls.CtrlDatabaseOptions();
            pnlBottom = new Panel();
            tabControl.SuspendLayout();
            pageGeneral.SuspendLayout();
            pageDatabase.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // ctrlCurrentArchiveOptions
            // 
            ctrlCurrentArchiveOptions.ArchiveOptions = null;
            ctrlCurrentArchiveOptions.Location = new Point(8, 8);
            ctrlCurrentArchiveOptions.Name = "ctrlCurrentArchiveOptions";
            ctrlCurrentArchiveOptions.Size = new Size(360, 116);
            ctrlCurrentArchiveOptions.TabIndex = 0;
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
            // tabControl
            // 
            tabControl.Controls.Add(pageGeneral);
            tabControl.Controls.Add(pageDatabase);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(384, 220);
            tabControl.TabIndex = 0;
            // 
            // pageGeneral
            // 
            pageGeneral.Controls.Add(ctrlCurrentArchiveOptions);
            pageGeneral.Location = new Point(4, 24);
            pageGeneral.Name = "pageGeneral";
            pageGeneral.Padding = new Padding(5);
            pageGeneral.Size = new Size(376, 192);
            pageGeneral.TabIndex = 0;
            pageGeneral.Text = "General";
            pageGeneral.UseVisualStyleBackColor = true;
            // 
            // pageDatabase
            // 
            pageDatabase.Controls.Add(ctrlDatabaseOptions);
            pageDatabase.Location = new Point(4, 24);
            pageDatabase.Name = "pageDatabase";
            pageDatabase.Padding = new Padding(5);
            pageDatabase.Size = new Size(376, 192);
            pageDatabase.TabIndex = 1;
            pageDatabase.Text = "Database";
            pageDatabase.UseVisualStyleBackColor = true;
            // 
            // ctrlDatabaseOptions
            // 
            ctrlDatabaseOptions.Location = new Point(8, 8);
            ctrlDatabaseOptions.Name = "ctrlDatabaseOptions";
            ctrlDatabaseOptions.Size = new Size(360, 140);
            ctrlDatabaseOptions.TabIndex = 0;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnManageConn);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 220);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(384, 41);
            pnlBottom.TabIndex = 1;
            // 
            // FrmPostgreCAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 261);
            Controls.Add(tabControl);
            Controls.Add(pnlBottom);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPostgreCAO";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Current Archive Options";
            Load += FrmPostgreHAO_Load;
            tabControl.ResumeLayout(false);
            pageGeneral.ResumeLayout(false);
            pageDatabase.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnManageConn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Server.Forms.Controls.CtrlCurrentArchiveOptions ctrlCurrentArchiveOptions;
        private TabControl tabControl;
        private TabPage pageGeneral;
        private TabPage pageDatabase;
        private Controls.CtrlDatabaseOptions ctrlDatabaseOptions;
        private Panel pnlBottom;
    }
}