// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a mimic diagram.
    /// <para>Представляет мнемосхему.</para>
    /// </summary>
    public class Mimic : MimicBase
    {
        /// <summary>
        /// Gets the dependencies on the faceplates.
        /// </summary>
        public List<FaceplateMeta> Dependencies { get; } = [];

        /// <summary>
        /// Gets the faceplates accessed by type name.
        /// </summary>
        public Dictionary<string, Faceplate> Faceplates { get; } = [];


        /// <summary>
        /// Loads the mimic from the XML node.
        /// </summary>
        protected override void LoadFromXml(XmlElement rootElem)
        {
            if (rootElem.SelectSingleNode("Dependencies") is XmlNode dependenciesNode)
            {
                foreach (XmlElement faceplateElem in dependenciesNode.SelectNodes("Faceplate"))
                {
                    FaceplateMeta faceplateMeta = new();
                    faceplateMeta.LoadFromXml(faceplateElem);
                    Dependencies.Add(faceplateMeta);
                }
            }

            base.LoadFromXml(rootElem);
        }

        /// <summary>
        /// Saves the mimic into the XML node.
        /// </summary>
        protected override void SaveToXml(XmlElement rootElem)
        {
            XmlElement dependenciesElem = rootElem.AppendElem("Dependencies");

            foreach (FaceplateMeta faceplateMeta in Dependencies)
            {
                faceplateMeta.SaveToXml(dependenciesElem.AppendElem("Faceplate"));
            }

            base.SaveToXml(rootElem);
        }
    }
}
