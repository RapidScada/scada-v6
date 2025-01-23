// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Server.Modules.ModDbExport.Config;

namespace Scada.Server.Modules.ModDbExport.Logic.Queries
{
    /// <summary>
    /// Represents a runtime query filter.
    /// <para>Представляет фильтр запроса времени выполнения.</para>
    /// </summary>
    internal class RuntimeFilter
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RuntimeFilter(QueryFilter queryFilter)
        {
            ArgumentNullException.ThrowIfNull(queryFilter, nameof(queryFilter));
            CnlNums = new HashSet<int>(queryFilter.CnlNums);
            DeviceNums = new HashSet<int>(queryFilter.DeviceNums);
            ObjNums = new HashSet<int>(queryFilter.ObjNums);
        }


        /// <summary>
        /// Gets the channel numbers.
        /// </summary>
        public HashSet<int> CnlNums { get; }

        /// <summary>
        /// Gets the device numbers.
        /// </summary>
        public HashSet<int> DeviceNums { get; }

        /// <summary>
        /// Gets the object numbers.
        /// </summary>
        public HashSet<int> ObjNums { get; }


        /// <summary>
        /// Checks if the event matches the filter.
        /// </summary>
        public bool Match(Event ev)
        {
            ArgumentNullException.ThrowIfNull(ev, nameof(ev));
            return
                (CnlNums.Count == 0 || CnlNums.Contains(ev.CnlNum)) &&
                (DeviceNums.Count == 0 || DeviceNums.Contains(ev.DeviceNum)) &&
                (ObjNums.Count == 0 || ObjNums.Contains(ev.ObjNum));
        }

        /// <summary>
        /// Checks if the command matches the filter.
        /// </summary>
        public bool Match(TeleCommand command)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            return
                (CnlNums.Count == 0 || CnlNums.Contains(command.CnlNum)) &&
                (DeviceNums.Count == 0 || DeviceNums.Contains(command.DeviceNum)) &&
                (ObjNums.Count == 0 || ObjNums.Contains(command.ObjNum));
        }
    }
}
