// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Web.Components;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page containing an event list.
    /// <para>Представляет страницу, содержащую список событий.</para>
    /// </summary>
    public class EventLogModel(
        IWebContext webContext,
        IUserContext userContext,
        IClientAccessor clientAccessor,
        PluginContext pluginContext) : PageModel
    {
        public PaginatedList<EventFormatted> EventList { get; } = [];

        [BindProperty]
        public DateTime StartTime { get; set; }
        [BindProperty]
        public DateTime EndTime { get; set; }
        [BindProperty]
        public int ObjNum { get; set; } = 0;
        [BindProperty]
        public string Severity { get; set; } = "";
        [BindProperty]
        public int AckItemIndex { get; set; } = 0;
        [BindProperty]
        public int PageIndex { get; set; } = 0;


        private void FillEventList()
        {
            List<Event> events = clientAccessor.ScadaClient.GetEvents(
                GetArchiveBit(), GetTimeRange(), GetEventFilter(), false, out _);
            CnlDataFormatter formatter = new(webContext.ConfigDatabase, userContext.TimeZone);
            EventList.Fill(events.Select(formatter.FormatEvent).ToList(), PageIndex);
        }

        private int GetArchiveBit()
        {
            return webContext.ConfigDatabase.FindArchiveBit(
                pluginContext.Options.EventArchiveCode, Data.Const.ArchiveBit.Event);
        }

        private TimeRange GetTimeRange()
        {
            return new TimeRange(StartTime, EndTime, true);
        }

        private DataFilter GetEventFilter()
        {
            DataFilter dataFilter = new(typeof(Event));
            dataFilter.AddCondition("Hidden", FilterOperator.Equals, false);

            if (ObjNum > 0)
            {
                dataFilter.AddCondition("ObjNum", FilterOperator.In, 
                    webContext.ConfigDatabase.GetObjNumHierarchy(ObjNum, userContext.Rights));
            }

            if (!string.IsNullOrEmpty(Severity))
            {
                dataFilter.AddCondition("Severity", FilterOperator.In, 
                    ScadaUtils.ParseRange(Severity, true, true));
            }

            if (AckItemIndex > 0)
            {
                // 1 => false
                // 2 => true
                dataFilter.AddCondition("Ack", FilterOperator.Equals, AckItemIndex >= 2);
            }

            return dataFilter;
        }


        public IEnumerable<SelectListItem> GetObjectItems()
        {
            return userContext.Objects.Select(o => new SelectListItem
            {
                Text = o.Text,
                Value = o.ObjNum.ToString(),
                Disabled = !o.Right.View
            });
        }

        public static IEnumerable<SelectListItem> GetSeverityItems()
        {
            return SeverityRange.All.Select(sr => new SelectListItem
            {
                Text = sr.ToString(),
                Value = sr.ToRangeString()
            });
        }

        public void OnGet()
        {
            StartTime = userContext.ConvertTimeFromUtc(DateTime.UtcNow).Date;
            EndTime = StartTime.AddDays(1.0);
            FillEventList();
        }

        public IActionResult OnPost()
        {
            FillEventList();
            return Partial("_EventLogTable", EventList);
        }
    }
}
