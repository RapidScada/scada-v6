// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// Provides helper methods for using OPC client.
    /// <para>Предоставляет вспомогательные методы для клиента OPC.</para>
    /// </summary>
    internal class OpcClientHelper
    {
        /// <summary>
        /// The delay before reconnect.
        /// </summary>
        private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(5);

        private readonly OpcConnectionOptions connectionOptions;     // the connection options
        private readonly ILog log;                                   // implements logging
        private DateTime connAttemptDT; // the timestamp of a connection attempt


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcClientHelper(OpcConnectionOptions connectionOptions, ILog log)
        {
            this.connectionOptions = connectionOptions ?? throw new ArgumentNullException(nameof(connectionOptions));
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            connAttemptDT = DateTime.MinValue;
        }
    }
}
