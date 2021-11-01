// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Extensions.ExtCommConfig.Controls;
using Scada.Admin.Extensions.ExtCommConfig.Forms;
using Scada.Admin.Extensions.ExtCommConfig.Properties;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Comm;
using Scada.Comm.Config;
using Scada.Forms;
using Scada.Lang;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtCommConfigLogic : ExtensionLogic
    {
        /// <summary>
        /// Specifies the image keys.
        /// </summary>
        private static class ImageKey
        {
            private const string ImagePrefix = "comm_config_";
            public const string DataSource = ImagePrefix + "data_source.png";
            public const string Driver = ImagePrefix + "driver.png";
            public const string GeneralOptions = ImagePrefix + "general_options.png";
            public const string Line = ImagePrefix + "line.png";
            public const string LineInactive = ImagePrefix + "line_inactive.png";
            public const string Lines = ImagePrefix + "lines.png";
            public const string LineOptions = ImagePrefix + "general_options.png";
        }


        private CtrlContextMenu ctrlContextMenu; // contains the context menus


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtCommConfigLogic(IAdminContext adminContext)
            : base(adminContext)
        {
            ctrlContextMenu = null;
        }


        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "ExtCommConfig";
            }
        }


        /// <summary>
        /// Creates a tree node that represents communication lines.
        /// </summary>
        private TreeNode CreateLinesNode(CommApp commApp)
        {
            TreeNode linesNode = TreeViewExtensions.CreateNode(ExtensionPhrases.LinesNode, ImageKey.Lines);
            linesNode.ContextMenuStrip = ctrlContextMenu.LineMenu;

            foreach (LineConfig lineConfig in commApp.AppConfig.Lines)
            {
                linesNode.Nodes.Add(CreateLineNode(commApp, lineConfig));
            }

            return linesNode;
        }

        /// <summary>
        /// Creates a tree node that represents the specified communication line.
        /// </summary>
        private TreeNode CreateLineNode(CommApp commApp, LineConfig lineConfig)
        {
            TreeNode lineNode = TreeViewExtensions.CreateNode(
                CommUtils.GetLineTitle(lineConfig.CommLineNum, lineConfig.Name),
                lineConfig.Active ? ImageKey.Line : ImageKey.LineInactive);
            lineNode.ContextMenuStrip = ctrlContextMenu.LineMenu;

            TreeNode lineOptionsNode = TreeViewExtensions.CreateNode(
                ExtensionPhrases.LineOptionsNode, ImageKey.LineOptions);
            
            lineOptionsNode.Tag = new TreeNodeTag
            {
                FormType = typeof(FrmLineConfig),
                FormArgs = new object[] { AdminContext, commApp, lineConfig }
            };

            lineNode.Nodes.Add(lineOptionsNode);
            return lineNode;
        }

        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AdminContext.AppDirs.LangDir, Code, out string errMsg))
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);

            ExtensionPhrases.Init();
        }

        /// <summary>
        /// Gets tree nodes to add to the explorer tree.
        /// </summary>
        public override TreeNode[] GetTreeNodes(object relatedObject)
        {
            if (relatedObject is not CommApp commApp)
                return null;

            // create context menus
            // do not create IWin32Window objects in constructor
            if (ctrlContextMenu == null)
            {
                ctrlContextMenu = new CtrlContextMenu();
                FormTranslator.Translate(ctrlContextMenu, ctrlContextMenu.GetType().FullName);
            }

            // create tree nodes
            return new TreeNode[]
            {
                new TreeNode(ExtensionPhrases.GeneralOptionsNode)
                {
                    ImageKey = ImageKey.GeneralOptions,
                    SelectedImageKey = ImageKey.GeneralOptions,
                    Tag = new TreeNodeTag
                    {
                        FormType = typeof(FrmGeneralOptions),
                        FormArgs = new object[] { AdminContext.ErrLog, commApp }
                    }
                },
                new TreeNode(ExtensionPhrases.DriversNode)
                {
                    ImageKey = ImageKey.Driver,
                    SelectedImageKey = ImageKey.Driver,
                    Tag = new TreeNodeTag
                    {
                        FormType = typeof(FrmDrivers),
                        FormArgs = new object[] { AdminContext, commApp }
                    }
                },
                new TreeNode(ExtensionPhrases.DataSourcesNode)
                {
                    ImageKey = ImageKey.DataSource,
                    SelectedImageKey = ImageKey.DataSource,
                    Tag = new TreeNodeTag
                    {
                        FormType = typeof(FrmDataSources),
                        FormArgs = new object[] { AdminContext, commApp }
                    }
                },
                CreateLinesNode(commApp)
            };
        }

        /// <summary>
        /// Gets the images used by the explorer tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { ImageKey.DataSource, Resources.data_source },
                { ImageKey.Driver, Resources.driver },
                { ImageKey.GeneralOptions, Resources.general_options },
                { ImageKey.Line, Resources.line },
                { ImageKey.LineInactive, Resources.line_inactive },
                { ImageKey.Lines, Resources.lines }
            };
        }
    }
}
