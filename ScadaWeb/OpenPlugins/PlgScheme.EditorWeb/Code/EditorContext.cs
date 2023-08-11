// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Lang;
using Scada.Log;
using System.Reflection;

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// Contains application level data.
    /// <para>Содержит данные уровня приложения.</para>
    /// </summary>
    public class EditorContext
    {
        private readonly LogFile logFile; // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EditorContext()
        {
            logFile = new LogFile(LogFormat.Full);

            InstanceConfig = new InstanceConfig();
            AppDirs = new AppDirs { Lowercase = true };
            AppConfig = new EditorConfig(AppDirs);
            Manager = new EditorManager(AppConfig, Log);

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Gets the instance configuration.
        /// </summary>
        public InstanceConfig InstanceConfig { get; }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public EditorConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log => logFile;

        /// <summary>
        /// Gets the editor manager.
        /// </summary>
        public EditorManager Manager { get; }


        /// <summary>
        /// Writes information about the unhandled exception to the log.
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Log.WriteError(args.ExceptionObject as Exception, CommonPhrases.UnhandledException);
        }

        /// <summary>
        /// Loads the instance configuration.
        /// </summary>
        private void LoadInstanceConfig()
        {
            Locale.SetCultureToEnglish();

            if (InstanceConfig.Load(InstanceConfig.GetConfigFileName(AppDirs.InstanceDir), out string errMsg))
            {
                Locale.SetCulture(InstanceConfig.Culture);
                AppDirs.UpdateLogDir(InstanceConfig.LogDir);
            }
            else
            {
                Console.WriteLine(errMsg);
                Locale.SetCultureToDefault();
                AppDirs.CreateLogDir();
            }
        }

        /// <summary>
        /// Loads the application configuration.
        /// </summary>
        private void LoadAppConfig()
        {
            if (!AppConfig.Load(Path.Combine(AppDirs.ConfigDir, EditorConfig.DefaultFileName), out string errMsg))
                Log.WriteError(errMsg);
        }

        /// <summary>
        /// Localizes the application.
        /// </summary>
        private void LocalizeApp()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ScadaCommon", out string errMsg))
                Log.WriteError(errMsg);

            CommonPhrases.Init();
        }


        /// <summary>
        /// Initializes the context.
        /// </summary>
        public void Init()
        {
            AppDirs.Init(Assembly.GetExecutingAssembly());
            LoadInstanceConfig();

            logFile.FileName = Path.Combine(AppDirs.LogDir, EditorUtils.LogFileName);
            Log.WriteBreak();
            Log.WriteAction(Locale.IsRussian ?
                "Редактор схем {0} запущен" :
                "Scheme Editor {0} started", EditorUtils.AppVersion);

            LocalizeApp();
            LoadAppConfig();
        }

        /// <summary>
        /// Finalizes the context.
        /// </summary>
        public void FinalizeContext()
        {
            Log.WriteAction(Locale.IsRussian ?
                "Редактор схем остановлен" :
                "Scheme Editor is stopped");
            Log.WriteBreak();
        }
    }
}
