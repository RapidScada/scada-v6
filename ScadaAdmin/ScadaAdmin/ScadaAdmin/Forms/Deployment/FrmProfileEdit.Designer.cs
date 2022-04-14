
namespace Scada.Admin.App.Forms.Deployment
{
    partial class FrmProfileEdit
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageGeneral = new System.Windows.Forms.TabPage();
            this.txtWebUrl = new System.Windows.Forms.TextBox();
            this.lblWebUrl = new System.Windows.Forms.Label();
            this.cbExtension = new System.Windows.Forms.ComboBox();
            this.lblExtension = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.pageAgentConnection = new System.Windows.Forms.TabPage();
            this.chkAgentEnabled = new System.Windows.Forms.CheckBox();
            this.ctrlAgentConnection = new Scada.Forms.Controls.CtrlClientConnection();
            this.pageDbConnection = new System.Windows.Forms.TabPage();
            this.chkDbEnabled = new System.Windows.Forms.CheckBox();
            this.ctrlDbConnection = new Scada.Forms.Controls.CtrlDbConnection();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.pageGeneral.SuspendLayout();
            this.pageAgentConnection.SuspendLayout();
            this.pageDbConnection.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageGeneral);
            this.tabControl.Controls.Add(this.pageAgentConnection);
            this.tabControl.Controls.Add(this.pageDbConnection);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(384, 468);
            this.tabControl.TabIndex = 0;
            // 
            // pageGeneral
            // 
            this.pageGeneral.Controls.Add(this.txtWebUrl);
            this.pageGeneral.Controls.Add(this.lblWebUrl);
            this.pageGeneral.Controls.Add(this.cbExtension);
            this.pageGeneral.Controls.Add(this.lblExtension);
            this.pageGeneral.Controls.Add(this.txtName);
            this.pageGeneral.Controls.Add(this.lblName);
            this.pageGeneral.Location = new System.Drawing.Point(4, 24);
            this.pageGeneral.Name = "pageGeneral";
            this.pageGeneral.Padding = new System.Windows.Forms.Padding(5);
            this.pageGeneral.Size = new System.Drawing.Size(376, 440);
            this.pageGeneral.TabIndex = 0;
            this.pageGeneral.Text = "General";
            this.pageGeneral.UseVisualStyleBackColor = true;
            // 
            // txtWebUrl
            // 
            this.txtWebUrl.Location = new System.Drawing.Point(8, 111);
            this.txtWebUrl.Name = "txtWebUrl";
            this.txtWebUrl.Size = new System.Drawing.Size(360, 23);
            this.txtWebUrl.TabIndex = 5;
            // 
            // lblWebUrl
            // 
            this.lblWebUrl.AutoSize = true;
            this.lblWebUrl.Location = new System.Drawing.Point(5, 93);
            this.lblWebUrl.Name = "lblWebUrl";
            this.lblWebUrl.Size = new System.Drawing.Size(91, 15);
            this.lblWebUrl.TabIndex = 4;
            this.lblWebUrl.Text = "Webstation URL";
            // 
            // cbExtension
            // 
            this.cbExtension.FormattingEnabled = true;
            this.cbExtension.Location = new System.Drawing.Point(8, 67);
            this.cbExtension.Name = "cbExtension";
            this.cbExtension.Size = new System.Drawing.Size(360, 23);
            this.cbExtension.Sorted = true;
            this.cbExtension.TabIndex = 3;
            // 
            // lblExtension
            // 
            this.lblExtension.AutoSize = true;
            this.lblExtension.Location = new System.Drawing.Point(5, 49);
            this.lblExtension.Name = "lblExtension";
            this.lblExtension.Size = new System.Drawing.Size(58, 15);
            this.lblExtension.TabIndex = 2;
            this.lblExtension.Text = "Extension";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(8, 23);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(360, 23);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(5, 5);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(74, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Profile name";
            // 
            // pageAgentConnection
            // 
            this.pageAgentConnection.Controls.Add(this.chkAgentEnabled);
            this.pageAgentConnection.Controls.Add(this.ctrlAgentConnection);
            this.pageAgentConnection.Location = new System.Drawing.Point(4, 24);
            this.pageAgentConnection.Name = "pageAgentConnection";
            this.pageAgentConnection.Padding = new System.Windows.Forms.Padding(5);
            this.pageAgentConnection.Size = new System.Drawing.Size(376, 440);
            this.pageAgentConnection.TabIndex = 1;
            this.pageAgentConnection.Text = "Agent Connection";
            this.pageAgentConnection.UseVisualStyleBackColor = true;
            // 
            // chkAgentEnabled
            // 
            this.chkAgentEnabled.AutoSize = true;
            this.chkAgentEnabled.Location = new System.Drawing.Point(8, 8);
            this.chkAgentEnabled.Name = "chkAgentEnabled";
            this.chkAgentEnabled.Size = new System.Drawing.Size(103, 19);
            this.chkAgentEnabled.TabIndex = 0;
            this.chkAgentEnabled.Text = "Agent enabled";
            this.chkAgentEnabled.UseVisualStyleBackColor = true;
            this.chkAgentEnabled.CheckedChanged += new System.EventHandler(this.chkAgentEnabled_CheckedChanged);
            // 
            // ctrlAgentConnection
            // 
            this.ctrlAgentConnection.ConnectionOptions = null;
            this.ctrlAgentConnection.InstanceEnabled = false;
            this.ctrlAgentConnection.Location = new System.Drawing.Point(8, 33);
            this.ctrlAgentConnection.Name = "ctrlAgentConnection";
            this.ctrlAgentConnection.NameEnabled = false;
            this.ctrlAgentConnection.Size = new System.Drawing.Size(360, 366);
            this.ctrlAgentConnection.TabIndex = 0;
            // 
            // pageDbConnection
            // 
            this.pageDbConnection.Controls.Add(this.chkDbEnabled);
            this.pageDbConnection.Controls.Add(this.ctrlDbConnection);
            this.pageDbConnection.Location = new System.Drawing.Point(4, 24);
            this.pageDbConnection.Name = "pageDbConnection";
            this.pageDbConnection.Padding = new System.Windows.Forms.Padding(5);
            this.pageDbConnection.Size = new System.Drawing.Size(376, 440);
            this.pageDbConnection.TabIndex = 2;
            this.pageDbConnection.Text = "DB Connection";
            this.pageDbConnection.UseVisualStyleBackColor = true;
            // 
            // chkDbEnabled
            // 
            this.chkDbEnabled.AutoSize = true;
            this.chkDbEnabled.Location = new System.Drawing.Point(8, 8);
            this.chkDbEnabled.Name = "chkDbEnabled";
            this.chkDbEnabled.Size = new System.Drawing.Size(119, 19);
            this.chkDbEnabled.TabIndex = 0;
            this.chkDbEnabled.Text = "Database enabled";
            this.chkDbEnabled.UseVisualStyleBackColor = true;
            this.chkDbEnabled.CheckedChanged += new System.EventHandler(this.chkDbEnabled_CheckedChanged);
            // 
            // ctrlDbConnection
            // 
            this.ctrlDbConnection.BuildConnectionStringFunc = null;
            this.ctrlDbConnection.ConnectionOptions = null;
            this.ctrlDbConnection.DbmsEnabled = false;
            this.ctrlDbConnection.Location = new System.Drawing.Point(8, 33);
            this.ctrlDbConnection.Name = "ctrlDbConnection";
            this.ctrlDbConnection.NameEnabled = false;
            this.ctrlDbConnection.Size = new System.Drawing.Size(360, 399);
            this.ctrlDbConnection.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(216, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(297, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 468);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(384, 41);
            this.pnlBottom.TabIndex = 1;
            // 
            // FrmProfileEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 509);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProfileEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Profile";
            this.Load += new System.EventHandler(this.FrmProfileEdit_Load);
            this.tabControl.ResumeLayout(false);
            this.pageGeneral.ResumeLayout(false);
            this.pageGeneral.PerformLayout();
            this.pageAgentConnection.ResumeLayout(false);
            this.pageAgentConnection.PerformLayout();
            this.pageDbConnection.ResumeLayout(false);
            this.pageDbConnection.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageGeneral;
        private System.Windows.Forms.TabPage pageAgentConnection;
        private System.Windows.Forms.TabPage pageDbConnection;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cbExtension;
        private System.Windows.Forms.Label lblExtension;
        private System.Windows.Forms.TextBox txtWebUrl;
        private System.Windows.Forms.Label lblWebUrl;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkAgentEnabled;
        private Scada.Forms.Controls.CtrlClientConnection ctrlAgentConnection;
        private System.Windows.Forms.CheckBox chkDbEnabled;
        private Scada.Forms.Controls.CtrlDbConnection ctrlDbConnection;
        private System.Windows.Forms.Panel pnlBottom;
    }
}