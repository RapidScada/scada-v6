// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Web.Services;
using Scada.Web.TreeView;
using Scada.Web.Users;

namespace Scada.Web.Plugins.PlgDataReport
{
    /// <summary>
    /// Represents a plugin logic.
    /// <para>Представляет логику плагина.</para>
    /// </summary>
    public class PlgDataReportLogic : PluginLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgDataReportLogic(IWebContext webContext)
            : base(webContext)
        {
        }

        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => "PlgDataReport";

        /// <summary>
        /// Gets reports available for the specified user.
        /// </summary>
        public override List<MenuItem> GetUserReports(User user, UserRights userRights)
        {
            MenuItem headerItem = new() { Text = "General Reports", SortOrder = MenuItemSortOrder.First };
            headerItem.Subitems.Add(new MenuItem { Text = "Data report", Url = "~/DataReport/DataRep", SortOrder = 0 });
            headerItem.Subitems.Add(new MenuItem { Text = "Event report", Url = "~/DataReport/EventRep", SortOrder = 1 });
            return new List<MenuItem> { headerItem };
        }
    }
}
