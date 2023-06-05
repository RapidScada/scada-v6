namespace Scada.Admin.Extensions.ExtImport.Forms
{
    partial class FrmCnlImport
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
            lblStep = new Label();
            CtrlCnlImport1 = new Controls.CtrlCnlImport1();
            CtrlCnlImport2 = new Controls.CtrlCnlImport2();
            CtrlCnlImport3 = new Controls.CtrlCnlImport3();
            CtrlCnlImport4 = new Controls.CtrlCnlImport4();
            btnBack = new Button();
            btnNext = new Button();
            btnCreate = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblStep
            // 
            lblStep.BackColor = SystemColors.ActiveCaption;
            lblStep.Dock = DockStyle.Top;
            lblStep.Location = new Point(0, 0);
            lblStep.Name = "lblStep";
            lblStep.Size = new Size(445, 30);
            lblStep.TabIndex = 0;
            lblStep.Text = "Step 1 of 3: Step description";
            lblStep.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CtrlCnlImport1
            // 
            CtrlCnlImport1.Location = new Point(12, 33);
            CtrlCnlImport1.Margin = new Padding(3, 4, 3, 4);
            CtrlCnlImport1.Name = "CtrlCnlImport1";
            CtrlCnlImport1.Size = new Size(360, 200);
            CtrlCnlImport1.TabIndex = 1;
            CtrlCnlImport1.SelectedDeviceChanged += ctrlCnlCreate1_SelectedDeviceChanged;
            // 
            // CtrlCnlImport2
            // 
            CtrlCnlImport2.DeviceName = "";
            CtrlCnlImport2.Location = new Point(12, 33);
            CtrlCnlImport2.Margin = new Padding(3, 4, 3, 4);
            CtrlCnlImport2.Name = "CtrlCnlImport2";
            CtrlCnlImport2.Size = new Size(360, 100);
            CtrlCnlImport2.TabIndex = 2;
            // 
            // CtrlCnlImport3
            // 
            CtrlCnlImport3.ctrlCnlImport4 = null;
            CtrlCnlImport3.FileSelected = false;
            CtrlCnlImport3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CtrlCnlImport3.Location = new Point(12, 33);
            CtrlCnlImport3.Margin = new Padding(3, 4, 3, 4);
            CtrlCnlImport3.Name = "CtrlCnlImport3";
            CtrlCnlImport3.Size = new Size(421, 160);
            CtrlCnlImport3.TabIndex = 3;
            // 
            // CtrlCnlImport4
            // 
            CtrlCnlImport4.Location = new Point(0, 0);
            CtrlCnlImport4.Margin = new Padding(3, 4, 3, 4);
            CtrlCnlImport4.Name = "CtrlCnlImport4";
            CtrlCnlImport4.Size = new Size(1153, 564);
            CtrlCnlImport4.TabIndex = 0;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(135, 249);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 5;
            btnBack.Text = "< Back";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(216, 249);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(75, 23);
            btnNext.TabIndex = 6;
            btnNext.Text = "Next >";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnCreate
            // 
            btnCreate.Location = new Point(216, 249);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(75, 23);
            btnCreate.TabIndex = 7;
            btnCreate.Text = "Next >";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += btnCreate_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 249);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmCnlImport
            // 
            AcceptButton = btnCreate;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(445, 284);
            Controls.Add(btnCancel);
            Controls.Add(btnCreate);
            Controls.Add(btnNext);
            Controls.Add(btnBack);
            Controls.Add(CtrlCnlImport3);
            Controls.Add(CtrlCnlImport2);
            Controls.Add(CtrlCnlImport1);
            Controls.Add(lblStep);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCnlImport";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Import Channels";
            Load += FrmCnlImport_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label lblStep;
        private Controls.CtrlCnlImport1 CtrlCnlImport1;
        private Controls.CtrlCnlImport2 CtrlCnlImport2;
        private Controls.CtrlCnlImport3 CtrlCnlImport3;
        private Controls.CtrlCnlImport4 CtrlCnlImport4;
        private CheckBox chkPreview;
        private Button btnBack;
        private Button btnNext;
        private Button btnCreate;
        private Button btnCancel;
    }
}

