// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    /// Builds a table view report.
    /// <para>Строит отчёт по табличному представлению.</para>
    /// </summary>
    public class TableViewReportBuilder : ReportBuilder
    {
        /// <summary>
        /// Represents metadata about a column.
        /// </summary>
        private class ColumnMeta
        {
            public DateTime UtcTime { get; set; }
            public string ShortDate { get; set; }
            public string ShortTime { get; set; }
            public int PointIndex { get; set; }
        }

        /// <summary>
        /// The workbook template file name.
        /// </summary>
        private const string TemplateFileName = "TableViewTemplate.xml";
        /// <summary>
        /// Indicates a column for further data writing.
        /// </summary>
        private const string NextTimeSymbol = "*";

        private readonly string templateFilePath;
        private readonly WorkbookRenderer renderer;
        private readonly dynamic reportDict;
        private readonly dynamic dict;

        private TableViewReportArgs reportArgs;
        private Archive archiveEntity;
        private Cell dataColCellTemplate;
        private Cell dataCellTemplate;
        private TableItem currentTableItem;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableViewReportBuilder(IReportContext reportContext)
            : base(reportContext)
        {
            templateFilePath = Path.Combine(ReportContext.TemplateDir, TemplateFileName);
            renderer = new WorkbookRenderer();
            renderer.BeforeProcessing += Renderer_BeforeProcessing;
            renderer.AfterProcessing += Renderer_AfterProcessing;
            renderer.DirectiveFound += Renderer_DirectiveFound;
            reportDict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Report");
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Report.TableViewReportBuilder");

            reportArgs = null;
            archiveEntity = null;
            dataColCellTemplate = null;
            dataCellTemplate = null;
            currentTableItem = null;
        }


        /// <summary>
        /// Gets the workbook title.
        /// </summary>
        private string GetTitle()
        {
            return string.Format(dict.TitleFormat,
                reportArgs.TableView.Title,
                ReportContext.FormatDateTime(reportArgs.StartTime),
                ReportContext.FormatDateTime(reportArgs.EndTime));
        }

        /// <summary>
        /// Creates a map of channel indexes accessed by channel number.
        /// </summary>
        private static Dictionary<int, int> MapCnlNums(TrendBundle trendBundle)
        {
            Dictionary<int, int> map = [];
            int idx = 0;

            foreach (int cnlNum in trendBundle.CnlNums)
            {
                map[cnlNum] = idx;
                idx++;
            }

            return map;

        }

        /// <summary>
        /// Gets metadata about the columns.
        /// </summary>
        private List<ColumnMeta> GetColumnMetas(TrendBundle trendBundle)
        {
            List<ColumnMeta> columnMetas = [];
            DateTime curDT = reportArgs.StartTime;
            int tablePeriod = reportArgs.TableOptions.Period <= 0
                ? TableOptions.DefaultPeriod
                : reportArgs.TableOptions.Period;
            int pointIndex = 0;

            while (curDT <= reportArgs.EndTime)
            {
                DateTime userCurDT = ReportContext.ConvertTimeFromUtc(curDT);
                columnMetas.Add(new ColumnMeta
                {
                    UtcTime = curDT,
                    ShortDate = userCurDT.ToString("m", Locale.Culture),
                    ShortTime = userCurDT.ToString("t", Locale.Culture),
                    PointIndex = FindPointIndex(trendBundle, curDT, ref pointIndex)
                });
                curDT = curDT.AddMinutes(tablePeriod);
            }

            return columnMetas;
        }

        /// <summary>
        /// Finds a point index in the trend bundle. Returns -1 if the index is not found.
        /// </summary>
        private static int FindPointIndex(TrendBundle trendBundle, DateTime timestamp, ref int currentIndex)
        {
            while (currentIndex < trendBundle.Timestamps.Count)
            {
                DateTime pointTimestamp = trendBundle.Timestamps[currentIndex];

                if (timestamp == pointTimestamp)
                    return currentIndex++;
                else if (timestamp < pointTimestamp)
                    return -1;

                currentIndex++;
            }

            return -1;
        }


        /// <summary>
        /// Generates a report to the output stream.
        /// </summary>
        public override void Generate(ReportArgs args, Stream outStream)
        {
            throw new InvalidOperationException("Method not supported.");
        }

        /// <summary>
        /// Generates a report to the output stream.
        /// </summary>
        public void Generate(TableViewReportArgs args, Stream outStream)
        {
            reportArgs = args ?? throw new ArgumentNullException(nameof(args));
            reportArgs.Validate();
            ArgumentNullException.ThrowIfNull(outStream, nameof(outStream));

            archiveEntity = ReportContext.FindArchive(args.TableOptions.ArchiveCode);
            renderer.Render(templateFilePath, outStream);
        }


        private void Renderer_BeforeProcessing(object sender, XmlDocument e)
        {
            GenerateTime = DateTime.UtcNow;
        }

        private void Renderer_AfterProcessing(object sender, XmlDocument e)
        {
            if (dataColCellTemplate != null &&
                dataCellTemplate != null)
            {
                // prepare data
                TimeRange timeRange = new(reportArgs.StartTime, reportArgs.EndTime, true);
                TrendBundle trendBundle = ReportContext.ScadaClient.GetTrends(
                    archiveEntity.Bit, timeRange, reportArgs.TableView.CnlNumList.ToArray());
                Dictionary<int, int> cnlNumMap = MapCnlNums(trendBundle);
                List<ColumnMeta> columnMetas = GetColumnMetas(trendBundle);
                CnlDataFormatter formatter = new(ReportContext.ConfigDatabase, ReportContext.TimeZone)
                {
                    Culture = CultureInfo.InvariantCulture
                };

                // modify workbook
                Table table = dataColCellTemplate.ParentRow.ParentTable;
                table.RemoveUnwantedAttrs();
                table.ParentWorksheet.Name = dict.WorksheetName;
                table.Columns.Last().Span = columnMetas.Count - 1;

                // header row
                Row headerRow = dataColCellTemplate.ParentRow;
                headerRow.RemoveCell(dataColCellTemplate);
                bool showDate = columnMetas.First().ShortDate != columnMetas.Last().ShortDate;

                foreach (ColumnMeta columnMeta in columnMetas)
                {
                    Cell dataColCell = dataColCellTemplate.Clone();
                    dataColCell.Text = showDate
                        ? columnMeta.ShortDate + " " + columnMeta.ShortTime
                        : columnMeta.ShortTime;
                    headerRow.AppendCell(dataColCell);
                }

                // table view items
                Row itemRowTemplate = dataCellTemplate.ParentRow;
                itemRowTemplate.RemoveCell(dataCellTemplate);
                table.RemoveRow(itemRowTemplate);

                foreach (TableItem tableItem in reportArgs.TableView.VisibleItems)
                {
                    currentTableItem = tableItem;
                    Row itemRow = itemRowTemplate.Clone();
                    renderer.ProcessRow(itemRow);

                    TrendBundle.CnlDataList trend = cnlNumMap.TryGetValue(tableItem.CnlNum, out int cnlIdx)
                        ? trendBundle.Trends[cnlIdx]
                        : null;
                    Cnl itemCnl = tableItem.Cnl;
                    bool showVal = trend != null && itemCnl != null && itemCnl.IsArchivable();
                    ColumnMeta prevColumnMeta = null;

                    foreach (ColumnMeta columnMeta in columnMetas)
                    {
                        Cell dataCell = dataCellTemplate.Clone();

                        if (showVal)
                        {
                            if (columnMeta.PointIndex >= 0)
                            {
                                CnlData cnlData = trend[columnMeta.PointIndex];
                                CnlDataFormatted cnlDataF = formatter.FormatCnlData(cnlData, itemCnl, false);
                                dataCell.Text = cnlDataF.DispVal;

                                if (formatter.LastResultInfo.IsFloat)
                                    dataCell.SetNumberType();

                                if (cnlDataF.GetMainColor(out string color))
                                    renderer.Workbook.SetColor(dataCell.Node, null, color);
                            }
                            else
                            {
                                dataCell.Text = prevColumnMeta != null &&
                                    prevColumnMeta.UtcTime < GenerateTime && GenerateTime <= columnMeta.UtcTime
                                    ? NextTimeSymbol
                                    : "";
                            }
                        }
                        else
                        {
                            dataCell.Text = "";
                        }

                        itemRow.AppendCell(dataCell);
                        prevColumnMeta = columnMeta;
                    }

                    table.AppendRow(itemRow);
                }
            }
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
                    cellText = ReportContext.FormatDateTimeWithOffset(GenerateTime);
                else if (e.DirectiveValue == "UserCaption")
                    cellText = reportDict.UserCaption;
                else if (e.DirectiveValue == "User")
                    cellText = ReportContext.Username;
                else if (e.DirectiveValue == "ArcCaption")
                    cellText = reportDict.ArcCaption;
                else if (e.DirectiveValue == "Arc")
                    cellText = archiveEntity.Name;
                else if (e.DirectiveValue == "ItemCol")
                    cellText = dict.ItemCol;
                else if (e.DirectiveValue == "DataCol")
                    dataColCellTemplate = e.Cell;
                else if (e.DirectiveValue == "Data")
                    dataCellTemplate = e.Cell;
            }
            else if (e.Stage == ProcessingStage.Postprocessing)
            {
                if (e.DirectiveValue == "Name")
                    cellText = currentTableItem.Text;
            }

            if (cellText != null)
                e.Cell.Text = cellText;
        }
    }
}
