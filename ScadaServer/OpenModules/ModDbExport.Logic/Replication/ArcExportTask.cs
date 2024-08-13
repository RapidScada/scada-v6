// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System.Text;

namespace Scada.Server.Modules.ModDbExport.Logic.Replication
{
    /// <summary>
    /// Represents an archive export task.
    /// <para>Представляет задачу экспорта архива.</para>
    /// </summary>
    internal class ArcExportTask
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArcExportTask()
        {
            Name = "";
            StartDT = DateTime.MinValue;
            EndDT = DateTime.MinValue;
            State = new ArcReplicationState();
        }


        /// <summary>
        /// Gets or sets the task name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start time of the replication period, UTC.
        /// </summary>
        public DateTime StartDT { get; set; }

        /// <summary>
        /// Gets or sets the end time of the replication period, UTC.
        /// </summary>
        public DateTime EndDT { get; set; }

        /// <summary>
        /// Gets the replication state.
        /// </summary>
        public ArcReplicationState State { get; }

        /// <summary>
        /// Gets a value indicating whether the task is completed.
        /// </summary>
        public bool IsCompleted => State.PositionDT >= EndDT;


        /// <summary>
        /// Sets the default task name.
        /// </summary>
        private void SetDefaultName()
        {
            Name = string.Format(Locale.IsRussian ?
                "Экспорт с {0} по {1}" :
                "Export from {0} to {1}",
                StartDT.ToLocalTime().ToLocalizedString(),
                EndDT.ToLocalTime().ToLocalizedString());
        }

        /// <summary>
        /// Validates the task parameters.
        /// </summary>
        public bool Validate(out string errMsg)
        {
            StringBuilder sbError = new();

            if (StartDT == DateTime.MinValue)
            {
                sbError.AppendLine(Locale.IsRussian ?
                    "* Начальное время не задано." :
                    "* Start time is not specified.");
            }

            if (EndDT == DateTime.MinValue)
            {
                sbError.AppendLine(Locale.IsRussian ?
                    "* Конечное время не задано." :
                    "* End time is not specified.");
            }

            if (StartDT >= EndDT)
            {
                sbError.AppendLine(Locale.IsRussian ?
                    "* Начальное время должно быть меньше конечного." :
                    "* Start time must be less than end time.");
            }

            if (sbError.Length > 0)
            {
                errMsg = (Locale.IsRussian ?
                    "Некорректная задача репликации:" :
                    "Invalid replication task:") + Environment.NewLine + sbError;
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Creates an export task based on the exporter command.
        /// </summary>
        public static ArcExportTask CreateTask(ExporterCommand exporterCommand, IDictionary<string, string> cmdArgs)
        {
            ArgumentNullException.ThrowIfNull(cmdArgs, nameof(cmdArgs));

            if (exporterCommand == ExporterCommand.ExportArchive)
            {
                ArcExportTask task = new()
                {
                    StartDT = cmdArgs.GetValueAsDateTime("startDT", DateTimeKind.Utc),
                    EndDT = cmdArgs.GetValueAsDateTime("endDT", DateTimeKind.Utc)
                };

                task.SetDefaultName();
                return task;
            }
            else
            {
                return null;
            }
        }
    }
}
