// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Models;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page for acknowledging an event.
    /// <para>Представляет страницу для квитирования события.</para>
    /// </summary>
    public class EventAckModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IClientAccessor clientAccessor;

        public EventAckModel(IWebContext webContext, IUserContext userContext, IClientAccessor clientAccessor)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
        }


        public Event Event { get; private set; }
        public EventFormatted EventF { get; private set; }
        public bool AckRight { get; private set; }
        public bool AckDone { get; private set; }


        private Event GetEvent(int archiveBit, long eventID, out Right right)
        {
            Event ev = clientAccessor.ScadaClient.GetEventByID(archiveBit, eventID);
            right = ev == null ? Right.Empty : userContext.Rights.GetRightByObj(ev.ObjNum);
            return ev;
        }

        private EventFormatted FormatEvent(Event ev)
        {
            if (ev == null)
            {
                return null;
            }
            else
            {
                CnlDataFormatter dataFormatter = new(webContext.BaseDataSet, userContext.TimeZone);
                return dataFormatter.FormatEvent(ev);
            }
        }

        public IActionResult OnGet(int archiveBit, long eventID)
        {
            Event = GetEvent(archiveBit, eventID, out Right right);

            if (Event != null && !right.View)
                return Forbid();

            EventF = FormatEvent(Event);
            AckRight = right.Control;
            AckDone = false;
            return Page();
        }
    }
}
