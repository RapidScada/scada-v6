namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View.Controls
{
    partial class CtrlElemGroup
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
            gbElemGroup = new GroupBox();
            txtGrCode = new TextBox();
            lblCode = new Label();
            gbQuery = new GroupBox();
            txtQuerySql = new TextBox();
            btnViewParameters = new Button();
            chkGrActive = new CheckBox();
            lblGrElemCnt = new Label();
            numGrElemCnt = new NumericUpDown();
            txtGrName = new TextBox();
            lblGrName = new Label();
            txtProjectId = new TextBox();
            lblProjectId = new Label();
            gbElemGroup.SuspendLayout();
            gbQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numGrElemCnt).BeginInit();
            SuspendLayout();
            // 
            // gbElemGroup
            // 
            gbElemGroup.Controls.Add(txtGrCode);
            gbElemGroup.Controls.Add(lblCode);
            gbElemGroup.Controls.Add(gbQuery);
            gbElemGroup.Controls.Add(chkGrActive);
            gbElemGroup.Controls.Add(lblGrElemCnt);
            gbElemGroup.Controls.Add(numGrElemCnt);
            gbElemGroup.Controls.Add(txtGrName);
            gbElemGroup.Controls.Add(lblGrName);
            gbElemGroup.Location = new Point(0, 0);
            gbElemGroup.Name = "gbElemGroup";
            gbElemGroup.Padding = new Padding(10, 3, 10, 11);
            gbElemGroup.Size = new Size(300, 406);
            gbElemGroup.TabIndex = 0;
            gbElemGroup.TabStop = false;
            gbElemGroup.Text = "Element Group Parameters";
            // 
            // txtGrCode
            // 
            txtGrCode.Location = new Point(156, 70);
            txtGrCode.Name = "txtGrCode";
            txtGrCode.Size = new Size(131, 23);
            txtGrCode.TabIndex = 17;
            txtGrCode.TextChanged += txtGrCode_TextChanged;
            // 
            // lblCode
            // 
            lblCode.AutoSize = true;
            lblCode.Location = new Point(153, 50);
            lblCode.Name = "lblCode";
            lblCode.Size = new Size(39, 17);
            lblCode.TabIndex = 16;
            lblCode.Text = "Code";
            // 
            // gbQuery
            // 
            gbQuery.Controls.Add(lblProjectId);
            gbQuery.Controls.Add(txtProjectId);
            gbQuery.Controls.Add(txtQuerySql);
            gbQuery.Controls.Add(btnViewParameters);
            gbQuery.Location = new Point(3, 149);
            gbQuery.Name = "gbQuery";
            gbQuery.Size = new Size(297, 243);
            gbQuery.TabIndex = 15;
            gbQuery.TabStop = false;
            gbQuery.Text = "Query";
            // 
            // txtQuerySql
            // 
            txtQuerySql.Location = new Point(6, 52);
            txtQuerySql.Multiline = true;
            txtQuerySql.Name = "txtQuerySql";
            txtQuerySql.Size = new Size(285, 185);
            txtQuerySql.TabIndex = 13;
            txtQuerySql.TextChanged += txtQuerySql_TextChanged;
            // 
            // btnViewParameters
            // 
            btnViewParameters.FlatStyle = FlatStyle.Popup;
            btnViewParameters.Image = Properties.Resources.parameters;
            btnViewParameters.Location = new Point(264, 19);
            btnViewParameters.Name = "btnViewParameters";
            btnViewParameters.Size = new Size(23, 27);
            btnViewParameters.TabIndex = 14;
            btnViewParameters.UseVisualStyleBackColor = true;
            btnViewParameters.Click += btnViewParameters_Click;
            // 
            // chkGrActive
            // 
            chkGrActive.AutoSize = true;
            chkGrActive.Location = new Point(13, 25);
            chkGrActive.Name = "chkGrActive";
            chkGrActive.Size = new Size(61, 21);
            chkGrActive.TabIndex = 0;
            chkGrActive.Text = "Active";
            chkGrActive.UseVisualStyleBackColor = true;
            chkGrActive.CheckedChanged += chkGrActive_CheckedChanged;
            // 
            // lblGrElemCnt
            // 
            lblGrElemCnt.AutoSize = true;
            lblGrElemCnt.Location = new Point(10, 96);
            lblGrElemCnt.Name = "lblGrElemCnt";
            lblGrElemCnt.Size = new Size(90, 17);
            lblGrElemCnt.TabIndex = 10;
            lblGrElemCnt.Text = "Element count";
            // 
            // numGrElemCnt
            // 
            numGrElemCnt.Location = new Point(13, 117);
            numGrElemCnt.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            numGrElemCnt.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numGrElemCnt.Name = "numGrElemCnt";
            numGrElemCnt.Size = new Size(137, 23);
            numGrElemCnt.TabIndex = 11;
            numGrElemCnt.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numGrElemCnt.ValueChanged += numGrElemCnt_ValueChanged;
            // 
            // txtGrName
            // 
            txtGrName.Location = new Point(13, 70);
            txtGrName.Name = "txtGrName";
            txtGrName.Size = new Size(137, 23);
            txtGrName.TabIndex = 2;
            txtGrName.TextChanged += txtGrName_TextChanged;
            // 
            // lblGrName
            // 
            lblGrName.AutoSize = true;
            lblGrName.Location = new Point(10, 50);
            lblGrName.Name = "lblGrName";
            lblGrName.Size = new Size(43, 17);
            lblGrName.TabIndex = 1;
            lblGrName.Text = "Name";
            // 
            // txtProjectId
            // 
            txtProjectId.Location = new Point(73, 23);
            txtProjectId.Name = "txtProjectId";
            txtProjectId.Size = new Size(185, 23);
            txtProjectId.TabIndex = 18;
            txtProjectId.TextChanged += txtProjectId_TextChanged;
            // 
            // lblProjectId
            // 
            lblProjectId.AutoSize = true;
            lblProjectId.Location = new Point(7, 27);
            lblProjectId.Name = "lblProjectId";
            lblProjectId.Size = new Size(60, 17);
            lblProjectId.TabIndex = 18;
            lblProjectId.Text = "ProjectId";
            // 
            // CtrlElemGroup
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbElemGroup);
            Name = "CtrlElemGroup";
            Size = new Size(300, 409);
            gbElemGroup.ResumeLayout(false);
            gbElemGroup.PerformLayout();
            gbQuery.ResumeLayout(false);
            gbQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numGrElemCnt).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gbElemGroup;
        private System.Windows.Forms.CheckBox chkGrActive;
        private System.Windows.Forms.Label lblGrElemCnt;
        private System.Windows.Forms.NumericUpDown numGrElemCnt;
        private System.Windows.Forms.TextBox txtGrName;
        private System.Windows.Forms.Label lblGrName;
        private TextBox txtQuerySql;
        private GroupBox gbQuery;
        private Button btnViewParameters;
        private TextBox txtGrCode;
        private Label lblCode;
        private Label lblProjectId;
        private TextBox txtProjectId;
    }
}
