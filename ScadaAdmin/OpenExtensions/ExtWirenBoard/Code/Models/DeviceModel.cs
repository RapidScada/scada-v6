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
        private readonly Dictionary<string, ControlMeta> controlByCode; // the device controls accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceModel(string code)
        {
            controlByCode = new Dictionary<string, ControlMeta>();
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Meta = new DeviceMeta { Name = code };
            Controls = new List<ControlMeta>();
        }


        /// <summary>
        /// Gets the device code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the information associated with the device.
        /// </summary>
        public DeviceMeta Meta { get; private set; }

        /// <summary>
        /// Gets the device controls.
        /// </summary>
        public List<ControlMeta> Controls { get; }


        /// <summary>
        /// Updates the device information.
        /// </summary>
        public void UpdateDeviceMeta(DeviceMeta deviceMeta)
        {
            Meta = deviceMeta ?? throw new ArgumentNullException(nameof(deviceMeta));
        }

        /// <summary>
        /// Updates the control information.
        /// </summary>
        public void UpdateControlMeta(string controlCode, ControlMeta controlMeta)
        {
            if (controlByCode.ContainsKey(controlCode))
            {
                controlByCode[controlCode] = controlMeta;
            }
            else
            {
                Controls.Add(controlMeta);
                controlByCode.Add(controlCode, controlMeta);
            }
        }

        /// <summary>
        /// Gets the existing control meta, or adds a new one if not found.
        /// </summary>
        public ControlMeta GetOrAddControl(string deviceCode)
        {
            if (controlByCode.TryGetValue(deviceCode, out ControlMeta controlMeta))
            {
                return controlMeta;
            }
            else
            {
                controlMeta = new ControlMeta();
                Controls.Add(controlMeta);
                controlByCode.Add(deviceCode, controlMeta);
                return controlMeta;
            }
        }
    }
}
