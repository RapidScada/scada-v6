// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Config;
using System.IO.Ports;

namespace Scada.Comm.Drivers.DrvCnlBasic
{
    /// <summary>
    /// Represents options of a serial port channel.
    /// <para>Представляет параметры канала последовательного порта.</para>
    /// </summary>
    public class SerialChannelOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SerialChannelOptions(OptionList options)
        {
            PortName = options.GetValueAsString("PortName", "COM1");
            BaudRate = options.GetValueAsInt("BaudRate", 9600);
            DataBits = options.GetValueAsInt("DataBits", 8);
            Parity = options.GetValueAsEnum("Parity", Parity.None);
            StopBits = options.GetValueAsEnum("StopBits", StopBits.One);
            DtrEnable = options.GetValueAsBool("DtrEnable", false);
            RtsEnable = options.GetValueAsBool("RtsEnable", false);
            Behavior = options.GetValueAsEnum("Behavior", ChannelBehavior.Master);
        }


        /// <summary>
        /// Gets or sets the serial port name.
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the port baud rate.
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// Gets or sets the port parity.
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the standard number of stopbits per byte.
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal.
        /// </summary>
        public bool DtrEnable { get; set; }

        /// <summary>
        /// Gets or sets a value that enables the Request to Send (RTS) signal.
        /// </summary>
        public bool RtsEnable { get; set; }

        /// <summary>
        /// Gets or sets the channel behavior.
        /// </summary>
        public ChannelBehavior Behavior { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["PortName"] = PortName;
            options["BaudRate"] = BaudRate.ToString();
            options["DataBits"] = DataBits.ToString();
            options["Parity"] = Parity.ToString();
            options["StopBits"] = StopBits.ToString();
            options["DtrEnable"] = DtrEnable.ToString().ToLowerInvariant();
            options["RtsEnable"] = RtsEnable.ToString().ToLowerInvariant();
            options["Behavior"] = Behavior.ToString();
        }
    }
}
