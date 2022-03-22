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
using System;
using System.Collections.Generic;
using System.Threading;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevOpcUaLogic : DeviceLogic
    {
        /// <summary>
        /// Represents metadata about a subscription.
        /// </summary>
        private class SubscriptionTag
        {
            public SubscriptionTag(Subscription subscription)
            {
                Subscription = subscription;
                ItemsByNodeID = new Dictionary<string, ItemTag>();
            }
            public Subscription Subscription { get; }
            public Dictionary<string, ItemTag> ItemsByNodeID { get; }
        }

        /// <summary>
        /// Represents metadata about a monitored item.
        /// </summary>
        private class ItemTag
        {
            public ItemConfig ItemConfig { get; set; }
            public DeviceTag DeviceTag { get; set; }
        }

        /// <summary>
        /// Represents metadata about a device tag.
        /// </summary>
        private class DeviceTagMeta
        {
            public Type ActualDataType { get; set; }
        }


        /// <summary>
        /// The period of reconnecting to OPC server if a connection lost, ms
        /// </summary>
        private const int ReconnectPeriod = 10000;
        /// <summary>
        /// The delay before reconnect.
        /// </summary>
        private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(5);

        private readonly object opcLock;                      // synchronizes communication with OPC server
        private OpcDeviceConfig opcDeviceConfig;              // the device configuration
        private bool connected;                               // connection with OPC server is established
        private DateTime connAttemptDT;                       // the timestamp of a connection attempt
        private Session opcSession;                           // the OPC session
        private SessionReconnectHandler reconnectHandler;     // the object needed to reconnect
        private Dictionary<uint, SubscriptionTag> subscrByID; // the subscription tags accessed by IDs
        private Dictionary<int, CommandConfig> cmdByNum;      // the commands accessed by number
        private Dictionary<string, CommandConfig> cmdByCode;  // the commands accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevOpcUaLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            opcLock = new object();
            opcDeviceConfig = null;
            connected = false;
            connAttemptDT = DateTime.MinValue;
            opcSession = null;
            reconnectHandler = null;
            subscrByID = null;
            cmdByNum = null;
            cmdByCode = null;

            CanSendCommands = true;
            ConnectionRequired = false;
        }


        /// <summary>
        /// Initializes command maps.
        /// </summary>
        private void InitCmdMaps()
        {
            cmdByNum = new Dictionary<int, CommandConfig>();
            cmdByCode = new Dictionary<string, CommandConfig>();

            // explicit commands
            foreach (CommandConfig commandConfig in opcDeviceConfig.Commands)
            {
                if (commandConfig.CmdNum > 0 && !cmdByNum.ContainsKey(commandConfig.CmdNum))
                    cmdByNum.Add(commandConfig.CmdNum, commandConfig);

                if (!string.IsNullOrEmpty(commandConfig.CmdCode) && !cmdByCode.ContainsKey(commandConfig.CmdCode))
                    cmdByCode.Add(commandConfig.CmdCode, commandConfig);
            }

            // commands from subscriptions
            foreach (SubscriptionConfig subscriptionConfig in opcDeviceConfig.Subscriptions)
            {
                foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                {
                    if (itemConfig.Active && 
                        !string.IsNullOrEmpty(itemConfig.TagCode) && 
                        !cmdByCode.ContainsKey(itemConfig.TagCode))
                    {
                        // created command based on item, having empty data type
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
        /// Connects to the OPC server.
        /// </summary>
        private void ConnectToOpcServer()
        {
            try
            {
                OpcHelper helper = new OpcHelper(AppDirs, Log, DeviceNum, true);
                connected = helper.ConnectAsync(opcDeviceConfig.ConnectionOptions, PollingOptions.Timeout).Result;
                opcSession = helper.OpcSession;
                opcSession.KeepAlive += OpcSession_KeepAlive;
                opcSession.Notification += OpcSession_Notification;
            }
            catch (Exception ex)
            {
                connected = false;
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при соединении с OPC-сервером" :
                    "Error connecting OPC server");
            }
        }

        /// <summary>
        /// Creates subscriptions according to the configuration.
        /// </summary>
        private bool CreateSubscriptions()
        {
            try
            {
                if (opcSession == null)
                    throw new InvalidOperationException("OPC session must not be null.");

                subscrByID = new Dictionary<uint, SubscriptionTag>();

                foreach (SubscriptionConfig subscriptionConfig in opcDeviceConfig.Subscriptions)
                {
                    if (!subscriptionConfig.Active)
                        continue;

                    Subscription subscription = new Subscription(opcSession.DefaultSubscription)
                    {
                        DisplayName = subscriptionConfig.DisplayName,
                        PublishingInterval = subscriptionConfig.PublishingInterval
                    };

                    SubscriptionTag subscriptionTag = new SubscriptionTag(subscription);

                    foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                    {
                        if (!itemConfig.Active)
                            continue;

                        subscription.AddItem(new MonitoredItem(subscription.DefaultItem)
                        {
                            StartNodeId = itemConfig.NodeID,
                            DisplayName = itemConfig.DisplayName
                        });

                        if (itemConfig.Tag is DeviceTag deviceTag)
                        {
                            subscriptionTag.ItemsByNodeID[itemConfig.NodeID] = new ItemTag
                            {
                                ItemConfig = itemConfig,
                                DeviceTag = deviceTag
                            };
                        }
                    }

                    opcSession.AddSubscription(subscription);
                    subscription.Create();
                    subscrByID[subscription.Id] = subscriptionTag;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при создании подписок" :
                    "Error creating subscriptions");
                return false;
            }
        }

        /// <summary>
        /// Clears all subscriptions of the OPC session.
        /// </summary>
        private void ClearSubscriptions()
        {
            try
            {
                subscrByID = null;
                opcSession.RemoveSubscriptions(new List<Subscription>(opcSession.Subscriptions));
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при очистке подписок" :
                    "Error clearing subscriptions");
            }
        }

        /// <summary>
        /// Reconnects if needed.
        /// </summary>
        private void OpcSession_KeepAlive(Session sender, KeepAliveEventArgs e)
        {
            if (e.Status != null && ServiceResult.IsNotGood(e.Status))
            {
                Log.WriteLine("{0} {1}/{2}", e.Status, sender.OutstandingRequestCount, sender.DefunctRequestCount);

                if (reconnectHandler == null)
                {
                    DeviceData.Invalidate();
                    DeviceStatus = DeviceStatus.Error;
                    Log.WriteLine(Locale.IsRussian ?
                        "Переподключение к OPC-серверу" :
                        "Reconnecting to OPC server");
                    reconnectHandler = new SessionReconnectHandler();
                    reconnectHandler.BeginReconnect(sender, ReconnectPeriod, OpcSession_ReconnectComplete);
                }
            }
        }

        /// <summary>
        /// Processes the reconnect procedure.
        /// </summary>
        private void OpcSession_ReconnectComplete(object sender, EventArgs e)
        {
            // ignore callbacks from discarded objects
            if (!ReferenceEquals(sender, reconnectHandler))
            {
                return;
            }

            opcSession = reconnectHandler.Session;
            reconnectHandler.Dispose();
            reconnectHandler = null;

            // after reconnecting, the subscriptions are automatically recreated, but with the wrong IDs and names,
            // so it's needed to clear them and create again
            ClearSubscriptions();
            DeviceStatus = CreateSubscriptions() ? DeviceStatus.Normal : DeviceStatus.Error;
            Log.WriteLine(Locale.IsRussian ?
                "Переподключено" :
                "Reconnected");
        }

        /// <summary>
        /// Processes new data received from OPC server.
        /// </summary>
        private void OpcSession_Notification(Session session, NotificationEventArgs e)
        {
            try
            {
                Monitor.Enter(opcLock);
                Log.WriteLine();
                LastSessionTime = DateTime.UtcNow;

                if (subscrByID != null &&
                    subscrByID.TryGetValue(e.Subscription.Id, out SubscriptionTag subscriptionTag))
                {
                    Log.WriteAction(Locale.IsRussian ?
                        "Устройство {0}. Обработка новых данных. Подписка: {1}" :
                        "Device {0}. Process new data. Subscription: {1}",
                        DeviceNum, e.Subscription.DisplayName);
                    ProcessDataChanges(subscriptionTag, e.NotificationMessage);
                    ProcessEvents(e.NotificationMessage);
                    LastRequestOK = true;
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Ошибка: подписка [{0}] \"{1}\" не найдена" :
                        "Error: subscription [{0}] \"{1}\" not found",
                        e.Subscription.Id, e.Subscription.DisplayName);
                    LastRequestOK = false;
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

        /// <summary>
        /// Processes new data.
        /// </summary>
        private void ProcessDataChanges(SubscriptionTag subscriptionTag, NotificationMessage notificationMessage)
        {
            foreach (MonitoredItemNotification change in notificationMessage.GetDataChanges(false))
            {
                if (subscriptionTag.Subscription.FindItemByClientHandle(change.ClientHandle) is 
                    MonitoredItem monitoredItem)
                {
                    if (subscriptionTag.ItemsByNodeID.TryGetValue(monitoredItem.StartNodeId.ToString(),
                        out ItemTag itemTag))
                    {
                        Log.WriteLine("{0} {1} = {2} ({3})", CommPhrases.ReceiveNotation, 
                            monitoredItem.DisplayName, change.Value, change.Value.StatusCode);

                        int tagIndex = itemTag.DeviceTag.Index;
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

                            DeviceData.SetDoubleArray(tagIndex, arr, tagStatus);
                            DeviceTags[tagIndex].Format = tagFormat;
                        }
                        else
                        {
                            SetTagData(tagIndex, change.Value.Value, tagStatus);
                        }
                    }
                    else
                    {
                        Log.WriteLine(Locale.IsRussian ?
                            "Ошибка: тег \"{0}\" не найден" :
                            "Error: tag \"{0}\" not found", monitoredItem.StartNodeId);
                    }
                }
            }
        }

        /// <summary>
        /// Processes new events.
        /// </summary>
        private void ProcessEvents(NotificationMessage notificationMessage)
        {
            foreach (EventFieldList eventFields in notificationMessage.GetEvents(true))
            {
                // events are not really implemented
                Log.WriteLine(Locale.IsRussian ?
                    "Новое событие {0}" :
                    "New event {0}", eventFields);
            }
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
        private void SetTagData(int tagIndex, object val, int stat)
        {
            try
            {
                DeviceTag deviceTag = DeviceTags[tagIndex];

                if (deviceTag.Aux is DeviceTagMeta tagMeta && val != null)
                    tagMeta.ActualDataType = val.GetType();

                if (val is string strVal)
                {
                    deviceTag.DataType = TagDataType.Unicode;
                    deviceTag.Format = TagFormat.String;
                    DeviceData.SetUnicode(tagIndex, strVal, stat);
                }
                else if (val is DateTime dtVal)
                {
                    deviceTag.DataType = TagDataType.Double;
                    deviceTag.Format = TagFormat.DateTime;
                    DeviceData.SetDateTime(tagIndex, dtVal, stat);
                }
                else
                {
                    deviceTag.DataType = TagDataType.Double;
                    deviceTag.Format = TagFormat.FloatNumber;
                    DeviceData.Set(tagIndex, Convert.ToDouble(val), stat);
                }
            }
            catch (Exception ex)
            {
                Log.WriteInfo(ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при установке данных тега" :
                    "Error setting tag data"));
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
        /// Writes an item value to the OPC server.
        /// </summary>
        private bool WriteItemValue(CommandConfig commandConfig, double cmdVal, string cmdData)
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

            WriteValue valueToWrite = new WriteValue
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

        /// <summary>
        /// Calls a method of the OPC server.
        /// </summary>
        private bool CallMethod(CommandConfig commandConfig, string cmdData)
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

        /// <summary>
        /// Gets OPC method arguments from command data.
        /// </summary>
        private object[] GetMethodArgs(string cmdData)
        {
            if (string.IsNullOrEmpty(cmdData))
                return Array.Empty<object>();

            // each line contains argument type and value, for example
            // double: 1.2
            List<object> args = new List<object>();
            string[] lines = cmdData.Split('\n');

            foreach (string line in lines)
            {
                int colonIdx = line.IndexOf(':');

                if (colonIdx >= 0)
                {
                    string typeName = line.Substring(0, colonIdx);
                    string argVal = line.Substring(colonIdx + 1);

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
            opcDeviceConfig = new OpcDeviceConfig();

            if (opcDeviceConfig.Load(Storage, OpcDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                InitCmdMaps();
            }
            else
            {
                opcDeviceConfig = null;
                Log.WriteLine(errMsg);
                Log.WriteLine(Locale.IsRussian ?
                    "Взаимодействие с OPC-сервером невозможно, т.к. конфигурация устройства не загружена" :
                    "Interaction with OPC server is impossible because device configuration is not loaded");
            }
        }

        /// <summary>
        /// Performs actions when terminating a communication line.
        /// </summary>
        public override void OnCommLineTerminate()
        {
            opcSession?.Close();
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            if (opcDeviceConfig == null)
                return;

            foreach (SubscriptionConfig subscriptionConfig in opcDeviceConfig.Subscriptions)
            {
                TagGroup tagGroup = new TagGroup(subscriptionConfig.DisplayName);

                foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                {
                    DeviceTag deviceTag = tagGroup.AddTag(itemConfig.TagCode, itemConfig.DisplayName);
                    deviceTag.Aux = new DeviceTagMeta();
                    itemConfig.Tag = deviceTag;

                    if (itemConfig.IsString)
                    {
                        deviceTag.DataLen = DriverUtils.GetTagDataLength(itemConfig.DataLength);
                        deviceTag.DataType = TagDataType.Unicode;
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
            if (opcDeviceConfig == null)
            {
                LastRequestOK = false;
                DeviceStatus = DeviceStatus.Error;
            }
            else if (!connected)
            {
                base.Session();

                // delay before connecting
                DateTime utcNow = DateTime.UtcNow;
                TimeSpan connectDelay = ReconnectDelay - (utcNow - connAttemptDT);

                if (connectDelay > TimeSpan.Zero)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Задержка перед соединением {0} с" :
                        "Delay before connecting {0} sec", 
                        connectDelay.TotalSeconds.ToString("N1"));
                    Thread.Sleep(connectDelay);
                }

                // connect to OPC server and create subscriptions
                connAttemptDT = DateTime.UtcNow;
                ConnectToOpcServer();
                DeviceStatus = connected && CreateSubscriptions() ? DeviceStatus.Normal : DeviceStatus.Error;
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

                if (!connected)
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
                    try
                    {
                        LastRequestOK = commandConfig.IsMethod
                            ? CallMethod(commandConfig, cmd.GetCmdDataString())
                            : WriteItemValue(commandConfig, cmd.CmdVal, cmd.GetCmdDataString());
                    }
                    catch (ScadaException ex)
                    {
                        Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                    }
                }

                FinishCommand();
            }
            finally
            {
                Monitor.Exit(opcLock);
            }
        }
    }
}
