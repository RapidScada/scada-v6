// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Represents a table view specification.
    /// <para>Представляет спецификацию табличного представления.</para>
    /// </summary>
    public class TableViewSpec : ViewSpec
    {
        /// <summary>
        /// Gets the view type code.
        /// </summary>
        public override string TypeCode => nameof(TableView);

        /// <summary>
        /// Gets the extension of view files.
        /// </summary>
        public override string FileExtension => "tbl";

        /// <summary>
        /// Gets the view icon URL.
        /// </summary>
        public override string IconUrl => "~/plugins/Main/images/table-icon.png";

        /// <summary>
        /// Gets the view type.
        /// </summary>
        public override Type ViewType => typeof(TableView);


        /// <summary>
        /// Gets the view frame URL.
        /// </summary>
        public override string GetFrameUrl(int viewID) => "~/Main/TableView/" + viewID;
    }
}
