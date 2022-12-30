namespace Scada.Server.Modules.ModDbExport.View.Forms
{
    partial class FrmQueryParametrs
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lvParametrs = new System.Windows.Forms.ListView();
            this.colParamsName = new System.Windows.Forms.ColumnHeader();
            this.colParams = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(430, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(349, 353);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lvParametrs
            // 
            this.lvParametrs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvParametrs.BackColor = System.Drawing.SystemColors.Window;
            this.lvParametrs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colParamsName,
            this.colParams});
            this.lvParametrs.FullRowSelect = true;
            this.lvParametrs.GridLines = true;
            this.lvParametrs.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvParametrs.Location = new System.Drawing.Point(12, 12);
            this.lvParametrs.Name = "lvParametrs";
            this.lvParametrs.Size = new System.Drawing.Size(493, 325);
            this.lvParametrs.TabIndex = 0;
            this.lvParametrs.UseCompatibleStateImageBehavior = false;
            this.lvParametrs.View = System.Windows.Forms.View.Details;
            // 
            // colParamsName
            // 
            this.colParamsName.Text = "Name parameter";
            this.colParamsName.Width = 160;
            // 
            // colParams
            // 
            this.colParams.Text = "Parameter";
            this.colParams.Width = 260;
            // 
            // FrmQueryParametrs
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(517, 388);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lvParametrs);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmQueryParametrs";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Available parametrs";
            this.Load += new System.EventHandler(this.FrmQueryParametrs_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private ListView lvParametrs;
        private ColumnHeader colParamsName;
        private ColumnHeader colParams;
    }
}