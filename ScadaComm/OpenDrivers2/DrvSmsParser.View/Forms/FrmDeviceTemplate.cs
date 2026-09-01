// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;

namespace Scada.Comm.Drivers.DrvSmsParser.View.Forms
{
    /// <summary>
    /// Represents a form for editing a device template.
    /// <para>Представляет форму для редактирования шаблона устройства.</para>
    /// </summary>
    public partial class FrmDeviceTemplate : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceTemplate()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the full file name of the template.
        /// </summary>
        public string FileName { get; set; }


        private void FrmDeviceTemplate_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
