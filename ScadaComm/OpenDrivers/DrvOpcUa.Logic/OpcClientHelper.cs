// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Lang;
using Scada.Log;
using Scada.Storages;
using System.Reflection;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// Provides helper methods for using OPC client.
    /// <para>Предоставляет вспомогательные методы для клиента OPC.</para>
    /// </summary>
    internal class OpcClientHelper : OpcClientHelperBase
    {
        /// <summary>
        /// The OPC configuration file name for the logic runtime.
        /// </summary>
        private const string LogicOpcConfig = "DrvOpcUa.Logic.xml";
        /// <summary>
        /// The period of reconnecting to OPC server if a connection lost, ms.
        /// </summary>
        private const int ReconnectPeriod = 10000;
        /// <summary>
        /// The delay before reconnect.
        /// </summary>
        private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(5);

        private readonly IStorage storage;                    // the application storage
        private readonly List<SubscriptionTag> subscrTags;    // metadata about subscriptions

        private DateTime connAttemptDT;                       // the timestamp of a connection attempt
        private SessionReconnectHandler reconnectHandler;     // the object needed to reconnect
        private Dictionary<uint, SubscriptionTag> subscrByID; // the subscription tags accessed by IDs


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcClientHelper(OpcConnectionOptions connectionOptions, ILog log, IStorage storage)
            : base(connectionOptions, log)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            subscrTags = new List<SubscriptionTag>();

            connAttemptDT = DateTime.MinValue;
            reconnectHandler = null;
            subscrByID = null;
        }


        /// <summary>
        /// Performs a delay before connecting.
        /// </summary>
        private void DelayConnection()
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeSpan connectDelay = ReconnectDelay - (utcNow - connAttemptDT);

            if (connectDelay > TimeSpan.Zero)
            {
                log.WriteAction(Locale.IsRussian ?
                    "Задержка перед соединением {0} с" :
                    "Delay before connecting {0} sec",
                    connectDelay.TotalSeconds.ToString("N1"));
                Thread.Sleep(connectDelay);
            }

            connAttemptDT = DateTime.UtcNow;
        }

        /// <summary>
        /// Reconnects if needed.
        /// </summary>
        private void OpcSession_KeepAlive(Session sender, KeepAliveEventArgs e)
        {
            if (e.Status != null && ServiceResult.IsNotGood(e.Status))
            {
                log.WriteLine("{0} {1}/{2}", e.Status, sender.OutstandingRequestCount, sender.DefunctRequestCount);

                if (reconnectHandler == null)
                {
                    //DeviceData.Invalidate();
                    //DeviceStatus = DeviceStatus.Error;
                    log.WriteLine(Locale.IsRussian ?
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

            OpcSession = reconnectHandler.Session;
            reconnectHandler.Dispose();
            reconnectHandler = null;

            // after reconnecting, the subscriptions are automatically recreated, but with the wrong IDs and names,
            // so it's needed to clear them and create again
            //ClearSubscriptions();
            //DeviceStatus = CreateSubscriptions() ? DeviceStatus.Normal : DeviceStatus.Error;
            log.WriteLine(Locale.IsRussian ?
                "Переподключено" :
                "Reconnected");
        }

        /// <summary>
        /// Processes new data received from the OPC server.
        /// </summary>
        private void OpcSession_Notification(Session session, NotificationEventArgs e)
        {
            /*try
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
            }*/
        }

        /// <summary>
        /// Clears all subscriptions of the OPC session.
        /// </summary>
        private void ClearSubscriptions()
        {
            /*try
            {
                subscrByID = null;
                opcSession.RemoveSubscriptions(new List<Subscription>(opcSession.Subscriptions));
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при очистке подписок" :
                    "Error clearing subscriptions");
            }*/
        }

        /// <summary>
        /// Writes the OPC configuration.
        /// </summary>
        private void WriteConfiguration(Stream stream)
        {
            try
            {
                BinaryReader reader = new(stream); // do not close reader
                byte[] bytes = reader.ReadBytes((int)stream.Length);
                storage.WriteBytes(DataCategory.Config, LogicOpcConfig, bytes);
            }
            catch (Exception ex)
            {
                log.WriteLine(Locale.IsRussian ?
                    "Ошибка при записи конфигурации OPC: {0}" :
                    "Error writing OPC configuration: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Reads the OPC configuration.
        /// </summary>
        protected override Stream ReadConfiguration()
        {
            if (storage.GetFileInfo(DataCategory.Config, LogicOpcConfig).Exists)
            {
                byte[] bytes = storage.ReadBytes(DataCategory.Config, LogicOpcConfig);
                return new MemoryStream(bytes);
            }
            else
            {
                Stream resourceStream = GetConfigResourceStream();
                WriteConfiguration(resourceStream);
                resourceStream.Position = 0;
                return resourceStream;
            }
        }


        /// <summary>
        /// Connects to the OPC server.
        /// </summary>
        public bool Connect()
        {
            try
            {
                log.WriteLine();
                DelayConnection();

                if (string.IsNullOrEmpty(connectionOptions.ServerUrl))
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Адрес сервера не определён." :
                        "Server URL is undefined.");
                }

                log.WriteAction(Locale.IsRussian ?
                    "Соединение с {0}" :
                    "Connect to {0}",
                    connectionOptions.ServerUrl);

                ConnectAsync().Wait();
                OpcSession.KeepAlive += OpcSession_KeepAlive;
                OpcSession.Notification += OpcSession_Notification;
                return true;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при соединении с OPC-сервером" :
                    "Error connecting OPC server");
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the OPC server.
        /// </summary>
        public void Disconnect()
        {
            if (OpcSession != null)
            {
                log.WriteLine();

                try
                {
                    if (OpcSession.Connected)
                    {
                        log.WriteAction(Locale.IsRussian ?
                            "Отключение от OPC-сервера" :
                            "Disconnect from OPC server");
                        OpcSession.Close();
                    }
                }
                catch (Exception ex)
                {
                    log.WriteError(Locale.IsRussian ?
                        "Ошибка при отключении от OPC-сервера: {0}" :
                        "Error disconnecting OPC server: {0}", ex.Message);
                }

                OpcSession = null;
            }
        }

        /// <summary>
        /// Adds subscribtions according to the device configuration.
        /// </summary>
        public void AddSubscriptions(DevOpcUaLogic deviceLogic, OpcDeviceConfig deviceConfig)
        {
            foreach (SubscriptionConfig subscriptionConfig in deviceConfig.Subscriptions)
            {
                if (subscriptionConfig.Active)
                {
                    SubscriptionTag subscriptionTag = new()
                    {
                        DeviceLogic = deviceLogic,
                        SubscriptionConfig = subscriptionConfig
                    };

                    foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                    {
                        if (itemConfig.Active && 
                            itemConfig.Tag is DeviceTag deviceTag)
                        {
                            subscriptionTag.ItemsByNodeID[itemConfig.NodeID] = new ItemTag
                            {
                                DeviceTag = deviceTag,
                                ItemConfig = itemConfig
                            };
                        }
                    }

                    subscrTags.Add(subscriptionTag);
                }
            }
        }

        /// <summary>
        /// Creates the previously added subscriptions on the OPC server.
        /// </summary>
        public bool CreateSubscriptions()
        {
            try
            {
                log.WriteLine();
                log.WriteAction(Locale.IsRussian ?
                    "Создание подписок" :
                    "Create subscriptions");

                if (OpcSession == null)
                    throw new InvalidOperationException("OPC session must not be null.");

                subscrByID = new Dictionary<uint, SubscriptionTag>();

                foreach (SubscriptionTag subscriptionTag in subscrTags)
                {
                    SubscriptionConfig subscriptionConfig = subscriptionTag.SubscriptionConfig;
                    log.WriteLine(Locale.IsRussian ?
                        "Создание подписки \"{0}\" для устройства {1}" :
                        "Create subscription \"{0}\" for the device {1}",
                        subscriptionConfig.DisplayName, subscriptionTag.DeviceLogic.Title);

                    Subscription subscription = new(OpcSession.DefaultSubscription)
                    {
                        DisplayName = subscriptionConfig.DisplayName,
                        PublishingInterval = subscriptionConfig.PublishingInterval
                    };

                    foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                    {
                        if (itemConfig.Active)
                        {
                            subscription.AddItem(new MonitoredItem(subscription.DefaultItem)
                            {
                                StartNodeId = itemConfig.NodeID,
                                DisplayName = itemConfig.DisplayName
                            });
                        }
                    }

                    OpcSession.AddSubscription(subscription);
                    subscription.Create();
                    subscrByID[subscription.Id] = subscriptionTag;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteLine(Locale.IsRussian ?
                    "Ошибка при создании подписок: {0}" :
                    "Error creating subscriptions: {0}", ex.ToString());
                return false;
            }
        }
    }
}
