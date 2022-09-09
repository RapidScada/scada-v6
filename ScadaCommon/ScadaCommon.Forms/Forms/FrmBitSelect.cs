// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Windows.Forms;

namespace Scada.Forms.Forms
{
    /// <summary>
    /// Represents a form for selecting a single bit.
    /// <para>Представляет форму для выбора одного бита.</para>
    /// </summary>
    public partial class FrmBitSelect : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBitSelect()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the selected bit.
        /// </summary>
        public int SelectedBit { get; set; }

        /// <summary>
        /// Gets or sets the available bits.
        /// </summary>
        /// <remarks>This is a collection of BitItem or objects of any type.</remarks>
        public object Bits { get; set; }


        /// <summary>
        /// Shows the list of bits.
        /// </summary>
        private void ShowBits()
        {
            if (Bits is IEnumerable bits)
            {
                try
                {
                    lbBits.BeginUpdate();
                    lbBits.ItemCheck -= lbBits_ItemCheck;
                    int index = 0;
                    bool found = false;

                    foreach (object bitObj in bits)
                    {
                        if (bitObj != null)
                        {
                            BitItem bitItem = bitObj as BitItem ?? new BitItem(index, bitObj.ToString());
                            lbBits.Items.Add(bitItem);

                            if (bitItem.Bit == SelectedBit)
                            {
                                lbBits.SetItemChecked(index, true);
                                found = true;
                            }
                        }

                        index++;
                    }

                    btnOK.Enabled = found;
                }
                finally
                {
                    lbBits.EndUpdate();
                    lbBits.ItemCheck += lbBits_ItemCheck;
                }
            }
        }

        /// <summary>
        /// Deselects the list items.
        /// </summary>
        private void DeselectItems(int excludingIndex)
        {
            lbBits.ItemCheck -= lbBits_ItemCheck;

            for (int i = 0, cnt = lbBits.Items.Count; i < cnt; i++)
            {
                if (i != excludingIndex)
                    lbBits.SetItemChecked(i, false);
            }

            lbBits.ItemCheck += lbBits_ItemCheck;
        }


        private void FrmBitSelect_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ShowBits();
        }

        private void lbBits_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                BitItem bitItem = (BitItem)lbBits.Items[e.Index];
                SelectedBit = bitItem.Bit;
                DeselectItems(e.Index);
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lbBits.CheckedItems.Count > 0)
                DialogResult = DialogResult.OK;
        }
    }
}
