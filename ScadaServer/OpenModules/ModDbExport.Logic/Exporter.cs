// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Server.Modules.ModDbExport.Logic
{
    /// <summary>
    /// Exports data to a one target.
    /// <para>Экспортирует данные в одну базу данных.</para>
    /// </summary>
    internal sealed class Exporter
    {
        /// <summary>
        /// Starts the exporter.
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// Stops the exporter.
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// Enqueues the received current data to be exported.
        /// </summary>
        public void EnqueueCurrentData(Slice slice)
        {
            ArgumentNullException.ThrowIfNull(slice, nameof(slice));

        }

        /// <summary>
        /// Enqueues the calculated current data to be exported.
        /// </summary>
        public void EnqueueCalculatedData()
        {

        }

        /// <summary>
        /// Enqueues the historical data to be exported.
        /// </summary>
        public void EnqueueHistoricalData(Slice slice)
        {
            ArgumentNullException.ThrowIfNull(slice, nameof(slice));

        }

        /// <summary>
        /// Enqueues the event to be exported.
        /// </summary>
        public void EnqueueEvent(Event ev)
        {
            ArgumentNullException.ThrowIfNull(ev, nameof(ev));

        }

        /// <summary>
        /// Enqueues the event acknowledgement to be exported.
        /// </summary>
        public void EnqueueEventAck(EventAck eventAck)
        {
            ArgumentNullException.ThrowIfNull(eventAck, nameof(eventAck));

        }

        /// <summary>
        /// Enqueues the command to be exported or executed.
        /// </summary>
        public void EnqueueCommand(TeleCommand command, CommandResult commandResult)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentNullException.ThrowIfNull(commandResult, nameof(commandResult));

        }
    }
}
