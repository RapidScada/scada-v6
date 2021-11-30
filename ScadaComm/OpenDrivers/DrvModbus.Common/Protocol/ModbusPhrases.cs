// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvModbus.Protocol
{
    /// <summary>
    /// The phrases used in the Modbus protocol implementation.
    /// <para>Фразы, используемые в реализации протокола Modbus.</para>
    /// </summary>
    public static class ModbusPhrases
    {
        public static string IncorrectPduLength { get; set; }
        public static string IncorrectPduFuncCode { get; set; }
        public static string IncorrectPduData { get; set; }
        public static string Request { get; set; }
        public static string Command { get; set; }
        public static string DeviceError { get; set; }
        public static string IllegalDataBlock { get; set; }
        public static string OK { get; set; }
        public static string CrcError { get; set; }
        public static string LrcError { get; set; }
        public static string CommErrorWithExclamation { get; set; }
        public static string IncorrectDevAddr { get; set; }
        public static string IncorrectSymbol { get; set; }
        public static string IncorrectAduLength { get; set; }
        public static string IncorrectMbap { get; set; }

        static ModbusPhrases()
        {
            if (Locale.IsRussian)
            {
                IncorrectPduLength = "Некорректная длина PDU";
                IncorrectPduFuncCode = "Некорректный код функции PDU";
                IncorrectPduData = "Некорректные данные PDU";
                Request = "Запрос значений группы элементов \"{0}\"";
                Command = "Команда \"{0}\"";
                DeviceError = "Ошибка устройства";
                IllegalDataBlock = "Недопустимый блок данных.";
                OK = "OK";
                CrcError = "Ошибка CRC!";
                LrcError = "Ошибка LRC!";
                CommErrorWithExclamation = "Ошибка связи!";
                IncorrectDevAddr = "Некорректный адрес устройства!";
                IncorrectSymbol = "Некорректный символ!";
                IncorrectAduLength = "Некорректная длина ADU!";
                IncorrectMbap = "Некорректные данные MBAP Header!";
            }
            else
            {
                IncorrectPduLength = "Incorrect PDU length";
                IncorrectPduFuncCode = "Incorrect PDU function code";
                IncorrectPduData = "Incorrect PDU data";
                Request = "Request element group \"{0}\"";
                Command = "Command \"{0}\"";
                DeviceError = "Device error";
                IllegalDataBlock = "Illegal data block.";
                OK = "OK";
                CrcError = "CRC error!";
                LrcError = "LRC error!";
                CommErrorWithExclamation = "Communication error!";
                IncorrectDevAddr = "Incorrect device address!";
                IncorrectSymbol = "Incorrect symbol!";
                IncorrectAduLength = "Incorrect ADU length!";
                IncorrectMbap = "Incorrect MBAP Header data!";
            }
        }
    }
}
