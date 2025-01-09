using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Plugins.SchVolExtentionsComp;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Web.Plugins.PlgSchVolExtentionsComp
{
    public class PlgSchVolExtentionsCompLogic : PluginLogic, ISchemeComp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgSchVolExtentionsCompLogic(IWebContext webContext)
            : base(webContext)
        {

        }

        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => "PlgSchVolExtentionsComp";


        /// <summary>
        /// Gets the specification of the component library.
        /// </summary>
        CompLibSpec ISchemeComp.CompLibSpec
        {
            get
            {
                return new BasicCompLibSpec();
            }
        }
    }
}
