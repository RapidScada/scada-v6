// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Forms.Forms
{
    /// <summary>
    /// Represents a form for editing a numeric range.
    /// <para>Представляет форму для редактирования числового диапазона.</para>
    /// </summary>
    public partial class FrmRangeEdit : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmRangeEdit()
        {
            InitializeComponent();

            Range = null;
            AllowEmpty = false;
            DefaultValue = 0;
        }


        /// <summary>
        /// Gets or sets the range to edit.
        /// </summary>
        public ICollection<int> Range { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an ampty range is allowed.
        /// </summary>
        public bool AllowEmpty { get; set; }

        /// <summary>
        /// Gets or sets the default value if an ampty range is not allowed.
        /// </summary>
        public int DefaultValue { get; set; }


        private void FrmRangeEdit_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            Range ??= new List<int>();
            txtRange.Text = Range.ToRangeString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ScadaUtils.ParseRange(txtRange.Text, true, true, out IList<int> newRange))
            {
                // fill edited range
                Range.Clear();

                foreach (int val in newRange)
                {
                    Range.Add(val);
                }

                if (Range.Count == 0 && !AllowEmpty)
                    Range.Add(DefaultValue);

                DialogResult = DialogResult.OK;
            }
            else
            {
                ScadaUiUtils.ShowError(CommonPhrases.ValidRangeRequired);
            }
        }
    }
}
