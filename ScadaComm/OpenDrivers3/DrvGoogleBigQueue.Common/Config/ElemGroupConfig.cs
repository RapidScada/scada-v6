// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Config
{
    /// <summary>
    /// Represents an element group configuration.
    /// <para>Представляет конфигурацию группы элементов.</para>
    /// </summary>
    public class ElemGroupConfig : DataUnitConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ElemGroupConfig()
            : base()
        {
            Active = true;
            Elems = new List<ElemConfig>();
            StartTagNum = 0;
            QuerySql = "";
        }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the element group is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets the configuration of the elements.
        /// </summary>
        public List<ElemConfig> Elems { get; private set; }

        /// <summary>
        /// Gets or sets the device tag number that corresponds to the start element.
        /// </summary>
        public int StartTagNum { get; set; }

        /// <summary>
        /// big queue sql
        /// </summary>
        public string QuerySql { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public string ProjectId { get; set; }


        /// <summary>
        /// Creates a new element configuration.
        /// </summary>
        public virtual ElemConfig CreateElemConfig()
        {
            return new ElemConfig {  };
        }

        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Active = xmlElem.GetAttrAsBool("active");
            Name = xmlElem.GetAttrAsString("name");
            Code = xmlElem.GetAttrAsString("code");
            ProjectId = xmlElem.GetAttrAsString("projectId");
            QuerySql = xmlElem.GetAttrAsString("querySql");

            foreach (XmlElement elemElem in xmlElem.SelectNodes("Elem"))
            {
                ElemConfig elemConfig = CreateElemConfig();
                elemConfig.TagCode = elemElem.GetAttrAsString("tagCode");
                elemConfig.Name = elemElem.GetAttrAsString("name");
                Elems.Add(elemConfig);
            }
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("code", Code);
            xmlElem.SetAttribute("projectId", ProjectId);
            xmlElem.SetAttribute("querySql", QuerySql);

            foreach (ElemConfig elemConfig in Elems)
            {
                XmlElement elemElem = xmlElem.AppendElem("Elem");
                elemElem.SetAttribute("tagCode", elemConfig.TagCode);
                elemElem.SetAttribute("name", elemConfig.Name);
            }
        }
    }
}
