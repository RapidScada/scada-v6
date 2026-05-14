// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvRsClient.Config;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvRsClient.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevRsClientLogic : DeviceLogic
    {
        /// <summary>
        /// Contains data common to a communication line.
        /// </summary>
        private class RsClientLineData
        {
            public bool FatalError { get; init; }
            public ScadaClient ScadaClient { get; init; }
            public override string ToString() => CommPhrases.SharedObject;
        }

        private readonly RsClientLineConfig lineConfig;     // the communication line configuration
        private readonly RsClientDeviceConfig deviceConfig; // the device configuration

        private bool deviceConfigError;                     // loading the device configuration failed
        private RsClientLineData lineData;                  // data common to the communication line


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevRsClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            lineConfig = new RsClientLineConfig();
            this.deviceConfig = new RsClientDeviceConfig();

            deviceConfigError = false;
            lineData = null;

            CanSendCommands = true;
            ConnectionRequired = false;
        }


        /// <summary>
        /// Initializes data common to the communication line.
        /// </summary>
        private void InitLineData()
        {
            if (LineContext.SharedData.TryGetValueOfType(nameof(RsClientLineData), out RsClientLineData data))
            {
                lineData = data;
            }
            else
            {
                bool lineConfigError = false;

                if (!lineConfig.Load(Storage, LineContext.CommLineNum, out string errMsg))
                {
                    Log.WriteLine(errMsg);
                    Log.WriteLine(Locale.IsRussian ?
                        "Взаимодействие с сервером SCADA невозможно, т.к. конфигурация линии не загружена" :
                        "Interaction with SCADA server is impossible because line configuration is not loaded");
                    lineConfigError = true;
                }

                lineData = new RsClientLineData
                {
                    FatalError = lineConfigError,
                    ScadaClient = new ScadaClient(lineConfig.UseDefaultConnection 
                        ? CommContext.AppConfig.ConnectionOptions 
                        : lineConfig.ConnectionOptions)
                };

                LineContext.SharedData[nameof(RsClientLineData)] = lineData;
            }
        }

        /// <summary>
        /// Requests current data from Server.
        /// </summary>
        private bool RequestCurrentData(out CnlData[] cnlDataArr)
        {
            cnlDataArr = [];
            return true;
        }

        /// <summary>
        /// Sets the device tags according to the channel data.
        /// </summary>
        private void SetTagData(CnlData[] cnlDataArr)
        {

        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            InitLineData();

            if (!deviceConfig.Load(Storage, DeviceNum, out string errMsg))
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);
                deviceConfigError = true;
            }
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {

        }

        /// <summary>
        /// Initializes the device data.
        /// </summary>
        public override void InitDeviceData()
        {
            base.InitDeviceData();

            if (lineData.FatalError)
                DeviceStatus = DeviceStatus.Error;
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (lineData.FatalError)
            {
                SleepPollingDelay();
            }
            else
            {
                base.Session();

                if (deviceConfigError)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Невозможно выполнить запрос данных, потому что конфигурация устройства не загружена" :
                        "Unable to request data because device configuration is not loaded");
                    SleepPollingDelay();
                    LastRequestOK = false;
                }
                else if (deviceConfig.Items.Count > 0)
                {
                    LastRequestOK = false;
                    int tryNum = 0;

                    while (RequestNeeded(ref tryNum))
                    {
                        if (RequestCurrentData(out CnlData[] cnlDataArr))
                        {
                            LastRequestOK = true;
                            SetTagData(cnlDataArr);
                        }

                        FinishRequest();
                        tryNum++;
                    }
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Отсутствуют элементы для запроса" :
                        "No items to request");
                    SleepPollingDelay();
                }

                FinishSession();
            }
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {

        }
    }
}
