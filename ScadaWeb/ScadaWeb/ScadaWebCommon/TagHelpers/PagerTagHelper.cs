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

using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Scada.Web.TagHelpers
{
    /// <summary>
    /// Renders pagination HTML.
    /// <para>Формирует HTML-код нумерации страниц.</para>
    /// </summary>
    [HtmlTargetElement("pager", Attributes = "page-index, total-pages")]
    public class PagerTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the index of the current page.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the total page count.
        /// </summary>
        public int TotalPages { get; set; }


        /// <summary>
        /// Renders HTML.
        /// </summary>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.PreContent.SetHtmlContent("<ul class=\"pagination\">");

            for (int i = 0; i < TotalPages; i++)
            {
                // <li class="page-item active"><a class="page-link" href="#">1</a></li>
                output.Content
                    .AppendHtml("<li class=\"page-item\"")
                    .AppendHtml(i == PageIndex ? " active" : "")
                    .AppendHtml("\"><a class=\"page-link\" href=\"#\">")
                    .AppendHtml((i + 1).ToString())
                    .AppendHtml("</a></li>");
            }

            output.PostContent.SetHtmlContent("</ul>");
            output.Attributes.Clear();
        }
    }
}
