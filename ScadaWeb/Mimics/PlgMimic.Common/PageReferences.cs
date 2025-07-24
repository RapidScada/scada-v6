// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Scada.Web.Plugins.PlgMimic.Config;
using System.Text;

namespace Scada.Web.Plugins.PlgMimic
{
    /// <summary>
    /// Provides references to insert into a page that contains a mimic.
    /// <para>Предоставляет ссылки для вставки на страницу, содержащую мнемосхему.</para>
    /// </summary>
    public class PageReferences
    {
        private readonly List<FontOptions> fonts = [];


        /// <summary>
        /// Appends a link tag to the string builder.
        /// </summary>
        private static void AppendLinkTag(StringBuilder sbHtml, IUrlHelper urlHelper, string href)
        {
            sbHtml.AppendLine($"<link href='{urlHelper.Content(href)}' rel='stylesheet' />");
        }

        /// <summary>
        /// Appends a script tag to the string builder.
        /// </summary>
        private static void AppendScriptTag(StringBuilder sbHtml, IUrlHelper urlHelper, string src)
        {
            sbHtml.AppendLine($"<script src='{urlHelper.Content(src)}'></script>");
        }


        /// <summary>
        /// Adds the specified fonts to the page references.
        /// </summary>
        public void RegisterFonts(List<FontOptions> fonts)
        {
            ArgumentNullException.ThrowIfNull(fonts, nameof(fonts));
            this.fonts.Clear();
            this.fonts.AddRange(fonts);
        }

        /// <summary>
        /// Renders an HTML code containing links to stylesheets.
        /// </summary>
        public HtmlString RenderStyles(IUrlHelper urlHelper)
        {
            StringBuilder sbHtml = new();

            foreach(FontOptions font in fonts.Where(f => !string.IsNullOrEmpty(f.Url)))
            {
                AppendLinkTag(sbHtml, urlHelper, font.Url);
            }

            return sbHtml.ToHtmlString();
        }

        /// <summary>
        /// Renders an HTML code containing links to scripts.
        /// </summary>
        public HtmlString RenderScripts(IUrlHelper urlHelper)
        {
            return HtmlString.Empty;
        }
    }
}
