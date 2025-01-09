// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Dbms;
using Scada.Forms;
using Scada.Lang;
using Scada.Server.Lang;
using Scada.Server.Modules.ModDeviceAlarm.Config;
using Scada.Server.Modules.ModDeviceAlarm.View.Properties;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModDeviceAlarm.View.Forms
{
    /// <summary>
    /// Represents a form for editing a module configuration.
    /// </summary>
    public partial class FrmModuleConfig : Form
    {
        /// <summary>
        /// Specifies the image keys.
        /// </summary>
        private static class ImageKey
        {
            public const string DbMssql = "db_mssql.png";
            public const string DbMssqlInactive = "db_mssql_inactive.png";
            public const string DbMуsql = "db_mуsql.png";
            public const string DbMуsqlInactive = "db_mуsql_inactive.png";
            public const string DbOracle = "db_oracle.png";
            public const string DbOracleInactive = "db_oracle_inactive.png";
            public const string DbPostgresql = "db_postgresql.png";
            public const string DbPostgresqlInactive = "db_postgresql_inactive.png";
            public const string Connect = "connect.png";
            public const string ExportOption = "export_options.png";
            public const string Option = "options.png";
            public const string Query = "query";
            public const string QueryAck = "query_ack";
            public const string QueryAckInactive = "query_ack_inactive";
            public const string QueryCmd = "query_cmd";
            public const string QueryCmdInactive = "query_cmd_inactive";
            public const string QueryCur = "query_cur";
            public const string QueryCurInactive = "query_cur_inactive";
            public const string QueryEvent = "query_event";
            public const string QueryEventInactive = "query_event_inactive";
            public const string QueryHist = "query_hist";
            public const string QueryHistInactive = "query_hist_inactive";
        }

        private readonly string configFileName; // the configuration file name
        private ModuleConfig config;            // the module configuration
        private ModuleConfig configCopy;        // the configuration copy to revert changes
        private bool modified;                  // indicates that the module configuration is modified
        private object clipboard;               // contains a copied object


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmModuleConfig()
        {
            InitializeComponent();
            HideControls();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmModuleConfig(ConfigDataset configDataset, AppDirs appDirs)
            : this()
        {
            ArgumentNullException.ThrowIfNull(configDataset, nameof(configDataset));
            ArgumentNullException.ThrowIfNull(appDirs, nameof(appDirs));

            configFileName = Path.Combine(appDirs.ConfigDir, ModuleConfig.DefaultFileName);
            config = new ModuleConfig();
            configCopy = null;
            modified = false;
            clipboard = null;

            btnPaste.Enabled = false;
            ctrlGeneral.ConfigDataset = configDataset;
            ctrlQuery.ConfigDataset = configDataset;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the module configuration is modified.
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
                btnCancel.Enabled = modified;
            }
        }


        /// <summary>
        /// Hides the controls that display.
        /// </summary>
        private void HideControls()
        {
            ctrlGeneral.Visible = ctrlQuery.Visible = lblHint.Visible = false;
        }

        /// <summary>
        /// Takes the tree view images and loads them into an image list.
        /// </summary>
        private void TakeTreeViewImages()
        {
            // loading images from resources instead of storing in image list prevents them from corruption
            ilTree.Images.Add(ImageKey.DbMssql, Resources.db_mssql);
            ilTree.Images.Add(ImageKey.DbMssqlInactive, Resources.db_mssql_inactive);
            ilTree.Images.Add(ImageKey.DbMуsql, Resources.db_mysql);
            ilTree.Images.Add(ImageKey.DbMуsqlInactive, Resources.db_mysql_inactive);
            ilTree.Images.Add(ImageKey.DbOracle, Resources.db_oracle);
            ilTree.Images.Add(ImageKey.DbOracleInactive, Resources.db_oracle_inactive);
            ilTree.Images.Add(ImageKey.DbPostgresql, Resources.db_postgresql);
            ilTree.Images.Add(ImageKey.DbPostgresqlInactive, Resources.db_postgresql_inactive);
            ilTree.Images.Add(ImageKey.Connect, Resources.connect);
            ilTree.Images.Add(ImageKey.ExportOption, Resources.export_options);
            ilTree.Images.Add(ImageKey.Option, Resources.options);
            ilTree.Images.Add(ImageKey.Query, Resources.query);
            ilTree.Images.Add(ImageKey.QueryAck, Resources.query_ack);
            ilTree.Images.Add(ImageKey.QueryAckInactive, Resources.query_ack_inactive);
            ilTree.Images.Add(ImageKey.QueryCmd, Resources.query_cmd);
            ilTree.Images.Add(ImageKey.QueryCmdInactive, Resources.query_cmd_inactive);
            ilTree.Images.Add(ImageKey.QueryCur, Resources.query_cur);
            ilTree.Images.Add(ImageKey.QueryCurInactive, Resources.query_cur_inactive);
            ilTree.Images.Add(ImageKey.QueryEvent, Resources.query_event);
            ilTree.Images.Add(ImageKey.QueryEventInactive, Resources.query_event_inactive);
            ilTree.Images.Add(ImageKey.QueryHist, Resources.query_hist);
            ilTree.Images.Add(ImageKey.QueryHistInactive, Resources.query_hist_inactive);
        }

        /// <summary>
        /// Fills the tree view.
        /// </summary>
        private void FillTreeView()
        {
            try
            {
                tvTargets.BeginUpdate();
                tvTargets.Nodes.Clear();

                foreach (ExportTargetConfig exportTargetConfig in config.ExportTargets)
                {
                    tvTargets.Nodes.Add(CreateGroupNode(exportTargetConfig));
                }

                if (tvTargets.Nodes.Count > 0)
                {
                    tvTargets.Nodes[0].ExpandAll();
                    tvTargets.SelectedNode = tvTargets.Nodes[0].FirstNode;
                }
                else
                {
                    SetButtonsEnabled();
                    HideControls();
                    lblHint.Visible = true;
                    lblHint.Text = ModulePhrases.AddTargets;
                }
            }
            finally
            {
                tvTargets.EndUpdate();
            }
        }

        /// <summary>
        /// Gets next target ID.
        /// </summary>
        private int GetNextTargetID()
        {
            return config.ExportTargets.Count > 0 ? config.ExportTargets.Max(x => x.GeneralOptions.ID) + 1 : 1;
        }

        /// <summary>
        /// Creates a tree node according to the gates configuration.
        /// </summary>
        private static TreeNode CreateGroupNode(ExportTargetConfig exportTargetConfig)
        {
            TreeNode groupNode = TreeViewExtensions.CreateNode(exportTargetConfig, ChooseNodeImage(exportTargetConfig));
            groupNode.Text = exportTargetConfig.GeneralOptions.Title;

            groupNode.Nodes.Add(TreeViewExtensions.CreateNode(ModulePhrases.GeneralOptionsNode,
                ChooseNodeImage(exportTargetConfig.GeneralOptions), exportTargetConfig.GeneralOptions));

            TreeNode triggersNode = TreeViewExtensions.CreateNode(ModulePhrases.TriggersNode,
                ChooseNodeImage(exportTargetConfig.Triggers), exportTargetConfig.Triggers);
            groupNode.Nodes.Add(triggersNode);

            foreach (TriggerOptions queryOptions in exportTargetConfig.Triggers)
            {
                triggersNode.Nodes.Add(TreeViewExtensions.CreateNode(
                    string.IsNullOrEmpty(queryOptions.Name) ? ModulePhrases.UnnamedTrigger : queryOptions.Name,
                    ChooseNodeImage(queryOptions), queryOptions));
            }

            return groupNode;
        }

        /// <summary>
        /// Selects a node image key corresponding to the specified object.
        /// </summary>
        private static string ChooseNodeImage(object obj)
        {
            if (obj is GeneralOptions)
            {
                return ImageKey.Option;
            }
            else if (obj is DbConnectionOptions)
            {
                return ImageKey.Connect;
            }
            else if (obj is ExportOptions)
            {
                return ImageKey.ExportOption;
            }
            else if (obj is CurDataExportOptions)
            {
                return ImageKey.Option;
            }
            else if (obj is ArcReplicationOptions)
            {
                return ImageKey.Option;
            }
            else if (obj is TriggerOptionList)
            {
                return ImageKey.Query;
            }
            else if (obj is TriggerOptions queryOptions)
            {
                return queryOptions.TriggerKind switch
                {
                    TriggerKind.Status => queryOptions.Active ? ImageKey.QueryCur : ImageKey.QueryCurInactive,
                    TriggerKind.ValueUnchange => queryOptions.Active ? ImageKey.QueryEvent : ImageKey.QueryEventInactive,
                    _ => ImageKey.Option,
                };
            }
            else
            {
                return ImageKey.Option;
            }
        }

        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            TreeNode selectedNode = tvTargets.SelectedNode;

            btnAddStatusTrigger.Enabled = btnAddValueUnchangedTrigger.Enabled = selectedNode != null;

            btnMoveUp.Enabled = tvTargets.MoveUpSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent) &&
                (selectedNode.Parent == null || selectedNode.Tag is TriggerOptions);

            btnMoveDown.Enabled = tvTargets.MoveDownSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent) &&
                (selectedNode.Parent == null || selectedNode.Tag is TriggerOptions);

            btnDelete.Enabled = btnCut.Enabled = btnCopy.Enabled = selectedNode != null &&
                (selectedNode.Parent == null || selectedNode.Tag is TriggerOptions);
        }

        /// <summary>
        /// Checks targets name for uniqueness.
        /// </summary>
        private bool CheckTargetNamesUnique()
        {
            return config.ExportTargets.Count ==
                config.ExportTargets.DistinctBy(g => g.GeneralOptions.Name.ToLowerInvariant()).Count();
        }

        /// <summary>
        /// Checks the correctness of the gate.
        /// </summary>
        private bool ValidateConfig()
        {
            if (!CheckTargetNamesUnique())
            {
                ScadaUiUtils.ShowError(ModulePhrases.TargetNameNotUnique);
                return false;
            }
            else
            {
                return true;
            }
        }


        private void FrmModuleConfig_Load(object sender, EventArgs e)
        {
            // translate form
            FormTranslator.Translate(this, GetType().FullName,
                new FormTranslatorOptions { ContextMenus = new ContextMenuStrip[] { cmsTree } });
            FormTranslator.Translate(ctrlGeneral, ctrlGeneral.GetType().FullName);
            //FormTranslator.Translate(ctrlCurDataExport, ctrlCurDataExport.GetType().FullName);
            //FormTranslator.Translate(ctrlArcReplication, ctrlArcReplication.GetType().FullName);
            FormTranslator.Translate(ctrlQuery, ctrlQuery.GetType().FullName,
                new FormTranslatorOptions { ToolTip = ctrlQuery.ToolTip, SkipUserControls = false });

            // load configuration
            if (File.Exists(configFileName) && !config.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            configCopy = config.DeepClone();
            Modified = false;

            // display configuration
            TakeTreeViewImages();
            FillTreeView();
        }

        private void FrmModuleConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(ServerPhrases.SaveModuleConfigConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!config.Save(configFileName, out string errMsg))
                        {
                            ScadaUiUtils.ShowError(errMsg);
                            e.Cancel = true;
                        }
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        /// <summary>
        /// add target
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ExportTargetConfig target = new() { Parent = config };
            target.GeneralOptions.ID = GetNextTargetID();
            target.GeneralOptions.Name = string.Format(ModulePhrases.TargetName, target.GeneralOptions.ID);

            TreeNode targetNode = CreateGroupNode(target);
            tvTargets.Insert(null, targetNode, config.ExportTargets, target);
            tvTargets.SelectedNode = targetNode.FirstNode;
            Modified = true;
            ctrlGeneral.SetFocus();
        }

        private void btnAddQuery_Click(object sender, EventArgs e)
        {
            if (tvTargets.SelectedNode is not TreeNode selectedNode)
                return;

            TreeNode queriesNode =
                selectedNode.FindClosest(typeof(TriggerOptionList)) ??
                selectedNode.GetTopParentNode().FindFirst(typeof(TriggerOptionList));

            if (queriesNode != null)
            {
                // add query
                TriggerOptions queryOptions = new()
                {
                    Name = string.Format(ModulePhrases.TriggerName, queriesNode.GetNodeCount(true) + 1)
                };

                if (sender == btnAddStatusTrigger)
                    queryOptions.TriggerKind = TriggerKind.Status;
                else if (sender == btnAddValueUnchangedTrigger)
                    queryOptions.TriggerKind = TriggerKind.ValueUnchange;
                else
                    throw new ScadaException("Unknown query trigger kind.");

                tvTargets.Insert(queriesNode, TreeViewExtensions.CreateNode(queryOptions.Name,
                    ChooseNodeImage(queryOptions), queryOptions));
                Modified = true;
                ctrlQuery.SetFocus();
            }
        }

        private void ctrl_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            Modified = true;
        }

        private void ctrlGeneral_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (tvTargets.SelectedNode != null &&
                (e.ChangeArgument is not TreeUpdateTypes treeUpdateTypes ||
                treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode)) &&
                e.ChangedObject is GeneralOptions generalOptions)
            {
                tvTargets.SelectedNode.Parent.Text = generalOptions.Title;
                tvTargets.SelectedNode.Parent.SetImageKey(ChooseNodeImage(tvTargets.SelectedNode.Parent.Tag));
            }

            Modified = true;
        }

        private void ctrlQuery_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (tvTargets.SelectedNode != null &&
                (e.ChangeArgument is not TreeUpdateTypes treeUpdateTypes ||
                treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode)) &&
                e.ChangedObject is TriggerOptions queryOptions)
            {
                tvTargets.SelectedNode.Text =
                    string.IsNullOrEmpty(queryOptions.Name) ? ModulePhrases.UnnamedTrigger : queryOptions.Name;
                tvTargets.SelectedNode.SetImageKey(ChooseNodeImage(tvTargets.SelectedNode.Tag));
            }

            Modified = true;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            tvTargets.MoveUpSelectedNode(TreeNodeBehavior.WithinParent);
            Modified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            tvTargets.MoveDownSelectedNode(TreeNodeBehavior.WithinParent);
            Modified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            tvTargets.RemoveSelectedNode();
            Modified = true;

            if (tvTargets.Nodes.Count == 0)
            {
                SetButtonsEnabled();
                HideControls();
                lblHint.Visible = true;
                lblHint.Text = ModulePhrases.AddTargets;
            }
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            if (tvTargets.SelectedNode?.Tag is object obj)
            {
                clipboard = obj;
                btnPaste.Enabled = true;
            }

            btnDelete_Click(null, null);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (tvTargets.SelectedNode?.Tag is object obj)
            {
                clipboard = obj.DeepClone();
                btnPaste.Enabled = true;
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (clipboard is ExportTargetConfig exportTargetConfig)
            {
                // copy target
                ExportTargetConfig exportTargetConfigCopy = exportTargetConfig.DeepClone();
                exportTargetConfigCopy.GeneralOptions.Name += ModulePhrases.CopySuffix;
                exportTargetConfigCopy.GeneralOptions.ID = GetNextTargetID();
                exportTargetConfigCopy.Parent = config;
                exportTargetConfigCopy.RestoreHierarchy();
                TreeNode targetNode = CreateGroupNode(exportTargetConfigCopy);
                tvTargets.Insert(null, targetNode, config.ExportTargets, exportTargetConfigCopy);
                tvTargets.SelectedNode = targetNode.FirstNode;

                ctrlGeneral.SetFocus();
                Modified = true;
            }
            else if (clipboard is TriggerOptions queryOptions)
            {
                TriggerOptions queryOptionsCopy = queryOptions.DeepClone();
                queryOptionsCopy.Name += ModulePhrases.CopySuffix;
                queryOptionsCopy.Parent = config;
                queryOptionsCopy.RestoreHierarchy();
                TreeNode queryNode = TreeViewExtensions.CreateNode(queryOptionsCopy.Name,
                    ChooseNodeImage(queryOptionsCopy), queryOptionsCopy);

                if (tvTargets.SelectedNode?.Tag is TriggerOptions)
                    tvTargets.Insert(tvTargets.SelectedNode.Parent, queryNode);
                else if (tvTargets.SelectedNode?.Tag is TriggerOptionList)
                    tvTargets.Insert(tvTargets.SelectedNode, queryNode);

                ctrlQuery.SetFocus();
                Modified = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateConfig())
            {
                if (config.Save(configFileName, out string errMsg))
                    Modified = false;
                else
                    ScadaUiUtils.ShowError(errMsg);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // cancel configuration changes
            config = configCopy;
            configCopy = config.DeepClone();
            config.RestoreHierarchy();
            FillTreeView();
            Modified = false;
        }

        private void miCollapseAll_Click(object sender, EventArgs e)
        {
            if (tvTargets.Nodes.Count > 0)
            {
                tvTargets.SelectedNode = null;
                tvTargets.CollapseAll();
                tvTargets.SelectedNode = tvTargets.Nodes[0];
            }
        }

        private void tvTargets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // show properties of the selected object
            object selectedObject = e.Node.Tag;
            HideControls();

            if (selectedObject is GeneralOptions generalOptions)
            {
                ctrlGeneral.GeneralOptions = generalOptions;
                ctrlGeneral.Visible = true;
            }
            else if (selectedObject is TriggerOptions queryOptions)
            {
                ctrlQuery.QueryOptions = queryOptions;
                ctrlQuery.Visible = true;
            }
            else if (selectedObject is TriggerOptionList || selectedObject is ExportOptions ||
                selectedObject is ExportTargetConfig)
            {
                lblHint.Visible = true;
                lblHint.Text = ModulePhrases.SelectChildNode;
            }

            SetButtonsEnabled();
        }

        /// <summary>
        /// SMTP设置
        /// </summary>
        private void btnSmtpConfig_Click(object sender, EventArgs e)
        {
            var smtpConfig = new FrmSmtpConfig(config.EmailDeviceConfig);
            if (smtpConfig.ShowDialog() == DialogResult.OK)
            {
                Modified = true;
            }
        }
    }
}
