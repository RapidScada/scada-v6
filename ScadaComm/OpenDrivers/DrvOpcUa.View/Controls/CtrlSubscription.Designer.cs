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
            this.gbSubscription = new System.Windows.Forms.GroupBox();
            this.numPublishingInterval = new System.Windows.Forms.NumericUpDown();
            this.lblPublishingInterval = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.chkSubscrActive = new System.Windows.Forms.CheckBox();
            this.gbSubscription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPublishingInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSubscription
            // 
            this.gbSubscription.Controls.Add(this.numPublishingInterval);
            this.gbSubscription.Controls.Add(this.lblPublishingInterval);
            this.gbSubscription.Controls.Add(this.txtDisplayName);
            this.gbSubscription.Controls.Add(this.lblDisplayName);
            this.gbSubscription.Controls.Add(this.chkSubscrActive);
            this.gbSubscription.Location = new System.Drawing.Point(0, 0);
            this.gbSubscription.Name = "gbSubscription";
            this.gbSubscription.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSubscription.Size = new System.Drawing.Size(250, 500);
            this.gbSubscription.TabIndex = 0;
            this.gbSubscription.TabStop = false;
            this.gbSubscription.Text = "Subscription Parameters";
            // 
            // numPublishingInterval
            // 
            this.numPublishingInterval.Location = new System.Drawing.Point(13, 106);
            this.numPublishingInterval.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPublishingInterval.Name = "numPublishingInterval";
            this.numPublishingInterval.Size = new System.Drawing.Size(120, 23);
            this.numPublishingInterval.TabIndex = 4;
            this.numPublishingInterval.ValueChanged += new System.EventHandler(this.numPublishingInterval_ValueChanged);
            // 
            // lblPublishingInterval
            // 
            this.lblPublishingInterval.AutoSize = true;
            this.lblPublishingInterval.Location = new System.Drawing.Point(10, 88);
            this.lblPublishingInterval.Name = "lblPublishingInterval";
            this.lblPublishingInterval.Size = new System.Drawing.Size(105, 15);
            this.lblPublishingInterval.TabIndex = 3;
            this.lblPublishingInterval.Text = "Publishing interval";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(13, 62);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(224, 23);
            this.txtDisplayName.TabIndex = 2;
            this.txtDisplayName.TextChanged += new System.EventHandler(this.txtDisplayName_TextChanged);
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(10, 44);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(78, 15);
            this.lblDisplayName.TabIndex = 1;
            this.lblDisplayName.Text = "Display name";
            // 
            // chkSubscrActive
            // 
            this.chkSubscrActive.AutoSize = true;
            this.chkSubscrActive.Location = new System.Drawing.Point(13, 22);
            this.chkSubscrActive.Name = "chkSubscrActive";
            this.chkSubscrActive.Size = new System.Drawing.Size(59, 19);
            this.chkSubscrActive.TabIndex = 0;
            this.chkSubscrActive.Text = "Active";
            this.chkSubscrActive.UseVisualStyleBackColor = true;
            this.chkSubscrActive.CheckedChanged += new System.EventHandler(this.chkSubscrActive_CheckedChanged);
            // 
            // CtrlSubscription
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbSubscription);
            this.Name = "CtrlSubscription";
            this.Size = new System.Drawing.Size(250, 500);
            this.gbSubscription.ResumeLayout(false);
            this.gbSubscription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPublishingInterval)).EndInit();
            this.ResumeLayout(false);

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
