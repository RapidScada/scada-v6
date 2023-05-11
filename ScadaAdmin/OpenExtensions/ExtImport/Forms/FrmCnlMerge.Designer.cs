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
			label3 = new Label();
			checkBox2 = new CheckBox();
			checkBox1 = new CheckBox();
			checkBox3 = new CheckBox();
			dataGridView1 = new DataGridView();
			Column1Txt = new DataGridViewTextBoxColumn();
			ColumnChk = new DataGridViewCheckBoxColumn();
			Column2Txt = new DataGridViewTextBoxColumn();
			Column33 = new DataGridViewTextBoxColumn();
			Column4 = new DataGridViewTextBoxColumn();
			ColumnVide = new DataGridViewTextBoxColumn();
			Column2Chk = new DataGridViewCheckBoxColumn();
			Column2 = new DataGridViewTextBoxColumn();
			Column3 = new DataGridViewTextBoxColumn();
			Column44 = new DataGridViewTextBoxColumn();
			btnCancel = new Button();
			btnAdd = new Button();
			((ISupportInitialize)dataGridView1).BeginInit();
			SuspendLayout();
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(5, 20);
			label3.Name = "label3";
			label3.Size = new Size(0, 15);
			label3.TabIndex = 17;
			// 
			// checkBox2
			// 
			checkBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			checkBox2.AutoSize = true;
			checkBox2.Location = new Point(1056, 100);
			checkBox2.Margin = new Padding(3, 2, 3, 2);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new Size(145, 19);
			checkBox2.TabIndex = 20;
			checkBox2.Text = "Check all existing lines";
			checkBox2.UseVisualStyleBackColor = true;
			checkBox2.CheckedChanged += checkBox2_CheckedChanged;
			// 
			// checkBox1
			// 
			checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			checkBox1.AutoSize = true;
			checkBox1.Location = new Point(18, 100);
			checkBox1.Margin = new Padding(3, 2, 3, 2);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(153, 19);
			checkBox1.TabIndex = 19;
			checkBox1.Text = "Check all imported lines";
			checkBox1.UseVisualStyleBackColor = true;
			checkBox1.CheckedChanged += checkBox1_CheckedChanged;
			// 
			// checkBox3
			// 
			checkBox3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			checkBox3.AutoSize = true;
			checkBox3.Location = new Point(438, 100);
			checkBox3.Margin = new Padding(3, 2, 3, 2);
			checkBox3.Name = "checkBox3";
			checkBox3.Size = new Size(204, 19);
			checkBox3.TabIndex = 18;
			checkBox3.Text = " Preserve the existing descriptions";
			checkBox3.UseVisualStyleBackColor = true;
			checkBox3.CheckedChanged += checkBox3_CheckedChanged;
			// 
			// dataGridView1
			// 
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1Txt, ColumnChk, Column2Txt, Column33, Column4, ColumnVide, Column2Chk, Column2, Column3, Column44 });
			dataGridView1.Location = new Point(10, 122);
			dataGridView1.Margin = new Padding(3, 2, 3, 2);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.RowHeadersWidth = 180;
			dataGridView1.RowTemplate.Height = 29;
			dataGridView1.Size = new Size(1223, 280);
			dataGridView1.TabIndex = 21;
			dataGridView1.CellContentClick += dataGridView1_CellContentClick;
			dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
			// 
			// Column1Txt
			// 
			Column1Txt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column1Txt.HeaderText = "Adresse";
			Column1Txt.MinimumWidth = 6;
			Column1Txt.Name = "Column1Txt";
			// 
			// ColumnChk
			// 
			ColumnChk.HeaderText = "";
			ColumnChk.MinimumWidth = 6;
			ColumnChk.Name = "ColumnChk";
			ColumnChk.Resizable = DataGridViewTriState.True;
			ColumnChk.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// Column2Txt
			// 
			Column2Txt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column2Txt.HeaderText = "Mnémonique";
			Column2Txt.MinimumWidth = 6;
			Column2Txt.Name = "Column2Txt";
			// 
			// Column33
			// 
			Column33.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column33.HeaderText = "Type";
			Column33.MinimumWidth = 6;
			Column33.Name = "Column33";
			// 
			// Column4
			// 
			Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column4.HeaderText = "Commentaire";
			Column4.MinimumWidth = 6;
			Column4.Name = "Column4";
			// 
			// ColumnVide
			// 
			ColumnVide.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			ColumnVide.HeaderText = "";
			ColumnVide.MinimumWidth = 6;
			ColumnVide.Name = "ColumnVide";
			// 
			// Column2Chk
			// 
			Column2Chk.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column2Chk.HeaderText = "";
			Column2Chk.MinimumWidth = 6;
			Column2Chk.Name = "Column2Chk";
			Column2Chk.Resizable = DataGridViewTriState.True;
			Column2Chk.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// Column2
			// 
			Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column2.HeaderText = "Mnémonique";
			Column2.MinimumWidth = 6;
			Column2.Name = "Column2";
			// 
			// Column3
			// 
			Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column3.HeaderText = "Type";
			Column3.MinimumWidth = 6;
			Column3.Name = "Column3";
			// 
			// Column44
			// 
			Column44.HeaderText = "Commentaire";
			Column44.MinimumWidth = 6;
			Column44.Name = "Column44";
			// 
			// btnCancel
			// 
			btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnCancel.Location = new Point(1067, 443);
			btnCancel.Margin = new Padding(3, 2, 3, 2);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(72, 26);
			btnCancel.TabIndex = 23;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnAdd.Location = new Point(940, 443);
			btnAdd.Margin = new Padding(3, 2, 3, 2);
			btnAdd.Name = "btnAdd";
			btnAdd.Size = new Size(75, 26);
			btnAdd.TabIndex = 22;
			btnAdd.Text = "Add";
			btnAdd.UseVisualStyleBackColor = true;
			btnAdd.Click += btnAdd_Click;
			// 
			// FrmCnlMerge
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1246, 502);
			CancelButton = btnCancel;
			Controls.Add(btnCancel);
			Controls.Add(btnAdd);
			Controls.Add(dataGridView1);
			Controls.Add(checkBox2);
			Controls.Add(checkBox1);
			Controls.Add(checkBox3);
			Controls.Add(label3);
			MinimumSize = new Size(1262, 541);
			Name = "FrmCnlMerge";
			Text = "Merge";
			((ISupportInitialize)dataGridView1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Label label3;
		private CheckBox checkBox2;
		private CheckBox checkBox1;
		private CheckBox checkBox3;
		private DataGridView dataGridView1;
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
		private Button btnCancel;
		private Button btnAdd;
	}
}