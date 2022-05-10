// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Web.Api;
using Scada.Web.Authorization;
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
    [Authorize(Policy = PolicyName.Restricted)]
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

        public void OnGet(IdList cnlNums, DateTime startDate)
        {
            // get request parameters and plugin options
            int cnlNum = cnlNums != null && cnlNums.Count > 0 ? cnlNums[0] : 0;
            DateTime utcStartDate = ChartUtils.GetUtcStartDate(startDate, userContext.TimeZone);
            PluginOptions pluginOptions = new(webContext.AppConfig.GetOptions("Chart"));

            // prepare chart data
            ChartDataBuilder chartDataBuilder = new(webContext.ConfigBase, clientAccessor.ScadaClient,
                new ChartDataBuilderOptions
                {
                    CnlNums = new int[] { cnlNum },
                    TimeRange = ChartUtils.GetTimeRange(utcStartDate, 1, true),
                    ArchiveBit = ChartUtils.FindArchiveBit(webContext.ConfigBase, pluginOptions.ChartArchiveCode),
                    TimeZone = userContext.TimeZone
                });

            chartDataBuilder.FillCnls();
            chartDataBuilder.FillData();

            // get chart title and status
            dynamic dict = Locale.GetDictionary("Scada.Web.Plugins.PlgChart.Areas.Chart.Pages.Chart");
            ViewData["Title"] = string.Format(dict.Title, cnlNum);
            string chartTitle = string.Format("[{0}] {1}, {2}", cnlNum, 
                webContext.ConfigBase.CnlTable.GetItem(cnlNum)?.Name,
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
