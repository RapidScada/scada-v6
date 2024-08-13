// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Entities;
using Scada.Lang;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a table view page.
    /// <para>Представляет страницу табличного представления.</para>
    /// </summary>
    public class TableViewModel : PageModel
    {
        /// <summary>
        /// Represents metadata about a column.
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

        private List<ColumnMeta> columnMetas1;
        private List<ColumnMeta> columnMetas2;
        private List<ColumnMeta> allColumnMetas;
        private TableView tableView;


        public TableViewModel(IWebContext webContext, IUserContext userContext,
            IViewLoader viewLoader, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.viewLoader = viewLoader;
            this.pluginContext = pluginContext;
        }


        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; private set; } = "";
        public int ViewID { get; private set; } = 0;
        public int ArchiveBit { get; private set; } = 0;
        public HtmlString ChartArgs { get; private set; } = HtmlString.Empty;
        public string LocalDate { get; private set; } = "";


        private void LoadView(int? id, string localDate)
        {
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;

            if (viewLoader.GetView(ViewID, true, out tableView, out string errMsg))
            {
                TableOptions tableOptions = pluginContext.GetTableOptions(tableView);
                ArchiveBit = webContext.ConfigDatabase.FindArchiveBit(
                    tableOptions.ArchiveCode, Data.Const.ArchiveBit.Hourly);
                ChartArgs = new HtmlString(tableOptions.ChartArgs);

                DateTime selectedDate = DateTime.TryParse(localDate, out DateTime dateTime)
                    ? dateTime.Date
                    : userContext.ConvertTimeFromUtc(DateTime.UtcNow).Date;
                LocalDate = selectedDate.ToString(WebUtils.InputDateFormat);
                InitColumnMetas(selectedDate, tableOptions.Period);
            }
            else
            {
                ErrorMessage = errMsg;
            }

            ViewData["Title"] = tableView == null
                ? string.Format(PluginPhrases.TableViewTitle, ViewID)
                : tableView.Title;
        }

        private void InitColumnMetas(DateTime selectedDate, int tablePeriod)
        {
            columnMetas1 = new List<ColumnMeta>();
            columnMetas2 = new List<ColumnMeta>();
            allColumnMetas = new List<ColumnMeta>();

            if (tablePeriod <= 0)
                tablePeriod = TableOptions.DefaultPeriod;

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

        private string GetQuantityIconUrl(Cnl cnl)
        {
            string icon = cnl?.QuantityID == null ?
                "" : webContext.ConfigDatabase.QuantityTable.GetItem(cnl.QuantityID.Value).Icon;

            if (string.IsNullOrEmpty(icon))
                icon = "item.png";

            return Url.Content("~/plugins/Main/images/quantity/" + icon);
        }

        private void AddTooltipHtml(StringBuilder sbHtml, int cnlNum, Cnl cnl)
        {
            int deviceNum = 0;
            int objNum = 0;
            int quantityID = 0;
            int unitID = 0;

            if (cnlNum > 0)
            {
                sbHtml.Append(PluginPhrases.CnlTip).Append(": [").Append(cnlNum).Append("] ");

                if (cnl != null)
                {
                    sbHtml.Append(HttpUtility.HtmlEncode(cnl.Name));
                    deviceNum = cnl.DeviceNum ?? 0;
                    objNum = cnl.ObjNum ?? 0;
                    quantityID = cnl.QuantityID ?? 0;
                    unitID = cnl.UnitID ?? 0;
                }
            }

            if (deviceNum > 0 && webContext.ConfigDatabase.DeviceTable.GetItem(deviceNum) is Device device)
            {
                sbHtml.Append("<br>").Append(PluginPhrases.DeviceTip).Append(": [")
                    .Append(device.DeviceNum).Append("] ").Append(HttpUtility.HtmlEncode(device.Name));
            }

            if (objNum > 0 && webContext.ConfigDatabase.ObjTable.GetItem(objNum) is Obj obj)
            {
                sbHtml.Append("<br>").Append(PluginPhrases.ObjTip).Append(": [")
                    .Append(obj.ObjNum).Append("] ").Append(HttpUtility.HtmlEncode(obj.Name));
            }

            if (quantityID > 0 && webContext.ConfigDatabase.QuantityTable.GetItem(quantityID) is Quantity quantity)
            {
                sbHtml.Append("<br>").Append(PluginPhrases.QuantityTip).Append(": ")
                    .Append(HttpUtility.HtmlEncode(quantity.Name));
            }

            if (unitID > 0 && webContext.ConfigDatabase.UnitTable.GetItem(unitID) is Unit unit)
            {
                sbHtml.Append("<br>").Append(PluginPhrases.UnitTip).Append(": ")
                    .Append(HttpUtility.HtmlEncode(unit.Name));
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
            bool enableCommands = webContext.AppConfig.GeneralOptions.EnableCommands &&
                userContext.Rights.GetRightByView(tableView.ViewEntity).Control;
            sbHtml.AppendLine("<tbody>");

            foreach (TableItem tableItem in tableView.VisibleItems)
            {
                Cnl itemCnl = tableItem.Cnl;
                bool showVal = itemCnl != null && itemCnl.IsArchivable();
                int joinLen = itemCnl == null ? 1 : itemCnl.GetJoinLength();
                string itemText = string.IsNullOrWhiteSpace(tableItem.Text) ?
                    "&nbsp;" : HttpUtility.HtmlEncode(tableItem.Text);

                sbHtml.Append("<tr class='row-item' data-cnlnum='").Append(tableItem.CnlNum)
                    .Append("' data-showval='").Append(showVal.ToLowerString())
                    .Append("' data-joinlen='").Append(joinLen).AppendLine("'>")
                    .Append("<td><div class='item'>");

                if (tableItem.CnlNum > 0)
                {
                    sbHtml
                        .Append("<span class='item-icon'><img src='").Append(GetQuantityIconUrl(itemCnl))
                        .Append("' data-bs-toggle='tooltip' data-bs-placement='right' data-bs-html='true' title='");
                    AddTooltipHtml(sbHtml, tableItem.CnlNum, itemCnl);

                    sbHtml
                        .Append("' /></span><span class='item-text")
                        .Append(showVal ? " item-link" : "").Append("'>")
                        .Append(itemText).Append("</span>");

                    if (enableCommands && itemCnl != null && itemCnl.IsOutput())
                    {
                        sbHtml
                            .Append("<span class='item-cmd' title='").Append(PluginPhrases.SendCommandTip)
                            .Append("'><i class='fa-solid fa-rocket'></i></span>");
                    }
                }
                else
                {
                    sbHtml.Append("<span class='item-hdr'>").Append(itemText).Append("</span>");
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
