using Scada.Admin.Extensions;
using Scada.Admin.Extensions.ExtImport.Controls;
using Scada.Forms;

namespace Scada.Admin.Extensions.ExtImport
{
    public class ExtImportLogic : ExtensionLogic
    {
        private CtrlExtensionMenu ctrlExtensionMenu;

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