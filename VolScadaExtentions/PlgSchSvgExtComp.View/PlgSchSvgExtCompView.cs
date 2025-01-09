using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Web.Plugins.PlgSchSvgExtComp.View
{
    public class PlgSchSvgExtCompView : PluginView
    {
        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Svg Ext Components";
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return "Svg ext components, pipe etc.";
            }
        }
    }
}
