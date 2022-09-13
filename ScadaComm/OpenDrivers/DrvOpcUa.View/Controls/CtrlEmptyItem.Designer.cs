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
            this.gbEmptyItem = new System.Windows.Forms.GroupBox();
            this.lblNotSelected = new System.Windows.Forms.Label();
            this.gbEmptyItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmptyItem
            // 
            this.gbEmptyItem.Controls.Add(this.lblNotSelected);
            this.gbEmptyItem.Location = new System.Drawing.Point(0, 0);
            this.gbEmptyItem.Name = "gbEmptyItem";
            this.gbEmptyItem.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbEmptyItem.Size = new System.Drawing.Size(250, 500);
            this.gbEmptyItem.TabIndex = 4;
            this.gbEmptyItem.TabStop = false;
            this.gbEmptyItem.Text = "Item Parameters";
            // 
            // lblNotSelected
            // 
            this.lblNotSelected.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNotSelected.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNotSelected.Location = new System.Drawing.Point(13, 19);
            this.lblNotSelected.Name = "lblNotSelected";
            this.lblNotSelected.Size = new System.Drawing.Size(224, 50);
            this.lblNotSelected.TabIndex = 0;
            this.lblNotSelected.Text = "Item not selected";
            this.lblNotSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CtrlEmptyItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbEmptyItem);
            this.Name = "CtrlEmptyItem";
            this.Size = new System.Drawing.Size(250, 500);
            this.gbEmptyItem.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbEmptyItem;
        private Label lblNotSelected;
    }
}
