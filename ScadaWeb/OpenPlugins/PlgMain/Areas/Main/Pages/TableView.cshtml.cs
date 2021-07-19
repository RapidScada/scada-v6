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
            public string UtcTime { get; set; }
            public string ShortDate { get; set; }
            public string ShortTime { get; set; }
        }

        /// <summary>
        /// Specifies the selection of the select HTML element.
        /// </summary>
        public enum SelectOption { None, First, Last }


        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IViewLoader viewLoader;
        private readonly PluginContext pluginContext;

        private DateTime selectedDate;
        private List<ColumnMeta> columnMetas1;
        private List<ColumnMeta> columnMetas2;
        private List<ColumnMeta> allColumnMetas;
        private TableView tableView;

        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public int ViewID { get; set; }
        public int ArchiveBit { get; set; }
        public string LocalDate { get; set; }


        public TableViewModel(IWebContext webContext, IUserContext userContext, 
            IViewLoader viewLoader, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.viewLoader = viewLoader;
            this.pluginContext = pluginContext;
        }


        private void LoadView(int? id, string localDate)
        {
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;
            ArchiveBit = FindArchiveBit();

            selectedDate = DateTime.TryParse(localDate, out DateTime dateTime)
                ? dateTime.Date
                : userContext.ConvertTimeFromUtc(DateTime.UtcNow).Date;
            LocalDate = selectedDate.ToString(WebUtils.InputDateFormat);
            InitColumnMetas();

            viewLoader.GetView(ViewID, out tableView, out string errMsg);
            ErrorMessage = errMsg;
        }

        private int FindArchiveBit()
        {
            Archive archive = webContext.BaseDataSet.ArchiveTable.SelectFirst(
                new TableFilter("Code", pluginContext.Options.TableArchiveCode));
            return archive == null ? -1 : archive.Bit;
        }

        private void InitColumnMetas()
        {
            columnMetas1 = new List<ColumnMeta>();
            columnMetas2 = new List<ColumnMeta>();
            allColumnMetas = new List<ColumnMeta>();

            int tablePeriod = pluginContext.Options.TablePeriod > 0 ? pluginContext.Options.TablePeriod : 60;
            DateTime utcSelectedDate = userContext.ConvertTimeToUtc(selectedDate);
            DateTime utcPrevDate = userContext.ConvertTimeToUtc(selectedDate.AddDays(-1));
            DateTime utcNextDate = userContext.ConvertTimeToUtc(selectedDate.AddDays(1));

            void AddColumnMetas(List<ColumnMeta> columnMetas, DateTime utcStartDate, DateTime utcEndDate)
            {
                DateTime curDT = utcStartDate;

                while (curDT < utcEndDate)
                {
                    DateTime dt = userContext.ConvertTimeFromUtc(curDT);

                    columnMetas.Add(new ColumnMeta
                    {
                        UtcTime = curDT.ToString(WebUtils.JsDateTimeFormat),
                        ShortDate = dt.ToString("m", Locale.Culture),
                        ShortTime = dt.ToString("t", Locale.Culture),
                    });

                    curDT = curDT.AddMinutes(tablePeriod);
                }
            }

            AddColumnMetas(columnMetas1, utcPrevDate, utcSelectedDate);
            AddColumnMetas(columnMetas2, utcSelectedDate, utcNextDate);
            allColumnMetas.AddRange(columnMetas1);
            allColumnMetas.AddRange(columnMetas2);
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
            LoadView(id, null);
        }

        public void OnPost(int? id, string localDate)
        {
            LoadView(id, localDate);
        }

        public HtmlString RenderOptionGroup(string label, bool isSelectedDate, SelectOption selectOption)
        {
            StringBuilder sbHtml = new();
            List<ColumnMeta> columnMetas = isSelectedDate ? columnMetas2 : columnMetas1;
            sbHtml.Append("<optgroup label='").Append(HttpUtility.HtmlEncode(label)).AppendLine("'>");

            for (int i = 0, lastIdx = columnMetas.Count - 1; i <= lastIdx; i++)
            {
                ColumnMeta columnMeta = columnMetas[i];
                bool isSelected = selectOption == SelectOption.First && i == 0
                    || selectOption == SelectOption.Last && i == lastIdx;
                string optionValue = columnMeta.ShortTime;
                string optionText = columnMeta.ShortTime;

                if (!isSelectedDate)
                {
                    optionValue += "-1d";
                    optionText += PluginPhrases.MinusOneDay;
                }

                sbHtml.Append("<option value='").Append(optionValue)
                    .Append("' data-time='").Append(columnMeta.UtcTime)
                    .Append(isSelected ? "' selected>" : "'>")
                    .Append(optionText).AppendLine("</option>");
            }

            sbHtml.AppendLine("</optgroup>");
            return new HtmlString(sbHtml.ToString());
        }

        public HtmlString RenderTableView()
        {
            StringBuilder sbHtml = new();
            sbHtml.AppendLine("<table class='table-main'>");

            // columns
            sbHtml.AppendLine("<colgroup>");
            sbHtml.AppendLine("<col class='col-cap'>");
            sbHtml.AppendLine("<col class='col-cur'>");

            foreach (ColumnMeta columnMeta in allColumnMetas)
            {
                sbHtml.Append("<col class='col-hist' data-time='").Append(columnMeta.UtcTime).AppendLine("'>");
            }

            sbHtml.AppendLine("</colgroup>");

            // header
            sbHtml.AppendLine("<thead><tr class='row-hdr'>");
            sbHtml.Append("<th class='hdr-cap'>").Append(PluginPhrases.ItemColumn).AppendLine("</th>");
            sbHtml.Append("<th class='hdr-cur'>").Append(PluginPhrases.CurrentColumn).AppendLine("</th>");

            foreach (ColumnMeta columnMeta in allColumnMetas)
            {
                sbHtml.Append("<th class='hdr-hist'><span class='hdr-date'>").Append(columnMeta.ShortDate)
                    .Append("</span> <span class='hdr-time'>").Append(columnMeta.ShortTime)
                    .AppendLine("</span></th>");
            }

            sbHtml.AppendLine("</tr></thead>");

            // rows
            BaseTable<InCnl> inCnlTable = webContext.BaseDataSet.InCnlTable;
            BaseTable<OutCnl> outCnlTable = webContext.BaseDataSet.OutCnlTable;
            bool enableCommands = webContext.AppConfig.GeneralOptions.EnableCommands;
            sbHtml.AppendLine("<tbody>");

            foreach (TableItem tableItem in tableView.VisibleItems)
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

                for (int i = 0, cnt = allColumnMetas.Count; i < cnt; i++)
                {
                    sbHtml.AppendLine("<td class='cell-hist'></td>"); // historical data cell
                }

                sbHtml.AppendLine("</tr>");
            }

            sbHtml.AppendLine("</tbody>");
            sbHtml.AppendLine("</table>");
            return new HtmlString(sbHtml.ToString());
        }
    }
}
