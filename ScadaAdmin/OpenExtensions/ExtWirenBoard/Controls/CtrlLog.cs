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

namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    /// <summary>
    /// Represents a control for displaying a log.
    /// <para>Представляет элемент управления для отображения журнала.</para>
    /// </summary>
    public partial class CtrlLog : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the rich text box control.
        /// </summary>
        public RichTextBox RichTextBox => txtLog;

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            txtLog.Select();
        }
    }
}
