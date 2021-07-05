// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a table view page.
    /// <para>ѕредставл€ет страницу табличного представлени€.</para>
    /// </summary>
    public class TableViewModel : PageModel
    {
        /// <summary>
        /// Represents meta information about a column.
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
                    HeaderText = GetColumnHeaderText(timeOffset, true)
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
                    HeaderText = GetColumnHeaderText(timeOffset, false)
                });
                timeOffset += tablePeriod;
            }

            return columnMetas;
        }

        private static string GetColumnHeaderText(int timeOffset, bool yesterday)
        {
            return yesterday
                ? DateTime.MinValue.AddMinutes(timeOffset + MinPerDay).ToString("t", Locale.Culture) + 
                    PluginPhrases.MinusOneDay
                : DateTime.MinValue.AddMinutes(timeOffset).ToString("t", Locale.Culture);
        }

        private string GetQuantityIconUrl(InCnl inCnl)
        {
            string icon = inCnl?.QuantityID == null ? 
                "" : webContext.BaseDataSet.QuantityTable.GetItem(inCnl.QuantityID.Value).Icon;

            if (string.IsNullOrEmpty(icon))
                icon = "item.png";

            return Url.Content("~/plugins/Main/images/quantity/" + icon);
        }

        private void AddTooltipHtml(StringBuilder sbHtml, int inCnlNum, InCnl inCnl, int outCnlNum, OutCnl outCnl)
        {
            sbHtml.Append(PluginPhrases.InCnlTip).Append(": [").Append(inCnlNum).Append("] ");

            if (inCnl != null)
                sbHtml.Append(HttpUtility.HtmlEncode(inCnl.Name));

            if (outCnlNum > 0)
            {
                sbHtml.Append("<br>").Append(PluginPhrases.OutCnlTip).Append(": [").Append(outCnlNum).Append("] ");

                if (outCnl != null)
                    sbHtml.Append(HttpUtility.HtmlEncode(outCnl.Name));
            }

            if (inCnl != null)
            {
                Device device = inCnl.DeviceNum == null ?
                    null : webContext.BaseDataSet.DeviceTable.GetItem(inCnl.DeviceNum.Value);
                Obj obj = inCnl.ObjNum == null ?
                    null : webContext.BaseDataSet.ObjTable.GetItem(inCnl.ObjNum.Value);
                Quantity quantity = inCnl.QuantityID == null ?
                    null : webContext.BaseDataSet.QuantityTable.GetItem(inCnl.QuantityID.Value);
                Unit unit = inCnl.UnitID == null ?
                    null : webContext.BaseDataSet.UnitTable.GetItem(inCnl.UnitID.Value);

                if (device != null)
                {
                    sbHtml.Append("<br>").Append(PluginPhrases.DeviceTip).Append(": [")
                        .Append(device.DeviceNum).Append("] ").Append(HttpUtility.HtmlEncode(device.Name));
                }

                if (obj != null)
                {
                    sbHtml.Append("<br>").Append(PluginPhrases.ObjTip).Append(": [")
                        .Append(obj.ObjNum).Append("] ").Append(HttpUtility.HtmlEncode(obj.Name));
                }

                if (quantity != null)
                {
                    sbHtml.Append("<br>").Append(PluginPhrases.QuantityTip).Append(": ")
                        .Append(HttpUtility.HtmlEncode(quantity.Name));
                }

                if (unit != null)
                {
                    sbHtml.Append("<br>").Append(PluginPhrases.UnitTip).Append(": ")
                        .Append(HttpUtility.HtmlEncode(unit.Name));
                }
            }
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
            sbHtml.AppendLine("<thead><tr class='row-hdr'>");
            sbHtml.Append("<th>").Append(PluginPhrases.ItemColumn).AppendLine("</th>");
            sbHtml.Append("<th>").Append(PluginPhrases.CurrentColumn).AppendLine("</th>");

            foreach (ColumnMeta columnMeta in columnMetas)
            {
                sbHtml.Append("<th>").Append(columnMeta.HeaderText).AppendLine("</th>");
            }

            sbHtml.AppendLine("</tr></thead>");

            // rows
            BaseTable<InCnl> inCnlTable = webContext.BaseDataSet.InCnlTable;
            BaseTable<OutCnl> outCnlTable = webContext.BaseDataSet.OutCnlTable;
            bool enableCommands = webContext.AppConfig.GeneralOptions.EnableCommands;
            sbHtml.AppendLine("<tbody>");

            foreach (TableItem tableItem in TableView.VisibleItems)
            {
                int inCnlNum = tableItem.CnlNum;
                int outCnlNum = tableItem.OutCnlNum;
                string itemText = string.IsNullOrWhiteSpace(tableItem.Text) ? 
                    "&nbsp;" : HttpUtility.HtmlEncode(tableItem.Text);

                sbHtml.Append("<tr class='row-item' data-cnlNum='").Append(inCnlNum)
                    .Append("' data-outCnlNum='").Append(outCnlNum).AppendLine("'>")
                    .Append("<td><div class='item'>");

                if (tableItem.CnlNum > 0 || tableItem.OutCnlNum > 0)
                {
                    InCnl inCnl = inCnlNum > 0 ? inCnlTable.GetItem(inCnlNum) : null;
                    OutCnl outCnl = outCnlNum > 0 ? outCnlTable.GetItem(outCnlNum) : null;

                    sbHtml.Append("<span class='item-icon'><img src='").Append(GetQuantityIconUrl(inCnl))
                        .Append("' data-bs-toggle='tooltip' data-bs-placement='right' data-bs-html='true' title='");
                    AddTooltipHtml(sbHtml, inCnlNum, inCnl, outCnlNum, outCnl);
                    sbHtml.Append("' /></span>");

                    sbHtml.Append("<span class='item-text item-link'>").Append(itemText).Append("</span>");

                    if (tableItem.OutCnlNum > 0 && enableCommands)
                    {
                        sbHtml.Append("<span class='item-cmd' title='").Append(PluginPhrases.SendCommandTip)
                            .Append("'><i class='fas fa-rocket'></i></span>");
                    }
                }
                else
                {
                    sbHtml.Append("<span class='item-text'>").Append(itemText).Append("</span>");
                }

                sbHtml.AppendLine("</div></td>"); // close first cell
                sbHtml.AppendLine("<td class='cell-cur'></td>"); // current data cell

                for (int i = 0, cnt = columnMetas.Count; i < cnt; i++)
                {
                    sbHtml.AppendLine("<td class='cell-arc'></td>"); // archive data cell
                }

                sbHtml.AppendLine("</tr>");
            }

            sbHtml.AppendLine("</tbody>");
            sbHtml.AppendLine("</table>");
            return new HtmlString(sbHtml.ToString());
        }
    }
}
