// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Forms;
using Scada.Web.Config;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    /// <summary>
    /// Represents a control for editing general line options.
    /// <para>Представляет элемент управления для редактирования основных параметров.</para>
    /// </summary>
    public partial class CtrlGeneralOptions : UserControl
    {
        /// <summary>
        /// Reprensents a culture information.
        /// </summary>
        private class Culture
        {
            public string DisplayInfo { get; set; }
            public CultureInfo CultureInfo { get; set; }
        }
        
         
        private IAdminContext adminContext;      // the Administrator context
        private WebApp webApp;                   // the web application in a project
        private bool changing;                   // controls are being changed programmatically
        private List<Culture> cultures;          // the list of culture info 

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlGeneralOptions()
        {
            InitializeComponent();
            adminContext = null;
            webApp = null;
            changing = false;
            cultures = null;
        }


        /// <summary>
        /// Raises an OptionsChanged event.
        /// </summary>
        private void OnOptionsChanged()
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
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

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(IAdminContext adminContext, WebApp webApp)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.webApp = webApp ?? throw new ArgumentNullException(nameof(webApp));

            cbDefaultTimeZone.Items.Clear();
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

            foreach (TimeZoneInfo timeZone in timeZones)
            {
                cbDefaultTimeZone.Items.Add(timeZone);
            }

            cbDefaultCulture.Items.Clear();

            cultures = new();

            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                Culture culture = new()
                {
                    CultureInfo = cultureInfo,
                    DisplayInfo = cultureInfo.Name + ", " + cultureInfo.DisplayName
                };
                
                cultures.Add(culture);
            }

            changing = true;
            cbDefaultCulture.DataSource = cultures;
            cbDefaultCulture.DisplayMember = "DisplayInfo";
        }

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        public void OptionsToControls(GeneralOptions generalOptions)
        {
            ArgumentNullException.ThrowIfNull(generalOptions, nameof(generalOptions));
            changing = true;
                       
            bool prCulture = false;

            foreach (Culture culture in cultures)
            {
                if (culture.CultureInfo.Name == generalOptions.DefaultCulture)
                {
                    cbDefaultCulture.SelectedItem = culture;
                    prCulture = true;
                    break;
                }
            }
            
            if (!prCulture)
                cbDefaultCulture.Text = generalOptions.DefaultCulture;


            bool prTime = false;
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            
            foreach (TimeZoneInfo timeZone in timeZones)
            {
                if (timeZone.Id == generalOptions.DefaultTimeZone)
                {
                    cbDefaultTimeZone.SelectedItem = timeZone;
                    prTime = true;
                    break;
                }
            }

            if (!prTime)
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

            generalOptions.DefaultCulture =  ((Culture)cbDefaultCulture.SelectedItem) != null ?
                ((Culture)cbDefaultCulture.SelectedItem).CultureInfo.Name : cbDefaultCulture.Text;

            generalOptions.DefaultTimeZone = ((TimeZoneInfo)cbDefaultTimeZone.SelectedItem) != null ? 
                ((TimeZoneInfo)cbDefaultTimeZone.SelectedItem).Id : cbDefaultTimeZone.Text;
          
            generalOptions.DefaultStartPage = txtDefaultStartPage.Text;
            generalOptions.EnableCommands = chkEnableCommands.Checked;
            generalOptions.ShareStats = chkShareStats.Checked;
            generalOptions.MaxLogSize = Convert.ToInt32(numMaxLogSize.Value);
        }
    }
}
