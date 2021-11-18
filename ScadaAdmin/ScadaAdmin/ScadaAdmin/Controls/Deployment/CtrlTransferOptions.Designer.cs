namespace Scada.Admin.App.Controls.Deployment
{
    partial class CtrlTransferOptions
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
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.btnSelectObj = new System.Windows.Forms.Button();
            this.txtObjFilter = new System.Windows.Forms.TextBox();
            this.lblObjFilter = new System.Windows.Forms.Label();
            this.chkIgnoreRegKeys = new System.Windows.Forms.CheckBox();
            this.lblIgnore = new System.Windows.Forms.Label();
            this.chkRestartWeb = new System.Windows.Forms.CheckBox();
            this.chkIncludeWeb = new System.Windows.Forms.CheckBox();
            this.chkRestartComm = new System.Windows.Forms.CheckBox();
            this.chkIncludeComm = new System.Windows.Forms.CheckBox();
            this.chkRestartServer = new System.Windows.Forms.CheckBox();
            this.chkIncludeServer = new System.Windows.Forms.CheckBox();
            this.chkIncludeView = new System.Windows.Forms.CheckBox();
            this.chkIncludeBase = new System.Windows.Forms.CheckBox();
            this.lblInclude = new System.Windows.Forms.Label();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.btnSelectObj);
            this.gbOptions.Controls.Add(this.txtObjFilter);
            this.gbOptions.Controls.Add(this.lblObjFilter);
            this.gbOptions.Controls.Add(this.chkIgnoreRegKeys);
            this.gbOptions.Controls.Add(this.lblIgnore);
            this.gbOptions.Controls.Add(this.chkRestartWeb);
            this.gbOptions.Controls.Add(this.chkIncludeWeb);
            this.gbOptions.Controls.Add(this.chkRestartComm);
            this.gbOptions.Controls.Add(this.chkIncludeComm);
            this.gbOptions.Controls.Add(this.chkRestartServer);
            this.gbOptions.Controls.Add(this.chkIncludeServer);
            this.gbOptions.Controls.Add(this.chkIncludeView);
            this.gbOptions.Controls.Add(this.chkIncludeBase);
            this.gbOptions.Controls.Add(this.lblInclude);
            this.gbOptions.Location = new System.Drawing.Point(0, 0);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(469, 273);
            this.gbOptions.TabIndex = 0;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // btnSelectObj
            // 
            this.btnSelectObj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectObj.Location = new System.Drawing.Point(381, 237);
            this.btnSelectObj.Name = "btnSelectObj";
            this.btnSelectObj.Size = new System.Drawing.Size(75, 23);
            this.btnSelectObj.TabIndex = 13;
            this.btnSelectObj.Text = "Select...";
            this.btnSelectObj.UseVisualStyleBackColor = true;
            this.btnSelectObj.Click += new System.EventHandler(this.btnSelectObj_Click);
            // 
            // txtObjFilter
            // 
            this.txtObjFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjFilter.Location = new System.Drawing.Point(13, 237);
            this.txtObjFilter.Name = "txtObjFilter";
            this.txtObjFilter.Size = new System.Drawing.Size(362, 23);
            this.txtObjFilter.TabIndex = 12;
            this.txtObjFilter.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblObjFilter
            // 
            this.lblObjFilter.AutoSize = true;
            this.lblObjFilter.Location = new System.Drawing.Point(10, 219);
            this.lblObjFilter.Name = "lblObjFilter";
            this.lblObjFilter.Size = new System.Drawing.Size(72, 15);
            this.lblObjFilter.TabIndex = 11;
            this.lblObjFilter.Text = "Object filter:";
            // 
            // chkIgnoreRegKeys
            // 
            this.chkIgnoreRegKeys.AutoSize = true;
            this.chkIgnoreRegKeys.Location = new System.Drawing.Point(13, 187);
            this.chkIgnoreRegKeys.Name = "chkIgnoreRegKeys";
            this.chkIgnoreRegKeys.Size = new System.Drawing.Size(115, 19);
            this.chkIgnoreRegKeys.TabIndex = 10;
            this.chkIgnoreRegKeys.Text = "Registration keys";
            this.chkIgnoreRegKeys.UseVisualStyleBackColor = true;
            this.chkIgnoreRegKeys.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblIgnore
            // 
            this.lblIgnore.AutoSize = true;
            this.lblIgnore.Location = new System.Drawing.Point(10, 169);
            this.lblIgnore.Name = "lblIgnore";
            this.lblIgnore.Size = new System.Drawing.Size(44, 15);
            this.lblIgnore.TabIndex = 9;
            this.lblIgnore.Text = "Ignore:";
            // 
            // chkRestartWeb
            // 
            this.chkRestartWeb.AutoSize = true;
            this.chkRestartWeb.Location = new System.Drawing.Point(150, 137);
            this.chkRestartWeb.Name = "chkRestartWeb";
            this.chkRestartWeb.Size = new System.Drawing.Size(125, 19);
            this.chkRestartWeb.TabIndex = 8;
            this.chkRestartWeb.Text = "Restart Webstation";
            this.chkRestartWeb.UseVisualStyleBackColor = true;
            this.chkRestartWeb.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeWeb
            // 
            this.chkIncludeWeb.AutoSize = true;
            this.chkIncludeWeb.Location = new System.Drawing.Point(13, 137);
            this.chkIncludeWeb.Name = "chkIncludeWeb";
            this.chkIncludeWeb.Size = new System.Drawing.Size(86, 19);
            this.chkIncludeWeb.TabIndex = 7;
            this.chkIncludeWeb.Text = "Webstation";
            this.chkIncludeWeb.UseVisualStyleBackColor = true;
            this.chkIncludeWeb.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkRestartComm
            // 
            this.chkRestartComm.AutoSize = true;
            this.chkRestartComm.Location = new System.Drawing.Point(150, 112);
            this.chkRestartComm.Name = "chkRestartComm";
            this.chkRestartComm.Size = new System.Drawing.Size(146, 19);
            this.chkRestartComm.TabIndex = 6;
            this.chkRestartComm.Text = "Restart Communicator";
            this.chkRestartComm.UseVisualStyleBackColor = true;
            this.chkRestartComm.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeComm
            // 
            this.chkIncludeComm.AutoSize = true;
            this.chkIncludeComm.Location = new System.Drawing.Point(13, 112);
            this.chkIncludeComm.Name = "chkIncludeComm";
            this.chkIncludeComm.Size = new System.Drawing.Size(107, 19);
            this.chkIncludeComm.TabIndex = 5;
            this.chkIncludeComm.Text = "Communicator";
            this.chkIncludeComm.UseVisualStyleBackColor = true;
            this.chkIncludeComm.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkRestartServer
            // 
            this.chkRestartServer.AutoSize = true;
            this.chkRestartServer.Location = new System.Drawing.Point(150, 87);
            this.chkRestartServer.Name = "chkRestartServer";
            this.chkRestartServer.Size = new System.Drawing.Size(97, 19);
            this.chkRestartServer.TabIndex = 4;
            this.chkRestartServer.Text = "Restart Server";
            this.chkRestartServer.UseVisualStyleBackColor = true;
            this.chkRestartServer.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeServer
            // 
            this.chkIncludeServer.AutoSize = true;
            this.chkIncludeServer.Location = new System.Drawing.Point(13, 87);
            this.chkIncludeServer.Name = "chkIncludeServer";
            this.chkIncludeServer.Size = new System.Drawing.Size(58, 19);
            this.chkIncludeServer.TabIndex = 3;
            this.chkIncludeServer.Text = "Server";
            this.chkIncludeServer.UseVisualStyleBackColor = true;
            this.chkIncludeServer.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeView
            // 
            this.chkIncludeView.AutoSize = true;
            this.chkIncludeView.Location = new System.Drawing.Point(13, 62);
            this.chkIncludeView.Name = "chkIncludeView";
            this.chkIncludeView.Size = new System.Drawing.Size(56, 19);
            this.chkIncludeView.TabIndex = 2;
            this.chkIncludeView.Text = "Views";
            this.chkIncludeView.UseVisualStyleBackColor = true;
            this.chkIncludeView.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeBase
            // 
            this.chkIncludeBase.AutoSize = true;
            this.chkIncludeBase.Location = new System.Drawing.Point(13, 37);
            this.chkIncludeBase.Name = "chkIncludeBase";
            this.chkIncludeBase.Size = new System.Drawing.Size(150, 19);
            this.chkIncludeBase.TabIndex = 1;
            this.chkIncludeBase.Text = "Configuration database";
            this.chkIncludeBase.UseVisualStyleBackColor = true;
            this.chkIncludeBase.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblInclude
            // 
            this.lblInclude.AutoSize = true;
            this.lblInclude.Location = new System.Drawing.Point(10, 19);
            this.lblInclude.Name = "lblInclude";
            this.lblInclude.Size = new System.Drawing.Size(49, 15);
            this.lblInclude.TabIndex = 0;
            this.lblInclude.Text = "Include:";
            // 
            // CtrlTransferOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOptions);
            this.Name = "CtrlTransferOptions";
            this.Size = new System.Drawing.Size(469, 273);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.CheckBox chkIgnoreRegKeys;
        private System.Windows.Forms.Label lblIgnore;
        private System.Windows.Forms.Label lblInclude;
        private System.Windows.Forms.CheckBox chkIncludeWeb;
        private System.Windows.Forms.CheckBox chkIncludeComm;
        private System.Windows.Forms.CheckBox chkIncludeServer;
        private System.Windows.Forms.CheckBox chkIncludeView;
        private System.Windows.Forms.CheckBox chkIncludeBase;
        private System.Windows.Forms.TextBox txtObjFilter;
        private System.Windows.Forms.Label lblObjFilter;
        private System.Windows.Forms.CheckBox chkRestartComm;
        private System.Windows.Forms.CheckBox chkRestartServer;
        private System.Windows.Forms.Button btnSelectObj;
        private System.Windows.Forms.CheckBox chkRestartWeb;
    }
}
