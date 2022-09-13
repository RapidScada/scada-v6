namespace Scada.Comm.Drivers.DrvOpcUa.View.Forms
{
    partial class FrmDeviceConfig
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
            this.components = new System.ComponentModel.Container();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.tvDevice = new System.Windows.Forms.TreeView();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnMoveDownItem = new System.Windows.Forms.Button();
            this.btnMoveUpItem = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnAddSubscription = new System.Windows.Forms.Button();
            this.gbServerBrowse = new System.Windows.Forms.GroupBox();
            this.tvServer = new System.Windows.Forms.TreeView();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnViewAttrs = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gbConnectionOptions = new System.Windows.Forms.GroupBox();
            this.btnSecurityOptions = new System.Windows.Forms.Button();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ctrlItem = new Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlItem();
            this.ctrlSubscription = new Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlSubscription();
            this.btnEditingOptions = new System.Windows.Forms.Button();
            this.ctrlCommand = new Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlCommand();
            this.ctrlEmptyItem = new Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlEmptyItem();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblConnectionInfo = new System.Windows.Forms.Label();
            this.pbConnectionInfo = new System.Windows.Forms.PictureBox();
            this.gbDevice.SuspendLayout();
            this.gbServerBrowse.SuspendLayout();
            this.gbConnectionOptions.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectionInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(797, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(716, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbDevice
            // 
            this.gbDevice.Controls.Add(this.tvDevice);
            this.gbDevice.Controls.Add(this.btnDeleteItem);
            this.gbDevice.Controls.Add(this.btnMoveDownItem);
            this.gbDevice.Controls.Add(this.btnMoveUpItem);
            this.gbDevice.Controls.Add(this.btnAddItem);
            this.gbDevice.Controls.Add(this.btnAddSubscription);
            this.gbDevice.Location = new System.Drawing.Point(317, 113);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevice.Size = new System.Drawing.Size(299, 500);
            this.gbDevice.TabIndex = 2;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Device Configuration";
            // 
            // tvDevice
            // 
            this.tvDevice.HideSelection = false;
            this.tvDevice.ImageIndex = 0;
            this.tvDevice.ImageList = this.ilTree;
            this.tvDevice.Location = new System.Drawing.Point(13, 51);
            this.tvDevice.Name = "tvDevice";
            this.tvDevice.SelectedImageIndex = 0;
            this.tvDevice.Size = new System.Drawing.Size(273, 436);
            this.tvDevice.TabIndex = 5;
            this.tvDevice.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterCollapse);
            this.tvDevice.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterExpand);
            this.tvDevice.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterSelect);
            // 
            // ilTree
            // 
            this.ilTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ilTree.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeleteItem.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.delete;
            this.btnDeleteItem.Location = new System.Drawing.Point(129, 22);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(23, 23);
            this.btnDeleteItem.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnDeleteItem, "Delete");
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnMoveDownItem
            // 
            this.btnMoveDownItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveDownItem.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.move_down;
            this.btnMoveDownItem.Location = new System.Drawing.Point(100, 22);
            this.btnMoveDownItem.Name = "btnMoveDownItem";
            this.btnMoveDownItem.Size = new System.Drawing.Size(23, 23);
            this.btnMoveDownItem.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnMoveDownItem, "Move Down");
            this.btnMoveDownItem.UseVisualStyleBackColor = true;
            this.btnMoveDownItem.Click += new System.EventHandler(this.btnMoveDownItem_Click);
            // 
            // btnMoveUpItem
            // 
            this.btnMoveUpItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveUpItem.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.move_up;
            this.btnMoveUpItem.Location = new System.Drawing.Point(71, 22);
            this.btnMoveUpItem.Name = "btnMoveUpItem";
            this.btnMoveUpItem.Size = new System.Drawing.Size(23, 23);
            this.btnMoveUpItem.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnMoveUpItem, "Move Up");
            this.btnMoveUpItem.UseVisualStyleBackColor = true;
            this.btnMoveUpItem.Click += new System.EventHandler(this.btnMoveUpItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddItem.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.add;
            this.btnAddItem.Location = new System.Drawing.Point(42, 22);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(23, 23);
            this.btnAddItem.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnAddItem, "Add Selected Item");
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnAddSubscription
            // 
            this.btnAddSubscription.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddSubscription.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.folder_add;
            this.btnAddSubscription.Location = new System.Drawing.Point(13, 22);
            this.btnAddSubscription.Name = "btnAddSubscription";
            this.btnAddSubscription.Size = new System.Drawing.Size(23, 23);
            this.btnAddSubscription.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnAddSubscription, "Add Subscription");
            this.btnAddSubscription.UseVisualStyleBackColor = true;
            this.btnAddSubscription.Click += new System.EventHandler(this.btnAddSubscription_Click);
            // 
            // gbServerBrowse
            // 
            this.gbServerBrowse.Controls.Add(this.tvServer);
            this.gbServerBrowse.Controls.Add(this.btnDisconnect);
            this.gbServerBrowse.Controls.Add(this.btnViewAttrs);
            this.gbServerBrowse.Controls.Add(this.btnConnect);
            this.gbServerBrowse.Location = new System.Drawing.Point(12, 113);
            this.gbServerBrowse.Name = "gbServerBrowse";
            this.gbServerBrowse.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbServerBrowse.Size = new System.Drawing.Size(299, 500);
            this.gbServerBrowse.TabIndex = 1;
            this.gbServerBrowse.TabStop = false;
            this.gbServerBrowse.Text = "Server Browse";
            // 
            // tvServer
            // 
            this.tvServer.HideSelection = false;
            this.tvServer.ImageIndex = 0;
            this.tvServer.ImageList = this.ilTree;
            this.tvServer.Location = new System.Drawing.Point(13, 51);
            this.tvServer.Name = "tvServer";
            this.tvServer.SelectedImageIndex = 0;
            this.tvServer.Size = new System.Drawing.Size(273, 436);
            this.tvServer.TabIndex = 3;
            this.tvServer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvServer_BeforeExpand);
            this.tvServer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvServer_AfterSelect);
            this.tvServer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvServer_NodeMouseDoubleClick);
            this.tvServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvServer_KeyDown);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDisconnect.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.disconnect;
            this.btnDisconnect.Location = new System.Drawing.Point(42, 22);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(23, 23);
            this.btnDisconnect.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnDisconnect, "Disconnect from Server");
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnViewAttrs
            // 
            this.btnViewAttrs.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnViewAttrs.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.attributes;
            this.btnViewAttrs.Location = new System.Drawing.Point(71, 22);
            this.btnViewAttrs.Name = "btnViewAttrs";
            this.btnViewAttrs.Size = new System.Drawing.Size(23, 23);
            this.btnViewAttrs.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnViewAttrs, "View Attributes");
            this.btnViewAttrs.UseVisualStyleBackColor = true;
            this.btnViewAttrs.Click += new System.EventHandler(this.btnViewAttrs_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnect.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.connect;
            this.btnConnect.Location = new System.Drawing.Point(13, 22);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(23, 23);
            this.btnConnect.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnConnect, "Connect to Server");
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_ClickAsync);
            // 
            // gbConnectionOptions
            // 
            this.gbConnectionOptions.Controls.Add(this.lblConnectionInfo);
            this.gbConnectionOptions.Controls.Add(this.pbConnectionInfo);
            this.gbConnectionOptions.Controls.Add(this.btnSecurityOptions);
            this.gbConnectionOptions.Controls.Add(this.txtServerUrl);
            this.gbConnectionOptions.Controls.Add(this.lblServerUrl);
            this.gbConnectionOptions.Location = new System.Drawing.Point(12, 12);
            this.gbConnectionOptions.Name = "gbConnectionOptions";
            this.gbConnectionOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnectionOptions.Size = new System.Drawing.Size(860, 95);
            this.gbConnectionOptions.TabIndex = 0;
            this.gbConnectionOptions.TabStop = false;
            this.gbConnectionOptions.Text = "Connection Options";
            // 
            // btnSecurityOptions
            // 
            this.btnSecurityOptions.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSecurityOptions.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources._lock;
            this.btnSecurityOptions.Location = new System.Drawing.Point(824, 37);
            this.btnSecurityOptions.Name = "btnSecurityOptions";
            this.btnSecurityOptions.Size = new System.Drawing.Size(23, 23);
            this.btnSecurityOptions.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnSecurityOptions, "Security Options");
            this.btnSecurityOptions.UseVisualStyleBackColor = true;
            this.btnSecurityOptions.Click += new System.EventHandler(this.btnSecurityOptions_Click);
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Location = new System.Drawing.Point(13, 37);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Size = new System.Drawing.Size(805, 23);
            this.txtServerUrl.TabIndex = 1;
            this.txtServerUrl.TextChanged += new System.EventHandler(this.txtServerUrl_TextChanged);
            // 
            // lblServerUrl
            // 
            this.lblServerUrl.AutoSize = true;
            this.lblServerUrl.Location = new System.Drawing.Point(10, 19);
            this.lblServerUrl.Name = "lblServerUrl";
            this.lblServerUrl.Size = new System.Drawing.Size(63, 15);
            this.lblServerUrl.TabIndex = 0;
            this.lblServerUrl.Text = "Server URL";
            // 
            // ctrlItem
            // 
            this.ctrlItem.ItemConfig = null;
            this.ctrlItem.Location = new System.Drawing.Point(622, 250);
            this.ctrlItem.Name = "ctrlItem";
            this.ctrlItem.Size = new System.Drawing.Size(250, 500);
            this.ctrlItem.TabIndex = 5;
            this.ctrlItem.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlItem_ObjectChanged);
            // 
            // ctrlSubscription
            // 
            this.ctrlSubscription.Location = new System.Drawing.Point(622, 200);
            this.ctrlSubscription.Name = "ctrlSubscription";
            this.ctrlSubscription.Size = new System.Drawing.Size(250, 500);
            this.ctrlSubscription.SubscriptionConfig = null;
            this.ctrlSubscription.TabIndex = 4;
            this.ctrlSubscription.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlItem_ObjectChanged);
            // 
            // btnEditingOptions
            // 
            this.btnEditingOptions.Location = new System.Drawing.Point(12, 10);
            this.btnEditingOptions.Name = "btnEditingOptions";
            this.btnEditingOptions.Size = new System.Drawing.Size(100, 23);
            this.btnEditingOptions.TabIndex = 0;
            this.btnEditingOptions.Text = "Options";
            this.btnEditingOptions.UseVisualStyleBackColor = true;
            this.btnEditingOptions.Click += new System.EventHandler(this.btnEditingOptions_Click);
            // 
            // ctrlCommand
            // 
            this.ctrlCommand.CommandConfig = null;
            this.ctrlCommand.Location = new System.Drawing.Point(622, 300);
            this.ctrlCommand.Name = "ctrlCommand";
            this.ctrlCommand.Size = new System.Drawing.Size(250, 500);
            this.ctrlCommand.TabIndex = 6;
            this.ctrlCommand.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlItem_ObjectChanged);
            // 
            // ctrlEmptyItem
            // 
            this.ctrlEmptyItem.Location = new System.Drawing.Point(622, 113);
            this.ctrlEmptyItem.Name = "ctrlEmptyItem";
            this.ctrlEmptyItem.Size = new System.Drawing.Size(250, 500);
            this.ctrlEmptyItem.TabIndex = 3;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnEditingOptions);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 619);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(884, 45);
            this.pnlBottom.TabIndex = 7;
            // 
            // lblConnectionInfo
            // 
            this.lblConnectionInfo.AutoSize = true;
            this.lblConnectionInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblConnectionInfo.Location = new System.Drawing.Point(35, 67);
            this.lblConnectionInfo.Name = "lblConnectionInfo";
            this.lblConnectionInfo.Size = new System.Drawing.Size(330, 15);
            this.lblConnectionInfo.TabIndex = 3;
            this.lblConnectionInfo.Text = "Connection options are common to the communication line.";
            // 
            // pbConnectionInfo
            // 
            this.pbConnectionInfo.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.info;
            this.pbConnectionInfo.Location = new System.Drawing.Point(13, 66);
            this.pbConnectionInfo.Name = "pbConnectionInfo";
            this.pbConnectionInfo.Size = new System.Drawing.Size(16, 16);
            this.pbConnectionInfo.TabIndex = 12;
            this.pbConnectionInfo.TabStop = false;
            // 
            // FrmDeviceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(884, 664);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.ctrlCommand);
            this.Controls.Add(this.ctrlItem);
            this.Controls.Add(this.ctrlSubscription);
            this.Controls.Add(this.ctrlEmptyItem);
            this.Controls.Add(this.gbDevice);
            this.Controls.Add(this.gbServerBrowse);
            this.Controls.Add(this.gbConnectionOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDeviceConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device {0} Properties - OPC UA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDeviceConfig_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDeviceConfig_FormClosed);
            this.Load += new System.EventHandler(this.FrmDeviceConfig_Load);
            this.gbDevice.ResumeLayout(false);
            this.gbServerBrowse.ResumeLayout(false);
            this.gbConnectionOptions.ResumeLayout(false);
            this.gbConnectionOptions.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectionInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.GroupBox gbServerBrowse;
        private System.Windows.Forms.GroupBox gbConnectionOptions;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Label lblServerUrl;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSecurityOptions;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TreeView tvServer;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnMoveDownItem;
        private System.Windows.Forms.Button btnMoveUpItem;
        private System.Windows.Forms.Button btnAddSubscription;
        private System.Windows.Forms.TreeView tvDevice;
        private Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlSubscription ctrlSubscription;
        private Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlItem ctrlItem;
        private System.Windows.Forms.Button btnViewAttrs;
        private Button btnEditingOptions;
        private Controls.CtrlCommand ctrlCommand;
        private Controls.CtrlEmptyItem ctrlEmptyItem;
        private Panel pnlBottom;
        private Label lblConnectionInfo;
        private PictureBox pbConnectionInfo;
    }
}