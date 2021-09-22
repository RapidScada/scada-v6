/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaCommon
 * Summary  : Defines functionality of a data storage
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Tables;

namespace Scada.Storage
{
    /// <summary>
    /// Defines functionality of a data storage.
    /// <para>Определяет функциональность хранилища данных.</para>
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Gets current application.
        /// </summary>
        ServiceApp App { get; }

        /// <summary>
        /// Gets a value indicating whether a client application can load a view from the storage.
        /// </summary>
        /// <remarks>
        /// If true, loading views from the storage helps improve performance.
        /// If false, views should be downloaded from the Server service.
        /// The Server service always loads views from the storage regardless of the property value.
        /// </remarks>
        bool ViewAvailable { get; }


        /// <summary>
        /// Waits for the storage to be ready within a specified time interval (ms).
        /// </summary>
        bool Wait(int timeout);

        /// <summary>
        /// Reads text from the file.
        /// </summary>
        string ReadText(DataCategory category, string path);

        /// <summary>
        /// Reads the table of the configuration database.
        /// </summary>
        /// <remarks>This method is only used by the Server service 
        /// that provides the configuration database to the other services.</remarks>
        void ReadBaseTable(IBaseTable baseTable);

        /// <summary>
        /// Writes text to the file.
        /// </summary>
        void WriteText(DataCategory category, string path, string content);
    }
}
