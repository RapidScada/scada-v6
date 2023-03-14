
namespace Scada.Server.Modules.ModArcPostgreSql.View.Forms
{
    partial class FrmPostgreEAO
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
            ctrlEventArchiveOptions = new Server.Forms.Controls.CtrlEventArchiveOptions();
            btnManageConn = new Button();
            btnCancel = new Button();
            btnOK = new Button();
            ctrlDatabaseOptions = new Controls.CtrlDatabaseOptions();
            SuspendLayout();
            // 
            // ctrlEventArchiveOptions
            // 
            ctrlEventArchiveOptions.ArchiveOptions = null;
            ctrlEventArchiveOptions.Location = new Point(12, 12);
            ctrlEventArchiveOptions.Name = "ctrlEventArchiveOptions";
            ctrlEventArchiveOptions.Size = new Size(360, 116);
            ctrlEventArchiveOptions.TabIndex = 0;
            // 
            // btnManageConn
            // 
            btnManageConn.Location = new Point(12, 325);
            btnManageConn.Name = "btnManageConn";
            btnManageConn.Size = new Size(140, 23);
            btnManageConn.TabIndex = 2;
            btnManageConn.Text = "Manage Connections";
            btnManageConn.UseVisualStyleBackColor = true;
            btnManageConn.Click += btnManageConn_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 325);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 325);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // ctrlDatabaseOptions
            // 
            ctrlDatabaseOptions.Location = new Point(12, 134);
            ctrlDatabaseOptions.Name = "ctrlDatabaseOptions";
            ctrlDatabaseOptions.Size = new Size(360, 175);
            ctrlDatabaseOptions.TabIndex = 1;
            // 
            // FrmPostgreEAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 360);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnManageConn);
            Controls.Add(ctrlDatabaseOptions);
            Controls.Add(ctrlEventArchiveOptions);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPostgreEAO";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Event Archive Options";
            Load += FrmPostgreHAO_Load;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnManageConn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Server.Forms.Controls.CtrlEventArchiveOptions ctrlEventArchiveOptions;
        private Controls.CtrlDatabaseOptions ctrlDatabaseOptions;
    }
}