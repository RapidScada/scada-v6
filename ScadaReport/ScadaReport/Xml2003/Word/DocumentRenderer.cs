// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Xml2003.Word
{
    /// <summary>
    /// Renders a Word document in WordprocessingML format from a template.
    /// <para>Генерирует документ Word в формате WordprocessingML из шаблона.</para>
    /// </summary>
    public class DocumentRenderer : Xml2003Renderer
    {
        /// <summary>
        /// The name of an XML element that can contain a report directive.
        /// </summary>
        protected const string DirectiveElem = "w:t";


        /// <summary>
        /// Конструктор
        /// </summary>
        public DocumentRenderer()
            : base()
        {
        }


        /// <summary>
        /// Processes the XML document recursively.
        /// </summary>
        protected override void ProcessXmlDoc(XmlNode xmlNode)
        {
            if (xmlNode.Name == DirectiveElem)
            {
                FindDirectives(xmlNode);
                OnNodeProcessed(xmlNode);
            }
            else
            {
                OnNodeProcessed(xmlNode);

                // process child nodes
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    ProcessXmlDoc(node);
                }
            }
        }

        /// <summary>
        /// Finds and processes directives of the specified node.
        /// </summary>
        protected void FindDirectives(XmlNode xmlNode)
        {
            if (FindDirective(xmlNode.InnerText, ReportDirectives.RepRow, out string val, out string rest))
            {
                xmlNode.InnerText = rest;
                OnDirectiveFound(ReportDirectives.RepRow, val, xmlNode);
            }
            else if (FindDirective(xmlNode.InnerText, ReportDirectives.RepVal, out val, out _))
            {
                OnDirectiveFound(ReportDirectives.RepVal, val, xmlNode);
            }
        }
    }
}
