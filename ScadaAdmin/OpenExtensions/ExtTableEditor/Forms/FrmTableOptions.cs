// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtTableEditor.Forms
{
    /// <summary>
    /// Represents a form for editing table view options.
    /// <para>Представляет форму для редактирования параметров табличного представления.</para>
    /// </summary>
    public partial class FrmTableOptions : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableOptions()
        {
            InitializeComponent();
        }

        private void chkUseDefault_CheckedChanged(object sender, EventArgs e)
        {
            pnlOptions.Enabled = !chkUseDefault.Checked;
        }
    }
}
