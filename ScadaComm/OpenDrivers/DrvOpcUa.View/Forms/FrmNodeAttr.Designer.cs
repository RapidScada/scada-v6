namespace Scada.Comm.Drivers.DrvOpcUa.View.Forms
{
    partial class FrmNodeAttr
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
            components = new System.ComponentModel.Container();
            listView = new ListView();
            colName = new ColumnHeader();
            colValue = new ColumnHeader();
            btnClose = new Button();
            cmsAttr = new ContextMenuStrip(components);
            miCopyValue = new ToolStripMenuItem();
            miCopyName = new ToolStripMenuItem();
            cmsAttr.SuspendLayout();
            SuspendLayout();
            // 
            // listView
            // 
            listView.Columns.AddRange(new ColumnHeader[] { colName, colValue });
            listView.ContextMenuStrip = cmsAttr;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Location = new Point(12, 12);
            listView.MultiSelect = false;
            listView.Name = "listView";
            listView.ShowItemToolTips = true;
            listView.Size = new Size(310, 308);
            listView.TabIndex = 0;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            colName.Text = "Name";
            colName.Width = 120;
            // 
            // colValue
            // 
            colValue.Text = "Value";
            colValue.Width = 160;
            // 
            // btnClose
            // 
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(247, 326);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // cmsAttr
            // 
            cmsAttr.Items.AddRange(new ToolStripItem[] { miCopyName, miCopyValue });
            cmsAttr.Name = "cmsAttr";
            cmsAttr.Size = new Size(181, 70);
            // 
            // miCopyValue
            // 
            miCopyValue.Name = "miCopyValue";
            miCopyValue.Size = new Size(180, 22);
            miCopyValue.Text = "Copy Value";
            miCopyValue.Click += miCopyValue_Click;
            // 
            // miCopyName
            // 
            miCopyName.Name = "miCopyName";
            miCopyName.Size = new Size(180, 22);
            miCopyName.Text = "Copy Name";
            miCopyName.Click += miCopyName_Click;
            // 
            // FrmNodeAttr
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(334, 361);
            Controls.Add(btnClose);
            Controls.Add(listView);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmNodeAttr";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Node Attributes";
            Load += FrmNodeAttr_Load;
            Shown += FrmNodeAttr_Shown;
            cmsAttr.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colValue;
        private ContextMenuStrip cmsAttr;
        private ToolStripMenuItem miCopyValue;
        private ToolStripMenuItem miCopyName;
    }
}