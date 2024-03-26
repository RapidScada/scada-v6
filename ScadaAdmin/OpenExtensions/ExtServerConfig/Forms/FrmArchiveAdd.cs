// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Forms;
using Scada.Server.Config;
using System.Collections;

namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    /// <summary>
    /// Represents a form for adding an archive.
    /// <para>Представляет форму для добавления архива.</para>
    /// </summary>
    public partial class FrmArchiveAdd : Form
    {
        private readonly ScadaProject project; // the project under development


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmArchiveAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmArchiveAdd(ScadaProject project)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            ArchiveConfig = null;
        }


        /// <summary>
        /// Gets the archive configuration to add to the project.
        /// </summary>
        public ArchiveConfig ArchiveConfig { get; private set; }

        /// <summary>
        /// Gets or sets the items of the module list.
        /// </summary>
        public IEnumerable ModuleItems { get; set; }


        /// <summary>
        /// Fills the combo box with the archives.
        /// </summary>
        private void FillArchiveList()
        {
            Archive emptyItem = new() { ArchiveID = 0, Name = " " };
            List<Archive> archives = new(project.ConfigDatabase.ArchiveTable.ItemCount + 1) { emptyItem };
            archives.AddRange(project.ConfigDatabase.ArchiveTable.Enumerate().OrderBy(a => a.Name));

            cbSourceArchive.ValueMember = "project";
            cbSourceArchive.DisplayMember = "Name";
            cbSourceArchive.DataSource = archives;
            cbSourceArchive.SelectedValue = 0;
        }

        /// <summary>
        /// Fills the combo box with the modules.
        /// </summary>
        private void FillModuleList()
        {
            if (ModuleItems != null)
            {
                foreach (object item in ModuleItems)
                {
                    cbModule.Items.Add(item);
                }
            }
        }


        private void FrmArchiveAdd_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillArchiveList();
            FillModuleList();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ArchiveConfig = new()
            {
                Active = true,
                Module = cbModule.Text
            };

            if (cbSourceArchive.SelectedValue is Archive archive && 
                archive.ArchiveID > 0)
            {
                ArchiveConfig.Code = archive.Code;
                ArchiveConfig.Name = archive.Name;
                ArchiveConfig.Kind = (Server.Archives.ArchiveKind)(archive.ArchiveKindID ?? 0);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
