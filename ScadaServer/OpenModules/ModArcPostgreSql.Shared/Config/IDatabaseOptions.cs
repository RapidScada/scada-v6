// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModArcPostgreSql.Config
{
    /// <summary>
    /// Represents database options.
    /// <para>Представляет параметры базы данных.</para>
    /// </summary>
    internal interface IDatabaseOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use a connection specified in the instance configuration.
        /// </summary>
        bool UseDefaultConn { get; set; }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        /// Gets a value indicating whether partitioning is used.
        /// </summary>
        bool UsePartitioning { get; }

        /// <summary>
        /// Gets or sets the partition size.
        /// </summary>
        PartitionSize PartitionSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum queue size.
        /// </summary>
        int MaxQueueSize { get; set; }

        /// <summary>
        /// Gets or sets the number of data points transferred in one transaction.
        /// </summary>
        int BatchSize { get; set; }
    }
}
