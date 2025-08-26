namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    partial class CtrlSubscription
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
            gbSubscription = new GroupBox();
            numPublishingInterval = new NumericUpDown();
            lblPublishingInterval = new Label();
            txtDisplayName = new TextBox();
            lblDisplayName = new Label();
            chkSubscrActive = new CheckBox();
            gbSubscription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPublishingInterval).BeginInit();
            SuspendLayout();
            // 
            // gbSubscription
            // 
            gbSubscription.Controls.Add(numPublishingInterval);
            gbSubscription.Controls.Add(lblPublishingInterval);
            gbSubscription.Controls.Add(txtDisplayName);
            gbSubscription.Controls.Add(lblDisplayName);
            gbSubscription.Controls.Add(chkSubscrActive);
            gbSubscription.Dock = DockStyle.Fill;
            gbSubscription.Location = new Point(0, 0);
            gbSubscription.Name = "gbSubscription";
            gbSubscription.Padding = new Padding(10, 3, 10, 10);
            gbSubscription.Size = new Size(250, 500);
            gbSubscription.TabIndex = 0;
            gbSubscription.TabStop = false;
            gbSubscription.Text = "Subscription Parameters";
            // 
            // numPublishingInterval
            // 
            numPublishingInterval.Location = new Point(13, 106);
            numPublishingInterval.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numPublishingInterval.Name = "numPublishingInterval";
            numPublishingInterval.Size = new Size(120, 23);
            numPublishingInterval.TabIndex = 4;
            numPublishingInterval.ValueChanged += numPublishingInterval_ValueChanged;
            // 
            // lblPublishingInterval
            // 
            lblPublishingInterval.AutoSize = true;
            lblPublishingInterval.Location = new Point(10, 88);
            lblPublishingInterval.Name = "lblPublishingInterval";
            lblPublishingInterval.Size = new Size(105, 15);
            lblPublishingInterval.TabIndex = 3;
            lblPublishingInterval.Text = "Publishing interval";
            // 
            // txtDisplayName
            // 
            txtDisplayName.Location = new Point(13, 62);
            txtDisplayName.Name = "txtDisplayName";
            txtDisplayName.Size = new Size(224, 23);
            txtDisplayName.TabIndex = 2;
            txtDisplayName.TextChanged += txtDisplayName_TextChanged;
            // 
            // lblDisplayName
            // 
            lblDisplayName.AutoSize = true;
            lblDisplayName.Location = new Point(10, 44);
            lblDisplayName.Name = "lblDisplayName";
            lblDisplayName.Size = new Size(78, 15);
            lblDisplayName.TabIndex = 1;
            lblDisplayName.Text = "Display name";
            // 
            // chkSubscrActive
            // 
            chkSubscrActive.AutoSize = true;
            chkSubscrActive.Location = new Point(13, 22);
            chkSubscrActive.Name = "chkSubscrActive";
            chkSubscrActive.Size = new Size(59, 19);
            chkSubscrActive.TabIndex = 0;
            chkSubscrActive.Text = "Active";
            chkSubscrActive.UseVisualStyleBackColor = true;
            chkSubscrActive.CheckedChanged += chkSubscrActive_CheckedChanged;
            // 
            // CtrlSubscription
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbSubscription);
            Name = "CtrlSubscription";
            Size = new Size(250, 500);
            gbSubscription.ResumeLayout(false);
            gbSubscription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPublishingInterval).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSubscription;
        private System.Windows.Forms.CheckBox chkSubscrActive;
        private System.Windows.Forms.NumericUpDown numPublishingInterval;
        private System.Windows.Forms.Label lblPublishingInterval;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
    }
}
