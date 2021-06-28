// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;
using System.Text;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a table view page.
    /// <para>ѕредставл€ет страницу табличного представлени€.</para>
    /// </summary>
    public class TableViewModel : PageModel
    {
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

        public void OnGet(int? id)
        {
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;

            if (viewLoader.GetView(ViewID, out TableView view, out string errMsg))
            {
                ErrorMessage = "Loaded OK!";
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
            sbHtml.AppendLine("<table class='main-table'>");

            // columns
            sbHtml.AppendLine("<colgroup>");
            sbHtml.AppendLine("</colgroup>");

            // header
            sbHtml.AppendLine("<thead><tr>");
            sbHtml.AppendLine("</tr></thead>");

            // rows
            sbHtml.AppendLine("<tbody>");

            foreach (TableItem tableItem in TableView.VisibleItems)
            {
                sbHtml.Append("<tr data-cnlNum='").Append(tableItem.CnlNum)
                    .Append("' data-outCnlNum='").Append(tableItem.OutCnlNum)
                    .AppendLine("'>");

                sbHtml.Append("<td>").Append(tableItem.Text).AppendLine("</td>");
                sbHtml.AppendLine("</tr>");
            }

            sbHtml.AppendLine("</tbody>");
            sbHtml.AppendLine("</table>");
            return new HtmlString(sbHtml.ToString());
        }
    }
}
