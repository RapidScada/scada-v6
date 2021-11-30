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
    /// <para>Реализует логику КП.</para>
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
        public class ItemTag
        {
            public ItemConfig ItemConfig { get; set; }
            public DeviceTag DeviceTag { get; set; }
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
        private bool autoAccept;                              // auto accept OPC server certificate
        private bool connected;                               // connection with OPC server is established
        private DateTime connAttemptDT;                       // the time stamp of a connection attempt
        private Session opcSession;                           // the OPC session
        private SessionReconnectHandler reconnectHandler;     // the object needed to reconnect
        private Dictionary<uint, SubscriptionTag> subscrByID; // the subscription tags accessed by IDs
        private Dictionary<int, CommandConfig> cmdByNum;      // the commands accessed by their numbers


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevOpcUaLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            opcLock = new object();
            opcDeviceConfig = null;
            autoAccept = true;
            connected = false;
            connAttemptDT = DateTime.MinValue;
            opcSession = null;
            reconnectHandler = null;
            subscrByID = null;
            cmdByNum = null;

            CanSendCommands = true;
            ConnectionRequired = false;
        }


        /// <summary>
        /// Connects to the OPC server.
        /// </summary>
        private void ConnectToOpcServer()
        {
            try
            {
                OpcHelper helper = new OpcHelper(AppDirs, Log, DeviceNum, OpcHelper.RuntimeKind.Logic)
                {
                    CertificateValidation = CertificateValidator_CertificateValidation
                };

                connected = helper.ConnectAsync(opcDeviceConfig.ConnectionOptions, PollingOptions.Timeout).Result;
                autoAccept = autoAccept || helper.AutoAccept; // TODO: autoAccept always true?
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
        /// Validates the certificate.
        /// </summary>
        private void CertificateValidator_CertificateValidation(CertificateValidator validator,
            CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                e.Accept = autoAccept;

                if (autoAccept)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Принятый сертификат: {0}" :
                        "Accepted certificate: {0}", e.Certificate.Subject);
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Отклоненный сертификат: {0}" :
                        "Rejected certificate: {0}", e.Certificate.Subject);
                }
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
                    Log.WriteLine(Locale.IsRussian ?
                        "{0} КП {1}. Обработка новых данных. Подписка: {2}" :
                        "{0} Device {1}. Process new data. Subscription: {2}",
                        LastSessionTime.ToLocalTime().ToLocalizedString(), DeviceNum, e.Subscription.DisplayName);
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
                MonitoredItem monitoredItem = subscriptionTag.Subscription.FindItemByClientHandle(change.ClientHandle);

                if (monitoredItem != null)
                {
                    if (subscriptionTag.ItemsByNodeID.TryGetValue(monitoredItem.StartNodeId.ToString(),
                        out ItemTag itemTag))
                    {
                        Log.WriteLine("{0} {1} = {2} ({3})", CommPhrases.ReceiveNotation, 
                            monitoredItem.DisplayName, change.Value, change.Value.StatusCode);

                        int tagIndex = itemTag.DeviceTag.Index;
                        int tagStatus = StatusCode.IsGood(change.Value.StatusCode) ? 
                            CnlStatusID.Defined : CnlStatusID.Undefined;

                        if (itemTag.ItemConfig.IsArray)
                        {
                            if (itemTag.ItemConfig.ArrayLen > 0 && change.Value.Value is Array valArr)
                            {
                                for (int i = 0, len = Math.Min(itemTag.ItemConfig.ArrayLen, valArr.Length); 
                                    i < len; i++)
                                {
                                    SetTagData(tagIndex++, valArr.GetValue(i), tagStatus);
                                }
                            }
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
        /// Sets value, status and format of the specified tag.
        /// </summary>
        private void SetTagData(int tagIndex, object val, int stat)
        {
            try
            {
                if (val is string strVal)
                {
                    DeviceData.SetUnicode(tagIndex, strVal, stat);
                    DeviceTags[tagIndex].Format = TagFormat.String;
                }
                else if (val is DateTime dtVal)
                {
                    DeviceData.SetDateTime(tagIndex, dtVal, stat);
                    DeviceTags[tagIndex].Format = TagFormat.DateTime;
                }
                else
                {
                    DeviceData.Set(tagIndex, Convert.ToDouble(val), stat);
                    DeviceTags[tagIndex].Format = TagFormat.FloatNumber;
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при установке данных тега" :
                    "Error setting tag data");
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            opcDeviceConfig = new OpcDeviceConfig();

            if (opcDeviceConfig.Load(OpcDeviceConfig.GetFileName(AppDirs.ConfigDir, DeviceNum), out string errMsg))
            {
                InitDeviceTags();

                // fill the command dictionary
                cmdByNum = new Dictionary<int, CommandConfig>();
                opcDeviceConfig.Commands.ForEach(c => cmdByNum[c.CmdNum] = c);
            }
            else
            {
                opcDeviceConfig = null;
                Log.WriteLine(errMsg);
                Log.WriteLine(Locale.IsRussian ?
                    "Взаимодействие с OPC-сервером невозможно, т.к. конфигурация КП не загружена" :
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
            foreach (SubscriptionConfig subscriptionConfig in opcDeviceConfig.Subscriptions)
            {
                TagGroup tagGroup = new TagGroup(subscriptionConfig.DisplayName);

                foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                {
                    DeviceTag deviceTag = tagGroup.AddTag(itemConfig.NodeID, itemConfig.DisplayName);
                    itemConfig.Tag = deviceTag;

                    if (itemConfig.IsArray)
                        deviceTag.DataLen = itemConfig.ArrayLen;
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

                // delay before connection
                DateTime utcNow = DateTime.UtcNow;
                TimeSpan connectionDelay = ReconnectDelay - (utcNow - connAttemptDT);

                if (connectionDelay > TimeSpan.Zero)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Задержка перед соединением {0} с" :
                        "Delay before connection {0} sec", 
                        connectionDelay.TotalSeconds.ToString("N1"));
                    Thread.Sleep(connectionDelay);
                }

                // connect to OPC server and create subscriptions
                connAttemptDT = DateTime.UtcNow;
                ConnectToOpcServer();
                DeviceStatus = connected && CreateSubscriptions() ? DeviceStatus.Normal : DeviceStatus.Error;
            }

            Thread.Sleep(PollingOptions.Delay);
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
                        "Невозможно отправить команду ТУ, т.к. соединение с OPC-сервером не установлено" :
                        "Unable to send command because connection with the OPC server is not established");
                }
                else if (!cmdByNum.TryGetValue(cmd.CmdNum, out CommandConfig commandConfig))
                {
                    Log.WriteLine(CommPhrases.InvalidCommand);
                }
                else
                {
                    // prepare value to write
                    string dataTypeName = commandConfig.DataTypeName;
                    Type itemType = Type.GetType(dataTypeName, false, true);
                    object itemVal;

                    if (itemType == null)
                    {
                        throw new ScadaException(string.Format(Locale.IsRussian ?
                            "Не удалось получить тип данных {0}" :
                            "Unable to get data type {0}", dataTypeName));
                    }

                    if (itemType.IsArray)
                    {
                        throw new ScadaException(string.Format(Locale.IsRussian ?
                            "Тип данных {0} не поддерживается" :
                            "Data type {0} not supported", dataTypeName));
                    }

                    try
                    {
                        itemVal = Convert.ChangeType(cmd.CmdVal, itemType);
                    }
                    catch
                    {
                        throw new ScadaException(string.Format(Locale.IsRussian ?
                            "Не удалось привести значение команды к типу {0}" :
                            "Unable to convert command value to the type {0}", itemType.FullName));
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
                        LastRequestOK = true;
                    }
                    else
                    {
                        Log.WriteLine(CommPhrases.ResponseError);
                    }
                }
            }
            finally
            {
                FinishCommand();
                Monitor.Exit(opcLock);
            }
        }
    }
}
