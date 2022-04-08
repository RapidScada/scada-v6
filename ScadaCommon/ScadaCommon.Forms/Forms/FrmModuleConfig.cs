// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Windows.Forms;

namespace Scada.Forms.Forms
{
    /// <summary>
    /// Represents a universal form for editing module configuration.
    /// <para>Представляет универсальную форму для редактирования конфигурации модуля.</para>
    /// </summary>
    public partial class FrmModuleConfig : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmModuleConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmModuleConfig(ModuleConfigProvider moduleConfigProvider)
            : this()
        {
        }


        private void FrmModuleConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
        }
    }
}
