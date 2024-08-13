// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua.Client;
using Scada.Comm.Drivers.DrvOpcUa.Config;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// Represents metadata about a subscription.
    /// <para>Представляет метаданные о подписке.</para>
    /// </summary>
    internal class SubscriptionTag
    {
        /// <summary>
        /// Gets the device logic that added the subscription.
        /// </summary>
        public DevOpcUaLogic DeviceLogic { get; init; }

        /// <summary>
        /// Gets the subscription configuration.
        /// </summary>
        public SubscriptionConfig SubscriptionConfig { get; init; }

        /// <summary>
        /// Gets or sets the OPC subscription.
        /// </summary>
        public Subscription Subscription { get; set; }
    }
}
