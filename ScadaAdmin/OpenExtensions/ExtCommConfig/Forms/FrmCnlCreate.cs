// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for creating channels.
    /// <para>Представляет форму для создания каналов.</para>
    /// </summary>
    public partial class FrmCnlCreate : Form
    {
        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlCreate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlCreate(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));
        }


        private void FrmCnlCreate_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlCnlCreate1, ctrlCnlCreate1.GetType().FullName);
            FormTranslator.Translate(ctrlCnlCreate2, ctrlCnlCreate2.GetType().FullName);

            ctrlCnlCreate1.Init(adminContext, project, recentSelection);
            ctrlCnlCreate2.Init(project, recentSelection);
        }
    }
}
