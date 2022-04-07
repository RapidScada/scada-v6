// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
using System;

namespace Scada.Comm.Drivers.DrvSimulator.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevSimulatorLogic : DeviceLogic
    {
        /// <summary>
        /// The period of sine waves in minutes.
        /// </summary>
        private const int SinePeriod = 60;
        /// <summary>
        /// The period of square waves in minutes.
        /// </summary>
        private const int SquarePeriod = 15;
        /// <summary>
        /// The period of triangular waves in minutes.
        /// </summary>
        private const int TrianglePeriod = 30;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private readonly Random Random = new Random();
        /// <summary>
        /// The array containing random values.
        /// </summary>
        private double[] randomArray = null;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSimulatorLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;
            ConnectionRequired = false;
        }


        /// <summary>
        /// Simulates reading input values.
        /// </summary>
        private void SimulateInputs()
        {
            double Frac(double d)
            {
                return d - Math.Truncate(d);
            }

            double x = DateTime.UtcNow.TimeOfDay.TotalMinutes;
            double y1 = Math.Sin(2 * Math.PI * x / SinePeriod);
            double y2 = Frac(x / SquarePeriod) <= 0.5 ? 1 : 0;
            double y3 = Frac(x / TrianglePeriod) <= 0.5 ? x % TrianglePeriod : TrianglePeriod - x % TrianglePeriod;

            Log.WriteLine(DeviceTags[TagCode.Sin].Name + " = " + y1);
            Log.WriteLine(DeviceTags[TagCode.Sqr].Name + " = " + y2);
            Log.WriteLine(DeviceTags[TagCode.Tri].Name + " = " + y3);

            DeviceData.Set(TagCode.Sin, y1);
            DeviceData.Set(TagCode.Sqr, y2);
            DeviceData.Set(TagCode.Tri, y3);
        }

        /// <summary>
        /// Simulates reading array.
        /// </summary>
        private void SimulateArray()
        {
            randomArray = randomArray ?? new double[DeviceTags[TagCode.RA].DataLength];

            for (int i = 0; i < randomArray.Length; i++)
            {
                randomArray[i] = Random.NextDouble() * 10;
            }

            DeviceData.SetDoubleArray(TagCode.RA, randomArray, CnlStatusID.Defined);
        }


        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            foreach (CnlPrototypeGroup group in CnlPrototypeFactory.GetCnlPrototypeGroups())
            {
                DeviceTags.AddGroup(group.ToTagGroup());
            }
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();
            SimulateInputs();
            SimulateArray();
            FinishRequest();
            FinishSession();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {
            base.SendCommand(cmd);

            if (cmd.CmdCode == TagCode.DO || cmd.CmdNum == 4)
            {
                double relayVal = cmd.CmdVal > 0 ? 1 : 0;
                Log.WriteLine(Locale.IsRussian ?
                    "Установить состояние реле в {0}" :
                    "Set the relay state to {0}", relayVal);
                DeviceData.Set(TagCode.DO, relayVal);
            }
            else if (cmd.CmdCode == TagCode.AO || cmd.CmdNum == 5)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Установить аналоговый выход в {0}" :
                    "Set the analog output to {0}", cmd.CmdVal);
                DeviceData.Set(TagCode.AO, cmd.CmdVal);
            }
            else if (cmd.CmdCode == "Hist")
            {
                // demonstrate how to create a historical data slice
                DateTime now = DateTime.UtcNow;
                DeviceSlice deviceSlice = new DeviceSlice(
                    new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc),
                    1, 1);
                deviceSlice.DeviceTags[0] = DeviceTags[TagCode.Sin];
                deviceSlice.CnlData[0] = new CnlData(cmd.CmdVal, CnlStatusID.Defined);
                deviceSlice.Descr = "Demo slice";
                DeviceData.EnqueueSlice(deviceSlice);
            }
            else if (cmd.CmdCode == "Event")
            {
                // demonstrate how to create an event
                DeviceData.EnqueueEvent(new DeviceEvent(DeviceTags[TagCode.Sin])
                {
                    Timestamp = DateTime.UtcNow,
                    CnlVal = cmd.CmdVal,
                    CnlStat = CnlStatusID.Defined,
                    Descr = "Demo event"
                });
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
