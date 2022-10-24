// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Web.Config;
using System.Globalization;

namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    /// <summary>
    /// Represents a control for editing general options.
    /// <para>Представляет элемент управления для редактирования основных параметров.</para>
    /// </summary>
    public partial class CtrlGeneralOptions : UserControl
    {
        /// <summary>
        /// Reprensents a culture information.
        /// </summary>
        private class CultureItem
        {
            public CultureInfo CultureInfo { get; set; }
            public override string ToString()
            {
                return CultureInfo == null
                    ? ""
                    : CultureInfo.Name + ", " + CultureInfo.DisplayName;
            }
        }

        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlGeneralOptions()
        {
            InitializeComponent();
            changing = false;
        }


        /// <summary>
        /// Raises an OptionsChanged event.
        /// </summary>
        private void OnOptionsChanged()
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Fills the combo box by time zone.
        /// </summary>
        private void FillTimeZoneComboBox()
        {
            try
            {
                cbDefaultTimeZone.BeginUpdate();
                cbDefaultTimeZone.Items.Clear();
                
                foreach (TimeZoneInfo timeZone in TimeZoneInfo.GetSystemTimeZones())
                {
                    cbDefaultTimeZone.Items.Add(timeZone);
                }
            }
            finally
            {
                cbDefaultTimeZone.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the combo box by culture info .
        /// </summary>
        private void FillCutureInfoComboBox()
        {
            try
            {
                cbDefaultCulture.BeginUpdate();
                cbDefaultCulture.Items.Clear();
                
                foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                {
                    cbDefaultCulture.Items.Add(new CultureItem { CultureInfo = cultureInfo });
                }
            }
            finally
            {
                cbDefaultCulture.EndUpdate();
            }
        }


        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init()
        {
            FillTimeZoneComboBox();
            FillCutureInfoComboBox();
        }

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        public void OptionsToControls(GeneralOptions generalOptions)
        {
            ArgumentNullException.ThrowIfNull(generalOptions, nameof(generalOptions));
            changing = true;
                       
            bool cultureFound = false;

            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                if (cultureInfo.Name == generalOptions.DefaultCulture)
                {
                    cbDefaultCulture.Text = new CultureItem { CultureInfo = cultureInfo }.ToString() ;
                    cultureFound = true;
                    break;
                }
            }

            if (!cultureFound)
                cbDefaultCulture.Text = generalOptions.DefaultCulture;

            bool timeFound = false;
            
            foreach (TimeZoneInfo timeZone in TimeZoneInfo.GetSystemTimeZones())
            {
                if (timeZone.Id == generalOptions.DefaultTimeZone)
                {
                    cbDefaultTimeZone.SelectedItem = timeZone;
                    timeFound = true;
                    break;
                }
            }

            if (!timeFound)
                cbDefaultTimeZone.Text = generalOptions.DefaultTimeZone;

            txtDefaultStartPage.Text = generalOptions.DefaultStartPage;
            chkEnableCommands.Checked = generalOptions.EnableCommands;
            chkShareStats.Checked = generalOptions.ShareStats;
            numMaxLogSize.SetValue(generalOptions.MaxLogSize);

            changing = false;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        public void ControlsToOptions(GeneralOptions generalOptions)
        {
            ArgumentNullException.ThrowIfNull(generalOptions, nameof(generalOptions));

            generalOptions.DefaultCulture = cbDefaultCulture.SelectedItem is CultureItem cultureItem ?
                cultureItem.CultureInfo.Name : cbDefaultCulture.Text;

            generalOptions.DefaultTimeZone = cbDefaultTimeZone.SelectedItem is TimeZoneInfo timeZoneInfo ?
                 timeZoneInfo.Id : cbDefaultTimeZone.Text;
            
            generalOptions.DefaultStartPage = txtDefaultStartPage.Text;
            generalOptions.EnableCommands = chkEnableCommands.Checked;
            generalOptions.ShareStats = chkShareStats.Checked;
            generalOptions.MaxLogSize = Convert.ToInt32(numMaxLogSize.Value);
        }


        /// <summary>
        /// Occurs when the options change.
        /// </summary>
        public event EventHandler OptionsChanged;


        private void CtrlGeneralOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
        }

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                OnOptionsChanged();
        }
    }
}
