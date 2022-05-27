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
using System.Collections.Generic;
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
        /// The number of pages displayed by a pager, not including the Previous and Next buttons.
        /// </summary>
        private const int DisplayPageCount = 9;


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
        /// Gets an array that contains page numbers.
        /// </summary>
        private static int[] GetPageNumbers(int activePage, int pageCount)
        {
            // [1][.][4][5][6][.][9]
            List<int> leftPart = new();
            List<int> rightPart = new();
            int leftPage = activePage - 1;
            int rightPage = activePage + 1;
            int maxPartsCount = DisplayPageCount - 1;

            while (leftPart.Count + rightPart.Count < maxPartsCount)
            {
                bool pageAdded = false;

                if (leftPage >= 1)
                {
                    leftPart.Add(leftPage);
                    leftPage--;
                    pageAdded = true;
                }

                if (rightPage <= pageCount)
                {
                    rightPart.Add(rightPage);
                    rightPage++;
                    pageAdded = true;
                }

                if (!pageAdded)
                    break;
            }

            int[] pageNumbers = new int[leftPart.Count + rightPart.Count + 1];
            int pageNumIdx = 0;

            for (int i = leftPart.Count - 1; i >= 0; i--)
            {
                pageNumbers[pageNumIdx++] = leftPart[i];
            }

            pageNumbers[pageNumIdx++] = activePage;

            for (int i = 0; i < rightPart.Count; i++)
            {
                pageNumbers[pageNumIdx++] = rightPart[i];
            }

            if (pageNumbers[0] != 1 && pageNumbers[0] != activePage)
            {
                pageNumbers[0] = 1;
                pageNumbers[1] = 0;
            }

            if (pageNumbers[^1] != pageCount && pageNumbers[^1] != activePage)
            {
                pageNumbers[^1] = pageCount;
                pageNumbers[^2] = 0;
            }

            return pageNumbers;
        }


        /// <summary>
        /// Renders HTML.
        /// </summary>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            ArgumentNullException.ThrowIfNull(output, nameof(output));
            int pageIndex = GetPageIndex();

            if (pageIndex == 0 && PageCount <= 1)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "nav";
            output.AddClass("rs-pager", HtmlEncoder.Default);
            output.PreContent
                .AppendFormat("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", GetElementName(), pageIndex)
                .AppendHtml("<ul class=\"pagination\">");

            int activePage = pageIndex + 1;
            int[] pageNumbers = GetPageNumbers(activePage, PageCount);

            // previous page
            if (activePage > 1)
            {
                output.Content
                    .AppendHtml("<li class=\"page-item\"><a class=\"page-link\" href=\"#\" ")
                    .AppendHtml($"data-page=\"{activePage - 2}\">&laquo;</a></li>");
            }
            else
            {
                output.Content
                    .AppendHtml("<li class=\"page-item disabled\"><span class=\"page-link\">&laquo;</span></li>");
            }

            // pages numbers
            for (int i = 0; i < pageNumbers.Length; i++)
            {
                int pageNumber = pageNumbers[i];

                if (pageNumber <= 0)
                {
                    // <li class="page-item disabled"><span class="page-link">...</span></li>
                    output.Content
                        .AppendHtml("<li class=\"page-item disabled\"><span class=\"page-link\">...</span></li>");
                }
                else if (pageNumber == activePage)
                {
                    // <li class="page-item active"><span class="page-link">1</span></li>
                    output.Content
                        .AppendHtml("<li class=\"page-item active\">")
                        .AppendHtml($"<span class=\"page-link\">{pageNumber}</span></li>");
                }
                else
                {
                    // <li class="page-item"><a class="page-link" href="#" data-page="0">1</a></li>
                    output.Content
                        .AppendHtml("<li class=\"page-item\"><a class=\"page-link\" href=\"#\" ")
                        .AppendHtml($"data-page=\"{pageNumber - 1}\">{pageNumber}</a></li>");
                }
            }

            // next page
            if (activePage < PageCount)
            {
                output.Content
                    .AppendHtml("<li class=\"page-item\"><a class=\"page-link\" href=\"#\" ")
                    .AppendHtml($"data-page=\"{activePage}\">&raquo;</a></li>");
            }
            else
            {
                output.Content
                    .AppendHtml("<li class=\"page-item disabled\"><span class=\"page-link\">&raquo;</span></li>");
            }

            output.PostContent.SetHtmlContent("</ul>");
        }
    }
}
