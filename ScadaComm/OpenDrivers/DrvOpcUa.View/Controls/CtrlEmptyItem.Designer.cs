namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    partial class CtrlEmptyItem
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
            gbEmptyItem = new GroupBox();
            lblNotSelected = new Label();
            gbEmptyItem.SuspendLayout();
            SuspendLayout();
            // 
            // gbEmptyItem
            // 
            gbEmptyItem.Controls.Add(lblNotSelected);
            gbEmptyItem.Dock = DockStyle.Fill;
            gbEmptyItem.Location = new Point(0, 0);
            gbEmptyItem.Name = "gbEmptyItem";
            gbEmptyItem.Padding = new Padding(10, 3, 10, 10);
            gbEmptyItem.Size = new Size(250, 500);
            gbEmptyItem.TabIndex = 4;
            gbEmptyItem.TabStop = false;
            gbEmptyItem.Text = "Item Parameters";
            // 
            // lblNotSelected
            // 
            lblNotSelected.Font = new Font("Segoe UI", 11.25F);
            lblNotSelected.ForeColor = SystemColors.GrayText;
            lblNotSelected.Location = new Point(13, 19);
            lblNotSelected.Name = "lblNotSelected";
            lblNotSelected.Size = new Size(224, 50);
            lblNotSelected.TabIndex = 0;
            lblNotSelected.Text = "Item not selected";
            lblNotSelected.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CtrlEmptyItem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbEmptyItem);
            Name = "CtrlEmptyItem";
            Size = new Size(250, 500);
            gbEmptyItem.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private GroupBox gbEmptyItem;
        private Label lblNotSelected;
    }
}
