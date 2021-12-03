namespace Scada.Comm.Drivers.DrvOpcUa.View.Forms
{
    partial class FrmEditingOptions
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
            this.lblDefaultTagCode = new System.Windows.Forms.Label();
            this.cbDefaultTagCode = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDefaultTagCode
            // 
            this.lblDefaultTagCode.AutoSize = true;
            this.lblDefaultTagCode.Location = new System.Drawing.Point(9, 9);
            this.lblDefaultTagCode.Name = "lblDefaultTagCode";
            this.lblDefaultTagCode.Size = new System.Drawing.Size(94, 15);
            this.lblDefaultTagCode.TabIndex = 0;
            this.lblDefaultTagCode.Text = "Default tag code";
            // 
            // cbDefaultTagCode
            // 
            this.cbDefaultTagCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultTagCode.FormattingEnabled = true;
            this.cbDefaultTagCode.Items.AddRange(new object[] {
            "Node ID",
            "Display name"});
            this.cbDefaultTagCode.Location = new System.Drawing.Point(12, 27);
            this.cbDefaultTagCode.Name = "cbDefaultTagCode";
            this.cbDefaultTagCode.Size = new System.Drawing.Size(260, 23);
            this.cbDefaultTagCode.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(116, 66);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(197, 66);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmEditingOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 101);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbDefaultTagCode);
            this.Controls.Add(this.lblDefaultTagCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEditingOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editing Options";
            this.Load += new System.EventHandler(this.FrmEditingOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblDefaultTagCode;
        private ComboBox cbDefaultTagCode;
        private Button btnOK;
        private Button btnCancel;
    }
}