// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code;
using Scada.Admin.Extensions.ExtWirenBoard.Controls;
using Scada.Admin.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtWirenBoard.Forms
{
    /// <summary>
    /// Represents a wizard form for creating a project configuration.
    /// <para>Представляет форму мастера для создания конфигурации проекта.</para>
    /// </summary>
    public partial class FrmWirenBoardWizard : Form
    {
        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected parameters

        private readonly CtrlLineSelect ctrlLineSelect;
        private readonly CtrlLog ctrlLog;
        private readonly CtrlTopicTree ctrlTopicTree;
        private readonly CtrlEntityID ctrlEntityID;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmWirenBoardWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmWirenBoardWizard(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));

            ctrlLineSelect = new CtrlLineSelect { Dock = DockStyle.Fill, Visible = false };
            ctrlLog = new CtrlLog { Dock = DockStyle.Fill, Visible = false };
            ctrlTopicTree = new CtrlTopicTree { Dock = DockStyle.Fill, Visible = false };
            ctrlEntityID = new CtrlEntityID { Dock = DockStyle.Fill, Visible = false };

            pnlMain.Controls.Add(ctrlLineSelect);
            pnlMain.Controls.Add(ctrlLog);
            pnlMain.Controls.Add(ctrlTopicTree);
            pnlMain.Controls.Add(ctrlEntityID);
        }


        private void FrmWirenBoardWizard_Load(object sender, EventArgs e)
        {
            ctrlLineSelect.Visible = true;
        }
    }
}
