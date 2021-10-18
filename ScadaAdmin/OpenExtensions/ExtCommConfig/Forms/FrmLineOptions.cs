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
    /// Represents a form for editing communication line options.
    /// <para>Форма для редактирования параметров линии связи.</para>
    /// </summary>
    public partial class FrmLineOptions : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLineOptions()
        {
            InitializeComponent();
        }


        private void FrmLineOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

        }

        private void lbTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            lbTabs.DrawTabItem(e);
        }

        private void lbTabs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
