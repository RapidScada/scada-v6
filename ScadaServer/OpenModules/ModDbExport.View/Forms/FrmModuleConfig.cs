// Copyright (c) Rapid Software LLC. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Scada.Client;
using Scada.Config;
using Scada.Data.Models;
using Scada.Dbms;
using Scada.Forms;
using Scada.Forms.Controls;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModDbExport.Config;
using Scada.Server.Modules.ModDbExport.View.Properties;

namespace Scada.Server.Modules.ModDbExport.View.Forms
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
        public FrmModuleConfig()
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
            ilTree.Images.Add(ImageKey.DbMуsql, Resources.db_mysql_inactive);
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
                }
            }
            finally
            {
                tvTargets.EndUpdate();
            }
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

            groupNode.Nodes.Add(TreeViewExtensions.CreateNode(ModulePhrases.ConnectionOptionsNode,
                ChooseNodeImage(exportTargetConfig.ConnectionOptions), exportTargetConfig.ConnectionOptions));

            TreeNode exportOptionsNode = TreeViewExtensions.CreateNode(ModulePhrases.ExportOptionsNode,
                ChooseNodeImage(exportTargetConfig.ExportOptions), exportTargetConfig.ExportOptions);
            groupNode.Nodes.Add(exportOptionsNode);

            exportOptionsNode.Nodes.Add(TreeViewExtensions.CreateNode(ModulePhrases.CurrentExportOptionsNode,
                ChooseNodeImage(exportTargetConfig.ExportOptions.CurDataExportOptions), 
                exportTargetConfig.ExportOptions.CurDataExportOptions));

            exportOptionsNode.Nodes.Add(TreeViewExtensions.CreateNode(ModulePhrases.ArchiveExportOptionsNode,
                ChooseNodeImage(exportTargetConfig.ExportOptions.ArcReplicationOptions), 
                exportTargetConfig.ExportOptions.ArcReplicationOptions));

            TreeNode queriesNode  = TreeViewExtensions.CreateNode("Queries",
                ChooseNodeImage(exportTargetConfig.Queries), exportTargetConfig.Queries);
            groupNode.Nodes.Add(queriesNode);
            
            foreach (QueryOptions queryOptions in exportTargetConfig.Queries)
            {
                queriesNode.Nodes.Add(TreeViewExtensions.CreateNode(queryOptions.Name,
                    ChooseNodeImage(queryOptions), queryOptions));
            }

            return groupNode;
        }

        /// <summary>
        /// Selects a node image key corresponding to the specified object.
        /// </summary>
        private static string ChooseNodeImage(object obj)
        {
            if (obj is ExportTargetConfig exportTargetConfig)
            {
                if (exportTargetConfig.ConnectionOptions.DBMS == KnownDBMS.MSSQL.ToString())
                    return exportTargetConfig.GeneralOptions.Active ? ImageKey.DbMssql : ImageKey.DbMssqlInactive;
                else if (exportTargetConfig.ConnectionOptions.DBMS == KnownDBMS.MySQL.ToString())
                    return exportTargetConfig.GeneralOptions.Active ? ImageKey.DbMуsql : ImageKey.DbMуsqlInactive;
                else if (exportTargetConfig.ConnectionOptions.DBMS == KnownDBMS.Oracle.ToString())
                    return exportTargetConfig.GeneralOptions.Active ? ImageKey.DbOracle : ImageKey.DbOracleInactive;
                else if (exportTargetConfig.ConnectionOptions.DBMS == KnownDBMS.PostgreSQL.ToString())  
                    return exportTargetConfig.GeneralOptions.Active ? ImageKey.DbPostgresql : ImageKey.DbPostgresqlInactive;                    
                else
                    return ImageKey.Option;
            }
            else if (obj is Config.GeneralOptions)
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
            else if (obj is QueryOptionList)
            {
                return ImageKey.Query;
            }
            else if (obj is QueryOptions queryOptions)
            {
                return queryOptions.DataKind switch
                {
                    DataKind.Current => queryOptions.Active ? ImageKey.QueryCur : ImageKey.QueryCurInactive,
                    DataKind.Historical => queryOptions.Active ? ImageKey.QueryHist : ImageKey.QueryHistInactive,
                    DataKind.Event => queryOptions.Active ? ImageKey.QueryEvent : ImageKey.QueryEventInactive,
                    DataKind.EventAck => queryOptions.Active ? ImageKey.QueryAck : ImageKey.QueryAckInactive,
                    DataKind.Command => queryOptions.Active ? ImageKey.QueryCmd : ImageKey.QueryCmdInactive,
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
            btnMoveUp.Enabled = tvTargets.MoveUpSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent) &&
                tvTargets.SelectedNode.Parent == null;
            btnMoveDown.Enabled = tvTargets.MoveDownSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent) &&
                tvTargets.SelectedNode.Parent == null;
            btnDelete.Enabled = btnCut.Enabled = btnCopy.Enabled = selectedNode != null &&
                tvTargets.SelectedNode.Parent == null;
        }


        private void FrmModuleConfig_Load(object sender, EventArgs e)
        {
            // translate form
            FormTranslator.Translate(this, GetType().FullName,
                new FormTranslatorOptions { ContextMenus = new ContextMenuStrip[] { cmsTree } });
        
            // load configuration
            if (File.Exists(configFileName) && !config.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            configCopy = config.DeepClone();
            Modified = false;

            // display configuration
            TakeTreeViewImages();
            FillTreeView();
        }
    }
}
