using Scada.Admin.Extensions.ExtImport.Code;
using System.ComponentModel;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
    partial class FrmCnlsMerge
    {
        //private DataGridView dataGridView;

        /// <summary> 
        /// 
        /// </summary>
        private IContainer components = null;

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
            dataGridView1 = new DataGridView();
            Column1Txt = new DataGridViewTextBoxColumn();
            ColumnChk = new DataGridViewCheckBoxColumn();
            fcnlName = new DataGridViewTextBoxColumn();
            fdataType = new DataGridViewTextBoxColumn();
            fcnlType = new DataGridViewTextBoxColumn();
            fTagCode = new DataGridViewTextBoxColumn();
            ColumnVide = new DataGridViewTextBoxColumn();
            Column2Chk = new DataGridViewCheckBoxColumn();
            cnlName = new DataGridViewTextBoxColumn();
            dataType = new DataGridViewTextBoxColumn();
            cnlType = new DataGridViewTextBoxColumn();
            tagCode = new DataGridViewTextBoxColumn();
            btnCancel = new Button();
            btnAdd = new Button();
            lblSource = new Label();
            lblDestination = new Label();
            saveFileDialog1 = new SaveFileDialog();
            label1 = new Label();
            label2 = new Label();
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
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1Txt, ColumnChk, fcnlName, fdataType, fcnlType, fTagCode, ColumnVide, Column2Chk, cnlName, dataType, cnlType, tagCode });
            dataGridView1.Location = new Point(10, 33);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 180;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(1462, 498);
            dataGridView1.TabIndex = 21;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Column1Txt
            // 
            Column1Txt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column1Txt.HeaderText = ExtensionPhrases.NumCol;
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
            // fcnlName
            // 
            fcnlName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            fcnlName.HeaderText = ExtensionPhrases.NewCnlsNameCol;
            fcnlName.MinimumWidth = 6;
            fcnlName.Name = "fcnlName";
            // 
            // fdataType
            // 
            fdataType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            fdataType.HeaderText = ExtensionPhrases.NewTypeCol;
            fdataType.MinimumWidth = 6;
            fdataType.Name = "fdataType";
            // 
            // fcnlType
            // 
            fcnlType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            fcnlType.HeaderText = ExtensionPhrases.NewCnlsTypeCol;
            fcnlType.MinimumWidth = 6;
            fcnlType.Name = "fcnlType";
            // 
            // fTagCode
            // 
            fTagCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            fTagCode.HeaderText = ExtensionPhrases.NewTagCodeCol;
            fTagCode.MinimumWidth = 6;
            fTagCode.Name = "fTagCode";
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
            // cnlName
            // 
            cnlName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cnlName.HeaderText = ExtensionPhrases.EquipCnlsNameCol;
            cnlName.MinimumWidth = 6;
            cnlName.Name = "cnlName";
            // 
            // dataType
            // 
            dataType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataType.HeaderText = ExtensionPhrases.EquipTypeCol;
            dataType.MinimumWidth = 6;
            dataType.Name = "dataType";
            // 
            // cnlType
            // 
            cnlType.HeaderText = ExtensionPhrases.EquipCnlsTypeCol;
            cnlType.MinimumWidth = 6;
            cnlType.Name = "cnlType";
            // 
            // tagCode
            // 
            tagCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tagCode.HeaderText = ExtensionPhrases.EquipTagCodeCol;
            tagCode.MinimumWidth = 6;
            tagCode.Name = "tagCode";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(1354, 536);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(104, 26);
            btnCancel.TabIndex = 23;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.Location = new Point(1232, 536);
            btnAdd.Margin = new Padding(3, 2, 3, 2);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(95, 26);
            btnAdd.TabIndex = 22;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click_1;
            // 
            // lblSource
            // 
            lblSource.BorderStyle = BorderStyle.FixedSingle;
            lblSource.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            lblSource.Location = new Point(12, 5);
            lblSource.Name = "lblSource";
            lblSource.Padding = new Padding(2);
            lblSource.Size = new Size(125, 26);
            lblSource.TabIndex = 24;
            lblSource.Text = ExtensionPhrases.LblNewCnls;
            lblSource.Visible = false;
            // 
            // lblDestination
            // 
            lblDestination.BorderStyle = BorderStyle.FixedSingle;
            lblDestination.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            lblDestination.Location = new Point(1172, 5);
            lblDestination.Name = "lblDestination";
            lblDestination.Padding = new Padding(2);
            lblDestination.Size = new Size(155, 26);
            lblDestination.TabIndex = 25;
            lblDestination.Text = ExtensionPhrases.LblEquipCnls;
            lblDestination.Visible = false;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "Fichiers XML (*.xml)|*.xml";
            saveFileDialog1.RestoreDirectory = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(399, 9);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 28;
            label1.Text = ExtensionPhrases.LblNewCnls;
			// 
			// label2
			// 
			label2.AutoSize = true;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(1020, 9);
            label2.Name = "label2";
            label2.Size = new Size(98, 17);
            label2.TabIndex = 29;
            label2.Text = ExtensionPhrases.LblEquipCnls;
			// 
			// FrmCnlsMerge
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(1482, 567);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblDestination);
            Controls.Add(lblSource);
            Controls.Add(btnCancel);
            Controls.Add(btnAdd);
            Controls.Add(dataGridView1);
            Controls.Add(label3);
            MinimumSize = new Size(1262, 539);
            Name = "FrmCnlsMerge";
            Text = ExtensionPhrases.FrmCnlsName;
            Load += FrmCnlMerge_Load;
            ((ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label3;
        private DataGridView dataGridView1;
        private Button btnCancel;
        private Button btnAdd;
        private Label lblSource;
        private Label lblDestination;
        private SaveFileDialog saveFileDialog;
        private SaveFileDialog saveFileDialog1;
        private DataGridViewTextBoxColumn Column1Txt;
        private DataGridViewCheckBoxColumn ColumnChk;
        private DataGridViewTextBoxColumn fcnlName;
        private DataGridViewTextBoxColumn fdataType;
        private DataGridViewTextBoxColumn fcnlType;
        private DataGridViewTextBoxColumn fTagCode;
        private DataGridViewTextBoxColumn ColumnVide;
        private DataGridViewCheckBoxColumn Column2Chk;
        private DataGridViewTextBoxColumn cnlName;
        private DataGridViewTextBoxColumn dataType;
        private DataGridViewTextBoxColumn cnlType;
        private DataGridViewTextBoxColumn tagCode;
        private Label label1;
        private Label label2;
    }
}