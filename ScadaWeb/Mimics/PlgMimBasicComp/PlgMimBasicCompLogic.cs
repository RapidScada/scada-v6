// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimBasicComp.Code;
using Scada.Web.Plugins.PlgMimic.Components;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimBasicComp
{
    /// <summary>
    /// Implements the plugin logic.
    /// <para>Реализует логику плагина.</para>
    /// </summary>
    public class PlgMimBasicCompLogic : PluginLogic, IComponentPlugin
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgMimBasicCompLogic(IWebContext webContext)
            : base(webContext)
        {
            Info = new PluginInfo();
        }

        /// <summary>
        /// Gets the component library specification.
        /// </summary>
        public IComponentSpec ComponentSpec => new BasicComponentSpec();
    }
}
