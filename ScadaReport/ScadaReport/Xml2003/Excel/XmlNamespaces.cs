// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// Specifies the namespaces that are used in SpreadsheetML.
    /// <para>Задаёт пространства имён, которые используются в SpreadsheetML.</para>
    /// </summary>
    public static class XmlNamespaces
    {
        /// <summary>
        /// xmlns
        /// </summary>
        public const string noprefix = "urn:schemas-microsoft-com:office:spreadsheet";

        /// <summary>
        /// xmlns:o
        /// </summary>
        public const string o = "urn:schemas-microsoft-com:office:office";

        /// <summary>
        /// xmlns:x
        /// </summary>
        public const string x = "urn:schemas-microsoft-com:office:excel";

        /// <summary>
        /// xmlns:ss
        /// </summary>
        public const string ss = "urn:schemas-microsoft-com:office:spreadsheet";

        /// <summary>
        /// xmlns:html
        /// </summary>
        public const string html = "http://www.w3.org/TR/REC-html40";
    }
}
