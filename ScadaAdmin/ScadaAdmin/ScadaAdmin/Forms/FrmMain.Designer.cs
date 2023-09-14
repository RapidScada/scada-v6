namespace Scada.Admin.App.Forms
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            msMain = new System.Windows.Forms.MenuStrip();
            miFile = new System.Windows.Forms.ToolStripMenuItem();
            miFileNewProject = new System.Windows.Forms.ToolStripMenuItem();
            miFileOpenProject = new System.Windows.Forms.ToolStripMenuItem();
            miFileShowStartPage = new System.Windows.Forms.ToolStripMenuItem();
            miFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            miFileSave = new System.Windows.Forms.ToolStripMenuItem();
            miFileSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            miFileSep2 = new System.Windows.Forms.ToolStripSeparator();
            miFileClose = new System.Windows.Forms.ToolStripMenuItem();
            miFileCloseProject = new System.Windows.Forms.ToolStripMenuItem();
            miFileSep3 = new System.Windows.Forms.ToolStripSeparator();
            miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            miDeploy = new System.Windows.Forms.ToolStripMenuItem();
            miDeployInstanceProfile = new System.Windows.Forms.ToolStripMenuItem();
            miDeployDownloadConfig = new System.Windows.Forms.ToolStripMenuItem();
            miDeployUploadConfig = new System.Windows.Forms.ToolStripMenuItem();
            miDeployInstanceStatus = new System.Windows.Forms.ToolStripMenuItem();
            miTools = new System.Windows.Forms.ToolStripMenuItem();
            miToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            miToolsCulture = new System.Windows.Forms.ToolStripMenuItem();
            miWindow = new System.Windows.Forms.ToolStripMenuItem();
            miWindowCloseActive = new System.Windows.Forms.ToolStripMenuItem();
            miWindowCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            miWindowCloseAllButActive = new System.Windows.Forms.ToolStripMenuItem();
            miHelp = new System.Windows.Forms.ToolStripMenuItem();
            miHelpDoc = new System.Windows.Forms.ToolStripMenuItem();
            miHelpSupport = new System.Windows.Forms.ToolStripMenuItem();
            miHelpSep1 = new System.Windows.Forms.ToolStripSeparator();
            miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            tsMain = new System.Windows.Forms.ToolStrip();
            btnFileNewProject = new System.Windows.Forms.ToolStripButton();
            btnFileOpenProject = new System.Windows.Forms.ToolStripButton();
            btnFileSave = new System.Windows.Forms.ToolStripButton();
            btnFileSaveAll = new System.Windows.Forms.ToolStripButton();
            toolSep1 = new System.Windows.Forms.ToolStripSeparator();
            btnDeployInstanceProfile = new System.Windows.Forms.ToolStripButton();
            btnDeployDownloadConfig = new System.Windows.Forms.ToolStripButton();
            btnDeployUploadConfig = new System.Windows.Forms.ToolStripButton();
            btnDeployInstanceStatus = new System.Windows.Forms.ToolStripButton();
            ssMain = new System.Windows.Forms.StatusStrip();
            lblSelectedInstance = new System.Windows.Forms.ToolStripStatusLabel();
            lblSelectedProfile = new System.Windows.Forms.ToolStripStatusLabel();
            pnlLeft = new System.Windows.Forms.Panel();
            tvExplorer = new System.Windows.Forms.TreeView();
            ilExplorer = new System.Windows.Forms.ImageList(components);
            splVert = new System.Windows.Forms.Splitter();
            pnlRight = new System.Windows.Forms.Panel();
            wctrlMain = new WinControl.WinControl();
            ofdProject = new System.Windows.Forms.OpenFileDialog();
            cmsInstance = new System.Windows.Forms.ContextMenuStrip(components);
            miInstanceAdd = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceDelete = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceSep1 = new System.Windows.Forms.ToolStripSeparator();
            miInstanceProfile = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceDownloadConfig = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceUploadConfig = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceStatus = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceSep2 = new System.Windows.Forms.ToolStripSeparator();
            miInstanceOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceOpenInBrowser = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceRename = new System.Windows.Forms.ToolStripMenuItem();
            miInstanceProperties = new System.Windows.Forms.ToolStripMenuItem();
            cmsProject = new System.Windows.Forms.ContextMenuStrip(components);
            miProjectOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            miProjectRename = new System.Windows.Forms.ToolStripMenuItem();
            miProjectProperties = new System.Windows.Forms.ToolStripMenuItem();
            cmsDirectory = new System.Windows.Forms.ContextMenuStrip(components);
            miDirectoryNewFile = new System.Windows.Forms.ToolStripMenuItem();
            miDirectoryNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            miDirectorySep1 = new System.Windows.Forms.ToolStripSeparator();
            miDirectoryDelete = new System.Windows.Forms.ToolStripMenuItem();
            miDirectoryRename = new System.Windows.Forms.ToolStripMenuItem();
            miDirectorySep2 = new System.Windows.Forms.ToolStripSeparator();
            miDirectoryOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            miDirectoryRefresh = new System.Windows.Forms.ToolStripMenuItem();
            cmsFileItem = new System.Windows.Forms.ContextMenuStrip(components);
            miFileItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            miFileItemOpenLocation = new System.Windows.Forms.ToolStripMenuItem();
            miFileItemSep1 = new System.Windows.Forms.ToolStripSeparator();
            miFileItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            miFileItemRename = new System.Windows.Forms.ToolStripMenuItem();
            cmsApp = new System.Windows.Forms.ContextMenuStrip(components);
            miAppOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            miAppReloadConfig = new System.Windows.Forms.ToolStripMenuItem();
            cmsCnlTable = new System.Windows.Forms.ContextMenuStrip(components);
            miCnlTableComm = new System.Windows.Forms.ToolStripMenuItem();
            miCnlTableRefresh = new System.Windows.Forms.ToolStripMenuItem();
            cmsBase = new System.Windows.Forms.ContextMenuStrip(components);
            miBaseOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            miBaseReload = new System.Windows.Forms.ToolStripMenuItem();
            msMain.SuspendLayout();
            tsMain.SuspendLayout();
            ssMain.SuspendLayout();
            pnlLeft.SuspendLayout();
            pnlRight.SuspendLayout();
            cmsInstance.SuspendLayout();
            cmsProject.SuspendLayout();
            cmsDirectory.SuspendLayout();
            cmsFileItem.SuspendLayout();
            cmsApp.SuspendLayout();
            cmsCnlTable.SuspendLayout();
            cmsBase.SuspendLayout();
            SuspendLayout();
            // 
            // msMain
            // 
            msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miFile, miDeploy, miTools, miWindow, miHelp });
            msMain.Location = new System.Drawing.Point(0, 0);
            msMain.Name = "msMain";
            msMain.Size = new System.Drawing.Size(784, 24);
            msMain.TabIndex = 0;
            // 
            // miFile
            // 
            miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miFileNewProject, miFileOpenProject, miFileShowStartPage, miFileSep1, miFileSave, miFileSaveAll, miFileSep2, miFileClose, miFileCloseProject, miFileSep3, miFileExit });
            miFile.Name = "miFile";
            miFile.Size = new System.Drawing.Size(37, 20);
            miFile.Text = "&File";
            miFile.DropDownOpening += miFile_DropDownOpening;
            // 
            // miFileNewProject
            // 
            miFileNewProject.Image = Properties.Resources.blank;
            miFileNewProject.Name = "miFileNewProject";
            miFileNewProject.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            miFileNewProject.Size = new System.Drawing.Size(195, 22);
            miFileNewProject.Text = "New Project...";
            miFileNewProject.Click += miFileNewProject_Click;
            // 
            // miFileOpenProject
            // 
            miFileOpenProject.Image = Properties.Resources.open;
            miFileOpenProject.Name = "miFileOpenProject";
            miFileOpenProject.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            miFileOpenProject.Size = new System.Drawing.Size(195, 22);
            miFileOpenProject.Text = "Open Project...";
            miFileOpenProject.Click += miFileOpenProject_Click;
            // 
            // miFileShowStartPage
            // 
            miFileShowStartPage.Image = Properties.Resources.start_page;
            miFileShowStartPage.Name = "miFileShowStartPage";
            miFileShowStartPage.Size = new System.Drawing.Size(195, 22);
            miFileShowStartPage.Text = "Start Page";
            miFileShowStartPage.Click += miFileShowStartPage_Click;
            // 
            // miFileSep1
            // 
            miFileSep1.Name = "miFileSep1";
            miFileSep1.Size = new System.Drawing.Size(192, 6);
            // 
            // miFileSave
            // 
            miFileSave.Image = Properties.Resources.save;
            miFileSave.Name = "miFileSave";
            miFileSave.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            miFileSave.Size = new System.Drawing.Size(195, 22);
            miFileSave.Text = "Save";
            miFileSave.Click += miFileSave_Click;
            // 
            // miFileSaveAll
            // 
            miFileSaveAll.Image = Properties.Resources.save_all;
            miFileSaveAll.Name = "miFileSaveAll";
            miFileSaveAll.Size = new System.Drawing.Size(195, 22);
            miFileSaveAll.Text = "Save All";
            miFileSaveAll.Click += miFileSaveAll_Click;
            // 
            // miFileSep2
            // 
            miFileSep2.Name = "miFileSep2";
            miFileSep2.Size = new System.Drawing.Size(192, 6);
            // 
            // miFileClose
            // 
            miFileClose.Name = "miFileClose";
            miFileClose.Size = new System.Drawing.Size(195, 22);
            miFileClose.Text = "Close";
            miFileClose.Click += miFileClose_Click;
            // 
            // miFileCloseProject
            // 
            miFileCloseProject.Name = "miFileCloseProject";
            miFileCloseProject.Size = new System.Drawing.Size(195, 22);
            miFileCloseProject.Text = "Close Project";
            miFileCloseProject.Click += miFileCloseProject_Click;
            // 
            // miFileSep3
            // 
            miFileSep3.Name = "miFileSep3";
            miFileSep3.Size = new System.Drawing.Size(192, 6);
            // 
            // miFileExit
            // 
            miFileExit.Image = Properties.Resources.close;
            miFileExit.Name = "miFileExit";
            miFileExit.Size = new System.Drawing.Size(195, 22);
            miFileExit.Text = "Exit";
            miFileExit.Click += miFileExit_Click;
            // 
            // miDeploy
            // 
            miDeploy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miDeployInstanceProfile, miDeployDownloadConfig, miDeployUploadConfig, miDeployInstanceStatus });
            miDeploy.Name = "miDeploy";
            miDeploy.Size = new System.Drawing.Size(56, 20);
            miDeploy.Text = "&Deploy";
            // 
            // miDeployInstanceProfile
            // 
            miDeployInstanceProfile.Image = Properties.Resources.deploy_profile;
            miDeployInstanceProfile.Name = "miDeployInstanceProfile";
            miDeployInstanceProfile.Size = new System.Drawing.Size(240, 22);
            miDeployInstanceProfile.Text = "Deployment Profile...";
            miDeployInstanceProfile.Click += miDeployInstanceProfile_Click;
            // 
            // miDeployDownloadConfig
            // 
            miDeployDownloadConfig.Image = Properties.Resources.download;
            miDeployDownloadConfig.Name = "miDeployDownloadConfig";
            miDeployDownloadConfig.Size = new System.Drawing.Size(240, 22);
            miDeployDownloadConfig.Text = "Download Configuration...";
            miDeployDownloadConfig.Click += miDeployDownloadConfig_Click;
            // 
            // miDeployUploadConfig
            // 
            miDeployUploadConfig.Image = Properties.Resources.upload;
            miDeployUploadConfig.Name = "miDeployUploadConfig";
            miDeployUploadConfig.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U;
            miDeployUploadConfig.Size = new System.Drawing.Size(240, 22);
            miDeployUploadConfig.Text = "Upload Configuration...";
            miDeployUploadConfig.Click += miDeployUploadConfig_Click;
            // 
            // miDeployInstanceStatus
            // 
            miDeployInstanceStatus.Image = Properties.Resources.status;
            miDeployInstanceStatus.Name = "miDeployInstanceStatus";
            miDeployInstanceStatus.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I;
            miDeployInstanceStatus.Size = new System.Drawing.Size(240, 22);
            miDeployInstanceStatus.Text = "Instance Status...";
            miDeployInstanceStatus.Click += miDeployInstanceStatus_Click;
            // 
            // miTools
            // 
            miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miToolsOptions, miToolsCulture });
            miTools.Name = "miTools";
            miTools.Size = new System.Drawing.Size(46, 20);
            miTools.Text = "&Tools";
            // 
            // miToolsOptions
            // 
            miToolsOptions.Image = Properties.Resources.options;
            miToolsOptions.Name = "miToolsOptions";
            miToolsOptions.Size = new System.Drawing.Size(135, 22);
            miToolsOptions.Text = "Options...";
            miToolsOptions.Click += miToolsOptions_Click;
            // 
            // miToolsCulture
            // 
            miToolsCulture.Name = "miToolsCulture";
            miToolsCulture.Size = new System.Drawing.Size(135, 22);
            miToolsCulture.Text = "Language...";
            miToolsCulture.Click += miToolsCulture_Click;
            // 
            // miWindow
            // 
            miWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miWindowCloseActive, miWindowCloseAll, miWindowCloseAllButActive });
            miWindow.Name = "miWindow";
            miWindow.Size = new System.Drawing.Size(63, 20);
            miWindow.Text = "&Window";
            miWindow.DropDownOpening += miWindow_DropDownOpening;
            // 
            // miWindowCloseActive
            // 
            miWindowCloseActive.Name = "miWindowCloseActive";
            miWindowCloseActive.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4;
            miWindowCloseActive.Size = new System.Drawing.Size(185, 22);
            miWindowCloseActive.Text = "Close Active";
            miWindowCloseActive.Click += miWindowCloseActive_Click;
            // 
            // miWindowCloseAll
            // 
            miWindowCloseAll.Name = "miWindowCloseAll";
            miWindowCloseAll.Size = new System.Drawing.Size(185, 22);
            miWindowCloseAll.Text = "Close All";
            miWindowCloseAll.Click += miWindowCloseAll_Click;
            // 
            // miWindowCloseAllButActive
            // 
            miWindowCloseAllButActive.Name = "miWindowCloseAllButActive";
            miWindowCloseAllButActive.Size = new System.Drawing.Size(185, 22);
            miWindowCloseAllButActive.Text = "Close All But Active";
            miWindowCloseAllButActive.Click += miWindowCloseAllButActive_Click;
            // 
            // miHelp
            // 
            miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miHelpDoc, miHelpSupport, miHelpSep1, miHelpAbout });
            miHelp.Name = "miHelp";
            miHelp.Size = new System.Drawing.Size(44, 20);
            miHelp.Text = "&Help";
            // 
            // miHelpDoc
            // 
            miHelpDoc.Image = Properties.Resources.help;
            miHelpDoc.Name = "miHelpDoc";
            miHelpDoc.Size = new System.Drawing.Size(168, 22);
            miHelpDoc.Text = "Documentation";
            miHelpDoc.Click += miHelpDoc_Click;
            // 
            // miHelpSupport
            // 
            miHelpSupport.Image = Properties.Resources.support;
            miHelpSupport.Name = "miHelpSupport";
            miHelpSupport.Size = new System.Drawing.Size(168, 22);
            miHelpSupport.Text = "Technical Support";
            miHelpSupport.Click += miHelpSupport_Click;
            // 
            // miHelpSep1
            // 
            miHelpSep1.Name = "miHelpSep1";
            miHelpSep1.Size = new System.Drawing.Size(165, 6);
            // 
            // miHelpAbout
            // 
            miHelpAbout.Image = Properties.Resources.about;
            miHelpAbout.Name = "miHelpAbout";
            miHelpAbout.Size = new System.Drawing.Size(168, 22);
            miHelpAbout.Text = "About";
            miHelpAbout.Click += miHelpAbout_Click;
            // 
            // tsMain
            // 
            tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnFileNewProject, btnFileOpenProject, btnFileSave, btnFileSaveAll, toolSep1, btnDeployInstanceProfile, btnDeployDownloadConfig, btnDeployUploadConfig, btnDeployInstanceStatus });
            tsMain.Location = new System.Drawing.Point(0, 24);
            tsMain.Name = "tsMain";
            tsMain.Size = new System.Drawing.Size(784, 25);
            tsMain.TabIndex = 1;
            // 
            // btnFileNewProject
            // 
            btnFileNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnFileNewProject.Image = Properties.Resources.blank;
            btnFileNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnFileNewProject.Name = "btnFileNewProject";
            btnFileNewProject.Size = new System.Drawing.Size(23, 22);
            btnFileNewProject.ToolTipText = "New Project (Ctrl+N)";
            btnFileNewProject.Click += miFileNewProject_Click;
            // 
            // btnFileOpenProject
            // 
            btnFileOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnFileOpenProject.Image = Properties.Resources.open;
            btnFileOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnFileOpenProject.Name = "btnFileOpenProject";
            btnFileOpenProject.Size = new System.Drawing.Size(23, 22);
            btnFileOpenProject.ToolTipText = "Open Project (Ctrl+O)";
            btnFileOpenProject.Click += miFileOpenProject_Click;
            // 
            // btnFileSave
            // 
            btnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnFileSave.Image = Properties.Resources.save;
            btnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnFileSave.Name = "btnFileSave";
            btnFileSave.Size = new System.Drawing.Size(23, 22);
            btnFileSave.ToolTipText = "Save (Ctrl+S)";
            btnFileSave.Click += miFileSave_Click;
            // 
            // btnFileSaveAll
            // 
            btnFileSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnFileSaveAll.Image = Properties.Resources.save_all;
            btnFileSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnFileSaveAll.Name = "btnFileSaveAll";
            btnFileSaveAll.Size = new System.Drawing.Size(23, 22);
            btnFileSaveAll.ToolTipText = "Save All";
            btnFileSaveAll.Click += miFileSaveAll_Click;
            // 
            // toolSep1
            // 
            toolSep1.Name = "toolSep1";
            toolSep1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDeployInstanceProfile
            // 
            btnDeployInstanceProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnDeployInstanceProfile.Image = Properties.Resources.deploy_profile;
            btnDeployInstanceProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnDeployInstanceProfile.Name = "btnDeployInstanceProfile";
            btnDeployInstanceProfile.Size = new System.Drawing.Size(23, 22);
            btnDeployInstanceProfile.ToolTipText = "Deployment Profile";
            btnDeployInstanceProfile.Click += miDeployInstanceProfile_Click;
            // 
            // btnDeployDownloadConfig
            // 
            btnDeployDownloadConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnDeployDownloadConfig.Image = Properties.Resources.download;
            btnDeployDownloadConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnDeployDownloadConfig.Name = "btnDeployDownloadConfig";
            btnDeployDownloadConfig.Size = new System.Drawing.Size(23, 22);
            btnDeployDownloadConfig.ToolTipText = "Download Configuration";
            btnDeployDownloadConfig.Click += miDeployDownloadConfig_Click;
            // 
            // btnDeployUploadConfig
            // 
            btnDeployUploadConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnDeployUploadConfig.Image = Properties.Resources.upload;
            btnDeployUploadConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnDeployUploadConfig.Name = "btnDeployUploadConfig";
            btnDeployUploadConfig.Size = new System.Drawing.Size(23, 22);
            btnDeployUploadConfig.ToolTipText = "Upload Configuration (Ctrl+U)";
            btnDeployUploadConfig.Click += miDeployUploadConfig_Click;
            // 
            // btnDeployInstanceStatus
            // 
            btnDeployInstanceStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnDeployInstanceStatus.Image = Properties.Resources.status;
            btnDeployInstanceStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnDeployInstanceStatus.Name = "btnDeployInstanceStatus";
            btnDeployInstanceStatus.Size = new System.Drawing.Size(23, 22);
            btnDeployInstanceStatus.ToolTipText = "Instance Status (Ctrl+I)";
            btnDeployInstanceStatus.Click += miDeployInstanceStatus_Click;
            // 
            // ssMain
            // 
            ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblSelectedInstance, lblSelectedProfile });
            ssMain.Location = new System.Drawing.Point(0, 487);
            ssMain.Name = "ssMain";
            ssMain.Size = new System.Drawing.Size(784, 24);
            ssMain.TabIndex = 2;
            // 
            // lblSelectedInstance
            // 
            lblSelectedInstance.Name = "lblSelectedInstance";
            lblSelectedInstance.Size = new System.Drawing.Size(108, 19);
            lblSelectedInstance.Text = "lblSelectedInstance";
            // 
            // lblSelectedProfile
            // 
            lblSelectedProfile.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            lblSelectedProfile.Name = "lblSelectedProfile";
            lblSelectedProfile.Size = new System.Drawing.Size(102, 19);
            lblSelectedProfile.Text = "lblSelectedProfile";
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(tvExplorer);
            pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            pnlLeft.Location = new System.Drawing.Point(0, 49);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new System.Drawing.Size(300, 438);
            pnlLeft.TabIndex = 3;
            // 
            // tvExplorer
            // 
            tvExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            tvExplorer.HideSelection = false;
            tvExplorer.ImageIndex = 0;
            tvExplorer.ImageList = ilExplorer;
            tvExplorer.Location = new System.Drawing.Point(0, 0);
            tvExplorer.Name = "tvExplorer";
            tvExplorer.SelectedImageIndex = 0;
            tvExplorer.ShowRootLines = false;
            tvExplorer.Size = new System.Drawing.Size(300, 438);
            tvExplorer.TabIndex = 0;
            tvExplorer.BeforeCollapse += tvExplorer_BeforeCollapse;
            tvExplorer.AfterCollapse += tvExplorer_AfterCollapse;
            tvExplorer.BeforeExpand += tvExplorer_BeforeExpand;
            tvExplorer.AfterExpand += tvExplorer_AfterExpand;
            tvExplorer.AfterSelect += tvExplorer_AfterSelect;
            tvExplorer.NodeMouseClick += tvExplorer_NodeMouseClick;
            tvExplorer.NodeMouseDoubleClick += tvExplorer_NodeMouseDoubleClick;
            tvExplorer.KeyDown += tvExplorer_KeyDown;
            tvExplorer.MouseDown += tvExplorer_MouseDown;
            // 
            // ilExplorer
            // 
            ilExplorer.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            ilExplorer.ImageSize = new System.Drawing.Size(16, 16);
            ilExplorer.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splVert
            // 
            splVert.Location = new System.Drawing.Point(300, 49);
            splVert.MinExtra = 100;
            splVert.MinSize = 100;
            splVert.Name = "splVert";
            splVert.Size = new System.Drawing.Size(3, 438);
            splVert.TabIndex = 4;
            splVert.TabStop = false;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(wctrlMain);
            pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRight.Location = new System.Drawing.Point(303, 49);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new System.Drawing.Size(481, 438);
            pnlRight.TabIndex = 5;
            // 
            // wctrlMain
            // 
            wctrlMain.ButtonsVisible = false;
            wctrlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            wctrlMain.Image = null;
            wctrlMain.Location = new System.Drawing.Point(0, 0);
            wctrlMain.MessageFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            wctrlMain.Name = "wctrlMain";
            wctrlMain.SaveReqCancel = "Cancel";
            wctrlMain.SaveReqCaption = "Save changes";
            wctrlMain.SaveReqNo = "&No";
            wctrlMain.SaveReqQuestion = "Save changes to the following items?";
            wctrlMain.SaveReqYes = "&Yes";
            wctrlMain.Size = new System.Drawing.Size(481, 438);
            wctrlMain.TabIndex = 0;
            wctrlMain.ActiveFormChanged += wctrlMain_ActiveFormChanged;
            wctrlMain.ChildFormClosed += wctrlMain_ChildFormClosed;
            wctrlMain.ChildFormMessage += wctrlMain_ChildFormMessage;
            wctrlMain.ChildFormModifiedChanged += wctrlMain_ChildFormModifiedChanged;
            // 
            // ofdProject
            // 
            ofdProject.DefaultExt = "*.rsproj";
            ofdProject.Filter = "Projects (*.rsproj)|*.rsproj|All Files (*.*)|*.*";
            // 
            // cmsInstance
            // 
            cmsInstance.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miInstanceAdd, miInstanceMoveUp, miInstanceMoveDown, miInstanceDelete, miInstanceSep1, miInstanceProfile, miInstanceDownloadConfig, miInstanceUploadConfig, miInstanceStatus, miInstanceSep2, miInstanceOpenInExplorer, miInstanceOpenInBrowser, miInstanceRename, miInstanceProperties });
            cmsInstance.Name = "cmsCommLine";
            cmsInstance.Size = new System.Drawing.Size(220, 280);
            cmsInstance.Opening += cmsInstance_Opening;
            // 
            // miInstanceAdd
            // 
            miInstanceAdd.Image = Properties.Resources.add;
            miInstanceAdd.Name = "miInstanceAdd";
            miInstanceAdd.Size = new System.Drawing.Size(219, 22);
            miInstanceAdd.Text = "Add Instance...";
            miInstanceAdd.Click += miInstanceAdd_Click;
            // 
            // miInstanceMoveUp
            // 
            miInstanceMoveUp.Image = Properties.Resources.move_up;
            miInstanceMoveUp.Name = "miInstanceMoveUp";
            miInstanceMoveUp.Size = new System.Drawing.Size(219, 22);
            miInstanceMoveUp.Text = "Move Instance Up";
            miInstanceMoveUp.Click += miInstanceMoveUp_Click;
            // 
            // miInstanceMoveDown
            // 
            miInstanceMoveDown.Image = Properties.Resources.move_down;
            miInstanceMoveDown.Name = "miInstanceMoveDown";
            miInstanceMoveDown.Size = new System.Drawing.Size(219, 22);
            miInstanceMoveDown.Text = "Move Instance Down";
            miInstanceMoveDown.Click += miInstanceMoveDown_Click;
            // 
            // miInstanceDelete
            // 
            miInstanceDelete.Image = Properties.Resources.delete;
            miInstanceDelete.Name = "miInstanceDelete";
            miInstanceDelete.Size = new System.Drawing.Size(219, 22);
            miInstanceDelete.Text = "Delete Instance";
            miInstanceDelete.Click += miInstanceDelete_Click;
            // 
            // miInstanceSep1
            // 
            miInstanceSep1.Name = "miInstanceSep1";
            miInstanceSep1.Size = new System.Drawing.Size(216, 6);
            // 
            // miInstanceProfile
            // 
            miInstanceProfile.Image = Properties.Resources.deploy_profile;
            miInstanceProfile.Name = "miInstanceProfile";
            miInstanceProfile.Size = new System.Drawing.Size(219, 22);
            miInstanceProfile.Text = "Deployment Profile...";
            miInstanceProfile.Click += miDeployInstanceProfile_Click;
            // 
            // miInstanceDownloadConfig
            // 
            miInstanceDownloadConfig.Image = Properties.Resources.download;
            miInstanceDownloadConfig.Name = "miInstanceDownloadConfig";
            miInstanceDownloadConfig.Size = new System.Drawing.Size(219, 22);
            miInstanceDownloadConfig.Text = "Download Configuration...";
            miInstanceDownloadConfig.Click += miDeployDownloadConfig_Click;
            // 
            // miInstanceUploadConfig
            // 
            miInstanceUploadConfig.Image = Properties.Resources.upload;
            miInstanceUploadConfig.Name = "miInstanceUploadConfig";
            miInstanceUploadConfig.Size = new System.Drawing.Size(219, 22);
            miInstanceUploadConfig.Text = "Upload Configuration...";
            miInstanceUploadConfig.Click += miDeployUploadConfig_Click;
            // 
            // miInstanceStatus
            // 
            miInstanceStatus.Image = Properties.Resources.status;
            miInstanceStatus.Name = "miInstanceStatus";
            miInstanceStatus.Size = new System.Drawing.Size(219, 22);
            miInstanceStatus.Text = "Instance Status...";
            miInstanceStatus.Click += miDeployInstanceStatus_Click;
            // 
            // miInstanceSep2
            // 
            miInstanceSep2.Name = "miInstanceSep2";
            miInstanceSep2.Size = new System.Drawing.Size(216, 6);
            // 
            // miInstanceOpenInExplorer
            // 
            miInstanceOpenInExplorer.Image = Properties.Resources.open_explorer;
            miInstanceOpenInExplorer.Name = "miInstanceOpenInExplorer";
            miInstanceOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            miInstanceOpenInExplorer.Text = "Open Folder in File Explorer";
            miInstanceOpenInExplorer.Click += miDirectoryOpenInExplorer_Click;
            // 
            // miInstanceOpenInBrowser
            // 
            miInstanceOpenInBrowser.Image = Properties.Resources.web;
            miInstanceOpenInBrowser.Name = "miInstanceOpenInBrowser";
            miInstanceOpenInBrowser.Size = new System.Drawing.Size(219, 22);
            miInstanceOpenInBrowser.Text = "Open in Web Browser";
            miInstanceOpenInBrowser.Click += miInstanceOpenInBrowser_Click;
            // 
            // miInstanceRename
            // 
            miInstanceRename.Image = Properties.Resources.rename;
            miInstanceRename.Name = "miInstanceRename";
            miInstanceRename.Size = new System.Drawing.Size(219, 22);
            miInstanceRename.Text = "Rename Instance";
            miInstanceRename.Click += miInstanceRename_Click;
            // 
            // miInstanceProperties
            // 
            miInstanceProperties.Image = Properties.Resources.properties;
            miInstanceProperties.Name = "miInstanceProperties";
            miInstanceProperties.Size = new System.Drawing.Size(219, 22);
            miInstanceProperties.Text = "Properties";
            miInstanceProperties.Click += miInstanceProperties_Click;
            // 
            // cmsProject
            // 
            cmsProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miProjectOpenInExplorer, miProjectRename, miProjectProperties });
            cmsProject.Name = "cmsCommLine";
            cmsProject.Size = new System.Drawing.Size(220, 70);
            // 
            // miProjectOpenInExplorer
            // 
            miProjectOpenInExplorer.Image = Properties.Resources.open_explorer;
            miProjectOpenInExplorer.Name = "miProjectOpenInExplorer";
            miProjectOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            miProjectOpenInExplorer.Text = "Open Folder in File Explorer";
            miProjectOpenInExplorer.Click += miDirectoryOpenInExplorer_Click;
            // 
            // miProjectRename
            // 
            miProjectRename.Image = Properties.Resources.rename;
            miProjectRename.Name = "miProjectRename";
            miProjectRename.Size = new System.Drawing.Size(219, 22);
            miProjectRename.Text = "Rename Project";
            miProjectRename.Click += miProjectRename_Click;
            // 
            // miProjectProperties
            // 
            miProjectProperties.Image = Properties.Resources.properties;
            miProjectProperties.Name = "miProjectProperties";
            miProjectProperties.Size = new System.Drawing.Size(219, 22);
            miProjectProperties.Text = "Properties";
            miProjectProperties.Click += miProjectProperties_Click;
            // 
            // cmsDirectory
            // 
            cmsDirectory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miDirectoryNewFile, miDirectoryNewFolder, miDirectorySep1, miDirectoryDelete, miDirectoryRename, miDirectorySep2, miDirectoryOpenInExplorer, miDirectoryRefresh });
            cmsDirectory.Name = "cmsDirectory";
            cmsDirectory.Size = new System.Drawing.Size(220, 148);
            cmsDirectory.Opening += cmsDirectory_Opening;
            // 
            // miDirectoryNewFile
            // 
            miDirectoryNewFile.Image = Properties.Resources.new_file;
            miDirectoryNewFile.Name = "miDirectoryNewFile";
            miDirectoryNewFile.Size = new System.Drawing.Size(219, 22);
            miDirectoryNewFile.Text = "New File...";
            miDirectoryNewFile.Click += miDirectoryNewFile_Click;
            // 
            // miDirectoryNewFolder
            // 
            miDirectoryNewFolder.Image = Properties.Resources.new_folder;
            miDirectoryNewFolder.Name = "miDirectoryNewFolder";
            miDirectoryNewFolder.Size = new System.Drawing.Size(219, 22);
            miDirectoryNewFolder.Text = "New Folder...";
            miDirectoryNewFolder.Click += miDirectoryNewFolder_Click;
            // 
            // miDirectorySep1
            // 
            miDirectorySep1.Name = "miDirectorySep1";
            miDirectorySep1.Size = new System.Drawing.Size(216, 6);
            // 
            // miDirectoryDelete
            // 
            miDirectoryDelete.Image = Properties.Resources.delete;
            miDirectoryDelete.Name = "miDirectoryDelete";
            miDirectoryDelete.Size = new System.Drawing.Size(219, 22);
            miDirectoryDelete.Text = "Delete";
            miDirectoryDelete.Click += miDirectoryDelete_Click;
            // 
            // miDirectoryRename
            // 
            miDirectoryRename.Image = Properties.Resources.rename;
            miDirectoryRename.Name = "miDirectoryRename";
            miDirectoryRename.Size = new System.Drawing.Size(219, 22);
            miDirectoryRename.Text = "Rename";
            miDirectoryRename.Click += miDirectoryRename_Click;
            // 
            // miDirectorySep2
            // 
            miDirectorySep2.Name = "miDirectorySep2";
            miDirectorySep2.Size = new System.Drawing.Size(216, 6);
            // 
            // miDirectoryOpenInExplorer
            // 
            miDirectoryOpenInExplorer.Image = Properties.Resources.open_explorer;
            miDirectoryOpenInExplorer.Name = "miDirectoryOpenInExplorer";
            miDirectoryOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            miDirectoryOpenInExplorer.Text = "Open Folder in File Explorer";
            miDirectoryOpenInExplorer.Click += miDirectoryOpenInExplorer_Click;
            // 
            // miDirectoryRefresh
            // 
            miDirectoryRefresh.Image = Properties.Resources.refresh;
            miDirectoryRefresh.Name = "miDirectoryRefresh";
            miDirectoryRefresh.Size = new System.Drawing.Size(219, 22);
            miDirectoryRefresh.Text = "Refresh";
            miDirectoryRefresh.Click += miDirectoryRefresh_Click;
            // 
            // cmsFileItem
            // 
            cmsFileItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miFileItemOpen, miFileItemOpenLocation, miFileItemSep1, miFileItemDelete, miFileItemRename });
            cmsFileItem.Name = "cmsFileItem";
            cmsFileItem.Size = new System.Drawing.Size(233, 98);
            cmsFileItem.Opening += cmsFileItem_Opening;
            // 
            // miFileItemOpen
            // 
            miFileItemOpen.Image = Properties.Resources.open_file;
            miFileItemOpen.Name = "miFileItemOpen";
            miFileItemOpen.Size = new System.Drawing.Size(232, 22);
            miFileItemOpen.Text = "Open";
            miFileItemOpen.Click += miFileItemOpen_Click;
            // 
            // miFileItemOpenLocation
            // 
            miFileItemOpenLocation.Image = Properties.Resources.open_explorer;
            miFileItemOpenLocation.Name = "miFileItemOpenLocation";
            miFileItemOpenLocation.Size = new System.Drawing.Size(232, 22);
            miFileItemOpenLocation.Text = "Open Location in File Explorer";
            miFileItemOpenLocation.Click += miFileItemOpenLocation_Click;
            // 
            // miFileItemSep1
            // 
            miFileItemSep1.Name = "miFileItemSep1";
            miFileItemSep1.Size = new System.Drawing.Size(229, 6);
            // 
            // miFileItemDelete
            // 
            miFileItemDelete.Image = Properties.Resources.delete;
            miFileItemDelete.Name = "miFileItemDelete";
            miFileItemDelete.Size = new System.Drawing.Size(232, 22);
            miFileItemDelete.Text = "Delete";
            miFileItemDelete.Click += miFileItemDelete_Click;
            // 
            // miFileItemRename
            // 
            miFileItemRename.Image = Properties.Resources.rename;
            miFileItemRename.Name = "miFileItemRename";
            miFileItemRename.Size = new System.Drawing.Size(232, 22);
            miFileItemRename.Text = "Rename";
            miFileItemRename.Click += miFileItemRename_Click;
            // 
            // cmsApp
            // 
            cmsApp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miAppOpenInExplorer, miAppReloadConfig });
            cmsApp.Name = "cmsServer";
            cmsApp.Size = new System.Drawing.Size(220, 48);
            // 
            // miAppOpenInExplorer
            // 
            miAppOpenInExplorer.Image = Properties.Resources.open_explorer;
            miAppOpenInExplorer.Name = "miAppOpenInExplorer";
            miAppOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            miAppOpenInExplorer.Text = "Open Folder in File Explorer";
            miAppOpenInExplorer.Click += miDirectoryOpenInExplorer_Click;
            // 
            // miAppReloadConfig
            // 
            miAppReloadConfig.Image = Properties.Resources.refresh;
            miAppReloadConfig.Name = "miAppReloadConfig";
            miAppReloadConfig.Size = new System.Drawing.Size(219, 22);
            miAppReloadConfig.Text = "Reload Configuration";
            miAppReloadConfig.Click += miAppReloadConfig_Click;
            // 
            // cmsCnlTable
            // 
            cmsCnlTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miCnlTableComm, miCnlTableRefresh });
            cmsCnlTable.Name = "cmsChannels";
            cmsCnlTable.Size = new System.Drawing.Size(188, 48);
            cmsCnlTable.Opening += cmsCnlTable_Opening;
            // 
            // miCnlTableComm
            // 
            miCnlTableComm.Image = Properties.Resources.open_explorer;
            miCnlTableComm.Name = "miCnlTableComm";
            miCnlTableComm.Size = new System.Drawing.Size(187, 22);
            miCnlTableComm.Text = "Go to Communicator";
            miCnlTableComm.Click += miCnlTableComm_Click;
            // 
            // miCnlTableRefresh
            // 
            miCnlTableRefresh.Image = Properties.Resources.refresh;
            miCnlTableRefresh.Name = "miCnlTableRefresh";
            miCnlTableRefresh.Size = new System.Drawing.Size(187, 22);
            miCnlTableRefresh.Text = "Refresh";
            miCnlTableRefresh.Click += miCnlTableRefresh_Click;
            // 
            // cmsBase
            // 
            cmsBase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miBaseOpenInExplorer, miBaseReload });
            cmsBase.Name = "cmsBase";
            cmsBase.Size = new System.Drawing.Size(239, 70);
            // 
            // miBaseOpenInExplorer
            // 
            miBaseOpenInExplorer.Image = Properties.Resources.open_explorer;
            miBaseOpenInExplorer.Name = "miBaseOpenInExplorer";
            miBaseOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            miBaseOpenInExplorer.Text = "Open Folder in File Explorer";
            // 
            // miBaseReload
            // 
            miBaseReload.Image = Properties.Resources.refresh;
            miBaseReload.Name = "miBaseReload";
            miBaseReload.Size = new System.Drawing.Size(238, 22);
            miBaseReload.Text = "Reload Configuration Database";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(784, 511);
            Controls.Add(pnlRight);
            Controls.Add(splVert);
            Controls.Add(pnlLeft);
            Controls.Add(ssMain);
            Controls.Add(tsMain);
            Controls.Add(msMain);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = msMain;
            MinimumSize = new System.Drawing.Size(300, 200);
            Name = "FrmMain";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Administrator";
            FormClosing += FrmMain_FormClosing;
            FormClosed += FrmMain_FormClosed;
            Load += FrmMain_Load;
            msMain.ResumeLayout(false);
            msMain.PerformLayout();
            tsMain.ResumeLayout(false);
            tsMain.PerformLayout();
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            pnlLeft.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            cmsInstance.ResumeLayout(false);
            cmsProject.ResumeLayout(false);
            cmsDirectory.ResumeLayout(false);
            cmsFileItem.ResumeLayout(false);
            cmsApp.ResumeLayout(false);
            cmsCnlTable.ResumeLayout(false);
            cmsBase.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Splitter splVert;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miFileNewProject;
        private System.Windows.Forms.ToolStripMenuItem miFileOpenProject;
        private System.Windows.Forms.ToolStripMenuItem miFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAll;
        private System.Windows.Forms.ToolStripSeparator miFileSep1;
        private System.Windows.Forms.ToolStripMenuItem miFileExit;
        private System.Windows.Forms.ToolStripMenuItem miTools;
        private System.Windows.Forms.ToolStripMenuItem miToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem miHelpDoc;
        private System.Windows.Forms.ToolStripMenuItem miHelpSupport;
        private System.Windows.Forms.ToolStripSeparator miHelpSep1;
        private System.Windows.Forms.ToolStripButton btnFileNewProject;
        private System.Windows.Forms.TreeView tvExplorer;
        private WinControl.WinControl wctrlMain;
        private System.Windows.Forms.ImageList ilExplorer;
        private System.Windows.Forms.OpenFileDialog ofdProject;
        private System.Windows.Forms.ContextMenuStrip cmsInstance;
        private System.Windows.Forms.ToolStripMenuItem miInstanceAdd;
        private System.Windows.Forms.ToolStripMenuItem miInstanceMoveUp;
        private System.Windows.Forms.ToolStripMenuItem miInstanceMoveDown;
        private System.Windows.Forms.ToolStripMenuItem miInstanceDelete;
        private System.Windows.Forms.ToolStripSeparator miInstanceSep1;
        private System.Windows.Forms.ToolStripMenuItem miInstanceRename;
        private System.Windows.Forms.ToolStripMenuItem miInstanceProperties;
        private System.Windows.Forms.ContextMenuStrip cmsProject;
        private System.Windows.Forms.ToolStripMenuItem miProjectRename;
        private System.Windows.Forms.ToolStripMenuItem miProjectProperties;
        private System.Windows.Forms.ContextMenuStrip cmsDirectory;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryNewFile;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryNewFolder;
        private System.Windows.Forms.ToolStripSeparator miDirectorySep1;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryDelete;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryRename;
        private System.Windows.Forms.ToolStripSeparator miDirectorySep2;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryOpenInExplorer;
        private System.Windows.Forms.ContextMenuStrip cmsFileItem;
        private System.Windows.Forms.ToolStripMenuItem miFileItemOpen;
        private System.Windows.Forms.ToolStripMenuItem miFileItemOpenLocation;
        private System.Windows.Forms.ToolStripSeparator miFileItemSep1;
        private System.Windows.Forms.ToolStripMenuItem miFileItemDelete;
        private System.Windows.Forms.ToolStripMenuItem miFileItemRename;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryRefresh;
        private System.Windows.Forms.ToolStripMenuItem miFileClose;
        private System.Windows.Forms.ToolStripMenuItem miFileCloseProject;
        private System.Windows.Forms.ToolStripSeparator miFileSep2;
        private System.Windows.Forms.ToolStripButton btnFileOpenProject;
        private System.Windows.Forms.ToolStripButton btnFileSave;
        private System.Windows.Forms.ToolStripButton btnFileSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolSep1;
        private System.Windows.Forms.ToolStripMenuItem miDeploy;
        private System.Windows.Forms.ToolStripMenuItem miDeployDownloadConfig;
        private System.Windows.Forms.ToolStripMenuItem miDeployUploadConfig;
        private System.Windows.Forms.ToolStripMenuItem miDeployInstanceStatus;
        private System.Windows.Forms.ToolStripSeparator miInstanceSep2;
        private System.Windows.Forms.ToolStripMenuItem miInstanceDownloadConfig;
        private System.Windows.Forms.ToolStripMenuItem miInstanceUploadConfig;
        private System.Windows.Forms.ToolStripMenuItem miInstanceStatus;
        private System.Windows.Forms.ToolStripMenuItem miInstanceProfile;
        private System.Windows.Forms.ToolStripMenuItem miDeployInstanceProfile;
        private System.Windows.Forms.ToolStripMenuItem miProjectOpenInExplorer;
        private System.Windows.Forms.ToolStripMenuItem miInstanceOpenInExplorer;
        private System.Windows.Forms.ContextMenuStrip cmsApp;
        private System.Windows.Forms.ToolStripMenuItem miAppOpenInExplorer;
        private System.Windows.Forms.ToolStripMenuItem miToolsCulture;
        private System.Windows.Forms.ToolStripMenuItem miWindow;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseActive;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseAll;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseAllButActive;
        private System.Windows.Forms.ToolStripMenuItem miFileShowStartPage;
        private System.Windows.Forms.ToolStripSeparator miFileSep3;
        private System.Windows.Forms.ToolStripButton btnDeployInstanceProfile;
        private System.Windows.Forms.ToolStripButton btnDeployDownloadConfig;
        private System.Windows.Forms.ToolStripButton btnDeployUploadConfig;
        private System.Windows.Forms.ToolStripButton btnDeployInstanceStatus;
        private System.Windows.Forms.ContextMenuStrip cmsCnlTable;
        private System.Windows.Forms.ToolStripMenuItem miCnlTableRefresh;
        private System.Windows.Forms.ToolStripMenuItem miCnlTableComm;
        private System.Windows.Forms.ToolStripMenuItem miInstanceOpenInBrowser;
        private System.Windows.Forms.ToolStripStatusLabel lblSelectedInstance;
        private System.Windows.Forms.ToolStripStatusLabel lblSelectedProfile;
        private System.Windows.Forms.ToolStripMenuItem miAppReloadConfig;
        private System.Windows.Forms.ContextMenuStrip cmsBase;
        private System.Windows.Forms.ToolStripMenuItem miBaseOpenInExplorer;
        private System.Windows.Forms.ToolStripMenuItem miBaseReload;
    }
}

