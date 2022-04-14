
namespace Scada.Comm.Drivers.DrvDsScadaServer.View.Forms
{
    partial class FrmConnManager
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
            this.btnNewConn = new System.Windows.Forms.Button();
            this.btnDeleteConn = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbConnList = new System.Windows.Forms.GroupBox();
            this.lvConn = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.ctrlClientConnection = new Scada.Forms.Controls.CtrlClientConnection();
            this.gbConnList.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNewConn
            // 
            this.btnNewConn.Location = new System.Drawing.Point(13, 22);
            this.btnNewConn.Name = "btnNewConn";
            this.btnNewConn.Size = new System.Drawing.Size(94, 23);
            this.btnNewConn.TabIndex = 0;
            this.btnNewConn.Text = "New";
            this.btnNewConn.UseVisualStyleBackColor = true;
            this.btnNewConn.Click += new System.EventHandler(this.btnNewConn_Click);
            // 
            // btnDeleteConn
            // 
            this.btnDeleteConn.Location = new System.Drawing.Point(113, 22);
            this.btnDeleteConn.Name = "btnDeleteConn";
            this.btnDeleteConn.Size = new System.Drawing.Size(94, 23);
            this.btnDeleteConn.TabIndex = 1;
            this.btnDeleteConn.Text = "Delete";
            this.btnDeleteConn.UseVisualStyleBackColor = true;
            this.btnDeleteConn.Click += new System.EventHandler(this.btnDeleteConn_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(463, 394);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(382, 394);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbConnList
            // 
            this.gbConnList.Controls.Add(this.lvConn);
            this.gbConnList.Controls.Add(this.btnDeleteConn);
            this.gbConnList.Controls.Add(this.btnNewConn);
            this.gbConnList.Location = new System.Drawing.Point(12, 12);
            this.gbConnList.Name = "gbConnList";
            this.gbConnList.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnList.Size = new System.Drawing.Size(220, 366);
            this.gbConnList.TabIndex = 0;
            this.gbConnList.TabStop = false;
            this.gbConnList.Text = "Connections";
            // 
            // lvConn
            // 
            this.lvConn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
            this.lvConn.FullRowSelect = true;
            this.lvConn.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvConn.HideSelection = false;
            this.lvConn.Location = new System.Drawing.Point(13, 51);
            this.lvConn.MultiSelect = false;
            this.lvConn.Name = "lvConn";
            this.lvConn.ShowGroups = false;
            this.lvConn.ShowItemToolTips = true;
            this.lvConn.Size = new System.Drawing.Size(194, 302);
            this.lvConn.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvConn.TabIndex = 2;
            this.lvConn.UseCompatibleStateImageBehavior = false;
            this.lvConn.View = System.Windows.Forms.View.Details;
            this.lvConn.SelectedIndexChanged += new System.EventHandler(this.lvConn_SelectedIndexChanged);
            // 
            // colName
            // 
            this.colName.Width = 186;
            // 
            // ctrlClientConnection
            // 
            this.ctrlClientConnection.ConnectionOptions = null;
            this.ctrlClientConnection.InstanceEnabled = false;
            this.ctrlClientConnection.Location = new System.Drawing.Point(238, 12);
            this.ctrlClientConnection.Name = "ctrlClientConnection";
            this.ctrlClientConnection.NameEnabled = true;
            this.ctrlClientConnection.Size = new System.Drawing.Size(300, 366);
            this.ctrlClientConnection.TabIndex = 1;
            this.ctrlClientConnection.NameChanged += new System.EventHandler(this.ctrlClientConnection_NameChanged);
            this.ctrlClientConnection.NameValidated += new System.EventHandler(this.ctrlClientConnection_NameValidated);
            // 
            // FrmConnManager
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(550, 429);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ctrlClientConnection);
            this.Controls.Add(this.gbConnList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConnManager";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Client Connections";
            this.Load += new System.EventHandler(this.FrmConnManager_Load);
            this.gbConnList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnNewConn;
        private System.Windows.Forms.Button btnDeleteConn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbConnList;
        private System.Windows.Forms.ListView lvConn;
        private System.Windows.Forms.ColumnHeader colName;
        private Scada.Forms.Controls.CtrlClientConnection ctrlClientConnection;
    }
}