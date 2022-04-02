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
            this.lvAssoc = new System.Windows.Forms.ListView();
            this.colExt = new System.Windows.Forms.ColumnHeader();
            this.colPath = new System.Windows.Forms.ColumnHeader();
            this.btnRegisterProjectExt = new System.Windows.Forms.Button();
            this.btnDeleteAssoc = new System.Windows.Forms.Button();
            this.btnEditAssoc = new System.Windows.Forms.Button();
            this.btnAddAssoc = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pageCnlNum = new System.Windows.Forms.TabPage();
            this.chkPrependDeviceName = new System.Windows.Forms.CheckBox();
            this.numGap = new System.Windows.Forms.NumericUpDown();
            this.lblGap = new System.Windows.Forms.Label();
            this.numShift = new System.Windows.Forms.NumericUpDown();
            this.lblShift = new System.Windows.Forms.Label();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.numMultiplicity = new System.Windows.Forms.NumericUpDown();
            this.lblMultiplicity = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.pageExt.SuspendLayout();
            this.pageFileAssoc.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.pageCnlNum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMultiplicity)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageExt);
            this.tabControl.Controls.Add(this.pageFileAssoc);
            this.tabControl.Controls.Add(this.pageCnlNum);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(684, 470);
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
            this.pageExt.Padding = new System.Windows.Forms.Padding(5);
            this.pageExt.Size = new System.Drawing.Size(676, 442);
            this.pageExt.TabIndex = 0;
            this.pageExt.Text = "Extenstions";
            this.pageExt.UseVisualStyleBackColor = true;
            // 
            // txtExtDescr
            // 
            this.txtExtDescr.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtExtDescr.Location = new System.Drawing.Point(8, 355);
            this.txtExtDescr.Multiline = true;
            this.txtExtDescr.Name = "txtExtDescr";
            this.txtExtDescr.ReadOnly = true;
            this.txtExtDescr.Size = new System.Drawing.Size(660, 79);
            this.txtExtDescr.TabIndex = 10;
            // 
            // lblExtDescr
            // 
            this.lblExtDescr.AutoSize = true;
            this.lblExtDescr.Location = new System.Drawing.Point(5, 337);
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
            this.lbActiveExt.Location = new System.Drawing.Point(330, 53);
            this.lbActiveExt.Name = "lbActiveExt";
            this.lbActiveExt.Size = new System.Drawing.Size(338, 276);
            this.lbActiveExt.TabIndex = 8;
            this.lbActiveExt.SelectedIndexChanged += new System.EventHandler(this.lbActiveExt_SelectedIndexChanged);
            this.lbActiveExt.DoubleClick += new System.EventHandler(this.lbActiveExt_DoubleClick);
            // 
            // btnExtProperties
            // 
            this.btnExtProperties.Location = new System.Drawing.Point(588, 24);
            this.btnExtProperties.Name = "btnExtProperties";
            this.btnExtProperties.Size = new System.Drawing.Size(80, 23);
            this.btnExtProperties.TabIndex = 7;
            this.btnExtProperties.Text = "Properties";
            this.btnExtProperties.UseVisualStyleBackColor = true;
            this.btnExtProperties.Click += new System.EventHandler(this.btnExtProperties_Click);
            // 
            // btnMoveDownExt
            // 
            this.btnMoveDownExt.Location = new System.Drawing.Point(502, 23);
            this.btnMoveDownExt.Name = "btnMoveDownExt";
            this.btnMoveDownExt.Size = new System.Drawing.Size(80, 23);
            this.btnMoveDownExt.TabIndex = 6;
            this.btnMoveDownExt.Text = "Move Down";
            this.btnMoveDownExt.UseVisualStyleBackColor = true;
            this.btnMoveDownExt.Click += new System.EventHandler(this.btnMoveDownExt_Click);
            // 
            // btnMoveUpExt
            // 
            this.btnMoveUpExt.Location = new System.Drawing.Point(416, 23);
            this.btnMoveUpExt.Name = "btnMoveUpExt";
            this.btnMoveUpExt.Size = new System.Drawing.Size(80, 23);
            this.btnMoveUpExt.TabIndex = 5;
            this.btnMoveUpExt.Text = "Move Up";
            this.btnMoveUpExt.UseVisualStyleBackColor = true;
            this.btnMoveUpExt.Click += new System.EventHandler(this.btnMoveUpExt_Click);
            // 
            // btnDeactivateExt
            // 
            this.btnDeactivateExt.Location = new System.Drawing.Point(330, 23);
            this.btnDeactivateExt.Name = "btnDeactivateExt";
            this.btnDeactivateExt.Size = new System.Drawing.Size(80, 23);
            this.btnDeactivateExt.TabIndex = 4;
            this.btnDeactivateExt.Text = "Deactivate";
            this.btnDeactivateExt.UseVisualStyleBackColor = true;
            this.btnDeactivateExt.Click += new System.EventHandler(this.btnDeactivateExt_Click);
            // 
            // lblActiveExt
            // 
            this.lblActiveExt.AutoSize = true;
            this.lblActiveExt.Location = new System.Drawing.Point(330, 5);
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
            this.lbUnusedExt.Location = new System.Drawing.Point(8, 52);
            this.lbUnusedExt.Name = "lbUnusedExt";
            this.lbUnusedExt.Size = new System.Drawing.Size(316, 277);
            this.lbUnusedExt.TabIndex = 2;
            this.lbUnusedExt.SelectedIndexChanged += new System.EventHandler(this.lbUnusedExt_SelectedIndexChanged);
            this.lbUnusedExt.DoubleClick += new System.EventHandler(this.lbUnusedExt_DoubleClick);
            // 
            // btnActivateExt
            // 
            this.btnActivateExt.Location = new System.Drawing.Point(8, 23);
            this.btnActivateExt.Name = "btnActivateExt";
            this.btnActivateExt.Size = new System.Drawing.Size(100, 23);
            this.btnActivateExt.TabIndex = 1;
            this.btnActivateExt.Text = "Activate";
            this.btnActivateExt.UseVisualStyleBackColor = true;
            this.btnActivateExt.Click += new System.EventHandler(this.btnActivateExt_Click);
            // 
            // lblUnusedExt
            // 
            this.lblUnusedExt.AutoSize = true;
            this.lblUnusedExt.Location = new System.Drawing.Point(5, 5);
            this.lblUnusedExt.Name = "lblUnusedExt";
            this.lblUnusedExt.Size = new System.Drawing.Size(109, 15);
            this.lblUnusedExt.TabIndex = 0;
            this.lblUnusedExt.Text = "Unused extensions:";
            // 
            // pageFileAssoc
            // 
            this.pageFileAssoc.Controls.Add(this.lvAssoc);
            this.pageFileAssoc.Controls.Add(this.btnRegisterProjectExt);
            this.pageFileAssoc.Controls.Add(this.btnDeleteAssoc);
            this.pageFileAssoc.Controls.Add(this.btnEditAssoc);
            this.pageFileAssoc.Controls.Add(this.btnAddAssoc);
            this.pageFileAssoc.Location = new System.Drawing.Point(4, 24);
            this.pageFileAssoc.Name = "pageFileAssoc";
            this.pageFileAssoc.Padding = new System.Windows.Forms.Padding(5);
            this.pageFileAssoc.Size = new System.Drawing.Size(676, 442);
            this.pageFileAssoc.TabIndex = 1;
            this.pageFileAssoc.Text = "File Associations";
            this.pageFileAssoc.UseVisualStyleBackColor = true;
            // 
            // lvAssoc
            // 
            this.lvAssoc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colExt,
            this.colPath});
            this.lvAssoc.FullRowSelect = true;
            this.lvAssoc.GridLines = true;
            this.lvAssoc.Location = new System.Drawing.Point(8, 35);
            this.lvAssoc.MultiSelect = false;
            this.lvAssoc.Name = "lvAssoc";
            this.lvAssoc.ShowItemToolTips = true;
            this.lvAssoc.Size = new System.Drawing.Size(660, 399);
            this.lvAssoc.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvAssoc.TabIndex = 4;
            this.lvAssoc.UseCompatibleStateImageBehavior = false;
            this.lvAssoc.View = System.Windows.Forms.View.Details;
            this.lvAssoc.SelectedIndexChanged += new System.EventHandler(this.lvAssoc_SelectedIndexChanged);
            this.lvAssoc.DoubleClick += new System.EventHandler(this.lvAssoc_DoubleClick);
            // 
            // colExt
            // 
            this.colExt.Text = "File Extenstion";
            this.colExt.Width = 120;
            // 
            // colPath
            // 
            this.colPath.Text = "Executable Path";
            this.colPath.Width = 500;
            // 
            // btnRegisterProjectExt
            // 
            this.btnRegisterProjectExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegisterProjectExt.Location = new System.Drawing.Point(548, 8);
            this.btnRegisterProjectExt.Name = "btnRegisterProjectExt";
            this.btnRegisterProjectExt.Size = new System.Drawing.Size(120, 23);
            this.btnRegisterProjectExt.TabIndex = 3;
            this.btnRegisterProjectExt.Text = "Register .rsproj";
            this.btnRegisterProjectExt.UseVisualStyleBackColor = true;
            this.btnRegisterProjectExt.Click += new System.EventHandler(this.btnRegisterProjectExt_Click);
            // 
            // btnDeleteAssoc
            // 
            this.btnDeleteAssoc.Location = new System.Drawing.Point(170, 8);
            this.btnDeleteAssoc.Name = "btnDeleteAssoc";
            this.btnDeleteAssoc.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteAssoc.TabIndex = 2;
            this.btnDeleteAssoc.Text = "Delete";
            this.btnDeleteAssoc.UseVisualStyleBackColor = true;
            this.btnDeleteAssoc.Click += new System.EventHandler(this.btnDeleteAssoc_Click);
            // 
            // btnEditAssoc
            // 
            this.btnEditAssoc.Location = new System.Drawing.Point(89, 8);
            this.btnEditAssoc.Name = "btnEditAssoc";
            this.btnEditAssoc.Size = new System.Drawing.Size(75, 23);
            this.btnEditAssoc.TabIndex = 1;
            this.btnEditAssoc.Text = "Edit";
            this.btnEditAssoc.UseVisualStyleBackColor = true;
            this.btnEditAssoc.Click += new System.EventHandler(this.btnEditAssoc_Click);
            // 
            // btnAddAssoc
            // 
            this.btnAddAssoc.Location = new System.Drawing.Point(8, 8);
            this.btnAddAssoc.Name = "btnAddAssoc";
            this.btnAddAssoc.Size = new System.Drawing.Size(75, 23);
            this.btnAddAssoc.TabIndex = 0;
            this.btnAddAssoc.Text = "Add";
            this.btnAddAssoc.UseVisualStyleBackColor = true;
            this.btnAddAssoc.Click += new System.EventHandler(this.btnAddAssoc_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.pnlInfo);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 470);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(684, 41);
            this.pnlBottom.TabIndex = 1;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.pbInfo);
            this.pnlInfo.Controls.Add(this.lblInfo);
            this.pnlInfo.Location = new System.Drawing.Point(12, 7);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(360, 21);
            this.pnlInfo.TabIndex = 4;
            // 
            // pbInfo
            // 
            this.pbInfo.Image = global::Scada.Admin.App.Properties.Resources.info;
            this.pbInfo.Location = new System.Drawing.Point(0, 2);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(16, 16);
            this.pbInfo.TabIndex = 0;
            this.pbInfo.TabStop = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblInfo.Location = new System.Drawing.Point(22, 3);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(297, 15);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Changes will take effect after restarting the application.";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(597, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(516, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pageCnlNum
            // 
            this.pageCnlNum.Controls.Add(this.chkPrependDeviceName);
            this.pageCnlNum.Controls.Add(this.numGap);
            this.pageCnlNum.Controls.Add(this.lblGap);
            this.pageCnlNum.Controls.Add(this.numShift);
            this.pageCnlNum.Controls.Add(this.lblShift);
            this.pageCnlNum.Controls.Add(this.lblExplanation);
            this.pageCnlNum.Controls.Add(this.numMultiplicity);
            this.pageCnlNum.Controls.Add(this.lblMultiplicity);
            this.pageCnlNum.Location = new System.Drawing.Point(4, 24);
            this.pageCnlNum.Name = "pageCnlNum";
            this.pageCnlNum.Padding = new System.Windows.Forms.Padding(5);
            this.pageCnlNum.Size = new System.Drawing.Size(676, 442);
            this.pageCnlNum.TabIndex = 2;
            this.pageCnlNum.Text = "Channel Numbering";
            this.pageCnlNum.UseVisualStyleBackColor = true;
            // 
            // chkPrependDeviceName
            // 
            this.chkPrependDeviceName.AutoSize = true;
            this.chkPrependDeviceName.Location = new System.Drawing.Point(8, 96);
            this.chkPrependDeviceName.Name = "chkPrependDeviceName";
            this.chkPrependDeviceName.Size = new System.Drawing.Size(140, 19);
            this.chkPrependDeviceName.TabIndex = 7;
            this.chkPrependDeviceName.Text = "Prepend device name";
            this.chkPrependDeviceName.UseVisualStyleBackColor = true;
            // 
            // numGap
            // 
            this.numGap.Location = new System.Drawing.Point(8, 67);
            this.numGap.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numGap.Name = "numGap";
            this.numGap.Size = new System.Drawing.Size(150, 23);
            this.numGap.TabIndex = 6;
            this.numGap.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblGap
            // 
            this.lblGap.AutoSize = true;
            this.lblGap.Location = new System.Drawing.Point(5, 49);
            this.lblGap.Name = "lblGap";
            this.lblGap.Size = new System.Drawing.Size(28, 15);
            this.lblGap.TabIndex = 5;
            this.lblGap.Text = "Gap";
            // 
            // numShift
            // 
            this.numShift.Location = new System.Drawing.Point(218, 23);
            this.numShift.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numShift.Name = "numShift";
            this.numShift.Size = new System.Drawing.Size(150, 23);
            this.numShift.TabIndex = 4;
            this.numShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblShift
            // 
            this.lblShift.AutoSize = true;
            this.lblShift.Location = new System.Drawing.Point(215, 5);
            this.lblShift.Name = "lblShift";
            this.lblShift.Size = new System.Drawing.Size(31, 15);
            this.lblShift.TabIndex = 3;
            this.lblShift.Text = "Shift";
            // 
            // lblExplanation
            // 
            this.lblExplanation.AutoSize = true;
            this.lblExplanation.Location = new System.Drawing.Point(164, 27);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(38, 15);
            this.lblExplanation.TabIndex = 2;
            this.lblExplanation.Text = "× N +";
            // 
            // numMultiplicity
            // 
            this.numMultiplicity.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMultiplicity.Location = new System.Drawing.Point(8, 23);
            this.numMultiplicity.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMultiplicity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMultiplicity.Name = "numMultiplicity";
            this.numMultiplicity.Size = new System.Drawing.Size(150, 23);
            this.numMultiplicity.TabIndex = 1;
            this.numMultiplicity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lblMultiplicity
            // 
            this.lblMultiplicity.AutoSize = true;
            this.lblMultiplicity.Location = new System.Drawing.Point(5, 5);
            this.lblMultiplicity.Name = "lblMultiplicity";
            this.lblMultiplicity.Size = new System.Drawing.Size(67, 15);
            this.lblMultiplicity.TabIndex = 0;
            this.lblMultiplicity.Text = "Multiplicity";
            // 
            // FrmConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(684, 511);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.tabControl.ResumeLayout(false);
            this.pageExt.ResumeLayout(false);
            this.pageExt.PerformLayout();
            this.pageFileAssoc.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.pageCnlNum.ResumeLayout(false);
            this.pageCnlNum.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMultiplicity)).EndInit();
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
        private System.Windows.Forms.Button btnRegisterProjectExt;
        private System.Windows.Forms.ListView lvAssoc;
        private System.Windows.Forms.ColumnHeader colExt;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TabPage pageCnlNum;
        private System.Windows.Forms.CheckBox chkPrependDeviceName;
        private System.Windows.Forms.NumericUpDown numGap;
        private System.Windows.Forms.Label lblGap;
        private System.Windows.Forms.NumericUpDown numShift;
        private System.Windows.Forms.Label lblShift;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.NumericUpDown numMultiplicity;
        private System.Windows.Forms.Label lblMultiplicity;
    }
}