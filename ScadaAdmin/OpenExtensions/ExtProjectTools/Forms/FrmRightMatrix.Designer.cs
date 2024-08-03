namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    partial class FrmRightMatrix
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
            lvMatrix = new ListView();
            colObj = new ColumnHeader();
            SuspendLayout();
            // 
            // lvMatrix
            // 
            lvMatrix.BorderStyle = BorderStyle.None;
            lvMatrix.Columns.AddRange(new ColumnHeader[] { colObj });
            lvMatrix.Dock = DockStyle.Fill;
            lvMatrix.FullRowSelect = true;
            lvMatrix.GridLines = true;
            lvMatrix.Location = new Point(0, 0);
            lvMatrix.MultiSelect = false;
            lvMatrix.Name = "lvMatrix";
            lvMatrix.Size = new Size(784, 461);
            lvMatrix.TabIndex = 0;
            lvMatrix.UseCompatibleStateImageBehavior = false;
            lvMatrix.View = View.Details;
            // 
            // colObj
            // 
            colObj.Text = "";
            colObj.Width = 100;
            // 
            // FrmRightMatrix
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(lvMatrix);
            Name = "FrmRightMatrix";
            Text = "Right Matrix";
            Load += FrmRightMatrix_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListView lvMatrix;
        private ColumnHeader colObj;
    }
}