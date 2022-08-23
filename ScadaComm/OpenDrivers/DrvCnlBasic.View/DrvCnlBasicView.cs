// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvCnlBasic.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvCnlBasicView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvCnlBasicView()
            : base()
        {
            CanCreateChannel = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? 
                    "Основные каналы связи" : 
                    "Basic Communication Channels";
            }
        }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Предоставляет каналы связи:\n" +
                    "последовательный порт,\n" +
                    "TCP-клиент,\n" +
                    "TCP-сервер,\n" +
                    "UDP." :

                    "Provides communication channels:\n" +
                    "Serial port,\n" +
                    "TCP client,\n" +
                    "TCP server,\n" +
                    "UDP.";
            }
        }

        /// <summary>
        /// Gets the communication channel types provided by the driver.
        /// </summary>
        public override ICollection<ChannelTypeName> ChannelTypes
        {
            get
            {
                return Locale.IsRussian ?
                    new ChannelTypeName[]
                    {
                        new ChannelTypeName(ChannelTypeCode.SerialPort, "Последовательный порт"),
                        new ChannelTypeName(ChannelTypeCode.TcpClient, "TCP-клиент"),
                        new ChannelTypeName(ChannelTypeCode.TcpServer, "TCP-сервер"),
                        new ChannelTypeName(ChannelTypeCode.Udp, "UDP")
                    } :

                    new ChannelTypeName[]
                    {
                        new ChannelTypeName(ChannelTypeCode.SerialPort, "Serial port"),
                        new ChannelTypeName(ChannelTypeCode.TcpClient, "TCP client"),
                        new ChannelTypeName(ChannelTypeCode.TcpServer, "TCP server"),
                        new ChannelTypeName(ChannelTypeCode.Udp, "UDP")
                    };
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            DriverPhrases.Init();
        }

        /// <summary>
        /// Creates a new communication channel user interface.
        /// </summary>
        public override ChannelView CreateChannelView(ChannelConfig channelConfig)
        {
            return new BasicChannelView(this, channelConfig);
        }
    }
}
