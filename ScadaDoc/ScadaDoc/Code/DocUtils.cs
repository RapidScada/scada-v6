// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Doc.Code
{
    /// <summary>
    /// The class provides helper methods for the documentation.
    /// <para>Класс, предоставляющий вспомогательные методы для документации.</para>
    /// </summary>
    public static class DocUtils
    {
        /// <summary>
        /// Gets the child XML node value as a string.
        /// </summary>
        public static string GetChildAsString(this XmlNode parentXmlNode, string childNodeName, string defaultVal = "")
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ? defaultVal : node.InnerText;
        }

        /// <summary>
        /// Returns a string that represents the known language.
        /// </summary>
        public static string ConvertToString(this KnownLang lang)
        {
            return lang switch
            {
                KnownLang.En => "English",
                KnownLang.Ru => "Russian",
                KnownLang.Es => "Spanish",
                KnownLang.Zh => "简体中文",
                KnownLang.Fr => "French",
                _ => lang.ToString()
            };
        }

        /// <summary>
        /// Returns a string that represents the known version.
        /// </summary>
        public static string ConvertToString(this KnownVersion version)
        {
            return version switch
            {
                KnownVersion.V58 => "v5.8",
                KnownVersion.V60 => "v6.0",
                KnownVersion.V61 => "v6.1",
                _ => version.ToString()
            };
        }

        /// <summary>
        /// Prepends a tilde to the URL if needed.
        /// </summary>
        public static string PrependTilde(this string url)
        {
            return url.StartsWith('~') ? url : "~" + url;
        }
    }
}
