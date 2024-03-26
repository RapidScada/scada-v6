// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Comm.Lang;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevOpcUaLogic : DeviceLogic
    {
        /// <summary>
        /// Contains data common to a communication line.
        /// </summary>
        private class OpcUaLineData
        {
            public bool FatalError { get; set; } = false;
            public OpcClientHelper ClientHelper { get; init; }
            public override string ToString() => CommPhrases.SharedObject;
        }

        /// <summary>
        /// Represents metadata about a device tag.
        /// </summary>
        private class DeviceTagMeta
        {
            public Type ActualDataType { get; set; }
        }

        private readonly OpcDeviceConfig config;             // the device configuration
        private readonly object opcLock;                     // synchronizes communication with OPC server

        private bool configError;                            // indicates that that device configuration is not loaded
        private OpcUaLineData lineData;                      // data common to the communication line
        private Dictionary<int, CommandConfig> cmdByNum;     // the commands accessed by number
        private Dictionary<string, CommandConfig> cmdByCode; // the commands accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevOpcUaLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new OpcDeviceConfig();
            opcLock = new object();

            configError = false;
            lineData = null;
            cmdByNum = null;
            cmdByCode = null;

            CanSendCommands = true;
            ConnectionRequired = false;
        }


        /// <summary>
        /// Initializes data common to the communication line.
        /// </summary>
        private void InitLineData()
        {
            if (LineContext.SharedData.TryGetValueOfType(nameof(OpcUaLineData), out OpcUaLineData data))
            {
                lineData = data;
            }
            else
            {
                OpcLineConfig lineConfig = new();
                bool lineConfigError = false;

                if (!lineConfig.Load(Storage, OpcLineConfig.GetFileName(LineContext.CommLineNum), out string errMsg))
                {
                    Log.WriteLine(errMsg);
                    Log.WriteLine(Locale.IsRussian ?
                        "Взаимодействие с OPC-сервером невозможно, т.к. конфигурация линии не загружена" :
                        "Interaction with OPC server is impossible because line configuration is not loaded");
                    lineConfigError = true;
                }

                lineData = new OpcUaLineData()
                {
                    FatalError = lineConfigError,
                    ClientHelper = new OpcClientHelper(lineConfig.ConnectionOptions, Log, Storage) 
                };

                LineContext.SharedData[nameof(OpcUaLineData)] = lineData;
            }
        }

        /// <summary>
        /// Initializes command maps.
        /// </summary>
        private void InitCommandMaps()
        {
            cmdByNum = new Dictionary<int, CommandConfig>();
            cmdByCode = new Dictionary<string, CommandConfig>();

            // explicit commands
            foreach (CommandConfig commandConfig in config.Commands)
            {
                if (commandConfig.CmdNum > 0 && !cmdByNum.ContainsKey(commandConfig.CmdNum))
                    cmdByNum.Add(commandConfig.CmdNum, commandConfig);

                if (!string.IsNullOrEmpty(commandConfig.CmdCode) && !cmdByCode.ContainsKey(commandConfig.CmdCode))
                    cmdByCode.Add(commandConfig.CmdCode, commandConfig);
            }

            // commands from subscriptions
            foreach (SubscriptionConfig subscriptionConfig in config.Subscriptions)
            {
                foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                {
                    if (itemConfig.Active && 
                        !string.IsNullOrEmpty(itemConfig.TagCode) && 
                        !cmdByCode.ContainsKey(itemConfig.TagCode))
                    {
                        cmdByCode.Add(itemConfig.TagCode, new CommandConfig
                        {
                            NodeID = itemConfig.NodeID,
                            DisplayName = itemConfig.DisplayName,
                            CmdCode = itemConfig.TagCode,
                            DataTypeName = itemConfig.DataTypeName
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Finds a command configuration by command code or number.
        /// </summary>
        private bool FindCommandConfig(TeleCommand cmd, out CommandConfig commandConfig)
        {
            if (cmdByCode != null && !string.IsNullOrEmpty(cmd.CmdCode) &&
                cmdByCode.TryGetValue(cmd.CmdCode, out commandConfig))
            {
                return true;
            }

            if (cmdByNum != null && cmd.CmdNum > 0 &&
                cmdByNum.TryGetValue(cmd.CmdNum, out commandConfig))
            {
                return true;
            }

            commandConfig = null;
            return false;
        }

        /// <summary>
        /// Converts the array element value received from the OPC server to a double.
        /// </summary>
        private double ConvertArrayElem(object val, out TagFormat tagFormat)
        {
            try
            {
                if (val is string strVal)
                {
                    // string arrays not supported
                    tagFormat = TagFormat.FloatNumber;
                    return 0.0;
                }
                else if (val is DateTime dtVal)
                {
                    tagFormat = TagFormat.DateTime;
                    return dtVal.ToOADate();
                }
                else
                {
                    tagFormat = TagFormat.FloatNumber;
                    return Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {
                Log.WriteInfo(ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при конвертировании элемента массива" :
                    "Error converting array element"));
                tagFormat = TagFormat.FloatNumber;
                return 0.0;
            }
        }

        /// <summary>
        /// Sets value, status and format of the specified tag.
        /// </summary>
        private void SetTagData(DeviceTag deviceTag, object val, int stat)
        {
            try
            {
                if (deviceTag.Aux is DeviceTagMeta tagMeta && val != null)
                    tagMeta.ActualDataType = val.GetType();

                DeviceData.Set(deviceTag, val, stat);
            }
            catch (Exception ex)
            {
                Log.WriteInfo(ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при установке данных тега" :
                    "Error setting tag data"));
            }
        }

        /// <summary>
        /// Writes an item value to the OPC server.
        /// </summary>
        private bool WriteItemValue(ISession opcSession, CommandConfig commandConfig, double cmdVal, string cmdData)
        {
            try
            {
                // get data type
                string dataTypeName = commandConfig.DataTypeName;
                Type itemDataType = null;
                object itemVal;

                if (string.IsNullOrEmpty(dataTypeName))
                {
                    if (DeviceTags.TryGetTag(commandConfig.CmdCode, out DeviceTag deviceTag) &&
                        deviceTag.Aux is DeviceTagMeta tagMeta)
                    {
                        itemDataType = tagMeta.ActualDataType;
                    }
                }
                else
                {
                    itemDataType = Type.GetType(dataTypeName, false, true);
                }

                if (itemDataType == null)
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Не удалось получить тип данных {0}" :
                        "Unable to get data type {0}", dataTypeName);
                }

                if (itemDataType.IsArray)
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Тип данных {0} не поддерживается" :
                        "Data type {0} not supported", dataTypeName);
                }

                // define command value
                try
                {
                    if (itemDataType == typeof(string))
                        itemVal = cmdData;
                    else if (itemDataType == typeof(DateTime))
                        itemVal = DateTime.FromOADate(cmdVal);
                    else
                        itemVal = Convert.ChangeType(cmdVal, itemDataType);
                }
                catch
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Не удалось привести значение к типу {0}" :
                        "Unable to convert value to the type {0}", itemDataType.FullName);
                }

                // write value
                Log.WriteLine(Locale.IsRussian ?
                    "Отправка значения OPC-серверу: {0} = {1}" :
                    "Send value to the OPC server: {0} = {1}", commandConfig.DisplayName, itemVal);

                WriteValue valueToWrite = new()
                {
                    NodeId = commandConfig.NodeID,
                    AttributeId = Attributes.Value,
                    Value = new DataValue(new Variant(itemVal))
                };

                opcSession.Write(null, new WriteValueCollection { valueToWrite }, 
                    out StatusCodeCollection results, out _);

                if (StatusCode.IsGood(results[0]))
                {
                    Log.WriteLine(CommPhrases.ResponseOK);
                    return true;
                }
                else
                {
                    Log.WriteLine(CommPhrases.ResponseError);
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
        /// Calls a method of the OPC server.
        /// </summary>
        private bool CallMethod(ISession opcSession, CommandConfig commandConfig, string cmdData)
        {
            try
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Вызов метода {0}" :
                    "Call the {0} method", commandConfig.DisplayName);

                IList<object> methodResults = opcSession.Call(
                    new NodeId(commandConfig.ParentNodeID),
                    new NodeId(commandConfig.NodeID),
                    GetMethodArgs(cmdData));

                if (methodResults == null)
                {
                    Log.WriteLine(CommPhrases.ResponseOK);
                }
                else
                {
                    for (int i = 0, cnt = methodResults.Count; i < cnt; i++)
                    {
                        Log.WriteLine("Result[{0}] = {1}", i, methodResults[i]);
                    }
                }

                Log.WriteLine(CommPhrases.ResponseOK);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets OPC method arguments from command data.
        /// </summary>
        private static object[] GetMethodArgs(string cmdData)
        {
            if (string.IsNullOrEmpty(cmdData))
                return Array.Empty<object>();

            // each line contains argument type and value, for example
            // double: 1.2
            List<object> args = new();
            string[] lines = cmdData.Split('\n');

            foreach (string line in lines)
            {
                int colonIdx = line.IndexOf(':');

                if (colonIdx >= 0)
                {
                    string typeName = line[..colonIdx];
                    string argVal = line[(colonIdx + 1)..];

                    try
                    {
                        args.Add(KnownTypes.Parse(typeName, argVal));
                    }
                    catch (FormatException)
                    {
                        throw new ScadaException(Locale.IsRussian ?
                            "Неверное значение аргумента \"{0}\"" :
                            "Invalid argument value \"{0}\"", argVal);
                    }
                }
                else
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Неверный аргумент метода \"{0}\"" :
                        "Invalid method argument \"{0}\"", line);
                }
            }

            return args.ToArray();
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            InitLineData();

            if (config.Load(Storage, OpcDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                InitCommandMaps();
                lineData.ClientHelper.AddSubscriptions(this, config);
            }
            else
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);
                configError = true;
            }
        }

        /// <summary>
        /// Performs actions when terminating a communication line.
        /// </summary>
        public override void OnCommLineTerminate()
        {
            lineData.ClientHelper.Disconnect();
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            if (configError)
                return;

            foreach (SubscriptionConfig subscriptionConfig in config.Subscriptions)
            {
                TagGroup tagGroup = new(subscriptionConfig.DisplayName);

                foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                {
                    DeviceTag deviceTag = tagGroup.AddTag(itemConfig.TagCode, itemConfig.DisplayName);
                    deviceTag.Aux = new DeviceTagMeta();
                    itemConfig.Tag = deviceTag;

                    if (itemConfig.IsString)
                    {
                        deviceTag.DataType = TagDataType.Unicode;
                        deviceTag.DataLen = DeviceTag.CalcDataLength(itemConfig.DataLength, TagDataType.Unicode);
                        deviceTag.Format = TagFormat.String;
                    }
                    else if (itemConfig.IsArray)
                    {
                        deviceTag.DataLen = itemConfig.DataLength;
                    }
                }

                DeviceTags.AddGroup(tagGroup);
            }
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (lineData.FatalError || configError)
            {
                DeviceStatus = DeviceStatus.Error;
            }
            else if (lineData.ClientHelper.OpcSession == null)
            {
                if (!(lineData.ClientHelper.Connect() && lineData.ClientHelper.CreateSubscriptions(true)))
                {
                    DeviceStatus = DeviceStatus.Error;
                    DeviceData.Invalidate();
                }
            }
            else if (!lineData.ClientHelper.IsConnected)
            {
                DeviceStatus = DeviceStatus.Error;
                DeviceData.Invalidate();
            }

            SleepPollingDelay();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {
            try
            {
                Monitor.Enter(opcLock);
                base.SendCommand(cmd);
                LastRequestOK = false;
                ISession opcSession = lineData.ClientHelper.OpcSession;

                if (opcSession == null)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Ошибка: соединение с OPC-сервером не установлено" :
                        "Error: connection with the OPC server is not established");
                }
                else if (!FindCommandConfig(cmd, out CommandConfig commandConfig))
                {
                    Log.WriteLine(CommPhrases.InvalidCommand);
                }
                else
                {
                    LastRequestOK = commandConfig.IsMethod
                        ? CallMethod(opcSession, commandConfig, cmd.GetCmdDataString())
                        : WriteItemValue(opcSession, commandConfig, cmd.CmdVal, cmd.GetCmdDataString());
                }

                FinishCommand();
            }
            finally
            {
                Monitor.Exit(opcLock);
            }
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public void ProcessDataChanges(SubscriptionTag subscriptionTag, NotificationMessage notificationMessage)
        {
            if (subscriptionTag == null || notificationMessage == null)
                return;

            try
            {
                Monitor.Enter(opcLock);
                LastSessionTime = DateTime.UtcNow;
                LastRequestOK = true;

                Log.WriteLine();
                Log.WriteAction(Locale.IsRussian ?
                    "Устройство {0}. Обработка новых данных. Подписка: {1}" :
                    "Device {0}. Process new data. Subscription: {1}",
                    Title, subscriptionTag.SubscriptionConfig.DisplayName);

                foreach (MonitoredItemNotification change in notificationMessage.GetDataChanges(false))
                {
                    if (subscriptionTag.Subscription?.FindItemByClientHandle(change.ClientHandle) is 
                        MonitoredItem monitoredItem)
                    {
                        Log.WriteLine("{0} {1} = {2} ({3})", CommPhrases.ReceiveNotation,
                            monitoredItem.DisplayName, change.Value, change.Value.StatusCode);

                        if (monitoredItem.Handle is ItemTag itemTag &&
                            itemTag.DeviceTag is DeviceTag deviceTag)
                        {
                            int tagStatus = StatusCode.IsGood(change.Value.StatusCode) ?
                                CnlStatusID.Defined : CnlStatusID.Undefined;

                            if (itemTag.ItemConfig.IsArray && change.Value.Value is Array arrVal)
                            {
                                int arrLen = Math.Min(itemTag.ItemConfig.DataLength, arrVal.Length);
                                double[] arr = new double[arrLen];
                                TagFormat tagFormat = TagFormat.FloatNumber;

                                for (int i = 0; i < arrLen; i++)
                                {
                                    arr[i] = ConvertArrayElem(arrVal.GetValue(i), out TagFormat format);
                                    if (i == 0)
                                        tagFormat = format;
                                }

                                DeviceData.SetDoubleArray(deviceTag.Index, arr, tagStatus);
                                DeviceTags[deviceTag.Index].Format = tagFormat;
                            }
                            else
                            {
                                SetTagData(deviceTag, change.Value.Value, tagStatus);
                            }
                        }
                        else
                        {
                            Log.WriteLine(Locale.IsRussian ?
                                "Ошибка: тег не найден" :
                                "Error: tag not found");
                        }
                    }
                    else
                    {
                        Log.WriteLine(Locale.IsRussian ?
                            "{0} Неизвестный элемент {1} = {2} ({3})" :
                            "{0} Unknown item {1} = {2} ({3})", 
                            CommPhrases.ReceiveNotation, change.ClientHandle, change.Value, change.Value.StatusCode);
                    }
                }

                // events are not really implemented
                foreach (EventFieldList eventFields in notificationMessage.GetEvents(false))
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Новое событие {0}" :
                        "New event {0}", eventFields);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при обработке новых данных" :
                    "Error processing new data");
                LastRequestOK = false;
            }
            finally
            {
                FinishSession();
                Monitor.Exit(opcLock);
            }
        }
    }
}
