// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Code
{
    /// <summary>
    /// Represents a scheme view specification.
    /// <para>Представляет спецификацию представления схем.</para>
    /// </summary>
    public class MimicViewSpec : ViewSpec
    {
        /// <summary>
        /// Gets the view type code.
        /// </summary>
        public override string TypeCode => nameof(MimicView);

        /// <summary>
        /// Gets the extension of view files.
        /// </summary>
        public override string FileExtension => "mim";

        /// <summary>
        /// Gets the view icon URL.
        /// </summary>
        public override string IconUrl => "~/plugins/Mimic/images/mimic-icon.png";

        /// <summary>
        /// Gets the view type.
        /// </summary>
        public override Type ViewType => typeof(TypeCode);


        /// <summary>
        /// Gets the view frame URL.
        /// </summary>
        public override string GetFrameUrl(int viewID) => "~/Mimic/MimicView/" + viewID;
    }
}
