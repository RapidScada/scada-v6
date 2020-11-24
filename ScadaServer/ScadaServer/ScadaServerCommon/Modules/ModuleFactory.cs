/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaServerCommon
 * Summary  : Creates module instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using System;
using System.IO;
using System.Reflection;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Creates module instances.
    /// <para>Создает экземпляры модулей.</para>
    /// </summary>
    public static class ModuleFactory
    {
        /// <summary>
        /// Gets a new instance of the module logic.
        /// </summary>
        public static bool GetModuleLogic(string directory, string moduleCode, IServerContext serverContext,
            out ModuleLogic moduleLogic, out string message)
        {
            string fileName = Path.Combine(directory, moduleCode + ".Logic.dll");
            string typeName = string.Format("Scada.Server.Modules.{0}.Logic.{0}Logic", moduleCode);

            try
            {
                if (File.Exists(fileName))
                {
                    Assembly assembly = Assembly.LoadFile(fileName);
                    Type type = assembly.GetType(typeName, true);
                    moduleLogic = (ModuleLogic)Activator.CreateInstance(type, serverContext);

                    message = string.Format(Locale.IsRussian ?
                        "Загружен модуль {0} {1} из файла {2}" :
                        "Loaded module {0} {1} from file {2}",
                        moduleCode, assembly.GetName().Version, fileName);
                    return true;
                }
                else
                {
                    moduleLogic = null;
                    message = string.Format(Locale.IsRussian ?
                        "Невозможно создать логику модуля {0}. Файл {1} не найден" :
                        "Unable to create module logic {0}. File {1} not found", moduleCode, fileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                moduleLogic = null;
                message = string.Format(Locale.IsRussian ?
                    "Ошибка при создании логики модуля {0} типа {1} из файла {2}: {3}" :
                    "Error creating module logic {0} of type {1} from file {2}: {3}",
                    moduleCode, typeName, fileName, ex.Message);
                return false;
            }
        }
    }
}
