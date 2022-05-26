/*
 * Copyright 2022 Rapid Software LLC
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
 * Module   : ScadaWebCommon
 * Summary  : Renders pagination HTML
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;

namespace Scada.Web.TagHelpers
{
    /// <summary>
    /// Renders pagination HTML.
    /// <para>Формирует HTML-код нумерации страниц.</para>
    /// </summary>
    [HtmlTargetElement("pager", Attributes = "asp-for, page-count")]
    [HtmlTargetElement("pager", Attributes = "page-index, page-count")]
    public class PagerTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Gets or sets the index of the current page.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int PageCount { get; set; }


        /// <summary>
        /// Gets the name of the HTML element.
        /// </summary>
        private string GetElementName()
        {
            return For == null ? "" : For.Name;
        }

        /// <summary>
        /// Gets the index of the current page.
        /// </summary>
        private int GetPageIndex()
        {
            if (For == null)
                return PageIndex;
            else if (For.Model is int intVal)
                return intVal;
            else
                return 0;
        }

        /// <summary>
        /// Renders HTML.
        /// </summary>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            if (PageCount <= 1)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "nav";
            output.AddClass("rs-pager", HtmlEncoder.Default);

            int pageIndex = GetPageIndex();
            output.PreContent
                .AppendFormat("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", GetElementName(), pageIndex)
                .AppendHtml("<ul class=\"pagination\">");

            for (int i = 0; i < PageCount; i++)
            {
                // <li class="page-item active"><a class="page-link" href="#">1</a></li>
                output.Content
                    .AppendHtml($"<li class=\"page-item{(i == pageIndex ? " active" : "")}\">")
                    .AppendHtml($"<a class=\"page-link\" href=\"#\" data-page=\"{i}\">{i + 1}</a></li>");
            }

            output.PostContent.SetHtmlContent("</ul>");
        }
    }
}
