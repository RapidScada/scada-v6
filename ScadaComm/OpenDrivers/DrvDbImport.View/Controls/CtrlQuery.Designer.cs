namespace Scada.Comm.Drivers.DrvDbImport.View.Controls
{
    partial class CtrlQuery
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
            this.gbQuery = new System.Windows.Forms.GroupBox();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.chkSingleRow = new System.Windows.Forms.CheckBox();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.lblTag = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.gbQuery.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbQuery
            // 
            this.gbQuery.Controls.Add(this.txtSql);
            this.gbQuery.Controls.Add(this.chkSingleRow);
            this.gbQuery.Controls.Add(this.txtTags);
            this.gbQuery.Controls.Add(this.lblTag);
            this.gbQuery.Controls.Add(this.txtName);
            this.gbQuery.Controls.Add(this.lblName);
            this.gbQuery.Controls.Add(this.chkActive);
            this.gbQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbQuery.Location = new System.Drawing.Point(0, 0);
            this.gbQuery.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbQuery.Name = "gbQuery";
            this.gbQuery.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbQuery.Size = new System.Drawing.Size(404, 462);
            this.gbQuery.TabIndex = 0;
            this.gbQuery.TabStop = false;
            this.gbQuery.Text = "Query";
            // 
            // txtSql
            // 
            this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSql.Location = new System.Drawing.Point(13, 252);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.Size = new System.Drawing.Size(378, 197);
            this.txtSql.TabIndex = 6;
            this.txtSql.WordWrap = false;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // chkSingleRow
            // 
            this.chkSingleRow.AutoSize = true;
            this.chkSingleRow.Location = new System.Drawing.Point(13, 227);
            this.chkSingleRow.Name = "chkSingleRow";
            this.chkSingleRow.Size = new System.Drawing.Size(81, 19);
            this.chkSingleRow.TabIndex = 5;
            this.chkSingleRow.Text = "Single row";
            this.chkSingleRow.UseVisualStyleBackColor = true;
            this.chkSingleRow.CheckedChanged += new System.EventHandler(this.chkSingleRow_CheckedChanged);
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(13, 120);
            this.txtTags.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtTags.Multiline = true;
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(378, 94);
            this.txtTags.TabIndex = 4;
            this.txtTags.TextChanged += new System.EventHandler(this.txtTags_TextChanged);
            // 
            // lblTag
            // 
            this.lblTag.AutoSize = true;
            this.lblTag.Location = new System.Drawing.Point(10, 102);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(30, 15);
            this.lblTag.TabIndex = 3;
            this.lblTag.Text = "Tags";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(13, 69);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(378, 23);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 51);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(13, 22);
            this.chkActive.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(59, 19);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // CtrlQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbQuery);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.Name = "CtrlQuery";
            this.Size = new System.Drawing.Size(404, 462);
            this.gbQuery.ResumeLayout(false);
            this.gbQuery.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbQuery;
        private CheckBox chkActive;
        private TextBox txtName;
        private Label lblName;
        private Label lblTag;
        private TextBox txtTags;
        private CheckBox chkSingleRow;
        private TextBox txtSql;
    }
}
