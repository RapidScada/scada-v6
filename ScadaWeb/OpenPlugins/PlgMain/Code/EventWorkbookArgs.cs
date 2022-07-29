// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Represents arguments for generating an event workbook.
    /// <para>Представляет аргументы для генерации книги событий.</para>
    /// </summary>
    internal class EventWorkbookArgs
    {
        /// <summary>
        /// Gets the archive code.
        /// </summary>
        public string ArchiveCode { get; init; }

        /// <summary>
        /// Gets the number of events.
        /// </summary>
        public int EventCount { get; init; }

        /// <summary>
        /// Gets the number of days to receive events.
        /// </summary>
        public int EventDepth { get; init; }

        /// <summary>
        /// Gets the view to filter events.
        /// </summary>
        public ViewBase View { get; init; }

        /// <summary>
        /// Gets the user's time zone.
        /// </summary>
        public TimeZoneInfo TimeZone { get; init; }
    }
}
