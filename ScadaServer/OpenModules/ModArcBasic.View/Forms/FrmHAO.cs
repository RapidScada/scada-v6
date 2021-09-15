// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Config;
using System;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    /// <summary>
    /// Represents a form for editing historical archive options.
    /// <para>Представляет форму для редактирования параметров архива исторических данных.</para>
    /// </summary>
    public partial class FrmHAO : Form
    {
        private readonly BasicHAO options; // the archive options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmHAO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmHAO(ArchiveConfig archiveConfig)
            : this()
        {
            if (archiveConfig == null)
                throw new ArgumentNullException(nameof(archiveConfig));

            options = new BasicHAO(archiveConfig.CustomOptions);
        }

        private void FrmHAO_Load(object sender, EventArgs e)
        {
        }
    }
}
