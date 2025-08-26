// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// The class provides helper methods for the editor.
    /// <para>Класс, предоставляющий вспомогательные методы для редактора.</para>
    /// </summary>
    internal static class EditorUtils
    {
        /// <summary>
        /// The plugin log file name.
        /// </summary>
        public const string LogFileName = EditorPluginInfo.PluginCode + ".log";

        /// <summary>
        /// Finds the project file name traversing up from the specified file.
        /// </summary>
        public static bool FindProject(string startFileName, out string projectFileName)
        {
            FileInfo startFileInfo = new(startFileName);
            DirectoryInfo dirInfo = startFileInfo.Directory;

            while (dirInfo != null)
            {
                foreach (FileInfo fileInfo in dirInfo.EnumerateFiles("*.rsproj", SearchOption.TopDirectoryOnly))
                {
                    projectFileName = fileInfo.FullName;
                    return true;
                }

                dirInfo = dirInfo.Parent;
            }

            projectFileName = "";
            return false;
        }

        /// <summary>
        /// Gets the directory of views for the specified project.
        /// </summary>
        public static string GetViewDir(string projectFileName)
        {
            return ScadaUtils.NormalDir(Path.Combine(Path.GetDirectoryName(projectFileName), "Views"));
        }

        /// <summary>
        /// Convers the property name to a pascal-casing format.
        /// </summary>
        public static string ToPascalCase(this string s)
        {
            if (string.IsNullOrEmpty(s) || char.IsUpper(s[0]))
                return s;

            if (s == "id")
                return "ID";

            return s[0].ToString().ToUpperInvariant() + s[1..];
        }
    }
}
