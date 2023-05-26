

using Scada.Admin.Lang;
using Scada.Forms;
using Scada.Lang;

namespace  Scada.Admin.Extensions.ExtSubFolder
{
    public class ExtSubFolderLogic : ExtensionLogic
    {
        public ExtSubFolderLogic(IAdminContext adminContext) : base(adminContext)
        {
        }

        public override string Code { get
            {
                return "ExtSubFolder";
            } }

        public override string Name
        {
            get
            {
                return "Subfolder Extension";
            }
        }

        public override string Descr
        {
            get
            {
                return "The extension allows for the creation of subfolders based on the object table";
            }
        }
    }
}