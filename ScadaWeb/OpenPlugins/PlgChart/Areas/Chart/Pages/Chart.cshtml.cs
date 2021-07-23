// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Web.Api;
using Scada.Web.Plugins.PlgChart.Code;
using Scada.Web.Services;
using System;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.PlgChart.Areas.Chart.Pages
{
    /// <summary>
    /// Represents a chart page.
    /// <para>Представляет страницу графика.</para>
    /// </summary>
    public class ChartModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IClientAccessor clientAccessor;


        public ChartModel(IWebContext webContext, IUserContext userContext, IClientAccessor clientAccessor)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
        }


        public HtmlString ChartDataHtml { get; private set; }


        private DateTime GetUtcStartDate(DateTime startDate)
        {
            if (startDate == DateTime.MinValue)
                startDate = DateTime.UtcNow;

            if (startDate.Kind == DateTimeKind.Utc)
                startDate = userContext.ConvertTimeFromUtc(startDate).Date;

            return userContext.ConvertTimeToUtc(startDate);
        }

        private int FindArchiveBit(PluginOptions pluginOptions)
        {
            if (string.IsNullOrEmpty(pluginOptions.ChartArchiveCode))
            {
                return ArchiveBit.Minute;
            }
            else
            {
                Archive archive = webContext.BaseDataSet.ArchiveTable.SelectFirst(
                    new TableFilter("Code", pluginOptions.ChartArchiveCode));
                return archive == null ? ArchiveBit.Unknown : archive.Bit;
            }
        }

        public void OnGet(IdList cnlNums, DateTime startDate)
        {
            // TODO: check access rights

            // get request parameters and plugin options
            int cnlNum = cnlNums != null && cnlNums.Count > 0 ? cnlNums[0] : 0;
            DateTime utcStartDate = GetUtcStartDate(startDate);
            PluginOptions pluginOptions = new(webContext.AppConfig.GetOptions("Chart"));

            // prepare chart data
            ChartDataBuilder chartDataBuilder = new(webContext.BaseDataSet, clientAccessor.ScadaClient,
                new ChartDataBuilder.Options
                {
                    CnlNums = new int[] { cnlNum },
                    TimeRange = new TimeRange(utcStartDate, utcStartDate.AddDays(1.0), false),
                    ArchiveBit = FindArchiveBit(pluginOptions),
                    TimeZone = userContext.TimeZone
                });

            chartDataBuilder.FillInCnls();
            chartDataBuilder.FillData();

            // get chart title and status
            dynamic dict = Locale.GetDictionary("Scada.Web.Plugins.PlgChart.Areas.Chart.Pages.Chart");
            ViewData["Title"] = string.Format(dict.Title, cnlNum);
            string chartTitle = string.Format("[{0}] {1}, {2}", cnlNum, 
                webContext.BaseDataSet.InCnlTable.GetItem(cnlNum)?.Name,
                userContext.ConvertTimeFromUtc(utcStartDate).ToLocalizedDateString());
            string chartStatus = dict.Generated + userContext.ConvertTimeFromUtc(DateTime.UtcNow).ToLocalizedString();

            // build client script
            StringBuilder sbChartData = new();
            chartDataBuilder.ToJs(sbChartData);

            sbChartData
                .AppendFormat("var locale = '{0}';", Locale.Culture.Name).AppendLine()
                .AppendFormat("var gapBetweenPoints = {0};", pluginOptions.GapBetweenPoints).AppendLine()
                .AppendFormat("var chartTitle = '{0}';", HttpUtility.JavaScriptStringEncode(chartTitle)).AppendLine()
                .AppendFormat("var chartStatus = '{0}';", HttpUtility.JavaScriptStringEncode(chartStatus)).AppendLine()
                .AppendLine();

            ChartDataHtml = new HtmlString(sbChartData.ToString());
        }
    }
}
