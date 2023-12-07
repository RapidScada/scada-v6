namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View
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
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lvParameters = new System.Windows.Forms.ListView();
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
            // lvParameters
            // 
            this.lvParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvParameters.BackColor = System.Drawing.SystemColors.Window;
            this.lvParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colParamsName,
            this.colParams});
            this.lvParameters.FullRowSelect = true;
            this.lvParameters.GridLines = true;
            this.lvParameters.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
            this.lvParameters.Location = new System.Drawing.Point(12, 12);
            this.lvParameters.Name = "lvParameters";
            this.lvParameters.Size = new System.Drawing.Size(493, 325);
            this.lvParameters.TabIndex = 0;
            this.lvParameters.UseCompatibleStateImageBehavior = false;
            this.lvParameters.View = System.Windows.Forms.View.Details;
            // 
            // colParamsName
            // 
            this.colParamsName.Text = "Name";
            this.colParamsName.Width = 160;
            // 
            // colParams
            // 
            this.colParams.Text = "Description";
            this.colParams.Width = 310;
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
            this.Controls.Add(this.lvParameters);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "FrmQueryParametrs";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Available Parameters";
            this.Load += new System.EventHandler(this.FrmQueryParametrs_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private ListView lvParameters;
        private ColumnHeader colParamsName;
        private ColumnHeader colParams;
    }
}