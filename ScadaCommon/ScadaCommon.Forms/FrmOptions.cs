// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// Represents a form for editing options.
    /// <para>Представляет форму для редактирования параметров.</para>
    /// </summary>
    public partial class FrmOptions : Form
    {
        private object options; // the options to edit


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmOptions()
        {
            InitializeComponent();
            options = null;
        }


        /// <summary>
        /// Gets or sets the options to edit.
        /// </summary>
        public object Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
            }
        }


        private void FrmOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            //propertyGrid.SelectedObject = options;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
