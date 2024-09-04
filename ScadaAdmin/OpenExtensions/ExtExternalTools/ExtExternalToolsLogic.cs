// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Config;
using Scada.Admin.Extensions.ExtExternalTools.Config;
using Scada.Admin.Extensions.ExtExternalTools.Forms;
using Scada.Admin.Lang;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using System.Diagnostics;
using System.Text;
using WinControls;

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
            CanShowProperties = true;
        }


        /// <summary>
        /// Gets the full name of the extension configuration file.
        /// </summary>
        private string ConfigFileName =>
            Path.Combine(AdminContext.AppDirs.ConfigDir, ExtensionConfig.DefaultFileName);

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
                if (!File.Exists(itemConfig.FileName))
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.ExecutableNotFound);
                }
                else if (!GetArguments(itemConfig.Arguments, out string args, out string errMsg))
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
                else
                {
                    // call external program
                    try
                    {
                        ProcessStartInfo startInfo = new()
                        {
                            FileName = itemConfig.FileName,
                            Arguments = args,
                            WorkingDirectory = itemConfig.WorkingDirectory,
                            UseShellExecute = true
                        };
                        Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        AdminContext.ErrLog.HandleError(ex, ExtensionPhrases.StartToolError);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the arguments from the string that may contain variables.
        /// </summary>
        private bool GetArguments(string s, out string args, out string errMsg)
        {
            if (string.IsNullOrEmpty(s))
            {
                args = "";
                errMsg = "";
                return true;
            }

            StringBuilder sbArgs = new();
            bool undefArgs = false;
            int startIdx = 0;

            void AppendVar(string value)
            {
                if (string.IsNullOrEmpty(value))
                    undefArgs = true;
                else
                    sbArgs.Append(value);
            }

            while (startIdx < s.Length && !undefArgs)
            {
                int braceIdx1 = s.IndexOf('{', startIdx);
                int braceIdx2 = braceIdx1 < 0 ? -1 : s.IndexOf('}', braceIdx1 + 1);

                if (braceIdx1 >= 0 && braceIdx2 >= 0 && braceIdx2 - braceIdx1 > 1)
                {
                    sbArgs.Append(s[startIdx..braceIdx1]);
                    startIdx = braceIdx2 + 1;
                    string varName = s[(braceIdx1 + 1)..braceIdx2];

                    if (varName == VarName.ProjectFileName)
                    {
                        AppendVar(AdminContext.CurrentProject?.FileName);
                    }
                    else if (varName == VarName.ItemFileName)
                    {
                        AppendVar((AdminContext.MainForm.ActiveChildForm as IChildForm)?
                            .ChildFormTag?.Options?.FileName);
                    }
                    else
                    {
                        sbArgs.Append(s[braceIdx1..(braceIdx2 + 1)]);
                    }
                }
                else
                {
                    sbArgs.Append(s[startIdx..]);
                    break;
                }
            }

            args = sbArgs.ToString();

            if (undefArgs)
            {
                errMsg = ExtensionPhrases.UndefinedArgs;
                return false;
            }
            else
            {
                errMsg = "";
                return true;
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
            string fileName = ConfigFileName;

            if (File.Exists(fileName) &&
                !extensionConfig.Load(fileName, out string errMsg))
            {
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);
            }
        }

        /// <summary>
        /// Shows a modal dialog box for editing extension properties.
        /// </summary>
        public override void ShowProperties(AdminConfig adminConfig)
        {
            new FrmExtensionConfig(ConfigFileName).ShowDialog();
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
