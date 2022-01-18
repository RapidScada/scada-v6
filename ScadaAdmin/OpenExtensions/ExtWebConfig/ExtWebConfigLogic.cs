// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWebConfig.Code;
using Scada.Admin.Extensions.ExtWebConfig.Properties;
using Scada.Admin.Forms;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtWebConfig
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtWebConfigLogic : ExtensionLogic
    {
        /// <summary>
        /// Specifies the image keys.
        /// </summary>
        private static class ImageKey
        {
            private const string ImagePrefix = "web_config_";
            public const string Logs = ImagePrefix + "logs.png";
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtWebConfigLogic(IAdminContext adminContext)
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
                return "ExtWebConfig";
            }
        }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Конфигуратор Вебстанции" : "Webstation Configurator";
            }
        }

        /// <summary>
        /// Gets the extension description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Расширение предоставляет пользовательский интерфейс для конфигурирования приложения Вебстанция." :
                    "The extension provides a user interface for configuring the Webstation application.";
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
            if (relatedObject is not WebApp)
                return null;

            return new TreeNode[]
            {
                new TreeNode(ExtensionPhrases.LogsNode)
                {
                    ImageKey = ImageKey.Logs,
                    SelectedImageKey = ImageKey.Logs,
                    Tag = new TreeNodeTag
                    {
                        FormType = typeof(FrmLogs),
                        FormArgs = new object[] { AdminContext, ServiceApp.Web }
                    }
                }
            };
        }

        /// <summary>
        /// Gets images used by the explorer tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { ImageKey.Logs, Resources.logs },
            };
        }
    }
}
