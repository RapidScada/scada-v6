// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
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

        private readonly IStorage storage;                // the application storage
        private DateTime connAttemptDT;                   // the timestamp of a connection attempt
        private SessionReconnectHandler reconnectHandler; // the object needed to reconnect


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcClientHelper(OpcConnectionOptions connectionOptions, ILog log, IStorage storage)
            : base(connectionOptions, log)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
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
        private void OpcSession_KeepAlive(Session sender, KeepAliveEventArgs e)
        {
            if (e.Status != null && ServiceResult.IsNotGood(e.Status))
            {
                log.WriteLine("{0} {1}/{2}", e.Status, sender.OutstandingRequestCount, sender.DefunctRequestCount);

                if (reconnectHandler == null)
                {
                    log.WriteLine(Locale.IsRussian ?
                        "Переподключение к OPC-серверу" :
                        "Reconnecting to OPC server");
                    // TODO: invalidate devices
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
                string resourceName = GetConfigResourceName();
                Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ??
                    throw new ScadaException(string.Format(Locale.IsRussian ?
                        "Ресурс {0} не найден." :
                        "Resource {0} not found.", resourceName));

                using BinaryReader reader = new(resourceStream);
                byte[] bytes = reader.ReadBytes((int)resourceStream.Length);
                storage.WriteBytes(DataCategory.Config, LogicOpcConfig, bytes);

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
    }
}
