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
 * Module   : ScadaAdminCommon
 * Summary  : Creates extenstion instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Lang;
using System;
using System.IO;
using System.Reflection;

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Creates extenstion instances.
    /// <para>Создает экземпляры расширений.</para>
    /// </summary>
    public static class ExtensionFactory
    {
        /// <summary>
        /// Gets a new instance of the extension logic.
        /// </summary>
        public static bool GetExtensionLogic(string directory, string extensionCode, IAdminContext adminContext,
            out ExtensionLogic extensionLogic, out string message)
        {
            string fileName = Path.Combine(directory, extensionCode + ".dll");
            string typeName = string.Format("Scada.Admin.Extensions.{0}.{0}Logic", extensionCode);

            try
            {
                if (File.Exists(fileName))
                {
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    Type type = assembly.GetType(typeName, true);
                    extensionLogic = (ExtensionLogic)Activator.CreateInstance(type, adminContext);

                    message = string.Format(Locale.IsRussian ?
                        "Расширение {0} {1} загружено из файла {2}" :
                        "Extension {0} {1} loaded from file {2}",
                        extensionCode, extensionLogic.Version, fileName);
                    return true;
                }
                else
                {
                    extensionLogic = null;
                    message = string.Format(Locale.IsRussian ?
                        "Невозможно создать логику расширения {0}. Файл {1} не найден" :
                        "Unable to create extension logic {0}. File {1} not found", extensionCode, fileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                extensionLogic = null;
                message = string.Format(Locale.IsRussian ?
                    "Ошибка при создании логики расширения {0} типа {1} из файла {2}: {3}" :
                    "Error creating extension logic {0} of type {1} from file {2}: {3}",
                    extensionCode, typeName, fileName, ex.Message);
                return false;
            }
        }
    }
}
