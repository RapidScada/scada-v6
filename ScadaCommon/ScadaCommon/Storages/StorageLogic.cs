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
 * Summary  : Represents the base class for storage logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Storages
{
    /// <summary>
    /// Represents the base class for storage logic.
    /// <para>Представляет базовый класс логики хранилища.</para>
    /// </summary>
    /// <remarks>Derived classes must be thread-safe.</remarks>
    public abstract class StorageLogic : IStorage
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StorageLogic(StorageContext storageContext)
        {
            StorageContext = storageContext ?? throw new ArgumentNullException(nameof(storageContext));
            IsReady = false;
            IsFileStorage = false;
            ViewAvailable = false;
        }


        /// <summary>
        /// Gets the storage context.
        /// </summary>
        protected StorageContext StorageContext { get; }

        /// <summary>
        /// Gets the current application.
        /// </summary>
        public ServiceApp App => StorageContext.App;

        /// <summary>
        /// Gets or sets a value indicating whether the storage is ready for reading and writing.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets a value indicating whether the storage works with the file system.
        /// </summary>
        public bool IsFileStorage { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether a client application can load a view from the storage.
        /// </summary>
        public bool ViewAvailable { get; protected set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public virtual void LoadConfig(XmlElement xmlElement)
        {
            if (xmlElement == null)
                throw new ArgumentNullException(nameof(xmlElement));

            ViewAvailable = xmlElement.GetChildAsBool("ViewAvailable");
        }

        /// <summary>
        /// Makes the storage ready for operating.
        /// </summary>
        public virtual void MakeReady()
        {
        }

        /// <summary>
        /// Closes the storage.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Reads text from the file.
        /// </summary>
        public abstract string ReadText(DataCategory category, string path);

        /// <summary>
        /// Reads a byte array from the file.
        /// </summary>
        public abstract byte[] ReadBytes(DataCategory category, string path);

        /// <summary>
        /// Reads the table of the configuration database.
        /// </summary>
        public abstract void ReadBaseTable(IBaseTable baseTable);

        /// <summary>
        /// Writes the text to the file.
        /// </summary>
        public abstract void WriteText(DataCategory category, string path, string contents);

        /// <summary>
        /// Writes the byte array to the file.
        /// </summary>
        public abstract void WriteBytes(DataCategory category, string path, byte[] bytes);

        /// <summary>
        /// Opens a text file for reading.
        /// </summary>
        public abstract TextReader OpenText(DataCategory category, string path);

        /// <summary>
        /// Opens a binary file for reading.
        /// </summary>
        public abstract BinaryReader OpenBinary(DataCategory category, string path);

        /// <summary>
        /// Gets information associated with the file.
        /// </summary>
        public abstract ShortFileInfo GetFileInfo(DataCategory category, string path);

        /// <summary>
        /// Gets a list of file paths that match the specified pattern.
        /// </summary>
        public abstract ICollection<string> GetFileList(DataCategory category, string path, string searchPattern);
    }
}
