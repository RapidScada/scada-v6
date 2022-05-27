// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgWebPage.Code
{
    /// <summary>
    /// Represents a chart view specification.
    /// <para>Представляет спецификацию представления графика.</para>
    /// </summary>
    public class WebPageViewSpec : ViewSpec
    {
        /// <summary>
        /// Gets the view type code.
        /// </summary>
        public override string TypeCode => nameof(WebPageView);

        /// <summary>
        /// Gets the extension of view files.
        /// </summary>
        public override string FileExtension => string.Empty;

        /// <summary>
        /// Gets the view icon URL.
        /// </summary>
        public override string IconUrl => "~/plugins/WebPage/images/webpage-icon.png";

        /// <summary>
        /// Gets the view type.
        /// </summary>
        public override Type ViewType => typeof(WebPageView);


        /// <summary>
        /// Gets the view frame URL.
        /// </summary>
        public override string GetFrameUrl(int viewID) => "~/WebPage/Landing/" + viewID;
    }
}
