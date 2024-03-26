// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDiffCalculator.Logic
{
    /// <summary>
    /// Represents a task for calculating differences.
    /// <para>Представляет задачу для расчёта разностей.</para>
    /// </summary>
    internal class CalculationTask
    {
        /// <summary>
        /// The command name to execute the task.
        /// </summary>
        public const string CommandName = "Calculate";


        /// <summary>
        /// Gets the start time of the calculation period, UTC.
        /// </summary>
        public DateTime StartDT { get; init; }

        /// <summary>
        /// Gets the end time of the calculation period, UTC.
        /// </summary>
        public DateTime EndDT { get; init; }
    }
}
