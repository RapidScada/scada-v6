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
 * Module   : ScadaCommon
 * Summary  : Represents the base class for configurations
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Lang;
using Scada.Storages;
using System;
using System.IO;

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
            SetToDefault();
        }


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
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected virtual void Save(TextWriter writer)
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
        /// Saves the configuration to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    Save(writer);
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
