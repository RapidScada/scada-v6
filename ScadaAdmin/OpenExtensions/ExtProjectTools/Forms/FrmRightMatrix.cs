// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Properties;
using Scada.Admin.Project;
using Scada.Forms;
using WinControls;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents a right matrix form.
    /// <para>Представляет форму матрицы прав.</para>
    /// </summary>
    public partial class FrmRightMatrix : Form, IChildForm
    {
        private readonly IAdminContext adminContext;    // the application context
        private readonly ConfigDatabase configDatabase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmRightMatrix()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmRightMatrix(IAdminContext adminContext, ConfigDatabase configDatabase)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));

            ChildFormTag = new ChildFormTag(new ChildFormOptions { Image = Resources.matrix });
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AdminMessage.BaseReload)
            {
                //
            }
        }

        private void FrmRightMatrix_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
        }
    }
}
