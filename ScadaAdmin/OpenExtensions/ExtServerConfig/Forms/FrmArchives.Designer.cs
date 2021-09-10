
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
            this.btnAddArchive = new System.Windows.Forms.Button();
            this.btnMoveUpArchive = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnDeleteArchive = new System.Windows.Forms.Button();
            this.lvArchive = new System.Windows.Forms.ListView();
            this.colOrder = new System.Windows.Forms.ColumnHeader();
            this.colActive = new System.Windows.Forms.ColumnHeader();
            this.colCode = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colKind = new System.Windows.Forms.ColumnHeader();
            this.colModule = new System.Windows.Forms.ColumnHeader();
            this.gbArchive = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.gbArchive.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddArchive
            // 
            this.btnAddArchive.Location = new System.Drawing.Point(12, 12);
            this.btnAddArchive.Name = "btnAddArchive";
            this.btnAddArchive.Size = new System.Drawing.Size(75, 23);
            this.btnAddArchive.TabIndex = 0;
            this.btnAddArchive.Text = "Add";
            this.btnAddArchive.UseVisualStyleBackColor = true;
            // 
            // btnMoveUpArchive
            // 
            this.btnMoveUpArchive.Location = new System.Drawing.Point(93, 12);
            this.btnMoveUpArchive.Name = "btnMoveUpArchive";
            this.btnMoveUpArchive.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUpArchive.TabIndex = 1;
            this.btnMoveUpArchive.Text = "Move Up";
            this.btnMoveUpArchive.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(174, 12);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 2;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            // 
            // btnDeleteArchive
            // 
            this.btnDeleteArchive.Location = new System.Drawing.Point(255, 12);
            this.btnDeleteArchive.Name = "btnDeleteArchive";
            this.btnDeleteArchive.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteArchive.TabIndex = 3;
            this.btnDeleteArchive.Text = "Delete";
            this.btnDeleteArchive.UseVisualStyleBackColor = true;
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
            this.lvArchive.Size = new System.Drawing.Size(660, 278);
            this.lvArchive.TabIndex = 4;
            this.lvArchive.UseCompatibleStateImageBehavior = false;
            this.lvArchive.View = System.Windows.Forms.View.Details;
            // 
            // colOrder
            // 
            this.colOrder.Text = "#";
            this.colOrder.Width = 40;
            // 
            // colActive
            // 
            this.colActive.Text = "Active";
            this.colActive.Width = 50;
            // 
            // colCode
            // 
            this.colCode.Text = "Code";
            this.colCode.Width = 100;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 150;
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
            this.gbArchive.Controls.Add(this.button1);
            this.gbArchive.Controls.Add(this.textBox3);
            this.gbArchive.Controls.Add(this.label5);
            this.gbArchive.Controls.Add(this.comboBox2);
            this.gbArchive.Controls.Add(this.label4);
            this.gbArchive.Controls.Add(this.comboBox1);
            this.gbArchive.Controls.Add(this.label3);
            this.gbArchive.Controls.Add(this.textBox2);
            this.gbArchive.Controls.Add(this.label2);
            this.gbArchive.Controls.Add(this.textBox1);
            this.gbArchive.Controls.Add(this.label1);
            this.gbArchive.Controls.Add(this.checkBox1);
            this.gbArchive.Location = new System.Drawing.Point(12, 197);
            this.gbArchive.Name = "gbArchive";
            this.gbArchive.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbArchive.Size = new System.Drawing.Size(600, 344);
            this.gbArchive.TabIndex = 5;
            this.gbArchive.TabStop = false;
            this.gbArchive.Text = "Selected Archive";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 22);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(83, 19);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 62);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(119, 62);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(262, 23);
            this.textBox2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(13, 106);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(181, 23);
            this.comboBox1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "label4";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(200, 106);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(181, 23);
            this.comboBox2.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(384, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "label5";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(387, 62);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(200, 67);
            this.textBox3.TabIndex = 10;
            this.textBox3.WordWrap = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 135);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Properties";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FrmArchives
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 589);
            this.Controls.Add(this.gbArchive);
            this.Controls.Add(this.lvArchive);
            this.Controls.Add(this.btnDeleteArchive);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUpArchive);
            this.Controls.Add(this.btnAddArchive);
            this.Name = "FrmArchives";
            this.Text = "Archives";
            this.gbArchive.ResumeLayout(false);
            this.gbArchive.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddArchive;
        private System.Windows.Forms.Button btnMoveUpArchive;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnDeleteArchive;
        private System.Windows.Forms.ListView lvArchive;
        private System.Windows.Forms.ColumnHeader colOrder;
        private System.Windows.Forms.ColumnHeader colActive;
        private System.Windows.Forms.ColumnHeader colCode;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colKind;
        private System.Windows.Forms.ColumnHeader colModule;
        private System.Windows.Forms.GroupBox gbArchive;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}