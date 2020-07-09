/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaServerCommon
 * Summary  : Represents the location options of the data required for the server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.IO;
using System.Xml;

namespace Scada.Server.Config
{
    /// <summary>
    /// Represents the location options of the data required for the server.
    /// <para>Представляет параметры расположения данных, необходимых для работы сервера.</para>
    /// </summary>
    public class PathOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PathOptions()
        {
            if (ScadaUtils.IsRunningOnWin)
            {
                ArcDir = @"C:\SCADA\Archive\";
                ArcCopyDir = @"C:\SCADA\ArchiveCopy\";
                BaseDir = @"C:\SCADA\BaseDAT\";
                ViewDir = @"C:\SCADA\Views\";
            }
            else
            {
                ArcDir = "/opt/scada/Archive/";
                ArcCopyDir = "/opt/scada/ArchiveCopy/";
                BaseDir = "/opt/scada/BaseDAT/";
                ViewDir = "/opt/scada/Views/";
            }
        }


        /// <summary>
        /// Gets or sets the root directory of the main archive.
        /// </summary>
        public string ArcDir { get; set; }

        /// <summary>
        /// Gets or sets the root directory of the archive copy.
        /// </summary>
        public string ArcCopyDir { get; set; }

        /// <summary>
        /// Gets or sets the directory of the configuration database in DAT format.
        /// </summary>
        public string BaseDir { get; set; }

        /// <summary>
        /// Gets or sets the directory of views.
        /// </summary>
        public string ViewDir { get; set; }


        /// <summary>
        /// Checks that the directories exist.
        /// </summary>
        public bool CheckExistence(out string errMsg)
        {
            if (Directory.Exists(ArcDir) &&
                Directory.Exists(ArcCopyDir) &&
                Directory.Exists(BaseDir) &&
                Directory.Exists(ViewDir))
            {
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = (Locale.IsRussian ?
                    "Директории данных не существуют:" :
                    "Data directories do not exist:") + Environment.NewLine +
                    ArcDir + Environment.NewLine +
                    ArcCopyDir + Environment.NewLine +
                    BaseDir + Environment.NewLine +
                    ViewDir;
                return false;
            }
        }

        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            ArcDir = ScadaUtils.NormalDir(xmlNode.GetChildAsString("ArcDir"));
            ArcCopyDir = ScadaUtils.NormalDir(xmlNode.GetChildAsString("ArcCopyDir"));
            BaseDir = ScadaUtils.NormalDir(xmlNode.GetChildAsString("BaseDir"));
            ViewDir = ScadaUtils.NormalDir(xmlNode.GetChildAsString("ViewDir"));
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("ArcDir", ArcDir);
            xmlElem.AppendElem("ArcCopyDir", ArcCopyDir);
            xmlElem.AppendElem("BaseDir", BaseDir);
            xmlElem.AppendElem("ViewDir", ViewDir);
        }

    }
}
