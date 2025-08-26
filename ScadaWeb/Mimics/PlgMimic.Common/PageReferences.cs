// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Scada.Web.Plugins.PlgMimic.Components;
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
        /// <summary>
        /// The path to the custom stylesheet.
        /// </summary>
        private const string CustomStylesheet = "~/plugins/Mimic/css/mimic-custom.css";

        private HashSet<string> styleUrls = [];
        private HashSet<string> scriptUrls = [];


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
        /// Clears the page references.
        /// </summary>
        public void Clear()
        {
            styleUrls = [];
            scriptUrls = [];
        }

        /// <summary>
        /// Adds the font URLs to the page references.
        /// </summary>
        public void RegisterFonts(List<FontOptions> fonts)
        {
            ArgumentNullException.ThrowIfNull(fonts, nameof(fonts));

            foreach (FontOptions font in fonts)
            {
                if (font != null && !string.IsNullOrEmpty(font.Url))
                    styleUrls.Add(font.Url);
            }
        }

        /// <summary>
        /// Adds the styles and scripts of the components to the page references.
        /// </summary>
        public void RegisterComponents(List<IComponentSpec> componentSpecs)
        {
            ArgumentNullException.ThrowIfNull(componentSpecs, nameof(componentSpecs));

            foreach (IComponentSpec componentSpec in componentSpecs)
            {
                if (componentSpec != null)
                {
                    componentSpec.StyleUrls.ForEach(url => styleUrls.Add(url));
                    componentSpec.ScriptUrls.ForEach(url => scriptUrls.Add(url));
                }
            }
        }

        /// <summary>
        /// Renders an HTML code containing links to stylesheets.
        /// </summary>
        public HtmlString RenderStyles(IUrlHelper urlHelper)
        {
            StringBuilder sbHtml = new();

            foreach(string url in styleUrls.AsEnumerable().OrderBy(url => url))
            {
                AppendLinkTag(sbHtml, urlHelper, url);
            }

            AppendLinkTag(sbHtml, urlHelper, CustomStylesheet);
            return sbHtml.ToHtmlString();
        }

        /// <summary>
        /// Renders an HTML code containing links to scripts.
        /// </summary>
        public HtmlString RenderScripts(IUrlHelper urlHelper)
        {
            StringBuilder sbHtml = new();

            foreach (string url in scriptUrls.AsEnumerable().OrderBy(url => url))
            {
                AppendScriptTag(sbHtml, urlHelper, url);
            }

            return sbHtml.ToHtmlString();
        }
    }
}
