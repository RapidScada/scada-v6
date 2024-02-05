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
 * Module   : ScadaCommon
 * Summary  : Defines functionality of an application data storage
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Tables;
using System.Collections.Generic;
using System.IO;

namespace Scada.Storages
{
    /// <summary>
    /// Defines functionality of an application data storage.
    /// <para>Определяет функциональность хранилища данных приложения.</para>
    /// </summary>
    /// <remarks>Storage implementations must be thread-safe.</remarks>
    public interface IStorage
    {
        /// <summary>
        /// Gets the current application.
        /// </summary>
        ServiceApp App { get; }

        /// <summary>
        /// Gets a value indicating whether the storage is ready for reading and writing.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Gets a value indicating whether the storage works with the file system.
        /// </summary>
        bool IsFileStorage { get; }

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
        /// Reads text from the file.
        /// </summary>
        string ReadText(DataCategory category, string path);

        /// <summary>
        /// Reads a byte array from the file.
        /// </summary>
        byte[] ReadBytes(DataCategory category, string path);

        /// <summary>
        /// Reads the table of the configuration database.
        /// </summary>
        /// <remarks>
        /// This method is only used by the Server service 
        /// that provides the configuration database to the other services.
        /// </remarks>
        void ReadBaseTable(IBaseTable baseTable);

        /// <summary>
        /// Writes the text to the file.
        /// </summary>
        void WriteText(DataCategory category, string path, string contents);

        /// <summary>
        /// Writes the byte array to the file.
        /// </summary>
        void WriteBytes(DataCategory category, string path, byte[] bytes);

        /// <summary>
        /// Opens a text file for reading.
        /// </summary>
        TextReader OpenText(DataCategory category, string path);

        /// <summary>
        /// Opens a binary file for reading.
        /// </summary>
        BinaryReader OpenBinary(DataCategory category, string path);

        /// <summary>
        /// Gets information associated with the file.
        /// </summary>
        ShortFileInfo GetFileInfo(DataCategory category, string path);

        /// <summary>
        /// Gets a list of file paths that match the specified pattern.
        /// </summary>
        ICollection<string> GetFileList(DataCategory category, string path, string searchPattern);
    }
}
