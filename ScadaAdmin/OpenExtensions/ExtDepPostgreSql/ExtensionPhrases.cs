// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtDepPostgreSql
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal static class ExtensionPhrases
    {
        public static string DbNotEnabled { get; private set; }
        public static string FileCount { get; private set; }

        static ExtensionPhrases()
        {
            if (Locale.IsRussian)
            {
                DbNotEnabled = "База данных не включена в профиле развёртывания.";
                FileCount = "Количество файлов: {0}";
            }
            else
            {
                DbNotEnabled = "Database is not enabled in the deployment profile.";
                FileCount = "File count: {0}";
            }
        }
    }
}
