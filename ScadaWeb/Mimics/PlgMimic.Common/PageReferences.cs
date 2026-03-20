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
        /// Adds the references from the plugin configuration.
        /// </summary>
        public void AddConfigReferences(MimicPluginConfig pluginConfig)
        {
            ArgumentNullException.ThrowIfNull(pluginConfig, nameof(pluginConfig));

            // custom URLs
            if (!string.IsNullOrEmpty(pluginConfig.GeneralOptions.CustomCss))
                styleUrls.Add(pluginConfig.GeneralOptions.CustomCss);

            if (!string.IsNullOrEmpty(pluginConfig.GeneralOptions.CustomJs))
                scriptUrls.Add(pluginConfig.GeneralOptions.CustomJs);

            // font URLs
            foreach (FontOptions font in pluginConfig.Fonts)
            {
                if (!string.IsNullOrEmpty(font.Url))
                    styleUrls.Add(font.Url);
            }
        }

        /// <summary>
        /// Adds the references to the components.
        /// </summary>
        public void AddComponentReferences(List<IComponentSpec> componentSpecs)
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
