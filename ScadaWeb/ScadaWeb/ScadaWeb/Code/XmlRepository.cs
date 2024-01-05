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
 * Module   : Webstation Application
 * Summary  : Represents an XML repository backed by the storage mechanism
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.DataProtection.Repositories;
using Scada.Log;
using Scada.Storages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Scada.Web.Code
{
    /// <summary>
    /// Represents an XML repository backed by the storage mechanism.
    /// <para>Представляет XML-репозиторий на основе механизма хранилища.</para>
    /// </summary>
    /// <remarks>
    /// See https://github.com/dotnet/aspnetcore/blob/main/src/DataProtection/DataProtection/src/Repositories/FileSystemXmlRepository.cs
    /// </remarks>
    internal class XmlRepository : IXmlRepository
    {
        /// <summary>
        /// The repository directory.
        /// </summary>
        private const string RepositoryDir = "XmlRepository\\";

        private readonly IStorage storage; // the application storage
        private readonly ILog log;         // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public XmlRepository(IStorage storage, ILog log)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }


        /// <summary>
        /// Checks if the filename is safe.
        /// </summary>
        private static bool IsSafeFileName(string fileName)
        {
            // non-empty and contain only a-zA-Z0-9, hyphen, and underscore
            return !string.IsNullOrEmpty(fileName) && fileName.All(c =>
                c == '-'
                || c == '_'
                || ('0' <= c && c <= '9')
                || ('A' <= c && c <= 'Z')
                || ('a' <= c && c <= 'z'));
        }

        /// <summary>
        /// Gets all top-level XML elements in the repository.
        /// </summary>
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            try
            {
                List<XElement> xElements = new();

                foreach (string path in storage.GetFileList(DataCategory.Storage, RepositoryDir, "*.xml"))
                {
                    using TextReader reader = storage.OpenText(DataCategory.Storage, path);
                    xElements.Add(XElement.Load(reader));
                }

                return xElements.AsReadOnly();
            }
            catch (Exception ex)
            {
                log.WriteError(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds a top-level XML element to the repository.
        /// </summary>
        public virtual void StoreElement(XElement element, string friendlyName)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                if (!IsSafeFileName(friendlyName))
                    friendlyName = Guid.NewGuid().ToString();

                storage.WriteText(DataCategory.Storage, RepositoryDir + friendlyName + ".xml", element.ToString());
            }
            catch (Exception ex)
            {
                log.WriteError(ex);
                throw;
            }
        }
    }
}
