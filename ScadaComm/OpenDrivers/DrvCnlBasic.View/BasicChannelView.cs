// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvCnlBasic.View.Forms;
using Scada.Forms;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvCnlBasic.View
{
    /// <summary>
    /// Implements the communication channel user interface.
    /// <para>Реализует пользовательский интерфейс канала связи.</para>
    /// </summary>
    internal class BasicChannelView : ChannelView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicChannelView(DriverView parentView, ChannelConfig channelConfig)
            : base(parentView, channelConfig)
        {
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing communication channel properties.
        /// </summary>
        public override bool ShowProperties()
        {
            Form form = ChannelConfig.TypeCode switch
            {
                ChannelTypeCode.SerialPort => new FrmSerialPortChannelOptions(ChannelConfig),
                ChannelTypeCode.TcpClient => new FrmTcpClientChannelOptions(ChannelConfig),
                ChannelTypeCode.TcpServer => new FrmTcpServerChannelOptions(ChannelConfig),
                ChannelTypeCode.Udp => new FrmUdpChannelOptions(ChannelConfig),
                _ => null
            };

            if (form == null)
            {
                ScadaUiUtils.ShowError(DriverPhrases.ChannelTypeNotFound);
                return false;
            }
            else
            {
                return form.ShowDialog() == DialogResult.OK;
            }
        }
    }
}
