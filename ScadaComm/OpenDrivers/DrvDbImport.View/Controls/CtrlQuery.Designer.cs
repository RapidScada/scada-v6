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
            this.components = new System.ComponentModel.Container();
            this.gbQuery = new System.Windows.Forms.GroupBox();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.pbSqlInfo = new System.Windows.Forms.PictureBox();
            this.chkSingleRow = new System.Windows.Forms.CheckBox();
            this.lblSql = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.pbTagInfo = new System.Windows.Forms.PictureBox();
            this.lblTag = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSqlInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTagInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // gbQuery
            // 
            this.gbQuery.Controls.Add(this.txtSql);
            this.gbQuery.Controls.Add(this.pbSqlInfo);
            this.gbQuery.Controls.Add(this.chkSingleRow);
            this.gbQuery.Controls.Add(this.lblSql);
            this.gbQuery.Controls.Add(this.txtTags);
            this.gbQuery.Controls.Add(this.pbTagInfo);
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
            this.gbQuery.Text = "Query Parameters";
            // 
            // txtSql
            // 
            this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSql.Location = new System.Drawing.Point(13, 243);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.Size = new System.Drawing.Size(378, 206);
            this.txtSql.TabIndex = 7;
            this.txtSql.WordWrap = false;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // pbSqlInfo
            // 
            this.pbSqlInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSqlInfo.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.info;
            this.pbSqlInfo.Location = new System.Drawing.Point(375, 219);
            this.pbSqlInfo.Name = "pbSqlInfo";
            this.pbSqlInfo.Size = new System.Drawing.Size(16, 16);
            this.pbSqlInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbSqlInfo.TabIndex = 12;
            this.pbSqlInfo.TabStop = false;
            // 
            // chkSingleRow
            // 
            this.chkSingleRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSingleRow.AutoSize = true;
            this.chkSingleRow.Location = new System.Drawing.Point(256, 218);
            this.chkSingleRow.Name = "chkSingleRow";
            this.chkSingleRow.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkSingleRow.Size = new System.Drawing.Size(113, 19);
            this.chkSingleRow.TabIndex = 6;
            this.chkSingleRow.Text = "Single row result";
            this.chkSingleRow.UseVisualStyleBackColor = true;
            this.chkSingleRow.CheckedChanged += new System.EventHandler(this.chkSingleRow_CheckedChanged);
            // 
            // lblSql
            // 
            this.lblSql.AutoSize = true;
            this.lblSql.Location = new System.Drawing.Point(10, 220);
            this.lblSql.Margin = new System.Windows.Forms.Padding(3);
            this.lblSql.Name = "lblSql";
            this.lblSql.Size = new System.Drawing.Size(28, 15);
            this.lblSql.TabIndex = 5;
            this.lblSql.Text = "SQL";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(13, 112);
            this.txtTags.Multiline = true;
            this.txtTags.Name = "txtTags";
            this.txtTags.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTags.Size = new System.Drawing.Size(378, 100);
            this.txtTags.TabIndex = 4;
            this.txtTags.TextChanged += new System.EventHandler(this.txtTags_TextChanged);
            // 
            // pbTagInfo
            // 
            this.pbTagInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTagInfo.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.info;
            this.pbTagInfo.Location = new System.Drawing.Point(375, 91);
            this.pbTagInfo.Name = "pbTagInfo";
            this.pbTagInfo.Size = new System.Drawing.Size(16, 16);
            this.pbTagInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTagInfo.TabIndex = 13;
            this.pbTagInfo.TabStop = false;
            // 
            // lblTag
            // 
            this.lblTag.AutoSize = true;
            this.lblTag.Location = new System.Drawing.Point(10, 91);
            this.lblTag.Margin = new System.Windows.Forms.Padding(3);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(30, 15);
            this.lblTag.TabIndex = 3;
            this.lblTag.Text = "Tags";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(13, 62);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(378, 23);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
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
            ((System.ComponentModel.ISupportInitialize)(this.pbSqlInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTagInfo)).EndInit();
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
        private PictureBox pbSqlInfo;
        private Label lblSql;
        private ToolTip toolTip;
        private PictureBox pbTagInfo;
    }
}
