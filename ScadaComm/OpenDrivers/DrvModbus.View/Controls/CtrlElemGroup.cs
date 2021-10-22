// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Forms;
using Scada.Lang;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvModbus.View.Controls
{
    /// <summary>
    /// Represents a control for editing an element group.
    /// <para>Представляет элемент управления для редактирования группы элементов.</para>
    /// </summary>
    public partial class CtrlElemGroup : UserControl
    {
        private ElemGroupConfig elemGroup;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlElemGroup()
        {
            InitializeComponent();
            elemGroup = null;
            Settings = null;
        }


        /// <summary>
        /// Получить признак отображения адресов, начиная с 0
        /// </summary>
        private bool ZeroAddr
        {
            get
            {
                return Settings == null ? false : Settings.ZeroAddr;
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
                return Settings == null ? false : Settings.DecAddr;
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
        /// Получить или установить редактируемую группу элементов
        /// </summary>
        public ElemGroupConfig ElemGroup
        {
            get
            {
                return elemGroup;
            }
            set
            {
                elemGroup = null; // чтобы не вызывалось событие ObjectChanged
                ShowElemGroupProps(value);
                elemGroup = value;
            }
        }


        /// <summary>
        /// Отобразить свойства группы элементов
        /// </summary>
        private void ShowElemGroupProps(ElemGroupConfig elemGroup)
        {
            numGrAddress.Value = 1;
            numGrAddress.Minimum = AddrShift;
            numGrAddress.Maximum = ushort.MaxValue + AddrShift;
            numGrAddress.Hexadecimal = !DecAddr;
            ShowFuncCode(elemGroup);

            if (elemGroup == null)
            {
                chkGrActive.Checked = false;
                txtGrName.Text = "";
                cbGrTableType.SelectedIndex = 0;
                numGrAddress.Value = AddrShift;
                lblGrAddressHint.Text = "";
                numGrElemCnt.Value = 1;
                gbElemGroup.Enabled = false;
            }
            else
            {
                chkGrActive.Checked = elemGroup.Active;
                txtGrName.Text = elemGroup.Name;
                cbGrTableType.SelectedIndex = (int)elemGroup.DataBlock;
                numGrAddress.Value = elemGroup.Address + AddrShift;
                lblGrAddressHint.Text = string.Format(DriverPhrases.AddressHint, AddrNotation, AddrShift);
                numGrElemCnt.Value = 1;
                numGrElemCnt.Maximum = elemGroup.MaxElemCnt;
                numGrElemCnt.Value = elemGroup.Elems.Count;
                gbElemGroup.Enabled = true;
            }
        }

        /// <summary>
        /// Отобразить код функции группы элементов
        /// </summary>
        private void ShowFuncCode(ElemGroupConfig elemGroup)
        {
            if (elemGroup == null)
            {
                txtGrFuncCode.Text = "";
            }
            else
            {
                byte funcCode = ModbusUtils.GetReadFuncCode(elemGroup.DataBlock);
                txtGrFuncCode.Text = string.Format("{0} ({1}H)", funcCode, funcCode.ToString("X2"));
            }
        }

        /// <summary>
        /// Вызвать событие ObjectChanged
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(elemGroup, changeArgument));
        }

        /// <summary>
        /// Установить фокус ввода
        /// </summary>
        public void SetFocus()
        {
            txtGrName.Select();
        }


        /// <summary>
        /// Событие возникающее при изменении свойств редактируемого объекта
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void chkGrActive_CheckedChanged(object sender, EventArgs e)
        {
            // изменение активности группы элементов
            if (elemGroup != null)
            {
                elemGroup.Active = chkGrActive.Checked;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtGrName_TextChanged(object sender, EventArgs e)
        {
            // изменение наименования группы элементов
            if (elemGroup != null)
            {
                elemGroup.Name = txtGrName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void cbGrTableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // изменение типа таблицы данных группы элементов
            if (elemGroup != null)
            {
                DataBlock dataBlock = (DataBlock)cbGrTableType.SelectedIndex;
                int maxElemCnt = elemGroup.GetMaxElemCnt(dataBlock);

                bool cancel = elemGroup.Elems.Count > maxElemCnt &&
                    MessageBox.Show(string.Format(DriverPhrases.ElemRemoveWarning, maxElemCnt), 
                        CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, 
                        MessageBoxIcon.Question) != DialogResult.Yes;

                if (cancel)
                {
                    cbGrTableType.SelectedIndexChanged -= cbGrTableType_SelectedIndexChanged;
                    cbGrTableType.SelectedIndex = (int)elemGroup.DataBlock;
                    cbGrTableType.SelectedIndexChanged += cbGrTableType_SelectedIndexChanged;
                }
                else
                {
                    // установка типа таблицы данных
                    elemGroup.DataBlock = dataBlock;
                    ShowFuncCode(elemGroup);

                    // ограничение макс. количества элементов в группе
                    if (numGrElemCnt.Value > maxElemCnt)
                        numGrElemCnt.Value = maxElemCnt;
                    numGrElemCnt.Maximum = maxElemCnt;

                    // установка типа элементов группы по умолчанию
                    ElemType elemType = elemGroup.DefaultElemType;
                    foreach (ElemConfig elem in elemGroup.Elems)
                    {
                        elem.ElemType = elemType;
                    }

                    OnObjectChanged(TreeUpdateTypes.CurrentNode | TreeUpdateTypes.ChildNodes);
                }
            }
        }

        private void numGrAddress_ValueChanged(object sender, EventArgs e)
        {
            // изменение адреса начального элемента в группе
            if (elemGroup != null)
            {
                elemGroup.Address = (ushort)(numGrAddress.Value - AddrShift);
                OnObjectChanged(TreeUpdateTypes.ChildNodes);
            }
        }

        private void numGrElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // изменение количества элементов в группе
            if (elemGroup != null)
            {
                int oldElemCnt = elemGroup.Elems.Count;
                int newElemCnt = (int)numGrElemCnt.Value;

                if (oldElemCnt < newElemCnt)
                {
                    // добавление новых элементов
                    ElemType elemType = elemGroup.DefaultElemType;
                    for (int elemInd = oldElemCnt; elemInd < newElemCnt; elemInd++)
                    {
                        ElemConfig elem = elemGroup.CreateElemConfig();
                        elem.ElemType = elemType;
                        elemGroup.Elems.Add(elem);
                    }
                }
                else if (oldElemCnt > newElemCnt)
                {
                    // удаление лишних элементов
                    for (int i = newElemCnt; i < oldElemCnt; i++)
                    {
                        elemGroup.Elems.RemoveAt(newElemCnt);
                    }
                }

                OnObjectChanged(TreeUpdateTypes.UpdateSignals);
            }
        }
    }
}
