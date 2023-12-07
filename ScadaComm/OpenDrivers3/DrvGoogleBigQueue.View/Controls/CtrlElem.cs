// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvGoogleBigQueue.Config;
using Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol;
using Scada.Forms;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View.Controls
{
    /// <summary>
    /// Represents a control for editing a Modbus element.
    /// </summary>
    public partial class CtrlElem : UserControl
    {
        private ElemTag elemTag; // the element metadata


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlElem()
        {
            InitializeComponent();
            elemTag = null;
        }


        /// <summary>
        /// Gets or sets the element for editing.
        /// </summary>
        public ElemTag ElemTag
        {
            get
            {
                return elemTag;
            }
            set
            {
                elemTag = null; // to avoid ObjectChanged event
                ShowElemConfig(value);
                elemTag = value;
            }
        }


        /// <summary>
        /// Shows the element configuration.
        /// </summary>
        private void ShowElemConfig(ElemTag elemTag)
        {
            if (elemTag == null)
            {
                txtElemName.Text = "";
                txtElemTagCode.Text = "";
                pnlElemTagCodeWarn.Visible = false;
                txtElemTagNum.Text = "";
                rbBool.Checked = true;
                gbElem.Enabled = false;
            }
            else
            {
                ElemGroupConfig elemGroup = elemTag.ElemGroup;
                ElemConfig elem = elemTag.Elem;

                txtElemName.Text = elem.Name;
                txtElemTagCode.Text = elem.TagCode;
                pnlElemTagCodeWarn.Visible = string.IsNullOrEmpty(elem.TagCode);
                txtElemTagNum.Text = elemTag.TagNum.ToString();

                rbUShort.Enabled = rbShort.Enabled = rbUInt.Enabled = rbInt.Enabled =
                    rbULong.Enabled = rbLong.Enabled = rbFloat.Enabled = rbDouble.Enabled = rbBool.Enabled = true;

                switch (elem.ElemType)
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
                    case ElemType.Bool:
                        rbBool.Checked = true;
                        break;
                }
                gbElem.Enabled = true;
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(elemTag, changeArgument));
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        public void SetFocus()
        {
            txtElemName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void txtElemName_TextChanged(object sender, EventArgs e)
        {
            // update element name
            if (elemTag != null)
            {
                bool updateTagCode = elemTag.Elem.Name == elemTag.Elem.TagCode;
                elemTag.Elem.Name = txtElemName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);

                if (updateTagCode)
                    txtElemTagCode.Text = txtElemName.Text;
            }
        }

        private void txtElemTagCode_TextChanged(object sender, EventArgs e)
        {
            // update tag code
            if (elemTag != null)
            {
                elemTag.Elem.TagCode = txtElemTagCode.Text;
                pnlElemTagCodeWarn.Visible = txtElemTagCode.Text == "";
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void rbType_CheckedChanged(object sender, EventArgs e)
        {
            // update element type
            if (elemTag != null && ((RadioButton)sender).Checked)
            {
                ElemType elemType;

                if (rbUShort.Checked)
                    elemType = ElemType.UShort;
                else if (rbShort.Checked)
                    elemType = ElemType.Short;
                else if (rbUInt.Checked)
                    elemType = ElemType.UInt;
                else if (rbInt.Checked)
                    elemType = ElemType.Int;
                else if (rbULong.Checked)
                    elemType = ElemType.ULong;
                else if (rbLong.Checked)
                    elemType = ElemType.Long;
                else if (rbFloat.Checked)
                    elemType = ElemType.Float;
                else if (rbDouble.Checked)
                    elemType = ElemType.Double;
                else
                    elemType = ElemType.Bool;

                elemTag.Elem.ElemType = elemType;
                OnObjectChanged(TreeUpdateTypes.CurrentNode | TreeUpdateTypes.NextSiblings);
            }
        }
    }
}
