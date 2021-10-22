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
    /// The control for editing element
    /// <para>Элемент управления для редактирования элемента Modbus</para>
    /// </summary>
    public partial class CtrlElem : UserControl
    {
        private ElemInfo elemInfo;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlElem()
        {
            InitializeComponent();
            elemInfo = null;
        }


        /// <summary>
        /// Получить или установить редактируемый элемент
        /// </summary>
        public ElemInfo ElemInfo
        {
            get
            {
                return elemInfo;
            }
            set
            {
                elemInfo = null; // чтобы не вызывалось событие ObjectChanged
                ShowElemProps(value);
                elemInfo = value;
            }
        }


        /// <summary>
        /// Отобразить свойства элемента
        /// </summary>
        private void ShowElemProps(ElemInfo elemInfo)
        {
            if (elemInfo == null)
            {
                txtElemName.Text = "";
                txtElemAddress.Text = "";
                txtElemSignal.Text = "";
                rbBool.Checked = true;
                txtElemByteOrder.Text = "";
                gbElem.Enabled = false;
            }
            else
            {
                txtElemName.Text = elemInfo.Elem.Name;
                txtElemAddress.Text = elemInfo.AddressRange;
                txtElemSignal.Text = elemInfo.TagNum.ToString();
                ElemType elemType = elemInfo.Elem.ElemType;

                if (elemType == ElemType.Bool)
                {
                    rbUShort.Enabled = rbShort.Enabled = rbUInt.Enabled = rbInt.Enabled =
                        rbULong.Enabled = rbLong.Enabled = rbFloat.Enabled = rbDouble.Enabled = false;
                    rbBool.Enabled = true;
                    txtElemByteOrder.Text = "";
                    txtElemByteOrder.Enabled = false;
                }
                else
                {
                    rbUShort.Enabled = rbShort.Enabled = rbUInt.Enabled = rbInt.Enabled =
                        rbULong.Enabled = rbLong.Enabled = rbFloat.Enabled = rbDouble.Enabled = true;
                    rbBool.Enabled = false;
                    txtElemByteOrder.Text = elemInfo.Elem.ByteOrder;
                    txtElemByteOrder.Enabled = true;
                }

                switch (elemType)
                {
                    case ElemType.UShort:
                        rbUShort.Checked = true;
                        break;
                    case ElemType.Short:
                        rbShort.Checked = true;
                        break;
                    case ElemType.UInt:
                        rbUInt.Checked = true;
                        break;
                    case ElemType.Int:
                        rbInt.Checked = true;
                        break;
                    case ElemType.ULong:
                        rbULong.Checked = true;
                        break;
                    case ElemType.Long:
                        rbLong.Checked = true;
                        break;
                    case ElemType.Float:
                        rbFloat.Checked = true;
                        break;
                    case ElemType.Double:
                        rbDouble.Checked = true;
                        break;
                    default:
                        rbBool.Checked = true;
                        break;
                }

                gbElem.Enabled = true;
            }
        }

        /// <summary>
        /// Вызвать событие ObjectChanged
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(elemInfo, changeArgument));
        }

        /// <summary>
        /// Установить фокус ввода
        /// </summary>
        public void SetFocus()
        {
            txtElemName.Select();
        }


        /// <summary>
        /// Событие возникающее при изменении свойств редактируемого объекта
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void txtElemName_TextChanged(object sender, EventArgs e)
        {
            // изменение наименования элемента
            if (elemInfo != null)
            {
                elemInfo.Elem.Name = txtElemName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void rbType_CheckedChanged(object sender, EventArgs e)
        {
            // изменение типа элемента
            if (elemInfo != null && ((RadioButton)sender).Checked)
            {
                ElemConfig elem = elemInfo.Elem;

                if (rbUShort.Checked)
                    elem.ElemType = ElemType.UShort;
                else if (rbShort.Checked)
                    elem.ElemType = ElemType.Short;
                else if (rbUInt.Checked)
                    elem.ElemType = ElemType.UInt;
                else if (rbInt.Checked)
                    elem.ElemType = ElemType.Int;
                else if (rbULong.Checked)
                    elem.ElemType = ElemType.ULong;
                else if (rbLong.Checked)
                    elem.ElemType = ElemType.Long;
                else if (rbFloat.Checked)
                    elem.ElemType = ElemType.Float;
                else if (rbDouble.Checked)
                    elem.ElemType = ElemType.Double;
                else
                    elem.ElemType = ElemType.Bool;

                txtElemAddress.Text = elemInfo.AddressRange;
                OnObjectChanged(TreeUpdateTypes.CurrentNode | TreeUpdateTypes.NextSiblings);
            }
        }

        private void txtByteOrder_TextChanged(object sender, EventArgs e)
        {
            // изменение порядка байт элемента
            if (elemInfo != null)
            {
                elemInfo.Elem.ByteOrder = txtElemByteOrder.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
