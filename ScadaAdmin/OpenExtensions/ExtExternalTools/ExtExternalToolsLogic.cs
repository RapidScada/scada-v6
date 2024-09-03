// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtExternalTools.Config;
using Scada.Admin.Lang;
using Scada.Lang;
using System.Diagnostics;

namespace Scada.Admin.Extensions.ExtExternalTools
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtExternalToolsLogic : ExtensionLogic
    {
        private readonly ExtensionConfig extensionConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtExternalToolsLogic(IAdminContext adminContext)
            : base(adminContext)
        {
            extensionConfig = new ExtensionConfig();
        }


        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "ExtExternalTools";
            }
        }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Внешние инструменты" : "External Tools";
            }
        }

        /// <summary>
        /// Gets the extension description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Расширение предназначено для вызова внешних программ." :
                    "The extension is designed for calling external programs.";
            }
        }


        /// <summary>
        /// Handles the menu item click.
        /// </summary>
        private void ToolMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && 
                menuItem.Tag is ToolItemConfig itemConfig)
            {
                // call external program
                ProcessStartInfo startInfo = new()
                {
                    FileName = itemConfig.FileName,
                    Arguments = itemConfig.Arguments,
                    WorkingDirectory = itemConfig.WorkingDirectory,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AdminContext.AppDirs.LangDir, Code, out string errMsg))
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);

            ExtensionPhrases.Init();
        }

        /// <summary>
        /// Loads the extension configuration.
        /// </summary>
        public override void LoadConfig()
        {
            string fileName = Path.Combine(AdminContext.AppDirs.ConfigDir, ExtensionConfig.DefaultFileName);

            if (File.Exists(fileName) &&
                !extensionConfig.Load(fileName, out string errMsg))
            {
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);
            }
        }

        /// <summary>
        /// Gets menu items to add to the main menu.
        /// </summary>
        public override ToolStripItem[] GetMainMenuItems()
        {
            if (extensionConfig.ToolItems.Count > 0)
            {
                ToolStripMenuItem parentMenuItem = new(ExtensionPhrases.ParentMenuItem);

                foreach (ToolItemConfig toolItemConfig in extensionConfig.ToolItems)
                {
                    ToolStripMenuItem toolMenuItem = new(toolItemConfig.Title) { Tag = toolItemConfig };
                    toolMenuItem.Click += ToolMenuItem_Click;
                    parentMenuItem.DropDownItems.Add(toolMenuItem);
                }

                return [parentMenuItem];
            }
            else
            {
                return null;
            }
        }
    }
}
