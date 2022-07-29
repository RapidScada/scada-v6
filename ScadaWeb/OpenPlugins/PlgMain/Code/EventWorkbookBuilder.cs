// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Report.Xml2003;
using Scada.Report.Xml2003.Excel;
using System.Xml;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Builds an Excel workbook that contains events.
    /// <para>Создает книгу Excel, которая содержит события.</para>
    /// </summary>
    internal class EventWorkbookBuilder
    {
        /// <summary>
        /// The workbook template file name.
        /// </summary>
        private const string TemplateFileName = "EventTemplate.xml";
        /// <summary>
        /// The default code of the event archive.
        /// </summary>
        private const string DefaultEventArchiveCode = "Events";

        private readonly ConfigDatabase configDatabase;
        private readonly ScadaClient scadaClient;
        private readonly string templateFilePath;
        private readonly WorkbookRenderer renderer;

        private EventWorkbookArgs workbookArgs;
        private Archive archiveEntity;
        private DateTime generateDT;
        private Row eventRowTemplate;
        private Event currentEvent;
        private EventFormatted currentEventF;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventWorkbookBuilder(ConfigDatabase configDatabase, ScadaClient scadaClient, string templateDir)
        {
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            this.scadaClient = scadaClient ?? throw new ArgumentNullException(nameof(scadaClient));
            templateFilePath = Path.Combine(templateDir, TemplateFileName);
            renderer = new WorkbookRenderer();
            renderer.BeforeProcessing += Renderer_BeforeProcessing;
            renderer.AfterProcessing += Renderer_AfterProcessing;
            renderer.DirectiveFound += Renderer_DirectiveFound;

            workbookArgs = null;
            archiveEntity = null;
            generateDT = DateTime.MinValue;
            eventRowTemplate = null;
            currentEvent = null;
            currentEventF = null;
        }


        /// <summary>
        /// Builds a workbook to the output stream.
        /// </summary>
        public void Build(EventWorkbookArgs args, Stream outStream)
        {
            workbookArgs = args ?? throw new ArgumentNullException(nameof(args));
            ArgumentNullException.ThrowIfNull(outStream, nameof(outStream));

            string archiveCode = string.IsNullOrEmpty(args.ArchiveCode) ? DefaultEventArchiveCode : args.ArchiveCode;
            archiveEntity = configDatabase.ArchiveTable.SelectFirst(new TableFilter("Code", archiveCode));

            if (archiveEntity == null)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Архив не найдён в базе конфигурации." :
                    "Archive not found in the configuration database.");
            }

            renderer.Render(templateFilePath, outStream);
        }


        private void Renderer_BeforeProcessing(object sender, XmlDocument e)
        {
            generateDT = DateTime.UtcNow;
            eventRowTemplate = null;
        }

        private void Renderer_AfterProcessing(object sender, XmlDocument e)
        {
            if (eventRowTemplate != null)
            {
                // select events
                TimeRange timeRange = new(generateDT.AddDays(-workbookArgs.EventDepth), generateDT, true);
                EventFilter filter = workbookArgs.View == null
                    ? new EventFilter(workbookArgs.EventCount)
                    : new EventFilter(workbookArgs.EventCount, workbookArgs.View);
                List<Event> events = scadaClient.GetEvents(archiveEntity.Bit, timeRange, filter, false, out _);
                CnlDataFormatter formatter = new(configDatabase, configDatabase.Enums, workbookArgs.TimeZone);

                // modify workbook
                Table eventTable = eventRowTemplate.ParentTable;
                eventTable.RemoveTableNodeAttrs();

                int rowIndex = eventTable.Rows.IndexOf(eventRowTemplate);
                eventTable.RemoveRow(rowIndex);

                foreach (Event ev in events)
                {
                    currentEvent = ev;
                    currentEventF = formatter.FormatEvent(ev);
                    Row eventRow = eventRowTemplate.Clone();
                    renderer.ProcessRow(eventRow);
                    eventTable.AppendRow(eventRow);
                }
            }
        }

        private void Renderer_DirectiveFound(object sender, DirectiveEventArgs e)
        {
            string nodeText = null;

            if (e.Stage == ProcessingStage.Processing)
            {
                if (e.DirectiveName == "Arc")
                    nodeText = archiveEntity.Name;
            }
            else if (e.Stage == ProcessingStage.Postprocessing)
            {

            }

            if (nodeText != null)
                e.Node.InnerText = nodeText;
        }
    }
}
