/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaAdminCommon
 * Summary  : Represents configuration upload options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2022
 */

using Scada.Agent;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Represents configuration upload options.
    /// <para>Представляет параметры передачи конфигурации на сервер.</para>
    /// </summary>
    [Serializable]
    public class UploadOptions : TransferOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UploadOptions()
            : base()
        {
            RestartServer = true;
            RestartComm = true;
            RestartWeb = true;
            ObjectFilter = new List<int>();
        }


        /// <summary>
        /// Gets or sets a value indicating whether to restart Server after upload is complete.
        /// </summary>
        public bool RestartServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to restart Communicator after upload is complete.
        /// </summary>
        public bool RestartComm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to reload Webstation configuration after upload is complete.
        /// </summary>
        public bool RestartWeb { get; set; }

        /// <summary>
        /// Gets a value indicating whether to restart at least one service.
        /// </summary>
        public bool RestartAnyService => RestartServer || RestartComm || RestartWeb;

        /// <summary>
        /// Gets the object numbers to filter the uploaded configuration.
        /// </summary>
        public List<int> ObjectFilter { get; private set; }


        /// <summary>
        /// Sets the object filter.
        /// </summary>
        public void SetObjectFilter(ICollection<int> objNums)
        {
            ObjectFilter.Clear();

            if (objNums != null)
                ObjectFilter.AddRange(objNums);
        }
                
        /// <summary>
        /// Loads the settings from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            RestartServer = xmlNode.GetChildAsBool("RestartServer");
            RestartComm = xmlNode.GetChildAsBool("RestartComm");
            RestartWeb = xmlNode.GetChildAsBool("RestartWeb");
            SetObjectFilter(ScadaUtils.ParseRange(xmlNode.GetChildAsString("ObjectFilter"), true, true));
        }

        /// <summary>
        /// Saves the settings into the XML node.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("RestartServer", RestartServer);
            xmlElem.AppendElem("RestartComm", RestartComm);
            xmlElem.AppendElem("RestartWeb", RestartWeb);
            xmlElem.AppendElem("ObjectFilter", ObjectFilter.ToRangeString());
        }
    }
}
