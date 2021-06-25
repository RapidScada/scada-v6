/*
 * Copyright 2021 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : PlgMain
 * Summary  : Represents a table view page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public int ViewID { get; set; }
        public TableView TableView { get; set; }

        public TableViewModel(IWebContext webContext, IUserContext userContext, IViewLoader viewLoader)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.viewLoader = viewLoader;
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

            foreach (TableItem tableItem in TableView.VisibleItems)
            {
                sbHtml.Append("<div>").Append(tableItem.Text).AppendLine("</div>");
            }

            return new HtmlString(sbHtml.ToString());
        }
    }
}
