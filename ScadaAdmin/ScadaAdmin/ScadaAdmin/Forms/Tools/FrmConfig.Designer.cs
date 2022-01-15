namespace Scada.Admin.App.Forms.Tools
{
    partial class FrmConfig
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageExt = new System.Windows.Forms.TabPage();
            this.txtExtDescr = new System.Windows.Forms.TextBox();
            this.lblExtDescr = new System.Windows.Forms.Label();
            this.lbActiveExt = new System.Windows.Forms.ListBox();
            this.btnExtProperties = new System.Windows.Forms.Button();
            this.btnMoveDownExt = new System.Windows.Forms.Button();
            this.btnMoveUpExt = new System.Windows.Forms.Button();
            this.btnDeactivateExt = new System.Windows.Forms.Button();
            this.lblActiveExt = new System.Windows.Forms.Label();
            this.lbUnusedExt = new System.Windows.Forms.ListBox();
            this.btnActivateExt = new System.Windows.Forms.Button();
            this.lblUnusedExt = new System.Windows.Forms.Label();
            this.pageFileAssoc = new System.Windows.Forms.TabPage();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAddAssoc = new System.Windows.Forms.Button();
            this.btnEditAssoc = new System.Windows.Forms.Button();
            this.btnDeleteAssoc = new System.Windows.Forms.Button();
            this.btnRegisterRsproj = new System.Windows.Forms.Button();
            this.lvAssoc = new System.Windows.Forms.ListView();
            this.colExt = new System.Windows.Forms.ColumnHeader();
            this.colPath = new System.Windows.Forms.ColumnHeader();
            this.tabControl.SuspendLayout();
            this.pageExt.SuspendLayout();
            this.pageFileAssoc.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageExt);
            this.tabControl.Controls.Add(this.pageFileAssoc);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(734, 511);
            this.tabControl.TabIndex = 0;
            // 
            // pageExt
            // 
            this.pageExt.Controls.Add(this.txtExtDescr);
            this.pageExt.Controls.Add(this.lblExtDescr);
            this.pageExt.Controls.Add(this.lbActiveExt);
            this.pageExt.Controls.Add(this.btnExtProperties);
            this.pageExt.Controls.Add(this.btnMoveDownExt);
            this.pageExt.Controls.Add(this.btnMoveUpExt);
            this.pageExt.Controls.Add(this.btnDeactivateExt);
            this.pageExt.Controls.Add(this.lblActiveExt);
            this.pageExt.Controls.Add(this.lbUnusedExt);
            this.pageExt.Controls.Add(this.btnActivateExt);
            this.pageExt.Controls.Add(this.lblUnusedExt);
            this.pageExt.Location = new System.Drawing.Point(4, 24);
            this.pageExt.Name = "pageExt";
            this.pageExt.Padding = new System.Windows.Forms.Padding(3);
            this.pageExt.Size = new System.Drawing.Size(726, 483);
            this.pageExt.TabIndex = 0;
            this.pageExt.Text = "Extenstions";
            this.pageExt.UseVisualStyleBackColor = true;
            // 
            // txtExtDescr
            // 
            this.txtExtDescr.Location = new System.Drawing.Point(6, 340);
            this.txtExtDescr.Multiline = true;
            this.txtExtDescr.Name = "txtExtDescr";
            this.txtExtDescr.ReadOnly = true;
            this.txtExtDescr.Size = new System.Drawing.Size(714, 100);
            this.txtExtDescr.TabIndex = 10;
            // 
            // lblExtDescr
            // 
            this.lblExtDescr.AutoSize = true;
            this.lblExtDescr.Location = new System.Drawing.Point(3, 322);
            this.lblExtDescr.Name = "lblExtDescr";
            this.lblExtDescr.Size = new System.Drawing.Size(67, 15);
            this.lblExtDescr.TabIndex = 9;
            this.lblExtDescr.Text = "Description";
            // 
            // lbActiveExt
            // 
            this.lbActiveExt.FormattingEnabled = true;
            this.lbActiveExt.IntegralHeight = false;
            this.lbActiveExt.ItemHeight = 15;
            this.lbActiveExt.Location = new System.Drawing.Point(366, 53);
            this.lbActiveExt.Name = "lbActiveExt";
            this.lbActiveExt.Size = new System.Drawing.Size(354, 261);
            this.lbActiveExt.TabIndex = 8;
            // 
            // btnExtProperties
            // 
            this.btnExtProperties.Location = new System.Drawing.Point(624, 24);
            this.btnExtProperties.Name = "btnExtProperties";
            this.btnExtProperties.Size = new System.Drawing.Size(80, 23);
            this.btnExtProperties.TabIndex = 7;
            this.btnExtProperties.Text = "Properties";
            this.btnExtProperties.UseVisualStyleBackColor = true;
            // 
            // btnMoveDownExt
            // 
            this.btnMoveDownExt.Location = new System.Drawing.Point(538, 24);
            this.btnMoveDownExt.Name = "btnMoveDownExt";
            this.btnMoveDownExt.Size = new System.Drawing.Size(80, 23);
            this.btnMoveDownExt.TabIndex = 6;
            this.btnMoveDownExt.Text = "Move Down";
            this.btnMoveDownExt.UseVisualStyleBackColor = true;
            // 
            // btnMoveUpExt
            // 
            this.btnMoveUpExt.Location = new System.Drawing.Point(452, 24);
            this.btnMoveUpExt.Name = "btnMoveUpExt";
            this.btnMoveUpExt.Size = new System.Drawing.Size(80, 23);
            this.btnMoveUpExt.TabIndex = 5;
            this.btnMoveUpExt.Text = "Move Up";
            this.btnMoveUpExt.UseVisualStyleBackColor = true;
            // 
            // btnDeactivateExt
            // 
            this.btnDeactivateExt.Location = new System.Drawing.Point(366, 24);
            this.btnDeactivateExt.Name = "btnDeactivateExt";
            this.btnDeactivateExt.Size = new System.Drawing.Size(80, 23);
            this.btnDeactivateExt.TabIndex = 4;
            this.btnDeactivateExt.Text = "Deactivate";
            this.btnDeactivateExt.UseVisualStyleBackColor = true;
            // 
            // lblActiveExt
            // 
            this.lblActiveExt.AutoSize = true;
            this.lblActiveExt.Location = new System.Drawing.Point(363, 6);
            this.lblActiveExt.Name = "lblActiveExt";
            this.lblActiveExt.Size = new System.Drawing.Size(102, 15);
            this.lblActiveExt.TabIndex = 3;
            this.lblActiveExt.Text = "Active extensions:";
            // 
            // lbUnusedExt
            // 
            this.lbUnusedExt.FormattingEnabled = true;
            this.lbUnusedExt.IntegralHeight = false;
            this.lbUnusedExt.ItemHeight = 15;
            this.lbUnusedExt.Location = new System.Drawing.Point(6, 53);
            this.lbUnusedExt.Name = "lbUnusedExt";
            this.lbUnusedExt.Size = new System.Drawing.Size(354, 261);
            this.lbUnusedExt.TabIndex = 2;
            // 
            // btnActivateExt
            // 
            this.btnActivateExt.Location = new System.Drawing.Point(6, 24);
            this.btnActivateExt.Name = "btnActivateExt";
            this.btnActivateExt.Size = new System.Drawing.Size(100, 23);
            this.btnActivateExt.TabIndex = 1;
            this.btnActivateExt.Text = "Activate";
            this.btnActivateExt.UseVisualStyleBackColor = true;
            // 
            // lblUnusedExt
            // 
            this.lblUnusedExt.AutoSize = true;
            this.lblUnusedExt.Location = new System.Drawing.Point(3, 6);
            this.lblUnusedExt.Name = "lblUnusedExt";
            this.lblUnusedExt.Size = new System.Drawing.Size(109, 15);
            this.lblUnusedExt.TabIndex = 0;
            this.lblUnusedExt.Text = "Unused extensions:";
            // 
            // pageFileAssoc
            // 
            this.pageFileAssoc.Controls.Add(this.lvAssoc);
            this.pageFileAssoc.Controls.Add(this.btnRegisterRsproj);
            this.pageFileAssoc.Controls.Add(this.btnDeleteAssoc);
            this.pageFileAssoc.Controls.Add(this.btnEditAssoc);
            this.pageFileAssoc.Controls.Add(this.btnAddAssoc);
            this.pageFileAssoc.Location = new System.Drawing.Point(4, 24);
            this.pageFileAssoc.Name = "pageFileAssoc";
            this.pageFileAssoc.Padding = new System.Windows.Forms.Padding(3);
            this.pageFileAssoc.Size = new System.Drawing.Size(726, 483);
            this.pageFileAssoc.TabIndex = 1;
            this.pageFileAssoc.Text = "File Associations";
            this.pageFileAssoc.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 470);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(734, 41);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(647, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(566, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnAddAssoc
            // 
            this.btnAddAssoc.Location = new System.Drawing.Point(6, 6);
            this.btnAddAssoc.Name = "btnAddAssoc";
            this.btnAddAssoc.Size = new System.Drawing.Size(75, 23);
            this.btnAddAssoc.TabIndex = 0;
            this.btnAddAssoc.Text = "Add";
            this.btnAddAssoc.UseVisualStyleBackColor = true;
            // 
            // btnEditAssoc
            // 
            this.btnEditAssoc.Location = new System.Drawing.Point(87, 6);
            this.btnEditAssoc.Name = "btnEditAssoc";
            this.btnEditAssoc.Size = new System.Drawing.Size(75, 23);
            this.btnEditAssoc.TabIndex = 1;
            this.btnEditAssoc.Text = "Edit";
            this.btnEditAssoc.UseVisualStyleBackColor = true;
            // 
            // btnDeleteAssoc
            // 
            this.btnDeleteAssoc.Location = new System.Drawing.Point(168, 6);
            this.btnDeleteAssoc.Name = "btnDeleteAssoc";
            this.btnDeleteAssoc.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteAssoc.TabIndex = 2;
            this.btnDeleteAssoc.Text = "Delete";
            this.btnDeleteAssoc.UseVisualStyleBackColor = true;
            // 
            // btnRegisterRsproj
            // 
            this.btnRegisterRsproj.Location = new System.Drawing.Point(620, 6);
            this.btnRegisterRsproj.Name = "btnRegisterRsproj";
            this.btnRegisterRsproj.Size = new System.Drawing.Size(100, 23);
            this.btnRegisterRsproj.TabIndex = 3;
            this.btnRegisterRsproj.Text = "Register *.rsproj";
            this.btnRegisterRsproj.UseVisualStyleBackColor = true;
            // 
            // lvAssoc
            // 
            this.lvAssoc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colExt,
            this.colPath});
            this.lvAssoc.FullRowSelect = true;
            this.lvAssoc.GridLines = true;
            this.lvAssoc.Location = new System.Drawing.Point(6, 35);
            this.lvAssoc.MultiSelect = false;
            this.lvAssoc.Name = "lvAssoc";
            this.lvAssoc.ShowItemToolTips = true;
            this.lvAssoc.Size = new System.Drawing.Size(714, 405);
            this.lvAssoc.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvAssoc.TabIndex = 4;
            this.lvAssoc.UseCompatibleStateImageBehavior = false;
            this.lvAssoc.View = System.Windows.Forms.View.Details;
            // 
            // colExt
            // 
            this.colExt.Text = "File Extenstion";
            this.colExt.Width = 120;
            // 
            // colPath
            // 
            this.colPath.Text = "Executable Path";
            this.colPath.Width = 550;
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 511);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tabControl.ResumeLayout(false);
            this.pageExt.ResumeLayout(false);
            this.pageExt.PerformLayout();
            this.pageFileAssoc.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageExt;
        private System.Windows.Forms.TabPage pageFileAssoc;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnActivateExt;
        private System.Windows.Forms.Label lblUnusedExt;
        private System.Windows.Forms.ListBox lbUnusedExt;
        private System.Windows.Forms.Button btnExtProperties;
        private System.Windows.Forms.Button btnMoveDownExt;
        private System.Windows.Forms.Button btnMoveUpExt;
        private System.Windows.Forms.Button btnDeactivateExt;
        private System.Windows.Forms.Label lblActiveExt;
        private System.Windows.Forms.ListBox lbActiveExt;
        private System.Windows.Forms.TextBox txtExtDescr;
        private System.Windows.Forms.Label lblExtDescr;
        private System.Windows.Forms.Button btnDeleteAssoc;
        private System.Windows.Forms.Button btnEditAssoc;
        private System.Windows.Forms.Button btnAddAssoc;
        private System.Windows.Forms.Button btnRegisterRsproj;
        private System.Windows.Forms.ListView lvAssoc;
        private System.Windows.Forms.ColumnHeader colExt;
        private System.Windows.Forms.ColumnHeader colPath;
    }
}