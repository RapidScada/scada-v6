namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    partial class CtrlCommand
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
            gbCommand = new GroupBox();
            chkIsMethod = new CheckBox();
            pbDataTypeWarning = new PictureBox();
            cbDataType = new ComboBox();
            lblDataType = new Label();
            txtParentNodeID = new TextBox();
            lblParentNodeID = new Label();
            txtNodeID = new TextBox();
            lblNodeID = new Label();
            numCmdNum = new NumericUpDown();
            lblCmdNum = new Label();
            txtCmdCode = new TextBox();
            lblCmdCode = new Label();
            txtDisplayName = new TextBox();
            lblDisplayName = new Label();
            gbCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbDataTypeWarning).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCmdNum).BeginInit();
            SuspendLayout();
            // 
            // gbCommand
            // 
            gbCommand.Controls.Add(chkIsMethod);
            gbCommand.Controls.Add(pbDataTypeWarning);
            gbCommand.Controls.Add(cbDataType);
            gbCommand.Controls.Add(lblDataType);
            gbCommand.Controls.Add(txtParentNodeID);
            gbCommand.Controls.Add(lblParentNodeID);
            gbCommand.Controls.Add(txtNodeID);
            gbCommand.Controls.Add(lblNodeID);
            gbCommand.Controls.Add(numCmdNum);
            gbCommand.Controls.Add(lblCmdNum);
            gbCommand.Controls.Add(txtCmdCode);
            gbCommand.Controls.Add(lblCmdCode);
            gbCommand.Controls.Add(txtDisplayName);
            gbCommand.Controls.Add(lblDisplayName);
            gbCommand.Dock = DockStyle.Fill;
            gbCommand.Location = new Point(0, 0);
            gbCommand.Name = "gbCommand";
            gbCommand.Padding = new Padding(10, 3, 10, 10);
            gbCommand.Size = new Size(250, 500);
            gbCommand.TabIndex = 0;
            gbCommand.TabStop = false;
            gbCommand.Text = "Command Parameters";
            // 
            // chkIsMethod
            // 
            chkIsMethod.AutoSize = true;
            chkIsMethod.Enabled = false;
            chkIsMethod.Location = new Point(13, 286);
            chkIsMethod.Name = "chkIsMethod";
            chkIsMethod.Size = new Size(79, 19);
            chkIsMethod.TabIndex = 12;
            chkIsMethod.Text = "Is method";
            chkIsMethod.UseVisualStyleBackColor = true;
            // 
            // pbDataTypeWarning
            // 
            pbDataTypeWarning.BackColor = SystemColors.Window;
            pbDataTypeWarning.Image = Properties.Resources.warning;
            pbDataTypeWarning.Location = new Point(201, 260);
            pbDataTypeWarning.Name = "pbDataTypeWarning";
            pbDataTypeWarning.Size = new Size(16, 16);
            pbDataTypeWarning.TabIndex = 19;
            pbDataTypeWarning.TabStop = false;
            // 
            // cbDataType
            // 
            cbDataType.FormattingEnabled = true;
            cbDataType.Location = new Point(13, 257);
            cbDataType.Name = "cbDataType";
            cbDataType.Size = new Size(224, 23);
            cbDataType.TabIndex = 11;
            cbDataType.TextChanged += cbDataType_TextChanged;
            // 
            // lblDataType
            // 
            lblDataType.AutoSize = true;
            lblDataType.Location = new Point(10, 239);
            lblDataType.Name = "lblDataType";
            lblDataType.Size = new Size(57, 15);
            lblDataType.TabIndex = 10;
            lblDataType.Text = "Data type";
            // 
            // txtParentNodeID
            // 
            txtParentNodeID.Location = new Point(13, 213);
            txtParentNodeID.Name = "txtParentNodeID";
            txtParentNodeID.ReadOnly = true;
            txtParentNodeID.Size = new Size(224, 23);
            txtParentNodeID.TabIndex = 9;
            // 
            // lblParentNodeID
            // 
            lblParentNodeID.AutoSize = true;
            lblParentNodeID.Location = new Point(10, 195);
            lblParentNodeID.Name = "lblParentNodeID";
            lblParentNodeID.Size = new Size(85, 15);
            lblParentNodeID.TabIndex = 8;
            lblParentNodeID.Text = "Parent node ID";
            // 
            // txtNodeID
            // 
            txtNodeID.Location = new Point(13, 169);
            txtNodeID.Name = "txtNodeID";
            txtNodeID.ReadOnly = true;
            txtNodeID.Size = new Size(224, 23);
            txtNodeID.TabIndex = 7;
            // 
            // lblNodeID
            // 
            lblNodeID.AutoSize = true;
            lblNodeID.Location = new Point(10, 151);
            lblNodeID.Name = "lblNodeID";
            lblNodeID.Size = new Size(50, 15);
            lblNodeID.TabIndex = 6;
            lblNodeID.Text = "Node ID";
            // 
            // numCmdNum
            // 
            numCmdNum.Location = new Point(13, 125);
            numCmdNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numCmdNum.Name = "numCmdNum";
            numCmdNum.Size = new Size(120, 23);
            numCmdNum.TabIndex = 5;
            numCmdNum.ValueChanged += numCmdNum_ValueChanged;
            // 
            // lblCmdNum
            // 
            lblCmdNum.AutoSize = true;
            lblCmdNum.Location = new Point(10, 107);
            lblCmdNum.Name = "lblCmdNum";
            lblCmdNum.Size = new Size(109, 15);
            lblCmdNum.TabIndex = 4;
            lblCmdNum.Text = "Command number";
            // 
            // txtCmdCode
            // 
            txtCmdCode.Location = new Point(13, 81);
            txtCmdCode.Name = "txtCmdCode";
            txtCmdCode.Size = new Size(224, 23);
            txtCmdCode.TabIndex = 3;
            txtCmdCode.TextChanged += txtCmdCode_TextChanged;
            // 
            // lblCmdCode
            // 
            lblCmdCode.AutoSize = true;
            lblCmdCode.Location = new Point(10, 63);
            lblCmdCode.Name = "lblCmdCode";
            lblCmdCode.Size = new Size(93, 15);
            lblCmdCode.TabIndex = 2;
            lblCmdCode.Text = "Command code";
            // 
            // txtDisplayName
            // 
            txtDisplayName.Location = new Point(13, 37);
            txtDisplayName.Name = "txtDisplayName";
            txtDisplayName.Size = new Size(224, 23);
            txtDisplayName.TabIndex = 1;
            txtDisplayName.TextChanged += txtDisplayName_TextChanged;
            // 
            // lblDisplayName
            // 
            lblDisplayName.AutoSize = true;
            lblDisplayName.Location = new Point(10, 19);
            lblDisplayName.Name = "lblDisplayName";
            lblDisplayName.Size = new Size(78, 15);
            lblDisplayName.TabIndex = 0;
            lblDisplayName.Text = "Display name";
            // 
            // CtrlCommand
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbCommand);
            Name = "CtrlCommand";
            Size = new Size(250, 500);
            gbCommand.ResumeLayout(false);
            gbCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbDataTypeWarning).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCmdNum).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCommand;
        private System.Windows.Forms.TextBox txtNodeID;
        private System.Windows.Forms.Label lblNodeID;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.Label lblCmdNum;
        private TextBox txtCmdCode;
        private Label lblCmdCode;
        private CheckBox chkIsMethod;
        private TextBox txtParentNodeID;
        private Label lblParentNodeID;
        private PictureBox pbDataTypeWarning;
        private ComboBox cbDataType;
    }
}
