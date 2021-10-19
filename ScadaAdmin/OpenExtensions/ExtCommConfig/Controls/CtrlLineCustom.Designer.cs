namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    partial class CtrlLineCustom
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbSelectedOption = new System.Windows.Forms.GroupBox();
            this.lblOptionValue = new System.Windows.Forms.Label();
            this.txtOptionValue = new System.Windows.Forms.TextBox();
            this.txtOptionName = new System.Windows.Forms.TextBox();
            this.lblOptionName = new System.Windows.Forms.Label();
            this.lvCustomOptions = new System.Windows.Forms.ListView();
            this.colOptionName = new System.Windows.Forms.ColumnHeader();
            this.colOptionValue = new System.Windows.Forms.ColumnHeader();
            this.btnAddOption = new System.Windows.Forms.Button();
            this.btnDeleteOption = new System.Windows.Forms.Button();
            this.gbSelectedOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSelectedOption
            // 
            this.gbSelectedOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSelectedOption.Controls.Add(this.lblOptionValue);
            this.gbSelectedOption.Controls.Add(this.txtOptionValue);
            this.gbSelectedOption.Controls.Add(this.txtOptionName);
            this.gbSelectedOption.Controls.Add(this.lblOptionName);
            this.gbSelectedOption.Location = new System.Drawing.Point(9, 365);
            this.gbSelectedOption.Margin = new System.Windows.Forms.Padding(9, 3, 3, 12);
            this.gbSelectedOption.Name = "gbSelectedOption";
            this.gbSelectedOption.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSelectedOption.Size = new System.Drawing.Size(500, 73);
            this.gbSelectedOption.TabIndex = 3;
            this.gbSelectedOption.TabStop = false;
            this.gbSelectedOption.Text = "Selected Option";
            // 
            // lblOptionValue
            // 
            this.lblOptionValue.AutoSize = true;
            this.lblOptionValue.Location = new System.Drawing.Point(186, 19);
            this.lblOptionValue.Name = "lblOptionValue";
            this.lblOptionValue.Size = new System.Drawing.Size(35, 15);
            this.lblOptionValue.TabIndex = 2;
            this.lblOptionValue.Text = "Value";
            // 
            // txtOptionValue
            // 
            this.txtOptionValue.Location = new System.Drawing.Point(189, 37);
            this.txtOptionValue.Name = "txtOptionValue";
            this.txtOptionValue.Size = new System.Drawing.Size(298, 23);
            this.txtOptionValue.TabIndex = 3;
            this.txtOptionValue.TextChanged += new System.EventHandler(this.txtOptionValue_TextChanged);
            // 
            // txtOptionName
            // 
            this.txtOptionName.Location = new System.Drawing.Point(13, 37);
            this.txtOptionName.Name = "txtOptionName";
            this.txtOptionName.Size = new System.Drawing.Size(170, 23);
            this.txtOptionName.TabIndex = 1;
            this.txtOptionName.TextChanged += new System.EventHandler(this.txtOptionName_TextChanged);
            // 
            // lblOptionName
            // 
            this.lblOptionName.AutoSize = true;
            this.lblOptionName.Location = new System.Drawing.Point(10, 19);
            this.lblOptionName.Name = "lblOptionName";
            this.lblOptionName.Size = new System.Drawing.Size(39, 15);
            this.lblOptionName.TabIndex = 0;
            this.lblOptionName.Text = "Name";
            // 
            // lvCustomOptions
            // 
            this.lvCustomOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCustomOptions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colOptionName,
            this.colOptionValue});
            this.lvCustomOptions.FullRowSelect = true;
            this.lvCustomOptions.GridLines = true;
            this.lvCustomOptions.HideSelection = false;
            this.lvCustomOptions.Location = new System.Drawing.Point(9, 41);
            this.lvCustomOptions.Margin = new System.Windows.Forms.Padding(9, 3, 12, 3);
            this.lvCustomOptions.MultiSelect = false;
            this.lvCustomOptions.Name = "lvCustomOptions";
            this.lvCustomOptions.ShowItemToolTips = true;
            this.lvCustomOptions.Size = new System.Drawing.Size(529, 318);
            this.lvCustomOptions.TabIndex = 2;
            this.lvCustomOptions.UseCompatibleStateImageBehavior = false;
            this.lvCustomOptions.View = System.Windows.Forms.View.Details;
            this.lvCustomOptions.SelectedIndexChanged += new System.EventHandler(this.lvCustomOptions_SelectedIndexChanged);
            // 
            // colOptionName
            // 
            this.colOptionName.Text = "Name";
            this.colOptionName.Width = 200;
            // 
            // colOptionValue
            // 
            this.colOptionValue.Text = "Value";
            this.colOptionValue.Width = 300;
            // 
            // btnAddOption
            // 
            this.btnAddOption.Location = new System.Drawing.Point(9, 12);
            this.btnAddOption.Margin = new System.Windows.Forms.Padding(9, 12, 3, 3);
            this.btnAddOption.Name = "btnAddOption";
            this.btnAddOption.Size = new System.Drawing.Size(80, 23);
            this.btnAddOption.TabIndex = 0;
            this.btnAddOption.Text = "Add";
            this.btnAddOption.UseVisualStyleBackColor = true;
            this.btnAddOption.Click += new System.EventHandler(this.btnAddOption_Click);
            // 
            // btnDeleteOption
            // 
            this.btnDeleteOption.Location = new System.Drawing.Point(95, 12);
            this.btnDeleteOption.Name = "btnDeleteOption";
            this.btnDeleteOption.Size = new System.Drawing.Size(80, 23);
            this.btnDeleteOption.TabIndex = 1;
            this.btnDeleteOption.Text = "Delete";
            this.btnDeleteOption.UseVisualStyleBackColor = true;
            this.btnDeleteOption.Click += new System.EventHandler(this.btnDeleteOption_Click);
            // 
            // CtrlLineCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDeleteOption);
            this.Controls.Add(this.btnAddOption);
            this.Controls.Add(this.gbSelectedOption);
            this.Controls.Add(this.lvCustomOptions);
            this.Name = "CtrlLineCustom";
            this.Size = new System.Drawing.Size(550, 450);
            this.Load += new System.EventHandler(this.CtrlLineCustomParams_Load);
            this.gbSelectedOption.ResumeLayout(false);
            this.gbSelectedOption.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSelectedOption;
        private System.Windows.Forms.Label lblOptionValue;
        private System.Windows.Forms.TextBox txtOptionValue;
        private System.Windows.Forms.TextBox txtOptionName;
        private System.Windows.Forms.Label lblOptionName;
        private System.Windows.Forms.ListView lvCustomOptions;
        private System.Windows.Forms.ColumnHeader colOptionName;
        private System.Windows.Forms.ColumnHeader colOptionValue;
        private System.Windows.Forms.Button btnAddOption;
        private System.Windows.Forms.Button btnDeleteOption;
    }
}
