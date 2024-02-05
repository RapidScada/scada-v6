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
            gbSelectedOption = new System.Windows.Forms.GroupBox();
            txtOptionValue = new System.Windows.Forms.TextBox();
            lblOptionValue = new System.Windows.Forms.Label();
            txtOptionName = new System.Windows.Forms.TextBox();
            lblOptionName = new System.Windows.Forms.Label();
            lvCustomOptions = new System.Windows.Forms.ListView();
            colOptionName = new System.Windows.Forms.ColumnHeader();
            colOptionValue = new System.Windows.Forms.ColumnHeader();
            btnAddOption = new System.Windows.Forms.Button();
            btnDeleteOption = new System.Windows.Forms.Button();
            pnlTop = new System.Windows.Forms.Panel();
            pnlBottom = new System.Windows.Forms.Panel();
            gbSelectedOption.SuspendLayout();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // gbSelectedOption
            // 
            gbSelectedOption.Controls.Add(txtOptionValue);
            gbSelectedOption.Controls.Add(lblOptionValue);
            gbSelectedOption.Controls.Add(txtOptionName);
            gbSelectedOption.Controls.Add(lblOptionName);
            gbSelectedOption.Location = new System.Drawing.Point(0, 3);
            gbSelectedOption.Name = "gbSelectedOption";
            gbSelectedOption.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            gbSelectedOption.Size = new System.Drawing.Size(500, 73);
            gbSelectedOption.TabIndex = 0;
            gbSelectedOption.TabStop = false;
            gbSelectedOption.Text = "Selected Option";
            // 
            // txtOptionValue
            // 
            txtOptionValue.Location = new System.Drawing.Point(189, 37);
            txtOptionValue.Name = "txtOptionValue";
            txtOptionValue.Size = new System.Drawing.Size(298, 23);
            txtOptionValue.TabIndex = 3;
            txtOptionValue.TextChanged += txtOptionValue_TextChanged;
            // 
            // lblOptionValue
            // 
            lblOptionValue.AutoSize = true;
            lblOptionValue.Location = new System.Drawing.Point(186, 19);
            lblOptionValue.Name = "lblOptionValue";
            lblOptionValue.Size = new System.Drawing.Size(35, 15);
            lblOptionValue.TabIndex = 2;
            lblOptionValue.Text = "Value";
            // 
            // txtOptionName
            // 
            txtOptionName.Location = new System.Drawing.Point(13, 37);
            txtOptionName.Name = "txtOptionName";
            txtOptionName.Size = new System.Drawing.Size(170, 23);
            txtOptionName.TabIndex = 1;
            txtOptionName.TextChanged += txtOptionName_TextChanged;
            // 
            // lblOptionName
            // 
            lblOptionName.AutoSize = true;
            lblOptionName.Location = new System.Drawing.Point(10, 19);
            lblOptionName.Name = "lblOptionName";
            lblOptionName.Size = new System.Drawing.Size(39, 15);
            lblOptionName.TabIndex = 0;
            lblOptionName.Text = "Name";
            // 
            // lvCustomOptions
            // 
            lvCustomOptions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colOptionName, colOptionValue });
            lvCustomOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            lvCustomOptions.FullRowSelect = true;
            lvCustomOptions.GridLines = true;
            lvCustomOptions.Location = new System.Drawing.Point(0, 29);
            lvCustomOptions.MultiSelect = false;
            lvCustomOptions.Name = "lvCustomOptions";
            lvCustomOptions.ShowItemToolTips = true;
            lvCustomOptions.Size = new System.Drawing.Size(550, 345);
            lvCustomOptions.TabIndex = 1;
            lvCustomOptions.UseCompatibleStateImageBehavior = false;
            lvCustomOptions.View = System.Windows.Forms.View.Details;
            lvCustomOptions.SelectedIndexChanged += lvCustomOptions_SelectedIndexChanged;
            // 
            // colOptionName
            // 
            colOptionName.Text = "Name";
            colOptionName.Width = 200;
            // 
            // colOptionValue
            // 
            colOptionValue.Text = "Value";
            colOptionValue.Width = 300;
            // 
            // btnAddOption
            // 
            btnAddOption.Location = new System.Drawing.Point(0, 0);
            btnAddOption.Name = "btnAddOption";
            btnAddOption.Size = new System.Drawing.Size(80, 23);
            btnAddOption.TabIndex = 0;
            btnAddOption.Text = "Add";
            btnAddOption.UseVisualStyleBackColor = true;
            btnAddOption.Click += btnAddOption_Click;
            // 
            // btnDeleteOption
            // 
            btnDeleteOption.Location = new System.Drawing.Point(86, 0);
            btnDeleteOption.Name = "btnDeleteOption";
            btnDeleteOption.Size = new System.Drawing.Size(80, 23);
            btnDeleteOption.TabIndex = 1;
            btnDeleteOption.Text = "Delete";
            btnDeleteOption.UseVisualStyleBackColor = true;
            btnDeleteOption.Click += btnDeleteOption_Click;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(btnDeleteOption);
            pnlTop.Controls.Add(btnAddOption);
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Location = new System.Drawing.Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new System.Drawing.Size(550, 29);
            pnlTop.TabIndex = 0;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(gbSelectedOption);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 374);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(550, 76);
            pnlBottom.TabIndex = 2;
            // 
            // CtrlLineCustom
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lvCustomOptions);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Name = "CtrlLineCustom";
            Size = new System.Drawing.Size(550, 450);
            Load += CtrlLineCustomParams_Load;
            gbSelectedOption.ResumeLayout(false);
            gbSelectedOption.PerformLayout();
            pnlTop.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
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
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
    }
}
