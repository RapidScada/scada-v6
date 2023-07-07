using Scada.Admin.Lang;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtBitReader
{
    public class ExtBitReaderLogic : ExtensionLogic
    {
        public ExtBitReaderLogic(IAdminContext adminContext) : base(adminContext)
        {
        }

        public override string Code
        {
            get
            {
                return "ExtBitReader";
            }
        }

        public override string Name
        {
            get
            {
                return "Bit Reader Extension";
            }
        }

        public override string Descr
        {
            get
            {
                return "The extension allows for the reading and the seperation of bits.";
            }
        }
    }
}