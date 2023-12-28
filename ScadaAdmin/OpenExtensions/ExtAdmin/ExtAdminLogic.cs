using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Admin.Extensions.ExtAdmin
{
    public class ExtAdminLogic : ExtensionLogic
    {
        public ExtAdminLogic(IAdminContext adminContext) : base(adminContext)
        {

        }

        public override string Code
        {
            get
            {
                return "ExtAdmin";
            }
        }

        public override string Name
        {
            get
            {
                return "Admin Extension";
            }
        }

        public override string Descr
        {
            get
            {
                return "The extension allows for more feature on ScadaAdmin.";
            }
        }
    }
}
