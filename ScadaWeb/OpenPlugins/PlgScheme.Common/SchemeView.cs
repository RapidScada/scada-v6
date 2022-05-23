// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// Represents a scheme view.
    /// <para>Представляет схему.</para>
    /// </summary>
    public class SchemeView : ViewBase
    {
        /// <summary>
        /// Manages the scheme components.
        /// </summary>
        protected CompManager compManager;
        /// <summary>
        /// The maximum ID of the scheme components.
        /// </summary>
        protected int maxComponentID;
        /// <summary>
        /// The scheme arguments in template mode.
        /// </summary>
        protected TemplateArgs templateArgs;
        /// <summary>
        /// The scheme template bindings.
        /// </summary>
        protected TemplateBindings templateBindings;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SchemeView(View viewEntity)
            : base(viewEntity)
        {
            maxComponentID = 0;
            templateArgs = new TemplateArgs(Args);
            templateBindings = null;

            SchemeDoc = new SchemeDocument { SchemeView = this } ;
            Components = new SortedList<int, ComponentBase>();
            LoadErrors = new List<string>();
        }


        /// <summary>
        /// Gets or sets the component manager.
        /// </summary>
        public CompManager CompManager
        {
            get
            {
                return compManager ?? throw new InvalidOperationException("Component manager is undefined.");
            }
            set
            {
                compManager = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets the scheme document properties.
        /// </summary>
        public SchemeDocument SchemeDoc { get; protected set; }

        /// <summary>
        /// Gets the scheme components accessed by component ID.
        /// </summary>
        public SortedList<int, ComponentBase> Components { get; protected set; }

        /// <summary>
        /// Gets the errors occurred during download.
        /// </summary>
        /// <remarks>Allows to control loading of libraries and components.</remarks>
        public List<string> LoadErrors { get; protected set; }


        /// <summary>
        /// Adds the input channels to the view.
        /// </summary>
        private void AddInCnlNums(List<int> inCnlNums, int offset)
        {
            if (inCnlNums != null)
            {
                foreach (int cnlNum in inCnlNums)
                {
                    if (cnlNum > 0)
                        AddCnlNum(cnlNum + offset);
                }
            }
        }

        /// <summary>
        /// Loads the view from the specified stream.
        /// </summary>
        public override void LoadView(Stream stream)
        {
            // add template bindings as resource
            if (!string.IsNullOrEmpty(templateArgs.BindingFileName))
            {
                Resources.Add(new ViewResource 
                { 
                    Name = nameof(TemplateBindings), 
                    Path = templateArgs.BindingFileName 
                });
            }

            // load XML document
            XmlDocument xmlDoc = new();
            xmlDoc.Load(stream);

            // check data format
            XmlElement rootElem = xmlDoc.DocumentElement;
            if (!rootElem.Name.Equals("SchemeView", StringComparison.OrdinalIgnoreCase))
                throw new ScadaException(CommonPhrases.InvalidFileFormat);

            // load scheme document
            if (rootElem.SelectSingleNode("Scheme") is XmlNode schemeNode)
            {
                SchemeDoc.LoadFromXml(schemeNode);

                // update scheme title
                if (string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(SchemeDoc.Title))
                    Title = SchemeDoc.Title;
            }

            // load scheme components
            if (rootElem.SelectSingleNode("Components") is XmlNode componentsNode)
            {
                HashSet<string> errNodeNames = new(); // node names of unloaded components
                LoadErrors.AddRange(CompManager.LoadErrors);

                foreach (XmlNode compNode in componentsNode.ChildNodes)
                {
                    // create component
                    ComponentBase component = CompManager.CreateComponent(compNode, out string errMsg);

                    if (component == null)
                    {
                        component = new UnknownComponent { XmlNode = compNode };
                        if (errNodeNames.Add(compNode.Name))
                            LoadErrors.Add(errMsg);
                    }

                    // load component and add it to view
                    component.SchemeView = this;
                    component.LoadFromXml(compNode);
                    Components[component.ID] = component;

                    // get max component ID
                    if (component.ID > maxComponentID)
                        maxComponentID = component.ID;
                }
            }

            // load scheme images
            if (rootElem.SelectSingleNode("Images") is XmlNode imagesNode)
            {
                Dictionary<string, Image> images = SchemeDoc.Images;
                XmlNodeList imageNodes = imagesNode.SelectNodes("Image");
                foreach (XmlNode imageNode in imageNodes)
                {
                    Image image = new();
                    image.LoadFromXml(imageNode);
                    if (!string.IsNullOrEmpty(image.Name))
                        images[image.Name] = image;
                }
            }
        }

        /// <summary>
        /// Loads the view resource from the specified stream.
        /// </summary>
        public override void LoadResource(ViewResource resource, Stream stream)
        {
            if (resource.Name == nameof(TemplateBindings))
            {
                templateBindings = new TemplateBindings();
                templateBindings.Load(stream);
            }
        }

        /// <summary>
        /// Builds the view after loading the view itself and all required resources.
        /// </summary>
        public override void Build()
        {
            // get channel offsets in template mode
            int inCnlOffset = templateArgs.InCnlOffset;
            int ctrlCnlOffset = templateArgs.CtrlCnlOffset;

            // add channels from scheme document
            AddInCnlNums(SchemeDoc.CnlFilter, inCnlOffset);

            // update channels in components
            SortedDictionary<int, ComponentBinding> componentBindings = templateBindings?.ComponentBindings;

            foreach (ComponentBase component in Components.Values)
            {
                if (component is IDynamicComponent dynamicComponent)
                {
                    if (componentBindings != null &&
                        componentBindings.TryGetValue(component.ID, out ComponentBinding binding))
                    {
                        dynamicComponent.InCnlNum = binding.InCnlNum;
                        dynamicComponent.CtrlCnlNum = binding.CtrlCnlNum;
                    }
                    else
                    {
                        if (inCnlOffset > 0 && dynamicComponent.InCnlNum > 0)
                            dynamicComponent.InCnlNum += inCnlOffset;
                        if (ctrlCnlOffset > 0 && dynamicComponent.CtrlCnlNum > 0)
                            dynamicComponent.CtrlCnlNum += ctrlCnlOffset;
                    }

                    AddCnlNum(dynamicComponent.InCnlNum);
                    AddCnlNum(dynamicComponent.CtrlCnlNum);
                }

                AddInCnlNums(component.GetInCnlNums(), inCnlOffset);
                AddInCnlNums(component.GetCtrlCnlNums(), ctrlCnlOffset);
            }

            // set title component text
            int titleCompID = templateBindings == null 
                ? templateArgs.TitleCompID 
                : templateBindings.TitleCompID;

            if (titleCompID > 0 &&
                Components.TryGetValue(titleCompID, out ComponentBase titleComponent) &&
                titleComponent is StaticText staticText)
            {
                staticText.Text = Title;
            }
        }

        /// <summary>
        /// Binds the view to the configuration database.
        /// </summary>
        public override void Bind(ConfigDataset configDataset)
        {
            AddCnlNumsForArrays(configDataset.CnlTable);
        }

        /// <summary>
        /// Loads the scheme from the specified file.
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            try
            {
                using (FileStream fileStream = new(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    LoadView(fileStream);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(CommonPhrases.LoadViewError);
                return false;
            }
        }

        /// <summary>
        /// Saves the scheme to the specified file.
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                // view title
                XmlElement rootElem = xmlDoc.CreateElement("SchemeView");
                rootElem.SetAttribute("title", SchemeDoc.Title);
                xmlDoc.AppendChild(rootElem);

                // scheme document
                XmlElement documentElem = xmlDoc.CreateElement("Scheme");
                rootElem.AppendChild(documentElem);
                SchemeDoc.SaveToXml(documentElem);

                // scheme components
                HashSet<string> prefixes = new();
                XmlElement componentsElem = xmlDoc.CreateElement("Components");
                rootElem.AppendChild(componentsElem);

                foreach (ComponentBase component in Components.Values)
                {
                    if (component is UnknownComponent unknownComponent)
                    {
                        componentsElem.AppendChild(unknownComponent.XmlNode);
                    }
                    else
                    {
                        Type compType = component.GetType();
                        CompLibSpec compLibSpec = CompManager.GetSpecByType(compType);

                        // add namespace
                        if (compLibSpec != null && !prefixes.Contains(compLibSpec.XmlPrefix))
                        {
                            rootElem.SetAttribute("xmlns:" + compLibSpec.XmlPrefix, compLibSpec.XmlNs);
                            prefixes.Add(compLibSpec.XmlPrefix);
                        }

                        // create XML element of component
                        XmlElement componentElem = compLibSpec == null ?
                            xmlDoc.CreateElement(compType.Name) /*standard component*/ :
                            xmlDoc.CreateElement(compLibSpec.XmlPrefix, compType.Name, compLibSpec.XmlNs);

                        componentsElem.AppendChild(componentElem);
                        component.SaveToXml(componentElem);
                    }
                }

                // scheme images
                XmlElement imagesElem = xmlDoc.CreateElement("Images");
                rootElem.AppendChild(imagesElem);

                foreach (Image image in SchemeDoc.Images.Values)
                {
                    XmlElement imageElem = xmlDoc.CreateElement("Image");
                    imagesElem.AppendChild(imageElem);
                    image.SaveToXml(imageElem);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(CommonPhrases.SaveViewError);
                return false;
            }
        }

        /// <summary>
        /// Gets the next component ID.
        /// </summary>
        public int GetNextComponentID()
        {
            return ++maxComponentID;
        }
    }
}
