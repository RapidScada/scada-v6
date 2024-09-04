// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtExternalTools.Config;

namespace Scada.Admin.Extensions.ExtExternalTools.Forms
{
    /// <summary>
    /// Represents a form for editing an extension configuration.
    /// <para>Представляет форму для редактирования конфигурации расширения.</para>
    /// </summary>
    public partial class FrmExtensionConfig : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmExtensionConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal FrmExtensionConfig(ExtensionConfig extensionConfig)
            : this()
        {

        }


        private void FrmExtensionConfig_Load(object sender, EventArgs e)
        {

        }

        private void lvTool_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }


        private void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtArguments_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtWorkingDirectory_TextChanged(object sender, EventArgs e)
        {

        }


        private void btnBrowseFile_Click(object sender, EventArgs e)
        {

        }

        private void btnAddArgument_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowseDir_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
