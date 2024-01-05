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
 * Module   : ScadaCommCommon
 * Summary  : Represents the base class for data source user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Comm.Config;
using Scada.Comm.Drivers;
using System;

namespace Scada.Comm.DataSources
{
    /// <summary>
    /// Represents the base class for data source user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса источника данных.</para>
    /// </summary>
    public abstract class DataSourceView : LibraryView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataSourceView(DriverView parentView, DataSourceConfig dataSourceConfig)
            : base(parentView)
        {
            AppConfig = parentView.AppConfig;
            DataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <remarks>Do not modify the configuration.</remarks>
        public CommConfig AppConfig { get; }

        /// <summary>
        /// Gets the data source configuration.
        /// </summary>
        public DataSourceConfig DataSourceConfig { get; }
    }
}
