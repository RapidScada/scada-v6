// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a faceplate, i.e. a user component.
    /// <para>Представляет фейсплейт, то есть пользовательский компонент.</para>
    /// </summary>
    public class Faceplate : MimicBase
    {
        /// <summary>
        /// Saves the faceplate.
        /// </summary>
        public override void Save(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            using StreamWriter writer = new(stream);
            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("Faceplate");
            xmlDoc.AppendChild(rootElem);
            SaveToXml(rootElem);
        }
    }
}
