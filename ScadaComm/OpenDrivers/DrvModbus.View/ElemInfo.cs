// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvModbus.View
{
    /// <summary>
    /// Contains information about a Modbus element.
    /// <para>Содержит информацию об элементе Modbus.</para>
    /// </summary>
    public class ElemInfo
    {
        /// <summary>
        /// Получить или установить элемент
        /// </summary>
        public ElemConfig Elem { get; set; }

        /// <summary>
        /// Получить или установить группу элементов, в которую входит элемент
        /// </summary>
        public ElemGroupConfig ElemGroup { get; set; }

        /// <summary>
        /// Получить или установить ссылку настройки шаблона
        /// </summary>
        public DeviceTemplateOptions Settings { get; set; }

        /// <summary>
        /// Получить или установить адрес, начинающийся от 0
        /// </summary>
        public ushort Address { get; set; }

        /// <summary>
        /// Получить или установить сигнал КП
        /// </summary>
        public int Signal { get; set; }

        /// <summary>
        /// Получить строковую запись диапазона адресов элемента
        /// </summary>
        public string AddressRange
        {
            get
            {
                return ModbusUtils.GetAddressRange(Address, Elem.Quantity, Settings.ZeroAddr, Settings.DecAddr);
            }
        }

        /// <summary>
        /// Получить обозначение элемента в дереве
        /// </summary>
        public string Caption
        {
            get
            {
                return $"{(string.IsNullOrEmpty(Elem.Name) ? DriverPhrases.UnnamedElem : Elem.Name)} ({AddressRange})";
            }
        }
    }
}
