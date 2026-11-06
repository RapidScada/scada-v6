// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page for acknowledging an event.
    /// <para>Представляет страницу для квитирования события.</para>
    /// </summary>
    public class EventAckModel(
        IWebContext webContext,
        IUserContext userContext,
        IClientAccessor clientAccessor,
        PluginContext pluginContext) : PageModel
    {
        private readonly dynamic dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Areas.Main.Pages.EventAck");

        public bool HasError { get; private set; } = false;
        public string Message { get; private set; } = "";
        public Event Event { get; private set; } = null;
        public EventFormatted EventF { get; private set; } = null;
        public bool AckAllowed { get; private set; } = false;
        public bool CloseModal { get; private set; } = false;
        
        [BindProperty]
        public string AckNote { get; set; }


        private int GetArchiveBit(int? archiveBitOrNull)
        {
            return archiveBitOrNull != null && archiveBitOrNull > 0
                ? archiveBitOrNull.Value
                : webContext.ConfigDatabase.FindArchiveBit(
                    pluginContext.Options.EventArchiveCode, ArchiveBit.Event);
        }

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

        private string GetEventText(int archiveBit, long eventID)
        {
            if (eventID <= 0) return "";

            try
            {
                Event ev = clientAccessor.ScadaClient.GetEventByID(archiveBit, eventID);
                return ev == null ? "" : ev.Text;
            }
            catch (Exception ex)
            {
                HasError = true;
                Message = WebPhrases.ClientError;
                webContext.Log.WriteError(ex, Message);
                return "";
            }
        }

        private EventFormatted FormatEvent(Event ev)
        {
            return new CnlDataFormatter(webContext.ConfigDatabase, userContext.TimeZone)
                .FormatEvent(ev);
        }

        private void AckEvent(int archiveBit, Event ev, int userID, string note)
        {
            // create event for acknowledgement notes
            DateTime timestamp = DateTime.UtcNow;
            long noteEventID = 0;

            if (!string.IsNullOrWhiteSpace(note))
            {
                // TODO: set mask
                noteEventID = ScadaUtils.GenerateUniqueID(timestamp);
                clientAccessor.ScadaClient.WriteEvent(ArchiveMask.All, new Event
                {
                    EventID = noteEventID,
                    Timestamp = timestamp,
                    Hidden = true,
                    CnlNum = ev.CnlNum,
                    ObjNum = ev.ObjNum,
                    DeviceNum = ev.DeviceNum,
                    TextFormat = EventTextFormat.AckNote,
                    Text = note,
                });
            }

            // acknowledge event
            clientAccessor.ScadaClient.AckEvent(new EventAck
            {
                EventID = ev.EventID,
                Timestamp = timestamp,
                UserID = userID,
                //NoteEventID = noteEventID
            });
        }

        public IActionResult OnGet(long eventID, int? archiveBit)
        {
            archiveBit = GetArchiveBit(archiveBit);
            Event = GetEvent(archiveBit.Value, eventID, out Right right);

            if (Event == null)
                return Page();

            if (!right.View)
                return Forbid();

            EventF = FormatEvent(Event);
            AckAllowed = right.Control && !Event.Ack;
            AckNote = GetEventText(archiveBit.Value, Event.AckNoteEventID);
            return Page();
        }

        public IActionResult OnPost(long eventID, int? archiveBit)
        {
            archiveBit = GetArchiveBit(archiveBit);
            Event = GetEvent(archiveBit.Value, eventID, out Right right);

            if (Event == null)
                return Page();

            if (!right.Control)
                return Forbid();

            try
            {
                // acknowledge event
                clientAccessor.ScadaClient.AckEvent(new EventAck
                {
                    EventID = eventID,
                    Timestamp = DateTime.UtcNow,
                    UserID = User.GetUserID()
                });

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
