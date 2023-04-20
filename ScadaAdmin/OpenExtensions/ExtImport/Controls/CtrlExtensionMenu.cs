using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Controls
{
    public partial class CtrlExtensionMenu : UserControl
    {
        private readonly IAdminContext adminContext;
        private CtrlExtensionMenu()
        {
            InitializeComponent();
        }

        public CtrlExtensionMenu(IAdminContext adminContext) :this() 
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            SetMenuItempsEnabled();
            adminContext.CurrentProjectChanged += AdminContext_CurrentProjectChanged;
        }

        private void AdminContext_CurrentProjectChanged(object sender, EventArgs e)
        {
            SetMenuItempsEnabled();
        }

        private void SetMenuItempsEnabled()
        {
            bool projectIsOpen = adminContext.CurrentProject != null;
            miImport.Enabled = btnImport.Enabled = projectIsOpen;
        }

        public ToolStripItem[] GetMainMenuItems()
        {
            return new ToolStripItem[] { miImport };
        }

        public ToolStripItem[] GetToobarButtons()
        {
            return new ToolStripItem[] { btnImport };
        }
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I swear this is an import");
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I swear this is an import 22");
        }


    }
}
