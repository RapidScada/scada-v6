/*
 * Copyright 2023 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Represents a main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Admin.App.Code;
using Scada.Admin.App.Forms.Deployment;
using Scada.Admin.App.Forms.Tables;
using Scada.Admin.App.Forms.Tools;
using Scada.Admin.App.Properties;
using Scada.Admin.Deployment;
using Scada.Admin.Extensions;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Agent.Client;
using Scada.Comm.Config;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Represents a main form of the application.
    /// <para>Представляет главную форма приложения.</para>
    /// </summary>
    public partial class FrmMain : Form, IMainForm
    {
        private readonly AppData appData;                 // the common data of the application
        private readonly ExplorerBuilder explorerBuilder; // the object to manipulate the explorer tree
        private FrmStartPage frmStartPage;                // the start page
        private bool preventNodeExpand;                   // prevent a tree node from expanding or collapsing


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmMain(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            appData.MainForm = this;
            explorerBuilder = new ExplorerBuilder(appData, tvExplorer, new ContextMenus
            {
                ProjectMenu = cmsProject,
                CnlTableMenu = cmsCnlTable,
                DirectoryMenu = cmsDirectory,
                FileItemMenu = cmsFileItem,
                InstanceMenu = cmsInstance,
                AppMenu = cmsApp
            });
            frmStartPage = null;
            preventNodeExpand = false;
        }


        /// <summary>
        /// Gets the application log.
        /// </summary>
        private ILog Log => appData.ErrLog;

        /// <summary>
        /// Gets or sets the project currently open.
        /// </summary>
        private ScadaProject Project => appData.CurrentProject;

        /// <summary>
        /// Gets or sets the explorer width.
        /// </summary>
        public int ExplorerWidth
        {
            get
            {
                return pnlLeft.Width;
            }
            set
            {
                pnlLeft.Width = Math.Max(value, splVert.MinSize);
            }
        }

        /// <summary>
        /// Gets the explorer tree.
        /// </summary>
        TreeView IMainForm.ExplorerTree => tvExplorer;

        /// <summary>
        /// Gets the selected node of the explorer tree.
        /// </summary>
        TreeNode IMainForm.SelectedNode => tvExplorer.SelectedNode;

        /// <summary>
        /// Gets the item type of the configuration database table of the active child form.
        /// </summary>
        Type IMainForm.ActiveBaseTable => (wctrlMain.ActiveForm as FrmBaseTable)?.ItemType;


        /// <summary>
        /// Applies localization to the form.
        /// </summary>
        private void LocalizeForm()
        {
            FormTranslator.Translate(this, GetType().FullName, new FormTranslatorOptions
            {
                ContextMenus = new ContextMenuStrip[] {
                    cmsProject, cmsCnlTable, cmsDirectory, cmsFileItem, cmsInstance, cmsApp }
            });
            Text = AppPhrases.EmptyTitle;
            wctrlMain.MessageText = AppPhrases.WelcomeMessage;
            ofdProject.SetFilter(AppPhrases.ProjectFileFilter);
        }

        /// <summary>
        /// Adds main menu items and toolbar buttons created by the extensions.
        /// </summary>
        private void AddMainMenuItems()
        {
            // main menu items
            ArrayList existingMenuItems = new(miTools.DropDownItems);
            miTools.DropDownItems.Clear();

            foreach (ToolStripItem item in appData.ExtensionHolder.GetMainMenuItems())
            {
                miTools.DropDownItems.Add(item);
            }

            if (miTools.DropDownItems.Count > 0)
                miTools.DropDownItems.Add(new ToolStripSeparator());

            foreach (ToolStripItem item in existingMenuItems)
            {
                miTools.DropDownItems.Add(item);
            }

            // toolbar buttons
            foreach (ToolStripItem item in appData.ExtensionHolder.GetToobarButtons())
            {
                tsMain.Items.Add(item);
            }
        }

        /// <summary>
        /// Takes the explorer images and loads them into an image list.
        /// </summary>
        private void TakeExplorerImages()
        {
            // loading images from resources instead of storing in image list prevents them from corruption
            ilExplorer.Images.Add("chrome.png", Resources.chrome);
            ilExplorer.Images.Add("comm.png", Resources.comm);
            ilExplorer.Images.Add("database.png", Resources.database);
            ilExplorer.Images.Add("empty.png", Resources.empty);
            ilExplorer.Images.Add("file.png", Resources.file);
            ilExplorer.Images.Add("folder_closed.png", Resources.folder_closed);
            ilExplorer.Images.Add("folder_open.png", Resources.folder_open);
            ilExplorer.Images.Add("instance.png", Resources.instance);
            ilExplorer.Images.Add("instances.png", Resources.instances);
            ilExplorer.Images.Add("project.png", Resources.project);
            ilExplorer.Images.Add("server.png", Resources.server);
            ilExplorer.Images.Add("table.png", Resources.table);
            ilExplorer.Images.Add("views.png", Resources.views);

            // add images provided by the extensions
            foreach (KeyValuePair<string, Image> pair in appData.ExtensionHolder.GetTreeViewImages())
            {
                ilExplorer.Images.Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Enables or disables main menu items and toolbar buttons.
        /// </summary>
        private void SetMenuItemsEnabled()
        {
            miFileSave.Enabled = btnFileSave.Enabled = false;
            miFileSaveAll.Enabled = btnFileSaveAll.Enabled = false;
            miFileCloseProject.Enabled = Project != null;
            SetDeployMenuItemsEnabled();
        }

        /// <summary>
        /// Loads the application state.
        /// </summary>
        private void LoadAppState()
        {
            if (appData.State.Load(Path.Combine(appData.AppDirs.ConfigDir, AppState.DefaultFileName),
                out string errMsg))
            {
                appData.State.MainFormState.Apply(this);
                ofdProject.InitialDirectory = appData.State.ProjectDir;
            }
            else
            {
                Log.HandleError(errMsg);
            }
        }

        /// <summary>
        /// Saves the application state.
        /// </summary>
        private void SaveAppState()
        {
            appData.State.MainFormState.Retrieve(this);

            if (!appData.State.Save(Path.Combine(appData.AppDirs.ConfigDir, AppState.DefaultFileName),
                out string errMsg))
            {
                Log.HandleError(errMsg);
            }
        }

        /// <summary>
        /// Enables or disables the deployment menu items and tool buttons.
        /// </summary>
        private void SetDeployMenuItemsEnabled()
        {
            bool instanceExists = Project != null && Project.Instances.Count > 0;
            miDeployInstanceProfile.Enabled = btnDeployInstanceProfile.Enabled = instanceExists;
            miDeployDownloadConfig.Enabled = btnDeployDownloadConfig.Enabled = instanceExists;
            miDeployUploadConfig.Enabled = btnDeployUploadConfig.Enabled = instanceExists;
            miDeployInstanceStatus.Enabled = btnDeployInstanceStatus.Enabled = instanceExists;
        }

        /// <summary>
        /// Disables the Save All menu item and tool button if all data is saved.
        /// </summary>
        private void DisableSaveAll()
        {
            if (miFileSaveAll.Enabled && !Project.ConfigDatabase.Modified)
            {
                bool saveAllEnabled = false;

                foreach (Form form in wctrlMain.Forms)
                {
                    if (form is IChildForm childForm && childForm.ChildFormTag.Modified)
                    {
                        saveAllEnabled = true;
                        break;
                    }
                }

                miFileSaveAll.Enabled = btnFileSaveAll.Enabled = saveAllEnabled;
            }
        }

        /// <summary>
        /// Executes an action related to the tree node.
        /// </summary>
        private void ExecNodeAction(TreeNode treeNode)
        {
            if (treeNode?.Tag is TreeNodeTag tag)
            {
                if (tag.ExistingForm == null)
                {
                    if (tag.FormType != null)
                    {
                        // create a new form
                        object formObj = tag.FormArgs == null ?
                            Activator.CreateInstance(tag.FormType) :
                            Activator.CreateInstance(tag.FormType, tag.FormArgs);

                        // display the form
                        if (formObj is Form form)
                        {
                            tag.ExistingForm = form;
                            wctrlMain.AddForm(form, treeNode.FullPath, ilExplorer.Images[treeNode.ImageKey], treeNode);
                        }
                    }
                }
                else
                {
                    // activate the existing form
                    wctrlMain.ActivateForm(tag.ExistingForm);
                }
            }
        }

        /// <summary>
        /// Executes an action to open the file associated with the tree node.
        /// </summary>
        private void ExecOpenFileAction(TreeNode treeNode)
        {
            if (treeNode?.Tag is TreeNodeTag tag && tag.RelatedObject is FileItem fileItem)
            {
                if (tag.ExistingForm == null)
                {
                    if (appData.AppConfig.FileAssociations.TryGetValue(
                        AppUtils.GetExtensionLower(fileItem.Name), out string exePath) && File.Exists(exePath))
                    {
                        // run external editor
                        ScadaUiUtils.StartProcess(exePath, $"\"{fileItem.Path}\"");
                    }
                    else
                    {
                        // create editor form by extension or use default text editor
                        Form form = appData.ExtensionHolder.GetEditorForm(fileItem.Path) ??
                            new FrmTextEditor(appData, fileItem.Path);
                        tag.ExistingForm = form;
                        wctrlMain.AddForm(form, treeNode.FullPath, ilExplorer.Images[treeNode.ImageKey], treeNode);
                    }
                }
                else
                {
                    // activate existing form
                    wctrlMain.ActivateForm(tag.ExistingForm);
                }
            }
        }

        /// <summary>
        /// Finds a tree node that relates to the specified child form.
        /// </summary>
        private TreeNode FindTreeNode(Form form)
        {
            if (form is IChildForm childForm)
            {
                return childForm.ChildFormTag.TreeNode;
            }
            else
            {
                foreach (TreeNode node in tvExplorer.Nodes.IterateNodes())
                {
                    if (node.Tag is TreeNodeTag tag && tag.ExistingForm == form)
                        return node;
                }

                return null;
            }
        }

        /// <summary>
        /// Updates file names of the open editors corresponding to the specified tree node and its children.
        /// </summary>
        private void UpdateTextEditorFileNames(TreeNode treeNode)
        {
            if (treeNode == null)
                return;

            foreach (TreeNode node in treeNode.IterateNodes())
            {
                if (node.Tag is TreeNodeTag tag && tag.RelatedObject is FileItem fileItem &&
                    node.Parent.GetRelatedObject() is FileItem parentFileItem)
                {
                    fileItem.Path = Path.Combine(parentFileItem.Path, fileItem.Name);

                    if (tag.ExistingForm is IChildForm childForm)
                    {
                        wctrlMain.UpdateHint(tag.ExistingForm, node.FullPath);
                        childForm.ChildFormTag.SendMessage(this, AdminMessage.UpdateFileName,
                            new Dictionary<string, object> { { "FileName", fileItem.Path } });
                    }
                }
            }
        }

        /// <summary>
        /// Prepares to close all forms.
        /// </summary>
        private void PrepareToCloseAll()
        {
            foreach (Form form in wctrlMain.Forms)
            {
                if (form is FrmBaseTable frmBaseTable)
                    frmBaseTable.PrepareToClose();
            }
        }

        /// <summary>
        /// Saves the child forms corresponding to the specified tree node and its children.
        /// </summary>
        private static void SaveChildForms(TreeNode treeNode)
        {
            if (treeNode == null)
                return;

            foreach (TreeNode node in treeNode.IterateNodes())
            {
                if (node.Tag is TreeNodeTag tag &&
                    tag.ExistingForm is IChildForm childForm && childForm.ChildFormTag.Modified)
                {
                    childForm.Save();
                }
            }
        }

        /// <summary>
        /// Gets the path associated with the specified tree node.
        /// </summary>
        private bool TryGetFilePath(TreeNode treeNode, out string path)
        {
            if (treeNode?.Tag is TreeNodeTag tag)
            {
                if (tag.NodeType == ExplorerNodeType.Project)
                {
                    path = Project.ProjectDir;
                    return true;
                }
                else if (tag.NodeType == ExplorerNodeType.Directory || tag.NodeType == ExplorerNodeType.File)
                {
                    FileItem fileItem = (FileItem)tag.RelatedObject;
                    path = fileItem.Path;
                    return true;
                }
                else if (tag.NodeType == ExplorerNodeType.Views)
                {
                    path = Project.Views.ViewDir;
                    return true;
                }
                else if (tag.NodeType == ExplorerNodeType.Instance)
                {
                    if (FindClosestInstance(treeNode, out LiveInstance liveInstance))
                    {
                        path = liveInstance.ProjectInstance.InstanceDir;
                        return true;
                    }
                }
                else if (tag.NodeType == ExplorerNodeType.App)
                {
                    ProjectApp projectApp = (ProjectApp)tag.RelatedObject;
                    path = projectApp.AppDir;
                    return true;
                }
                else if (tag.NodeType == ExplorerNodeType.AppConfig)
                {
                    ProjectApp projectApp = (ProjectApp)tag.RelatedObject;
                    path = projectApp.ConfigDir;
                    return true;
                }
            }

            path = "";
            return false;
        }

        /// <summary>
        /// Finds the closest instance in the explorer starting from the specified tree node and traversing up.
        /// </summary>
        private static bool FindClosestInstance(TreeNode treeNode, out LiveInstance liveInstance)
        {
            TreeNode instanceNode = treeNode?.FindClosest(ExplorerNodeType.Instance);

            if (instanceNode == null)
            {
                liveInstance = null;
                return false;
            }
            else
            {
                liveInstance = (LiveInstance)instanceNode.GetRelatedObject();
                return true;
            }
        }

        /// <summary>
        /// Finds an instance selected for deploy.
        /// </summary>
        private bool FindInstanceForDeploy(TreeNode treeNode, out TreeNode instanceNode, out LiveInstance liveInstance)
        {
            if (Project != null)
            {
                instanceNode = treeNode?.FindClosest(ExplorerNodeType.Instance);

                if (instanceNode == null && Project.Instances.Count > 0)
                    instanceNode = explorerBuilder.InstancesNode.FindFirst(ExplorerNodeType.Instance);

                if (instanceNode != null)
                {
                    liveInstance = (LiveInstance)instanceNode.GetRelatedObject();
                    return true;
                }
            }

            instanceNode = null;
            liveInstance = null;
            return false;
        }

        /// <summary>
        /// Prepares data and fills the instance tree node.
        /// </summary>
        private bool PrepareInstanceNode(TreeNode instanceNode, LiveInstance liveInstance)
        {
            if (liveInstance.IsReady)
            {
                return false; // already prepared
            }
            else
            {
                if (liveInstance.ProjectInstance.LoadAppConfig(out string errMsg))
                {
                    LoadDeploymentConfig();
                    InitAgentClient(liveInstance);
                    explorerBuilder.FillInstanceNode(instanceNode);
                    liveInstance.IsReady = true;
                }
                else
                {
                    Log.HandleError(errMsg);
                }

                return true; // just prepared
            }
        }

        /// <summary>
        /// Prepares data and fills the instance tree node.
        /// </summary>
        private void PrepareInstanceNode(TreeNode instanceNode)
        {
            if (instanceNode.GetRelatedObject() is LiveInstance liveInstance)
                PrepareInstanceNode(instanceNode, liveInstance);
        }

        /// <summary>
        /// Refreshes the content of the instance tree node.
        /// </summary>
        private void RefreshInstanceNode(TreeNode instanceNode, LiveInstance liveInstance)
        {
            if (!liveInstance.IsReady)
                PrepareInstanceNode(instanceNode, liveInstance);
            else if (liveInstance.ProjectInstance.ConfigLoaded)
                explorerBuilder.FillInstanceNode(instanceNode);
            else if (liveInstance.ProjectInstance.LoadAppConfig(out string errMsg))
                explorerBuilder.FillInstanceNode(instanceNode);
            else
                Log.HandleError(errMsg);
        }

        /// <summary>
        /// Gets the current deployment profile of the instance.
        /// </summary>
        private DeploymentProfile GetDeploymentProfile(LiveInstance liveInstance)
        {
            string profileName = liveInstance.ProjectInstance.DeploymentProfile;

            if (!string.IsNullOrEmpty(profileName) &&
                Project.DeploymentConfig.Profiles.TryGetValue(profileName, out DeploymentProfile profile))
            {
                return profile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes an Agent client of the specified instance.
        /// </summary>
        private void InitAgentClient(LiveInstance liveInstance)
        {
            liveInstance.AgentClient = CreateAgentClient(liveInstance);
        }

        /// <summary>
        /// Creates a new Agent client for the specified instance.
        /// </summary>
        private IAgentClient CreateAgentClient(LiveInstance liveInstance)
        {
            DeploymentProfile profile = GetDeploymentProfile(liveInstance);
            return profile != null && profile.AgentEnabled
                ? new AgentClient(profile.AgentConnectionOptions)
                : null;
        }

        /// <summary>
        /// Handles the deployment changes.
        /// </summary>
        private void HandleDeploymentChanges(IDeploymentForm deploymentForm, LiveInstance liveInstance)
        {
            if (deploymentForm.ProfileChanged)
            {
                InitAgentClient(liveInstance);
                SaveProject();
                ShowStatus(liveInstance.ProjectInstance);
                MessageToChildForms(AdminMessage.UpdateAgentClient);
            }
            else if (deploymentForm.ConnectionModified)
            {
                InitAgentClient(liveInstance);
                MessageToChildForms(AdminMessage.UpdateAgentClient);
            }
        }

        /// <summary>
        /// Sends the specified message to the child forms that support IChildForm.
        /// </summary>
        private void MessageToChildForms(string message)
        {
            foreach (Form form in wctrlMain.Forms)
            {
                if (form is IChildForm childForm)
                    childForm.ChildFormTag.SendMessage(this, message);
            }
        }

        /// <summary>
        /// Loads the deployment configuration of the current project.
        /// </summary>
        private void LoadDeploymentConfig()
        {
            if (!Project.DeploymentConfig.Loaded && File.Exists(Project.DeploymentConfig.FileName) &&
                !Project.DeploymentConfig.Load(out string errMsg))
            {
                Log.HandleError(errMsg);
            }
        }

        /// <summary>
        /// Saves the deployment configuration of the current project.
        /// </summary>
        private void SaveDeploymentConfig()
        {
            if (!Project.DeploymentConfig.Save(out string errMsg))
                Log.HandleError(errMsg);
        }

        /// <summary>
        /// Saves the current project.
        /// </summary>
        private void SaveProject()
        {
            if (!Project.Save(Project.FileName, out string errMsg))
                Log.HandleError(errMsg);
        }

        /// <summary>
        /// Loads the configuration database.
        /// </summary>
        private void LoadConfigDatabase()
        {
            if (!Project.ConfigDatabase.Load(out string errMsg))
                Log.HandleError(errMsg);
        }

        /// <summary>
        /// Saves the configuration database.
        /// </summary>
        private bool SaveConfigDatabase()
        {
            if (Project.ConfigDatabase.Save(out string errMsg))
            {
                return true;
            }
            else
            {
                Log.HandleError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Save all open forms.
        /// </summary>
        private void SaveAll()
        {
            foreach (Form form in wctrlMain.Forms)
            {
                if (form is IChildForm childForm && childForm.ChildFormTag.Modified)
                    childForm.Save();
            }

            SaveConfigDatabase();
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        private void NewProject()
        {
            FrmProjectNew frmNewProject = new(appData);

            if (frmNewProject.ShowDialog() == DialogResult.OK && CloseProject())
            {
                CloseStartPage();

                if (ScadaProject.Create(frmNewProject.ProjectName, frmNewProject.ProjectLocation,
                    frmNewProject.ProjectTemplate, out ScadaProject newProject, out string errMsg))
                {
                    appData.State.AddRecentProject(newProject.FileName);
                    appData.CurrentProject = newProject;
                    LoadConfigDatabase();
                    Text = string.Format(AppPhrases.ProjectTitle, Project.Name);
                    wctrlMain.MessageText = AppPhrases.SelectItemMessage;
                    SetMenuItemsEnabled();
                    explorerBuilder.CreateNodes(Project);
                }
                else
                {
                    Log.HandleError(errMsg);
                }
            }
        }

        /// <summary>
        /// Opens the project.
        /// </summary>
        private void OpenProject(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                ofdProject.FileName = "";

                if (ofdProject.ShowDialog() == DialogResult.OK)
                    fileName = ofdProject.FileName;
            }

            if (!string.IsNullOrEmpty(fileName) && CloseProject())
            {
                CloseStartPage();
                ofdProject.InitialDirectory = Path.GetDirectoryName(fileName);
                appData.CurrentProject = new ScadaProject();

                if (Project.Load(fileName, out string errMsg))
                    appData.State.AddRecentProject(Project.FileName);
                else
                    Log.HandleError(errMsg);

                LoadConfigDatabase();
                Text = string.Format(AppPhrases.ProjectTitle, Project.Name);
                wctrlMain.MessageText = AppPhrases.SelectItemMessage;
                SetMenuItemsEnabled();
                explorerBuilder.CreateNodes(Project);
            }
        }

        /// <summary>
        /// Closes the project.
        /// </summary>
        private bool CloseProject()
        {
            if (Project == null)
            {
                return true;
            }
            else
            {
                PrepareToCloseAll();
                wctrlMain.CloseAllForms(out bool cancel);

                if (!cancel && Project.ConfigDatabase.Modified)
                {
                    switch (MessageBox.Show(AppPhrases.SaveConfigDatabaseConfirm,
                        CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            cancel = !SaveConfigDatabase();
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            cancel = true;
                            break;
                    }
                }

                if (cancel)
                {
                    return false;
                }
                else
                {
                    appData.CurrentProject = null;
                    Text = AppPhrases.EmptyTitle;
                    wctrlMain.MessageText = AppPhrases.WelcomeMessage;
                    SetMenuItemsEnabled();
                    tvExplorer.Nodes.Clear();
                    ShowStatus(null);
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets the start project from the command line arguments.
        /// </summary>
        private static bool GetStartProject(out string fileName)
        {
            string[] args = Environment.GetCommandLineArgs();
            fileName = args.Length > 1 ? args[1] : "";
            return File.Exists(fileName);
        }

        /// <summary>
        /// Shows the start page.
        /// </summary>
        private void ShowStartPage()
        {
            if (frmStartPage == null)
            {
                frmStartPage = new FrmStartPage(appData.State);
                wctrlMain.AddForm(frmStartPage, "", miFileShowStartPage.Image, null);
            }
            else
            {
                wctrlMain.ActivateForm(frmStartPage);
            }
        }

        /// <summary>
        /// Closes the start page.
        /// </summary>
        private void CloseStartPage()
        {
            frmStartPage?.Close();
        }

        /// <summary>
        /// Shows information in the status bar.
        /// </summary>
        private void ShowStatus(ProjectInstance projectInstance)
        {
            if (projectInstance == null)
            {
                lblSelectedInstance.Text = "";
                lblSelectedProfile.Text = "";
                lblSelectedProfile.Visible = false;
            }
            else
            {
                lblSelectedInstance.Text = projectInstance.Name;
                lblSelectedProfile.Text = projectInstance.DeploymentProfile;
                lblSelectedProfile.Visible = !string.IsNullOrEmpty(projectInstance.DeploymentProfile);
            }
        }

        /// <summary>
        /// Closes child forms corresponding to the specified tree node and its subnodes.
        /// </summary>
        private void CloseChildForms(TreeNode treeNode, bool saveChanges, bool skipRoot)
        {
            if (treeNode == null)
                return;

            foreach (TreeNode node in skipRoot ? treeNode.Nodes.IterateNodes() : treeNode.IterateNodes())
            {
                if (node.Tag is TreeNodeTag tag)
                    CloseChildForm(tag.ExistingForm, saveChanges);
            }
        }


        /// <summary>
        /// Closes the specified child form.
        /// </summary>
        public void CloseChildForm(Form form, bool saveChanges)
        {
            if (form != null)
            {
                if (saveChanges && form is IChildForm childForm && childForm.ChildFormTag.Modified)
                    childForm.Save();

                wctrlMain.CloseForm(form);
            }
        }

        /// <summary>
        /// Closes child forms corresponding to the specified tree node and its subnodes.
        /// </summary>
        public void CloseChildForms(TreeNode treeNode, bool saveChanges)
        {
            CloseChildForms(treeNode, saveChanges, false);
        }

        /// <summary>
        /// Updates hints of child form tabs corresponding to the specified tree node and its subnodes.
        /// </summary>
        public void UpdateChildFormHints(TreeNode treeNode)
        {
            if (treeNode == null)
                return;

            foreach (TreeNode node in treeNode.IterateNodes())
            {
                if (node.Tag is TreeNodeTag tag && tag.ExistingForm != null)
                {
                    wctrlMain.UpdateHint(tag.ExistingForm, node.FullPath);
                }
            }
        }

        /// <summary>
        /// Refreshes child forms that contains a configuration database table with the specified item type.
        /// </summary>
        public void RefreshBaseTables(Type itemType, bool saveChanges)
        {
            // save table
            bool tableSaved = false;

            if (saveChanges && Project != null &&
                Project.ConfigDatabase.GetTable(itemType) is IBaseTable baseTable && baseTable.Modified)
            {
                if (Project.ConfigDatabase.SaveTable(baseTable, out string errMsg))
                    tableSaved = true;
                else
                    appData.ErrLog.HandleError(errMsg);
            }

            // refresh child forms
            foreach (Form form in wctrlMain.Forms)
            {
                if (form is FrmBaseTable frmBaseTable && frmBaseTable.ItemType == itemType)
                    frmBaseTable.ChildFormTag.SendMessage(this, AdminMessage.RefreshData);
            }

            if (tableSaved)
                DisableSaveAll();
        }

        /// <summary>
        /// Finds a tree node that represents a configuration database table.
        /// </summary>
        public TreeNode FindBaseTableNode(Type itemType, object filterArgument)
        {
            ArgumentNullException.ThrowIfNull(itemType, nameof(itemType));

            if (Project != null && explorerBuilder.BaseTableNodes.TryGetValue(itemType.Name, out TreeNode treeNode))
            {
                if (filterArgument == null)
                {
                    return treeNode;
                }
                else if (itemType == typeof(Cnl) && filterArgument is int deviceNum)
                {
                    foreach (TreeNode childNode in treeNode.Nodes)
                    {
                        if (childNode.GetRelatedObject() is BaseTableItem baseTableItem &&
                            baseTableItem.DeviceNum == deviceNum)
                        {
                            return childNode;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds an instance tree node by instance name.
        /// </summary>
        public TreeNode FindInstanceNode(string instanceName, out bool justPrepared)
        {
            if (Project != null)
            {
                foreach (TreeNode treeNode in explorerBuilder.InstancesNode.Nodes)
                {
                    if (treeNode.GetRelatedObject() is LiveInstance liveInstance &&
                        string.Equals(liveInstance.ProjectInstance.Name, instanceName,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        justPrepared = PrepareInstanceNode(treeNode, liveInstance);
                        return treeNode;
                    }
                }
            }

            justPrepared = false;
            return null;
        }

        /// <summary>
        /// Gets the Agent client corresponding to the specified tree node.
        /// </summary>
        public IAgentClient GetAgentClient(TreeNode treeNode, bool createNew)
        {
            return FindInstanceForDeploy(treeNode, out _, out LiveInstance liveInstance)
                ? createNew
                    ? CreateAgentClient(liveInstance)
                    : liveInstance.AgentClient
                : null;
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();
            AddMainMenuItems();
            TakeExplorerImages();
            SetMenuItemsEnabled();
            LoadAppState();
            ShowStatus(null);

            if (GetStartProject(out string fileName))
                OpenProject(fileName);
            else
                ShowStartPage();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // confirm saving the project before closing
            e.Cancel = !CloseProject();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveAppState();
            appData.FinalizeApp();
        }


        private void tvExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            // execute a node action on press Enter
            if (e.KeyCode == Keys.Enter && tvExplorer.SelectedNode != null)
            {
                TreeNode selectedNode = tvExplorer.SelectedNode;
                if (selectedNode.TagIs(ExplorerNodeType.File))
                {
                    ExecOpenFileAction(selectedNode);
                }
                else if (selectedNode.Tag is TreeNodeTag tag && tag.FormType != null)
                {
                    ExecNodeAction(selectedNode);
                }
                else if (selectedNode.Nodes.Count > 0)
                {
                    if (selectedNode.IsExpanded)
                        selectedNode.Collapse(true);
                    else
                        selectedNode.Expand();
                }
            }
        }

        private void tvExplorer_MouseDown(object sender, MouseEventArgs e)
        {
            // check whether to prevent a node from expanding
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                TreeNode node = tvExplorer.GetNodeAt(e.Location);
                preventNodeExpand = node != null && node.Nodes.Count > 0 &&
                    node.Tag is TreeNodeTag tag && tag.FormType != null;
            }
        }

        private void tvExplorer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // select a tree node on right click
            if (e.Button == MouseButtons.Right && e.Node != null)
                tvExplorer.SelectedNode = e.Node;
        }

        private void tvExplorer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // execute a node action on double click
            if (e.Button == MouseButtons.Left)
            {
                TreeNode node = e.Node;
                if (node.TagIs(ExplorerNodeType.File))
                    ExecOpenFileAction(node);
                else
                    ExecNodeAction(node);
            }
        }

        private void tvExplorer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // prevent the node from expanding
            if (preventNodeExpand)
            {
                e.Cancel = true;
                preventNodeExpand = false;
                return;
            }

            // fill a node on demand
            TreeNode node = e.Node;

            if (node.TagIs(ExplorerNodeType.Views))
                explorerBuilder.FillViewsNode(node);
            else if (node.TagIs(ExplorerNodeType.Instance))
                PrepareInstanceNode(node);
            else if (node.TagIs(ExplorerNodeType.AppConfig))
                explorerBuilder.FillAppConfigNode(node);
        }

        private void tvExplorer_AfterExpand(object sender, TreeViewEventArgs e)
        {
            ExplorerBuilder.SetFolderImage(e.Node);
        }

        private void tvExplorer_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // prevent the node from collapsing
            if (preventNodeExpand)
            {
                e.Cancel = true;
                preventNodeExpand = false;
            }
        }

        private void tvExplorer_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            ExplorerBuilder.SetFolderImage(e.Node);
        }

        private void tvExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // show information about the selected instance
            if (FindInstanceForDeploy(tvExplorer.SelectedNode, out _, out LiveInstance liveInstance))
                ShowStatus(liveInstance.ProjectInstance);
            else
                ShowStatus(null);
        }


        private void wctrlMain_ActiveFormChanged(object sender, EventArgs e)
        {




                // enable or disable the Save menu item
            miFileSave.Enabled = btnFileSave.Enabled =
                wctrlMain.ActiveForm is IChildForm childForm && childForm.ChildFormTag.Modified;
            if (sender is WinControl.WinControl ctrl)
                if (ctrl.ActiveForm is FrmBaseTable table)
                    if (table.ItemType.Name == "Cnl") 
                        table.RefreshData();
                        

        }

        private void wctrlMain_ChildFormClosed(object sender, ChildFormClosedEventArgs e)
        {
            // clear the form pointer of the node
            TreeNode treeNode = FindTreeNode(e.ChildForm);

            if (treeNode?.Tag is TreeNodeTag tag)
                tag.ExistingForm = null;

            // clear the pointer to the start page
            if (e.ChildForm is FrmStartPage)
                frmStartPage = null;

            // disable the Save All menu item if needed
            if (e.ChildForm is IChildForm childForm && childForm.ChildFormTag.Modified)
                DisableSaveAll();
        }

        private void wctrlMain_ChildFormMessage(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AdminMessage.NewProject)
                NewProject();
            else if (e.Message == AdminMessage.OpenProject)
                OpenProject(e.GetArgument("Path") as string);
        }

        private void wctrlMain_ChildFormModifiedChanged(object sender, ChildFormEventArgs e)
        {
            // enable or disable the Save menu items
            miFileSave.Enabled = btnFileSave.Enabled =
                wctrlMain.ActiveForm is IChildForm activeForm && activeForm.ChildFormTag.Modified;

            if (e.ChildForm is IChildForm childForm)
            {
                if (childForm.ChildFormTag.Modified)
                    miFileSaveAll.Enabled = btnFileSaveAll.Enabled = true;
                else
                    DisableSaveAll();
            }
        }


        private void miFile_DropDownOpening(object sender, EventArgs e)
        {
            miFileClose.Enabled = wctrlMain.ActiveForm != null;
        }

        private void miFileNewProject_Click(object sender, EventArgs e)
        {
            NewProject();
        }

        private void miFileOpenProject_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void miFileShowStartPage_Click(object sender, EventArgs e)
        {
            ShowStartPage();
        }

        private void miFileSave_Click(object sender, EventArgs e)
        {
            // save the active form
            if (wctrlMain.ActiveForm is IChildForm childForm && childForm.ChildFormTag.Modified)
                childForm.Save();
        }

        private void miFileSaveAll_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        private void miFileClose_Click(object sender, EventArgs e)
        {
            miWindowCloseActive_Click(null, null);
        }

        private void miFileCloseProject_Click(object sender, EventArgs e)
        {
            CloseProject();
            ShowStartPage();
        }

        private void miFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miDeployInstanceProfile_Click(object sender, EventArgs e)
        {
            // select instance profile
            if (FindInstanceForDeploy(tvExplorer.SelectedNode, out _, out LiveInstance liveInstance))
            {
                // load deployment configuration
                LoadDeploymentConfig();

                // open instance profile form
                FrmInstanceProfile frmInstanceProfile = new(appData, Project, liveInstance.ProjectInstance);
                frmInstanceProfile.ShowDialog();

                // take changes into account
                HandleDeploymentChanges(frmInstanceProfile, liveInstance);
            }
        }

        private void miDeployDownloadConfig_Click(object sender, EventArgs e)
        {
            // download configuration
            if (FindInstanceForDeploy(tvExplorer.SelectedNode,
                out TreeNode instanceNode, out LiveInstance liveInstance))
            {
                // save all changes and load deployment configuration
                SaveAll();
                LoadDeploymentConfig();

                // open download configuration form
                FrmDownloadConfig frmDownloadConfig = new(appData, Project, liveInstance.ProjectInstance);
                frmDownloadConfig.ShowDialog();

                // take changes into account
                HandleDeploymentChanges(frmDownloadConfig, liveInstance);

                if (frmDownloadConfig.BaseModified)
                {
                    CloseChildForms(explorerBuilder.ConfigDatabaseNode, false);
                    SaveConfigDatabase();
                }

                if (frmDownloadConfig.ViewModified &&
                    TryGetFilePath(explorerBuilder.ViewsNode, out string path))
                {
                    CloseChildForms(explorerBuilder.ViewsNode, false);
                    explorerBuilder.FillFileNode(explorerBuilder.ViewsNode, path);
                }

                if (frmDownloadConfig.InstanceModified)
                {
                    liveInstance.ProjectInstance.ConfigLoaded = false;
                    CloseChildForms(instanceNode, false);
                    RefreshInstanceNode(instanceNode, liveInstance);
                }
            }
        }

        private void miDeployUploadConfig_Click(object sender, EventArgs e)
        {
            // upload configuration
            if (FindInstanceForDeploy(tvExplorer.SelectedNode, out _, out LiveInstance liveInstance))
            {
                // save all changes and load deployment configuration
                SaveAll();
                LoadDeploymentConfig();

                // open upload configuration form
                FrmUploadConfig frmUploadConfig = new(appData, Project, liveInstance.ProjectInstance);
                frmUploadConfig.ShowDialog();

                // take changes into account
                HandleDeploymentChanges(frmUploadConfig, liveInstance);
            }
        }

        private void miDeployInstanceStatus_Click(object sender, EventArgs e)
        {
            // display instance status
            if (FindInstanceForDeploy(tvExplorer.SelectedNode, out _, out LiveInstance liveInstance))
            {
                // load deployment settings
                LoadDeploymentConfig();

                // open instance status form
                FrmInstanceStatus frmInstanceStatus = new(appData, Project, liveInstance.ProjectInstance);
                frmInstanceStatus.ShowDialog();

                // take changes into account
                HandleDeploymentChanges(frmInstanceStatus, liveInstance);
            }
        }

        private void miToolsOptions_Click(object sender, EventArgs e)
        {
            // edit application configuration
            new FrmConfig(appData).ShowDialog();
        }

        private void miToolsCulture_Click(object sender, EventArgs e)
        {
            // show a form to select culture
            new FrmCulture(appData).ShowDialog();
        }

        private void miWindow_DropDownOpening(object sender, EventArgs e)
        {
            int formCount = wctrlMain.FormCount;
            miWindowCloseActive.Enabled = formCount > 0;
            miWindowCloseAll.Enabled = formCount > 0;
            miWindowCloseAllButActive.Enabled = formCount > 1;
        }

        private void miWindowCloseActive_Click(object sender, EventArgs e)
        {
            if (wctrlMain.ActiveForm is FrmBaseTable frmBaseTable)
                frmBaseTable.PrepareToClose();

            wctrlMain.CloseActiveForm(out _);
        }

        private void miWindowCloseAll_Click(object sender, EventArgs e)
        {
            PrepareToCloseAll();
            wctrlMain.CloseAllForms(out _);
        }

        private void miWindowCloseAllButActive_Click(object sender, EventArgs e)
        {
            wctrlMain.CloseAllButActive(out _);
        }

        private void miHelpDoc_Click(object sender, EventArgs e)
        {
            // open the documentation
            ScadaUiUtils.StartProcess(Locale.IsRussian ? AppUtils.DocRuUrl : AppUtils.DocEnUrl);
        }

        private void miHelpSupport_Click(object sender, EventArgs e)
        {
            // open the support forum
            ScadaUiUtils.StartProcess(Locale.IsRussian ? AppUtils.SupportRuUrl : AppUtils.SupportEnUrl);
        }

        private void miHelpAbout_Click(object sender, EventArgs e)
        {
            // show the about form
            FrmAbout.ShowAbout(appData);
        }


        private void miProjectRename_Click(object sender, EventArgs e)
        {
            // rename the selected project
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Project))
            {
                FrmItemName frmItemName = new() { ItemName = Project.Name };

                if (frmItemName.ShowDialog() == DialogResult.OK && frmItemName.Modified)
                {
                    if (!Project.Rename(frmItemName.ItemName, out string errMsg))
                        Log.HandleError(errMsg);

                    Text = string.Format(AppPhrases.ProjectTitle, Project.Name);
                    selectedNode.Text = Project.Name;
                    CloseChildForms(selectedNode, false);
                    explorerBuilder.FillInstancesNode();
                    SaveProject();
                }
            }
        }

        private void miProjectProperties_Click(object sender, EventArgs e)
        {
            // edit properties of the selected project
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Project))
            {
                FrmProjectProps frmProjectProps = new()
                {
                    ProjectName = Project.Name,
                    Version = Project.Version,
                    Description = Project.Description
                };

                if (frmProjectProps.ShowDialog() == DialogResult.OK && frmProjectProps.Modified)
                {
                    Project.Version = frmProjectProps.Version;
                    Project.Description = frmProjectProps.Description;
                    SaveProject();
                }
            }
        }


        private void cmsCnlTable_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu item
            miCnlTableComm.Enabled = tvExplorer.SelectedNode?.GetRelatedObject() is BaseTableItem baseTableItem &&
                baseTableItem.DeviceNum > 0;
        }

        private void miCnlTableComm_Click(object sender, EventArgs e)
        {
            // find a device tree node of Communicator
            if (tvExplorer.SelectedNode?.GetRelatedObject() is BaseTableItem baseTableItem &&
                baseTableItem.DeviceNum > 0)
            {
                int deviceNum = baseTableItem.DeviceNum;
                bool nodeFound = false;

                foreach (TreeNode treeNode in explorerBuilder.InstancesNode.Nodes.IterateNodes())
                {
                    if (treeNode.Tag is TreeNodeTag tag)
                    {
                        if (tag.NodeType == ExplorerNodeType.Instance)
                        {
                            PrepareInstanceNode(treeNode);
                        }
                        else if (tag.RelatedObject is DeviceConfig deviceConfig &&
                            deviceConfig.DeviceNum == deviceNum)
                        {
                            tvExplorer.SelectedNode = treeNode;
                            nodeFound = true;
                            break;
                        }
                    }
                }

                if (!nodeFound)
                    ScadaUiUtils.ShowWarning(AppPhrases.DeviceNotFoundInComm);
            }
        }

        private void miCnlTableRefresh_Click(object sender, EventArgs e)
        {
            // refresh channel table subnodes
            if (Project != null)
            {
                TreeNode cnlTableNode = explorerBuilder.BaseTableNodes[Project.ConfigDatabase.CnlTable.Name];
                CloseChildForms(cnlTableNode, true, true);
                explorerBuilder.FillCnlTableNode(cnlTableNode, Project.ConfigDatabase);
            }
        }


        private void cmsDirectory_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu items
            TreeNode selectedNode = tvExplorer.SelectedNode;
            bool isDirectoryNode = selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Directory);
            miDirectoryDelete.Enabled = isDirectoryNode;
            miDirectoryRename.Enabled = isDirectoryNode;
        }

        private void miDirectoryNewFile_Click(object sender, EventArgs e)
        {
            // create a new file in the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (TryGetFilePath(selectedNode, out string path))
            {
                FrmFileNew frmFileNew = new();

                if (frmFileNew.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string fileName = Path.Combine(path, frmFileNew.FileName);

                        if (File.Exists(fileName))
                        {
                            ScadaUiUtils.ShowError(AppPhrases.FileAlreadyExists);
                        }
                        else
                        {
                            FileCreator.CreateFile(fileName, frmFileNew.FileType);
                            explorerBuilder.InsertFileNode(selectedNode, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.HandleError(ex, AppPhrases.FileOperationError);
                    }
                }
            }
        }

        private void miDirectoryNewFolder_Click(object sender, EventArgs e)
        {
            // create a new subdirectory of the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (TryGetFilePath(selectedNode, out string path))
            {
                FrmItemName frmItemName = new();

                if (frmItemName.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string newDirectory = Path.Combine(path, frmItemName.ItemName);

                        if (Directory.Exists(newDirectory))
                        {
                            ScadaUiUtils.ShowError(AppPhrases.DirectoryAlreadyExists);
                        }
                        else
                        {
                            Directory.CreateDirectory(newDirectory);
                            explorerBuilder.InsertDirectoryNode(selectedNode, newDirectory);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.HandleError(ex, AppPhrases.FileOperationError);
                    }
                }
            }
        }

        private void miDirectoryDelete_Click(object sender, EventArgs e)
        {
            // delete the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Directory) &&
                TryGetFilePath(selectedNode, out string path) &&
                MessageBox.Show(AppPhrases.ConfirmDeleteDirectory, CommonPhrases.QuestionCaption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseChildForms(selectedNode, false);

                try
                {
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                    selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    Log.HandleError(ex, AppPhrases.FileOperationError);
                }
            }
        }

        private void miDirectoryRename_Click(object sender, EventArgs e)
        {
            // rename the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode?.Tag is TreeNodeTag tag && tag.NodeType == ExplorerNodeType.Directory)
            {
                FileItem fileItem = (FileItem)tag.RelatedObject;

                if (Directory.Exists(fileItem.Path))
                {
                    DirectoryInfo directoryInfo = new(fileItem.Path);
                    FrmItemName frmItemName = new() { ItemName = directoryInfo.Name };

                    if (frmItemName.ShowDialog() == DialogResult.OK && frmItemName.Modified)
                    {
                        try
                        {
                            string newDirectory = Path.Combine(directoryInfo.Parent.FullName, frmItemName.ItemName);
                            directoryInfo.MoveTo(newDirectory);
                            fileItem.Update(new DirectoryInfo(newDirectory));
                            selectedNode.Text = fileItem.Name;
                            UpdateTextEditorFileNames(selectedNode);
                        }
                        catch (Exception ex)
                        {
                            Log.HandleError(ex, AppPhrases.FileOperationError);
                        }
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.DirectoryNotExists);
                }
            }
        }

        private void miDirectoryOpenInExplorer_Click(object sender, EventArgs e)
        {
            // open the selected directory in File Explorer
            if (TryGetFilePath(tvExplorer.SelectedNode, out string path))
            {
                if (Directory.Exists(path))
                    ScadaUiUtils.StartProcess(path);
                else
                    ScadaUiUtils.ShowError(CommonPhrases.DirectoryNotExists);
            }
        }

        private void miDirectoryRefresh_Click(object sender, EventArgs e)
        {
            // refresh the tree nodes corresponding to the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (TryGetFilePath(selectedNode, out string path))
            {
                CloseChildForms(selectedNode, true);
                explorerBuilder.FillFileNode(selectedNode, path);
            }
        }


        private void cmsFileItem_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the Open menu item
            if (tvExplorer.SelectedNode.Tag is TreeNodeTag tag && tag.NodeType == ExplorerNodeType.File)
            {
                FileItem fileItem = (FileItem)tag.RelatedObject;
                miFileItemOpen.Enabled = FileCreator.ExtensionIsKnown(Path.GetExtension(fileItem.Path).TrimStart('.'));
            }
        }

        private void miFileItemOpen_Click(object sender, EventArgs e)
        {
            // open the selected file
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.File))
                ExecOpenFileAction(selectedNode);
        }

        private void miFileItemOpenLocation_Click(object sender, EventArgs e)
        {
            // open the selected file in File Explorer
            if (TryGetFilePath(tvExplorer.SelectedNode, out string path))
            {
                if (File.Exists(path))
                    ScadaUiUtils.StartProcess("explorer.exe", $"/select, \"{path}\"");
                else
                    ScadaUiUtils.ShowError(CommonPhrases.FileNotFound);
            }
        }

        private void miFileItemDelete_Click(object sender, EventArgs e)
        {
            // delete the selected file
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.File) &&
                TryGetFilePath(selectedNode, out string path) &&
                MessageBox.Show(AppPhrases.ConfirmDeleteFile, CommonPhrases.QuestionCaption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseChildForms(selectedNode, false);

                try
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    Log.HandleError(ex, AppPhrases.FileOperationError);
                }
            }
        }

        private void miFileItemRename_Click(object sender, EventArgs e)
        {
            // rename the selected file
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode?.Tag is TreeNodeTag tag && tag.NodeType == ExplorerNodeType.File)
            {
                FileItem fileItem = (FileItem)tag.RelatedObject;

                if (File.Exists(fileItem.Path))
                {
                    FileInfo fileInfo = new(fileItem.Path);
                    FrmItemName frmItemName = new() { ItemName = fileInfo.Name };

                    if (frmItemName.ShowDialog() == DialogResult.OK && frmItemName.Modified)
                    {
                        try
                        {
                            string newFileName = Path.Combine(fileInfo.DirectoryName, frmItemName.ItemName);
                            fileInfo.MoveTo(newFileName);
                            fileItem.Update(new FileInfo(newFileName));
                            selectedNode.Text = fileItem.Name;

                            if (tag.ExistingForm is IChildForm childForm)
                            {
                                wctrlMain.UpdateHint(tag.ExistingForm, selectedNode.FullPath);
                                childForm.ChildFormTag.SendMessage(this, AdminMessage.UpdateFileName,
                                    new Dictionary<string, object> { { "FileName", newFileName } });
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.HandleError(ex, AppPhrases.FileOperationError);
                        }
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.FileNotFound);
                }
            }
        }


        private void cmsInstance_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu items
            TreeNode selectedNode = tvExplorer.SelectedNode;
            bool isInstanceNode = selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Instance);
            bool instanceExists = Project != null && Project.Instances.Count > 0;

            miInstanceMoveUp.Enabled = isInstanceNode && selectedNode.PrevNode != null;
            miInstanceMoveDown.Enabled = isInstanceNode && selectedNode.NextNode != null;
            miInstanceDelete.Enabled = isInstanceNode;

            miInstanceProfile.Enabled = instanceExists;
            miInstanceDownloadConfig.Enabled = instanceExists;
            miInstanceUploadConfig.Enabled = instanceExists;
            miInstanceStatus.Enabled = instanceExists;

            miInstanceOpenInExplorer.Enabled = isInstanceNode;
            miInstanceOpenInBrowser.Enabled = isInstanceNode;
            miInstanceRename.Enabled = isInstanceNode;
            miInstanceProperties.Enabled = isInstanceNode;
        }

        private void miInstanceAdd_Click(object sender, EventArgs e)
        {
            // add a new instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null &&
                (selectedNode.TagIs(ExplorerNodeType.Instances) || selectedNode.TagIs(ExplorerNodeType.Instance)))
            {
                FrmInstanceEdit frmInstanceEdit = new();

                if (frmInstanceEdit.ShowDialog() == DialogResult.OK)
                {
                    if (Project.ContainsInstance(frmInstanceEdit.InstanceName))
                    {
                        ScadaUiUtils.ShowError(AppPhrases.InstanceAlreadyExists);
                    }
                    else
                    {
                        ProjectInstance projectInstance = Project.CreateInstance(frmInstanceEdit.InstanceName);
                        projectInstance.ServerApp.Enabled = frmInstanceEdit.ServerAppEnabled;
                        projectInstance.CommApp.Enabled = frmInstanceEdit.CommAppEnabled;
                        projectInstance.WebApp.Enabled = frmInstanceEdit.WebAppEnabled;

                        if (projectInstance.CreateInstanceFiles(out string errMsg))
                        {
                            TreeNode instancesNode = selectedNode.FindClosest(ExplorerNodeType.Instances);
                            TreeNode instanceNode = explorerBuilder.CreateInstanceNode(projectInstance);
                            instanceNode.Expand();
                            tvExplorer.Insert(instancesNode, instanceNode, Project.Instances, projectInstance);
                            SetDeployMenuItemsEnabled();
                            SaveProject();
                        }
                        else
                        {
                            Log.HandleError(errMsg);
                        }
                    }
                }
            }
        }

        private void miInstanceMoveUp_Click(object sender, EventArgs e)
        {
            // move up the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Instance))
            {
                tvExplorer.MoveUpSelectedNode(Project.Instances);
                SaveProject();
            }
        }

        private void miInstanceMoveDown_Click(object sender, EventArgs e)
        {
            // move down the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Instance))
            {
                tvExplorer.MoveDownSelectedNode(Project.Instances);
                SaveProject();
            }
        }

        private void miInstanceDelete_Click(object sender, EventArgs e)
        {
            // delete the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance) &&
                MessageBox.Show(AppPhrases.ConfirmDeleteInstance, CommonPhrases.QuestionCaption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseChildForms(selectedNode, false);
                tvExplorer.RemoveNode(selectedNode, Project.Instances);

                if (!liveInstance.ProjectInstance.DeleteInstanceFiles(out string errMsg))
                    Log.HandleError(errMsg);

                if (Project.DeploymentConfig.RemoveProfilesByInstance(liveInstance.ProjectInstance.ID))
                    SaveDeploymentConfig();

                SetDeployMenuItemsEnabled();
                SaveProject();
            }
        }

        private void miInstanceOpenInBrowser_Click(object sender, EventArgs e)
        {
            // open the web application of the instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                LoadDeploymentConfig();
                DeploymentProfile profile = GetDeploymentProfile(liveInstance);

                if (profile != null && ScadaUtils.IsValidUrl(profile.WebUrl))
                    ScadaUiUtils.StartProcess(profile.WebUrl);
                else
                    ScadaUiUtils.ShowWarning(AppPhrases.WebUrlNotSet);
            }
        }

        private void miInstanceRename_Click(object sender, EventArgs e)
        {
            // rename the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                ProjectInstance projectInstance = liveInstance.ProjectInstance;
                FrmItemName frmItemName = new()
                {
                    ItemName = projectInstance.Name,
                    ExistingNames = Project.GetInstanceNames(true, projectInstance.Name)
                };

                if (frmItemName.ShowDialog() == DialogResult.OK && frmItemName.Modified)
                {
                    if (!projectInstance.Rename(frmItemName.ItemName, out string errMsg))
                        Log.HandleError(errMsg);

                    if (Project.DeploymentConfig.UpdateInstanceName(projectInstance.ID, projectInstance.Name))
                        SaveDeploymentConfig();

                    selectedNode.Text = projectInstance.Name;
                    CloseChildForms(selectedNode, false);
                    RefreshInstanceNode(selectedNode, liveInstance);
                    InitAgentClient(liveInstance);
                    SaveProject();
                    ShowStatus(projectInstance);
                }
            }
        }

        private void miInstanceProperties_Click(object sender, EventArgs e)
        {
            // edit properties of the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(ExplorerNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                FrmInstanceEdit frmInstanceEdit = new() { EditMode = true };
                ProjectInstance projectInstance = liveInstance.ProjectInstance;
                frmInstanceEdit.Init(projectInstance);

                if (frmInstanceEdit.ShowDialog() == DialogResult.OK)
                {
                    // save the forms corresponding to the instance
                    SaveChildForms(selectedNode);

                    // enable or disable the applications
                    bool projectModified = false;

                    foreach (ProjectApp projectApp in projectInstance.AllApps)
                    {
                        bool prevState = projectApp.Enabled;
                        bool curState = frmInstanceEdit.GetAppEnabled(projectApp);

                        if (!prevState && curState)
                        {
                            if (projectApp.CreateConfigFiles(out string errMsg))
                            {
                                projectApp.Enabled = true;
                                projectModified = true;
                            }
                            else
                            {
                                Log.HandleError(errMsg);
                            }
                        }

                        if (prevState && !curState)
                        {
                            if (projectApp.DeleteConfigFiles(out string errMsg))
                            {
                                projectApp.ClearConfig();
                                projectApp.Enabled = false;
                                projectModified = true;
                            }
                            else
                            {
                                Log.HandleError(errMsg);
                            }
                        }
                    }

                    // save the changes and update the explorer
                    if (projectModified)
                    {
                        CloseChildForms(selectedNode, false);
                        RefreshInstanceNode(selectedNode, liveInstance);
                        SaveProject();
                    }
                }
            }
        }


        private void miAppReloadConfig_Click(object sender, EventArgs e)
        {
            // reload application configuration
            if (tvExplorer.GetSelectedObject() is ProjectApp projectApp)
            {
                CloseChildForms(tvExplorer.SelectedNode, false);
                projectApp.ConfigLoaded = false;

                if (projectApp.LoadConfig(out string errMsg))
                    explorerBuilder.FillAppNode(tvExplorer.SelectedNode);
                else
                    Log.HandleError(errMsg);
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TreeNode selectedNode = tvExplorer.SelectedNode;
            //if(selectedNode != null
            //    && selectedNode.TagIs(ExplorerNodeType.Views) ){
            //    TreeNode node = explorerBuilder;
            //}


        }
    }
}
