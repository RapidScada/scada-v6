// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Forms;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Drivers;
using Scada.Forms;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for editing a list of Communicator modules.
    /// <para>Представляет форму для редактирования списка драйверов Коммуникатора.</para>
    /// </summary>
    public partial class FrmDrivers : Form, IChildForm
    {
        /// <summary>
        /// Reprensents a driver.
        /// </summary>
        private class DriverItem
        {
            public bool IsInitialized { get; set; }
            public string DriverCode { get; set; }
            public string FileName { get; set; }
            public string Descr { get; set; }
            public DriverView DriverView { get; set; }
            public override string ToString() => DriverCode;
        }


        private readonly IAdminContext adminContext; // the Administrator context
        private readonly CommApp commApp;            // the Communicator application in a project


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDrivers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDrivers(IAdminContext adminContext, CommApp commApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.commApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Fills the list of available drivers.
        /// </summary>
        private void FillDriverList()
        {
            try
            {
                lbDrivers.BeginUpdate();
                lbDrivers.Items.Clear();
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);

                if (dirInfo.Exists)
                {
                    foreach (FileInfo fileInfo in
                        dirInfo.EnumerateFiles("Drv*.View.dll", SearchOption.TopDirectoryOnly))
                    {
                        lbDrivers.Items.Add(new DriverItem
                        {
                            IsInitialized = false,
                            DriverCode = ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name),
                            FileName = fileInfo.FullName
                        });
                    }
                }

                if (lbDrivers.Items.Count > 0)
                    lbDrivers.SelectedIndex = 0;
            }
            finally
            {
                lbDrivers.EndUpdate();
            }
        }

        /// <summary>
        /// Initializes the driver item if needed.
        /// </summary>
        private void InitDriverItem(DriverItem driverItem)
        {
            if (!driverItem.IsInitialized)
            {
                driverItem.IsInitialized = true;

                if (ExtensionUtils.GetDriverView(adminContext, commApp, driverItem.DriverCode,
                    out DriverView driverView, out string message))
                {
                    driverItem.Descr = BuildDriverDescr(driverView);
                    driverItem.DriverView = driverView;
                }
                else
                {
                    driverItem.Descr = message;
                    driverItem.DriverView = null;
                }
            }
        }

        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            if (lbDrivers.SelectedItem is DriverItem driverItem)
            {
                btnProperties.Enabled = driverItem.DriverView != null && driverItem.DriverView.CanShowProperties;
                btnRegister.Visible = driverItem.DriverView != null && driverItem.DriverView.RequireRegistration;
            }
            else
            {
                btnProperties.Enabled = false;
                btnRegister.Visible = false;
            }
        }

        /// <summary>
        /// Build the driver description.
        /// </summary>
        private static string BuildDriverDescr(DriverView driverView)
        {
            string title = string.Format("{0} {1}",
                driverView.Name,
                driverView.GetType().Assembly.GetName().Version);

            return new StringBuilder()
                .AppendLine(title)
                .AppendLine(new string('-', title.Length))
                .Append(driverView.Descr?.Replace("\n", Environment.NewLine))
                .ToString();
        }

        /// <summary>
        /// Saves the changes of the child form data.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void FrmDrivers_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillDriverList();
            SetButtonsEnabled();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            // show properties of the selected driver
            if (lbDrivers.SelectedItem is DriverItem driverItem && 
                driverItem.DriverView != null && driverItem.DriverView.CanShowProperties)
            {
                lbDrivers.Focus();
                driverItem.DriverView.ShowProperties();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // show registration form for the selected driver
            if (lbDrivers.SelectedItem is DriverItem driverItem &&
                driverItem.DriverView != null && driverItem.DriverView.RequireRegistration)
            {
                lbDrivers.Focus();
                new FrmRegistration(adminContext, commApp,
                    driverItem.DriverView.ProductCode, driverItem.DriverView.Name).ShowDialog();
            }
        }

        private void lbDrivers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display driver description
            if (lbDrivers.SelectedItem is DriverItem driverItem)
            {
                InitDriverItem(driverItem);
                txtDescr.Text = driverItem.Descr;
            }

            SetButtonsEnabled();
        }

        private void lbDrivers_DoubleClick(object sender, EventArgs e)
        {
            btnProperties_Click(null, null);
        }
    }
}
