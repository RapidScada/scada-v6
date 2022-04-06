// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Extensions.ExtCommConfig.Controls;
using Scada.Admin.Extensions.ExtCommConfig.Properties;
using Scada.Admin.Lang;
using Scada.Admin.Project;
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
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtCommConfigLogic(IAdminContext adminContext)
            : base(adminContext)
        {
        }


        /// <summary>
        /// Get the extension menu control.
        /// </summary>
        private CtrlExtensionMenu CtrlExtensionMenu
        {
            get
            {
                // do not create IWin32Window objects before calling SetCompatibleTextRenderingDefault
                if (ExtensionUtils.MenuControl == null)
                {
                    ExtensionUtils.MenuControl = new CtrlExtensionMenu(AdminContext);
                    FormTranslator.Translate(ExtensionUtils.MenuControl, typeof(CtrlExtensionMenu).FullName,
                        new FormTranslatorOptions
                        { 
                            ContextMenus = ExtensionUtils.MenuControl.AllContextMenus, 
                            SkipUserControls = false 
                        });
                }

                return ExtensionUtils.MenuControl;
            }
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
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Конфигуратор Коммуникатора" : "Communicator Configurator";
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
                    "Расширение предоставляет пользовательский интерфейс для конфигурирования приложения Коммуникатор." :
                    "The extension provides a user interface for configuring the Communicator application.";
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
        /// Gets menu items to add to the main menu.
        /// </summary>
        public override ToolStripItem[] GetMainMenuItems()
        {
            return CtrlExtensionMenu.GetMainMenuItems();
        }

        /// <summary>
        /// Gets tool buttons to add to the toolbar.
        /// </summary>
        public override ToolStripItem[] GetToobarButtons()
        {
            return CtrlExtensionMenu.GetToobarButtons();
        }

        /// <summary>
        /// Gets tree nodes to add to the explorer tree.
        /// </summary>
        public override TreeNode[] GetTreeNodes(object relatedObject)
        {
            return relatedObject is CommApp commApp
                ? new TreeViewBuilder(AdminContext, ExtensionUtils.MenuControl).CreateTreeNodes(commApp)
                : null;
        }

        /// <summary>
        /// Gets images used by the explorer tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { ImageKey.DataSource, Resources.data_source },
                { ImageKey.Device, Resources.device },
                { ImageKey.Driver, Resources.driver },
                { ImageKey.Options, Resources.options },
                { ImageKey.Line, Resources.line },
                { ImageKey.LineInactive, Resources.line_inactive },
                { ImageKey.Lines, Resources.lines },
                { ImageKey.Stats, Resources.stats }
            };
        }
    }
}
