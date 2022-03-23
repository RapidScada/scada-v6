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
 * Module   : ScadaCommCommon
 * Summary  : Represents a Communicator manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;

namespace Scada.Comm.Lang
{
    /// <summary>
    /// The phrases used by Communicator and its modules.
    /// <para>Фразы, используемые Коммуникатором и его модулями.</para>
    /// </summary>
    public static class CommPhrases
    {
        // Engine
        public static string ErrorInDriver { get; private set; }
        public static string ErrorInChannel { get; private set; }
        public static string ErrorInDevice { get; private set; }
        public static string ErrorInDataSource { get; private set; }

        // Data sources
        public static string DataSourceMessage { get; private set; }

        // Channels
        public static string SendNotation { get; private set; }
        public static string ReceiveNotation { get; private set; }
        public static string ReadDataError { get; private set; }
        public static string ReadDataStopCondError { get; private set; }
        public static string ReadLinesError { get; private set; }
        public static string WriteDataError { get; private set; }
        public static string WriteLineError { get; private set; }
        public static string UnableFindDevice { get; private set; }

        // Devices
        public static string DeviceMessage { get; private set; }
        public static string ResponseOK { get; private set; }
        public static string ResponseError { get; private set; }
        public static string ResponseCsError { get; private set; }
        public static string ResponseCrcError { get; private set; }
        public static string InvalidCommand { get; private set; }
        public static string ErrorPrefix { get; private set; }
        public static string Off { get; private set; }
        public static string On { get; private set; }

        // Drivers
        public static string DriverMessage { get; private set; }
        public static string SharedObject { get; private set; }

        // Scada.Comm.Devices
        public static string LoadDeviceConfigError { get; private set; }
        public static string SaveDeviceConfigError { get; private set; }
        public static string SaveDeviceConfigConfirm { get; private set; }

        // Scada.Comm.Drivers
        public static string LoadDriverConfigError { get; private set; }
        public static string SaveDriverConfigError { get; private set; }
        public static string SaveDriverConfigConfirm { get; private set; }

        public static void Init()
        {
            // set phrases that are used in the bilingual service logic, depending on the locale
            if (Locale.IsRussian)
            {
                ErrorInDriver = "Ошибка при вызове метода {0} драйвера {1}";
                ErrorInChannel = "Ошибка при вызове метода {0} канала связи {1}";
                ErrorInDevice = "Ошибка при вызове метода {0} устройства {1}";
                ErrorInDataSource = "Ошибка при вызове метода {0} источника данных {1}";

                DataSourceMessage = "Источник данных {0}: {1}";

                SendNotation = "Отправка";
                ReceiveNotation = "Приём";
                ReadDataError = "Ошибка при считывании данных";
                ReadDataStopCondError = "Ошибка при считывании данных с условием остановки";
                ReadLinesError = "Ошибка при считывании строк";
                WriteDataError = "Ошибка при записи данных";
                WriteLineError = "Ошибка при записи строки";
                UnableFindDevice = "Не удалось найти ни одного устройства с адресом {0}";

                DeviceMessage = "Устройство {0}: {1}";
                ResponseOK = "OK";
                ResponseError = "Ошибка связи";
                ResponseCsError = "Ошибка КС";
                ResponseCrcError = "Ошибка CRC";
                InvalidCommand = "Ошибка: недопустимая команда";
                ErrorPrefix = "Ошибка: ";
                Off = "Откл";
                On = "Вкл";

                DriverMessage = "Драйвер {0}: {1}";
                SharedObject = "<Объект>";
            }
            else
            {
                ErrorInDriver = "Error calling the {0} method of the {1} driver";
                ErrorInChannel = "Error calling the {0} method of the {1} communication channel";
                ErrorInDevice = "Error calling the {0} method of the {1} device";
                ErrorInDataSource = "Error calling the {0} method of the {1} data source";

                DataSourceMessage = "Data source {0}: {1}";

                SendNotation = "Send";
                ReceiveNotation = "Receive";
                ReadDataError = "Error reading data";
                ReadDataStopCondError = "Error reading data with stop condition";
                ReadLinesError = "Error reading lines";
                WriteDataError = "Error writing data";
                WriteLineError = "Error writing line";
                UnableFindDevice = "Unable to find any device with address {0}";

                DeviceMessage = "Device {0}: {1}";
                ResponseOK = "OK";
                ResponseError = "Error: communication failed";
                ResponseCsError = "Error: invalid checksum";
                ResponseCrcError = "Error: invalid CRC";
                InvalidCommand = "Error: invalid command";
                ErrorPrefix = "Error: ";
                Off = "Off";
                On = "On";

                DriverMessage = "Driver {0}: {1}";
                SharedObject = "<Object>";
            }


            // load phrases that are used in the multilingual user interface from dictionaries
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Devices");
            LoadDeviceConfigError = dict["LoadDeviceConfigError"];
            SaveDeviceConfigError = dict["SaveDeviceConfigError"];
            SaveDeviceConfigConfirm = dict["SaveDeviceConfigConfirm"];

            dict = Locale.GetDictionary("Scada.Comm.Drivers");
            LoadDriverConfigError = dict["LoadDriverConfigError"];
            SaveDriverConfigError = dict["SaveDriverConfigError"];
            SaveDriverConfigConfirm = dict["SaveDriverConfigConfirm"];
        }
    }
}
