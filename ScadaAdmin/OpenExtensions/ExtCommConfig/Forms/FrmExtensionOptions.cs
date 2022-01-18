// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Config;
using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for editing extension options.
    /// <para>Форма для редактирования параметров расширения.</para>
    /// </summary>
    public partial class FrmExtensionOptions : Form
    {
        private readonly OptionList optionList;
        private readonly ChannelWizardOptions options;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmExtensionOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmExtensionOptions(OptionList optionList)
            : this()
        {
            this.optionList = optionList ?? throw new ArgumentNullException(nameof(optionList));
            options = new ChannelWizardOptions(optionList);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            numMultiplicity.SetValue(options.Multiplicity);
            numShift.SetValue(options.Shift);
            numGap.SetValue(options.Gap);
            chkPrependDeviceName.Checked = options.PrependDeviceName;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.Multiplicity = Convert.ToInt32(numMultiplicity.Value);
            options.Shift = Convert.ToInt32(numShift.Value);
            options.Gap = Convert.ToInt32(numGap.Value);
            options.PrependDeviceName = chkPrependDeviceName.Checked;
            options.AddToOptionList(optionList);
        }


        private void FrmExtensionOptions_Load(object sender, EventArgs e)
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
