// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvOpcUa.View.Forms
{
    public partial class FrmEditingOptions : Form
    {
        private readonly EditingOptions editingOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmEditingOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmEditingOptions(EditingOptions editingOptions)
            : this()
        {
            this.editingOptions = editingOptions ?? throw new ArgumentNullException(nameof(editingOptions));
        }


        private void FrmEditingOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            cbDefaultTagCode.SelectedIndex = (int)editingOptions.DefaultTagCode;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            editingOptions.DefaultTagCode = (DefaultTagCode)cbDefaultTagCode.SelectedIndex;
            DialogResult = DialogResult.OK;
        }
    }
}
