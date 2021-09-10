// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Forms;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    /// <summary>
    /// Represents a form for editing archives.
    /// <para>Представляет форму для редактирования архивов.</para>
    /// </summary>
    public partial class FrmArchives : Form
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly ServerApp serverApp;        // the server application in a project
        private readonly ServerConfig serverConfig;  // the server configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmArchives()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmArchives(IAdminContext adminContext, ServerApp serverApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.serverApp = serverApp ?? throw new ArgumentNullException(nameof(serverApp));
            serverConfig = serverApp.Config;
        }


        /// <summary>
        /// Fills the combo box that contains archive kinds.
        /// </summary>
        private void FillKindComboBox()
        {
            cbKind.BeginUpdate();
            cbKind.Items.Add(ServerPhrases.UnspecifiedArchiveKind);
            cbKind.Items.Add(ServerPhrases.CurrentArchiveKind);
            cbKind.Items.Add(ServerPhrases.HistoricalArchiveKind);
            cbKind.Items.Add(ServerPhrases.EventsArchiveKind);
            cbKind.EndUpdate();
        }

        /// <summary>
        /// Fills the combo box by the modules that support archives.
        /// </summary>
        private void FillModuleComboBox()
        {
            try
            {
                cbModule.BeginUpdate();
                cbModule.Items.Clear();
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);

                foreach (FileInfo fileInfo in
                    dirInfo.EnumerateFiles("ModArc*.View.dll", SearchOption.TopDirectoryOnly))
                {
                    cbModule.Items.Add(ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name));
                }
            }
            finally
            {
                cbModule.EndUpdate();
            }
        }
        
        /// <summary>
        /// Creates a new list view item that represents an archive.
        /// </summary>
        private ListViewItem CreateArchiveItem(ArchiveConfig archiveConfig, ref int index)
        {
            return new ListViewItem(new string[]
            {
                (++index).ToString(),
                archiveConfig.Active ? "V" : " ", 
                archiveConfig.Code,
                archiveConfig.Name,
                TranslateArchiveKind(archiveConfig.Kind),
                archiveConfig.Module
            })
            {
                Tag = archiveConfig
            };
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            try
            {
                lvArchive.BeginUpdate();
                lvArchive.Items.Clear();
                int index = 0;

                foreach (ArchiveConfig archiveConfig in serverConfig.Archives)
                {
                    lvArchive.Items.Add(CreateArchiveItem(archiveConfig.DeepClone(), ref index));
                }

                if (lvArchive.Items.Count > 0)
                    lvArchive.Items[0].Selected = true;
            }
            finally
            {
                lvArchive.EndUpdate();
            }
        }

        /// <summary>
        /// Translates the specified archive kind.
        /// </summary>
        private static string TranslateArchiveKind(ArchiveKind archiveKind)
        {
            return archiveKind switch
            {
                ArchiveKind.Current => ServerPhrases.CurrentArchiveKind,
                ArchiveKind.Historical => ServerPhrases.HistoricalArchiveKind,
                ArchiveKind.Events => ServerPhrases.EventsArchiveKind,
                _ => ServerPhrases.UnspecifiedArchiveKind
            };
        }


        private void FrmArchives_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillKindComboBox();
            FillModuleComboBox();
            ConfigToControls();
        }
    }
}
