// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page that contains a list of events.
    /// <para>Представляет страницу, содержащую список событий.</para>
    /// </summary>
    public class EventsModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly PluginContext pluginContext;


        public EventsModel(IWebContext webContext, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.pluginContext = pluginContext;
        }


        public int ArchiveBit { get; set; }


        private int FindArchiveBit()
        {
            if (string.IsNullOrEmpty(pluginContext.Options.EventArchiveCode))
            {
                return Data.Const.ArchiveBit.Event;
            }
            else
            {
                Archive archive = webContext.ConfigDatabase.ArchiveTable.SelectFirst(
                    new TableFilter("Code", pluginContext.Options.EventArchiveCode));
                return archive == null ? Data.Const.ArchiveBit.Unknown : archive.Bit;
            }
        }

        public void OnGet()
        {
            ArchiveBit = FindArchiveBit();
        }
    }
}
