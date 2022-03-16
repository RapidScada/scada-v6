// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqttClient.Config;
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
        private MqttClientDeviceConfig config; // the device configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = null;
            ConnectionRequired = false;
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            config = new MqttClientDeviceConfig();

            if (config.Load(Storage, MqttClientDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                //InitCmdMaps();
            }
            else
            {
                config = null;
                Log.WriteLine(errMsg);
                Log.WriteLine(Locale.IsRussian ?
                    "Взаимодействие с MQTT-брокером невозможно, т.к. конфигурация устройства не загружена" :
                    "Interaction with MQTT broker is impossible because device configuration is not loaded");
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
