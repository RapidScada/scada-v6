// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Xml2003.Word
{
    /// <summary>
    /// The class provides helper methods for manipulating a Word document.
    /// <para>Класс, предоставляющий вспомогательные методы для для работы с документом Word.</para>
    /// </summary>
    internal static class WordUtils
    {
        /// <summary>
        /// The prefix of the line break element.
        /// </summary>
        private const string BreakPrefix = "w";
        /// <summary>
        /// The name of the line break element.
        /// </summary>
        private const string BreakElem = "br";
        /// <summary>
        /// The number of levels between a cell XML node and a row XML node.
        /// </summary>
        private const int RowTreeHeight = 4;


        /// <summary>
        /// Sets the XML node text to a string, which may contain line breaks. Creates new XML nodes as needed.
        /// </summary>
        public static void SetNodeTextWithBreak(XmlNode xmlNode, object text, string lineBreak = "\n")
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));

            if (xmlNode.ParentNode is not XmlNode parentNode)
                throw new Exception("Parent XML node is missing.");

            // detouch node
            parentNode.RemoveChild(xmlNode);
            XmlNode clonedNode = xmlNode.Clone();

            string textStr = text?.ToString();
            string uri = parentNode.NamespaceURI;

            do
            {
                // get line
                int breakIdx = textStr.IndexOf(lineBreak);
                bool breakFound = breakIdx >= 0;
                string line = breakFound ? textStr[..breakIdx] : textStr;

                // create line node
                XmlNode newNode = clonedNode.Clone();
                newNode.InnerText = line;
                parentNode.AppendChild(newNode);

                // create break node
                if (breakFound)
                    parentNode.AppendChild(xmlNode.OwnerDocument.CreateElement(BreakPrefix, BreakElem, uri));

                // remove processed text
                textStr = breakFound && breakIdx + lineBreak.Length < textStr.Length
                    ? textStr[(breakIdx + lineBreak.Length)..]
                    : "";
            } while (textStr != "");
        }

        /// <summary>
        /// Gets the table nodes using the specified child node that contains a directive.
        /// </summary>
        public static bool GetTableNodes(XmlNode directiveNode, out XmlNode rowNode, out XmlNode tableNode)
        {
            rowNode = directiveNode;
            tableNode = null;

            for (int i = 0; i < RowTreeHeight; i++)
            {
                if (rowNode == null || rowNode.ParentNode == null)
                    return false;

                rowNode = rowNode.ParentNode;
            }

            tableNode = rowNode.ParentNode;
            return tableNode != null;
        }
    }
}
