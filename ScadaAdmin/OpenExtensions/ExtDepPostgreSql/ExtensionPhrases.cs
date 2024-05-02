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
        // Scada.Admin.Extensions.ExtDepPostgreSql.Downloader
        public static string DownloadTable { get; private set; }
        public static string DownloadView { get; private set; }
        public static string DownloadConfigFile { get; private set; }

        // Scada.Admin.Extensions.ExtDepPostgreSql.Uploader
        public static string CreateSchema { get; private set; }
        public static string CreateAppDict { get; private set; }
        public static string ClearBase { get; private set; }
        public static string CreateBase { get; private set; }
        public static string DeleteTable { get; private set; }
        public static string TruncateTable { get; private set; }
        public static string CreateTable { get; private set; }
        public static string CreateFKs { get; private set; }
        public static string CreateTableFKs { get; private set; }
        public static string ClearViews { get; private set; }
        public static string CreateViews { get; private set; }
        public static string CreateView { get; private set; }
        public static string ClearAllAppConfig { get; private set; }
        public static string ClearAppConfig { get; private set; }
        public static string CreateAppConfig { get; private set; }
        public static string CreateConfigFile { get; private set; }
        public static string UnableRestartServices { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtDepPostgreSql.Downloader");
            DownloadTable = dict[nameof(DownloadTable)];
            DownloadView = dict[nameof(DownloadView)];
            DownloadConfigFile = dict[nameof(DownloadConfigFile)];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtDepPostgreSql.Uploader");
            CreateSchema = dict[nameof(CreateSchema)];
            CreateAppDict = dict[nameof(CreateAppDict)];
            ClearBase = dict[nameof(ClearBase)];
            CreateBase = dict[nameof(CreateBase)];
            DeleteTable = dict[nameof(DeleteTable)];
            TruncateTable = dict[nameof(TruncateTable)];
            CreateTable = dict[nameof(CreateTable)];
            CreateFKs = dict[nameof(CreateFKs)];
            CreateTableFKs = dict[nameof(CreateTableFKs)];
            ClearViews = dict[nameof(ClearViews)];
            CreateViews = dict[nameof(CreateViews)];
            CreateView = dict[nameof(CreateView)];
            ClearAllAppConfig = dict[nameof(ClearAllAppConfig)];
            ClearAppConfig = dict[nameof(ClearAppConfig)];
            CreateAppConfig = dict[nameof(CreateAppConfig)];
            CreateConfigFile = dict[nameof(CreateConfigFile)];
            UnableRestartServices = dict[nameof(UnableRestartServices)];
        }
    }
}
