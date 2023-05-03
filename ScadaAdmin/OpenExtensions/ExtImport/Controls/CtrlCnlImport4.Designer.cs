using System.ComponentModel;
using System.Windows.Forms;


namespace Scada.Admin.Extensions.ExtImport.Controls
{
	partial class CtrlCnlImport4
	{
		private DataGridView dataGridView;

		/// <summary> 
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Nettoyage des ressources utilisées.
		/// </summary>
		/// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur de composants

		/// <summary> 
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
			label1 = new Label();
			checkBox1 = new CheckBox();
			checkBox2 = new CheckBox();
			checkBox3 = new CheckBox();
			label2 = new Label();
			label3 = new Label();
			dataGridView1 = new DataGridView();
			ColumnChk = new DataGridViewCheckBoxColumn();
			Column1Txt = new DataGridViewTextBoxColumn();
			ColumnVide = new DataGridViewTextBoxColumn();
			Column2Chk = new DataGridViewCheckBoxColumn();
			Column2Txt = new DataGridViewTextBoxColumn();
			((ISupportInitialize)dataGridView1).BeginInit();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(711, 48);
			label1.Name = "label1";
			label1.Size = new Size(0, 20);
			label1.TabIndex = 0;
			//label1.Click += label1_Click;
			// 
			// checkBox1
			// 
			checkBox1.AutoSize = true;
			checkBox1.Location = new Point(128, 493);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(112, 24);
			checkBox1.TabIndex = 9;
			checkBox1.Text = "Tout cocher ";
			checkBox1.UseVisualStyleBackColor = true;
			checkBox1.CheckedChanged += checkBox1_CheckedChanged;
			// 
			// checkBox2
			// 
			checkBox2.AutoSize = true;
			checkBox2.Location = new Point(699, 493);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new Size(108, 24);
			checkBox2.TabIndex = 10;
			checkBox2.Text = "Tout cocher";
			checkBox2.UseVisualStyleBackColor = true;
			checkBox2.CheckedChanged += checkBox2_CheckedChanged;
			// 
			// checkBox3
			// 
			checkBox3.AutoSize = true;
			checkBox3.Location = new Point(296, 136);
			checkBox3.Name = "checkBox3";
			checkBox3.Size = new Size(300, 24);
			checkBox3.TabIndex = 11;
			checkBox3.Text = "préserver les descriptions déjà existantes";
			checkBox3.UseVisualStyleBackColor = true;
			checkBox3.CheckedChanged += checkBox3_CheckedChanged;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(13, 19);
			label2.Name = "label2";
			label2.Size = new Size(480, 20);
			label2.TabIndex = 13;
			label2.Text = "Vous n'avez pas de conflit avec les variables que vous venez d'importer.";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(13, 48);
			label3.Name = "label3";
			label3.Size = new Size(220, 20);
			label3.TabIndex = 14;
			label3.Text = "Vous pouvez passer cette étape.";
			// 
			// dataGridView1
			// 
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColumnChk, Column1Txt, ColumnVide, Column2Chk, Column2Txt });
			dataGridView1.Location = new Point(128, 179);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.RowHeadersWidth = 51;
			dataGridView1.RowTemplate.Height = 29;
			dataGridView1.Size = new Size(679, 308);
			dataGridView1.TabIndex = 16;
			dataGridView1.CellContentClick += dataGridView1_CellContentClick;
			dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
			// 
			// ColumnChk
			// 
			ColumnChk.Frozen = true;
			ColumnChk.HeaderText = "selection";
			ColumnChk.MinimumWidth = 6;
			ColumnChk.Name = "ColumnChk";
			ColumnChk.Resizable = DataGridViewTriState.True;
			ColumnChk.SortMode = DataGridViewColumnSortMode.Automatic;
			ColumnChk.Width = 125;
			// 
			// Column1Txt
			// 
			Column1Txt.Frozen = true;
			Column1Txt.HeaderText = "entrées importées";
			Column1Txt.MinimumWidth = 6;
			Column1Txt.Name = "Column1Txt";
			Column1Txt.Width = 125;
			// 
			// ColumnVide
			// 
			ColumnVide.Frozen = true;
			ColumnVide.HeaderText = "";
			ColumnVide.MinimumWidth = 6;
			ColumnVide.Name = "ColumnVide";
			ColumnVide.Width = 125;
			// 
			// Column2Chk
			// 
			Column2Chk.HeaderText = "selection";
			Column2Chk.MinimumWidth = 6;
			Column2Chk.Name = "Column2Chk";
			Column2Chk.Resizable = DataGridViewTriState.True;
			Column2Chk.SortMode = DataGridViewColumnSortMode.Automatic;
			Column2Chk.Width = 125;
			// 
			// Column2Txt
			// 
			Column2Txt.HeaderText = "entrées existantes";
			Column2Txt.MinimumWidth = 6;
			Column2Txt.Name = "Column2Txt";
			Column2Txt.Width = 125;
			// 
			// CtrlCnlImport4
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(dataGridView1);
			Controls.Add(label3);
			Controls.Add(label2);
			Controls.Add(checkBox3);
			Controls.Add(checkBox2);
			Controls.Add(checkBox1);
			Controls.Add(label1);
			Margin = new Padding(3, 4, 3, 4);
			Name = "CtrlCnlImport4";
			Size = new Size(915, 566);
			((ISupportInitialize)dataGridView1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label label1;
		private CheckBox checkBox1;
		private CheckBox checkBox2;
		private CheckBox checkBox3;
		private Label label2;
		private Label label3;
		private DataGridView dataGridView1;
		private DataGridViewCheckBoxColumn ColumnChk;
		private DataGridViewTextBoxColumn Column1Txt;
		private DataGridViewTextBoxColumn ColumnVide;
		private DataGridViewCheckBoxColumn Column2Chk;
		private DataGridViewTextBoxColumn Column2Txt;
	}


}
