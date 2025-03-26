// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Config;
using Scada.Admin.Extensions.ExtMimicLauncher.Config;
using Scada.Admin.Lang;
using Scada.ComponentModel;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using System.Net;

namespace Scada.Admin.Extensions.ExtMimicLauncher
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtMimicLauncherLogic : ExtensionLogic
    {
        private readonly ExtensionConfig extensionConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtMimicLauncherLogic(IAdminContext adminContext)
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
                return "ExtMimicLauncher";
            }
        }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Запуск редактора мнемосхем" : "Mimic Editor Launcher";
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
                    "Расширение открывает мнемосхемы для редактирования в браузере." :
                    "The extension open mimics for editing in a browser.";
            }
        }

        /// <summary>
        /// Gets the file extensions that the extension can open.
        /// </summary>
        public override ICollection<string> FileExtensions => ["mim"];


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AdminContext.AppDirs.LangDir, Code, out string errMsg))
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);

            ExtensionPhrases.Init();
            AttrTranslator.Translate(typeof(ExtensionConfig));
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
            FrmOptions frmOptions = new() { Options = extensionConfig };

            if (frmOptions.ShowDialog() == DialogResult.OK &&
                !extensionConfig.Save(ConfigFileName, out string errMsg))
            {
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        /// <summary>
        /// Opens the specified file.
        /// </summary>
        public override OpenFileResult OpenFile(string fileName)
        {
            try
            {
                Uri uri = new(
                    extensionConfig.WebUrl.TrimEnd('/') + 
                    "/MimicEditor/MimicOpen?fileName=" +
                    WebUtility.UrlEncode(fileName));
                string absUri = uri.AbsoluteUri;

                switch (extensionConfig.Browser)
                {
                    case Browser.Chrome:
                        ScadaUiUtils.StartProcess("chrome", absUri);
                        break;

                    case Browser.Edge:
                        ScadaUiUtils.StartProcess("msedge", absUri);
                        break;

                    case Browser.Firefox:
                        ScadaUiUtils.StartProcess("firefox", absUri);
                        break;

                    default:
                        ScadaUiUtils.StartProcess(absUri);
                        break;
                }
            }
            catch (Exception ex)
            {
                string errMsg = ex.BuildErrorMessage(ExtensionPhrases.OpenMimicError);
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }

            return new OpenFileResult { Handled = true };
        }
    }
}
