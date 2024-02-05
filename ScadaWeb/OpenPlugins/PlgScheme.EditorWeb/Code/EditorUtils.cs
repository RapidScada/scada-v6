// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// The class provides helper methods for the editor.
    /// <para>Класс, предоставляющий вспомогательные методы для редактора.</para>
    /// </summary>
    internal static class EditorUtils
    {
        /// <summary>
        /// The application log file name.
        /// </summary>
        public const string LogFileName = "ScadaSchemeEditor.log";

        /// <summary>
        /// The application version.
        /// </summary>
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
