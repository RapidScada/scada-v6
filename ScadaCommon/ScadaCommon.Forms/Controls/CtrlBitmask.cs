// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Windows.Forms;

namespace Scada.Forms.Controls
{
    /// <summary>
    /// Represents a control for editing a bitmask.
    /// <para>Представляет элемент управления для редактирования битовой маски.</para>
    /// </summary>
    public partial class CtrlBitmask : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlBitmask()
        {
            InitializeComponent();

            MaskValue = 0;
            MaskBits = null;
        }


        /// <summary>
        /// Gets or sets the bit mask value.
        /// </summary>
        public int MaskValue { get; set; }

        /// <summary>
        /// Gets or sets the available mask bits.
        /// </summary>
        /// <remarks>This is a collection of BitItem or objects of any type.</remarks>
        public object MaskBits { get; set; }


        /// <summary>
        /// Shows the decimal value of the mask.
        /// </summary>
        private void ShowMaskValue()
        {
            txtMaskValue.Text = MaskValue.ToString();
        }

        /// <summary>
        /// Shows the list of bits.
        /// </summary>
        private void ShowBits()
        {
            if (MaskBits is IEnumerable maskBits)
            {
                try
                {
                    lbMaskBits.BeginUpdate();
                    lbMaskBits.ItemCheck -= lbMaskBits_ItemCheck;
                    int index = 0;

                    foreach (object bitObj in maskBits)
                    {
                        if (bitObj != null)
                        {
                            BitItem bitItem = bitObj as BitItem ??
                                new BitItem(index, bitObj.ToString());
                            lbMaskBits.Items.Add(bitItem);

                            if (MaskValue.BitIsSet(bitItem.Bit))
                                lbMaskBits.SetItemChecked(index, true);
                        }

                        index++;
                    }
                }
                finally
                {
                    lbMaskBits.EndUpdate();
                    lbMaskBits.ItemCheck += lbMaskBits_ItemCheck;
                }
            }
        }

        /// <summary>
        /// Shows the specified bit mask.
        /// </summary>
        public void ShowMask()
        {
            ShowMaskValue();
            ShowBits();
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            lbMaskBits.Select();
        }


        private void btnResetMask_Click(object sender, EventArgs e)
        {
            MaskValue = 0;
            ShowMaskValue();
            lbMaskBits.ItemCheck -= lbMaskBits_ItemCheck;

            for (int i = 0, cnt = lbMaskBits.Items.Count; i < cnt; i++)
            {
                lbMaskBits.SetItemChecked(i, false);
            }

            lbMaskBits.ItemCheck += lbMaskBits_ItemCheck;
        }

        private void lbMaskBits_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            BitItem bitItem = (BitItem)lbMaskBits.Items[e.Index];
            MaskValue = MaskValue.SetBit(bitItem.Bit, e.NewValue == CheckState.Checked);
            ShowMaskValue();
        }
    }
}
