// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Forms.Forms
{
    /// <summary>
    /// Represents a form for editing a bitmask.
    /// <para>Представляет форму для редактирования битовой маски.</para>
    /// </summary>
    public partial class FrmBitmask : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBitmask()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the bit mask value.
        /// </summary>
        public int MaskValue
        {
            get
            {
                return ctrlBitmask.MaskValue;
            }
            set
            {
                ctrlBitmask.MaskValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the available mask bits.
        /// </summary>
        /// <remarks>This is a collection of BitItem or objects of any type.</remarks>
        public object MaskBits
        {
            get
            {
                return ctrlBitmask.MaskBits;
            }
            set
            {
                ctrlBitmask.MaskBits = value;
            }
        }


        private void FrmBitmask_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlBitmask, ctrlBitmask.GetType().FullName);

            ctrlBitmask.ShowMask();
            ctrlBitmask.SetFocus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
