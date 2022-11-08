// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;

namespace Scada.Doc.Code
{
    /// <summary>
    /// Renders table of contents HTML.
    /// <para>Формирует HTML-код оглавления.</para>
    /// </summary>
    public class TocRenderer
    {
        private readonly IUrlHelper urlHelper;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TocRenderer(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            ActivePagePath = "";
        }


        /// <summary>
        /// Gets or sets the path of the active web page.
        /// </summary>
        public string ActivePagePath { get; set; }


        /// <summary>
        /// Renders HTML of the specified item.
        /// </summary>
        private void RenderItem(TocItem item, StringBuilder sbHtml)
        {
            sbHtml.Append("<li>");

            if (string.IsNullOrEmpty(item.Url))
            {
                sbHtml.Append("<div class=\"item-text\">");
                sbHtml.Append(HttpUtility.HtmlEncode(item.Text));
                sbHtml.Append("</div>");
            }
            else
            {
                string itemUrl = urlHelper.Content(item.Url);
                string itemClass = string.Equals(itemUrl, ActivePagePath, StringComparison.OrdinalIgnoreCase) ? 
                    "active" : "";
                sbHtml.AppendFormat("<div class=\"item-text {0}\"><a href=\"{1}\">", itemClass, itemUrl);
                sbHtml.Append(HttpUtility.HtmlEncode(item.Text));
                sbHtml.Append("</a></div>");
            }

            if (item.Subitems.Count > 0)
            {
                sbHtml.AppendLine();
                sbHtml.AppendLine("<ul>");

                foreach (TocItem subitem in item.Subitems)
                {
                    RenderItem(subitem, sbHtml);
                }

                sbHtml.AppendLine("</ul>");
            }

            sbHtml.AppendLine("</li>");
        }

        /// <summary>
        /// Renders HTML for the table of contents.
        /// </summary>
        public HtmlString RenderHtml(Toc toc)
        {
            StringBuilder sbHtml = new();
            sbHtml.AppendLine("<nav class=\"doc-contents\">");

            if (toc != null && toc.Items.Count > 0)
            {
                sbHtml.AppendLine("<ul>");

                foreach (TocItem item in toc.Items)
                {
                    RenderItem(item, sbHtml);
                }

                sbHtml.AppendLine("</ul>");
            }
            else
            {
                sbHtml.AppendLine("Table of contents is empty.");
            }

            sbHtml.AppendLine("</nav>");
            return new HtmlString(sbHtml.ToString());
        }
    }
}
