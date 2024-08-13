// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Xml2003
{
    /// <summary>
    /// Renders a document in XML 2003 format from a template.
    /// <para>Генерирует документ в формате XML 2003 из шаблона.</para>
    /// </summary>
    public abstract class Xml2003Renderer
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Xml2003Renderer()
        {
            ResetFields();
        }


        /// <summary>
        /// Gets the current processing stage.
        /// </summary>
        public ProcessingStage Stage { get; protected set; }

        /// <summary>
        /// Gets the rendered XML document.
        /// </summary>
        public XmlDocument XmlDoc { get; protected set; }


        /// <summary>
        /// Resets the fields before processing.
        /// </summary>
        protected virtual void ResetFields()
        {
            Stage = ProcessingStage.NotStarted;
            XmlDoc = null;
        }

        /// <summary>
        /// Processes the XML document recursively.
        /// </summary>
        protected abstract void ProcessXmlDoc(XmlNode xmlNode);

        /// <summary>
        /// Raises a BeforeProcessing event.
        /// </summary>
        protected void OnBeforeProcessing(XmlDocument xmlDoc)
        {
            BeforeProcessing?.Invoke(this, xmlDoc);
        }

        /// <summary>
        /// Raises an AfterProcessing event.
        /// </summary>
        protected void OnAfterProcessing(XmlDocument xmlDoc)
        {
            AfterProcessing?.Invoke(this, xmlDoc);
        }

        /// <summary>
        /// Raises a NodeProcessed event.
        /// </summary>
        protected void OnNodeProcessed(XmlNode xmlNode)
        {
            NodeProcessed?.Invoke(this, xmlNode);
        }

        /// <summary>
        /// Raises a DirectiveFound event.
        /// </summary>
        protected void OnDirectiveFound(DirectiveEventArgs e)
        {
            DirectiveFound?.Invoke(this, e);
        }

        /// <summary>
        /// Raises a DirectiveFound event.
        /// </summary>
        protected void OnDirectiveFound(string directiveName, string directiveValue, XmlNode xmlNode)
        {
            DirectiveFound?.Invoke(this, new DirectiveEventArgs
            {
                Stage = Stage,
                DirectiveName = directiveName,
                DirectiveValue = directiveValue,
                Node = xmlNode,
            });
        }

        /// <summary>
        /// Finds the specified directive in the string, provides the directive value and the rest of the string.
        /// </summary>
        protected static bool FindDirective(string s, string name, out string val, out string rest)
        {
            // "name=val", instead of '=' can be any character
            int valStartInd = name.Length + 1;

            if (valStartInd <= s.Length && s.StartsWith(name, StringComparison.Ordinal))
            {
                int valEndInd = s.IndexOf(" ", valStartInd);

                if (valEndInd < 0)
                {
                    val = s[valStartInd..];
                    rest = "";
                }
                else
                {
                    val = s[valStartInd..valEndInd];
                    rest = s[(valEndInd + 1)..];
                }

                return true;
            }
            else
            {
                val = "";
                rest = s;
                return false;
            }
        }


        /// <summary>
        /// Renders a document to the output stream.
        /// </summary>
        public virtual void Render(string templateFileName, Stream outStream)
        {
            ArgumentNullException.ThrowIfNull(templateFileName, nameof(templateFileName));
            ArgumentNullException.ThrowIfNull(outStream, nameof(outStream));
            ResetFields();

            // load and parse XML template
            XmlDoc = new XmlDocument();
            XmlDoc.Load(templateFileName);

            // render report by modifying XML document
            Stage = ProcessingStage.Preprocessing;
            OnBeforeProcessing(XmlDoc);

            Stage = ProcessingStage.Processing;
            ProcessXmlDoc(XmlDoc.DocumentElement);

            Stage = ProcessingStage.Postprocessing;
            OnAfterProcessing(XmlDoc);
            Stage = ProcessingStage.Completed;

            // write report to output stream
            XmlDoc.Save(outStream);
        }


        /// <summary>
        /// Occurs before processing an XML document.
        /// </summary>
        public event EventHandler<XmlDocument> BeforeProcessing;

        /// <summary>
        /// Occurs after processing an XML document.
        /// </summary>
        public event EventHandler<XmlDocument> AfterProcessing;

        /// <summary>
        /// Occurs when an XML node is processed.
        /// </summary>
        public event EventHandler<XmlNode> NodeProcessed;

        /// <summary>
        /// Occurs when a report directive is found.
        /// </summary>
        public event EventHandler<DirectiveEventArgs> DirectiveFound;
    }
}
