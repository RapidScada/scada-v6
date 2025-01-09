﻿/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : ScadaSchemeCommon
 * Summary  : Form for editing range of integers
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Form for editing range of integers.
    /// <para>Форма редактирования диапазона целых чисел.</para>
    /// </summary>
    internal partial class FrmRangeDialog : Form
    {
        private ICollection<int> range; // the edited range


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmRangeDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmRangeDialog(ICollection<int> range)
            : this()
        {
            this.range = range ?? throw new ArgumentNullException("range");
        }


        private void FrmRangeEdit_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            txtRange.Text = RangeUtils.RangeToStr(range);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (RangeUtils.StrToRange(txtRange.Text, true, true, out ICollection<int> newRange))
            {
                range.Clear();

                foreach (int val in newRange)
                {
                    range.Add(val);
                }

                DialogResult = DialogResult.OK;
            }
            else
            {
                ScadaUiUtils.ShowError(SchemePhrases.RangeNotValid);
            }
        }
    }
}
