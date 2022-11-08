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
    }
}
