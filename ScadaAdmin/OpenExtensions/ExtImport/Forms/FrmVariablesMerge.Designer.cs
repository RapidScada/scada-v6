using System.ComponentModel;
using Scada.Admin.Extensions.ExtImport.Code;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
    partial class FrmVariablesMerge
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
            lblSource = new Label();
            lblDestination = new Label();
            saveFileDialog1 = new SaveFileDialog();
            button1 = new Button();
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
            // checkBox3
            // 
            checkBox3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(10, 465);
            checkBox3.Margin = new Padding(3, 2, 3, 2);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(204, 19);
            checkBox3.TabIndex = 18;
            checkBox3.Text = ExtensionPhrases.ChkBoxMrgDesc;
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1Txt, ColumnChk, Column2Txt, Column33, Column4, ColumnVide, Column2Chk, Column2, Column3, Column44 });
            dataGridView1.Location = new Point(10, 43);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 180;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(1223, 418);
            dataGridView1.TabIndex = 21;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Column1Txt
            // 
            Column1Txt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column1Txt.HeaderText = ExtensionPhrases.AdressColName;
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
            Column2Txt.HeaderText = ExtensionPhrases.SrcMneColName;
            Column2Txt.MinimumWidth = 6;
            Column2Txt.Name = "Column2Txt";
            // 
            // Column33
            // 
            Column33.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column33.HeaderText = ExtensionPhrases.SrcTypeColName;
            Column33.MinimumWidth = 6;
            Column33.Name = "Column33";
            // 
            // Column4
            // 
            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column4.HeaderText = ExtensionPhrases.SrcCmentColName;
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
            Column2.HeaderText = ExtensionPhrases.DestMneColName;
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column3.HeaderText = ExtensionPhrases.DestTypeColName;
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            // 
            // Column44
            // 
            Column44.HeaderText = ExtensionPhrases.DestCmentColName;
            Column44.MinimumWidth = 6;
            Column44.Name = "Column44";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(1162, 465);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(72, 26);
            btnCancel.TabIndex = 23;
            btnCancel.Text = ExtensionPhrases.BtnCancel;
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblSource
            // 
            lblSource.BorderStyle = BorderStyle.FixedSingle;
            lblSource.Location = new Point(12, 9);
            lblSource.Name = "lblSource";
            lblSource.Padding = new Padding(2);
            lblSource.Size = new Size(120, 21);
            lblSource.TabIndex = 24;
            lblSource.Text = ExtensionPhrases.SrcLblName;
            lblSource.Visible = false;
            // 
            // lblDestination
            // 
            lblDestination.BorderStyle = BorderStyle.FixedSingle;
            lblDestination.Location = new Point(610, 15);
            lblDestination.Name = "lblDestination";
            lblDestination.Padding = new Padding(2);
            lblDestination.Size = new Size(121, 21);
            lblDestination.TabIndex = 25;
            lblDestination.Text = ExtensionPhrases.DestLblName;
            lblDestination.Visible = false;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "Fichiers XML (*.xml)|*.xml";
            saveFileDialog1.RestoreDirectory = true;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Location = new Point(1068, 465);
            button1.Name = "button1";
            button1.Size = new Size(88, 26);
            button1.TabIndex = 26;
            button1.Text = ExtensionPhrases.BtnSave;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(397, 15);
            label1.Name = "label1";
            label1.Size = new Size(45, 17);
            label1.TabIndex = 27;
            label1.Text = ExtensionPhrases.SrcLblName;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(920, 15);
            label2.Name = "label2";
            label2.Size = new Size(69, 17);
            label2.TabIndex = 28;
            label2.Text = ExtensionPhrases.DestLblName;
            // 
            // FrmVariableMerge
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(1246, 502);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(lblDestination);
            Controls.Add(lblSource);
            Controls.Add(btnCancel);
            Controls.Add(dataGridView1);
            Controls.Add(checkBox3);
            Controls.Add(label3);
            MinimumSize = new Size(1262, 539);
            Name = "FrmVariableMerge";
            Text = ExtensionPhrases.FrmVariablesMergeName;
            Load += FrmCnlMerge_Load;
            ((ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label3;
        private CheckBox checkBox3;
        private DataGridView dataGridView1;
        private Button btnCancel;
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
        private SaveFileDialog saveFileDialog;
        private SaveFileDialog saveFileDialog1;
        private Button button1;
        private Label label1;
        private Label label2;
    }
}