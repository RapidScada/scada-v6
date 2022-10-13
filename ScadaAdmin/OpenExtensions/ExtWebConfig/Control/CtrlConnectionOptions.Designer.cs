namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    partial class CtrlConnectionOptions
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
            this.ctrlClientConnection = new Scada.Forms.Controls.CtrlClientConnection();
            this.SuspendLayout();
            // 
            // ctrlClientConnection
            // 
            this.ctrlClientConnection.ConnectionOptions = null;
            this.ctrlClientConnection.InstanceEnabled = false;
            this.ctrlClientConnection.Location = new System.Drawing.Point(0, 0);
            this.ctrlClientConnection.Name = "ctrlClientConnection";
            this.ctrlClientConnection.NameEnabled = false;
            this.ctrlClientConnection.Size = new System.Drawing.Size(487, 366);
            this.ctrlClientConnection.TabIndex = 4;
            this.ctrlClientConnection.ConnectionOptionsChanged += new System.EventHandler(this.control_Changed);
            // 
            // CtrlConnectionOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlClientConnection);
            this.Name = "CtrlConnectionOptions";
            this.Size = new System.Drawing.Size(550, 600);
            this.Load += new System.EventHandler(this.CtrlConnectionOptions_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Scada.Forms.Controls.CtrlClientConnection ctrlClientConnection;
    }
}
