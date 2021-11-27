// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for displaying Communicator logs.
    /// <para>Представляет форму для отображения журналов Коммуникатора.</para>
    /// </summary>
    public class FrmCommLogs : FrmLogs, IChildForm
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCommLogs(IAdminContext adminContext)
            : base(adminContext)
        {
            ServiceApp = ServiceApp.Comm;
        }

        /// <summary>
        /// Fills the file filter and selects the default item.
        /// </summary>
        protected override void FillFilter()
        {
            FilterComboBox.Items.AddRange(new FilterItem[] 
            {
                new FilterItem
                {
                    Name = ExtensionPhrases.AppFilter,
                    SearchPattern = "ScadaComm.*"
                },
                new FilterItem
                {
                    Name = ExtensionPhrases.LinesFilter,
                    SearchPattern = "line*.*"
                },
                new FilterItem
                {
                    Name = ExtensionPhrases.DevicesFilter,
                    SearchPattern = "device*.*"
                }
            });

            base.FillFilter();
        }
    }
}
