// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// Provides data for report events that contain an Excel directive.
    /// <para>Предоставляет данные для событий отчета, содержащих директиву Excel.</para>
    /// </summary>
    public class ExcelDirectiveEventArgs : DirectiveEventArgs
    {
        /// <summary>
        /// Gets the Excel cell that contains the directive.
        /// </summary>
        public Cell Cell { get; init; }
    }
}
