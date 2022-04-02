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
        private readonly Dictionary<string, ControlModel> controlByCode; // the device controls accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceModel(string code)
            : this(code, new DeviceMeta { Name = code })
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceModel(string code, DeviceMeta deviceMeta)
        {
            controlByCode = new Dictionary<string, ControlModel>();
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Topic = "/devices/" + code;
            Meta = deviceMeta ?? throw new ArgumentNullException(nameof(deviceMeta));
            Controls = new List<ControlModel>();
        }


        /// <summary>
        /// Gets the device code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the device topic.
        /// </summary>
        public string Topic { get; }

        /// <summary>
        /// Gets the information associated with the device.
        /// </summary>
        public DeviceMeta Meta { get; private set; }

        /// <summary>
        /// Gets the device controls.
        /// </summary>
        public List<ControlModel> Controls { get; }


        /// <summary>
        /// Updates the device information.
        /// </summary>
        public void UpdateMeta(DeviceMeta deviceMeta)
        {
            Meta = deviceMeta ?? throw new ArgumentNullException(nameof(deviceMeta));
        }

        /// <summary>
        /// Gets the existing control meta, or adds a new one if not found.
        /// </summary>
        public ControlModel GetOrAddControl(string controlCode)
        {
            if (controlByCode.TryGetValue(controlCode, out ControlModel controlModel))
            {
                return controlModel;
            }
            else
            {
                controlModel = new ControlModel(controlCode, Topic + "/controls/" + controlCode);
                Controls.Add(controlModel);
                controlByCode.Add(controlCode, controlModel);
                return controlModel;
            }
        }
    }
}
