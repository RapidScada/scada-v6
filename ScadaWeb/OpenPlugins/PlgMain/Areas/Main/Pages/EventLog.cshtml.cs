// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scada.Data.Models;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page containing an event list.
    /// <para>Представляет страницу, содержащую список событий.</para>
    /// </summary>
    public class EventLogModel(
        IWebContext webContext,
        IUserContext userContext) : PageModel
    {
        [BindProperty]
        public DateTime StartTime { get; set; }
        [BindProperty]
        public DateTime EndTime { get; set; }
        [BindProperty]
        public int ObjNum { get; set; } = 0;
        [BindProperty]
        public int Severity { get; set; } = 0;
        [BindProperty]
        public int Ack { get; set; } = 0;


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
        }
    }
}
