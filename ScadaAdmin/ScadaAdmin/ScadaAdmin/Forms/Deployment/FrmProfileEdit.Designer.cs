
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
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.txtWebUrl = new System.Windows.Forms.TextBox();
            this.lblWebUrl = new System.Windows.Forms.Label();
            this.cbExtension = new System.Windows.Forms.ComboBox();
            this.lblExtension = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tabAgentConnection = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAgentEnabled = new System.Windows.Forms.CheckBox();
            this.tabDbConnection = new System.Windows.Forms.TabPage();
            this.chkDbEnabled = new System.Windows.Forms.CheckBox();
            this.ctrlDbConnection = new Scada.Forms.CtrlDbConnection();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabAgentConnection.SuspendLayout();
            this.tabDbConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabAgentConnection);
            this.tabControl.Controls.Add(this.tabDbConnection);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(384, 464);
            this.tabControl.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.txtWebUrl);
            this.tabGeneral.Controls.Add(this.lblWebUrl);
            this.tabGeneral.Controls.Add(this.cbExtension);
            this.tabGeneral.Controls.Add(this.lblExtension);
            this.tabGeneral.Controls.Add(this.txtName);
            this.tabGeneral.Controls.Add(this.lblName);
            this.tabGeneral.Location = new System.Drawing.Point(4, 24);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(376, 436);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // txtWebUrl
            // 
            this.txtWebUrl.Location = new System.Drawing.Point(6, 109);
            this.txtWebUrl.Name = "txtWebUrl";
            this.txtWebUrl.Size = new System.Drawing.Size(364, 23);
            this.txtWebUrl.TabIndex = 5;
            // 
            // lblWebUrl
            // 
            this.lblWebUrl.AutoSize = true;
            this.lblWebUrl.Location = new System.Drawing.Point(3, 91);
            this.lblWebUrl.Name = "lblWebUrl";
            this.lblWebUrl.Size = new System.Drawing.Size(91, 15);
            this.lblWebUrl.TabIndex = 4;
            this.lblWebUrl.Text = "Webstation URL";
            // 
            // cbExtension
            // 
            this.cbExtension.FormattingEnabled = true;
            this.cbExtension.Location = new System.Drawing.Point(6, 65);
            this.cbExtension.Name = "cbExtension";
            this.cbExtension.Size = new System.Drawing.Size(364, 23);
            this.cbExtension.TabIndex = 3;
            // 
            // lblExtension
            // 
            this.lblExtension.AutoSize = true;
            this.lblExtension.Location = new System.Drawing.Point(3, 47);
            this.lblExtension.Name = "lblExtension";
            this.lblExtension.Size = new System.Drawing.Size(58, 15);
            this.lblExtension.TabIndex = 2;
            this.lblExtension.Text = "Extension";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(6, 21);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(364, 23);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(74, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Profile name";
            // 
            // tabAgentConnection
            // 
            this.tabAgentConnection.Controls.Add(this.groupBox1);
            this.tabAgentConnection.Controls.Add(this.chkAgentEnabled);
            this.tabAgentConnection.Location = new System.Drawing.Point(4, 24);
            this.tabAgentConnection.Name = "tabAgentConnection";
            this.tabAgentConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabAgentConnection.Size = new System.Drawing.Size(376, 436);
            this.tabAgentConnection.TabIndex = 1;
            this.tabAgentConnection.Text = "Agent Connection";
            this.tabAgentConnection.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(6, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 399);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // chkAgentEnabled
            // 
            this.chkAgentEnabled.AutoSize = true;
            this.chkAgentEnabled.Location = new System.Drawing.Point(6, 6);
            this.chkAgentEnabled.Name = "chkAgentEnabled";
            this.chkAgentEnabled.Size = new System.Drawing.Size(103, 19);
            this.chkAgentEnabled.TabIndex = 0;
            this.chkAgentEnabled.Text = "Agent enabled";
            this.chkAgentEnabled.UseVisualStyleBackColor = true;
            this.chkAgentEnabled.CheckedChanged += new System.EventHandler(this.chkAgentEnabled_CheckedChanged);
            // 
            // tabDbConnection
            // 
            this.tabDbConnection.Controls.Add(this.chkDbEnabled);
            this.tabDbConnection.Controls.Add(this.ctrlDbConnection);
            this.tabDbConnection.Location = new System.Drawing.Point(4, 24);
            this.tabDbConnection.Name = "tabDbConnection";
            this.tabDbConnection.Size = new System.Drawing.Size(376, 436);
            this.tabDbConnection.TabIndex = 2;
            this.tabDbConnection.Text = "DB Connection";
            this.tabDbConnection.UseVisualStyleBackColor = true;
            // 
            // chkDbEnabled
            // 
            this.chkDbEnabled.AutoSize = true;
            this.chkDbEnabled.Location = new System.Drawing.Point(6, 6);
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
            this.ctrlDbConnection.Location = new System.Drawing.Point(6, 31);
            this.ctrlDbConnection.Name = "ctrlDbConnection";
            this.ctrlDbConnection.NameEnabled = false;
            this.ctrlDbConnection.Size = new System.Drawing.Size(364, 399);
            this.ctrlDbConnection.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 470);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 470);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmProfileEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 505);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProfileEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Profile";
            this.Load += new System.EventHandler(this.FrmProfileEdit_Load);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabAgentConnection.ResumeLayout(false);
            this.tabAgentConnection.PerformLayout();
            this.tabDbConnection.ResumeLayout(false);
            this.tabDbConnection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabAgentConnection;
        private System.Windows.Forms.TabPage tabDbConnection;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cbExtension;
        private System.Windows.Forms.Label lblExtension;
        private System.Windows.Forms.TextBox txtWebUrl;
        private System.Windows.Forms.Label lblWebUrl;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkAgentEnabled;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkDbEnabled;
        private Scada.Forms.CtrlDbConnection ctrlDbConnection;
    }
}