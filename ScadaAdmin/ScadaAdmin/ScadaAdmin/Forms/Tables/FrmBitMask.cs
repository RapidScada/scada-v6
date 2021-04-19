/*
 * Copyright 2021 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Represents a form for editing a bit mask
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Forms;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a form for editing a bit mask.
    /// <para>Представляет форму для редактирования битовой маски.</para>
    /// </summary>
    public partial class FrmBitMask : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBitMask()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the bit mask value.
        /// </summary>
        public int MaskValue { get; set; }

        /// <summary>
        /// Gets or sets the available mask bits.
        /// </summary>
        public object MaskBits { get; set; }


        /// <summary>
        /// Shows the list of bits.
        /// </summary>
        private void ShowBits()
        {
            if (MaskBits is IEnumerable maskBits)
            {
                foreach (object bitObj in maskBits)
                {
                    if (bitObj is BitItem bitItem)
                    {
                        checkedListBox.Items.Add(bitItem);
                    }
                }
            }
        }

        private void FrmBitMask_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            txtValue.Text = MaskValue.ToString();
            ShowBits();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
