// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtCommConfig.ExtCommConfigLogic
        public static string GeneralOptionsNode { get; private set; }
        public static string DriversNode { get; private set; }
        public static string DataSourcesNode { get; private set; }
        public static string LinesNode { get; private set; }
        public static string LineOptionsNode { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDataSources
        public static string DriverNotSpecified { get; private set; }
        public static string DataSourceNotSupported { get; private set; }
        public static string UnableCreateDataSourceView { get; private set; }
        public static string NoDataSourceView { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.ExtCommConfigLogic");
            GeneralOptionsNode = dict["GeneralOptionsNode"];
            DriversNode = dict["DriversNode"];
            DataSourcesNode = dict["DataSourcesNode"];
            LinesNode = dict["LinesNode"];
            LineOptionsNode = dict["LineOptionsNode"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDataSources");
            DriverNotSpecified = dict["DriverNotSpecified"];
            DataSourceNotSupported = dict["DataSourceNotSupported"];
            UnableCreateDataSourceView = dict["UnableCreateDataSourceView"];
            NoDataSourceView = dict["NoDataSourceView"];
        }
    }
}
