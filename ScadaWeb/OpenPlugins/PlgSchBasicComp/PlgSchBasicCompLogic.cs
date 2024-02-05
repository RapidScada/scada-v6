// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgSchBasicComp.Code;
using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgSchBasicComp
{
    /// <summary>
    /// Implements the plugin logic.
    /// <para>Реализует логику плагина.</para>
    /// </summary>
    public class PlgSchBasicCompLogic : PluginLogic, ISchemeComp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgSchBasicCompLogic(IWebContext webContext)
            : base(webContext)
        {
            Info = new PluginInfo();
        }

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
