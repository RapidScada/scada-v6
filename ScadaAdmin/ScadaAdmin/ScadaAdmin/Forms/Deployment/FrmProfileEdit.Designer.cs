
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
            tabControl = new System.Windows.Forms.TabControl();
            pageGeneral = new System.Windows.Forms.TabPage();
            txtWebUrl = new System.Windows.Forms.TextBox();
            lblWebUrl = new System.Windows.Forms.Label();
            cbExtension = new System.Windows.Forms.ComboBox();
            lblExtension = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            lblName = new System.Windows.Forms.Label();
            pageAgentConnection = new System.Windows.Forms.TabPage();
            chkAgentEnabled = new System.Windows.Forms.CheckBox();
            ctrlAgentConnection = new Scada.Forms.Controls.CtrlClientConnection();
            pageDbConnection = new System.Windows.Forms.TabPage();
            chkDbEnabled = new System.Windows.Forms.CheckBox();
            ctrlDbConnection = new Scada.Forms.Controls.CtrlDbConnection();
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            pnlBottom = new System.Windows.Forms.Panel();
            tabControl.SuspendLayout();
            pageGeneral.SuspendLayout();
            pageAgentConnection.SuspendLayout();
            pageDbConnection.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(pageGeneral);
            tabControl.Controls.Add(pageAgentConnection);
            tabControl.Controls.Add(pageDbConnection);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(384, 468);
            tabControl.TabIndex = 0;
            // 
            // pageGeneral
            // 
            pageGeneral.Controls.Add(txtWebUrl);
            pageGeneral.Controls.Add(lblWebUrl);
            pageGeneral.Controls.Add(cbExtension);
            pageGeneral.Controls.Add(lblExtension);
            pageGeneral.Controls.Add(txtName);
            pageGeneral.Controls.Add(lblName);
            pageGeneral.Location = new System.Drawing.Point(4, 24);
            pageGeneral.Name = "pageGeneral";
            pageGeneral.Padding = new System.Windows.Forms.Padding(5);
            pageGeneral.Size = new System.Drawing.Size(376, 440);
            pageGeneral.TabIndex = 0;
            pageGeneral.Text = "General";
            pageGeneral.UseVisualStyleBackColor = true;
            // 
            // txtWebUrl
            // 
            txtWebUrl.Location = new System.Drawing.Point(8, 111);
            txtWebUrl.Name = "txtWebUrl";
            txtWebUrl.Size = new System.Drawing.Size(360, 23);
            txtWebUrl.TabIndex = 5;
            // 
            // lblWebUrl
            // 
            lblWebUrl.AutoSize = true;
            lblWebUrl.Location = new System.Drawing.Point(5, 93);
            lblWebUrl.Name = "lblWebUrl";
            lblWebUrl.Size = new System.Drawing.Size(91, 15);
            lblWebUrl.TabIndex = 4;
            lblWebUrl.Text = "Webstation URL";
            // 
            // cbExtension
            // 
            cbExtension.FormattingEnabled = true;
            cbExtension.Location = new System.Drawing.Point(8, 67);
            cbExtension.Name = "cbExtension";
            cbExtension.Size = new System.Drawing.Size(360, 23);
            cbExtension.Sorted = true;
            cbExtension.TabIndex = 3;
            // 
            // lblExtension
            // 
            lblExtension.AutoSize = true;
            lblExtension.Location = new System.Drawing.Point(5, 49);
            lblExtension.Name = "lblExtension";
            lblExtension.Size = new System.Drawing.Size(58, 15);
            lblExtension.TabIndex = 2;
            lblExtension.Text = "Extension";
            // 
            // txtName
            // 
            txtName.Location = new System.Drawing.Point(8, 23);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(360, 23);
            txtName.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(5, 5);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(74, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Profile name";
            // 
            // pageAgentConnection
            // 
            pageAgentConnection.Controls.Add(chkAgentEnabled);
            pageAgentConnection.Controls.Add(ctrlAgentConnection);
            pageAgentConnection.Location = new System.Drawing.Point(4, 24);
            pageAgentConnection.Name = "pageAgentConnection";
            pageAgentConnection.Padding = new System.Windows.Forms.Padding(5);
            pageAgentConnection.Size = new System.Drawing.Size(376, 440);
            pageAgentConnection.TabIndex = 1;
            pageAgentConnection.Text = "Agent Connection";
            pageAgentConnection.UseVisualStyleBackColor = true;
            // 
            // chkAgentEnabled
            // 
            chkAgentEnabled.AutoSize = true;
            chkAgentEnabled.Location = new System.Drawing.Point(8, 8);
            chkAgentEnabled.Name = "chkAgentEnabled";
            chkAgentEnabled.Size = new System.Drawing.Size(103, 19);
            chkAgentEnabled.TabIndex = 0;
            chkAgentEnabled.Text = "Agent enabled";
            chkAgentEnabled.UseVisualStyleBackColor = true;
            chkAgentEnabled.CheckedChanged += chkAgentEnabled_CheckedChanged;
            // 
            // ctrlAgentConnection
            // 
            ctrlAgentConnection.ConnectionOptions = null;
            ctrlAgentConnection.InstanceEnabled = false;
            ctrlAgentConnection.Location = new System.Drawing.Point(8, 33);
            ctrlAgentConnection.Name = "ctrlAgentConnection";
            ctrlAgentConnection.NameEnabled = false;
            ctrlAgentConnection.Size = new System.Drawing.Size(360, 399);
            ctrlAgentConnection.TabIndex = 0;
            // 
            // pageDbConnection
            // 
            pageDbConnection.Controls.Add(chkDbEnabled);
            pageDbConnection.Controls.Add(ctrlDbConnection);
            pageDbConnection.Location = new System.Drawing.Point(4, 24);
            pageDbConnection.Name = "pageDbConnection";
            pageDbConnection.Padding = new System.Windows.Forms.Padding(5);
            pageDbConnection.Size = new System.Drawing.Size(376, 440);
            pageDbConnection.TabIndex = 2;
            pageDbConnection.Text = "DB Connection";
            pageDbConnection.UseVisualStyleBackColor = true;
            // 
            // chkDbEnabled
            // 
            chkDbEnabled.AutoSize = true;
            chkDbEnabled.Location = new System.Drawing.Point(8, 8);
            chkDbEnabled.Name = "chkDbEnabled";
            chkDbEnabled.Size = new System.Drawing.Size(119, 19);
            chkDbEnabled.TabIndex = 0;
            chkDbEnabled.Text = "Database enabled";
            chkDbEnabled.UseVisualStyleBackColor = true;
            chkDbEnabled.CheckedChanged += chkDbEnabled_CheckedChanged;
            // 
            // ctrlDbConnection
            // 
            ctrlDbConnection.BuildConnectionStringFunc = null;
            ctrlDbConnection.ConnectionOptions = null;
            ctrlDbConnection.DbmsEnabled = false;
            ctrlDbConnection.Location = new System.Drawing.Point(8, 33);
            ctrlDbConnection.Name = "ctrlDbConnection";
            ctrlDbConnection.NameEnabled = false;
            ctrlDbConnection.Size = new System.Drawing.Size(360, 399);
            ctrlDbConnection.TabIndex = 0;
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnOK.Location = new System.Drawing.Point(216, 6);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(297, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 468);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(384, 41);
            pnlBottom.TabIndex = 1;
            // 
            // FrmProfileEdit
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(384, 509);
            Controls.Add(tabControl);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmProfileEdit";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Edit Profile";
            Load += FrmProfileEdit_Load;
            tabControl.ResumeLayout(false);
            pageGeneral.ResumeLayout(false);
            pageGeneral.PerformLayout();
            pageAgentConnection.ResumeLayout(false);
            pageAgentConnection.PerformLayout();
            pageDbConnection.ResumeLayout(false);
            pageDbConnection.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
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