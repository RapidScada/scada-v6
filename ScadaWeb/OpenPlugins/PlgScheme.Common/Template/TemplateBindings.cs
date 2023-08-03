// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Template
{
    /// <summary>
    /// Represents bindings of a scheme template.
    /// <para>Представляет привязки шаблона схемы.</para>
    /// </summary>
    public class TemplateBindings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TemplateBindings()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the scheme template file name.
        /// </summary>
        public string TemplateFileName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the component that displays a scheme title.
        /// </summary>
        public int TitleCompID { get; set; }

        /// <summary>
        /// Gets the bindings of the scheme components.
        /// </summary>
        public SortedDictionary<int, ComponentBinding> ComponentBindings { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            TemplateFileName = "";
            TitleCompID = 0;
            ComponentBindings = new SortedDictionary<int, ComponentBinding>();
        }

        /// <summary>
        /// Loads the bindings from the specified stream.
        /// </summary>
        public void Load(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            XmlDocument xmlDoc = new();
            xmlDoc.Load(stream);
            XmlElement rootElem = xmlDoc.DocumentElement;

            TemplateFileName = rootElem.GetChildAsString("TemplateFileName");
            TitleCompID = rootElem.GetChildAsInt("TitleCompID");

            foreach (XmlElement bindingElem in rootElem.SelectNodes("Binding"))
            {
                ComponentBinding binding = new();
                binding.LoadFromXml(bindingElem);

                if (binding.CompID > 0)
                    ComponentBindings[binding.CompID] = binding;
            }
        }

        /// <summary>
        /// Loads the bindings from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                using (FileStream stream = new(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Load(stream);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(SchemePhrases.LoadTemplateBindingsError);
                return false;
            }
        }

        /// <summary>
        /// Saves the bindings from the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("TemplateBindings");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendElem("TemplateFileName", TemplateFileName);
                rootElem.AppendElem("TitleCompID", TitleCompID);

                foreach (ComponentBinding binding in ComponentBindings.Values)
                {
                    XmlElement bindingElem = rootElem.AppendElem("Binding");
                    bindingElem.SetAttribute("compID", binding.CompID);

                    if (binding.InCnlNum > 0)
                        bindingElem.SetAttribute("inCnlNum", binding.InCnlNum);

                    if (binding.CtrlCnlNum > 0)
                        bindingElem.SetAttribute("ctrlCnlNum", binding.CtrlCnlNum);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(SchemePhrases.SaveTemplateBindingsError);
                return false;
            }
        }
    }
}
