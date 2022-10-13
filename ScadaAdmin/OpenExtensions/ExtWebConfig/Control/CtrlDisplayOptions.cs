// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Forms;
using Scada.Web.Config;

namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    /// <summary>
    /// Represents a control for editing main display line options.
    /// <para>Представляет элемент управления для редактирования основных параметров отображения.</para>
    /// </summary>
    public partial class CtrlDisplayOptions : UserControl
    {
        private IAdminContext adminContext;      // the Administrator context
        private WebApp webApp;                   // the web application in a project
        private bool changing;                   // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlDisplayOptions()
        {
            InitializeComponent();
            adminContext = null;
            webApp = null;
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
        /// Occurs when the options change.
        /// </summary>
        public event EventHandler OptionsChanged;


        private void CtrlDisplayOptions_Load(object sender, EventArgs e)
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
        }

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        public void OptionsToControls(DisplayOptions displayOptions)
        {
            ArgumentNullException.ThrowIfNull(displayOptions, nameof(displayOptions));
            changing = true;

            chkShowHeader.Checked = displayOptions.ShowHeader;
            chkShowMainMenu.Checked = displayOptions.ShowMainMenu;
            chkShowViewExplorer.Checked = displayOptions.ShowViewExplorer;
            numRefreshRate.SetValue(displayOptions.RefreshRate);            
            
            changing = false;
        }

        /// <summary>
        /// Sets the configuration according to the options.
        /// </summary>
        public void ControlsToOptions(DisplayOptions displayOptions)
        {
            ArgumentNullException.ThrowIfNull(displayOptions, nameof(displayOptions));

            displayOptions.ShowHeader = chkShowHeader.Checked;
            displayOptions.ShowMainMenu = chkShowMainMenu.Checked;
            displayOptions.ShowViewExplorer = chkShowViewExplorer.Checked;
            displayOptions.RefreshRate = Convert.ToInt32(numRefreshRate.Value);
        }
    }
}
