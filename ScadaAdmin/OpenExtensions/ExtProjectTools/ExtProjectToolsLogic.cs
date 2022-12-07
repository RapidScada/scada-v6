// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Extensions.ExtProjectTools.Controls;
using Scada.Admin.Lang;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtProjectTools
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtProjectToolsLogic : ExtensionLogic
    {
        private CtrlMainMenu ctrlMainMenu; // contains main menu


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtProjectToolsLogic(IAdminContext adminContext)
            : base(adminContext)
        {
            ctrlMainMenu = null;
        }


        /// <summary>
        /// Get the main menu control.
        /// </summary>
        private CtrlMainMenu CtrlMainMenu
        {
            get
            {
                if (ctrlMainMenu == null)
                {
                    ctrlMainMenu = new CtrlMainMenu(AdminContext);
                    FormTranslator.Translate(ctrlMainMenu, ctrlMainMenu.GetType().FullName);
                }

                return ctrlMainMenu;
            }
        }

        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "ExtProjectTools";
            }
        }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Инструменты проекта" : "Project Tools";
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
                    "Расширение предоставляет инструменты для удобного редактирования проектов." :
                    "The extension provides tools for handy project editing.";
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
            return CtrlMainMenu.GetMainMenuItems();
        }
    }
}
