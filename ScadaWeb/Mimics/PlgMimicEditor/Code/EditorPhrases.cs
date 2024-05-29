// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// The phrases used by the editor.
    /// <para>Фразы, используемые редактором.</para>
    /// </summary>
    internal static class EditorPhrases
    {
        // Scada.Web.Plugins.PlgMimicEditor.Code.EditorManager
        public static string ProjectNotFound { get; private set; }
        public static string MimicNotFound { get; private set; }
        public static string LoadMimicError { get; private set; }

        // Scada.Web.Plugins.PlgMimicEditor.PlgMimicEditorLogic
        public static string EditorMenuItem { get; private set; }
        public static string MimicsMenuItem { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMimicEditor.Code.EditorManager");
            ProjectNotFound = dict[nameof(ProjectNotFound)];
            MimicNotFound = dict[nameof(MimicNotFound)];
            LoadMimicError = dict[nameof(LoadMimicError)];

            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMimicEditor.PlgMimicEditorLogic");
            EditorMenuItem = dict[nameof(EditorMenuItem)];
            MimicsMenuItem = dict[nameof(MimicsMenuItem)];
        }
    }
}
