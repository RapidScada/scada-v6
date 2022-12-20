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
using Scada.Data.Models;
using Scada.Forms;
using Scada.Forms.Controls;
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
            public const string C = "connect.png";
            public const string Connect = "connect.png";
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
            TreeNode groupNode = TreeViewExtensions.CreateNode(exportTargetConfig, ImageKey.C);
            
            groupNode.Text = exportTargetConfig.GeneralOptions.Title;
            
            groupNode.Nodes.Add(TreeViewExtensions.CreateNode("General Options",
                ImageKey.C, exportTargetConfig.GeneralOptions));

            groupNode.Nodes.Add(TreeViewExtensions.CreateNode("Connection Options", 
                ImageKey.Connect, exportTargetConfig.ConnectionOptions));

            TreeNode exportOptionsNode = TreeViewExtensions.CreateNode("Export Options",
                ImageKey.C, exportTargetConfig.ExportOptions);
            groupNode.Nodes.Add(exportOptionsNode);

            exportOptionsNode.Nodes.Add(TreeViewExtensions.CreateNode("Curent Export Options",
                ImageKey.C, exportTargetConfig.ExportOptions.CurDataExportOptions));

            exportOptionsNode.Nodes.Add(TreeViewExtensions.CreateNode("Archive Export Options",
                ImageKey.C, exportTargetConfig.ExportOptions.ArcReplicationOptions));

            TreeNode queriesNode  = TreeViewExtensions.CreateNode("Queries",
                ImageKey.C, exportTargetConfig.Queries);
            groupNode.Nodes.Add(queriesNode);
            
            foreach (QueryOptions queryOptions in exportTargetConfig.Queries)
            {
                queriesNode.Nodes.Add(TreeViewExtensions.CreateNode("Query",
                    ImageKey.C, queryOptions.Name));
            }

            return groupNode;
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
