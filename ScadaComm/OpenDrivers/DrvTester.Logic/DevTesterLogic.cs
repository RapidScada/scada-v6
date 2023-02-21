// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;
using System;
using System.Diagnostics;

namespace Scada.Comm.Drivers.DrvTester.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevTesterLogic : DeviceLogic
    {
        private readonly TesterOptions options;          // the tester options
        private readonly byte[] inBuf;                   // the input buffer
        private readonly BinStopCondition binStopCond;   // the stop condition for reading binary data
        private readonly TextStopCondition textStopCond; // the stop condition for reading text


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevTesterLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            options = new TesterOptions(deviceConfig.PollingOptions.CustomOptions);
            inBuf = new byte[options.BufferLength];
            binStopCond = options.BinStopCode > 0 ? new BinStopCondition(options.BinStopCode) : null;
            textStopCond = string.IsNullOrEmpty(options.StopEnding) ? 
                TextStopCondition.OneLine : new TextStopCondition(options.StopEnding);

            CanSendCommands = true;
        }


        /// <summary>
        /// Performs actions after setting the connection.
        /// </summary>
        public override void OnConnectionSet()
        {
            if (Connection != null)
                Connection.NewLine = "\x0D";
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (options.ReadMode == ReadMode.Binary)
            {
                if (binStopCond == null)
                    Connection.Read(inBuf, 0, inBuf.Length, PollingOptions.Timeout);
                else
                    Connection.Read(inBuf, 0, inBuf.Length, PollingOptions.Timeout, binStopCond, out _);
            }
            else
            {
                Connection.ReadLines(PollingOptions.Timeout, textStopCond, out _);
            }

            stopwatch.Stop();
            Log.WriteLine(Locale.IsRussian ?
                "Получено за {0} мс" :
                "Received in {0} ms", stopwatch.ElapsedMilliseconds);

            FinishRequest();
            FinishSession();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {
            base.SendCommand(cmd);

            if (cmd.CmdCode == "SendStr" || cmd.CmdNum == 1)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                Connection.WriteLine(cmd.GetCmdDataString());

                stopwatch.Stop();
                Log.WriteLine(Locale.IsRussian ?
                    "Отправлено за {0} мс" :
                    "Sent in {0} ms", stopwatch.ElapsedMilliseconds);
                FinishRequest();
            }
            else if (cmd.CmdCode == "SendBin" || cmd.CmdNum == 2)
            {
                byte[] buffer = cmd.CmdData ?? Array.Empty<byte>();
                Stopwatch stopwatch = Stopwatch.StartNew();
                Connection.Write(buffer, 0, buffer.Length);

                stopwatch.Stop();
                Log.WriteLine(Locale.IsRussian ?
                    "Отправлено за {0} мс" :
                    "Sent in {0} ms", stopwatch.ElapsedMilliseconds);
                FinishRequest();
            }
            else
            {
                LastRequestOK = false;
                Log.WriteLine(CommPhrases.InvalidCommand);
            }

            FinishCommand();
        }
    }
}
