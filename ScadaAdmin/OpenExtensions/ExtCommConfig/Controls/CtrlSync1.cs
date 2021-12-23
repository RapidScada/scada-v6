// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for selecting a sync direction.
    /// <para>Представляет элемент управления для выбора направления синхронизации.</para>
    /// </summary>
    public partial class CtrlSync1 : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlSync1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets a value indicating whether the sync is performed from the configuration database to Communicator.
        /// </summary>
        public bool BaseToComm => rbBaseToComm.Checked;
    }
}
