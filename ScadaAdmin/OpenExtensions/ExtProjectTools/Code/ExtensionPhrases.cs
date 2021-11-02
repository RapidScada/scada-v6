// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtProjectTools.Code.IntegrityCheck
        public static string IntegrityCheckTitle { get; private set; }
        public static string TableCorrect { get; private set; }
        public static string TableHasErrors { get; private set; }
        public static string LostPrimaryKeys { get; private set; }
        public static string BaseCorrect { get; private set; }
        public static string BaseHasErrors { get; private set; }
        public static string IntegrityCheckError { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtProjectTools.Code.IntegrityCheck");
            IntegrityCheckTitle = dict["IntegrityCheckTitle"];
            TableCorrect = dict["TableCorrect"];
            TableHasErrors = dict["TableHasErrors"];
            LostPrimaryKeys = dict["LostPrimaryKeys"];
            BaseHasErrors = dict["BaseHasErrors"];
            IntegrityCheckError = dict["IntegrityCheckError"];
        }
    }
}
