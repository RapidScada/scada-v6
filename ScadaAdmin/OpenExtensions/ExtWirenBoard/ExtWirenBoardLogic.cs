// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code;
using Scada.Admin.Extensions.ExtWirenBoard.Controls;
using Scada.Admin.Lang;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtWirenBoard
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtWirenBoardLogic : ExtensionLogic
    {
        private CtrlExtensionMenu ctrlExtensionMenu; // provides extension menus


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtWirenBoardLogic(IAdminContext adminContext)
            : base(adminContext)
        {
            ctrlExtensionMenu = null;
        }


        /// <summary>
        /// Get the extension menu control.
        /// </summary>
        private CtrlExtensionMenu CtrlExtensionMenu
        {
            get
            {
                if (ctrlExtensionMenu == null)
                {
                    ctrlExtensionMenu = new CtrlExtensionMenu(AdminContext);
                    FormTranslator.Translate(ctrlExtensionMenu, ctrlExtensionMenu.GetType().FullName);
                }

                return ctrlExtensionMenu;
            }
        }

        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "ExtWirenBoard";
            }
        }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Расширение Wiren Board" : "Wiren Board Extension";
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
                    "Расширение создаёт конфигурацию проекта для взаимодействия с Wiren Board." :
                    "The extension creates a project configuration for interacting with Wiren Board.";
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
    }
}
