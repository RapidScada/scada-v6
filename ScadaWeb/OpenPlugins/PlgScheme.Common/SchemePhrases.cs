// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// The common phrases for schemes.
    /// <para>Общие фразы для схем.</para>
    /// </summary>
    public static class SchemePhrases
    {
        // Scada.Web.Plugins.PlgScheme.Template.TemplateBindings
        public static string LoadTemplateBindingsError { get; private set; }
        public static string SaveTemplateBindingsError { get; private set; }

        // Scada.Web.Plugins.PlgScheme.CompManager
        public static string UnknownComponent { get; private set; }
        public static string CompLibraryNotFound { get; private set; }
        public static string UnableCreateComponent { get; private set; }
        public static string ErrorCreatingComponent { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Web.Plugins.PlgScheme.Template.TemplateBindings");
            LoadTemplateBindingsError = dict.GetPhrase("LoadTemplateBindingsError");
            SaveTemplateBindingsError = dict.GetPhrase("SaveTemplateBindingsError");

            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgScheme.CompManager");
            UnknownComponent = dict.GetPhrase("UnknownComponent");
            CompLibraryNotFound = dict.GetPhrase("CompLibraryNotFound");
            UnableCreateComponent = dict.GetPhrase("UnableCreateComponent");
            ErrorCreatingComponent = dict.GetPhrase("ErrorCreatingComponent");
        }
    }
}
