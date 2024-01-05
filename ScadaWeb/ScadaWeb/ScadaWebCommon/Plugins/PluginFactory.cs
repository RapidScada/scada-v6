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
 * Module   : ScadaWebCommon
 * Summary  : Creates plugin instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Lang;
using Scada.Web.Services;
using System.Reflection;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Creates plugin instances.
    /// <para>Создает экземпляры плагинов.</para>
    /// </summary>
    public static class PluginFactory
    {
        /// <summary>
        /// Gets a new instance of the plugin logic.
        /// </summary>
        public static bool GetPluginLogic(string directory, string pluginCode, IWebContext webContext,
            out PluginLogic pluginLogic, out string message)
        {
            string fileName = Path.Combine(directory, pluginCode + ".dll");
            string typeName = string.Format("Scada.Web.Plugins.{0}.{0}Logic", pluginCode);

            try
            {
                if (File.Exists(fileName))
                {
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    Type type = assembly.GetType(typeName, true);
                    pluginLogic = (PluginLogic)Activator.CreateInstance(type, webContext);

                    message = string.Format(Locale.IsRussian ?
                        "Плагин {0} {1} загружен из файла {2}" :
                        "Plugin {0} {1} loaded from file {2}",
                        pluginCode, pluginLogic.Version, fileName);
                    return true;
                }
                else
                {
                    pluginLogic = null;
                    message = string.Format(Locale.IsRussian ?
                        "Невозможно создать логику плагина {0}. Файл {1} не найден" :
                        "Unable to create plugin logic {0}. File {1} not found", pluginCode, fileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                pluginLogic = null;
                message = string.Format(Locale.IsRussian ?
                    "Ошибка при создании логики плагина {0} типа {1} из файла {2}: {3}" :
                    "Error creating plugin logic {0} of type {1} from file {2}: {3}",
                    pluginCode, typeName, fileName, ex.Message);
                return false;
            }
        }
    }
}
