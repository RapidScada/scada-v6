
namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    partial class FrmArchives
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lvArchive = new System.Windows.Forms.ListView();
            this.colOrder = new System.Windows.Forms.ColumnHeader();
            this.colActive = new System.Windows.Forms.ColumnHeader();
            this.colCode = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colKind = new System.Windows.Forms.ColumnHeader();
            this.colModule = new System.Windows.Forms.ColumnHeader();
            this.gbArchive = new System.Windows.Forms.GroupBox();
            this.btnProperties = new System.Windows.Forms.Button();
            this.txtOptions = new System.Windows.Forms.TextBox();
            this.lblOptions = new System.Windows.Forms.Label();
            this.cbModule = new System.Windows.Forms.ComboBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.cbKind = new System.Windows.Forms.ComboBox();
            this.lblKind = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.btnCut = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.gbArchive.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(98, 12);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(80, 23);
            this.btnMoveUp.TabIndex = 1;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(184, 12);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(80, 23);
            this.btnMoveDown.TabIndex = 2;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(270, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lvArchive
            // 
            this.lvArchive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvArchive.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colOrder,
            this.colActive,
            this.colCode,
            this.colName,
            this.colKind,
            this.colModule});
            this.lvArchive.FullRowSelect = true;
            this.lvArchive.GridLines = true;
            this.lvArchive.HideSelection = false;
            this.lvArchive.Location = new System.Drawing.Point(12, 41);
            this.lvArchive.MultiSelect = false;
            this.lvArchive.Name = "lvArchive";
            this.lvArchive.ShowItemToolTips = true;
            this.lvArchive.Size = new System.Drawing.Size(710, 193);
            this.lvArchive.TabIndex = 7;
            this.lvArchive.UseCompatibleStateImageBehavior = false;
            this.lvArchive.View = System.Windows.Forms.View.Details;
            this.lvArchive.SelectedIndexChanged += new System.EventHandler(this.lvArchive_SelectedIndexChanged);
            this.lvArchive.DoubleClick += new System.EventHandler(this.lvArchive_DoubleClick);
            // 
            // colOrder
            // 
            this.colOrder.Text = "#";
            this.colOrder.Width = 50;
            // 
            // colActive
            // 
            this.colActive.Text = "Active";
            this.colActive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colCode
            // 
            this.colCode.Text = "Code";
            this.colCode.Width = 100;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 200;
            // 
            // colKind
            // 
            this.colKind.Text = "Kind";
            this.colKind.Width = 150;
            // 
            // colModule
            // 
            this.colModule.Text = "Module";
            this.colModule.Width = 150;
            // 
            // gbArchive
            // 
            this.gbArchive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbArchive.Controls.Add(this.btnProperties);
            this.gbArchive.Controls.Add(this.txtOptions);
            this.gbArchive.Controls.Add(this.lblOptions);
            this.gbArchive.Controls.Add(this.cbModule);
            this.gbArchive.Controls.Add(this.lblModule);
            this.gbArchive.Controls.Add(this.cbKind);
            this.gbArchive.Controls.Add(this.lblKind);
            this.gbArchive.Controls.Add(this.txtName);
            this.gbArchive.Controls.Add(this.lblName);
            this.gbArchive.Controls.Add(this.txtCode);
            this.gbArchive.Controls.Add(this.lblCode);
            this.gbArchive.Controls.Add(this.chkActive);
            this.gbArchive.Location = new System.Drawing.Point(12, 240);
            this.gbArchive.Name = "gbArchive";
            this.gbArchive.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbArchive.Size = new System.Drawing.Size(600, 259);
            this.gbArchive.TabIndex = 8;
            this.gbArchive.TabStop = false;
            this.gbArchive.Text = "Selected Archive";
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(13, 223);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(100, 23);
            this.btnProperties.TabIndex = 11;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // txtOptions
            // 
            this.txtOptions.Location = new System.Drawing.Point(387, 62);
            this.txtOptions.Multiline = true;
            this.txtOptions.Name = "txtOptions";
            this.txtOptions.ReadOnly = true;
            this.txtOptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOptions.Size = new System.Drawing.Size(200, 155);
            this.txtOptions.TabIndex = 10;
            this.txtOptions.WordWrap = false;
            // 
            // lblOptions
            // 
            this.lblOptions.AutoSize = true;
            this.lblOptions.Location = new System.Drawing.Point(384, 44);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(49, 15);
            this.lblOptions.TabIndex = 9;
            this.lblOptions.Text = "Options";
            // 
            // cbModule
            // 
            this.cbModule.FormattingEnabled = true;
            this.cbModule.Location = new System.Drawing.Point(13, 194);
            this.cbModule.Name = "cbModule";
            this.cbModule.Size = new System.Drawing.Size(368, 23);
            this.cbModule.TabIndex = 8;
            this.cbModule.TextChanged += new System.EventHandler(this.cbModule_TextChanged);
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(10, 176);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(48, 15);
            this.lblModule.TabIndex = 7;
            this.lblModule.Text = "Module";
            // 
            // cbKind
            // 
            this.cbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKind.FormattingEnabled = true;
            this.cbKind.Location = new System.Drawing.Point(13, 150);
            this.cbKind.Name = "cbKind";
            this.cbKind.Size = new System.Drawing.Size(368, 23);
            this.cbKind.TabIndex = 6;
            this.cbKind.SelectedIndexChanged += new System.EventHandler(this.cbKind_SelectedIndexChanged);
            // 
            // lblKind
            // 
            this.lblKind.AutoSize = true;
            this.lblKind.Location = new System.Drawing.Point(10, 132);
            this.lblKind.Name = "lblKind";
            this.lblKind.Size = new System.Drawing.Size(31, 15);
            this.lblKind.TabIndex = 5;
            this.lblKind.Text = "Kind";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(13, 106);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(368, 23);
            this.txtName.TabIndex = 4;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 88);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(13, 62);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(368, 23);
            this.txtCode.TabIndex = 2;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(10, 44);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(35, 15);
            this.lblCode.TabIndex = 1;
            this.lblCode.Text = "Code";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(13, 22);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(59, 19);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // btnCut
            // 
            this.btnCut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCut.Location = new System.Drawing.Point(470, 12);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(80, 23);
            this.btnCut.TabIndex = 4;
            this.btnCut.Text = "Cut";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(556, 12);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(80, 23);
            this.btnCopy.TabIndex = 5;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPaste.Location = new System.Drawing.Point(642, 12);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(80, 23);
            this.btnPaste.TabIndex = 6;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // FrmArchives
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 511);
            this.Controls.Add(this.gbArchive);
            this.Controls.Add(this.lvArchive);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnCut);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.btnAdd);
            this.Name = "FrmArchives";
            this.Text = "Archives";
            this.Load += new System.EventHandler(this.FrmArchives_Load);
            this.gbArchive.ResumeLayout(false);
            this.gbArchive.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListView lvArchive;
        private System.Windows.Forms.ColumnHeader colOrder;
        private System.Windows.Forms.ColumnHeader colActive;
        private System.Windows.Forms.ColumnHeader colCode;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colKind;
        private System.Windows.Forms.ColumnHeader colModule;
        private System.Windows.Forms.GroupBox gbArchive;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cbModule;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.ComboBox cbKind;
        private System.Windows.Forms.Label lblKind;
        private System.Windows.Forms.TextBox txtOptions;
        private System.Windows.Forms.Label lblOptions;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnCut;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnPaste;
    }
}