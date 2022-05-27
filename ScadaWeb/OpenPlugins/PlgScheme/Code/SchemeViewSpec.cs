// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Scada.Web.Plugins.PlgScheme.Code
{
    /// <summary>
    /// Represents a scheme view specification.
    /// <para>Представляет спецификацию представления схем.</para>
    /// </summary>
    public class SchemeViewSpec : ViewSpec
    {
        /// <summary>
        /// Gets the view type code.
        /// </summary>
        public override string TypeCode => nameof(SchemeView);

        /// <summary>
        /// Gets the extension of view files.
        /// </summary>
        public override string FileExtension => "sch";

        /// <summary>
        /// Gets the view icon URL.
        /// </summary>
        public override string IconUrl => "~/plugins/Scheme/images/scheme-icon.png";

        /// <summary>
        /// Gets the view type.
        /// </summary>
        public override Type ViewType => typeof(SchemeView);


        /// <summary>
        /// Gets the view frame URL.
        /// </summary>
        public override string GetFrameUrl(int viewID) => "~/Scheme/SchemeView/" + viewID;
    }
}
