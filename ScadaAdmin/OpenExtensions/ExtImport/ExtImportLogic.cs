using Scada.Admin.Extensions;
using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Extensions.ExtImport.Controls;
using Scada.Admin.Lang;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtImport
{
	/// <summary>
	/// Represents an extension logic.
	/// </summary>
	public class ExtImportLogic : ExtensionLogic
    {
        private CtrlExtensionMenu ctrlExtensionMenu;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public ExtImportLogic(IAdminContext adminContext) : base(adminContext)
        {
            ctrlExtensionMenu = null;
        }
        private CtrlExtensionMenu CtrlExtensionMenu
        {
            get
            {
                if (ctrlExtensionMenu == null)
                {
                    ctrlExtensionMenu = new CtrlExtensionMenu(AdminContext);
                }

                return ctrlExtensionMenu;
            }
        }
        public override string Code
        {
            get
            {
                return "ExtImport";
            }
        }
		public override void LoadDictionaries()
		{
			if (!Locale.LoadDictionaries(AdminContext.AppDirs.LangDir, Code, out string errMsg))
				AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);

			ExtensionPhrases.Init();
		}

		public override string Name
        {
            get
            {
                return "Import Extension";
            }
        }

        public override string Descr
        {
            get
            {
                return "Allows the user to import variables from an automaton.";
            }
        }
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