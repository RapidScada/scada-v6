// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtDepAgent
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal static class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtDepAgent.Downloader
        public static string ImportTable { get; private set; }
        public static string ExtractArchive { get; private set; }
        public static string MergeDirectory { get; private set; }

        // Scada.Admin.Extensions.ExtDepAgent.Uploader
        public static string TestAgentConn { get; private set; }
        public static string CompressConfig { get; private set; }
        public static string CompressBase { get; private set; }
        public static string CompressViews { get; private set; }
        public static string CompressAppConfig { get; private set; }
        public static string AddProjectInfo { get; private set; }
        public static string AddTransferOptions { get; private set; }
        public static string TransferConfig { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtDepAgent.Downloader");
            ImportTable = dict["ImportTable"];
            ExtractArchive = dict["ExtractArchive"];
            MergeDirectory = dict["MergeDirectory"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtDepAgent.Uploader");
            TestAgentConn = dict["TestAgentConn"];
            CompressConfig = dict["CompressConfig"];
            CompressBase = dict["CompressBase"];
            CompressViews = dict["CompressViews"];
            CompressAppConfig = dict["CompressAppConfig"];
            AddProjectInfo = dict["AddProjectInfo"];
            AddTransferOptions = dict["AddTransferOptions"];
            TransferConfig = dict["TransferConfig"];
        }
    }
}
