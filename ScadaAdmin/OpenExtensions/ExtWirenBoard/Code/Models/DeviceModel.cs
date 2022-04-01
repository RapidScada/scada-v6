// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code.Meta;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code.Models
{
    /// <summary>
    /// Represents a device connected to Wiren Board.
    /// <para>Представляет устройство, подключенное к Wiren Board.</para>
    /// </summary>
    internal class DeviceModel
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceModel(DeviceMeta deviceMeta)
        {
            Meta = deviceMeta ?? throw new ArgumentNullException(nameof(deviceMeta));
            Controls = new List<ControlMeta>();
        }


        /// <summary>
        /// Gets the information associated with the device.
        /// </summary>
        public DeviceMeta Meta { get; }

        /// <summary>
        /// Gets the device controls.
        /// </summary>
        public List<ControlMeta> Controls { get; }
    }
}
