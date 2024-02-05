namespace Scada.Server.Modules.ModDbExport.View.Forms
{
    partial class FrmQueryParametrs
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ListViewItem listViewItem1 = new ListViewItem("");
            btnClose = new Button();
            lvParameters = new ListView();
            colParamsName = new ColumnHeader();
            colParams = new ColumnHeader();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(430, 353);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // lvParameters
            // 
            lvParameters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvParameters.BackColor = SystemColors.Window;
            lvParameters.Columns.AddRange(new ColumnHeader[] { colParamsName, colParams });
            lvParameters.FullRowSelect = true;
            lvParameters.GridLines = true;
            lvParameters.Items.AddRange(new ListViewItem[] { listViewItem1 });
            lvParameters.Location = new Point(12, 12);
            lvParameters.Name = "lvParameters";
            lvParameters.Size = new Size(493, 325);
            lvParameters.TabIndex = 0;
            lvParameters.UseCompatibleStateImageBehavior = false;
            lvParameters.View = System.Windows.Forms.View.Details;
            // 
            // colParamsName
            // 
            colParamsName.Text = "Name";
            colParamsName.Width = 160;
            // 
            // colParams
            // 
            colParams.Text = "Description";
            colParams.Width = 310;
            // 
            // FrmQueryParametrs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(517, 388);
            Controls.Add(btnClose);
            Controls.Add(lvParameters);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(300, 200);
            Name = "FrmQueryParametrs";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Available Parameters";
            Load += FrmQueryParametrs_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnClose;
        private ListView lvParameters;
        private ColumnHeader colParamsName;
        private ColumnHeader colParams;
    }
}