// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Web.Lang;
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
        private readonly dynamic dict;

        public EventAckModel(IWebContext webContext, IUserContext userContext, IClientAccessor clientAccessor)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Areas.Main.Pages.EventAck");
        }


        public bool HasError { get; private set; } = false;
        public string Message { get; private set; } = "";
        public Event Event { get; private set; } = null;
        public EventFormatted EventF { get; private set; } = null;
        public bool AckAllowed { get; private set; } = false;
        public bool CloseModal { get; private set; } = false;


        private Event GetEvent(int archiveBit, long eventID, out Right right)
        {
            try
            {
                Event ev = clientAccessor.ScadaClient.GetEventByID(archiveBit, eventID);

                if (ev == null)
                {
                    HasError = true;
                    Message = string.Format(dict.EventNotFound, eventID);
                }
                else
                {
                    right = userContext.Rights.GetRightByObj(ev.ObjNum);
                    return ev;
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                Message = WebPhrases.ClientError;
                webContext.Log.WriteError(ex, Message);
            }

            right = Right.Empty;
            return null;
        }

        private EventFormatted FormatEvent(Event ev)
        {
            return new CnlDataFormatter(
                webContext.ConfigDatabase, webContext.ConfigDatabase.Enums, userContext.TimeZone)
                .FormatEvent(ev);
        }

        public IActionResult OnGet(int archiveBit, long eventID)
        {
            Event = GetEvent(archiveBit, eventID, out Right right);

            if (Event == null)
                return Page();

            if (!right.View)
                return Forbid();

            EventF = FormatEvent(Event);
            AckAllowed = right.Control && !Event.Ack;
            return Page();
        }

        public IActionResult OnPost(int archiveBit, long eventID)
        {
            Event = GetEvent(archiveBit, eventID, out Right right);

            if (Event == null)
                return Page();

            if (!right.Control)
                return Forbid();

            try
            {
                // acknowledge event
                clientAccessor.ScadaClient.AckEvent(eventID, DateTime.UtcNow, User.GetUserID());
                Message = dict.EventAcknowledged;
                CloseModal = true;
            }
            catch (Exception ex)
            {
                HasError = true;
                Message = WebPhrases.ClientError;
                webContext.Log.WriteError(ex, Message);

                EventF = FormatEvent(Event);
                AckAllowed = !Event.Ack;
            }

            return Page();
        }
    }
}
