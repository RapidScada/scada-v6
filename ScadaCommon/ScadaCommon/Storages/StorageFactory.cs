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
 * Summary  : Creates storage instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Lang;
using System;
using System.IO;
using System.Reflection;

namespace Scada.Storages
{
    /// <summary>
    /// Creates storage instances.
    /// <para>Создает экземпляры хранилища.</para>
    /// </summary>
    public static class StorageFactory
    {
        /// <summary>
        /// Gets a new instance of the application storage.
        /// </summary>
        public static bool GetStorage(string directory, string storageCode, StorageContext storageContext,
            out StorageLogic storageLogic, out string message)
        {
            string fileName = Path.Combine(directory, storageCode + ".dll");
            string typeName = string.Format("Scada.Storages.{0}.{0}Logic", storageCode);

            try
            {
                if (File.Exists(fileName))
                {
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    Type type = assembly.GetType(typeName, true);
                    storageLogic = (StorageLogic)Activator.CreateInstance(type, storageContext);

                    message = string.Format(Locale.IsRussian ?
                        "Хранилище {0} {1} загружено из файла {2}" :
                        "Storage {0} {1} loaded from file {2}",
                        storageCode, assembly.GetName().Version, fileName);
                    return true;
                }
                else
                {
                    storageLogic = null;
                    message = string.Format(Locale.IsRussian ?
                        "Невозможно создать логику хранилища {0}. Файл {1} не найден" :
                        "Unable to create storage logic {0}. File {1} not found", storageCode, fileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                storageLogic = null;
                message = string.Format(Locale.IsRussian ?
                    "Ошибка при создании логики хранилища {0} типа {1} из файла {2}: {3}" :
                    "Error creating storage logic {0} of type {1} from file {2}: {3}",
                    storageCode, typeName, fileName, ex.Message);
                return false;
            }
        }
    }
}
