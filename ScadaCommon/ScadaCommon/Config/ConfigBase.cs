/*
 * Copyright 2025 Rapid Software LLC
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
 * Summary  : Represents the base class for configurations
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2024
 */

using Scada.Lang;
using Scada.Storages;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Scada.Config
{
    /// <summary>
    /// Represents the base class for configurations.
    /// <para>Представляет базовый класс конфигураций.</para>
    /// </summary>
    [Serializable]
    public abstract class ConfigBase : IConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigBase()
        {
            UseOutputBuffer = true;
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets a value indicating whether data should be saved to the buffer first and then written to file.
        /// This helps prevent file corruption if the Save method fails.
        /// </summary>
        protected bool UseOutputBuffer { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected virtual void SetToDefault()
        {
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected virtual void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            LoadFromXml(xmlDoc);
        }

        /// <summary>
        /// Loads the configuration from the XML document.
        /// </summary>
        protected virtual void LoadFromXml(XmlDocument xmlDoc)
        {
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected virtual void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            SaveToXml(xmlDoc);
            xmlDoc.Save(writer);
        }

        /// <summary>
        /// Saves the configuration into the XML document.
        /// </summary>
        protected virtual void SaveToXml(XmlDocument xmlDoc)
        {
        }

        /// <summary>
        /// Builds an error message for the load operation.
        /// </summary>
        protected virtual string BuildLoadErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(CommonPhrases.LoadConfigError);
        }

        /// <summary>
        /// Builds an error message for the save operation.
        /// </summary>
        protected virtual string BuildSaveErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(CommonPhrases.SaveConfigError);
        }


        /// <summary>
        /// Loads the configuration from the specified storage.
        /// </summary>
        public bool Load(IStorage storage, string fileName, out string errMsg)
        {
            if (storage == null)
                throw new ArgumentNullException(nameof(storage));

            try
            {
                SetToDefault();

                using (TextReader reader = storage.OpenText(DataCategory.Config, fileName))
                {
                    Load(reader);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = BuildLoadErrorMessage(ex);
                return false;
            }
        }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                using (StreamReader reader = new StreamReader(fileName))
                {
                    Load(reader);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = BuildLoadErrorMessage(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves the configuration to the specified storage.
        /// </summary>
        public bool Save(IStorage storage, string fileName, out string errMsg)
        {
            if (storage == null)
                throw new ArgumentNullException(nameof(storage));

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        Save(writer);
                        string contents = Encoding.UTF8.GetString(stream.ToArray());
                        storage.WriteText(DataCategory.Config, fileName, contents);
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = BuildSaveErrorMessage(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves the configuration to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                if (UseOutputBuffer)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (StreamWriter writer = new StreamWriter(memoryStream))
                        {
                            // save to buffer
                            Save(writer);
                            byte[] buffer = memoryStream.ToArray();

                            // write to file
                            using (FileStream fileStream = 
                                new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                            {
                                fileStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                    }
                }
                else
                {
                    // save to file
                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        Save(writer);
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = BuildSaveErrorMessage(ex);
                return false;
            }
        }
    }
}
