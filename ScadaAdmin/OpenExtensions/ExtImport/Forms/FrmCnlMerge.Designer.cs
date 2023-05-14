using System.ComponentModel;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
	partial class FrmCnlMerge
	{
		//private DataGridView dataGridView;

		/// <summary> 
		/// 
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 
		/// </summary>
		/// <param name="disposing">
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur de composants

		
		private void InitializeComponent()
		{
      this.label3 = new System.Windows.Forms.Label();
      this.checkBox3 = new System.Windows.Forms.CheckBox();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.Column1Txt = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.Column2Txt = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnVide = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column2Chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.lblSource = new System.Windows.Forms.Label();
      this.lblDestination = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.SuspendLayout();
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(5, 20);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(0, 15);
      this.label3.TabIndex = 17;
      // 
      // checkBox3
      // 
      this.checkBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBox3.AutoSize = true;
      this.checkBox3.Location = new System.Drawing.Point(10, 465);
      this.checkBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.checkBox3.Name = "checkBox3";
      this.checkBox3.Size = new System.Drawing.Size(204, 19);
      this.checkBox3.TabIndex = 18;
      this.checkBox3.Text = " Preserve the existing descriptions";
      this.checkBox3.UseVisualStyleBackColor = true;
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1Txt,
            this.ColumnChk,
            this.Column2Txt,
            this.Column33,
            this.Column4,
            this.ColumnVide,
            this.Column2Chk,
            this.Column2,
            this.Column3,
            this.Column44});
      this.dataGridView1.Location = new System.Drawing.Point(10, 28);
      this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.RowHeadersWidth = 180;
      this.dataGridView1.RowTemplate.Height = 29;
      this.dataGridView1.Size = new System.Drawing.Size(1223, 433);
      this.dataGridView1.TabIndex = 21;
      // 
      // Column1Txt
      // 
      this.Column1Txt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Column1Txt.HeaderText = "Adresse";
      this.Column1Txt.MinimumWidth = 6;
      this.Column1Txt.Name = "Column1Txt";
      // 
      // ColumnChk
      // 
      this.ColumnChk.HeaderText = "";
      this.ColumnChk.MinimumWidth = 6;
      this.ColumnChk.Name = "ColumnChk";
      this.ColumnChk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.ColumnChk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      // 
      // Column2Txt
      // 
      this.Column2Txt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Column2Txt.HeaderText = "Mnémonique";
      this.Column2Txt.MinimumWidth = 6;
      this.Column2Txt.Name = "Column2Txt";
      // 
      // Column33
      // 
      this.Column33.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Column33.HeaderText = "Type";
      this.Column33.MinimumWidth = 6;
      this.Column33.Name = "Column33";
      // 
      // Column4
      // 
      this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Column4.HeaderText = "Commentaire";
      this.Column4.MinimumWidth = 6;
      this.Column4.Name = "Column4";
      // 
      // ColumnVide
      // 
      this.ColumnVide.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.ColumnVide.HeaderText = "";
      this.ColumnVide.MinimumWidth = 6;
      this.ColumnVide.Name = "ColumnVide";
      // 
      // Column2Chk
      // 
      this.Column2Chk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Column2Chk.HeaderText = "";
      this.Column2Chk.MinimumWidth = 6;
      this.Column2Chk.Name = "Column2Chk";
      this.Column2Chk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.Column2Chk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      // 
      // Column2
      // 
      this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Column2.HeaderText = "Mnémonique";
      this.Column2.MinimumWidth = 6;
      this.Column2.Name = "Column2";
      // 
      // Column3
      // 
      this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Column3.HeaderText = "Type";
      this.Column3.MinimumWidth = 6;
      this.Column3.Name = "Column3";
      // 
      // Column44
      // 
      this.Column44.HeaderText = "Commentaire";
      this.Column44.MinimumWidth = 6;
      this.Column44.Name = "Column44";
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.Location = new System.Drawing.Point(1162, 465);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(72, 26);
      this.btnCancel.TabIndex = 23;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnAdd
      // 
      this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAdd.Location = new System.Drawing.Point(1081, 465);
      this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(75, 26);
      this.btnAdd.TabIndex = 22;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      // 
      // lblSource
      // 
      this.lblSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lblSource.Location = new System.Drawing.Point(10, 8);
      this.lblSource.Name = "lblSource";
      this.lblSource.Padding = new System.Windows.Forms.Padding(2);
      this.lblSource.Size = new System.Drawing.Size(49, 21);
      this.lblSource.TabIndex = 24;
      this.lblSource.Text = "Source";
      this.lblSource.Click += new System.EventHandler(this.lblSource_Click);
      // 
      // lblDestination
      // 
      this.lblDestination.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lblDestination.Location = new System.Drawing.Point(606, 8);
      this.lblDestination.Name = "lblDestination";
      this.lblDestination.Padding = new System.Windows.Forms.Padding(2);
      this.lblDestination.Size = new System.Drawing.Size(73, 21);
      this.lblDestination.TabIndex = 25;
      this.lblDestination.Text = "Destination";
      // 
      // FrmCnlMerge
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(1246, 502);
      this.Controls.Add(this.lblDestination);
      this.Controls.Add(this.lblSource);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.dataGridView1);
      this.Controls.Add(this.checkBox3);
      this.Controls.Add(this.label3);
      this.MinimumSize = new System.Drawing.Size(1262, 541);
      this.Name = "FrmCnlMerge";
      this.Text = "Merge";
      this.Load += new System.EventHandler(this.FrmCnlMerge_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion
		private Label label3;
		private CheckBox checkBox3;
		private DataGridView dataGridView1;
		private Button btnCancel;
		private Button btnAdd;
    private DataGridViewTextBoxColumn Column1Txt;
    private DataGridViewCheckBoxColumn ColumnChk;
    private DataGridViewTextBoxColumn Column2Txt;
    private DataGridViewTextBoxColumn Column33;
    private DataGridViewTextBoxColumn Column4;
    private DataGridViewTextBoxColumn ColumnVide;
    private DataGridViewCheckBoxColumn Column2Chk;
    private DataGridViewTextBoxColumn Column2;
    private DataGridViewTextBoxColumn Column3;
    private DataGridViewTextBoxColumn Column44;
    private Label lblSource;
    private Label lblDestination;
  }
}