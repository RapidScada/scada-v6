// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Properties;
using Scada.Admin.Project;
using Scada.Forms;
using WinControls;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents an object editor form.
    /// <para>Представляет форму редактора проектов.</para>
    /// </summary>
    public partial class FrmObjectEditor : Form, IChildForm
    {
        private readonly IAdminContext adminContext;
        private readonly ConfigDatabase configDatabase;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmObjectEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmObjectEditor(IAdminContext adminContext, ConfigDatabase configDatabase)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            ChildFormTag = new ChildFormTag(new ChildFormOptions { Image = Resources.objects });
            IsClosed = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }

        /// <summary>
        /// Gets a value indicating whether the form has been closed.
        /// </summary>
        public bool IsClosed { get; private set; }


        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {

        }


        private void FrmObjectEditor_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
        }

        private void FrmObjectEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsClosed = true;
        }
    }
}
