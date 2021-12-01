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
        // Scada.Scheme.Model.PropertyGrid
        public static string StringConvertError { get; private set; }
        public static string StringUniqueError { get; private set; }
        public static string TrueValue { get; private set; }
        public static string FalseValue { get; private set; }
        public static string EmptyValue { get; private set; }
        public static string ObjectValue { get; private set; }
        public static string CollectionValue { get; private set; }
        public static string ComponentNotFound { get; private set; }
        public static string BoldSymbol { get; private set; }
        public static string ItalicSymbol { get; private set; }
        public static string UnderlineSymbol { get; private set; }

        // Scada.Scheme.Model.PropertyGrid.FrmImageDialog
        public static string ImageFileFilter { get; private set; }
        public static string DisplayImageError { get; private set; }
        public static string LoadImageError { get; private set; }
        public static string SaveImageError { get; private set; }

        // Scada.Scheme.Model.PropertyGrid.FrmRangeDialog
        public static string RangeNotValid { get; private set; }

        // Scada.Scheme.Template.TemplateBindings
        public static string LoadTemplateBindingsError { get; private set; }
        public static string SaveTemplateBindingsError { get; private set; }

        // Scada.Scheme.CompManager
        public static string UnknownComponent { get; private set; }
        public static string CompLibraryNotFound { get; private set; }
        public static string UnableCreateComponent { get; private set; }
        public static string ErrorCreatingComponent { get; private set; }

        // Scada.Scheme.SchemeView
        public static string LoadSchemeViewError { get; private set; }
        public static string SaveSchemeViewError { get; private set; }
        public static string IncorrectFileFormat { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Scheme.Model.PropertyGrid");
            StringConvertError = dict.GetPhrase("StringConvertError");
            StringUniqueError = dict.GetPhrase("StringUniqueError");
            TrueValue = dict.GetPhrase("TrueValue");
            FalseValue = dict.GetPhrase("FalseValue");
            EmptyValue = dict.GetPhrase("EmptyValue");
            ObjectValue = dict.GetPhrase("ObjectValue");
            CollectionValue = dict.GetPhrase("CollectionValue");
            ComponentNotFound = dict.GetPhrase("ComponentNotFound");
            BoldSymbol = dict.GetPhrase("BoldSymbol");
            ItalicSymbol = dict.GetPhrase("ItalicSymbol");
            UnderlineSymbol = dict.GetPhrase("UnderlineSymbol");

            dict = Locale.GetDictionary("Scada.Scheme.Model.PropertyGrid.FrmImageDialog");
            ImageFileFilter = dict.GetPhrase("ImageFileFilter");
            DisplayImageError = dict.GetPhrase("DisplayImageError");
            LoadImageError = dict.GetPhrase("LoadImageError");
            SaveImageError = dict.GetPhrase("SaveImageError");

            dict = Locale.GetDictionary("Scada.Scheme.Model.PropertyGrid.FrmRangeDialog");
            RangeNotValid = dict.GetPhrase("RangeNotValid");

            dict = Locale.GetDictionary("Scada.Scheme.Template.TemplateBindings");
            LoadTemplateBindingsError = dict.GetPhrase("LoadTemplateBindingsError");
            SaveTemplateBindingsError = dict.GetPhrase("SaveTemplateBindingsError");

            dict = Locale.GetDictionary("Scada.Scheme.CompManager");
            UnknownComponent = dict.GetPhrase("UnknownComponent");
            CompLibraryNotFound = dict.GetPhrase("CompLibraryNotFound");
            UnableCreateComponent = dict.GetPhrase("UnableCreateComponent");
            ErrorCreatingComponent = dict.GetPhrase("ErrorCreatingComponent");

            dict = Locale.GetDictionary("Scada.Scheme.SchemeView");
            LoadSchemeViewError = dict.GetPhrase("LoadSchemeViewError");
            SaveSchemeViewError = dict.GetPhrase("SaveSchemeViewError");
            IncorrectFileFormat = dict.GetPhrase("IncorrectFileFormat");
        }
    }
}
