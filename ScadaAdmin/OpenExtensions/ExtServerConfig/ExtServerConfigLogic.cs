// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtServerConfig.Code;
using Scada.Admin.Extensions.ExtServerConfig.Forms;
using Scada.Admin.Extensions.ExtServerConfig.Properties;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Lang;
using Scada.Server.Config;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtServerConfig
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtServerConfigLogic : ExtensionLogic
    {
        /// <summary>
        /// Specifies the image keys.
        /// </summary>
        private static class ImageKey
        {
            private const string ImagePrefix = "server_config_";
            public const string Archive = ImagePrefix + "archive.png";
            public const string GeneralOptions = ImagePrefix + "general_options.png";
            public const string Module = ImagePrefix + "module.png";
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtServerConfigLogic(IAdminContext adminContext)
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
                return "ExtServerConfig";
            }
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
            if (relatedObject is not ServerApp serverApp)
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
                        FormArgs = new object[] { AdminContext.ErrLog, serverApp }
                    }
                },
                new TreeNode(ExtensionPhrases.ModulesNode)
                {
                    ImageKey = ImageKey.Module,
                    SelectedImageKey = ImageKey.Module,
                    Tag = new TreeNodeTag
                    {
                        FormType = typeof(FrmModules),
                        FormArgs = new object[] { AdminContext, serverApp }
                    }
                },
                new TreeNode(ExtensionPhrases.ArchivesNode)
                {
                    ImageKey = ImageKey.Archive,
                    SelectedImageKey = ImageKey.Archive,
                    Tag = new TreeNodeTag
                    {
                        FormType = typeof(FrmArchives),
                        FormArgs = new object[] { AdminContext, serverApp }
                    }
                },
            };
        }

        /// <summary>
        /// Gets the images used by the explorer tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { ImageKey.Archive, Resources.archive },
                { ImageKey.GeneralOptions, Resources.general_options },
                { ImageKey.Module, Resources.module },
            };
        }
    }
}
