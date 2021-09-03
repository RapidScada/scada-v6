// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtServerConfig.Code;
using Scada.Admin.Extensions.ExtServerConfig.Forms;
using Scada.Admin.Extensions.ExtServerConfig.Properties;
using Scada.Agent;
using Scada.Forms;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtServerConfig
{
    /// <summary>
    /// Represents a plugin logic.
    /// <para>Представляет логику плагина.</para>
    /// </summary>
    public class ExtServerConfigLogic : ExtensionLogic
    {
        /// <summary>
        /// The prefix of the tree view images.
        /// </summary>
        private const string ImagePrefix = "server_config_";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtServerConfigLogic(IAdminContext adminContext)
            : base(adminContext)
        {
        }


        /// <summary>
        /// Gets tree nodes to add to the explorer tree.
        /// </summary>
        public override TreeNode[] GetTreeNodes(ConfigParts configPart, object appConfig)
        {
            // check the arguments
            if (configPart != ConfigParts.Server)
                return null;

            if (appConfig is not ServerConfig)
                throw new ArgumentException("Server config expected.", nameof(appConfig));

            // create nodes
            return new TreeNode[]
            {
                new TreeNode(ExtensionPhrases.CommonParamsNode)
                {
                    ImageKey = ImagePrefix + "general_options.png",
                    SelectedImageKey = ImagePrefix + "general_options.png",
                    Tag = new TreeNodeTag
                    {
                        FormType = typeof(FrmGeneralOptions),
                        FormArgs = new object[] { appConfig }
                    }
                },
                /*new TreeNode(ServerShellPhrases.SaveParamsNode)
                {
                    ImageKey = "server_save.png",
                    SelectedImageKey = "server_save.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmSaveParams),
                        FormArgs = new object[] { settings }
                    }
                },
                new TreeNode(ServerShellPhrases.ModulesNode)
                {
                    ImageKey = "server_module.png",
                    SelectedImageKey = "server_module.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmModules),
                        FormArgs = new object[] { settings, environment }
                    }
                },
                new TreeNode(ServerShellPhrases.ArchiveNode,
                    new TreeNode[]
                    {
                        new TreeNode(ServerShellPhrases.CurDataNode)
                        {
                            ImageKey = "server_data.png",
                            SelectedImageKey = "server_data.png",
                            Tag = new TreeNodeTag()
                            {
                                FormType = typeof(FrmArchive),
                                FormArgs = new object[] { settings, environment, ArcType.CurData }
                            }
                        },
                        new TreeNode(ServerShellPhrases.MinDataNode)
                        {
                            ImageKey = "server_data.png",
                            SelectedImageKey = "server_data.png",
                            Tag = new TreeNodeTag()
                            {
                                FormType = typeof(FrmArchive),
                                FormArgs = new object[] { settings, environment, ArcType.MinData }
                            }
                        },
                        new TreeNode(ServerShellPhrases.HourDataNode)
                        {
                            ImageKey = "server_data.png",
                            SelectedImageKey = "server_data.png",
                            Tag = new TreeNodeTag()
                            {
                                FormType = typeof(FrmArchive),
                                FormArgs = new object[] { settings, environment, ArcType.HourData }
                            }
                        },
                        new TreeNode(ServerShellPhrases.EventsNode)
                        {
                            ImageKey = "server_event.png",
                            SelectedImageKey = "server_event.png",
                            Tag = new TreeNodeTag()
                            {
                                FormType = typeof(FrmArchive),
                                FormArgs = new object[] { settings, environment, ArcType.Events }
                            }
                        }
                    })
                {
                    ImageKey = "server_archive.png",
                    SelectedImageKey = "server_archive.png"
                },
                new TreeNode(ServerShellPhrases.GeneratorNode)
                {
                    ImageKey = "server_generator.png",
                    SelectedImageKey = "server_generator.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmGenerator),
                        FormArgs = new object[] { settings, environment }
                    }
                },
                new TreeNode(ServerShellPhrases.StatsNode)
                {
                    ImageKey = "server_stats.png",
                    SelectedImageKey = "server_stats.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmStats),
                        FormArgs = new object[] { environment }
                    }
                },*/
            };
        }

        /// <summary>
        /// Gets the images used by the explorer tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { ImagePrefix + "general_options.png", Resources.general_options },
            };
        }
    }
}
