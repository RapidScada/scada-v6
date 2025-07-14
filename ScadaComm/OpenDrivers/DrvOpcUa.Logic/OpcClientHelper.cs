// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Lang;
using Scada.Log;
using Scada.Storages;

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


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcClientHelper(OpcConnectionOptions connectionOptions, ILog log, IStorage storage)
            : base(connectionOptions, log)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            subscrTags = [];

            connAttemptDT = DateTime.MinValue;
            reconnectHandler = null;
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
        private void OpcSession_KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            if (e.Status != null && ServiceResult.IsNotGood(e.Status))
            {
                log.WriteLine();
                log.WriteAction(Locale.IsRussian ?
                    "Статус сессии: {0}" :
                    "Session status: {0}", e.Status);

                if (reconnectHandler == null)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Переподключение к OPC-серверу" :
                        "Reconnecting to OPC server");
                    reconnectHandler = new SessionReconnectHandler();
                    reconnectHandler.BeginReconnect(session, ReconnectPeriod, OpcSession_ReconnectComplete);
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
                return;

            log.WriteLine();
            log.WriteAction(Locale.IsRussian ?
                "Переподключено" :
                "Reconnected");

            OpcSession = reconnectHandler.Session;
            reconnectHandler.Dispose();
            reconnectHandler = null;

            // after reconnecting, the subscriptions are automatically recreated, but with the wrong IDs and names,
            // so it's needed to clear them and create again
            ClearSubscriptions();
            CreateSubscriptions(false);
        }

        /// <summary>
        /// Processes new data received from the OPC server.
        /// </summary>
        private void OpcSession_Notification(ISession session, NotificationEventArgs e)
        {
            if (e.Subscription?.Handle is SubscriptionTag subscriptionTag)
            {
                subscriptionTag.DeviceLogic.ProcessDataChanges(subscriptionTag, e.NotificationMessage);
            }
            else
            {
                log.WriteError(Locale.IsRussian ?
                    "Получены данные по неизвестной подписке" :
                    "Received data for unknown subscription");
            }
        }

        /// <summary>
        /// Clears all subscriptions of the OPC session.
        /// </summary>
        private void ClearSubscriptions()
        {
            try
            {
                OpcSession.RemoveSubscriptions(OpcSession.Subscriptions.ToArray());
            }
            catch (Exception ex)
            {
                log.WriteLine(ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при очистке подписок" :
                    "Error clearing subscriptions"));
            }
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
                log.WriteLine(ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при записи конфигурации OPC" :
                    "Error writing OPC configuration"));
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
                    log.WriteError(ex.BuildErrorMessage(Locale.IsRussian ?
                        "Ошибка при отключении от OPC-сервера" :
                        "Error disconnecting OPC server"));
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
                    subscrTags.Add(new SubscriptionTag()
                    {
                        DeviceLogic = deviceLogic,
                        SubscriptionConfig = subscriptionConfig
                    });
                }
            }
        }

        /// <summary>
        /// Creates the previously added subscriptions on the OPC server.
        /// </summary>
        public bool CreateSubscriptions(bool detailedLog)
        {
            try
            {
                log.WriteLine();
                log.WriteAction(Locale.IsRussian ?
                    "Создание подписок" :
                    "Create subscriptions");

                if (OpcSession == null)
                {
                    throw new InvalidOperationException(Locale.IsRussian ?
                        "OPC-сессия не определена." :
                        "OPC session is undefined.");
                }

                foreach (SubscriptionTag subscriptionTag in subscrTags)
                {
                    SubscriptionConfig subscriptionConfig = subscriptionTag.SubscriptionConfig;

                    if (detailedLog)
                    {
                        log.WriteLine(Locale.IsRussian ?
                            "Создание подписки \"{0}\" для устройства {1}" :
                            "Create subscription \"{0}\" for the device {1}",
                            subscriptionConfig.DisplayName, subscriptionTag.DeviceLogic.Title);
                    }

                    Subscription subscription = new(OpcSession.DefaultSubscription)
                    {
                        DisplayName = subscriptionConfig.DisplayName,
                        PublishingInterval = subscriptionConfig.PublishingInterval,
                        Handle = subscriptionTag
                    };

                    foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                    {
                        if (itemConfig.Active)
                        {
                            subscription.AddItem(new MonitoredItem(subscription.DefaultItem)
                            {
                                StartNodeId = itemConfig.NodeID,
                                DisplayName = itemConfig.DisplayName,
                                Handle = new ItemTag
                                {
                                    DeviceTag = itemConfig.Tag as DeviceTag,
                                    ItemConfig = itemConfig
                                }
                            });
                        }
                    }

                    subscriptionTag.Subscription = subscription;
                    OpcSession.AddSubscription(subscription);
                    subscription.Create();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteLine(ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при создании подписок" :
                    "Error creating subscriptions"));
                return false;
            }
        }
    }
}
