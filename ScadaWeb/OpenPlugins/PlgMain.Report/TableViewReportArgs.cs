// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Report;

namespace Scada.Web.Plugins.PlgMain.Report
{
    /// <summary>
    /// Represents arguments for generating a table view report.
    /// <para>Представляет аргументы для генерации отчёта по табличному представлению.</para>
    /// </summary>
    public class TableViewReportArgs : ReportArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableViewReportArgs()
            : base()
        {
            TableView = null;
            TableOptions = null;
            MaxPeriod = 0;
        }


        /// <summary>
        /// Gets the table view.
        /// </summary>
        public TableView TableView { get; init; }

        /// <summary>
        /// Gets the table view options.
        /// </summary>
        public TableOptions TableOptions { get; init; }

        /// <summary>
        /// Gets the time maximum report period, in days.
        /// </summary>
        public int MaxPeriod { get; init; }


        /// <summary>
        /// Validates the arguments, raises an exception on failure.
        /// </summary>
        public override void Validate()
        {
            base.Validate();

            if (TableView == null)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Табличное представление не может быть null." :
                    "Table view must not be null.")
                { MessageIsPublic = true };
            }

            if (TableOptions == null)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Параметры табличного представления не могут быть null." :
                    "Table view options must not be null.")
                { MessageIsPublic = true };
            }

            if (MaxPeriod > 0 && (EndTime - StartTime).TotalDays > MaxPeriod)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Превышен период отчёта." :
                    "Report period exceeded.")
                { MessageIsPublic = true };
            }
        }
    }
}
