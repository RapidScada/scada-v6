// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
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
        PluginContext pluginContext,
        IMemoryCache memoryCache) : PageModel
    {
        private static readonly TimeSpan EventLogCacheExpiration = TimeSpan.FromSeconds(60);

        public dynamic Dict { get; } = Locale.GetDictionary(typeof(EventLogModel).FullName);
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
        [BindProperty]
        public int PageSize { get; set; } = PaginatedList<EventFormatted>.DefaultPageSize;


        private void FillEventList(bool clearCache)
        {
            int archiveBit = GetArchiveBit();
            TimeRange timeRange = GetTimeRange();
            DataFilter eventFilter = GetEventFilter();
            string cacheKey = PluginUtils.GetCacheKey("EventLog", archiveBit, timeRange.Key, eventFilter.Key);

            if (clearCache)
            {
                memoryCache.Remove(cacheKey);
            }

            List<EventFormatted> formattedEvents = memoryCache.GetOrCreate(
                cacheKey,
                entry =>
                {
                    entry.SetAbsoluteExpiration(EventLogCacheExpiration);
                    CnlDataFormatter formatter = new(webContext.ConfigDatabase, userContext.TimeZone);
                    return clientAccessor.ScadaClient
                        .GetEvents(archiveBit, timeRange, eventFilter, false, out _)
                        .Select(formatter.FormatEvent).ToList();
                });

            EventList.Fill(formattedEvents, PageIndex);
        }

        private int GetArchiveBit()
        {
            return webContext.ConfigDatabase.FindArchiveBit(
                pluginContext.Options.EventArchiveCode, Data.Const.ArchiveBit.Event);
        }

        private TimeRange GetTimeRange()
        {
            return new TimeRange(
                userContext.ConvertTimeToUtc(StartTime),
                userContext.ConvertTimeToUtc(EndTime),
                true);
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
                dataFilter.AddCondition("Severity", FilterOperator.Between, ParseSeveriryRange(Severity));
            }

            if (AckItemIndex > 0)
            {
                // 1 => false
                // 2 => true
                dataFilter.AddCondition("Ack", FilterOperator.Equals, AckItemIndex >= 2);
            }

            return dataFilter;
        }

        private static int[] ParseSeveriryRange(string s)
        {
            string[] parts = s.Split('-');
            return parts.Length >= 2 && int.TryParse(parts[0], out int min) && int.TryParse(parts[1], out int max)
                ? [min, max]
                : [0, 0];
        }

        private bool ValidateTimeRange(out string errMsg)
        {
            if (StartTime > EndTime)
            {
                errMsg = Dict.InvalidPeriod;
                return false;
            }

            int maxReportPeriod = pluginContext.Options.MaxReportPeriod;

            if (maxReportPeriod > 0 && (EndTime - StartTime).TotalDays > maxReportPeriod)
            {
                errMsg = string.Format(Dict.PeriodTooLong, maxReportPeriod);
                return false;
            }

            errMsg = "";
            return true;
        }

        private IActionResult CreateEventLogTable(bool clearCache)
        {
            if (ValidateTimeRange(out string errMsg))
            {
                FillEventList(clearCache);
                return Partial("_EventLogTable", EventList);
            }
            else
            {
                return BadRequest(errMsg);
            }
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
            FillEventList(true);
        }

        public IActionResult OnPost()
        {
            return CreateEventLogTable(false);
        }

        public IActionResult OnPostRefresh()
        {
            return CreateEventLogTable(true);
        }
    }
}
