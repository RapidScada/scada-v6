// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Web.Config;


namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    /// <summary>
    /// Represents a control for editing plugin assigment.
    /// <para>Представляет элемент управления для редактирования назначения подключаемого модуля.</para>
    /// </summary>
    public partial class CtrlPluginAssignment : UserControl
    {
        private IAdminContext adminContext; // the Administrator context
        private bool changing;              // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlPluginAssignment()
        {
            InitializeComponent();
            adminContext = null;
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
        /// Fills the combo box by the plugins.
        /// </summary>
        private void FillPluginComboBox(ComboBox comboBox)
        {
            try
            {
                comboBox.BeginUpdate();
                comboBox.Items.Clear();
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);
                
                foreach (FileInfo fileInfo in
                    dirInfo.EnumerateFiles("Plg*.View.dll", SearchOption.TopDirectoryOnly))
                {
                    comboBox.Items.Add(ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name));
                }
            }
            finally
            {
                comboBox.EndUpdate();
            }
        }


        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(IAdminContext adminContext)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            FillPluginComboBox(cbChartFeature);
            FillPluginComboBox(cbCommandFeature);
            FillPluginComboBox(cbEventAckFeature);
            FillPluginComboBox(cbUserManagementFeature);
            FillPluginComboBox(cbNotificationFeature);
        }

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        public void OptionsToControls(PluginAssignment pluginAssignment)
        {
            ArgumentNullException.ThrowIfNull(pluginAssignment, nameof(pluginAssignment));
            changing = true;

            cbChartFeature.Text = pluginAssignment.ChartFeature;
            cbCommandFeature.Text = pluginAssignment.CommandFeature;
            cbEventAckFeature.Text = pluginAssignment.EventAckFeature;
            cbUserManagementFeature.Text = pluginAssignment.UserManagementFeature;
            cbNotificationFeature.Text = pluginAssignment.NotificationFeature;

            changing = false;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        public void ControlsToOptions(PluginAssignment pluginAssignment)
        {
            ArgumentNullException.ThrowIfNull(pluginAssignment, nameof(pluginAssignment));

            pluginAssignment.ChartFeature = cbChartFeature.Text;
            pluginAssignment.CommandFeature = cbCommandFeature.Text;
            pluginAssignment.EventAckFeature = cbEventAckFeature.Text;
            pluginAssignment.UserManagementFeature = cbUserManagementFeature.Text;
            pluginAssignment.NotificationFeature = cbNotificationFeature.Text;
        }


        /// <summary>
        /// Occurs when the options change.
        /// </summary>
        public event EventHandler OptionsChanged;


        private void CtrlPluginAssignment_Load(object sender, EventArgs e)
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
