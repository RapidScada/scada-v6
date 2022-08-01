// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Represents arguments for generating a table view workbook.
    /// <para>Представляет аргументы для генерации книги табличного представления.</para>
    /// </summary>
    internal class TableWorkbookArgs
    {
        /// <summary>
        /// Gets the table view.
        /// </summary>
        public TableView TableView { get; init; }

        /// <summary>
        /// Gets the table view options.
        /// </summary>
        public TableOptions TableOptions { get; init; }

        /// <summary>
        /// Gets the time range to request data.
        /// </summary>
        public TimeRange TimeRange { get; init; }

        /// <summary>
        /// Gets the user's time zone.
        /// </summary>
        public TimeZoneInfo TimeZone { get; init; }


        /// <summary>
        /// Validates the arguments, raises an exception on failure.
        /// </summary>
        public void Validate()
        {
            if (TableView == null)
                throw new ScadaException("Table view must not be null.");

            if (TableOptions == null)
                throw new ScadaException("Table view options must not be null.");

            if (TimeZone == null)
                throw new ScadaException("Time zone must not be null.");
        }
    }
}
