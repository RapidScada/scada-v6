// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Forms;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvModbus.View.Controls
{
    /// <summary>
    /// Represetns a control for editing a command.
    /// <para>Представляет элемент управления для редактирования команды.</para>
    /// </summary>
    public partial class CtrlCmd : UserControl
    {
        private CmdConfig cmd; // the command configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCmd()
        {
            InitializeComponent();
            cmd = null;
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
        /// Gets or sets the command for editing.
        /// </summary>
        public CmdConfig Cmd
        {
            get
            {
                return cmd;
            }
            set
            {
                cmd = null; // to avoid ObjectChanged event
                ShowCmdConfig(value);
                cmd = value;
            }
        }


        /// <summary>
        /// Shows the command configuration.
        /// </summary>
        private void ShowCmdConfig(CmdConfig cmd)
        {
            numCmdAddress.Value = 1;
            numCmdAddress.Minimum = AddrShift;
            numCmdAddress.Maximum = ushort.MaxValue + AddrShift;
            numCmdAddress.Hexadecimal = !DecAddr;
            ShowFuncCode(cmd);
            ShowByteOrder(cmd);
            ShowScaling(cmd);

            if (cmd == null)
            {
                txtCmdName.Text = "";
                txtCmdCode.Text = "";
                pnlCmdCodeWarn.Visible = false;
                numCmdNum.Value = 0;
                cbCmdDataBlock.SelectedIndex = 0;
                chkCmdMultiple.Checked = false;
                numCmdAddress.Value = AddrShift;
                lblCmdAddressHint.Text = "";
                cbCmdElemType.SelectedIndex = 0;
                numCmdElemCnt.Value = 1;
                pnlCmdElem.Visible = true;
                gbCmd.Enabled = false;
            }
            else
            {
                txtCmdName.Text = cmd.Name;
                txtCmdCode.Text = cmd.CmdCode;
                pnlCmdCodeWarn.Visible = string.IsNullOrEmpty(cmd.CmdCode);
                numCmdNum.SetValue(cmd.CmdNum);
                cbCmdDataBlock.SelectedIndex = cmd.DataBlock switch
                {
                    DataBlock.Coils => 0,
                    DataBlock.HoldingRegisters => 1,
                    _ => 2
                };

                if (cmd.DataBlock == DataBlock.Custom)
                {
                    chkCmdMultiple.Checked = false;
                    chkCmdMultiple.Enabled = false;
                    pnlCmdElem.Visible = false;
                }
                else
                {
                    chkCmdMultiple.Checked = cmd.Multiple;
                    chkCmdMultiple.Enabled = true;
                    numCmdAddress.SetValue(cmd.Address + AddrShift);
                    lblCmdAddressHint.Text = string.Format(DriverPhrases.AddressHint, AddrNotation, AddrShift);
                    cbCmdElemType.SelectedIndex = (int)cmd.ElemType;
                    cbCmdElemType.Enabled = cmd.ElemTypeEnabled;
                    numCmdElemCnt.Maximum = cmd.MaxElemCnt;
                    numCmdElemCnt.SetValue(cmd.ElemCnt);
                    numCmdElemCnt.Enabled = cmd.Multiple;
                    pnlCmdElem.Visible = true;
                }

                gbCmd.Enabled = true;
            }
        }

        /// <summary>
        /// Shows the function code of the command.
        /// </summary>
        private void ShowFuncCode(CmdConfig cmd)
        {
            if (cmd == null)
            {
                numCmdFuncCode.Value = 0;
                numCmdFuncCode.ReadOnly = true;
            }
            else if (cmd.DataBlock == DataBlock.Custom)
            {
                numCmdFuncCode.Value = (byte)cmd.CustomFuncCode;
                numCmdFuncCode.ReadOnly = false;
            }
            else
            {
                numCmdFuncCode.Value = ModbusUtils.GetWriteFuncCode(cmd.DataBlock, cmd.Multiple);
                numCmdFuncCode.ReadOnly = true;
            }
        }

        /// <summary>
        /// Shows the byte order of the command.
        /// </summary>
        private void ShowByteOrder(CmdConfig cmd)
        {
            if (cmd != null && cmd.ByteOrderEnabled)
            {
                txtCmdByteOrder.Text = cmd.ByteOrder;
                txtCmdByteOrder.Enabled = true;
            }
            else
            {
                txtCmdByteOrder.Text = "";
                txtCmdByteOrder.Enabled = false;
            }
        }

        /// <summary>
        /// Shows the scaling of the command.
        /// </summary>
        private void ShowScaling(CmdConfig cmd)
        {
            if (cmd != null && cmd.ScalingEnabled)
            {
                txtCmdScaling.Text = cmd.Scaling;
                txtCmdScaling.Enabled = true;
            }
            else
            {
                txtCmdScaling.Text = "";
                txtCmdScaling.Enabled = false;
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(cmd, changeArgument));
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        public void SetFocus()
        {
            txtCmdName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void txtCmdName_TextChanged(object sender, EventArgs e)
        {
            // update command name
            if (cmd != null)
            {
                bool updateCmdCode = cmd.Name == cmd.CmdCode;
                cmd.Name = txtCmdName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);

                if (updateCmdCode)
                    txtCmdCode.Text = txtCmdName.Text;
            }
        }

        private void txtCmdCode_TextChanged(object sender, EventArgs e)
        {
            // update command code
            if (cmd != null)
            {
                cmd.CmdCode = txtCmdCode.Text;
                pnlCmdCodeWarn.Visible = txtCmdCode.Text == "";
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numCmdNum_ValueChanged(object sender, EventArgs e)
        {
            // update command number
            if (cmd != null)
            {
                cmd.CmdNum = (int)numCmdNum.Value;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void cbCmdDataBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update data block
            if (cmd != null)
            {
                cmd.DataBlock = cbCmdDataBlock.SelectedIndex switch
                {
                    0 => DataBlock.Coils,
                    1 => DataBlock.HoldingRegisters,
                    _ => DataBlock.Custom
                };

                ShowFuncCode(cmd);
                ShowByteOrder(cmd);
                ShowScaling(cmd);
                OnObjectChanged(TreeUpdateTypes.CurrentNode);

                if (cmd.DataBlock == DataBlock.Custom)
                {
                    chkCmdMultiple.Checked = false;
                    chkCmdMultiple.Enabled = false;
                    pnlCmdElem.Visible = false;
                }
                else
                {
                    chkCmdMultiple.Enabled = true;
                    pnlCmdElem.Visible = true;
                }

                cbCmdElemType.SelectedIndex = (int)cmd.DefaultElemType;
                cbCmdElemType.Enabled = cmd.ElemTypeEnabled;
                numCmdElemCnt.Maximum = cmd.MaxElemCnt;
            }
        }

        private void chkCmdMultiple_CheckedChanged(object sender, EventArgs e)
        {
            // update multiple
            if (cmd != null)
            {
                cmd.Multiple = chkCmdMultiple.Checked;
                ShowFuncCode(cmd);
                ShowByteOrder(cmd);
                ShowScaling(cmd);
                OnObjectChanged(TreeUpdateTypes.None);

                cbCmdElemType.SelectedIndex = (int)cmd.DefaultElemType;
                cbCmdElemType.Enabled = cmd.ElemTypeEnabled;

                if (cmd.Multiple)
                {
                    numCmdElemCnt.Enabled = true;
                }
                else
                {
                    numCmdElemCnt.Value = 1;
                    numCmdElemCnt.Enabled = false;
                }
            }
        }

        private void numCmdFuncCode_ValueChanged(object sender, EventArgs e)
        {
            // update function code
            int funcCode = Convert.ToInt32(numCmdFuncCode.Value);
            txtCmdFuncCodeHex.Text = funcCode.ToString("X2") + 'h';

            if (cmd != null)
            {
                cmd.CustomFuncCode = funcCode;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numCmdAddress_ValueChanged(object sender, EventArgs e)
        {
            // update address
            if (cmd != null)
            {
                cmd.Address = (ushort)(numCmdAddress.Value - AddrShift);
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void cbCmdElemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update element type
            if (cmd != null)
            {
                ElemType newElemType = (ElemType)cbCmdElemType.SelectedIndex;

                if (cmd.DataBlock == DataBlock.HoldingRegisters && newElemType == ElemType.Bool)
                {
                    // cancel the Bool type selection
                    cbCmdElemType.SelectedIndexChanged -= cbCmdElemType_SelectedIndexChanged;
                    cbCmdElemType.SelectedIndex = (int)cmd.ElemType;
                    cbCmdElemType.SelectedIndexChanged += cbCmdElemType_SelectedIndexChanged;
                }
                else
                {
                    cmd.ElemType = newElemType;
                    OnObjectChanged(TreeUpdateTypes.CurrentNode);
                }
            }
        }

        private void numCmdElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // update element count
            if (cmd != null)
            {
                cmd.ElemCnt = (int)numCmdElemCnt.Value;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtCmdByteOrder_TextChanged(object sender, EventArgs e)
        {
            // update byte order
            if (cmd != null)
            {
                cmd.ByteOrder = txtCmdByteOrder.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtCmdScaling_TextChanged(object sender, EventArgs e)
        {
            // update scaling
            if (cmd != null)
            {
                cmd.Scaling = txtCmdScaling.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
