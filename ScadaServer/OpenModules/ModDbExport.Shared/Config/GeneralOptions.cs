// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Represents general options of an export target.
    /// <para>Представляет основные параметры цели экспорта.</para>
    /// </summary>
    [Serializable]
    internal class GeneralOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GeneralOptions()
        {
            Active = true;
            ID = 0;
            Name = "";
            CmdCode = "";
            StatusCnlNum = 0;
            MaxQueueSize = 1000;
            DataLifetime = 3600;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the export target is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the export target ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the export target name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the export target title.
        /// </summary>
        public string Title
        {
            get
            {
                return string.Format("[{0}] {1}", ID, Name);
            }
        }

        /// <summary>
        /// Gets or sets the command code to control the export target.
        /// </summary>
        public string CmdCode { get; set; }

        /// <summary>
        /// Gets or sets the channel number to write the status of the export target.
        /// </summary>
        public int StatusCnlNum { get; set; }

        /// <summary>
        /// Gets or sets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; set; }

        /// <summary>
        /// Gets or sets the data lifetime in the queue, in seconds.
        /// </summary>
        public int DataLifetime { get; set; }
        
        
        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            Active = xmlNode.GetChildAsBool("Active");
            ID = xmlNode.GetChildAsInt("ID");
            Name = xmlNode.GetChildAsString("Name");
            CmdCode = xmlNode.GetChildAsString("CmdCode");
            StatusCnlNum = xmlNode.GetChildAsInt("StatusCnlNum", StatusCnlNum);
            MaxQueueSize = xmlNode.GetChildAsInt("MaxQueueSize", MaxQueueSize);
            DataLifetime = xmlNode.GetChildAsInt("DataLifetime", DataLifetime);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("Active", Active);
            xmlElem.AppendElem("ID", ID);
            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("CmdCode", CmdCode);
            xmlElem.AppendElem("StatusCnlNum", StatusCnlNum);
            xmlElem.AppendElem("MaxQueueSize", MaxQueueSize);
            xmlElem.AppendElem("DataLifetime", DataLifetime);
        }
    }
}
