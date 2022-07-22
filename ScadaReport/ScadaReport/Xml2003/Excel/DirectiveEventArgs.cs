// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// Provides data for report events that contain a directive.
    /// <para>Предоставляет данные для событий отчета, содержащих директиву.</para>
    /// </summary>
    public class DirectiveEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the processing stage.
        /// </summary>
        public ProcessingStage Stage { get; init; }

        /// <summary>
        /// Gets the Excel cell.
        /// </summary>
        public Cell Cell { get; init; }

        /// <summary>
        /// Gets the directive name.
        /// </summary>
        public string DirectiveName { get; init; }

        /// <summary>
        /// Gets the directive value.
        /// </summary>
        public string DirectiveValue { get; init; }
    }
}
