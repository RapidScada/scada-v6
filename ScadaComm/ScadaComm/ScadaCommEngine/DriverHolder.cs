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
 * Module   : ScadaCommEngine
 * Summary  : Holds drivers used and helps to call their methods
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Drivers;
using Scada.Log;
using System;
using System.Collections.Generic;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Holds drivers used and helps to call their methods.
    /// <para>Содержит используемые драйверы и помогает вызывать их методы.</para>
    /// </summary>
    internal class DriverHolder
    {
        private readonly ILog log;                  // the application log
        private readonly List<DriverLogic> drivers; // the drivers used
        private readonly Dictionary<string, DriverLogic> driverMap; // the drivers accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DriverHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            drivers = new List<DriverLogic>();
            driverMap = new Dictionary<string, DriverLogic>();
        }


        /// <summary>
        /// Adds the specified driver to the lists.
        /// </summary>
        public void AddDriver(DriverLogic driverLogic)
        {
            if (driverLogic == null)
                throw new ArgumentNullException(nameof(driverLogic));

            drivers.Add(driverLogic);
            driverMap[driverLogic.Code] = driverLogic;
        }

        /// <summary>
        /// Gets the driver by code.
        /// </summary>
        public bool GetDriver(string driverCode, out DriverLogic driverLogic)
        {
            return driverMap.TryGetValue(driverCode, out driverLogic);
        }

        /// <summary>
        /// Calls the OnServiceStart method of the drivers.
        /// </summary>
        public void OnServiceStart()
        {
            foreach (DriverLogic driverLogic in drivers)
            {
                try
                {
                    driverLogic.OnServiceStart();
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, CommPhrases.ErrorInDriver, nameof(OnServiceStart), driverLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the OnServiceStop method of the drivers.
        /// </summary>
        public void OnServiceStop()
        {
            foreach (DriverLogic driverLogic in drivers)
            {
                try
                {
                    driverLogic.OnServiceStop();
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, CommPhrases.ErrorInDriver, nameof(OnServiceStop), driverLogic.Code);
                }
            }
        }
    }
}
