// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtServerConfig.Code;
using Scada.Admin.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    /// <summary>
    /// Represents a form for displaying Server logs.
    /// <para>Представляет форму для отображения журналов Сервера.</para>
    /// </summary>
    public class FrmServerLogs : FrmLogs, IChildForm
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmServerLogs(IAdminContext adminContext)
            : base(adminContext)
        {
            ServiceApp = ServiceApp.Server;
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
                    SearchPattern = "ScadaServer.*"
                },
                new FilterItem
                {
                    Name = ExtensionPhrases.ModulesFilter,
                    SearchPattern = "Mod*.*"
                },
                new FilterItem
                {
                    Name = ExtensionPhrases.AllFilesFilter,
                    SearchPattern = "*"
                }
            });
            
            FilterComboBox.SelectedIndex = 0;
        }
    }
}
