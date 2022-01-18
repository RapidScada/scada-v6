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
 * Module   : ScadaCommon.Svc
 * Summary  : Represents properties of a Windows service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Lang;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Scada.Svc
{
    /// <summary>
    /// Represents properties of a Windows service.
    /// <para>Представляет свойства службы Windows.</para>
    /// </summary>
    public class SvcProps
    {
        /// <summary>
        /// The name of the file containing the service properties.
        /// </summary>
        private const string SvcPropsFileName = "svc_config.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SvcProps()
        {
            ServiceName = "";
            Description = "";
        }


        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the service description.
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Loads the service properties from the specified file.
        /// </summary>
        private void LoadFromFile(string fileName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                ServiceName = xmlDoc.DocumentElement.GetChildAsString("ServiceName");
                Description = xmlDoc.DocumentElement.GetChildAsString("Description");
            }
            catch (Exception ex)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Ошибка при загрузке свойств службы" :
                    "Error loading service properties", ex);
            }
        }

        /// <summary>
        /// Loads the service properties.
        /// </summary>
        public bool LoadFromFile()
        {
            string fileName = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                SvcPropsFileName);

            if (File.Exists(fileName))
            {
                LoadFromFile(fileName);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
