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
            components = new System.ComponentModel.Container();
            btnClose = new Button();
            btnSave = new Button();
            gbDevice = new GroupBox();
            tvDevice = new TreeView();
            ilTree = new ImageList(components);
            btnDeleteItem = new Button();
            btnMoveDownItem = new Button();
            btnMoveUpItem = new Button();
            btnAddItem = new Button();
            btnAddSubscription = new Button();
            gbServerBrowse = new GroupBox();
            tvServer = new TreeView();
            btnDisconnect = new Button();
            btnViewAttrs = new Button();
            btnConnect = new Button();
            gbConnectionOptions = new GroupBox();
            lblConnectionInfo = new Label();
            pbConnectionInfo = new PictureBox();
            btnSecurityOptions = new Button();
            txtServerUrl = new TextBox();
            lblServerUrl = new Label();
            toolTip = new ToolTip(components);
            ctrlItem = new Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlItem();
            ctrlSubscription = new Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlSubscription();
            btnEditingOptions = new Button();
            ctrlCommand = new Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlCommand();
            ctrlEmptyItem = new Scada.Comm.Drivers.DrvOpcUa.View.Controls.CtrlEmptyItem();
            pnlBottom = new Panel();
            tabControl1 = new TabControl();
            pageLine = new TabPage();
            pageDevice = new TabPage();
            gbDevice.SuspendLayout();
            gbServerBrowse.SuspendLayout();
            gbConnectionOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbConnectionInfo).BeginInit();
            pnlBottom.SuspendLayout();
            tabControl1.SuspendLayout();
            pageLine.SuspendLayout();
            pageDevice.SuspendLayout();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(797, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Enabled = false;
            btnSave.Location = new Point(716, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // gbDevice
            // 
            gbDevice.Controls.Add(tvDevice);
            gbDevice.Controls.Add(btnDeleteItem);
            gbDevice.Controls.Add(btnMoveDownItem);
            gbDevice.Controls.Add(btnMoveUpItem);
            gbDevice.Controls.Add(btnAddItem);
            gbDevice.Controls.Add(btnAddSubscription);
            gbDevice.Location = new Point(313, 8);
            gbDevice.Name = "gbDevice";
            gbDevice.Padding = new Padding(10, 3, 10, 10);
            gbDevice.Size = new Size(299, 572);
            gbDevice.TabIndex = 2;
            gbDevice.TabStop = false;
            gbDevice.Text = "Device Configuration";
            // 
            // tvDevice
            // 
            tvDevice.HideSelection = false;
            tvDevice.ImageIndex = 0;
            tvDevice.ImageList = ilTree;
            tvDevice.Location = new Point(13, 51);
            tvDevice.Name = "tvDevice";
            tvDevice.SelectedImageIndex = 0;
            tvDevice.Size = new Size(273, 508);
            tvDevice.TabIndex = 5;
            tvDevice.AfterCollapse += tvDevice_AfterCollapse;
            tvDevice.AfterExpand += tvDevice_AfterExpand;
            tvDevice.AfterSelect += tvDevice_AfterSelect;
            // 
            // ilTree
            // 
            ilTree.ColorDepth = ColorDepth.Depth24Bit;
            ilTree.ImageSize = new Size(16, 16);
            ilTree.TransparentColor = Color.Transparent;
            // 
            // btnDeleteItem
            // 
            btnDeleteItem.FlatStyle = FlatStyle.Popup;
            btnDeleteItem.Image = Properties.Resources.delete;
            btnDeleteItem.Location = new Point(129, 22);
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.Size = new Size(23, 23);
            btnDeleteItem.TabIndex = 4;
            toolTip.SetToolTip(btnDeleteItem, "Delete");
            btnDeleteItem.UseVisualStyleBackColor = true;
            btnDeleteItem.Click += btnDeleteItem_Click;
            // 
            // btnMoveDownItem
            // 
            btnMoveDownItem.FlatStyle = FlatStyle.Popup;
            btnMoveDownItem.Image = Properties.Resources.move_down;
            btnMoveDownItem.Location = new Point(100, 22);
            btnMoveDownItem.Name = "btnMoveDownItem";
            btnMoveDownItem.Size = new Size(23, 23);
            btnMoveDownItem.TabIndex = 3;
            toolTip.SetToolTip(btnMoveDownItem, "Move Down");
            btnMoveDownItem.UseVisualStyleBackColor = true;
            btnMoveDownItem.Click += btnMoveDownItem_Click;
            // 
            // btnMoveUpItem
            // 
            btnMoveUpItem.FlatStyle = FlatStyle.Popup;
            btnMoveUpItem.Image = Properties.Resources.move_up;
            btnMoveUpItem.Location = new Point(71, 22);
            btnMoveUpItem.Name = "btnMoveUpItem";
            btnMoveUpItem.Size = new Size(23, 23);
            btnMoveUpItem.TabIndex = 2;
            toolTip.SetToolTip(btnMoveUpItem, "Move Up");
            btnMoveUpItem.UseVisualStyleBackColor = true;
            btnMoveUpItem.Click += btnMoveUpItem_Click;
            // 
            // btnAddItem
            // 
            btnAddItem.FlatStyle = FlatStyle.Popup;
            btnAddItem.Image = Properties.Resources.add;
            btnAddItem.Location = new Point(42, 22);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(23, 23);
            btnAddItem.TabIndex = 0;
            toolTip.SetToolTip(btnAddItem, "Add Selected Item");
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // btnAddSubscription
            // 
            btnAddSubscription.FlatStyle = FlatStyle.Popup;
            btnAddSubscription.Image = Properties.Resources.folder_add;
            btnAddSubscription.Location = new Point(13, 22);
            btnAddSubscription.Name = "btnAddSubscription";
            btnAddSubscription.Size = new Size(23, 23);
            btnAddSubscription.TabIndex = 1;
            toolTip.SetToolTip(btnAddSubscription, "Add Subscription");
            btnAddSubscription.UseVisualStyleBackColor = true;
            btnAddSubscription.Click += btnAddSubscription_Click;
            // 
            // gbServerBrowse
            // 
            gbServerBrowse.Controls.Add(tvServer);
            gbServerBrowse.Controls.Add(btnDisconnect);
            gbServerBrowse.Controls.Add(btnViewAttrs);
            gbServerBrowse.Controls.Add(btnConnect);
            gbServerBrowse.Location = new Point(8, 8);
            gbServerBrowse.Name = "gbServerBrowse";
            gbServerBrowse.Padding = new Padding(10, 3, 10, 10);
            gbServerBrowse.Size = new Size(299, 572);
            gbServerBrowse.TabIndex = 1;
            gbServerBrowse.TabStop = false;
            gbServerBrowse.Text = "Server Browse";
            // 
            // tvServer
            // 
            tvServer.HideSelection = false;
            tvServer.ImageIndex = 0;
            tvServer.ImageList = ilTree;
            tvServer.Location = new Point(13, 51);
            tvServer.Name = "tvServer";
            tvServer.SelectedImageIndex = 0;
            tvServer.Size = new Size(273, 508);
            tvServer.TabIndex = 3;
            tvServer.BeforeExpand += tvServer_BeforeExpand;
            tvServer.AfterSelect += tvServer_AfterSelect;
            tvServer.NodeMouseDoubleClick += tvServer_NodeMouseDoubleClick;
            tvServer.KeyDown += tvServer_KeyDown;
            // 
            // btnDisconnect
            // 
            btnDisconnect.FlatStyle = FlatStyle.Popup;
            btnDisconnect.Image = Properties.Resources.disconnect;
            btnDisconnect.Location = new Point(42, 22);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(23, 23);
            btnDisconnect.TabIndex = 1;
            toolTip.SetToolTip(btnDisconnect, "Disconnect from Server");
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnViewAttrs
            // 
            btnViewAttrs.FlatStyle = FlatStyle.Popup;
            btnViewAttrs.Image = Properties.Resources.attributes;
            btnViewAttrs.Location = new Point(71, 22);
            btnViewAttrs.Name = "btnViewAttrs";
            btnViewAttrs.Size = new Size(23, 23);
            btnViewAttrs.TabIndex = 2;
            toolTip.SetToolTip(btnViewAttrs, "View Attributes");
            btnViewAttrs.UseVisualStyleBackColor = true;
            btnViewAttrs.Click += btnViewAttrs_Click;
            // 
            // btnConnect
            // 
            btnConnect.FlatStyle = FlatStyle.Popup;
            btnConnect.Image = Properties.Resources.connect;
            btnConnect.Location = new Point(13, 22);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(23, 23);
            btnConnect.TabIndex = 0;
            toolTip.SetToolTip(btnConnect, "Connect to Server");
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_ClickAsync;
            // 
            // gbConnectionOptions
            // 
            gbConnectionOptions.Controls.Add(lblConnectionInfo);
            gbConnectionOptions.Controls.Add(pbConnectionInfo);
            gbConnectionOptions.Controls.Add(btnSecurityOptions);
            gbConnectionOptions.Controls.Add(txtServerUrl);
            gbConnectionOptions.Controls.Add(lblServerUrl);
            gbConnectionOptions.Location = new Point(8, 8);
            gbConnectionOptions.Name = "gbConnectionOptions";
            gbConnectionOptions.Padding = new Padding(10, 3, 10, 10);
            gbConnectionOptions.Size = new Size(860, 95);
            gbConnectionOptions.TabIndex = 0;
            gbConnectionOptions.TabStop = false;
            gbConnectionOptions.Text = "Connection Options";
            // 
            // lblConnectionInfo
            // 
            lblConnectionInfo.AutoSize = true;
            lblConnectionInfo.ForeColor = SystemColors.GrayText;
            lblConnectionInfo.Location = new Point(35, 67);
            lblConnectionInfo.Name = "lblConnectionInfo";
            lblConnectionInfo.Size = new Size(330, 15);
            lblConnectionInfo.TabIndex = 3;
            lblConnectionInfo.Text = "Connection options are common to the communication line.";
            // 
            // pbConnectionInfo
            // 
            pbConnectionInfo.Image = Properties.Resources.info;
            pbConnectionInfo.Location = new Point(13, 66);
            pbConnectionInfo.Name = "pbConnectionInfo";
            pbConnectionInfo.Size = new Size(16, 16);
            pbConnectionInfo.TabIndex = 12;
            pbConnectionInfo.TabStop = false;
            // 
            // btnSecurityOptions
            // 
            btnSecurityOptions.FlatStyle = FlatStyle.Popup;
            btnSecurityOptions.Image = Properties.Resources._lock;
            btnSecurityOptions.Location = new Point(824, 37);
            btnSecurityOptions.Name = "btnSecurityOptions";
            btnSecurityOptions.Size = new Size(23, 23);
            btnSecurityOptions.TabIndex = 2;
            toolTip.SetToolTip(btnSecurityOptions, "Security Options");
            btnSecurityOptions.UseVisualStyleBackColor = true;
            btnSecurityOptions.Click += btnSecurityOptions_Click;
            // 
            // txtServerUrl
            // 
            txtServerUrl.Location = new Point(13, 37);
            txtServerUrl.Name = "txtServerUrl";
            txtServerUrl.Size = new Size(805, 23);
            txtServerUrl.TabIndex = 1;
            txtServerUrl.TextChanged += txtServerUrl_TextChanged;
            // 
            // lblServerUrl
            // 
            lblServerUrl.AutoSize = true;
            lblServerUrl.Location = new Point(10, 19);
            lblServerUrl.Name = "lblServerUrl";
            lblServerUrl.Size = new Size(63, 15);
            lblServerUrl.TabIndex = 0;
            lblServerUrl.Text = "Server URL";
            // 
            // ctrlItem
            // 
            ctrlItem.ItemConfig = null;
            ctrlItem.Location = new Point(618, 145);
            ctrlItem.Name = "ctrlItem";
            ctrlItem.Size = new Size(250, 572);
            ctrlItem.TabIndex = 5;
            ctrlItem.ObjectChanged += ctrlItem_ObjectChanged;
            // 
            // ctrlSubscription
            // 
            ctrlSubscription.Location = new Point(618, 95);
            ctrlSubscription.Name = "ctrlSubscription";
            ctrlSubscription.Size = new Size(250, 572);
            ctrlSubscription.SubscriptionConfig = null;
            ctrlSubscription.TabIndex = 4;
            ctrlSubscription.ObjectChanged += ctrlItem_ObjectChanged;
            // 
            // btnEditingOptions
            // 
            btnEditingOptions.Location = new Point(12, 10);
            btnEditingOptions.Name = "btnEditingOptions";
            btnEditingOptions.Size = new Size(100, 23);
            btnEditingOptions.TabIndex = 0;
            btnEditingOptions.Text = "Options";
            btnEditingOptions.UseVisualStyleBackColor = true;
            btnEditingOptions.Click += btnEditingOptions_Click;
            // 
            // ctrlCommand
            // 
            ctrlCommand.CommandConfig = null;
            ctrlCommand.Location = new Point(618, 195);
            ctrlCommand.Name = "ctrlCommand";
            ctrlCommand.Size = new Size(250, 572);
            ctrlCommand.TabIndex = 6;
            ctrlCommand.ObjectChanged += ctrlItem_ObjectChanged;
            // 
            // ctrlEmptyItem
            // 
            ctrlEmptyItem.Location = new Point(618, 8);
            ctrlEmptyItem.Name = "ctrlEmptyItem";
            ctrlEmptyItem.Size = new Size(250, 572);
            ctrlEmptyItem.TabIndex = 3;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnClose);
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Controls.Add(btnEditingOptions);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 616);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(884, 45);
            pnlBottom.TabIndex = 7;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(pageLine);
            tabControl1.Controls.Add(pageDevice);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(884, 616);
            tabControl1.TabIndex = 8;
            // 
            // pageLine
            // 
            pageLine.Controls.Add(gbConnectionOptions);
            pageLine.Location = new Point(4, 24);
            pageLine.Name = "pageLine";
            pageLine.Padding = new Padding(5);
            pageLine.Size = new Size(876, 588);
            pageLine.TabIndex = 0;
            pageLine.Text = "Line";
            pageLine.UseVisualStyleBackColor = true;
            // 
            // pageDevice
            // 
            pageDevice.Controls.Add(gbServerBrowse);
            pageDevice.Controls.Add(gbDevice);
            pageDevice.Controls.Add(ctrlCommand);
            pageDevice.Controls.Add(ctrlItem);
            pageDevice.Controls.Add(ctrlSubscription);
            pageDevice.Controls.Add(ctrlEmptyItem);
            pageDevice.Location = new Point(4, 24);
            pageDevice.Name = "pageDevice";
            pageDevice.Padding = new Padding(5);
            pageDevice.Size = new Size(876, 588);
            pageDevice.TabIndex = 1;
            pageDevice.Text = "Device";
            pageDevice.UseVisualStyleBackColor = true;
            // 
            // FrmDeviceConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(884, 661);
            Controls.Add(tabControl1);
            Controls.Add(pnlBottom);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDeviceConfig";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Device {0} Properties - OPC UA";
            FormClosing += FrmDeviceConfig_FormClosing;
            FormClosed += FrmDeviceConfig_FormClosed;
            Load += FrmDeviceConfig_Load;
            gbDevice.ResumeLayout(false);
            gbServerBrowse.ResumeLayout(false);
            gbConnectionOptions.ResumeLayout(false);
            gbConnectionOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbConnectionInfo).EndInit();
            pnlBottom.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            pageLine.ResumeLayout(false);
            pageDevice.ResumeLayout(false);
            ResumeLayout(false);

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
        private TabControl tabControl1;
        private TabPage pageLine;
        private TabPage pageDevice;
    }
}