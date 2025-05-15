// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents information about a faceplate.
    /// <para>Представляет информацию о фейсплейте.</para>
    /// </summary>
    public class FaceplateMeta : IComparable<FaceplateMeta>
    {
        /// <summary>
        /// Gets or sets the name of the faceplate type.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the file path relative to the directory of views.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets a value indicating whether the faceplate is referenced by another faceplate of the mimic.
        /// </summary>
        public bool IsTransitive { get; init; }


        /// <summary>
        /// Loads the faceplate information from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            TypeName = xmlElem.GetAttrAsString("typeName");
            Path = xmlElem.GetAttrAsString("path");
        }

        /// <summary>
        /// Saves the faceplate information into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("typeName", TypeName);
            xmlElem.SetAttribute("path", Path);
        }

        /// <summary>
        /// Creates a copy of the current object and marks it as transitive.
        /// </summary>
        public FaceplateMeta Transit()
        {
            return new FaceplateMeta
            {
                TypeName = TypeName,
                Path = Path,
                IsTransitive = true
            };
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        public int CompareTo(FaceplateMeta other)
        {
            return string.CompareOrdinal(TypeName, other?.TypeName);
        }
    }
}
