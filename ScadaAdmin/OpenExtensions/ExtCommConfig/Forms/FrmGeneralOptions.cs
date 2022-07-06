// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Forms;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for editing general options.
    /// <para>Форма для редактирования основных параметров.</para>
    /// </summary>
    public partial class FrmGeneralOptions : Form, IChildForm
    {
        private readonly ILog log;              // the application log
        private readonly CommApp commApp;       // the Communicator application in a project
        private readonly CommConfig commConfig; // the Communicator configuration
        private bool changing;                  // controls are being changed programmatically

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmGeneralOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmGeneralOptions(ILog log, CommApp commApp)
            : this()
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.commApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
            commConfig = commApp.AppConfig;
            changing = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            changing = true;

            GeneralOptions generalOptions = commConfig.GeneralOptions;
            chkIsBound.Checked = generalOptions.IsBound;
            chkSendModifiedData.Checked = generalOptions.SendModifiedData;
            numSendAllDataPeriod.SetValue(generalOptions.SendAllDataPeriod);
            chkEnableCommands.Checked = generalOptions.EnableCommands;
            chkEnableFileCommands.Checked = generalOptions.EnableFileCommands;
            chkStartLinesOnCommand.Checked = generalOptions.StartLinesOnCommand;
            numMaxLogSize.SetValue(generalOptions.MaxLogSize);

            ctrlClientConnection.ConnectionOptions = commConfig.ConnectionOptions.DeepClone();
            changing = false;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfing()
        {
            GeneralOptions generalOptions = commConfig.GeneralOptions;
            generalOptions.IsBound = chkIsBound.Checked;
            generalOptions.SendModifiedData = chkSendModifiedData.Checked;
            generalOptions.SendAllDataPeriod = decimal.ToInt32(numSendAllDataPeriod.Value);
            generalOptions.EnableCommands = chkEnableCommands.Checked;
            generalOptions.EnableFileCommands = chkEnableFileCommands.Checked;
            generalOptions.StartLinesOnCommand = chkStartLinesOnCommand.Checked;
            generalOptions.MaxLogSize = decimal.ToInt32(numMaxLogSize.Value);

            ctrlClientConnection.ConnectionOptions.CopyTo(commConfig.ConnectionOptions);
        }

        /// <summary>
        /// Saves the changes of the child form data.
        /// </summary>
        public void Save()
        {
            ControlsToConfing();

            if (commApp.SaveConfig(out string errMsg))
                ChildFormTag.Modified = false;
            else
                log.HandleError(errMsg);
        }


        private void FrmGeneralOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlClientConnection, ctrlClientConnection.GetType().FullName);
            ConfigToControls();
        }
        
        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                ChildFormTag.Modified = true;
        }
    }
}
