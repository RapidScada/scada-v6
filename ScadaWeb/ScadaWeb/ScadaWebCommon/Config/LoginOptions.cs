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
 * Module   : ScadaWebCommon
 * Summary  : Represents login options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

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
            RequireCaptcha = false;
            AllowRememberMe = false;
            RememberMeExpires = 30;
            AutoLoginUsername = "";
            AutoLoginPassword = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether to require a captcha at login.
        /// </summary>
        public bool RequireCaptcha { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user is allowed to remember login.
        /// </summary>
        public bool AllowRememberMe { get; set; }

        /// <summary>
        /// Gets or sets the period in which a user's login expires, in days.
        /// </summary>
        public int RememberMeExpires { get; set; }

        /// <summary>
        /// Gets or sets the username for automatic login.
        /// </summary>
        public string AutoLoginUsername { get; set; }

        /// <summary>
        /// Gets or sets the password for automatic login.
        /// </summary>
        public string AutoLoginPassword { get; set; }



        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            RequireCaptcha = xmlNode.GetChildAsBool("RequireCaptcha", RequireCaptcha);
            AllowRememberMe = xmlNode.GetChildAsBool("AllowRememberMe", AllowRememberMe);
            RememberMeExpires = xmlNode.GetChildAsInt("RememberMeExpires", RememberMeExpires);
            AutoLoginUsername = xmlNode.GetChildAsString("AutoLoginUsername");
            AutoLoginPassword = ScadaUtils.Decrypt(xmlNode.GetChildAsString("AutoLoginPassword"));
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("RequireCaptcha", RequireCaptcha);
            xmlElem.AppendElem("AllowRememberMe", AllowRememberMe);
            xmlElem.AppendElem("RememberMeExpires", RememberMeExpires);
            xmlElem.AppendElem("AutoLoginUsername", AutoLoginUsername);
            xmlElem.AppendElem("AutoLoginPassword", ScadaUtils.Encrypt(AutoLoginPassword));
        }
    }
}
