// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Log;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimic.Config;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Manages mimic diagrams being edited.
    /// <para>Управляет редактируемыми мнемосхемами.</para>
    /// </summary>
    public class EditorManager
    {
        private readonly IWebContext webContext; // the web application context


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EditorManager(IWebContext webContext)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));

            EditorConfig = new EditorConfig();
            MimicPluginConfig = new MimicPluginConfig();
            PluginLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(webContext.AppDirs.LogDir, EditorUtils.LogFileName),
                CapacityMB = webContext.AppConfig.GeneralOptions.MaxLogSize
            };
        }


        /// <summary>
        /// Gets the configuration of the editor plugin.
        /// </summary>
        public EditorConfig EditorConfig { get; }

        /// <summary>
        /// Gets the configuration of the mimic plugin.
        /// </summary>
        public MimicPluginConfig MimicPluginConfig { get; }

        /// <summary>
        /// Gets the plugin log.
        /// </summary>
        public ILog PluginLog { get; }


        /// <summary>
        /// Loads the configuration of the editor and mimic plugins.
        /// </summary>
        public void LoadConfig()
        {
            if (!EditorConfig.Load(webContext.Storage, EditorConfig.DefaultFileName, out string errMsg))
            {
                PluginLog.WriteError(errMsg);
                webContext.Log.WriteError(WebPhrases.PluginMessage, EditorPluginInfo.PluginCode, errMsg);
            }

            if (!MimicPluginConfig.Load(webContext.Storage, MimicPluginConfig.DefaultFileName, out errMsg))
            {
                PluginLog.WriteError(errMsg);
                webContext.Log.WriteError(WebPhrases.PluginMessage, EditorPluginInfo.PluginCode, errMsg);
            }
        }

        /// <summary>
        /// Opens a mimic diagram from the specified file.
        /// </summary>
        public OpenResult OpenMimic(string fileName)
        {
            return new OpenResult
            {
                IsSuccessful = false,
                ErrorMessage = "Not implemented"
            };
        }
    }
}
