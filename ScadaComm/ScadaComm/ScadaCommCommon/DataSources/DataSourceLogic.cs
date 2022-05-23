/*
 * Copyright 2022 Rapid Software LLC
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
 * Module   : ScadaCommCommon
 * Summary  : Represents the base class for data source logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using System;
using System.IO;
using System.Text;

namespace Scada.Comm.DataSources
{
    /// <summary>
    /// Represents the base class for data source logic.
    /// <para>Представляет базовый класс логики источника данных.</para>
    /// </summary>
    public abstract class DataSourceLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataSourceLogic(ICommContext commContext, DataSourceConfig dataSourceConfig)
        {
            CommContext = commContext ?? throw new ArgumentNullException(nameof(commContext));
            DataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
            Code = dataSourceConfig.Code;
            Title = CommUtils.GetDataSourceTitle(Code, dataSourceConfig.Name);
            IsReady = false;
        }


        /// <summary>
        /// Gets the application context.
        /// </summary>
        protected ICommContext CommContext { get; }

        /// <summary>
        /// Gets the data source configuration.
        /// </summary>
        protected DataSourceConfig DataSourceConfig { get; }

        /// <summary>
        /// Gets the data source code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the data source title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the data source is ready for operating.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets the current data source status as text.
        /// </summary>
        public virtual string StatusText
        {
            get
            {
                return Locale.IsRussian ?
                    (IsReady ? "готовность" : "не готов") :
                    (IsReady ? "Ready" : "Not Ready");
            }
        }


        /// <summary>
        /// Creates a log file of the data source.
        /// </summary>
        protected ILog CreateLog(string driverCode)
        {
            return new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(CommContext.AppDirs.LogDir, driverCode + "_" + Code + ".log"),
                CapacityMB = CommContext.AppConfig.GeneralOptions.MaxLogSize
            };
        }

        /// <summary>
        /// Makes the data source ready for operating.
        /// </summary>
        public virtual void MakeReady()
        {
        }

        /// <summary>
        /// Starts the data source.
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Closes the data source.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Reads the configuration database.
        /// </summary>
        public virtual bool ReadConfigDatabase(out ConfigDatabase configDatabase)
        {
            configDatabase = null;
            return false;
        }

        /// <summary>
        /// Writes the slice of the current data.
        /// </summary>
        public virtual void WriteCurrentData(DeviceSlice deviceSlice)
        {
        }

        /// <summary>
        /// Writes the slice of historical data.
        /// </summary>
        public virtual void WriteHistoricalData(DeviceSlice deviceSlice)
        {
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public virtual void WriteEvent(DeviceEvent deviceEvent)
        {
        }

        /// <summary>
        /// Appends information about the data source to the string builder.
        /// </summary>
        public virtual void AppendInfo(StringBuilder sb)
        {
        }
    }
}
