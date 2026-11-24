/*
 * Copyright 2026 Rapid Software LLC
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
 * Modified : 2026
 */

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
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
        /// Specifies the visual states of page items.
        /// </summary>
        private enum PageState { Disabled, Normal, Active };
        
        /// <summary>
        /// The number of pages displayed by a pager, not including the Previous and Next buttons.
        /// </summary>
        private const int DisplayPageCount = 9;


        /// <summary>
        /// Gets or sets the view context.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

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
        /// Gets the normalized index of the current page.
        /// </summary>
        private int GetValidPageIndex()
        {
            int pageIndex = 0;

            if (For == null)
            {
                pageIndex = PageIndex;
            }
            else if (For.Model is int intVal)
            {
                pageIndex = intVal;
            }

            return PageCount > 0 ? Math.Clamp(pageIndex, 0, PageCount - 1) : 0;
        }

        /// <summary>
        /// Create a hidden input element containing the page index.
        /// </summary>
        private TagBuilder CreateHiddenInput(int pageIndex)
        {
            TagBuilder input = new("input") { TagRenderMode = TagRenderMode.SelfClosing };
            input.MergeAttribute("type", "hidden");
            input.MergeAttribute("name", ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name));
            input.MergeAttribute("value", pageIndex.ToString(CultureInfo.InvariantCulture));
            return input;
        }

        /// <summary>
        /// Create a list element containing page numbers.
        /// </summary>
        private TagBuilder CreatePagination(int activePage)
        {
            TagBuilder ul = new("ul");
            ul.AddCssClass("pagination");

            // previous page
            ul.InnerHtml.AppendHtml(CreatePageItem(
                activePage - 2,
                "&laquo;",
                activePage <= 1 ? PageState.Disabled : PageState.Normal));

            // page numbers
            foreach (int pageNumber in GetPageNumbers(activePage, PageCount))
            {
                if (pageNumber <= 0)
                {
                    ul.InnerHtml.AppendHtml(CreatePageItem(-1, "...", PageState.Disabled));
                }
                else if (pageNumber == activePage)
                {
                    ul.InnerHtml.AppendHtml(CreatePageItem(pageNumber - 1, pageNumber.ToString(), PageState.Active));
                }
                else
                {
                    ul.InnerHtml.AppendHtml(CreatePageItem(pageNumber - 1, pageNumber.ToString(), PageState.Normal));
                }
            }

            // next page
            ul.InnerHtml.AppendHtml(CreatePageItem(
                activePage, 
                "&raquo;", 
                activePage >= PageCount ? PageState.Disabled : PageState.Normal));

            return ul;
        }

        /// <summary>
        /// Creates a list item element representing the page.
        /// </summary>
        private static TagBuilder CreatePageItem(int pageIndex, string encodedText, PageState pageState)
        {
            TagBuilder li = new("li");
            li.AddCssClass("page-item");

            switch (pageState)
            {
                case PageState.Disabled:
                    li.AddCssClass("disabled");
                    li.InnerHtml.AppendHtml(
                        $"<span class=\"page-link\">{encodedText}</span>");
                    break;

                case PageState.Normal:
                    li.InnerHtml.AppendHtml(
                        $"<a class=\"page-link\" href=\"#\" data-page=\"{pageIndex}\">{encodedText}</a>");
                    break;

                case PageState.Active:
                    li.AddCssClass("active");
                    li.InnerHtml.AppendHtml(
                        $"<span class=\"page-link\">{encodedText}</span>");
                    break;
            }

            return li;
        }

        /// <summary>
        /// Gets an array that contains page numbers.
        /// </summary>
        private static int[] GetPageNumbers(int activePage, int pageCount)
        {
            // [1][.][4][5][6][.][9]
            List<int> leftPart = [];
            List<int> rightPart = [];
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
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(output);

            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("rs-pager", HtmlEncoder.Default);
            int pageIndex = GetValidPageIndex();

            if (For != null)
            {
                output.PreContent.SetHtmlContent(CreateHiddenInput(pageIndex));
            }

            if (PageCount <= 1)
            {
                // hide pager
                output.Attributes.SetAttribute(new TagHelperAttribute("hidden"));
            }
            else
            {
                // add pages
                output.Content.SetHtmlContent(CreatePagination(pageIndex + 1));
            }
        }
    }
}
