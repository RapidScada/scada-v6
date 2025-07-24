// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Scada.Web.Plugins.PlgMimic.Config;

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
            return HtmlString.Empty;
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
