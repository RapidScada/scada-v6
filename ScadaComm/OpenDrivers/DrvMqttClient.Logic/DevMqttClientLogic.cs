// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvMqttClient.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevMqttClientLogic : DeviceLogic
    {
        private readonly MqttClientDeviceConfig config; // the device configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new MqttClientDeviceConfig();
            ConnectionRequired = false;
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            if (config.Load(Storage, MqttClientDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                //InitCmdMaps();
            }
            else
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);
            }
        }

        /// <summary>
        /// Performs actions when terminating a communication line.
        /// </summary>
        public override void OnCommLineTerminate()
        {
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {

        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            //base.Session();
            //FinishSession();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {

        }
    }
}
