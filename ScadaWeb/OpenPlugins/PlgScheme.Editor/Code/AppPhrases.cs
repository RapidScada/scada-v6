// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// The phrases used by the application.
    /// <para>Фразы, используемые приложением.</para>
    /// </summary>
    internal static class AppPhrases
    {
        // Scada.Web.Plugins.PlgScheme.Editor.Code.Editor
        public static string EditorTitle { get; private set; }

        // Scada.Web.Plugins.PlgScheme.Editor.Code.FormState
        public static string LoadFormStateError { get; private set; }
        public static string SaveFormStateError { get; private set; }

        // Scada.Web.Plugins.PlgScheme.Editor.Forms.FrmMain
        public static string CloseSecondInstance { get; private set; }
        public static string FailedToStartEditor { get; private set; }
        public static string OpenBrowserError { get; private set; }
        public static string PointerItem { get; private set; }
        public static string SchemeFileFilter { get; private set; }
        public static string SaveSchemeConfirm { get; private set; }
        public static string RestartNeeded { get; private set; }

        // Scada.Web.Plugins.PlgScheme.Editor.Forms.FrmSettings
        public static string WebDirNotExists { get; private set; }
        public static string ChooseWebDir { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Web.Plugins.PlgScheme.Editor.Code.Editor");
            EditorTitle = dict[nameof(EditorTitle)];

            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgScheme.Editor.Code.FormState");
            LoadFormStateError = dict[nameof(LoadFormStateError)];
            SaveFormStateError = dict[nameof(SaveFormStateError)];

            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgScheme.Editor.Forms.FrmMain");
            CloseSecondInstance = dict[nameof(CloseSecondInstance)];
            FailedToStartEditor = dict[nameof(FailedToStartEditor)];
            OpenBrowserError = dict[nameof(OpenBrowserError)];
            PointerItem = dict[nameof(PointerItem)];
            SchemeFileFilter = dict[nameof(SchemeFileFilter)];
            SaveSchemeConfirm = dict[nameof(SaveSchemeConfirm)];
            RestartNeeded = dict[nameof(RestartNeeded)];

            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgScheme.Editor.Forms.FrmSettings");
            WebDirNotExists = dict[nameof(WebDirNotExists)];
            ChooseWebDir = dict[nameof(ChooseWebDir)];
        }
    }
}
