// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Doc.Code
{
    /// <summary>
    /// Represents metadata about a web page.
    /// <para>Представляет метаданные веб-страницы.</para>
    /// </summary>
    public class PageMeta
    {
        /// <summary>
        /// Gets the page language.
        /// </summary>
        public KnownLang Lang { get; private set; } = KnownLang.None;

        /// <summary>
        /// Gets the documentation version.
        /// </summary>
        public KnownVersion Version { get; private set; } = KnownVersion.None;


        /// <summary>
        /// Converts the string representation to a page metadata.
        /// </summary>
        public static PageMeta Parse(string pagePath)
        {
            PageMeta pageMeta = new();
            string[] parts = (pagePath ?? "").Split('/', StringSplitOptions.RemoveEmptyEntries);
            string langStr = parts.Length > 0 ? parts[0].ToLowerInvariant() : "";
            string versionStr = parts.Length > 1 ? parts[1].ToLowerInvariant() : "";

            pageMeta.Lang = langStr switch
            {
                "en" => KnownLang.En,
                "ru" => KnownLang.Ru,
                _ => KnownLang.None
            };

            pageMeta.Version = versionStr switch
            {
                "5.8" => KnownVersion.V58,
                "6.0" => KnownVersion.V60,
                _ => KnownVersion.None
            };

            return pageMeta;
        }
    }
}
