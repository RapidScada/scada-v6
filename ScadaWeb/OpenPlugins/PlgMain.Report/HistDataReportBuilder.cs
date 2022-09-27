// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Entities;
using Scada.Data.Models;
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
        private List<Cnl> cnls;
        private Cnl currentCnl;
        private Row cnlRowTemplate;
        private Cell dataColCellTemplate;
        private Cell dataCellTemplate;
        private Cell avgCellTemplate;
        private Cell minCellTemplate;
        private Cell maxCellTemplate;


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
            cnls = null;
            currentCnl = null;
            cnlRowTemplate = null;
            dataColCellTemplate = null;
            dataCellTemplate = null;
            avgCellTemplate = null;
            minCellTemplate = null;
            maxCellTemplate = null;
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
        /// Receives and prints channel data.
        /// </summary>
        private void PrintChannelData()
        {
            if (dataColCellTemplate == null || dataCellTemplate == null ||
                avgCellTemplate == null || minCellTemplate == null || maxCellTemplate == null)
            {
                return;
            }

            // prepare data
            TimeRange timeRange = new(reportArgs.StartTime, reportArgs.EndTime, true);
            TrendBundle trendBundle = ReportContext.ScadaClient.GetTrends(
                archiveEntity.Bit, timeRange, reportArgs.CnlNums.ToArray());
            CnlDataFormatter formatter = new(ReportContext.ConfigDatabase, ReportContext.ConfigDatabase.Enums, reportArgs.TimeZone)
            {
                Culture = CultureInfo.InvariantCulture
            };

            // columns
            Table table = dataColCellTemplate.ParentRow.ParentTable;
            table.Columns.Last().Span = cnls.Count - 1;

            // header row
            Row headerRow = dataColCellTemplate.ParentRow;
            headerRow.RemoveCell(dataColCellTemplate);

            foreach (Cnl cnl in cnls)
            {
                Cell dataColCell = dataColCellTemplate.Clone();
                dataColCell.Text = string.Format(dict.DataCol, cnl.CnlNum);
                headerRow.AppendCell(dataColCell);
            }

            /*Row dataRowTemplate = dataCellTemplate.ParentRow;
            Table table = dataRowTemplate.ParentTable;
            int dataRowIndex = table.Rows.IndexOf(dataRowTemplate);
            table.ParentWorksheet.SplitHorizontal(dataRowIndex);
            table.RemoveRow(dataRowIndex);*/
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

            // find archive
            string archiveCode = ScadaUtils.FirstNonEmpty(args.ArchiveCode, DefaultArchiveCode);
            archiveEntity = ReportContext.ConfigDatabase.ArchiveTable
                .SelectFirst(new TableFilter("Code", archiveCode)) ??
                throw new ScadaException(reportDict.ArchiveNotFound);

            // find channels
            cnls = new List<Cnl>(reportArgs.CnlNums.Count);

            foreach (int cnlNum in reportArgs.CnlNums)
            {
                cnls.Add(
                    ReportContext.ConfigDatabase.CnlTable.GetItem(cnlNum) ?? 
                    new Cnl { CnlNum = cnlNum, Name = "" });
            }

            renderer.Render(templateFilePath, outStream);
        }


        private void Renderer_BeforeProcessing(object sender, XmlDocument e)
        {
            GenerateTime = DateTime.UtcNow;
        }

        private void Renderer_AfterProcessing(object sender, XmlDocument e)
        {
            if (cnlRowTemplate != null)
            {
                // update table
                Table table = cnlRowTemplate.ParentTable;
                table.RemoveUnwantedAttrs();
                table.ParentWorksheet.Name = dict.WorksheetName;

                // print channels
                int cnlRowIndex = table.Rows.IndexOf(cnlRowTemplate);
                table.RemoveRow(cnlRowIndex);

                foreach (Cnl cnl in cnls)
                {
                    currentCnl = cnl;
                    Row cnlRow = cnlRowTemplate.Clone();
                    renderer.ProcessRow(cnlRow);
                    table.InsertRow(cnlRowIndex, cnlRow);
                    cnlRowIndex++;
                }

                PrintChannelData();
            }
        }

        private void Renderer_DirectiveFound(object sender, ExcelDirectiveEventArgs e)
        {
            string cellText = null;

            if (e.Stage == ProcessingStage.Processing)
            {
                if (e.DirectiveName == ReportDirectives.RepRow && e.DirectiveValue == "CnlRow")
                    cnlRowTemplate = e.Cell.ParentRow;
                else if (e.DirectiveValue == "Title")
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
                    cellText = dict.CnlCaption;
                else if (e.DirectiveValue == "DataCol")
                    dataColCellTemplate = e.Cell;
                else if (e.DirectiveValue == "Data")
                    dataCellTemplate = e.Cell;
                else if (e.DirectiveValue == "Avg")
                    avgCellTemplate = e.Cell;
                else if (e.DirectiveValue == "Min")
                    minCellTemplate = e.Cell;
                else if (e.DirectiveValue == "Max")
                    maxCellTemplate = e.Cell;
            }
            else if (e.Stage == ProcessingStage.Postprocessing)
            {
                if (e.DirectiveValue == "CnlNum")
                {
                    cellText = currentCnl.CnlNum.ToString();
                    e.Cell.SetNumberType();
                }
                else if (e.DirectiveValue == "CnlName")
                    cellText = currentCnl.Name;
            }

            if (cellText != null)
                e.Cell.Text = cellText;
        }
    }
}
