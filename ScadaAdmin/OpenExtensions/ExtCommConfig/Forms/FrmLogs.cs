// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for displaying logs.
    /// <para>Представляет форму для отображения журналов.</para>
    /// </summary>
    public partial class FrmLogs : Form
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly RemoteLogBox logBox;        // updates log


        public FrmLogs()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLogs(IAdminContext adminContext)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            logBox = new RemoteLogBox(lbLog) { AutoScroll = true };
        }


        private void FrmLogs_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            cbFilter.SelectedIndex = 0;
        }

        private void FrmLogs_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FrmLogs_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbFiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            lbFiles.DrawTabItem(e);
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkPause_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {

        }
    }
}
