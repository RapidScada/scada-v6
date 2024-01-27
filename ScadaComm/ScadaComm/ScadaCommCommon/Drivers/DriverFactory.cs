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
 * Module   : ScadaCommCommon
 * Summary  : Creates driver instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Lang;
using System;
using System.IO;
using System.Reflection;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Creates driver instances.
    /// <para>Создает экземпляры драйверов.</para>
    /// </summary>
    public static class DriverFactory
    {
        /// <summary>
        /// Gets a new instance of the driver logic.
        /// </summary>
        public static bool GetDriverLogic(string directory, string driverCode, ICommContext commContext,
            out DriverLogic driverLogic, out string message)
        {
            string fileName = Path.Combine(directory, driverCode + ".Logic.dll");
            string typeName = string.Format("Scada.Comm.Drivers.{0}.Logic.{0}Logic", driverCode);

            try
            {
                if (File.Exists(fileName))
                {
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    Type type = assembly.GetType(typeName, true);
                    driverLogic = (DriverLogic)Activator.CreateInstance(type, commContext);

                    message = string.Format(Locale.IsRussian ?
                        "Драйвер {0} {1} загружен из файла {2}" :
                        "Driver {0} {1} loaded from file {2}",
                        driverCode, driverLogic.Version, fileName);
                    return true;
                }
                else
                {
                    driverLogic = null;
                    message = string.Format(Locale.IsRussian ?
                        "Невозможно создать логику драйвера {0}. Файл {1} не найден" :
                        "Unable to create driver logic {0}. File {1} not found", driverCode, fileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                driverLogic = null;
                message = string.Format(Locale.IsRussian ?
                    "Ошибка при создании логики драйвера {0} типа {1} из файла {2}: {3}" :
                    "Error creating driver logic {0} of type {1} from file {2}: {3}",
                    driverCode, typeName, fileName, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets a new instance of the driver user interface.
        /// </summary>
        public static bool GetDriverView(string directory, string driverCode,
            out DriverView driverView, out string message)
        {
            string fileName = Path.Combine(directory, driverCode + ".View.dll");
            string typeName = string.Format("Scada.Comm.Drivers.{0}.View.{0}View", driverCode);

            try
            {
                if (File.Exists(fileName))
                {
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    Type type = assembly.GetType(typeName, true);
                    driverView = (DriverView)Activator.CreateInstance(type);

                    message = string.Format(Locale.IsRussian ?
                        "Загружен интерфейс драйвера {0} {1} из файла {2}" :
                        "Loaded driver interface {0} {1} from file {2}",
                        driverCode, driverView.Version, fileName);
                    return true;
                }
                else
                {
                    driverView = null;
                    message = string.Format(Locale.IsRussian ?
                        "Невозможно создать интерфейс драйвера {0}. Файл {1} не найден" :
                        "Unable to create driver interface {0}. File {1} not found", driverCode, fileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                driverView = null;
                message = string.Format(Locale.IsRussian ?
                    "Ошибка при создании интерфейса драйвера {0} типа {1} из файла {2}: {3}" :
                    "Error creating driver interface {0} of type {1} from file {2}: {3}",
                    driverCode, typeName, fileName, ex.Message);
                return false;
            }
        }
    }
}
