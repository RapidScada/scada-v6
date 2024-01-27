/*
 * Copyright 2024 Rapid Software LLC
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
 * Modified : 2023
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
        private readonly ILog log;                    // the application log
        private readonly List<DriverWrapper> drivers; // the drivers used
        private readonly Dictionary<string, DriverWrapper> driverMap; // the drivers accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DriverHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            drivers = new List<DriverWrapper>();
            driverMap = new Dictionary<string, DriverWrapper>();
        }


        /// <summary>
        /// Checks if a driver with the specified code exists.
        /// </summary>
        public bool DriverExists(string code)
        {
            return driverMap.ContainsKey(code);
        }

        /// <summary>
        /// Adds the specified driver to the lists.
        /// </summary>
        public DriverWrapper AddDriver(DriverLogic driverLogic)
        {
            if (driverLogic == null)
                throw new ArgumentNullException(nameof(driverLogic));

            if (driverMap.ContainsKey(driverLogic.Code))
                throw new ScadaException("Driver already exists.");

            DriverWrapper driverWrapper = new DriverWrapper(driverLogic, log);
            drivers.Add(driverWrapper);
            driverMap.Add(driverLogic.Code, driverWrapper);
            return driverWrapper;
        }

        /// <summary>
        /// Gets the driver wrapper by code.
        /// </summary>
        public bool GetDriver(string driverCode, out DriverWrapper driverWrapper)
        {
            return driverMap.TryGetValue(driverCode, out driverWrapper);
        }

        /// <summary>
        /// Calls the OnServiceStart method of the drivers.
        /// </summary>
        public void OnServiceStart()
        {
            drivers.ForEach(d => d.OnServiceStart());
        }

        /// <summary>
        /// Calls the OnServiceStop method of the drivers.
        /// </summary>
        public void OnServiceStop()
        {
            drivers.ForEach(d => d.OnServiceStop());
        }
    }
}
