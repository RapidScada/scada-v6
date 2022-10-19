namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    partial class FrmArchiveAdd
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
            this.lblSourceArchive = new System.Windows.Forms.Label();
            this.cbSourceArchive = new System.Windows.Forms.ComboBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.cbModule = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSourceArchive
            // 
            this.lblSourceArchive.AutoSize = true;
            this.lblSourceArchive.Location = new System.Drawing.Point(9, 9);
            this.lblSourceArchive.Name = "lblSourceArchive";
            this.lblSourceArchive.Size = new System.Drawing.Size(84, 15);
            this.lblSourceArchive.TabIndex = 0;
            this.lblSourceArchive.Text = "Source archive";
            // 
            // cbSourceArchive
            // 
            this.cbSourceArchive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSourceArchive.FormattingEnabled = true;
            this.cbSourceArchive.Location = new System.Drawing.Point(12, 27);
            this.cbSourceArchive.Name = "cbSourceArchive";
            this.cbSourceArchive.Size = new System.Drawing.Size(360, 23);
            this.cbSourceArchive.TabIndex = 1;
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(9, 53);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(48, 15);
            this.lblModule.TabIndex = 2;
            this.lblModule.Text = "Module";
            // 
            // cbModule
            // 
            this.cbModule.FormattingEnabled = true;
            this.cbModule.Location = new System.Drawing.Point(12, 71);
            this.cbModule.Name = "cbModule";
            this.cbModule.Size = new System.Drawing.Size(360, 23);
            this.cbModule.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 110);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 110);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmArchiveAdd
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 145);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbModule);
            this.Controls.Add(this.lblModule);
            this.Controls.Add(this.cbSourceArchive);
            this.Controls.Add(this.lblSourceArchive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmArchiveAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Archive";
            this.Load += new System.EventHandler(this.FrmArchiveAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSourceArchive;
        private System.Windows.Forms.ComboBox cbSourceArchive;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.ComboBox cbModule;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}