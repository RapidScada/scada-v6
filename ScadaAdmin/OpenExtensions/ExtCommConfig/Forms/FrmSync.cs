// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public FrmSync()
        {
            InitializeComponent();

            ctrlSync2.Visible = false;
            ctrlSync2.Top = ctrlSync1.Top;
            btnNext.Left = btnSync.Left;
            btnSync.Visible = false;
        }

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
