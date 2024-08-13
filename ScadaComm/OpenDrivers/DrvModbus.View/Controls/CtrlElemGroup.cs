// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Forms;
using Scada.Lang;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvModbus.View.Controls
{
    /// <summary>
    /// Represents a control for editing an element group.
    /// <para>Представляет элемент управления для редактирования группы элементов.</para>
    /// </summary>
    public partial class CtrlElemGroup : UserControl
    {
        private ElemGroupConfig elemGroup; // the element group configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlElemGroup()
        {
            InitializeComponent();
            elemGroup = null;
            TemplateOptions = null;
        }


        /// <summary>
        /// Gets a value indicating whether addresses are displayed starting from zero.
        /// </summary>
        private bool ZeroAddr
        {
            get
            {
                return TemplateOptions != null && TemplateOptions.ZeroAddr;
            }
        }

        /// <summary>
        /// Gets a value indicating whether addresses are displayed as decimals.
        /// </summary>
        private bool DecAddr
        {
            get
            {
                return TemplateOptions != null && TemplateOptions.DecAddr;
            }
        }

        /// <summary>
        /// Gets the address shift.
        /// </summary>
        private int AddrShift
        {
            get
            {
                return ZeroAddr ? 0 : 1;
            }
        }

        /// <summary>
        /// Gets the address numbering notation.
        /// </summary>
        private string AddrNotation
        {
            get
            {
                return DecAddr ? "DEC" : "HEX";
            }
        }

        /// <summary>
        /// Gets or sets a reference to the device template options.
        /// </summary>
        public DeviceTemplateOptions TemplateOptions { get; set; }

        /// <summary>
        /// Gets or sets the element group for editing.
        /// </summary>
        public ElemGroupConfig ElemGroup
        {
            get
            {
                return elemGroup;
            }
            set
            {
                elemGroup = null; // to avoid ObjectChanged event
                ShowElemGroupConfig(value);
                elemGroup = value;
            }
        }


        /// <summary>
        /// Shows the element group configuration.
        /// </summary>
        private void ShowElemGroupConfig(ElemGroupConfig elemGroup)
        {
            numGrAddress.Minimum = AddrShift;
            numGrAddress.Maximum = ushort.MaxValue + AddrShift;
            numGrAddress.Value = 1;
            numGrAddress.Hexadecimal = !DecAddr;
            ShowFuncCode(elemGroup);

            if (elemGroup == null)
            {
                chkGrActive.Checked = false;
                txtGrName.Text = "";
                cbGrDataBlock.SelectedIndex = 0;
                numGrAddress.SetValue(AddrShift);
                lblGrAddressHint.Text = "";
                numGrElemCnt.Value = 1;
                gbElemGroup.Enabled = false;
            }
            else
            {
                chkGrActive.Checked = elemGroup.Active;
                txtGrName.Text = elemGroup.Name;
                cbGrDataBlock.SelectedIndex = (int)elemGroup.DataBlock;
                numGrAddress.SetValue(elemGroup.Address + AddrShift);
                lblGrAddressHint.Text = string.Format(DriverPhrases.AddressHint, AddrNotation, AddrShift);
                numGrElemCnt.Maximum = elemGroup.MaxElemCnt;
                numGrElemCnt.SetValue(elemGroup.Elems.Count);
                gbElemGroup.Enabled = true;
            }
        }

        /// <summary>
        /// Shows the function code of the element group.
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
                txtGrFuncCode.Text = string.Format("{0} ({1}h)", funcCode, funcCode.ToString("X2"));
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(elemGroup, changeArgument));
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        public void SetFocus()
        {
            txtGrName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void chkGrActive_CheckedChanged(object sender, EventArgs e)
        {
            // update group activity
            if (elemGroup != null)
            {
                elemGroup.Active = chkGrActive.Checked;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtGrName_TextChanged(object sender, EventArgs e)
        {
            // update group name
            if (elemGroup != null)
            {
                elemGroup.Name = txtGrName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void cbGrDataBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update group data block
            if (elemGroup != null)
            {
                DataBlock dataBlock = (DataBlock)cbGrDataBlock.SelectedIndex;
                int maxElemCnt = elemGroup.GetMaxElemCnt(dataBlock);

                bool cancel = elemGroup.Elems.Count > maxElemCnt &&
                    MessageBox.Show(
                        string.Format(DriverPhrases.ElemRemoveWarning, maxElemCnt),
                        CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question) != DialogResult.Yes;

                if (cancel)
                {
                    // revert selected item
                    cbGrDataBlock.SelectedIndexChanged -= cbGrDataBlock_SelectedIndexChanged;
                    cbGrDataBlock.SelectedIndex = (int)elemGroup.DataBlock;
                    cbGrDataBlock.SelectedIndexChanged += cbGrDataBlock_SelectedIndexChanged;
                }
                else
                {
                    // set data block
                    elemGroup.DataBlock = dataBlock;
                    ShowFuncCode(elemGroup);

                    // limit element count
                    numGrElemCnt.Maximum = maxElemCnt;

                    // reset type of elements
                    ElemType elemType = elemGroup.DefaultElemType;
                    elemGroup.Elems.ForEach(elem => elem.ElemType = elemType);

                    OnObjectChanged(TreeUpdateTypes.CurrentNode | TreeUpdateTypes.ChildNodes);
                }
            }
        }

        private void numGrAddress_ValueChanged(object sender, EventArgs e)
        {
            // update group address
            if (elemGroup != null)
            {
                elemGroup.Address = (ushort)(numGrAddress.Value - AddrShift);
                OnObjectChanged(TreeUpdateTypes.ChildNodes);
            }
        }

        private void numGrElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // update number of elements
            if (elemGroup != null)
            {
                int oldElemCnt = elemGroup.Elems.Count;
                int newElemCnt = (int)numGrElemCnt.Value;

                if (oldElemCnt < newElemCnt)
                {
                    // add new elements
                    for (int elemInd = oldElemCnt; elemInd < newElemCnt; elemInd++)
                    {
                        ElemConfig elem = elemGroup.CreateElemConfig();
                        elemGroup.Elems.Add(elem);
                    }
                }
                else if (oldElemCnt > newElemCnt)
                {
                    // remove redundant elements
                    for (int i = newElemCnt; i < oldElemCnt; i++)
                    {
                        elemGroup.Elems.RemoveAt(newElemCnt);
                    }
                }

                OnObjectChanged(TreeUpdateTypes.ChildCount);
            }
        }
    }
}
