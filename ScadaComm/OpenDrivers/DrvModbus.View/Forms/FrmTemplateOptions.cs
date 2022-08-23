// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvModbus.View.Forms
{
    /// <summary>
    /// Represents a form for editing device template options.
    /// <para>Представляет форму для редактирования параметров шаблона устройства.</para>
    /// </summary>
    public partial class FrmTemplateOptions : Form
    {
        private readonly DeviceTemplateOptions options; // the device template options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTemplateOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTemplateOptions(DeviceTemplateOptions options)
            : this()
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            if (options.ZeroAddr)
                rbZeroBased.Checked = true;
            else
                rbOneBased.Checked = true;

            if (options.DecAddr)
                rbDec.Checked = true;
            else
                rbHex.Checked = true;

            txtDefByteOrder2.Text = options.DefByteOrder2;
            txtDefByteOrder4.Text = options.DefByteOrder4;
            txtDefByteOrder8.Text = options.DefByteOrder8;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.ZeroAddr = rbZeroBased.Checked;
            options.DecAddr = rbDec.Checked;
            options.DefByteOrder2 = txtDefByteOrder2.Text;
            options.DefByteOrder4 = txtDefByteOrder4.Text;
            options.DefByteOrder8 = txtDefByteOrder8.Text;
        }


        private void FrmTemplateSettings_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            OptionsToControls();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
