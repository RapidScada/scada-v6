using System.ComponentModel;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
	partial class FrmCnlMerge
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
			groupBox1 = new GroupBox();
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
			label3 = new Label();
			checkBox2 = new CheckBox();
			checkBox1 = new CheckBox();
			checkBox3 = new CheckBox();
			groupBox1.SuspendLayout();
			((ISupportInitialize)dataGridView1).BeginInit();
			SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(dataGridView1);
			groupBox1.Location = new Point(3, 81);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(1150, 431);
			groupBox1.TabIndex = 16;
			groupBox1.TabStop = false;
			groupBox1.Text = "groupBox1";
			// 
			// dataGridView1
			// 
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1Txt, ColumnChk, Column2Txt, Column33, Column4, ColumnVide, Column2Chk, Column2, Column3, Column44 });
			dataGridView1.Dock = DockStyle.Fill;
			dataGridView1.Location = new Point(3, 23);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.RowHeadersWidth = 51;
			dataGridView1.RowTemplate.Height = 29;
			dataGridView1.Size = new Size(1144, 405);
			dataGridView1.TabIndex = 16;
			dataGridView1.CellContentClick += dataGridView1_CellContentClick;
			dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
			// 
			// Column1Txt
			// 
			Column1Txt.Frozen = true;
			Column1Txt.HeaderText = "Adresse";
			Column1Txt.MinimumWidth = 6;
			Column1Txt.Name = "Column1Txt";
			Column1Txt.Width = 125;
			// 
			// ColumnChk
			// 
			ColumnChk.HeaderText = "";
			ColumnChk.MinimumWidth = 6;
			ColumnChk.Name = "ColumnChk";
			ColumnChk.Resizable = DataGridViewTriState.True;
			ColumnChk.SortMode = DataGridViewColumnSortMode.Automatic;
			ColumnChk.Width = 50;
			// 
			// Column2Txt
			// 
			Column2Txt.HeaderText = "Mnémonique";
			Column2Txt.MinimumWidth = 6;
			Column2Txt.Name = "Column2Txt";
			Column2Txt.Width = 125;
			// 
			// Column33
			// 
			Column33.HeaderText = "Type";
			Column33.MinimumWidth = 6;
			Column33.Name = "Column33";
			Column33.Width = 125;
			// 
			// Column4
			// 
			Column4.HeaderText = "Commentaire";
			Column4.MinimumWidth = 6;
			Column4.Name = "Column4";
			Column4.Width = 125;
			// 
			// ColumnVide
			// 
			ColumnVide.HeaderText = "";
			ColumnVide.MinimumWidth = 6;
			ColumnVide.Name = "ColumnVide";
			ColumnVide.Width = 125;
			// 
			// Column2Chk
			// 
			Column2Chk.HeaderText = "";
			Column2Chk.MinimumWidth = 6;
			Column2Chk.Name = "Column2Chk";
			Column2Chk.Resizable = DataGridViewTriState.True;
			Column2Chk.SortMode = DataGridViewColumnSortMode.Automatic;
			Column2Chk.Width = 50;
			// 
			// Column2
			// 
			Column2.HeaderText = "Mnémonique";
			Column2.MinimumWidth = 6;
			Column2.Name = "Column2";
			Column2.Width = 125;
			// 
			// Column3
			// 
			Column3.HeaderText = "Type";
			Column3.MinimumWidth = 6;
			Column3.Name = "Column3";
			Column3.Width = 125;
			// 
			// Column44
			// 
			Column44.HeaderText = "Commentaire";
			Column44.MinimumWidth = 6;
			Column44.Name = "Column44";
			Column44.Width = 125;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(6, 27);
			label3.Name = "label3";
			label3.Size = new Size(340, 20);
			label3.TabIndex = 17;
			label3.Text = "Rapprochement avec les données déjà existantes :";
			// 
			// checkBox2
			// 
			checkBox2.AutoSize = true;
			checkBox2.Location = new Point(819, 518);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new Size(108, 24);
			checkBox2.TabIndex = 20;
			checkBox2.Text = "Tout cocher";
			checkBox2.UseVisualStyleBackColor = true;
			checkBox2.CheckedChanged += checkBox2_CheckedChanged;
			// 
			// checkBox1
			// 
			checkBox1.AutoSize = true;
			checkBox1.Location = new Point(211, 518);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(112, 24);
			checkBox1.TabIndex = 19;
			checkBox1.Text = "Tout cocher ";
			checkBox1.UseVisualStyleBackColor = true;
			checkBox1.CheckedChanged += checkBox1_CheckedChanged;
			// 
			// checkBox3
			// 
			checkBox3.AutoSize = true;
			checkBox3.Location = new Point(406, 529);
			checkBox3.Name = "checkBox3";
			checkBox3.Size = new Size(300, 24);
			checkBox3.TabIndex = 18;
			checkBox3.Text = "préserver les descriptions déjà existantes";
			checkBox3.UseVisualStyleBackColor = true;
			checkBox3.CheckedChanged += checkBox3_CheckedChanged;
			// 
			// CtrlCnlImport4
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(checkBox2);
			Controls.Add(checkBox1);
			Controls.Add(checkBox3);
			Controls.Add(label3);
			Controls.Add(groupBox1);
			Margin = new Padding(3, 4, 3, 4);
			Name = "CtrlCnlImport4";
			Size = new Size(1153, 564);
			groupBox1.ResumeLayout(false);
			((ISupportInitialize)dataGridView1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private GroupBox groupBox1;
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
		private Label label3;
		private CheckBox checkBox2;
		private CheckBox checkBox1;
		private CheckBox checkBox3;

		///// <summary>
		///// Required designer variable.
		///// </summary>
		//private System.ComponentModel.IContainer components = null;

		///// <summary>
		///// Clean up any resources being used.
		///// </summary>
		///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		//protected override void Dispose(bool disposing)
		//{
		//	if (disposing && (components != null))
		//	{
		//		components.Dispose();
		//	}
		//	base.Dispose(disposing);
		//}

		//#region Windows Form Designer generated code

		///// <summary>
		///// Required method for Designer support - do not modify
		///// the contents of this method with the code editor.
		///// </summary>
		//private void InitializeComponent()
		//{
		//	this.components = new System.ComponentModel.Container();
		//	this.btnDelete = new System.Windows.Forms.Button();
		//	this.dataGridView = new System.Windows.Forms.DataGridView();
		//	this.colCnlNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
		//	this.colActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		//	this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		//	this.colTagNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
		//	this.colTagCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		//	this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
		//	this.btnAdd = new System.Windows.Forms.Button();
		//	this.btnCancel = new System.Windows.Forms.Button();
		//	((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
		//	((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
		//	this.SuspendLayout();
		//	// 
		//	// btnDelete
		//	// 
		//	this.btnDelete.Location = new System.Drawing.Point(12, 12);
		//	this.btnDelete.Name = "btnDelete";
		//	this.btnDelete.Size = new System.Drawing.Size(75, 23);
		//	this.btnDelete.TabIndex = 0;
		//	this.btnDelete.Text = "Delete";
		//	this.btnDelete.UseVisualStyleBackColor = true;
		//	this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
		//	// 
		//	// dataGridView
		//	// 
		//	this.dataGridView.AllowUserToAddRows = false;
		//	this.dataGridView.AllowUserToResizeRows = false;
		//	this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
		//	| System.Windows.Forms.AnchorStyles.Left)
		//	| System.Windows.Forms.AnchorStyles.Right)));
		//	this.dataGridView.AutoGenerateColumns = false;
		//	this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		//	this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
		//	this.colCnlNum,
		//	this.colActive,
		//	this.colName,
		//	this.colTagNum,
		//	this.colTagCode});
		//	this.dataGridView.DataSource = this.bindingSource;
		//	this.dataGridView.Location = new System.Drawing.Point(12, 41);
		//	this.dataGridView.Name = "dataGridView";
		//	this.dataGridView.Size = new System.Drawing.Size(660, 369);
		//	this.dataGridView.TabIndex = 1;
		//	this.dataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView_RowsRemoved);
		//	// 
		//	// colCnlNum
		//	// 
		//	this.colCnlNum.DataPropertyName = "CnlNum";
		//	this.colCnlNum.HeaderText = "Number";
		//	this.colCnlNum.Name = "colCnlNum";
		//	// 
		//	// colActive
		//	// 
		//	this.colActive.DataPropertyName = "Active";
		//	this.colActive.HeaderText = "Active";
		//	this.colActive.Name = "colActive";
		//	// 
		//	// colName
		//	// 
		//	this.colName.DataPropertyName = "Name";
		//	this.colName.HeaderText = "Name";
		//	this.colName.Name = "colName";
		//	// 
		//	// colTagNum
		//	// 
		//	this.colTagNum.DataPropertyName = "TagNum";
		//	this.colTagNum.HeaderText = "Tag Number";
		//	this.colTagNum.Name = "colTagNum";
		//	// 
		//	// colTagCode
		//	// 
		//	this.colTagCode.DataPropertyName = "TagCode";
		//	this.colTagCode.HeaderText = "Tag Code";
		//	this.colTagCode.Name = "colTagCode";
		//	// 
		//	// bindingSource
		//	// 
		//	this.bindingSource.AllowNew = false;
		//	// 
		//	// btnAdd
		//	// 
		//	this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		//	this.btnAdd.Location = new System.Drawing.Point(516, 426);
		//	this.btnAdd.Name = "btnAdd";
		//	this.btnAdd.Size = new System.Drawing.Size(75, 23);
		//	this.btnAdd.TabIndex = 2;
		//	this.btnAdd.Text = "Add";
		//	this.btnAdd.UseVisualStyleBackColor = true;
		//	this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
		//	// 
		//	// btnCancel
		//	// 
		//	this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		//	this.btnCancel.Location = new System.Drawing.Point(597, 426);
		//	this.btnCancel.Name = "btnCancel";
		//	this.btnCancel.Size = new System.Drawing.Size(75, 23);
		//	this.btnCancel.TabIndex = 3;
		//	this.btnCancel.Text = "Cancel";
		//	this.btnCancel.UseVisualStyleBackColor = true;
		//	// 
		//	// FrmCnlPreview
		//	// 
		//	this.AcceptButton = this.btnAdd;
		//	this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		//	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		//	this.CancelButton = this.btnCancel;
		//	this.ClientSize = new System.Drawing.Size(684, 461);
		//	this.Controls.Add(this.btnCancel);
		//	this.Controls.Add(this.btnAdd);
		//	this.Controls.Add(this.dataGridView);
		//	this.Controls.Add(this.btnDelete);
		//	this.MaximizeBox = false;
		//	this.MinimizeBox = false;
		//	this.Name = "FrmCnlPreview";
		//	this.ShowIcon = false;
		//	this.ShowInTaskbar = false;
		//	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		//	this.Text = "Merge Channels";
		//	this.Load += new System.EventHandler(this.FrmCnlPreview_Load);
		//	((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
		//	((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
		//	this.ResumeLayout(false);

		//}

		//#endregion

		//private System.Windows.Forms.Button btnDelete;
		//private System.Windows.Forms.DataGridView dataGridView;
		//private System.Windows.Forms.Button btnAdd;
		//private System.Windows.Forms.Button btnCancel;
		//private System.Windows.Forms.DataGridViewTextBoxColumn colCnlNum;
		//private System.Windows.Forms.DataGridViewCheckBoxColumn colActive;
		//private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		//private System.Windows.Forms.DataGridViewTextBoxColumn colTagNum;
		//private System.Windows.Forms.DataGridViewTextBoxColumn colTagCode;
		//private System.Windows.Forms.BindingSource bindingSource;
	}
}