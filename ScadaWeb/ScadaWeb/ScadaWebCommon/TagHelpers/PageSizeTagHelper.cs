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
 * Summary  : Renders a drop-down list for selecting the page size
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2026
 * Modified : 2026
 */

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Scada.Web.Lang;
using System.Globalization;
using System.Text.Encodings.Web;

namespace Scada.Web.TagHelpers
{
    /// <summary>
    /// Renders a drop-down list for selecting the page size.
    /// <para>Формирует выпадающий список для выбора количества элементов на странице.</para>
    /// </summary>
    /// <remarks>
    /// Usage examples:
    /// <code>
    /// &lt;page-size asp-for="PageSize" /&gt;
    /// &lt;page-size asp-for="PageSize" values="@(new[] { 10, 20, 50 })" /&gt;
    /// &lt;page-size page-size="25" /&gt;
    /// </code>
    /// </remarks>
    [HtmlTargetElement("page-size", Attributes = "asp-for")]
    [HtmlTargetElement("page-size", Attributes = "page-size")]
    public class PageSizeTagHelper : TagHelper
    {
        /// <summary>
        /// The default values offered by the drop-down list.
        /// </summary>
        private static readonly int[] DefaultValues = [10, 25, 50, 100];

        /// <summary>
        /// The CSS classes of the toggle button.
        /// </summary>
        private const string ButtonClass = "btn btn-outline-secondary dropdown-toggle";


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
        /// Gets or sets the current page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page sizes available for selection.
        /// </summary>
        public IEnumerable<int> Values { get; set; }


        /// <summary>
        /// Gets the current page size.
        /// </summary>
        private int GetCurrentPageSize()
        {
            if (For == null)
            {
                return PageSize;
            }
            else if (For.Model is int intVal)
            {
                return intVal;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets a sorted list of unique values to select from, which always includes the current value.
        /// </summary>
        private List<int> GetPageSizes(int currentSize)
        {
            SortedSet<int> sizes = [.. Values ?? DefaultValues];
            sizes.RemoveWhere(x => x <= 0);

            if (currentSize > 0)
            {
                sizes.Add(currentSize);
            }

            return [.. sizes];
        }

        /// <summary>
        /// Creates a hidden input element containing the page size.
        /// </summary>
        private TagBuilder CreateHiddenInput(int pageSize)
        {
            TagBuilder input = new("input") { TagRenderMode = TagRenderMode.SelfClosing };
            input.MergeAttribute("type", "hidden");
            input.MergeAttribute("name", ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name));
            input.MergeAttribute("value", pageSize.ToString(CultureInfo.InvariantCulture));
            return input;
        }

        /// <summary>
        /// Creates a button that toggles the drop-down menu.
        /// </summary>
        private static TagBuilder CreateToggleButton(int currentSize)
        {
            TagBuilder button = new("button");
            button.AddCssClass(ButtonClass);
            button.MergeAttribute("type", "button");
            button.MergeAttribute("data-bs-toggle", "dropdown");
            button.InnerHtml.AppendFormat(WebPhrases.ItemsPerPage, currentSize);
            return button;
        }

        /// <summary>
        /// Creates a drop-down menu containing the page sizes.
        /// </summary>
        private static TagBuilder CreateMenu(List<int> pageSizes, int currentSize)
        {
            TagBuilder ul = new("ul");
            ul.AddCssClass("dropdown-menu");

            foreach (int size in pageSizes)
            {
                ul.InnerHtml.AppendHtml(CreateMenuItem(size, size == currentSize));
            }

            return ul;
        }

        /// <summary>
        /// Creates a list item element representing the page size.
        /// </summary>
        private static TagBuilder CreateMenuItem(int pageSize, bool isActive)
        {
            string text = pageSize.ToString(CultureInfo.InvariantCulture);

            TagBuilder button = new("button");
            button.AddCssClass("dropdown-item");
            button.MergeAttribute("type", "button");
            button.MergeAttribute("data-page-size", text);
            button.InnerHtml.Append(text);

            if (isActive)
            {
                button.AddCssClass("active");
            }

            TagBuilder li = new("li");
            li.InnerHtml.AppendHtml(button);
            return li;
        }


        /// <summary>
        /// Renders HTML.
        /// </summary>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(output);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("dropdown", HtmlEncoder.Default);
            output.AddClass("rs-page-size", HtmlEncoder.Default);

            int currentSize = GetCurrentPageSize();
            List<int> pageSizes = GetPageSizes(currentSize);

            if (For != null)
            {
                output.PreContent.SetHtmlContent(CreateHiddenInput(currentSize));
            }

            output.Content.SetHtmlContent(PageSizeTagHelper.CreateToggleButton(currentSize));
            output.Content.AppendHtml(CreateMenu(pageSizes, currentSize));
        }
    }
}
