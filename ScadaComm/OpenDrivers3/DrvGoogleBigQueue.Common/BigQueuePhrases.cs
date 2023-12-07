// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue
{
    /// <summary>
    /// The phrases used in the Modbus protocol implementation.
    /// <para>Фразы, используемые в реализации протокола Modbus.</para>
    /// </summary>
    public static class ModbusPhrases
    {
        // Scada.Comm.Drivers.DrvGoogleBigQueue.Config
        public static string LoadTemplateError { get; private set; }
        public static string SaveTemplateError { get; private set; }

        // Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol
        public static string Request { get; set; }
        public static string Command { get; set; }
        public static string DeviceError { get; set; }
        public static string IllegalDataBlock { get; set; }
        public static string OK { get; set; }
        public static string CrcError { get; set; }
        public static string LrcError { get; set; }
        public static string CommError { get; set; }
        public static string InvalidDevAddr { get; set; }
        public static string InvalidSymbol { get; set; }
        public static string InvalidAduLength { get; set; }
        public static string InvalidMbap { get; set; }
        public static string InvalidPduLength { get; set; }
        public static string InvalidPduFuncCode { get; set; }
        public static string InvalidPduData { get; set; }

        static ModbusPhrases()
        {
            if (Locale.IsRussian)
            {
                LoadTemplateError = "Ошибка при загрузке шаблона устройства";
                SaveTemplateError = "Ошибка при сохранении шаблона устройства";

                Request = "Запрос значений группы элементов \"{0}\"";
                Command = "Команда \"{0}\"";
                DeviceError = "Ошибка, полученная от устройства";
                IllegalDataBlock = "Ошибка: недопустимый блок данных";
                OK = "OK";
                CrcError = "Ошибка CRC";
                LrcError = "Ошибка LRC";
                CommError = "Ошибка связи";
                InvalidDevAddr = "Ошибка: неверный адрес устройства";
                InvalidSymbol = "Ошибка: неверный символ";
                InvalidAduLength = "Ошибка: неверная длина ADU";
                InvalidMbap = "Ошибка: неверные данные MBAP Header";
                InvalidPduLength = "Ошибка: неверная длина PDU";
                InvalidPduFuncCode = "Ошибка: неверный код функции PDU";
                InvalidPduData = "Ошибка: неверные данные PDU";
            }
            else
            {
                LoadTemplateError = "Error loading device template";
                SaveTemplateError = "Error saving device template";

                Request = "Request element group \"{0}\"";
                Command = "Command \"{0}\"";
                DeviceError = "Error received from device";
                IllegalDataBlock = "Error: illegal data block.";
                OK = "OK";
                CrcError = "Error: invalid CRC";
                LrcError = "Error: invalid LRC";
                CommError = "Error: communication failed";
                InvalidDevAddr = "Error: invalid device address";
                InvalidSymbol = "Error: invalid symbol";
                InvalidAduLength = "Error: invalid ADU length";
                InvalidMbap = "Error: invalid MBAP Header data";
                InvalidPduLength = "Error: invalid PDU length";
                InvalidPduFuncCode = "Error: invalid PDU function code";
                InvalidPduData = "Error: invalid PDU data";
            }
        }
    }
}
