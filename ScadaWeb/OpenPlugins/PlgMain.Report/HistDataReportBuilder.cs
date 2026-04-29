// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Report;
using Scada.Report.Xml2003;
using Scada.Report.Xml2003.Excel;
using System.Globalization;
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
        /// Contains totals of a column.
        /// </summary>
        private class ColumnTotal
        {
            public int Count { get; set; } = 0;
            public double Sum { get; set; } = 0;
            public double Avg => Count > 0 ? Sum / Count : double.NaN;
            public double Min { get; set; } = double.NaN;
            public double Max { get; set; } = double.NaN;

            public void CountData(CnlData cnlData)
            {
                if (cnlData.IsDefined && !double.IsNaN(cnlData.Val))
                {
                    Count++;
                    Sum += cnlData.Val;

                    if (double.IsNaN(Min) || Min > cnlData.Val)
                        Min = cnlData.Val;

                    if (double.IsNaN(Max) || Max < cnlData.Val)
                        Max = cnlData.Val;
                }
            }
        }


        /// <summary>
        /// The workbook template file name.
        /// </summary>
        private const string TemplateFileName = "HistDataTemplate.xml";
        /// <summary>
        /// The default code of the historical archive.
        /// </summary>
        public const string DefaultArchiveCode = "Min";

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
            return string.Format(dict.TitleFormat,
                ReportContext.FormatDateTime(reportArgs.StartTime),
                ReportContext.FormatDateTime(reportArgs.EndTime));
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
            CnlDataFormatter formatter = new(ReportContext.ConfigDatabase, ReportContext.TimeZone)
            {
                Culture = CultureInfo.InvariantCulture
            };
            int cnlCnt = cnls.Count;
            ColumnTotal[] totals = new ColumnTotal[cnlCnt];

            for (int cnlIdx = 0; cnlIdx < cnlCnt; cnlIdx++)
            {
                totals[cnlIdx] = new ColumnTotal();
            }

            // columns
            Table table = dataColCellTemplate.ParentRow.ParentTable;
            table.Columns.Last().Span = cnlCnt - 1;

            // header row
            Row headerRow = dataColCellTemplate.ParentRow;
            headerRow.RemoveCell(dataColCellTemplate);

            foreach (Cnl cnl in cnls)
            {
                Cell dataColCell = dataColCellTemplate.Clone();
                dataColCell.Text = string.Format(dict.DataCol, cnl.CnlNum);
                headerRow.AppendCell(dataColCell);
            }

            // data rows
            Row dataRowTemplate = dataCellTemplate.ParentRow;
            dataRowTemplate.RemoveCell(dataCellTemplate);
            int dataRowIndex = table.Rows.IndexOf(dataRowTemplate);
            table.ParentWorksheet.SplitHorizontal(dataRowIndex);
            table.RemoveRow(dataRowIndex);

            for (int timeIdx = 0, timeCnt = trendBundle.Timestamps.Count; timeIdx < timeCnt; timeIdx++)
            {
                Row dataRow = dataRowTemplate.Clone();
                dataRow.Cells[0].Text = ReportContext.FormatDateTime(trendBundle.Timestamps[timeIdx]);

                for (int cnlIdx = 0; cnlIdx < cnlCnt; cnlIdx++)
                {
                    CnlData cnlData = trendBundle.Trends[cnlIdx][timeIdx];
                    CnlDataFormatted cnlDataF = formatter.FormatCnlData(cnlData, cnls[cnlIdx], false);

                    Cell dataCell = dataCellTemplate.Clone();
                    dataCell.Text = cnlDataF.DispVal;

                    if (formatter.LastResultInfo.IsFloat)
                        dataCell.SetNumberType();

                    if (cnlDataF.GetMainColor(out string color))
                        renderer.Workbook.SetColor(dataCell.Node, null, color);

                    dataRow.AppendCell(dataCell);
                    totals[cnlIdx].CountData(cnlData);
                }

                table.InsertRow(dataRowIndex, dataRow);
                dataRowIndex++;
            }

            // totals
            Row avgRow = avgCellTemplate.ParentRow;
            Row minRow = minCellTemplate.ParentRow;
            Row maxRow = maxCellTemplate.ParentRow;
            avgRow.RemoveCell(avgCellTemplate);
            minRow.RemoveCell(minCellTemplate);
            maxRow.RemoveCell(maxCellTemplate);
            avgCellTemplate.Text = "";
            minCellTemplate.Text = "";
            maxCellTemplate.Text = "";

            void AppendTotalCell(Row row, Cell cellTemplate, double value, Cnl cnl)
            {
                Cell cell = cellTemplate.Clone();
                row.AppendCell(cell);

                if (!double.IsNaN(value))
                {
                    CnlDataFormatted cnlDataF = formatter.FormatCnlData(
                        new CnlData(value, CnlStatusID.Defined), cnl, false);

                    if (formatter.LastResultInfo.IsFloat)
                    {
                        cell.Text = cnlDataF.DispVal;
                        cell.SetNumberType();
                    }
                }
            }

            for (int cnlIdx = 0; cnlIdx < cnlCnt; cnlIdx++)
            {
                Cnl cnl = cnls[cnlIdx];
                ColumnTotal total = totals[cnlIdx];
                AppendTotalCell(avgRow, avgCellTemplate, total.Avg, cnl);
                AppendTotalCell(minRow, minCellTemplate, total.Min, cnl);
                AppendTotalCell(maxRow, maxCellTemplate, total.Max, cnl);
            }
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

            // find entities
            archiveEntity = ReportContext.FindArchive(args.ArchiveCode, DefaultArchiveCode);
            cnls = ReportContext.FindChannels(reportArgs.CnlNums);

            // render report
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
                    cellText = ReportContext.FormatDateTimeWithOffset(GenerateTime);
                else if (e.DirectiveValue == "UserCaption")
                    cellText = reportDict.UserCaption;
                else if (e.DirectiveValue == "User")
                    cellText = ReportContext.Username;
                else if (e.DirectiveValue == "ArcCaption")
                    cellText = reportDict.ArcCaption;
                else if (e.DirectiveValue == "Arc")
                    cellText = archiveEntity.Name;
                else if (e.DirectiveValue == "CnlCaption")
                    cellText = dict.CnlCaption;
                else if (e.DirectiveValue == "TimeCol")
                    cellText = dict.TimeCol;
                else if (e.DirectiveValue == "DataCol")
                    dataColCellTemplate = e.Cell;
                else if (e.DirectiveValue == "Data")
                    dataCellTemplate = e.Cell;
                else if (e.DirectiveValue == "AvgCaption")
                    cellText = dict.AvgCaption;
                else if (e.DirectiveValue == "MinCaption")
                    cellText = dict.MinCaption;
                else if (e.DirectiveValue == "MaxCaption")
                    cellText = dict.MaxCaption;
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
