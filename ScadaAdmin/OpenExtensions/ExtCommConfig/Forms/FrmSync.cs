// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for synchronizing properties of communication lines and devices.
    /// <para>Представляет форму для синхронизации свойств линий связи и устройств.</para>
    /// </summary>
    public partial class FrmSync : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmSync()
        {
            InitializeComponent();

            ctrlSync2.Visible = false;
            ctrlSync2.Top = ctrlSync1.Top;
            btnNext.Left = btnSync.Left;
            btnSync.Visible = false;
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSync(IAdminContext adminContext, CommConfig commConfig)
            : this()
        {
            SelectedLineNum = 0;
        }


        /// <summary>
        /// Gets or sets the selected line number.
        /// </summary>
        public int SelectedLineNum { get; set; }

        /// <summary>
        /// Gets a value indicating whether the sync is performed from the configuration database to Communicator.
        /// </summary>
        public bool BaseToComm { get; }

        /// <summary>
        /// Gets a value indicating whether a communication line or device has been added to 
        /// the Communicator configuration.
        /// </summary>
        public bool AddedToComm { get; }


        private void FrmSync_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlSync1, ctrlSync1.GetType().FullName);
            FormTranslator.Translate(ctrlSync2, ctrlSync2.GetType().FullName);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ctrlSync1.Visible = false;
            ctrlSync2.Visible = true;
            btnNext.Visible = false;
            btnSync.Visible = true;
        }

        private void btnSync_Click(object sender, EventArgs e)
        {

        }
    }
}
