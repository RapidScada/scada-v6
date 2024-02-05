// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;

namespace Scada.Comm.Drivers.DrvDsMqtt.Logic
{
    /// <summary>
    /// Represents a dictionary that stores device topics accessed by tag code.
    /// <para>Представляет словарь, в котором хранятся топики устройств, доступные по коду тега.</para>
    /// </summary>
    internal class DeviceTopics : Dictionary<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceTopics(DeviceLogic deviceLogic, string lineTopic)
            : base()
        {
            DeviceLogic = deviceLogic ?? throw new ArgumentNullException(nameof(deviceLogic));
            LineTopic = lineTopic;
            IsInitialized = false;
        }


        /// <summary>
        /// Gets the device logic.
        /// </summary>
        public DeviceLogic DeviceLogic { get; }

        /// <summary>
        /// Gets the line topic used as a root for device topics.
        /// </summary>
        public string LineTopic { get; }

        /// <summary>
        /// Gets a value indicating whether device topics have been created according to the device tags.
        /// </summary>
        public bool IsInitialized { get; private set; }


        /// <summary>
        /// Creates device topics according to the device tags.
        /// </summary>
        public void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                string deviceTopic = LineTopic + CommUtils.GetDeviceLogFileName(DeviceLogic.DeviceNum, "") + "/";

                foreach (DeviceTag deviceTag in DeviceLogic.DeviceTags)
                {
                    if (!string.IsNullOrEmpty(deviceTag.Code))
                        this[deviceTag.Code] = deviceTopic + deviceTag.Code;
                }
            }
        }
    }
}
