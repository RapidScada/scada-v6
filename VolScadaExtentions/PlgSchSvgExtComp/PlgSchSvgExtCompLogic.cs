using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Plugins.SchSvgExtComp;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Web.Plugins.PlgSchSvgExtComp
{
    public class PlgSchSvgExtCompLogic : PluginLogic, ISchemeComp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgSchSvgExtCompLogic(IWebContext webContext)
            : base(webContext)
        {
            SvgExtApp.WebContext = webContext;
        }

        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => "PlgSchSvgExtComp";


        /// <summary>
        /// Gets the specification of the component library.
        /// </summary>
        CompLibSpec ISchemeComp.CompLibSpec
        {
            get
            {
                return new BasicCompLibSpec(base.WebContext);
            }
        }
    }
}
