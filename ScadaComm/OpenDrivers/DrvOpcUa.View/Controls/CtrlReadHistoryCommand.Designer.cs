namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    partial class CtrlReadHistoryCommand
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
            numValuesPerNode = new NumericUpDown();
            lblValuesPerNode = new Label();
            lblNodeIDsHelp = new Label();
            txtNodeIDs = new TextBox();
            lblNodeIDs = new Label();
            numCmdNum = new NumericUpDown();
            lblCmdNum = new Label();
            txtCmdCode = new TextBox();
            lblCmdCode = new Label();
            txtDisplayName = new TextBox();
            lblDisplayName = new Label();
            gbCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numValuesPerNode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCmdNum).BeginInit();
            SuspendLayout();
            // 
            // gbCommand
            // 
            gbCommand.Controls.Add(numValuesPerNode);
            gbCommand.Controls.Add(lblValuesPerNode);
            gbCommand.Controls.Add(lblNodeIDsHelp);
            gbCommand.Controls.Add(txtNodeIDs);
            gbCommand.Controls.Add(lblNodeIDs);
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
            gbCommand.Text = "History Reading Command";
            // 
            // numValuesPerNode
            // 
            numValuesPerNode.Location = new Point(13, 275);
            numValuesPerNode.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numValuesPerNode.Name = "numValuesPerNode";
            numValuesPerNode.Size = new Size(120, 23);
            numValuesPerNode.TabIndex = 10;
            numValuesPerNode.ValueChanged += numValuesPerNode_ValueChanged;
            // 
            // lblValuesPerNode
            // 
            lblValuesPerNode.AutoSize = true;
            lblValuesPerNode.Location = new Point(10, 257);
            lblValuesPerNode.Name = "lblValuesPerNode";
            lblValuesPerNode.Size = new Size(90, 15);
            lblValuesPerNode.TabIndex = 9;
            lblValuesPerNode.Text = "Values per node";
            // 
            // lblNodeIDsHelp
            // 
            lblNodeIDsHelp.AutoSize = true;
            lblNodeIDsHelp.ForeColor = SystemColors.GrayText;
            lblNodeIDsHelp.Location = new Point(10, 232);
            lblNodeIDsHelp.Margin = new Padding(3, 0, 3, 10);
            lblNodeIDsHelp.Name = "lblNodeIDsHelp";
            lblNodeIDsHelp.Size = new Size(71, 15);
            lblNodeIDsHelp.TabIndex = 8;
            lblNodeIDsHelp.Text = "One per line";
            // 
            // txtNodeIDs
            // 
            txtNodeIDs.Location = new Point(13, 169);
            txtNodeIDs.Multiline = true;
            txtNodeIDs.Name = "txtNodeIDs";
            txtNodeIDs.ScrollBars = ScrollBars.Vertical;
            txtNodeIDs.Size = new Size(224, 60);
            txtNodeIDs.TabIndex = 7;
            txtNodeIDs.WordWrap = false;
            txtNodeIDs.TextChanged += txtNodeID_TextChanged;
            // 
            // lblNodeIDs
            // 
            lblNodeIDs.AutoSize = true;
            lblNodeIDs.Location = new Point(10, 151);
            lblNodeIDs.Name = "lblNodeIDs";
            lblNodeIDs.Size = new Size(55, 15);
            lblNodeIDs.TabIndex = 6;
            lblNodeIDs.Text = "Node IDs";
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
            // CtrlReadHistoryCommand
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbCommand);
            Name = "CtrlReadHistoryCommand";
            Size = new Size(250, 500);
            gbCommand.ResumeLayout(false);
            gbCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numValuesPerNode).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCmdNum).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCommand;
        private System.Windows.Forms.TextBox txtNodeIDs;
        private System.Windows.Forms.Label lblNodeIDs;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.Label lblCmdNum;
        private TextBox txtCmdCode;
        private Label lblCmdCode;
        private Label lblNodeIDsHelp;
        private NumericUpDown numValuesPerNode;
        private Label lblValuesPerNode;
    }
}
