// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Forms;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvModbus.View.Controls
{
    /// <summary>
    /// Represetns a control for editing a command.
    /// <para>Представляет элемент управления для редактирования команды.</para>
    /// </summary>
    public partial class CtrlCmd : UserControl
    {
        private CmdConfig modbusCmd;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlCmd()
        {
            InitializeComponent();
            modbusCmd = null;
            Settings = null;
        }


        /// <summary>
        /// Получить признак отображения адресов, начиная с 0
        /// </summary>
        private bool ZeroAddr
        {
            get
            {
                return Settings != null && Settings.ZeroAddr;
            }
        }

        /// <summary>
        /// Получить смещение адреса
        /// </summary>
        private int AddrShift
        {
            get
            {
                return ZeroAddr ? 0 : 1;
            }
        }

        /// <summary>
        /// Получить признак отображения адресов в 10-тичной системе
        /// </summary>
        private bool DecAddr
        {
            get
            {
                return Settings != null && Settings.DecAddr;
            }
        }

        /// <summary>
        /// Получить обозначение системы счисления адресов
        /// </summary>
        private string AddrNotation
        {
            get
            {
                return DecAddr ? "DEC" : "HEX";
            }
        }

        /// <summary>
        /// Получить или установить ссылку настройки шаблона
        /// </summary>
        public DeviceTemplateOptions Settings { get; set; }

        /// <summary>
        /// Получить или установить редактируемую команду
        /// </summary>
        public CmdConfig ModbusCmd
        {
            get
            {
                return modbusCmd;
            }
            set
            {
                modbusCmd = null; // чтобы не вызывалось событие ObjectChanged
                ShowCmdProps(value);
                modbusCmd = value;
            }
        }


        /// <summary>
        /// Отобразить свойства команды
        /// </summary>
        private void ShowCmdProps(CmdConfig modbusCmd)
        {
            numCmdAddress.Value = 1;
            numCmdAddress.Minimum = AddrShift;
            numCmdAddress.Maximum = ushort.MaxValue + AddrShift;
            numCmdAddress.Hexadecimal = !DecAddr;
            ShowFuncCode(modbusCmd);
            ShowByteOrder(modbusCmd);

            if (modbusCmd == null)
            {
                txtCmdName.Text = "";
                cbCmdTableType.SelectedIndex = 0;
                numCmdAddress.Value = AddrShift;
                lblCmdAddressHint.Text = "";
                cbCmdElemType.SelectedIndex = 0;
                numCmdElemCnt.Value = 1;
                txtCmdByteOrder.Text = "";
                numCmdNum.Value = 1;
                gbCmd.Enabled = false;
            }
            else
            {
                txtCmdName.Text = modbusCmd.Name;
                cbCmdTableType.SelectedIndex = modbusCmd.DataBlock == DataBlock.Coils ? 0 : 1;
                chkCmdMultiple.Checked = modbusCmd.Multiple;
                numCmdAddress.Value = modbusCmd.Address + AddrShift;
                lblCmdAddressHint.Text = string.Format(DriverPhrases.AddressHint, AddrNotation, AddrShift);
                cbCmdElemType.SelectedIndex = (int)modbusCmd.ElemType;
                cbCmdElemType.Enabled = modbusCmd.ElemTypeEnabled;
                numCmdElemCnt.Maximum = modbusCmd.MaxElemCnt;
                numCmdElemCnt.SetValue(modbusCmd.ElemCnt);
                numCmdElemCnt.Enabled = modbusCmd.Multiple;
                numCmdNum.Value = modbusCmd.CmdNum;
                gbCmd.Enabled = true;
            }
        }

        /// <summary>
        /// Отобразить код функции команды
        /// </summary>
        private void ShowFuncCode(CmdConfig modbusCmd)
        {
            if (modbusCmd == null)
            {
                txtCmdFuncCode.Text = "";
            }
            else
            {
                byte funcCode = ModbusUtils.GetWriteFuncCode(modbusCmd.DataBlock, modbusCmd.Multiple);
                txtCmdFuncCode.Text = string.Format("{0} ({1}h)", funcCode, funcCode.ToString("X2"));
            }
        }

        /// <summary>
        /// Отобразить порядок байт команды
        /// </summary>
        private void ShowByteOrder(CmdConfig modbusCmd)
        {
            if (modbusCmd != null && modbusCmd.ByteOrderEnabled)
            {
                txtCmdByteOrder.Text = modbusCmd.ByteOrder;
                txtCmdByteOrder.Enabled = true;
            }
            else
            {
                txtCmdByteOrder.Text = "";
                txtCmdByteOrder.Enabled = false;
            }
        }

        /// <summary>
        /// Вызвать событие ObjectChanged
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(modbusCmd, changeArgument));
        }

        /// <summary>
        /// Установить фокус ввода
        /// </summary>
        public void SetFocus()
        {
            txtCmdName.Select();
        }


        /// <summary>
        /// Событие возникающее при изменении свойств редактируемого объекта
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void txtCmdName_TextChanged(object sender, EventArgs e)
        {
            // изменение наименования команды
            if (modbusCmd != null)
            {
                modbusCmd.Name = txtCmdName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void cbCmdTableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // изменение типа таблицы данных команды
            if (modbusCmd != null)
            {
                modbusCmd.DataBlock = cbCmdTableType.SelectedIndex == 0 ? DataBlock.Coils : DataBlock.HoldingRegisters;
                ShowFuncCode(modbusCmd);
                ShowByteOrder(modbusCmd);

                cbCmdElemType.SelectedIndex = (int)modbusCmd.DefaultElemType;
                cbCmdElemType.Enabled = modbusCmd.ElemTypeEnabled;
                numCmdElemCnt.Maximum = modbusCmd.MaxElemCnt;
                numCmdElemCnt.SetValue(modbusCmd.ElemCnt);
                numCmdElemCnt.Enabled = modbusCmd.Multiple;

                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void chkCmdMultiple_CheckedChanged(object sender, EventArgs e)
        {
            // изменение множественности команды
            if (modbusCmd != null)
            {
                modbusCmd.Multiple = chkCmdMultiple.Checked;
                ShowFuncCode(modbusCmd);
                ShowByteOrder(modbusCmd);

                cbCmdElemType.SelectedIndex = (int)modbusCmd.DefaultElemType;
                cbCmdElemType.Enabled = modbusCmd.ElemTypeEnabled;
                numCmdElemCnt.Enabled = modbusCmd.Multiple;

                if (!modbusCmd.Multiple)
                    numCmdElemCnt.Value = 1;

                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numCmdAddress_ValueChanged(object sender, EventArgs e)
        {
            // изменение адреса команды
            if (modbusCmd != null)
            {
                modbusCmd.Address = (ushort)(numCmdAddress.Value - AddrShift);
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void cbCmdElemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // изменение типа элементов
            if (modbusCmd != null)
            {
                ElemType newElemType = (ElemType)cbCmdElemType.SelectedIndex;

                if (modbusCmd.DataBlock == DataBlock.HoldingRegisters && newElemType == ElemType.Bool)
                {
                    // отмена выбора типа Bool для регистров хранения
                    cbCmdElemType.SelectedIndexChanged -= cbCmdElemType_SelectedIndexChanged;
                    cbCmdElemType.SelectedIndex = (int)modbusCmd.ElemType;
                    cbCmdElemType.SelectedIndexChanged += cbCmdElemType_SelectedIndexChanged;
                }
                else
                {
                    modbusCmd.ElemType = newElemType;
                    OnObjectChanged(TreeUpdateTypes.CurrentNode);
                }
            }
        }

        private void numCmdElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // изменение количества элементов команды
            if (modbusCmd != null)
            {
                modbusCmd.ElemCnt = (int)numCmdElemCnt.Value;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtCmdByteOrder_TextChanged(object sender, EventArgs e)
        {
            // изменение порядка байт команды
            if (modbusCmd != null)
            {
                modbusCmd.ByteOrder = txtCmdByteOrder.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numCmdNum_ValueChanged(object sender, EventArgs e)
        {
            // изменение номера команды КП
            if (modbusCmd != null)
            {
                modbusCmd.CmdNum = (int)numCmdNum.Value;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
