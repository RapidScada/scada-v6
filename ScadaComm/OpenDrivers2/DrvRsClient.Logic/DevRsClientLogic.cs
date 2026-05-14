// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvRsClient.Config;
using Scada.Comm.Lang;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
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

        private readonly RsClientLineConfig lineConfig;          // the communication line configuration
        private readonly RsClientDeviceConfig deviceConfig;      // the device configuration
        private readonly Dictionary<int, DeviceTag> tagByCnlNum; // the device tags accessed by channel number

        private bool deviceConfigError;    // loading the device configuration failed
        private RsClientLineData lineData; // data common to the communication line
        private int[] cnlNumsToRequest;    // the channel numbers to request data
        private long cnlListID;            // to cache data queries


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevRsClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            lineConfig = new RsClientLineConfig();
            this.deviceConfig = new RsClientDeviceConfig();
            tagByCnlNum = [];

            deviceConfigError = false;
            lineData = null;
            cnlNumsToRequest = null;
            cnlListID = 0;

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
            try
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Запрос текущих данных" :
                    "Request current data");

                cnlDataArr = cnlListID > 0 
                    ? lineData.ScadaClient.GetCurrentData(ref cnlListID)
                    : lineData.ScadaClient.GetCurrentData(cnlNumsToRequest, false, out cnlListID);

                if (cnlListID > 0 && cnlDataArr.Length > 0)
                {
                    Log.WriteLine(CommPhrases.ResponseOK);
                    return true;
                }
                else
                {
                    Log.WriteLine(CommPhrases.ErrorPrefix + CommonPhrases.NoData);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                cnlDataArr = null;
                return false;
            }
        }

        /// <summary>
        /// Sets the device tags according to the channel data.
        /// </summary>
        private void SetDeviceData(CnlData[] cnlDataArr)
        {
            for (int i = 0, len = cnlNumsToRequest.Length; i < len; i++)
            {
                if (tagByCnlNum.TryGetValue(cnlNumsToRequest[i], out DeviceTag deviceTag))
                {
                    CnlData cnlData = cnlDataArr[i];
                    DeviceData.Set(deviceTag.Index, cnlData.Val, cnlData.Stat);
                }
            }
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
            TagGroup tagGroup = new();
            BaseTable<Cnl> cnlTable = lineConfig.UseDefaultConnection 
                ? CommContext.ConfigDatabase?.CnlTable 
                : null;

            int itemCnt = deviceConfig.Items.Count;
            List<int> cnlNumList = new(itemCnt);
            HashSet<int> cnlNumSet = new(itemCnt);

            foreach (ItemConfig itemConfig in deviceConfig.Items)
            {
                int cnlNum = itemConfig.CnlNum;
                DeviceTag deviceTag;

                if (cnlNum > 0 && cnlNumSet.Add(cnlNum))
                {
                    string tagCode = "Cnl" + cnlNum;
                    string tagName = cnlTable?.GetItem(cnlNum) is Cnl cnl
                        ? cnl.Name
                        : Locale.IsRussian ? "Канал " + cnlNum : "Channel " + cnlNum;
                    deviceTag = tagGroup.AddTag(tagCode, tagName);
                    deviceTag.Aux = itemConfig;
                    cnlNumList.Add(cnlNum);
                    tagByCnlNum.Add(cnlNum, deviceTag);
                }
            }

            DeviceTags.AddGroup(tagGroup);
            DeviceTags.FlattenGroups = true;
            cnlNumsToRequest = [.. cnlNumList];
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
                else if (cnlNumsToRequest.Length > 0)
                {
                    LastRequestOK = false;
                    int tryNum = 0;

                    while (RequestNeeded(ref tryNum))
                    {
                        if (RequestCurrentData(out CnlData[] cnlDataArr))
                        {
                            LastRequestOK = true;
                            SetDeviceData(cnlDataArr);
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
