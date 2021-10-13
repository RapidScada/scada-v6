// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
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
            public const string Lines = ImagePrefix + "lines.png";
            public const string LineOptions = ImagePrefix + "general_options.png";
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtCommConfigLogic(IAdminContext adminContext)
            : base(adminContext)
        {
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
        private TreeNode CreateCommLinesNode(CommApp commApp)
        {
            TreeNode commLinesNode = TreeViewExtensions.CreateNode(ExtensionPhrases.LinesNode, ImageKey.Lines);

            foreach (LineConfig lineConfig in commApp.AppConfig.Lines)
            {
                commLinesNode.Nodes.Add(CreateCommLineNode(lineConfig));
            }

            return commLinesNode;
        }

        /// <summary>
        /// Creates a tree node that represents communication lines.
        /// </summary>
        private TreeNode CreateCommLineNode(LineConfig lineConfig)
        {
            TreeNode commLineNode = TreeViewExtensions.CreateNode(
                CommUtils.GetLineTitle(lineConfig.CommLineNum, lineConfig.Name),
                lineConfig.Active ? ImageKey.Line : ImageKey.Line);

            commLineNode.Nodes.Add(TreeViewExtensions.CreateNode(
                ExtensionPhrases.LineOptionsNode, ImageKey.LineOptions));
            return commLineNode;
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
                        //FormType = typeof(FrmDrivers),
                        FormArgs = new object[] { AdminContext, commApp }
                    }
                },
                new TreeNode(ExtensionPhrases.DataSourcesNode)
                {
                    ImageKey = ImageKey.DataSource,
                    SelectedImageKey = ImageKey.DataSource,
                    Tag = new TreeNodeTag
                    {
                        //FormType = typeof(FrmDataSources),
                        FormArgs = new object[] { AdminContext, commApp }
                    }
                },
                CreateCommLinesNode(commApp)
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
                { ImageKey.Lines, Resources.lines }
            };
        }
    }
}
