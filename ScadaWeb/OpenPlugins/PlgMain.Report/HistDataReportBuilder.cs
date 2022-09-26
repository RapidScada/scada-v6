// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Report;
using Scada.Report.Xml2003;
using Scada.Report.Xml2003.Excel;
using System.Xml;

namespace Scada.Web.Plugins.PlgMain.Report
{
    /// <summary>
    /// Builds a historical data report.
    /// <para>Строит отчёт по историческим данным.</para>
    /// </summary>
    public class HistDataReportBuilder : ReportBuilder
    {
        /// <summary>
        /// The workbook template file name.
        /// </summary>
        private const string TemplateFileName = "HistDataTemplate.xml";
        /// <summary>
        /// The default code of the historical archive.
        /// </summary>
        private const string DefaultArchiveCode = "Min";

        private readonly string templateFilePath;
        private readonly WorkbookRenderer renderer;
        private readonly dynamic reportDict;
        private readonly dynamic dict;

        private HistDataReportArgs reportArgs;
        private Archive archiveEntity;
        private Row cnlRowTemplate;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistDataReportBuilder(IReportContext reportContext)
            : base(reportContext)
        {
            templateFilePath = Path.Combine(ReportContext.TemplateDir, TemplateFileName);
            renderer = new WorkbookRenderer();
            renderer.BeforeProcessing += Renderer_BeforeProcessing;
            renderer.AfterProcessing += Renderer_AfterProcessing;
            renderer.DirectiveFound += Renderer_DirectiveFound;
            reportDict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Report");
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Report.HistDataReportBuilder");

            reportArgs = null;
            archiveEntity = null;
            cnlRowTemplate = null;
        }


        /// <summary>
        /// Gets the report title.
        /// </summary>
        private string GetTitle()
        {
            DateTime localStartTime = TimeZoneInfo.ConvertTimeFromUtc(reportArgs.StartTime, reportArgs.TimeZone);
            DateTime localEndTime = TimeZoneInfo.ConvertTimeFromUtc(reportArgs.EndTime, reportArgs.TimeZone);
            return string.Format(dict.TitleFormat,
                localStartTime.ToLocalizedString(),
                localEndTime.ToLocalizedString());
        }

        /// <summary>
        /// Generates a report to the output stream.
        /// </summary>
        public override void Generate(ReportArgs args, Stream outStream)
        {
            Generate(new HistDataReportArgs(args), outStream);
        }

        /// <summary>
        /// Generates a report to the output stream.
        /// </summary>
        public void Generate(HistDataReportArgs args, Stream outStream)
        {
            reportArgs = args ?? throw new ArgumentNullException(nameof(args));
            reportArgs.Validate();
            ArgumentNullException.ThrowIfNull(outStream, nameof(outStream));

            string archiveCode = ScadaUtils.FirstNonEmpty(args.ArchiveCode, DefaultArchiveCode);
            archiveEntity = ReportContext.ConfigDatabase.ArchiveTable
                .SelectFirst(new TableFilter("Code", archiveCode)) ??
                throw new ScadaException(reportDict.ArchiveNotFound);
            renderer.Render(templateFilePath, outStream);
        }


        private void Renderer_BeforeProcessing(object sender, XmlDocument e)
        {
            GenerateTime = DateTime.UtcNow;
        }

        private void Renderer_AfterProcessing(object sender, XmlDocument e)
        {

        }

        private void Renderer_DirectiveFound(object sender, ExcelDirectiveEventArgs e)
        {
            string cellText = null;

            if (e.Stage == ProcessingStage.Processing)
            {
                if (e.DirectiveValue == "Title")
                    cellText = GetTitle();
                else if (e.DirectiveValue == "GenCaption")
                    cellText = reportDict.GenCaption;
                else if (e.DirectiveValue == "Gen")
                    cellText = TimeZoneInfo.ConvertTimeFromUtc(GenerateTime, reportArgs.TimeZone).ToLocalizedString();
                else if (e.DirectiveValue == "ArcCaption")
                    cellText = reportDict.ArcCaption;
                else if (e.DirectiveValue == "Arc")
                    cellText = archiveEntity.Name;
                else if (e.DirectiveValue == "TzCaption")
                    cellText = reportDict.TzCaption;
                else if (e.DirectiveValue == "Tz")
                    cellText = reportArgs.TimeZone.DisplayName;
                else if (e.DirectiveValue == "CnlCaption")
                    cellText = reportDict.CnlCaption;
            }

            if (cellText != null)
                e.Cell.Text = cellText;
        }
    }
}
