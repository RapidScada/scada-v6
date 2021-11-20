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
            DownloadTable = dict["DownloadTable"];
            DownloadView = dict["DownloadView"];
            DownloadConfigFile = dict["DownloadConfigFile"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtDepPostgreSql.Uploader");
            CreateSchema = dict["CreateSchema"];
            CreateAppDict = dict["CreateAppDict"];
            ClearBase = dict["ClearBase"];
            CreateBase = dict["CreateBase"];
            DeleteTable = dict["DeleteTable"];
            CreateTable = dict["CreateTable"];
            CreateFKs = dict["CreateFKs"];
            CreateTableFKs = dict["CreateTableFKs"];
            ClearViews = dict["ClearViews"];
            CreateViews = dict["CreateViews"];
            CreateView = dict["CreateView"];
            ClearAllAppConfig = dict["ClearAllAppConfig"];
            ClearAppConfig = dict["ClearAppConfig"];
            CreateAppConfig = dict["CreateAppConfig"];
            CreateConfigFile = dict["CreateConfigFile"];
            UnableRestartServices = dict["UnableRestartServices"];
        }
    }
}
