// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Log;
using Scada.Server.Modules.ModDeviceAlarm.Config;

namespace Scada.Server.Modules.ModDeviceAlarm.Logic
{
    /// <summary>
    /// Defines functionality to access exporter data.
    /// <para>Определяет функциональность для доступа к данным экспортёра.</para>
    /// </summary>
    internal interface IExporterContext
    {
        /// <summary>
        /// Smtp配置
        /// </summary>
        EmailDeviceConfig EmailDeviceConfig { get; }

        /// <summary>
        /// Gets the exporter configuration.
        /// </summary>
        ExportTargetConfig ExporterConfig { get; }

        /// <summary>
        /// Gets the exporter log.
        /// </summary>
        ILog ExporterLog { get; }

        /// <summary>
        /// Gets the prefix of the exporter files.
        /// </summary>
        string FilePrefix { get; }
    }
}
