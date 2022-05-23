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
 * Module   : ScadaCommEngine
 * Summary  : Holds data sources used and helps to call their methods
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Holds data sources used and helps to call their methods.
    /// <para>Содержит источники данных и помогает вызывать их методы.</para>
    /// </summary>
    internal class DataSourceHolder
    {
        private readonly ILog log;                          // the application log
        private readonly List<DataSourceLogic> dataSources; // the active data sources
        private readonly Dictionary<string, DataSourceLogic> dataSourceMap; // the data sources accessed by code
        private int maxTitleLength;                         // the maximum length of data source title


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataSourceHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            dataSources = new List<DataSourceLogic>();
            dataSourceMap = new Dictionary<string, DataSourceLogic>();
            maxTitleLength = 0;
        }


        /// <summary>
        /// Gets the number of data sources.
        /// </summary>
        public int Count
        {
            get
            {
                return dataSources.Count;
            }
        }


        /// <summary>
        /// Checks if a data source with the specified code exists.
        /// </summary>
        public bool DataSourceExists(string code)
        {
            return dataSourceMap.ContainsKey(code);
        }

        /// <summary>
        /// Adds the specified data source to the lists.
        /// </summary>
        public void AddDataSource(DataSourceLogic dataSourceLogic)
        {
            if (dataSourceLogic == null)
                throw new ArgumentNullException(nameof(dataSourceLogic));

            if (dataSourceMap.ContainsKey(dataSourceLogic.Code))
                throw new ScadaException("Data source already exists.");

            dataSources.Add(dataSourceLogic);
            dataSourceMap.Add(dataSourceLogic.Code, dataSourceLogic);

            if (maxTitleLength < dataSourceLogic.Title.Length)
                maxTitleLength = dataSourceLogic.Title.Length;
        }

        /// <summary>
        /// Appends information about the data sources to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sb)
        {
            string header = Locale.IsRussian ?
                $"Источники данных ({dataSources.Count})" :
                $"Data Sources ({dataSources.Count})";

            sb
                .AppendLine()
                .AppendLine(header)
                .Append('-', header.Length).AppendLine();

            if (dataSources.Count > 0)
            {
                foreach (DataSourceLogic dataSourceLogic in dataSources)
                {
                    sb
                        .Append(dataSourceLogic.Title)
                        .Append(' ', maxTitleLength - dataSourceLogic.Title.Length)
                        .Append(" : ")
                        .AppendLine(dataSourceLogic.StatusText);
                }

                foreach (DataSourceLogic dataSourceLogic in dataSources)
                {
                    dataSourceLogic.AppendInfo(sb);
                }
            }
            else
            {
                sb.AppendLine(Locale.IsRussian ? "Источников данных нет" : "No data sources");
            }
        }

        /// <summary>
        /// Calls the MakeReady method of the data sources.
        /// </summary>
        public void MakeReady()
        {
            foreach (DataSourceLogic dataSourceLogic in dataSources)
            {
                try
                {
                    dataSourceLogic.MakeReady();
                    dataSourceLogic.IsReady = true;
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, CommPhrases.ErrorInDataSource, nameof(MakeReady), dataSourceLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the Start method of the data sources.
        /// </summary>
        public void Start()
        {
            foreach (DataSourceLogic dataSourceLogic in dataSources)
            {
                try
                {
                    if (dataSourceLogic.IsReady)
                        dataSourceLogic.Start();
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, CommPhrases.ErrorInDataSource, nameof(Start), dataSourceLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the Close method of the data sources.
        /// </summary>
        public void Close()
        {
            foreach (DataSourceLogic dataSourceLogic in dataSources)
            {
                try
                {
                    dataSourceLogic.Close();
                    dataSourceLogic.IsReady = false;
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, CommPhrases.ErrorInDataSource, nameof(Close), dataSourceLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the ReadConfigDatabase method of the data sources.
        /// </summary>
        public bool ReadConfigDatabase(out ConfigDatabase configDatabase)
        {
            foreach (DataSourceLogic dataSourceLogic in dataSources)
            {
                try
                {
                    if (dataSourceLogic.IsReady)
                    {
                        if (dataSourceLogic.ReadConfigDatabase(out configDatabase))
                            return true;
                    }
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, CommPhrases.ErrorInDataSource, 
                        nameof(ReadConfigDatabase), dataSourceLogic.Code);
                }
            }

            configDatabase = null;
            return false;
        }

        /// <summary>
        /// Calls the WriteCurrentData method of the data sources.
        /// </summary>
        public void WriteCurrentData(DeviceSlice deviceSlice)
        {
            foreach (DataSourceLogic dataSourceLogic in dataSources)
            {
                try
                {
                    if (dataSourceLogic.IsReady)
                        dataSourceLogic.WriteCurrentData(deviceSlice);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, CommPhrases.ErrorInDataSource, 
                        nameof(WriteCurrentData), dataSourceLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the WriteHistoricalData method of the data sources.
        /// </summary>
        public void WriteHistoricalData(DeviceSlice deviceSlice)
        {
            foreach (DataSourceLogic dataSourceLogic in dataSources)
            {
                try
                {
                    if (dataSourceLogic.IsReady)
                        dataSourceLogic.WriteHistoricalData(deviceSlice);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, CommPhrases.ErrorInDataSource, 
                        nameof(WriteHistoricalData), dataSourceLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the WriteEvent method of the data sources.
        /// </summary>
        public void WriteEvent(DeviceEvent deviceEvent)
        {
            foreach (DataSourceLogic dataSourceLogic in dataSources)
            {
                try
                {
                    if (dataSourceLogic.IsReady)
                        dataSourceLogic.WriteEvent(deviceEvent);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, CommPhrases.ErrorInDataSource, nameof(WriteEvent), dataSourceLogic.Code);
                }
            }
        }
    }
}
