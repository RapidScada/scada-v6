// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

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
        }

        /// <summary>
        /// Renders HTML for the table of contents.
        /// </summary>
        public HtmlString RenderHtml(Toc toc)
        {
            return new HtmlString(
$@"<nav>
    <ul>
        <li>
            <div>Software overview</div>
            <ul>
                <li><a href='#'>Introduction</a></li>
                <li><a href='#'>Line</a></li>
                <li><a href='#'>Line</a></li>
                <li><a href='#'>Line</a></li>
            </ul>
        </li>
    </ul>
</nav>"
);
        }
    }
}
