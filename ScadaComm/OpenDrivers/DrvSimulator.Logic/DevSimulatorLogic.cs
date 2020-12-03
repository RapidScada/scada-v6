/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : DrvSimulator
 * Summary  : Implements the device logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2020
 */

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Data.Models;
using System;
using System.Threading;

namespace Scada.Comm.Drivers.DrvSimulator.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику КП.</para>
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
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSimulatorLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;
            InitDeviceTags();
        }


        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        private void InitDeviceTags()
        {
            TagGroup tagGroup = new TagGroup("Inputs");
            DeviceTags.AddGroup(tagGroup);

            tagGroup = new TagGroup("Outputs");
            DeviceTags.AddGroup(tagGroup);

            /*List<TagGroup> tagGroups = new List<TagGroup>();

            TagGroup tagGroup = new TagGroup("Inputs");
            tagGroup.KPTags.Add(new KPTag(1, "Sine"));
            tagGroup.KPTags.Add(new KPTag(2, "Square"));
            tagGroup.KPTags.Add(new KPTag(3, "Triangle"));
            tagGroups.Add(tagGroup);

            tagGroup = new TagGroup("Outputs");
            tagGroup.KPTags.Add(new KPTag(4, "Relay State"));
            tagGroup.KPTags.Add(new KPTag(5, "Analog Output"));
            tagGroups.Add(tagGroup);

            InitKPTags(tagGroups);*/
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

            double x = DateTime.Now.TimeOfDay.TotalMinutes;
            double y1 = Math.Sin(2 * Math.PI * x / SinePeriod);
            double y2 = Frac(x / SquarePeriod) <= 0.5 ? 1 : 0;
            double y3 = Frac(x / TrianglePeriod) <= 0.5 ? x % TrianglePeriod : TrianglePeriod - x % TrianglePeriod;

            /*Log.WriteLine(KPTags[0].Name + " = " + y1);
            Log.WriteLine(KPTags[1].Name + " = " + y2);
            Log.WriteLine(KPTags[2].Name + " = " + y3);

            SetCurData(0, y1, BaseValues.CnlStatuses.Defined);
            SetCurData(1, y2, BaseValues.CnlStatuses.Defined);
            SetCurData(2, y3, BaseValues.CnlStatuses.Defined);*/
        }


        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();
            SimulateInputs();
            Thread.Sleep(PollingOptions.Delay);
            //CalcSessStats();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {

        }
    }
}
