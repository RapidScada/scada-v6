// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Models;

namespace Scada.Report
{
    /// <summary>
    /// Defines functionality to access the report environment.
    /// <para>Определяет функциональность для доступа к окружению отчёта.</para>
    /// </summary>
    public interface IReportContext
    {
        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        ConfigDataset ConfigDatabase { get; }

        /// <summary>
        /// Gets the client that interacts with the Server service.
        /// </summary>
        ScadaClient ScadaClient { get; }

        /// <summary>
        /// Gets the directory of templates.
        /// </summary>
        public string TemplateDir { get; }
    }
}
