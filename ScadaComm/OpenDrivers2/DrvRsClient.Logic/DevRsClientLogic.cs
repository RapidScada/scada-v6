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
using Scada.Protocol;

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

        /// <summary>
        /// Represents an item group.
        /// </summary>
        private class ItemGroup(ItemGroupConfig config, TagGroup tagGroup)
        {
            public long CnlListID = 0;
            public ItemGroupConfig Config { get; } = config ?? throw new ArgumentNullException(nameof(config));
            public TagGroup TagGroup { get; } = tagGroup ?? throw new ArgumentNullException(nameof(tagGroup));
            public Dictionary<int, DeviceTag> TagByCnlNum { get; } = [];
            public int[] CnlNumsToRequest { get; set; } = null;
            public int FirstTagIndex => TagGroup.DeviceTags.FirstOrDefault()?.Index ?? -1;
            public int Count => TagGroup.DeviceTags.Count;
        }

        private readonly RsClientLineConfig lineConfig;     // the communication line configuration
        private readonly RsClientDeviceConfig deviceConfig; // the device configuration
        private readonly List<ItemGroup> itemGroups;        // the active item groups

        private bool deviceConfigError;    // loading the device configuration failed
        private RsClientLineData lineData; // data common to the communication line


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevRsClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            lineConfig = new RsClientLineConfig();
            this.deviceConfig = new RsClientDeviceConfig();
            itemGroups = [];

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
        private bool RequestCurrentData(ItemGroup itemGroup, out CnlData[] cnlDataArr)
        {
            try
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Запрос текущих данных" :
                    "Request current data");

                cnlDataArr = itemGroup.CnlListID > 0 
                    ? lineData.ScadaClient.GetCurrentData(ref itemGroup.CnlListID)
                    : lineData.ScadaClient.GetCurrentData(itemGroup.CnlNumsToRequest, true, out itemGroup.CnlListID);

                if (itemGroup.CnlListID > 0 && cnlDataArr.Length > 0)
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
        private void SetDeviceData(ItemGroup itemGroup, CnlData[] cnlDataArr)
        {
            // assumed that itemGroup.CnlNumsToRequest and cnlDataArr are of the same length
            for (int i = 0, len = itemGroup.CnlNumsToRequest.Length; i < len; i++)
            {
                if (itemGroup.TagByCnlNum.TryGetValue(itemGroup.CnlNumsToRequest[i], out DeviceTag deviceTag))
                {
                    CnlData cnlData = cnlDataArr[i];
                    DeviceData.Set(deviceTag.Index, cnlData.Val, cnlData.Stat);
                }
            }
        }

        /// <summary>
        /// Sets the tags of the group to undefined.
        /// </summary>
        private void InvalidateDeviceData(ItemGroup itemGroup)
        {
            if (itemGroup.Count > 0)
                DeviceData.Invalidate(itemGroup.FirstTagIndex, itemGroup.Count);
        }

        /// <summary>
        /// Sends the command to Server.
        /// </summary>
        private bool SendCommand(ItemConfig itemConfig, TeleCommand srcCmd)
        {
            try
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Отправка команды на канал {0}" :
                    "Send command to channel {0}", itemConfig.CnlNum);

                CommandResult result = lineData.ScadaClient.SendCommand(
                    new TeleCommand
                    {
                        CnlNum = itemConfig.CnlNum,
                        CmdVal = srcCmd.CmdVal,
                        CmdData = srcCmd.CmdData
                    },
                    WriteCommandFlags.Default); // command cannot get into infinite loop

                if (result.IsSuccessful)
                {
                    Log.WriteLine(CommPhrases.ResponseOK);
                    return true;
                }
                else
                {
                    Log.WriteLine(result.ErrorMessage);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                return false;
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
            BaseTable<Cnl> cnlTable = lineConfig.UseDefaultConnection 
                ? CommContext.ConfigDatabase?.CnlTable 
                : null;

            foreach (ItemGroupConfig itemGroupConfig in deviceConfig.ItemGroups.Where(g => g.Items.Count > 0))
            {
                TagGroup tagGroup = new(itemGroupConfig.Name) { Hidden = !itemGroupConfig.Active };
                ItemGroup itemGroup = new(itemGroupConfig, tagGroup);
                int itemCnt = itemGroupConfig.Items.Count;
                List<int> cnlNumList = new(itemCnt);
                HashSet<int> cnlNumSet = new(itemCnt);

                foreach (ItemConfig itemConfig in itemGroupConfig.Items)
                {
                    int cnlNum = itemConfig.CnlNum;
                    string tagCode = "Cnl" + cnlNum;
                    string tagName = cnlTable?.GetItem(cnlNum) is Cnl cnl
                        ? cnl.Name
                        : Locale.IsRussian ? "Канал " + cnlNum : "Channel " + cnlNum;
                    DeviceTag deviceTag = tagGroup.AddTag(tagCode, tagName);
                    deviceTag.Aux = itemConfig;

                    if (cnlNum > 0 && cnlNumSet.Add(cnlNum))
                    {
                        cnlNumList.Add(cnlNum);
                        itemGroup.TagByCnlNum.Add(cnlNum, deviceTag);
                    }
                }

                DeviceTags.AddGroup(tagGroup);

                if (itemGroupConfig.Active)
                {
                    itemGroup.CnlNumsToRequest = [.. cnlNumList];
                    itemGroups.Add(itemGroup);
                }
            }
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
                else if (itemGroups.Count > 0)
                {
                    foreach (ItemGroup itemGroup in itemGroups)
                    {
                        if (LastRequestOK)
                        {
                            LastRequestOK = false;
                            int tryNum = 0;

                            while (RequestNeeded(ref tryNum))
                            {
                                if (RequestCurrentData(itemGroup, out CnlData[] cnlDataArr))
                                {
                                    LastRequestOK = true;
                                    SetDeviceData(itemGroup, cnlDataArr);
                                }

                                FinishRequest();
                                tryNum++;
                            }
                        }

                        if (IsTerminated)
                            break;

                        if (!LastRequestOK)
                            InvalidateDeviceData(itemGroup);
                    }
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Отсутствуют активные группы элементов для запроса" :
                        "No active item groups to request");
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
            base.SendCommand(cmd);
            LastRequestOK = false;

            if (lineData.FatalError || deviceConfigError)
            {
                Log.WriteLine(CommPhrases.UnablePollDevice);
            }
            else if (string.IsNullOrEmpty(cmd.CmdCode) || 
                !DeviceTags.TryGetTag(cmd.CmdCode, out DeviceTag deviceTag) ||
                deviceTag.Aux is not ItemConfig itemConfig)
            {
                Log.WriteLine(CommPhrases.InvalidCommand);
            }
            else if (itemConfig.ReadOnly)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0} Элемент {1} предназначен только для чтения" :
                    "{0} Element {1} is read-only",
                    CommPhrases.ErrorPrefix, cmd.CmdCode);
            }
            else
            {
                int tryNum = 0;

                while (RequestNeeded(ref tryNum))
                {
                    if (SendCommand(itemConfig, cmd))
                        LastRequestOK = true;

                    FinishRequest();
                    tryNum++;
                }
            }

            FinishCommand();
        }
    }
}
