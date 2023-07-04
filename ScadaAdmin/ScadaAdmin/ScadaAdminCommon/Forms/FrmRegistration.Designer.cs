namespace Scada.Admin.Forms
{
    partial class FrmRegistration
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
            btnSave = new System.Windows.Forms.Button();
            ctrlRegistration = new Scada.Forms.Controls.CtrlRegistration();
            btnCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(366, 398);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 23);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // ctrlRegistration
            // 
            ctrlRegistration.ComputerCode = "";
            ctrlRegistration.Location = new System.Drawing.Point(12, 12);
            ctrlRegistration.Name = "ctrlRegistration";
            ctrlRegistration.PermanentKeyUrl = "";
            ctrlRegistration.ProductCode = "";
            ctrlRegistration.RegistrationKey = "";
            ctrlRegistration.Size = new System.Drawing.Size(510, 375);
            ctrlRegistration.TabIndex = 0;
            ctrlRegistration.TrialKeyUrl = "";
            ctrlRegistration.RefreshCompCode += ctrlRegistration_RefreshCompCode;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(447, 398);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmRegistration
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(534, 433);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(ctrlRegistration);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRegistration";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Registration";
            Load += FrmRegistration_Load;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private Scada.Forms.Controls.CtrlRegistration ctrlRegistration;
        private System.Windows.Forms.Button btnCancel;
    }
}