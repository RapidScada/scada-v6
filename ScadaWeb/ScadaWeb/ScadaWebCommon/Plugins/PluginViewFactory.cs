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
 * Summary  : Creates instances of plugin user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2023
 */

using Scada.Lang;
using System.Reflection;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Creates instances of plugin user interface.
    /// <para>Создает экземпляры пользовательского интерфейса плагинов.</para>
    /// </summary>
    public static class PluginViewFactory
    {
        /// <summary>
        /// Gets a new instance of the plugin user interface.
        /// </summary>
        public static bool GetPluginView(string directory, string pluginCode,
            out PluginView pluginView, out string message)
        {
            string fileName = Path.Combine(directory, pluginCode + ".View.dll");
            string typeName = string.Format("Scada.Web.Plugins.{0}.View.{0}View", pluginCode);

            try
            {
                if (File.Exists(fileName))
                {
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    Type type = assembly.GetType(typeName, true);
                    pluginView = (PluginView)Activator.CreateInstance(type);

                    message = string.Format(Locale.IsRussian ?
                        "Загружен интерфейс плагина {0} {1} из файла {2}" :
                        "Loaded plugin interface {0} {1} from file {2}",
                        pluginCode, pluginView.Version, fileName);
                    return true;
                }
                else
                {
                    pluginView = null;
                    message = string.Format(Locale.IsRussian ?
                        "Невозможно создать интерфейс плагина {0}. Файл {1} не найден" :
                        "Unable to create plugin interface {0}. File {1} not found", pluginCode, fileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                pluginView = null;
                message = string.Format(Locale.IsRussian ?
                    "Ошибка при создании интерфейса плагина {0} типа {1} из файла {2}: {3}" :
                    "Error creating plugin interface {0} of type {1} from file {2}: {3}",
                    pluginCode, typeName, fileName, ex.Message);
                return false;
            }
        }
    }
}
