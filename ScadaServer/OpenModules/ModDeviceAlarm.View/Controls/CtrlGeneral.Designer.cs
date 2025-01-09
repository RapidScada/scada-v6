namespace Scada.Server.Modules.ModDeviceAlarm.View.Controls
{
    partial class CtrlGeneral
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
            gbGeneral = new GroupBox();
            numSendTimes = new NumericUpDown();
            lblSendTimes = new Label();
            gbEmailAddress = new GroupBox();
            txtEmailAddress = new TextBox();
            gbEmailContent = new GroupBox();
            lblEmailContent = new Label();
            lblEmailSubject = new Label();
            txtEmailSubject = new TextBox();
            txtEmailContent = new TextBox();
            txtName = new TextBox();
            lblName = new Label();
            txtTargetID = new TextBox();
            lblTargetID = new Label();
            chkActive = new CheckBox();
            gbGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSendTimes).BeginInit();
            gbEmailAddress.SuspendLayout();
            gbEmailContent.SuspendLayout();
            SuspendLayout();
            // 
            // gbGeneral
            // 
            gbGeneral.Controls.Add(numSendTimes);
            gbGeneral.Controls.Add(lblSendTimes);
            gbGeneral.Controls.Add(gbEmailAddress);
            gbGeneral.Controls.Add(gbEmailContent);
            gbGeneral.Controls.Add(txtName);
            gbGeneral.Controls.Add(lblName);
            gbGeneral.Controls.Add(txtTargetID);
            gbGeneral.Controls.Add(lblTargetID);
            gbGeneral.Controls.Add(chkActive);
            gbGeneral.Dock = DockStyle.Fill;
            gbGeneral.Location = new Point(0, 0);
            gbGeneral.Margin = new Padding(3, 3, 3, 11);
            gbGeneral.Name = "gbGeneral";
            gbGeneral.Padding = new Padding(10, 3, 10, 11);
            gbGeneral.Size = new Size(404, 524);
            gbGeneral.TabIndex = 0;
            gbGeneral.TabStop = false;
            gbGeneral.Text = "General Options";
            // 
            // numSendTimes
            // 
            numSendTimes.Location = new Point(271, 67);
            numSendTimes.Margin = new Padding(3, 3, 3, 11);
            numSendTimes.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numSendTimes.Name = "numSendTimes";
            numSendTimes.Size = new Size(120, 23);
            numSendTimes.TabIndex = 22;
            numSendTimes.Value = new decimal(new int[] { 3, 0, 0, 0 });
            numSendTimes.ValueChanged += numSendTimes_ValueChanged;
            // 
            // lblSendTimes
            // 
            lblSendTimes.AutoSize = true;
            lblSendTimes.Location = new Point(268, 46);
            lblSendTimes.Name = "lblSendTimes";
            lblSendTimes.Size = new Size(100, 17);
            lblSendTimes.TabIndex = 21;
            lblSendTimes.Text = "Mail send times";
            // 
            // gbEmailAddress
            // 
            gbEmailAddress.Controls.Add(txtEmailAddress);
            gbEmailAddress.Dock = DockStyle.Bottom;
            gbEmailAddress.Location = new Point(10, 97);
            gbEmailAddress.Name = "gbEmailAddress";
            gbEmailAddress.Padding = new Padding(4);
            gbEmailAddress.Size = new Size(384, 165);
            gbEmailAddress.TabIndex = 7;
            gbEmailAddress.TabStop = false;
            gbEmailAddress.Text = "EmailAddress";
            // 
            // txtEmailAddress
            // 
            txtEmailAddress.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtEmailAddress.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            txtEmailAddress.Location = new Point(7, 23);
            txtEmailAddress.Multiline = true;
            txtEmailAddress.Name = "txtEmailAddress";
            txtEmailAddress.ScrollBars = ScrollBars.Both;
            txtEmailAddress.Size = new Size(370, 135);
            txtEmailAddress.TabIndex = 3;
            txtEmailAddress.WordWrap = false;
            txtEmailAddress.TextChanged += txtEmailAddress_TextChanged;
            // 
            // gbEmailContent
            // 
            gbEmailContent.Controls.Add(lblEmailContent);
            gbEmailContent.Controls.Add(lblEmailSubject);
            gbEmailContent.Controls.Add(txtEmailSubject);
            gbEmailContent.Controls.Add(txtEmailContent);
            gbEmailContent.Dock = DockStyle.Bottom;
            gbEmailContent.Location = new Point(10, 262);
            gbEmailContent.Margin = new Padding(1);
            gbEmailContent.Name = "gbEmailContent";
            gbEmailContent.Padding = new Padding(4);
            gbEmailContent.Size = new Size(384, 251);
            gbEmailContent.TabIndex = 6;
            gbEmailContent.TabStop = false;
            gbEmailContent.Text = "EmailContent";
            // 
            // lblEmailContent
            // 
            lblEmailContent.AutoSize = true;
            lblEmailContent.Location = new Point(7, 59);
            lblEmailContent.Name = "lblEmailContent";
            lblEmailContent.Size = new Size(53, 17);
            lblEmailContent.TabIndex = 8;
            lblEmailContent.Text = "Content";
            // 
            // lblEmailSubject
            // 
            lblEmailSubject.AutoSize = true;
            lblEmailSubject.Location = new Point(7, 16);
            lblEmailSubject.Name = "lblEmailSubject";
            lblEmailSubject.Size = new Size(50, 17);
            lblEmailSubject.TabIndex = 8;
            lblEmailSubject.Text = "Subject";
            // 
            // txtEmailSubject
            // 
            txtEmailSubject.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtEmailSubject.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            txtEmailSubject.Location = new Point(7, 36);
            txtEmailSubject.Name = "txtEmailSubject";
            txtEmailSubject.ScrollBars = ScrollBars.Both;
            txtEmailSubject.Size = new Size(370, 23);
            txtEmailSubject.TabIndex = 5;
            txtEmailSubject.WordWrap = false;
            txtEmailSubject.TextChanged += txtEmailSubject_TextChanged;
            // 
            // txtEmailContent
            // 
            txtEmailContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtEmailContent.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            txtEmailContent.Location = new Point(7, 79);
            txtEmailContent.Margin = new Padding(1);
            txtEmailContent.Multiline = true;
            txtEmailContent.Name = "txtEmailContent";
            txtEmailContent.ScrollBars = ScrollBars.Both;
            txtEmailContent.Size = new Size(372, 167);
            txtEmailContent.TabIndex = 4;
            txtEmailContent.WordWrap = false;
            txtEmailContent.TextChanged += txtEmailContent_TextChanged;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(79, 67);
            txtName.Margin = new Padding(3, 3, 3, 11);
            txtName.Name = "txtName";
            txtName.Size = new Size(186, 23);
            txtName.TabIndex = 5;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(76, 47);
            lblName.Name = "lblName";
            lblName.Size = new Size(43, 17);
            lblName.TabIndex = 3;
            lblName.Text = "Name";
            // 
            // txtTargetID
            // 
            txtTargetID.Location = new Point(13, 67);
            txtTargetID.Margin = new Padding(3, 3, 3, 11);
            txtTargetID.Name = "txtTargetID";
            txtTargetID.ReadOnly = true;
            txtTargetID.Size = new Size(60, 23);
            txtTargetID.TabIndex = 4;
            // 
            // lblTargetID
            // 
            lblTargetID.AutoSize = true;
            lblTargetID.Location = new Point(10, 47);
            lblTargetID.Name = "lblTargetID";
            lblTargetID.Size = new Size(63, 17);
            lblTargetID.TabIndex = 2;
            lblTargetID.Text = "Target ID";
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Location = new Point(13, 25);
            chkActive.Margin = new Padding(3, 3, 3, 11);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(61, 21);
            chkActive.TabIndex = 1;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            chkActive.CheckedChanged += chkActive_CheckedChanged;
            // 
            // CtrlGeneral
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbGeneral);
            Margin = new Padding(3, 3, 3, 11);
            Name = "CtrlGeneral";
            Size = new Size(404, 524);
            gbGeneral.ResumeLayout(false);
            gbGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSendTimes).EndInit();
            gbEmailAddress.ResumeLayout(false);
            gbEmailAddress.PerformLayout();
            gbEmailContent.ResumeLayout(false);
            gbEmailContent.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbGeneral;
        private CheckBox chkActive;
        private TextBox txtName;
        private Label lblName;
        private TextBox txtTargetID;
        private Label lblTargetID;
        private GroupBox gbEmailAddress;
        private GroupBox gbEmailContent;
        private Label lblEmailContent;
        private Label lblEmailSubject;
        private TextBox txtEmailSubject;
        private TextBox txtEmailContent;
        private TextBox txtEmailAddress;
        private NumericUpDown numSendTimes;
        private Label lblSendTimes;
    }
}
