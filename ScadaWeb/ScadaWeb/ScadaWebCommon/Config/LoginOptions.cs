/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaWebCommon
 * Summary  : Represents login options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Xml;

namespace Scada.Web.Config
{
    /// <summary>
    /// Represents login options.
    /// <para>Представляет параметры входа в систему.</para>
    /// </summary>
    public class LoginOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LoginOptions()
        {
            CaptchaMode = CaptchaMode.Disabled;
            AllowRememberMe = false;
            AutoLoginAs = "";
        }


        /// <summary>
        /// Gets or sets the captcha mode.
        /// </summary>
        public CaptchaMode CaptchaMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user is allowed to remember login.
        /// </summary>
        public bool AllowRememberMe { get; set; }

        /// <summary>
        /// Gets or sets the username for automatic login.
        /// </summary>
        public string AutoLoginAs { get; set; }



        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            CaptchaMode = xmlNode.GetChildAsEnum("CaptchaMode", CaptchaMode);
            AllowRememberMe = xmlNode.GetChildAsBool("AllowRememberMe", AllowRememberMe);
            AutoLoginAs = xmlNode.GetChildAsString("AutoLoginAs", AutoLoginAs);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("CaptchaMode", CaptchaMode);
            xmlElem.AppendElem("AllowRememberMe", AllowRememberMe);
            xmlElem.AppendElem("AutoLoginAs", AutoLoginAs);
        }
    }
}
