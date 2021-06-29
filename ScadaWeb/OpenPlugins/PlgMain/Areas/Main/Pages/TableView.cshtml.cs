// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Lang;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a table view page.
    /// <para>ѕредставл€ет страницу табличного представлени€.</para>
    /// </summary>
    public class TableViewModel : PageModel
    {
        /// <summary>
        /// Represents met ainformation about a column.
        /// </summary>
        private class ColumnMeta
        {
            public int TimeOffset { get; set; }
            public string HeaderText { get; set; }
        }

        private const int MinPerDay = 1440;

        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IViewLoader viewLoader;
        private readonly PluginContext pluginContext;
        private readonly dynamic dict;

        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public int ViewID { get; set; }
        public TableView TableView { get; set; }

        public TableViewModel(IWebContext webContext, IUserContext userContext, 
            IViewLoader viewLoader, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.viewLoader = viewLoader;
            this.pluginContext = pluginContext;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Areas.Main.Pages.TableView");
        }


        private List<ColumnMeta> GetColumnMetas()
        {
            List<ColumnMeta> columnMetas = new();

            // previous day
            int timeOffset = -MinPerDay;
            int tablePeriod = pluginContext.Options.TablePeriod;

            while (timeOffset < 0)
            {
                columnMetas.Add(new ColumnMeta
                {
                    TimeOffset = timeOffset,
                    HeaderText = GetHeaderText(timeOffset, true)
                });
                timeOffset += tablePeriod;
            }

            // current day
            timeOffset = 0;

            while (timeOffset < MinPerDay)
            {
                columnMetas.Add(new ColumnMeta
                {
                    TimeOffset = timeOffset,
                    HeaderText = GetHeaderText(timeOffset, false)
                });
                timeOffset += tablePeriod;
            }

            return columnMetas;
        }

        private string GetHeaderText(int timeOffset, bool yesterday)
        {
            return yesterday
                ? DateTime.MinValue.AddMinutes(timeOffset + MinPerDay).ToString("t", Locale.Culture) + dict.MinusOneDay
                : DateTime.MinValue.AddMinutes(timeOffset).ToString("t", Locale.Culture);
        }

        public void OnGet(int? id)
        {
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;

            if (viewLoader.GetView(ViewID, out TableView view, out string errMsg))
            {
                ErrorMessage = "";
                TableView = view;
            }
            else
            {
                ErrorMessage = errMsg;
                TableView = null;
            }
        }

        public HtmlString RenderTableView()
        {
            StringBuilder sbHtml = new();
            sbHtml.AppendLine("<table class='table-main'>");

            // columns
            List<ColumnMeta> columnMetas = GetColumnMetas();
            sbHtml.AppendLine("<colgroup>");
            sbHtml.AppendLine("<col class='col-cap'>");
            sbHtml.AppendLine("<col class='col-cur'>");

            foreach (ColumnMeta columnMeta in columnMetas)
            {
                sbHtml.Append("<col class='col-arc' data-time='").Append(columnMeta.TimeOffset).AppendLine("'>");
            }

            sbHtml.AppendLine("</colgroup>");

            // header
            sbHtml.AppendLine("<thead><tr>");
            sbHtml.AppendLine($"<th>{dict.ItemColumn}</th>");
            sbHtml.AppendLine($"<th>{dict.CurrentColumn}</th>");

            foreach (ColumnMeta columnMeta in columnMetas)
            {
                sbHtml.Append("<th>").Append(columnMeta.HeaderText).AppendLine("</th>");
            }

            sbHtml.AppendLine("</tr></thead>");

            // rows
            sbHtml.AppendLine("<tbody>");

            foreach (TableItem tableItem in TableView.VisibleItems)
            {
                sbHtml.Append("<tr data-cnlNum='").Append(tableItem.CnlNum)
                    .Append("' data-outCnlNum='").Append(tableItem.OutCnlNum)
                    .AppendLine("'>");

                sbHtml.Append("<td>").Append(tableItem.Text).AppendLine("</td>");
                sbHtml.Append("<td>").Append("---").AppendLine("</td>");

                foreach (ColumnMeta columnMeta in columnMetas)
                {
                    sbHtml.Append("<td>").Append("---").AppendLine("</td>");
                }

                sbHtml.AppendLine("</tr>");
            }

            sbHtml.AppendLine("</tbody>");
            sbHtml.AppendLine("</table>");
            return new HtmlString(sbHtml.ToString());
        }
    }
}
